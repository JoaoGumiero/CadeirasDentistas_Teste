
using CadeirasDentistas.models;

namespace CadeirasDentistas.Repository
{
    public interface IAlocacaoRepository
    {
    Task<IEnumerable<Alocacao>> GetAllAlocacoesAsync();
    Task<Alocacao> AddAlocacaoAsync(Alocacao alocacao);
    Task<IEnumerable<Alocacao>> GetAlocacoesPorPeriodoAsync(DateTime inicio, DateTime fim);
    }
    
}