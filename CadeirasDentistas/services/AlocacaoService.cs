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

        public AlocacaoService(IAlocacaoRepository repository, ILogger<AlocacaoService> logger, ICadeiraRepository cadeiraRepository)
            {
                _repository = repository;
                _logger = logger;
                _cadeiraRepository = cadeiraRepository;
            }



        public async Task<IEnumerable<Alocacao>> GetAllAlocacoesAsync()
        {
            return await _repository.GetAllAlocacoesAsync();
        }



        public async Task<Alocacao> AlocarAutoAsync(DateTime dataHoraInicio, DateTime dataHoraFim)
        {
            _logger.LogInformation("Iniciando alocação automática para o período {Inicio} - {Fim}", dataHoraInicio, dataHoraFim);

            // Obter todas as cadeiras e alocações existentes
            var cadeiras = await _cadeiraRepository.GetAllCadeirasAsync();

            // Filtrar cadeiras que estão disponíveis no período informado
            var cadeirasDisponiveis = cadeiras.Where(c =>
                c.Alocacoes.All(a =>
                    a.DataHoraFim <= dataHoraInicio || a.DataHoraInicio >= dataHoraFim)).ToList();

            if (!cadeirasDisponiveis.Any())
            {
                // Nenhuma cadeira disponível
                return null;
            }

            // Selecionar a cadeira disponível com o menor número de alocações no dia
            var cadeiraSelecionada = cadeirasDisponiveis
                .OrderBy(c => c.TotalAlocacoes)
                .ThenBy(c => c.Numero) // Para desempate, considerar o número da cadeira
                .FirstOrDefault();

            var alocacao = new Alocacao 
            {
                Cadeira = cadeiraSelecionada,
                DataHoraInicio = dataHoraInicio,
                DataHoraFim = dataHoraFim
            };

            cadeiraSelecionada.Alocacoes.Add(alocacao);
            
            var alocacaoRealizada = await _repository.AddAlocacaoAsync(alocacao);

            alocacaoRealizada.Cadeira.Alocacoes = null;

            return alocacaoRealizada;
        }

        private bool AjustarHorarios(List<Alocacao> alocacoes, DateTime dataHoraInicio, DateTime dataHoraFim)
        {
            foreach (var alocacao in alocacoes)
            {
                if (alocacao.DataHoraFim <= dataHoraInicio || alocacao.DataHoraInicio >= dataHoraFim)
                {
                    return true;
                }
            }
            return false;
        }
        
    }
    
}