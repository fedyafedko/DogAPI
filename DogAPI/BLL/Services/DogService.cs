using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using BLL.Services.Interfaces;
using Common;
using Common.DTO.DogDTO;
using Common.Enums;
using Common.Extensions;
using DAL.Repositories.Interfaces;
using Entities;
using Entities.Attributes;

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
        var entity = _mapper.Map<Dog>(dog);
        if (await _repository.FindAsync(entity.Name) != null)
            throw new InvalidOperationException("Entity with such key already exists in database");

        await _repository.AddAsync(entity);
        return _mapper.Map<DogDTO>(entity);
    }

    public async Task<DogDTO?> GetDogByName(string name)
    {
        var dog = await _repository.FindAsync(name);
        return dog != null ? _mapper.Map<DogDTO>(dog) : null;
    }

    public async Task<bool> DeleteDog(string name)
    {
        var dog = await _repository.FindAsync(name);
        return dog != null && await _repository.DeleteAsync(dog) > 0;
    }

    public async Task<DogDTO> UpdateDog(string name, UpdateDogDTO dog)
    {
        var entity = await _repository.FindAsync(name);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Unable to find entity with such key {name}");
        }

        _mapper.Map(dog, entity);

        await _repository.UpdateAsync(entity);

        return _mapper.Map<DogDTO>(entity);
    }

    public List<DogDTO> GetDogs(GetDogsRequest request)
    {
        var dogs = _repository.Table.AsQueryable();

        request.Attribute ??= typeof(Dog).GetProperties()
            .FirstOrDefault(prop => prop.GetCustomAttribute<BaseSortAttribute>() != null)?.Name;

        var source = dogs.OrderByAttribute(
            request.Attribute?.ToEnum<DogOrderingProperty>() ??
            throw new InvalidOperationException("Unable to find attribute"),
            request.Order);

        var totalCount = source.Count();

        if (request.Pagination != null)
        {
            if (request.Pagination.PageNumber < 1
                || request.Pagination.PageSize < 1)
            {
                return new List<DogDTO>();
            }

            source = source
                .Skip((request.Pagination.PageNumber - 1) * request.Pagination.PageSize)
                .Take(request.Pagination.PageSize);
        }

        // Unable to mock Table, that's why using ToListAsync
        // Would be pleased to get help here <3
        return _mapper.Map<List<DogDTO>>(source.ToList());
    }
}