using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;

namespace CadeirasDentistas.Tests
{
    public class AlocacaoControllerTests
    {
        private readonly Mock<ICadeiraService> _mockService;
        private readonly CadeiraController _controller;

        public CadeiraControllerTests()
        {
            _mockService = new Mock<ICadeiraService>();
            _controller = new CadeiraController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllAlocacoes_ShouldReturnAllAlocacoes()
        {
            // Arrange
            var alocacoes = new List<Alocacao>
            {
                new Alocacao
                {
                    Id = 1,
                    Cadeira = new Cadeira { Id = 1, Numero = "001", Descricao = "Cadeira 1" },
                    DataHoraInicio = DateTime.Parse("2024-12-20T10:00:00"),
                    DataHoraFim = DateTime.Parse("2024-12-20T12:00:00")
                },
                new Alocacao
                {
                    Id = 2,
                    Cadeira = new Cadeira { Id = 2, Numero = "002", Descricao = "Cadeira 2" },
                    DataHoraInicio = DateTime.Parse("2024-12-20T12:00:00"),
                    DataHoraFim = DateTime.Parse("2024-12-20T14:00:00")
                }
            };

            _mockService
                .Setup(service => service.GetAllAlocacoesAsync())
                .ReturnsAsync(alocacoes);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(alocacoes);
        } 

        [Fact]
        public async Task GetAllAlocacoes_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _mockService
                .Setup(service => service.GetAllAlocacoesAsync())
                .ThrowsAsync(new Exception("Erro inesperado"));

            // Act
            var result = await _controller.GetAll();

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(500);
            objectResult.Value.Should().Be("Erro interno no servidor.");
        }


        [Fact]
        public async Task AlocarAutomaticamente_ShouldReturnAlocacoesFeitas()
        {
            // Arrange
            var inicio = DateTime.Parse("2024-12-20T10:00:00");
            var fim = DateTime.Parse("2024-12-20T12:00:00");

            var alocacoesFeitas = new List<Alocacao>
            {
                new Alocacao
                {
                    Id = 1,
                    Cadeira = new Cadeira { Id = 1, Numero = "001", Descricao = "Cadeira 1" },
                    DataHoraInicio = inicio,
                    DataHoraFim = fim
                }
            };

            _mockService
                .Setup(service => service.AlocarAutoAsync(inicio, fim))
                .ReturnsAsync(alocacoesFeitas);

            // Act
            var result = await _controller.AlocarAutomaticamente(inicio, fim);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(alocacoesFeitas);
        }

        [Fact]
        public async Task AlocarAutomaticamente_ShouldReturnConflict_WhenNoCadeirasAvailable()
        {
            // Arrange
            var inicio = DateTime.Parse("2024-12-20T10:00:00");
            var fim = DateTime.Parse("2024-12-20T12:00:00");

            _mockService
                .Setup(service => service.AlocarAutoAsync(inicio, fim))
                .ThrowsAsync(new InvalidOperationException("Nenhuma cadeira disponível para o período especificado."));

            // Act
            var result = await _controller.AlocarAutomaticamente(inicio, fim);

            // Assert
            var conflictResult = result as ConflictObjectResult;
            conflictResult.Should().NotBeNull();
            conflictResult.StatusCode.Should().Be(409);
            conflictResult.Value.Should().Be("Nenhuma cadeira disponível para o período especificado.");
        }

        [Fact]
        public async Task AlocarAutomaticamente_ShouldReturnBadRequest_WhenDatesAreInvalid()
        {
            // Arrange
            var inicio = DateTime.Parse("2024-12-20T14:00:00");
            var fim = DateTime.Parse("2024-12-20T12:00:00"); // Data final é anterior à inicial.

            // Act
            var result = await _controller.AlocarAutomaticamente(inicio, fim);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("As datas fornecidas são inválidas.");
        }

  
    }
}
