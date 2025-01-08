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
        private readonly Mock<IAlocacaoRepository> _mockRepository;
        private readonly AlocacaoService _service;

        public AlocacaoServiceTests()
        {
            _mockRepository = new Mock<IAlocacaoRepository>();
            _service = new AlocacaoService(_mockRepository.Object);
        }



        [Fact]
        public async Task GetAllAlocacoesAsync_ShouldReturnAllAlocacoes()
        {
            // Arrange
            var mockAlocacoes = new List<Alocacao>
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

            _mockRepository
                .Setup(repo => repo.GetAllAlocacoesAsync())
                .ReturnsAsync(mockAlocacoes);

            // Act
            var result = await _service.GetAllAlocacoesAsync();

            // Assert
            result.Should().BeEquivalentTo(mockAlocacoes);
            _mockRepository.Verify(repo => repo.GetAllAlocacoesAsync(), Times.Once);
        }

        [Fact]
        public async Task AlocarAutoAsync_ShouldAllocateAutomatically()
        {
            // Arrange
            var inicio = DateTime.Parse("2024-12-20T10:00:00");
            var fim = DateTime.Parse("2024-12-20T12:00:00");

            var mockCadeiras = new List<Cadeira>
            {
                new Cadeira { Id = 1, Numero = "001", Descricao = "Cadeira 1" },
                new Cadeira { Id = 2, Numero = "002", Descricao = "Cadeira 2" }
            };

            var mockAlocacoesExistentes = new List<Alocacao>(); // Sem alocações existentes.

            _mockCadeiraRepository
                .Setup(repo => repo.GetAllCadeirasAsync())
                .ReturnsAsync(mockCadeiras);

            _mockRepository
                .Setup(repo => repo.GetAlocacoesPorPeriodoAsync(inicio, fim))
                .ReturnsAsync(mockAlocacoesExistentes);

            _mockRepository
                .Setup(repo => repo.AddAlocacaoAsync(It.IsAny<Alocacao>()))
                .ReturnsAsync((Alocacao alocacao) => alocacao);

            // Act
            var result = await _service.AlocarAutoAsync(inicio, fim);

            // Assert
            result.Should().HaveCount(1); // Uma alocação realizada
            result.First().Cadeira.Id.Should().Be(1);
            _mockRepository.Verify(repo => repo.AddAlocacaoAsync(It.IsAny<Alocacao>()), Times.Once);
        }


        [Fact]
        public async Task AlocarAutoAsync_ShouldThrowException_WhenNoCadeirasAvailable()
        {
            // Arrange
            var inicio = DateTime.Parse("2024-12-20T10:00:00");
            var fim = DateTime.Parse("2024-12-20T12:00:00");

            var mockCadeiras = new List<Cadeira>
            {
                new Cadeira { Id = 1, Numero = "001", Descricao = "Cadeira 1" }
            };

            var mockAlocacoesExistentes = new List<Alocacao>
            {
                new Alocacao
                {
                    Id = 1,
                    Cadeira = mockCadeiras.First(),
                    DataHoraInicio = inicio,
                    DataHoraFim = fim
                }
            };

            _mockCadeiraRepository
                .Setup(repo => repo.GetAllCadeirasAsync())
                .ReturnsAsync(mockCadeiras);

            _mockRepository
                .Setup(repo => repo.GetAlocacoesPorPeriodoAsync(inicio, fim))
                .ReturnsAsync(mockAlocacoesExistentes);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AlocarAutoAsync(inicio, fim));
        }


    }
}
