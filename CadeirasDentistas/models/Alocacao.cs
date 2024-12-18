

using System.ComponentModel.DataAnnotations;

namespace CadeirasDentistas.models
{
    public class Alocacao
    {
        public int Id { get; set; }
        
        // Associação com o objeto Cadeira
        [Required]
        public Cadeira Cadeira { get; set; }

        [Required]
        public DateTime DataHoraInicio { get; set; }

        [Required]
        public DateTime DataHoraFim { get; set; }
    }
    
}