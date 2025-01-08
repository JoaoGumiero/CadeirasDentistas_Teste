using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;

namespace CadeirasDentistas.Tests
{
    public class CadeiraControllerTests
    {
        private readonly Mock<ICadeiraService> _mockService;
        private readonly CadeiraController _controller;

        public CadeiraControllerTests()
        {
            _mockService = new Mock<ICadeiraService>();
            _controller = new CadeiraController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfCadeiras()
        {
            // Arrange
            var cadeiras = new List<Cadeira>
            {
                new Cadeira { Id = 1, Numero = "001", Descricao = "Cadeira 1" },
                new Cadeira { Id = 2, Numero = "002", Descricao = "Cadeira 2" }
            };

            _mockService
                .Setup(service => service.GetAllCadeirasAsync())
                .ReturnsAsync(cadeiras);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnedCadeiras = okResult.Value as IEnumerable<Cadeira>;
            returnedCadeiras.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WithCadeira()
        {
            // Arrange
            var cadeira = new Cadeira { Id = 1, Numero = "001", Descricao = "Cadeira 1" };

            _mockService
                .Setup(service => service.GetCadeiraByIdAsync(1))
                .ReturnsAsync(cadeira);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnedCadeira = okResult.Value as Cadeira;
            returnedCadeira.Should().NotBeNull();
            returnedCadeira.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenCadeiraDoesNotExist()
        {
            // Arrange
            _mockService
                .Setup(service => service.GetCadeiraByIdAsync(99))
                .ReturnsAsync((Cadeira)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Add_ShouldReturnCreatedAtAction_WithCreatedCadeira()
        {
            // Arrange
            var cadeira = new Cadeira { Numero = "003", Descricao = "Nova Cadeira" };
            var createdCadeira = new Cadeira { Id = 3, Numero = "003", Descricao = "Nova Cadeira" };

            _mockService
                .Setup(service => service.AddCadeiraAsync(It.IsAny<Cadeira>()))
                .ReturnsAsync(createdCadeira);

            // Act
            var result = await _controller.Add(cadeiraDTO);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.StatusCode.Should().Be(201);
            createdResult.RouteValues["id"].Should().Be(3);

            var returnedCadeira = createdResult.Value as Cadeira;
            returnedCadeira.Should().NotBeNull();
            returnedCadeira.Id.Should().Be(3);
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenAddFails()
        {
            // Arrange
            var cadeira = new Cadeira { Numero = "001", Descricao = "Cadeira Inválida" };

            _mockService
                .Setup(service => service.AddCadeiraAsync(It.IsAny<Cadeira>()))
                .ThrowsAsync(new Exception("Erro ao adicionar cadeira"));

            // Act
            var result = await _controller.Add(cadeiraDTO);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("Erro ao adicionar cadeira");
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updatedCadeira = new Cadeira { Id = 1, Numero = "001", Descricao = "Cadeira Atualizada" };

            _mockService
                .Setup(service => service.UpdateCadeiraAsync(updatedCadeira))
                .ReturnsAsync(updatedCadeira);

            // Act
            var result = await _controller.Update(1, updatedCadeira);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnedCadeira = okResult.Value as Cadeira;
            returnedCadeira.Should().NotBeNull();
            returnedCadeira.Descricao.Should().Be("Cadeira Atualizada");
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenCadeiraDoesNotExist()
        {
            // Arrange
            var updatedCadeira = new Cadeira { Id = 99, Numero = "999", Descricao = "Cadeira Inexistente" };

            _mockService
                .Setup(service => service.UpdateCadeiraAsync(updatedCadeira))
                .ThrowsAsync(new KeyNotFoundException("Cadeira não encontrada"));

            // Act
            var result = await _controller.Update(99, updatedCadeira);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be("Cadeira não encontrada");
        }


        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var cadeiraId = 1;

            _mockService
                .Setup(service => service.DeleteCadeiraAsync(cadeiraId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(cadeiraId);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Delete_ShouldReturnConflict_WhenCadeiraHasDependencies()
        {
            // Arrange
            var cadeiraId = 1;

            _mockService
                .Setup(service => service.DeleteCadeiraAsync(cadeiraId))
                .ThrowsAsync(new InvalidOperationException("A cadeira possui alocações associadas"));

            // Act
            var result = await _controller.Delete(cadeiraId);

            // Assert
            var conflictResult = result as ConflictObjectResult;
            conflictResult.Should().NotBeNull();
            conflictResult.StatusCode.Should().Be(409);
            conflictResult.Value.Should().Be("A cadeira possui alocações associadas");
        }

    }
}
