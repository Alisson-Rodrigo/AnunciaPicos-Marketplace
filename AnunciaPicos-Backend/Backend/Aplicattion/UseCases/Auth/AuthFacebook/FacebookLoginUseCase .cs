using AnunciaPicos.Backend.Aplicattion.Services.TokenLogin;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Shared.Communication.Request.Auth;
using AnunciaPicos.Shared.Communication.Response.Auth;
using AutoMapper;
using System.Text.Json;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Auth.AuthFacebook
{
    public class FacebookLoginUseCase : IFacebookLoginUseCase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserRepository _userRepository; // Assumindo que você tem esse repositório
        private readonly TokenJwt _jwtService; // Seu serviço JWT existente
        private readonly IMapper _mapper; // Se você usar AutoMapper

        public FacebookLoginUseCase(
            IHttpClientFactory httpClientFactory,
            IUserRepository userRepository,
            TokenJwt jwtService,
            IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<ResponseFacebookLoginCommunication> Execute(RequestFacebookLoginCommunication request)
        {
            try
            {
                // 1. Validar token do Facebook
                var facebookUser = await ValidateFacebookToken(request.AccessToken);

                if (facebookUser == null)
                    throw new UnauthorizedAccessException("Token do Facebook inválido");

                // 2. Verificar se usuário já existe
                var existingUser = await _userRepository.GetByProviderIdAsync("Facebook", facebookUser.Id);

                UserModel user;

                if (existingUser != null)
                {
                    // Usuário já existe, atualizar dados se necessário
                    user = existingUser;
                    user.Name = facebookUser.Name ?? user.Name;
                    user.ImageProfile = facebookUser.Picture?.Data?.Url ?? user.ImageProfile;

                    _userRepository.UpdateUser(user);
                }
                else
                {
                    // Verificar se já existe usuário com o mesmo email
                    var userByEmail = await _userRepository.GetByEmailAsync(facebookUser.Email);

                    if (userByEmail != null)
                    {
                        // Usuário existe mas não tem login social, vincular conta
                        userByEmail.Provider = "Facebook";
                        userByEmail.ProviderId = facebookUser.Id;
                        userByEmail.IsSocialLogin = true;
                        userByEmail.ImageProfile = facebookUser.Picture?.Data?.Url ?? userByEmail.ImageProfile;

                        user = userByEmail;
                        _userRepository.UpdateUser(user);
                    }
                    else
                    {
                        // Criar novo usuário
                        user = new UserModel
                        {
                            UserIdentifier = Guid.NewGuid(),
                            Name = facebookUser.Name ?? "",
                            Email = facebookUser.Email ?? "",
                            ImageProfile = facebookUser.Picture?.Data?.Url,
                            Provider = "Facebook",
                            ProviderId = facebookUser.Id,
                            IsSocialLogin = true,
                            Password = "", // Não precisa de senha para login social
                            CPF = "", // Será preenchido depois se necessário
                            Phone = "" // Será preenchido depois se necessário
                        };

                        await _userRepository.RegisterUser(user);
                    }
                }

                // 3. Gerar JWT token
                var token = _jwtService.GenerateToken(user);
                var expiresAt = DateTime.UtcNow.AddHours(1); 

                // 4. Retornar resposta
                return new ResponseFacebookLoginCommunication
                {
                    Token = token,
                    ExpiresAt = expiresAt,
                    User = new UserInfoResponse
                    {
                        UserIdentifier = user.UserIdentifier,
                        Name = user.Name,
                        Email = user.Email,
                        ImageProfile = user.ImageProfile,
                        Provider = user.Provider ?? "Facebook",
                        IsSocialLogin = user.IsSocialLogin
                    }
                };
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na autenticação com Facebook: {ex.Message}");
            }
        }

        private async Task<FacebookUserResponse?> ValidateFacebookToken(string accessToken)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.GetAsync(
                    $"https://graph.facebook.com/me?access_token={accessToken}&fields=id,name,email,picture.type(large)");

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<FacebookUserResponse>(content, options);
            }
            catch
            {
                return null;
            }
        }
    }

    // Modelos para deserializar resposta do Facebook
    public class FacebookUserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public FacebookPicture? Picture { get; set; }
    }

    public class FacebookPicture
    {
        public FacebookPictureData? Data { get; set; }
    }

    public class FacebookPictureData
    {
        public string Url { get; set; } = string.Empty;
    }
}

