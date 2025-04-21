using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Response.Profile;
using AnunciaPicos.Shared.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Profile.GetId
{
    public class GetUserIdUseCase : IGetUserIdUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserIdUseCase(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ResponseGetProfileIdCommunication> Execute(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_NOT_FOUND);
            }
            
            ResponseGetProfileIdCommunication response = _mapper.Map<ResponseGetProfileIdCommunication>(user);

            return response;
        }
    }
}
