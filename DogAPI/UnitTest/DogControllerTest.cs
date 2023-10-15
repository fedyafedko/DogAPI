using BLL.Services.Interfaces;
using Common.DTO.DogDTO;
using DogAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
// ToDo: use NSubstitue instead
using Moq;
using Xunit;

namespace UnitTest
{
    public class DogControllerTest
    {
        private readonly Mock<IDogService> _mockDogService;
        private readonly DogController _dogController;

        public DogControllerTest()
        {
            _mockDogService = new Mock<IDogService>();
            _dogController = new DogController(_mockDogService.Object);
        }

        [Fact]
        public async Task GetDogByName_ReturnsOkWithDogDto()
        {
            // Arrange
            string name = "Fido";
            DogDTO dogDto = new DogDTO { Name = name, Color = "Brown", TailLength = 10, Weight = 20 };

            _mockDogService.Setup(service => service.GetDogByName(name)).ReturnsAsync(dogDto);

            // Act
            var result = await _dogController.GetDogByName(name);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDog = Assert.IsType<DogDTO>(okResult.Value);
            // ToDo: Use FluentAssertions instead
            Assert.Equal(dogDto, returnedDog);
        }

        [Fact]
        public async Task InsertDog_ReturnsCreatedAtAction()
        {
            // Arrange
            CreateDogDTO createDogDto = new CreateDogDTO { Name = "Rex", Color = "Black", TailLength = 15, Weight = 25 };
            DogDTO insertedDogDto = new DogDTO { Name = "Rex", Color = "Black", TailLength = 15, Weight = 25 };

            _mockDogService.Setup(service => service.AddDog(createDogDto)).ReturnsAsync(insertedDogDto);

            // Act
            var result = await _dogController.InsertDog(createDogDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(DogController.GetDogByName), createdAtActionResult.ActionName);

            var value = Assert.IsType<DogDTO>(createdAtActionResult.Value);
            Assert.Equal(insertedDogDto, value);
        }

        [Fact]
        public async Task UpdateDog_ValidName_ReturnsOkWithUpdatedDogDto()
        {
            // Arrange
            string name = "Buddy";
            UpdateDogDTO updateDogDto = new UpdateDogDTO { Color = "White", TailLength = 12, Weight = 30 };
            DogDTO updatedDogDto = new DogDTO { Name = name, Color = "White", TailLength = 12, Weight = 30 };

            _mockDogService.Setup(service => service.UpdateDog(name, updateDogDto)).ReturnsAsync(updatedDogDto);

            // Act
            var result = await _dogController.UpdateDog(name, updateDogDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDog = Assert.IsType<DogDTO>(okResult.Value);

            Assert.Equal(updatedDogDto, returnedDog);
        }

        [Fact]
        public async Task DeleteDog_ValidName_ReturnsOk()
        {
            // Arrange
            string name = "Max";
            _mockDogService.Setup(service => service.DeleteDog(name)).ReturnsAsync(true);

            // Act
            var result = await _dogController.DeleteDog(name);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteDog_InvalidName_ReturnsNotFound()
        {
            // Arrange
            string name = "Max";
            _mockDogService.Setup(service => service.DeleteDog(name)).ReturnsAsync(false);

            // Act
            var result = await _dogController.DeleteDog(name);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetAll_NoQueryParams_ReturnsOkWithListOfDogDtos()
        {
            // Arrange
            List<DogDTO> dogDtos = new List<DogDTO>
            {
                new DogDTO { Name = "Fido", Color = "Brown", TailLength = 10, Weight = 20 },
                new DogDTO { Name = "Rex", Color = "Black", TailLength = 15, Weight = 25 }
            };

            _mockDogService.Setup(service => service.GetDogs("name", null)).Returns(dogDtos);

            // Act
            var result = _dogController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDogs = Assert.IsType<List<DogDTO>>(okResult.Value);

            Assert.Equal(dogDtos, returnedDogs);
        }

        [Fact]
        public void GetAll_WithQueryParams_ReturnsOkWithListOfDogDtos()
        {
            // Arrange
            List<DogDTO> dogDtos = new List<DogDTO>
            {
                new DogDTO { Name = "Fido", Color = "Brown", TailLength = 10, Weight = 20 },
                new DogDTO { Name = "Rex", Color = "Black", TailLength = 15, Weight = 25 }
            };

            _mockDogService.Setup(service => service.GetDogs("name", "asc")).Returns(dogDtos);

            // Act
            var result = _dogController.GetAll("name", "asc");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDogs = Assert.IsType<List<DogDTO>>(okResult.Value);

            Assert.Equal(dogDtos, returnedDogs);
        }

        [Fact]
        public void GetAll_Pagination_ReturnsOkWithListOfDogDtos()
        {
            // Arrange
            List<DogDTO> dogDtos = new List<DogDTO>
            {
                new DogDTO { Name = "Fido", Color = "Brown", TailLength = 10, Weight = 20 },
                new DogDTO { Name = "Rex", Color = "Black", TailLength = 15, Weight = 25 }
            };

            _mockDogService.Setup(service => service.GetDogs("name", 1, 10, "asc")).Returns(dogDtos);

            // Act
            var result = _dogController.GetAll("name", "asc", 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDogs = Assert.IsType<List<DogDTO>>(okResult.Value);

            Assert.Equal(dogDtos, returnedDogs);
        }
    }
}
