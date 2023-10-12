﻿using Common.DTO.DogDTO;

namespace BLL.Services.Interfaces;

public interface IDogService
{
    Task<DogDTO> AddDog(AddDogDTO dog);
    List<DogDTO> GetDogs();
    Task<DogDTO?> GetDogByName(string name);
    Task<bool> DeleteDog(string name);
    Task<DogDTO> UpdateDog(string name, UpdateDogDTO dog);
    List<DogDTO> GetDogs(string attribute, string? order);
    List<DogDTO> GetDogs(string attribute, string? order, int pageNumber, int pageSize);
}