

using System.ComponentModel.DataAnnotations;

namespace CadeirasDentistas.models
{
    public class Cadeira
    {
        public int Id { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public string Descricao { get; set; }

        public int TotalAlocacoes { get; set; }

        public List<Alocacao> Alocacoes {get; set;} = [];
    }
}