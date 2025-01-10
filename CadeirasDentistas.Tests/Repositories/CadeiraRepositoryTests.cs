using System.Collections.Generic;
using System.Data;
using Moq;
using Xunit;
using Dapper;
using CadeirasDentistas.Repository;
using CadeirasDentistas.models;

public class CadeiraRepositoryTests
{
    [Fact]
    public async Task GetAllCadeirasAsync_MustReturnAllCadeiras_WithNoError()
    {
        // Criação
        var mockConnection = new Mock<IDbConnection>();
        var repository = new CadeiraRepository(mockConnection.Object);

        var cadeirasEsperadas = new List<Cadeira>
        {
            new Cadeira { Id = 1, Numero = "001", Descricao = "Cadeira 1", TotalAlocacoes = 2 },
            new Cadeira { Id = 2, Numero = "002", Descricao = "Cadeira 2", TotalAlocacoes = 3 }
        };

        mockConnection
            .Setup(conn => conn.QueryAsync<Cadeira>("SELECT * FROM Cadeira", null, null, null, null))
            .ReturnsAsync(cadeirasEsperadas);

        // Ação
        var cadeiras = await repository.GetAllCadeirasAsync();

        // Assert
        Assert.Equal(2, cadeiras.Count);
        Assert.Equal("001", cadeiras[0].Numero);
    }

    [Fact]
    public async Task GetAllCadeirasAsync_ShouldThrowException_WhenDatabaseFails()
    {
        // Arrange
        _mockConnection
            .Setup(conn => conn.QueryAsync<Cadeira>(It.IsAny<string>(), null, null, null, null))
            .ThrowsAsync(new Exception("Erro ao buscar cadeiras no banco"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.GetAllCadeirasAsync());
        Assert.Equal("Erro ao buscar cadeiras no banco", exception.Message);
    }

    [Fact]
    public async Task AddCadeiraAsync_ShouldThrowException_WhenInsertFails()
    {
        // Arrange
        var cadeira = new Cadeira
        {
            Numero = "001",
            Descricao = "Cadeira Teste",
            TotalAlocacoes = 0
        };

        _mockConnection
            .Setup(conn => conn.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null))
            .ThrowsAsync(new Exception("Erro ao inserir cadeira no banco"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.AddCadeiraAsync(cadeira));
        Assert.Equal("Erro ao inserir cadeira no banco", exception.Message);
    }

    [Fact]
    public async Task UpdateCadeiraAsync_ShouldThrowException_WhenUpdateFails()
    {
        // Arrange
        var cadeira = new Cadeira
        {
            Id = 1,
            Numero = "002",
            Descricao = "Cadeira Atualizada",
            TotalAlocacoes = 5
        };

        _mockConnection
            .Setup(conn => conn.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null))
            .ThrowsAsync(new Exception("Erro ao atualizar cadeira no banco"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _repository.UpdateCadeiraAsync(cadeira));
        Assert.Equal("Erro ao atualizar cadeira no banco", exception.Message);
    }


}
