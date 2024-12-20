

using CadeirasDentistas.Data;
using CadeirasDentistas.models;
using Dapper;

namespace CadeirasDentistas.Repository
{
    public class AlocacaoRepository : IAlocacaoRepository
    {
        private readonly ApplicationDbContext _context;
         private readonly ILogger<AlocacaoRepository> _logger;

        public AlocacaoRepository(ApplicationDbContext context, ILogger<AlocacaoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Alocacao> AddAlocacaoAsync(Alocacao alocacao)
        {
            using var connection = _context.CreateConnection();
            using var transaction = connection.BeginTransaction();
            const string query= "INSERT INTO Alocacao (IdCadeira, Cadeira, DataHoraInicio, DataHoraFim) VALUES (@IdCadeira, @Cadeira, @DataHoraInicio, @DataHorafim)";
            try
            {
                await connection.ExecuteAsync(query, new
                {
                    IdCadeira = alocacao.Cadeira.Id,
                    Cadeira = alocacao.Cadeira,
                    DataHoraInicio = alocacao.DataHoraInicio,
                    DataHoraFim = alocacao.DataHoraFim,
                });
                transaction.Commit();

            return alocacao;
            }
            catch
            {
                // Rollback caso de problema
                transaction.Rollback();
                
                throw;
            }
        }
        public async Task<IEnumerable<Alocacao>> GetAllAlocacoesAsync()
        {
            using var connection = _context.CreateConnection();
            const string query = @"SELECT 
                    ac.Id AS IdAlocacao, 
                    ac.IdCadeira, 
                    ac.DataHoraInicio, 
                    ac.DataHoraFim,
                    c.Id AS IdCadeira, 
                    c.Numero, 
                    c.Descricao, 
                    c.TotalAlocacoes
                FROM Alocacao ac
                INNER JOIN Cadeira c ON ac.IdCadeira = c.Id";
            try
            {
                var alocacoes = await connection.QueryAsync<Alocacao>(query);
                return alocacoes;
            }
            catch
            {
                // Implementar erro
                throw;
            }
        }

    }
    
}