

using System.Text.Json;
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
            const string query= "INSERT INTO Alocacao (IdCadeira, DataHoraInicio, DataHoraFim) VALUES (@IdCadeira, @DataHoraInicio, @DataHorafim)";
            try
            {
                await connection.ExecuteAsync(query, new
                {
                    IdCadeira = alocacao.Cadeira.Id, // Extraindo apenas o ID da cadeira para a persistência
                    DataHoraInicio = alocacao.DataHoraInicio,
                    DataHoraFim = alocacao.DataHoraFim,
                });
                transaction.Commit();

            return alocacao;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                var additionalInfo = new
                {
                    TransactionId = Guid.NewGuid(),
                    Time = DateTime.UtcNow
                };

                var contextInfo = JsonSerializer.Serialize(additionalInfo);
                throw new Exception($"Erro ao executar a transação. Contexto: {contextInfo}", ex) ;
            }
        }
        public async Task<IEnumerable<Alocacao>> GetAllAlocacoesAsync()
        {
            using var connection = _context.CreateConnection();
            const string query = @"SELECT 
                    ac.Id AS IdAlocacao, 
                    ac.IdCadeira AS CadeiraId,  -- O splitOn precisa encontrar essa coluna
                    ac.DataHoraInicio, 
                    ac.DataHoraFim,
                    c.Id AS IdCadeira,  -- Coluna usada para mapear o objeto Cadeira
                    c.Numero, 
                    c.Descricao, 
                    c.TotalAlocacoes
                FROM Alocacao ac
                INNER JOIN Cadeira c ON ac.IdCadeira = c.Id";
            try
            {
                var alocacoes = await connection.QueryAsync<Alocacao, Cadeira, Alocacao>(query,(alocacao, cadeira) => {

                    alocacao.Cadeira = cadeira; // Associa o objeto Cadeira a Alocacao
                    return alocacao;
                }, splitOn: "CadeiraId"); // Informa ao Dapper onde ocorre a separação dos dados (Ele não lida mt bem com objetos complexos)
                return alocacoes;
            }
            catch (Exception ex)
            {
                var additionalInfo = new
                {
                    TransactionId = Guid.NewGuid(),
                    Time = DateTime.UtcNow
                };

                var contextInfo = JsonSerializer.Serialize(additionalInfo);
                throw new Exception($"Erro ao executar a transação. Contexto: {contextInfo}", ex) ;
            }
        }
        
        public async Task<IEnumerable<Alocacao>> GetAlocacoesPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            using var connection = _context.CreateConnection();
            const string query = @"
                SELECT * FROM Alocacao 
                WHERE (DataHoraInicio < @Fim AND DataHoraFim > @Inicio)";
            return await connection.QueryAsync<Alocacao>(query, new { Inicio = inicio, Fim = fim });
        }

    }
    
}