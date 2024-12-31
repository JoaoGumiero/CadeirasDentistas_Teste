

using CadeirasDentistas.models;
using Microsoft.Extensions.Logging;

namespace CadeirasDentistas.Repository
{
    public interface ICadeiraRepository
    {
        Task<IEnumerable<Cadeira>> GetAllCadeirasAsync();
        Task<Cadeira> GetCadeiraByIdAsync(int id);
        Task<Cadeira> GetCadeiraByNumberAsync(int number);
        Task<Cadeira> AddCadeiraAsync(Cadeira cadeira);
        Task<Cadeira> UpdateCadeiraAsync(Cadeira cadeira);
        Task<bool> DeleteCadeiraAsync(int id);
        Task<IEnumerable<Cadeira>> GetCadeirasDisponiveisAsync(DateTime dataHoraInicio, DateTime dataHoraFim);
    }
}