namespace WebApiCv
{
    using AutoMapper;
    using Dtos;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MappingModel : Profile
    {
        #region Construnctores
        public MappingModel()
        {
            CreateMap<Users, UserDto>()
                .ForMember(s => s.Username, option => option.MapFrom(o => o.UserName));
            CreateMap<UserDto, Users>()
                .ForMember(s => s.UserName, option => option.MapFrom(o => o.Username));
            CreateMap<UserDto, CreateUserDto>();
            CreateMap<CreateUserDto, UserDto>();
            CreateMap<Users, CreateUserDto>()
                .ForMember(s => s.Username, option => option.MapFrom(o => o.UserName));
            CreateMap<CreateUserDto, Users>()
                .ForMember(s => s.UserName, option => option.MapFrom(o => o.Username));
            CreateMap<RecoverPassDto, ChangePasswordDto>();
            CreateMap<ChangePasswordDto, RecoverPassDto>();
            CreateMap<UserTokenDto, Users>()
                .ForMember(s => s.TokenLogin, option => option.MapFrom(o => o.Token))
                .ForMember(s => s.UserName, option => option.MapFrom(o => o.Username));
            CreateMap<Users, UserTokenDto>()
                .ForMember(s => s.Token, option => option.MapFrom(o => o.TokenLogin))
                .ForMember(s => s.Username, option => option.MapFrom(o => o.UserName));
        }
        #endregion
    }
}
