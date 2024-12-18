
using CadeirasDentistas.models;

namespace CadeirasDentistas.Repository
{
    public interface IAlocacaoRepository
    {
    Task<IEnumerable<Alocacao>> GetAllAlocacoesAsync();
    Task<Alocacao> AddAlocacaoAsync(Alocacao alocacao);
    }
    
}