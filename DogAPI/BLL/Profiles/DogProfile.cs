using AutoMapper;
using Common.DTO.DogDTO;
using Entities;

namespace BLL.Profiles;

public class DogProfile : Profile
{
    public DogProfile()
    {
        CreateMap<Dog, DogDTO>();
        CreateMap<CreateDogDTO, Dog>();
        CreateMap<UpdateDogDTO, Dog>();
    }
}