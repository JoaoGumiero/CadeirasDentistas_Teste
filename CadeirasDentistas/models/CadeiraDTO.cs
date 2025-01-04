

using System.ComponentModel.DataAnnotations;

namespace CadeirasDentistas.models
{
    public class CadeiraDTO
    {

        [Required]
        public int Numero { get; set; }

        [Required]
        public string Descricao { get; set; }
    }
}