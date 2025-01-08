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
        public async Task GetAllAlocacoesAsync_ShouldReturnAllAlocacoes()
        {
            // Arrange
            const string query = @"
                SELECT * FROM Alocacao
                WHERE (DataHoraInicio < @Fim AND DataHoraFim > @Inicio)";

            var mockAlocacoes = new List<Alocacao>
            {
                new Alocacao { Id = 1, DataHoraInicio = DateTime.Now, DataHoraFim = DateTime.Now.AddHours(1) }
            };

            _mockConnection
                .Setup(conn => conn.QueryAsync<Alocacao>(query, It.IsAny<object>(), null, null, null))
                .ReturnsAsync(mockAlocacoes);

            // Act
            var result = await _repository.GetAllAlocacoesAsync();

            // Assert
            result.Should().BeEquivalentTo(mockAlocacoes);
        }

        [Fact]
        public async Task AddAlocacaoAsync_ShouldInsertAlocacaoIntoDatabase()
        {
            // Arrange
            const string query = @"
                INSERT INTO Alocacao (IdCadeira, DataHoraInicio, DataHoraFim) 
                VALUES (@IdCadeira, @DataHoraInicio, @DataHoraFim)";

            var alocacao = new Alocacao
            {
                Id = 1,
                Cadeira = new Cadeira { Id = 1 },
                DataHoraInicio = DateTime.Now,
                DataHoraFim = DateTime.Now.AddHours(1)
            };

            _mockConnection
                .Setup(conn => conn.ExecuteAsync(query, It.IsAny<object>(), null, null))
                .ReturnsAsync(1); // 1 linha afetada

            // Act
            var result = await _repository.AddAlocacaoAsync(alocacao);

            // Assert
            result.Should().BeEquivalentTo(alocacao);
        }


        [Fact]
        public async Task AddAlocacaoAsync_ShouldThrowException_WhenInsertFails()
        {
            // Arrange
            var alocacao = new Alocacao
            {
                Id = 1,
                Cadeira = new Cadeira { Id = 1 },
                DataHoraInicio = DateTime.Now,
                DataHoraFim = DateTime.Now.AddHours(1)
            };

            _mockConnection
                .Setup(conn => conn.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null))
                .ThrowsAsync(new Exception("Erro ao inserir no banco de dados"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _repository.AddAlocacaoAsync(alocacao));
            Assert.Equal("Erro ao inserir no banco de dados", exception.Message);
        }



    }
}
