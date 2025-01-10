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
        private readonly Mock<ICadeiraRepository> _mockRepository;

        public CadeiraServiceTests()
        {
            _mockRepository = new Mock<ICadeiraRepository>();
        }

        [Fact]
        public async Task GetAllCadeirasAsync_ShouldReturnAllCadeiras()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetAllCadeirasAsync())
                .ReturnsAsync(new List<Cadeira>
                {
                    new Cadeira { Id = 1, Numero = "001", Descricao = "Cadeira 1", TotalAlocacoes = 3 },
                    new Cadeira { Id = 2, Numero = "002", Descricao = "Cadeira 2", TotalAlocacoes = 5 }
                });

            // Act
            var result = await _service.GetAllCadeirasAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }


        [Fact]
        public async Task AddCadeiraAsync_ShouldAddCadeira()
        {
            // Arrange
            var cadeiraDto = new CadeiraDTO
            {
                Numero = "003",
                Descricao = "Cadeira Nova",
                TotalAlocacoes = 0
            };

            var expectedCadeira = new Cadeira
            {
                Id = 3,
                Numero = cadeiraDto.Numero,
                Descricao = cadeiraDto.Descricao,
                TotalAlocacoes = cadeiraDto.TotalAlocacoes
            };

            _mockRepository
                .Setup(repo => repo.AddCadeiraAsync(It.IsAny<Cadeira>()))
                .ReturnsAsync(expectedCadeira);

            // Act
            var result = await _service.AddCadeiraAsync(cadeiraDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCadeira.Numero, result.Numero);
            Assert.Equal(expectedCadeira.Descricao, result.Descricao);
        }

        [Fact]
        public async Task UpdateCadeiraAsync_ShouldUpdateCadeira()
        {
            // Arrange
            var cadeiraDto = new CadeiraDTO
            {
                Numero = "003",
                Descricao = "Cadeira Atualizada",
                TotalAlocacoes = 2
            };

            var updatedCadeira = new Cadeira
            {
                Id = 3,
                Numero = cadeiraDto.Numero,
                Descricao = cadeiraDto.Descricao,
                TotalAlocacoes = cadeiraDto.TotalAlocacoes
            };

            _mockRepository
                .Setup(repo => repo.UpdateCadeiraAsync(It.IsAny<Cadeira>()))
                .ReturnsAsync(updatedCadeira);

            // Act
            var result = await _service.UpdateCadeiraAsync(cadeiraDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedCadeira.Descricao, result.Descricao);
            Assert.Equal(updatedCadeira.TotalAlocacoes, result.TotalAlocacoes);
        }

        [Fact]
        public async Task DeleteCadeiraAsync_ShouldDeleteCadeira()
        {
            // Arrange
            var cadeiraId = 1;

            _mockRepository
                .Setup(repo => repo.DeleteCadeiraAsync(cadeiraId))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteCadeiraAsync(cadeiraId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteCadeiraAsync(cadeiraId), Times.Once);
        }






    }
}
