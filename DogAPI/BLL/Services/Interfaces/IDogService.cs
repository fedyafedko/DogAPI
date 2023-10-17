using Common;
using Common.DTO.DogDTO;

namespace BLL.Services.Interfaces;

public interface IDogService
{
    Task<DogDTO> AddDog(CreateDogDTO dog);
    Task<DogDTO?> GetDogByName(string name);
    Task<bool> DeleteDog(string name);
    Task<DogDTO> UpdateDog(string name, UpdateDogDTO dog);
    List<DogDTO> GetDogs(GetDogsRequest request);
}