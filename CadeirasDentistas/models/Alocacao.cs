

using System.ComponentModel.DataAnnotations;

namespace CadeirasDentistas.models
{
    public class Alocacao
    {
        public int Id { get; set; }
        
        // Associação com o objeto Cadeira || Dapper não consegue lidar com objetos complexos, assim eu preciso utilizar ID da cadeira de forma básica
        [Required]
        public Cadeira Cadeira { get; set; }

        [Required]
        public DateTime DataHoraInicio { get; set; }

        [Required]
        public DateTime DataHoraFim { get; set; }
    }
    
}