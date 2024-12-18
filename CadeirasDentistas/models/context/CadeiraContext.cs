namespace CadeirasDentistas.models.context
{
    public class CadeiraContext
    {
        public string Metodo { get; set; }
        public int? CadeiraId { get; set; }
        public int Numero { get; set; }
        public string Descricao { get; set; }
        public DateTime Timestamp { get; set; } //Data e hora da operação

        public CadeiraContext(string metodo, Cadeira cadeira)
        {
            Metodo = metodo;
            CadeiraId = cadeira?.Id;
            Numero = cadeira.Numero;
            Descricao = cadeira?.Descricao;
            Timestamp = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Método: {Metodo}, CadeiraId: {CadeiraId}, Numero: {Numero}, Descricao: {Descricao}, HorarioOperacao: {Timestamp}";
        }
    }
    
}