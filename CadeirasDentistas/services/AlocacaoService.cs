using CadeirasDentistas.Helper;
using CadeirasDentistas.models;
using CadeirasDentistas.Repository;

namespace CadeirasDentistas.services
{
    public class AlocacaoService : IAlocacaoService
    {
        private readonly IAlocacaoRepository _repository;
        private readonly ICadeiraRepository _cadeiraRepository;

        private readonly ILogger<AlocacaoService> _logger;

        public AlocacaoService(IAlocacaoRepository repository, ILogger<AlocacaoService> logger)
            {
                _repository = repository;
                _logger = logger;
            }
        public async Task<IEnumerable<Alocacao>> GetAllAlocacoesAsync()
        {
            return await _repository.GetAllAlocacoesAsync();
        }
        public async Task<IEnumerable<Alocacao>> AlocarAutoAsync(DateTime dataHoraInicio, DateTime dataHoraFim)
        {
            if (dataHoraFim <= dataHoraInicio)
            {
                throw new ArgumentException("A data e hora final devem ser maior que a data e hora inicial.");
            }

            var cadeiras = (await _cadeiraRepository.GetCadeirasDisponiveisAsync(dataHoraInicio, dataHoraFim)).ToList();

            if (!cadeiras.Any())
            {
                throw new InvalidOperationException("Não há cadeiras disponíveis no período selecionado.");
            }

            var alocacoes = new List<Alocacao>();
            var totalCadeiras = cadeiras.Count;

            // Calcular a duração de cada bloco baseado no número de cadeiras
            var duracaoPorCadeira = (dataHoraFim - dataHoraInicio).TotalMinutes / totalCadeiras;

            var horarioAtual = dataHoraInicio;

            // Itera pelas cadeiras disponíveis e distribui o tempo proporcionalmente
            for (int i = 0; i < totalCadeiras; i++)
            {
                var cadeira = cadeiras[i];
                var horarioFim = horarioAtual.AddMinutes(duracaoPorCadeira);

                if (horarioFim > dataHoraFim)
                {
                    horarioFim = dataHoraFim; // Garantir que não ultrapasse o horário final
                }

                var alocacao = new Alocacao
                {
                    Cadeira = cadeira,
                    DataHoraInicio = horarioAtual,
                    DataHoraFim = horarioFim
                };

                await _repository.AddAlocacaoAsync(alocacao);
                alocacoes.Add(alocacao);

                // Atualizar o horário inicial para a próxima alocação
                horarioAtual = horarioFim;

                // Se o horário final for alcançado, encerra o loop
                if (horarioAtual >= dataHoraFim)
                {
                    break;
                }
            }
            return alocacoes;
        }
    }
    
}