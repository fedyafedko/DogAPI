using AutoMapper;
using BLL.Services;
using BLL.Services.Interfaces;
using Common;
using Common.DTO.DogDTO;
using Common.Enums;
using DAL.Repositories.Interfaces;
using Entities;
using Xunit;
using NSubstitute;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace UnitTest;

public class DogServiceTest
{
    private readonly IDogService _service;
    private readonly IDogRepository _repository;
    private readonly IMapper _mapper;
        
    public DogServiceTest()
    {
        _repository = Substitute.For<IDogRepository>();
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateDogDTO, Dog>();
            cfg.CreateMap<UpdateDogDTO, Dog>();
            cfg.CreateMap<Dog, DogDTO>();
        }));

        _service = new DogService(_repository, _mapper);
    }
        
    [Fact]
    public async Task AddDog_Should_Return_DogDTO()
    {
        // Arrange
        var createDogDto = new CreateDogDTO
        {
            Name = "Fido",
            Color = "Brown",
            TailLength = 10,
            Weight = 50
        };

        var dogEntity = new Dog
        {
            Name = "Fido",
            Color = "Brown",
            TailLength = 10,
            Weight = 50
        };

        _repository.FindAsync(dogEntity.Name).Returns(default(Dog));
        _repository.AddAsync(dogEntity).Returns(1);
            
        // Act
        var result = await _service.AddDog(createDogDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().BeEquivalentTo(createDogDto.Name);
    }

    [Fact]
    public async Task AddDog_Should_Throw_Exception_If_Entity_Exists()
    {
        // Arrange
        var createDogDto = new CreateDogDTO
        {
            Name = "Fido",
            Color = "Brown",
            TailLength = 10,
            Weight = 50
        };

        var dogEntity = new Dog
        {
            Name = "Fido",
            Color = "Brown",
            TailLength = 10,
            Weight = 50
        };

        _repository.FindAsync(createDogDto.Name).Returns(dogEntity);

        // Act
        var act = () => _service.AddDog(createDogDto);
            
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Entity with such key already exists in database");
    }

    [Fact]
    public async Task GetDogByName_Should_Return_Null_If_Not_Found()
    {
        // Arrange
        _repository.FindAsync("Fido").Returns((Dog)null!);

        // Act
        var result = await _service.GetDogByName("Fido");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetDogByName_Should_Return_DogDTO_If_Found()
    {
        // Arrange
        var dogEntity = new Dog
        {
            Name = "Fido",
            Color = "Brown",
            TailLength = 10,
            Weight = 50
        };

        _repository.FindAsync("Fido").Returns(dogEntity);

        // Act
        var result = await _service.GetDogByName("Fido");

        // Assert
        result!.Name.Should().BeEquivalentTo("Fido");
    }

    [Fact]
    public async Task DeleteDog_Should_Return_True_If_Dog_Exists_And_Deleted()
    {
        // Arrange
        var dogEntity = new Dog
        {
            Name = "Fido",
            Color = "Brown",
            TailLength = 10,
            Weight = 50
        };

        _repository.FindAsync(dogEntity.Name).Returns(dogEntity);
        _repository.DeleteAsync(dogEntity).Returns(1);

        // Act
        var result = await _service.DeleteDog("Fido");

        // Assert
        Assert.True(result);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteDog_Should_Return_False_If_Dog_Not_Found()
    {
        // Arrange
        _repository.FindAsync("Fido").Returns((Dog)null!);

        // Act
        var result = await _service.DeleteDog("Fido");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateDog_Should_Update_Dog_And_Return_DogDTO()
    {
        // Arrange
        var updateDogDto = new UpdateDogDTO
        {
            Color = "Black",
            TailLength = 12,
            Weight = 60
        };
        
        var dogEntity = new Dog
        {
            Name = "Fido",
            Color = "Brown",
            TailLength = 10,
            Weight = 50
        };
        
        var updatedDogEntity = new Dog
        {
            Name = "Fido",
            Color = "Black",
            TailLength = 12,
            Weight = 60
        };

        _repository.FindAsync("Fido").Returns(dogEntity);
        _repository.UpdateAsync(updatedDogEntity).Returns(1);
        
        // Act
        var result = await _service.UpdateDog("Fido", updateDogDto);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedDogEntity);
    }

    [Fact]
    public async Task UpdateDog_Should_Throw_Exception_If_Dog_Not_Found()
    {
        // Arrange
        var updateDogDto = new UpdateDogDTO
        {
            Color = "Black",
            TailLength = 12,
            Weight = 60
        };

        _repository.FindAsync("Fido").Returns((Dog)null!);

        // Act
        var act  = () => _service.UpdateDog("Fido", updateDogDto);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Unable to find entity with such key Fido");
    }

    [Fact]
    public async Task GetDogs_Should_Return_All_Dogs_When_No_Pagination_Or_Sorting()
    {
        // Arrange
        var table = Substitute.For<DbSet<Dog>, IQueryable<Dog>>();
            
        var dogsList = new List<Dog>
        {
            new Dog { Name = "Fido", Color = "Brown", TailLength = 10, Weight = 50 },
            new Dog { Name = "Rex", Color = "Black", TailLength = 12, Weight = 60 },
        };
            
        var queryable = dogsList.AsQueryable();
        ((IQueryable<Dog>)table).Provider.Returns(queryable.Provider);
        ((IQueryable<Dog>)table).Expression.Returns(queryable.Expression);
        ((IQueryable<Dog>)table).ElementType.Returns(queryable.ElementType);
        ((IQueryable<Dog>)table).GetEnumerator().Returns(queryable.GetEnumerator());

        var request = new GetDogsRequest();
            
            
        _repository.Table.AsQueryable().Returns(dogsList.AsQueryable());
            
        // Act
        var result = _service.GetDogs(request);
            
        // Assert
        result.Should().NotBeNull();
        result[0].Name.Should().BeEquivalentTo(dogsList[0].Name);
        result[1].Name.Should().BeEquivalentTo(dogsList[1].Name);
    }

    [Fact]
    public void GetDogs_Should_Paginate_And_Sort_If_Requested()
    {
        var dogsList = new List<Dog>
        {
            new Dog { Name = "Fido", Color = "Brown", TailLength = 10, Weight = 50 },
            new Dog { Name = "Rex", Color = "Black", TailLength = 12, Weight = 60 },
            new Dog { Name = "Max", Color = "White", TailLength = 8, Weight = 45 },
        };

        var request = new GetDogsRequest
        {   
            Attribute = "weight",
            Order = Order.Descending,
            Pagination = new PaginationModel(2, 1)
        };
            
            
        _repository.Table.AsQueryable().Returns(dogsList.AsQueryable());
            
        // Act
        var result = _service.GetDogs(request);
            
        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result[0].Name.Should().BeEquivalentTo("Rex");
        result[1].Name.Should().BeEquivalentTo("Fido");
    }
}