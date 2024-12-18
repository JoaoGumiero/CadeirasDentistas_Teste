namespace CadeirasDentistas.models.context
{
    public class AlocacaoContext
    {
        public string Metodo { get; set; }
        public int? AlocacaoId { get; set; }
        public int? CadeiraId { get; set; }
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public DateTime Timestamp { get; set; } //Data e hora da operação

        public AlocacaoContext(string metodo, Alocacao alocacao)
        {
            Metodo = metodo;
            AlocacaoId = alocacao?.Id;
            DataHoraInicio = alocacao?.DataHoraInicio;
            DataHoraFim = alocacao?.DataHoraFim;
            Timestamp = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Método: {Metodo}, AlocacaoId: {AlocacaoId}, CadeiraId: {CadeiraId}, Inicio: {DataHoraInicio}, Fim: {DataHoraFim}, HorarioOperacao: {Timestamp}";
        }
}
    
}