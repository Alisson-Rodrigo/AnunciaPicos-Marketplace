using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Product;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Request.Product;
using AnunciaPicos.Shared.Exceptions;
using AutoMapper;
using SixLabors.ImageSharp.Formats.Webp;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Product.Register
{
    public class RegisterProductUseCase : IRegisterProductUseCase
    {
        private readonly ILogged _logged;
        private readonly IUnitOfWork _unitOfWord;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public RegisterProductUseCase(ILogged logged, IUnitOfWork unitOfWord, IMapper mapper, IProductRepository productRepository)
        {
            _logged = logged;
            _productRepository = productRepository;
            _unitOfWord = unitOfWord;
            _mapper = mapper;
        }

        public async Task Execute(RequestRegisterProductCommunication request)
        {
            // Valida requisição
            ValidateProduct(request);

            // Crie a lista de URLs de imagens
            var imagensUrls = new List<string>();

            // Defina caminho absoluto da pasta externa
            var caminhoBase = "/var/www/anunciapicos/uploads/products/images";

            Directory.CreateDirectory(caminhoBase); // garante que a pasta exista

            if (request.Imagens != null && request.Imagens.Count > 0)
            {
                foreach (var imagem in request.Imagens)
                {
                    var nameFileNotExtension = Path.GetFileNameWithoutExtension(imagem.FileName);
                    var nameFile = $"{Guid.NewGuid()}_{nameFileNotExtension}.webp";
                    var caminhoWebP = Path.Combine(caminhoBase, nameFile);

                    using (var inputStream = imagem.OpenReadStream())
                    using (var image = await Image.LoadAsync(inputStream))
                    {
                        var encoder = new WebpEncoder
                        {
                            Quality = 90
                        };

                        await image.SaveAsync(caminhoWebP, encoder);
                    }

                    var url = $"https://api.anunciapicos.shop/uploads/products/images/{nameFile}";
                    imagensUrls.Add(url);
                }
            }

            // Mapeamento de request para entidade
            var product = _mapper.Map<ProductModel>(request);

            product.ImageUrl = imagensUrls;

            // Pegar usuário logado e adicionar a entidade
            UserModel user_logged = await _logged.UserLogged();

            // Verifica se o usuário está logado e ativo
            if (user_logged == null)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_NOT_FOUND);
            }

            if (user_logged.Active == false)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_INACTIVE);
            }

            // Associar o usuário ao produto (sem necessidade de atribuir `UserId` manualmente)
            product.User = user_logged;

            // Salvar entidade
            await _productRepository.Register(product);

            // Salvar no banco
            await _unitOfWord.Commit();
        }

        private void ValidateProduct(RequestRegisterProductCommunication request)
        {
            var validator = new RegisterProductValidation();
            var response = validator.Validate(request);

            // Se a validação não for válida, lança exceção
            if (!response.IsValid)
            {
                var errorMessages = response.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }



    }
}
