using CadeirasDentistas.models;

namespace CadeirasDentistas.services
{
        public interface ICadeiraService
    {
        Task<IEnumerable<Cadeira>> GetAllCadeirasAsync();
        Task<Cadeira> GetCadeiraByIdAsync(int id);
        Task<Cadeira> AddCadeiraAsync(Cadeira cadeira);
        Task<Cadeira> UpdateCadeiraAsync(Cadeira cadeira);
        Task DeleteCadeiraAsync(int id);
    }
    
}