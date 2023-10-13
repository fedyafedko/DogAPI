using AutoMapper;
using BLL.Services.Interfaces;
using Common.DTO.DogDTO;
using Common.ExtensionMethods;
using DAL.Repositories.Interfaces;
using Entities;

namespace BLL.Services;

public class DogService : IDogService
{
    private readonly IDogRepository _repository;
    private readonly IMapper _mapper;

    public DogService(IDogRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<DogDTO> AddDog(CreateDogDTO dog)
    {
        Dog entity = _mapper.Map<Dog>(dog);
        if (await _repository.Table.FindAsync(entity.Name) != null)
            throw new InvalidOperationException("Entity with such key already exists in database");

        await _repository.AddAsync(entity);
        return _mapper.Map<DogDTO>(entity);
    }

    public List<DogDTO> GetDogs()
    {
        return _mapper.Map<IEnumerable<DogDTO>>(_repository.GetAll()).ToList();
    }

    public async Task<DogDTO?> GetDogByName(string name)
    {
        Dog? dog = await _repository.FindAsync(name);
        return dog != null ? _mapper.Map<DogDTO>(dog) : null;

    }

    public async Task<bool> DeleteDog(string name)
    {
        Dog? dog = await _repository.FindAsync(name);
        return dog != null && await _repository.DeleteAsync(dog) > 0;
    }

    public async Task<DogDTO> UpdateDog(string name, UpdateDogDTO dog)
    {
        Dog? entity = await _repository.FindAsync(name);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Unable to find entity with such key {name}");
        }

        _mapper.Map(dog, entity);

        await _repository.UpdateAsync(entity);

        return _mapper.Map<DogDTO>(entity);
    }

    public List<DogDTO> GetDogs(string attribute, string? order)
    {
        var dogs = _repository.Table.AsQueryable();
        
        var source = dogs.OrderByAttribute(attribute, order!).ToList();
        
        var dogDTO = _mapper.Map<IEnumerable<DogDTO>>(source).ToList();
        
        return dogDTO;
    }

    public List<DogDTO> GetDogs(string attribute, string? order, int pageNumber, int pageSize)
    {
        var dogs = _repository.Table.AsQueryable();
        
        var source = dogs.OrderByAttribute(attribute, order!).ToList();
        
        int totalCount = source.Count;
        
        if (pageSize > totalCount || pageNumber < 1 || pageSize < 1)
            return _mapper.Map<IEnumerable<DogDTO>>(source).ToList();
        
        source = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return _mapper.Map<IEnumerable<DogDTO>>(source).ToList();
    }
}