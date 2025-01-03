using Microsoft.Data.SqlClient;

using CadeirasDentistas.models;
using System.Transactions;
using Microsoft.Extensions.Logging;
using CadeirasDentistas.Data;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CadeirasDentistas.Repository
{
    public class CadeiraRepository : ICadeiraRepository
    {

        private readonly ApplicationDbContext _context;
         private readonly ILogger<CadeiraRepository> _logger;

        public CadeiraRepository(ApplicationDbContext context, ILogger<CadeiraRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Cadeira> AddCadeiraAsync(Cadeira cadeira)
        {
            _logger.LogInformation("Tentando adicionar cadeira: {@Cadeira}", cadeira);
            using var connection = _context.CreateConnection();
            _logger.LogInformation("Começando transação");
            using var transaction = connection.BeginTransaction();
            const string query= "INSERT INTO Cadeira (Numero, Descricao, TotalAlocacoes) VALUES (@Numero, @Descricao, @TotalAlocacoes)";
            try
            {
                _logger.LogInformation("Entrou no try");
                await connection.ExecuteAsync(query, new
                {
                    Numero = cadeira.Numero,
                    Descricao = cadeira.Descricao,
                    TotalAlocacoes = cadeira.TotalAlocacoes,
                });
            _logger.LogInformation("Saiu do try");
            transaction.Commit();
            _logger.LogInformation("Comitou");
            return cadeira;
            }
            catch
            {
                _logger.LogInformation("Achou erro e vai dar rollback");
                transaction.Rollback();
                _logger.LogInformation("Deu rollback, agora vai dar o throw");
                // Implementar erro
                throw;
            }
        }

        public async Task<bool> DeleteCadeiraAsync(int id)
        {

            using var connection = _context.CreateConnection();
            using var transaction = connection.BeginTransaction();
            const string query = "DELETE FROM Cadeira WHERE Id = @Id";
            const string deleteAlocacoesQuery = "DELETE FROM Alocacao WHERE IdCadeira = @Id";
            try
            {
                await connection.ExecuteAsync(deleteAlocacoesQuery, new { Id = id }, transaction);
                await connection.ExecuteAsync(query, new { Id = id });
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public async Task<Cadeira> GetCadeiraByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            const string query = "SELECT * FROM Cadeira WHERE Id = @Id";
            try
            {
                var cadeira = await connection.QueryFirstOrDefaultAsync<Cadeira>(query, new {Id = id});
                return cadeira;
            }
            catch
            {
                // Implementar erro
                throw;
            }
        }

        public async Task<Cadeira> UpdateCadeiraAsync(Cadeira cadeira)
        {
            using var connection = _context.CreateConnection();
            const string query = "UPDATE Cadeira SET Numero = @Numero, Descricao = @Descricao, TotalAlocacoes = @TotalAlocacoes WHERE Id = @Id";
            using var transaction = connection.BeginTransaction();
            try
            {
                await connection.ExecuteAsync(query, new{
                    Id = cadeira.Id,
                    Numero = cadeira.Numero,
                    Descricao = cadeira.Descricao,
                    TotalAlocacoes = cadeira.TotalAlocacoes
                });
            transaction.Commit();
            return cadeira;
            }
            catch
            {
                transaction.Rollback();
                // Implementar erro
                throw;
            }
        }

        public async Task<Cadeira> GetCadeiraByNumberAsync(int number)
        {
            using var connection = _context.CreateConnection();
            const string query = "SELECT * FROM Cadeira WHERE Numero = @Numero";
            try
            {
                return await connection.QueryFirstOrDefaultAsync<Cadeira>(query, new {Numero = number});
            }
            catch
            {
                // Implementar erro
                throw;
            }
        }

        public async Task<IEnumerable<Cadeira>> GetCadeirasDisponiveisAsync(DateTime dataHoraInicio, DateTime dataHoraFim)
        {
            using var connection = _context.CreateConnection();
            const string query = @"
                SELECT * FROM Cadeira WHERE NOT EXISTS (SELECT 1 FROM Alocacao a WHERE a.IdCadeira = c.Id AND ((a.DataHoraInicio < @DataHoraFim AND a.DataHoraFim > @DataHoraInicio)))";
            return await connection.QueryAsync<Cadeira>(query, new
            {
                DataHoraInicio = dataHoraInicio,
                DataHoraFim = dataHoraFim
            });
        }

        public async Task<IEnumerable<Cadeira>> GetAllCadeirasAsync()
        {
            _logger.LogInformation("Iniciando busca de todas as cadeiras.");
            using var connection = _context.CreateConnection();
            const string query = "SELECT * FROM Cadeira";
            try
            {
                var cadeiras = await connection.QueryAsync<Cadeira>(query);
                 _logger.LogInformation("{TotalCadeiras} cadeiras encontradas.", cadeiras.Count());
                return cadeiras;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as cadeiras.");
                throw;
            }
        }
    }

}