using CadeirasDentistas.models;

namespace CadeirasDentistas.services
{
    public interface IAlocacaoService
    {
        Task<IEnumerable<Alocacao>> GetAllAlocacoesAsync();
        Task<Alocacao>  AlocarAutoAsync(DateTime inicio, DateTime fim);
    }
    
}