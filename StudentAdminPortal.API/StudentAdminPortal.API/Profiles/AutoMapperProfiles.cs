using AutoMapper;
using StudentAdminPortal.API.Models;

namespace StudentAdminPortal.API.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Student, StudentDto>().ReverseMap();

            CreateMap<Gender, GenderDto>().ReverseMap();

            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
