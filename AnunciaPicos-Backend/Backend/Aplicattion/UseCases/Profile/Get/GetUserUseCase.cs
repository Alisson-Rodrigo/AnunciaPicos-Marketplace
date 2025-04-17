using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Shared.Communication.Response.Profile;
using AutoMapper;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Get
{
    public class GetUserUseCase : IGetUserUseCase
    {
        private readonly ILogged _logged;
        private readonly IMapper _mapper;

        public GetUserUseCase(ILogged logged, IMapper mapper)
        {
            _logged = logged;
            _mapper = mapper;
        }

        public async Task<ResponseGetProfileCommunication> Execute ()
        {
            var user = await _logged.UserLogged();
            return _mapper.Map<ResponseGetProfileCommunication>(user);
        }
    }
}
