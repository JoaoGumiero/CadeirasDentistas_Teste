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



        public async Task<IEnumerable<Alocacao>> AlocarAutoAsync(DateTime dataHoraInicio, DateTime dataHoraFim)
        {
            if (dataHoraInicio == default || dataHoraFim == default)
            {
                throw new ArgumentException("Datas inválidas fornecidas.");
            }

            if (dataHoraFim <= dataHoraInicio)
            {
                throw new ArgumentException("A data e hora final devem ser maior que a data e hora inicial.");
            }

            var alocacoesRealizadas = new List<Alocacao>();

            _logger.LogInformation("Iniciando alocação automática para o período {Inicio} - {Fim}", dataHoraInicio, dataHoraFim);

            var cadeiras = await _cadeiraRepository.GetAllCadeirasAsync();

            _logger.LogInformation("DataHoraInicio: {DataHoraInicio}, DataHoraFim: {DataHoraFim}, Cadeiras: {Cadeiras}", dataHoraInicio, dataHoraFim, cadeiras);

    
            if (cadeiras == null || !cadeiras.Any())
            {
                throw new InvalidOperationException("Não há cadeiras disponíveis no período selecionado.");
            }
    
            _logger.LogInformation("Existe Cadeiras");
            // Buscar alocações existentes dentro do período informado
            var alocacoesExistentes = await _repository.GetAlocacoesPorPeriodoAsync(dataHoraInicio, dataHoraFim);

            _logger.LogInformation("Passou o método GET ALOCACOES POR PERIODO");

            // Filtrar cadeiras disponíveis (sem conflito de horário)
            var cadeirasDisponiveis = cadeiras.FirstOrDefault(c => 
                !alocacoesExistentes.Any(a => 
                    a.Cadeira != null && 
                    a.Cadeira.Id == c.Id && 
                    dataHoraInicio < a.DataHoraFim && 
                    dataHoraFim > a.DataHoraInicio
                )
            );


            _logger.LogInformation("Passou o filtro para ter cadeiras disponíveis.");

            if (cadeirasDisponiveis != null)
            {
                // Aloca diretamente na cadeira disponível
                _logger.LogInformation("Cadeira disponível encontrada: {CadeiraId}", cadeirasDisponiveis.Id);
                var novaAlocacao = new Alocacao
                {
                    Cadeira = cadeirasDisponiveis,
                    DataHoraInicio = dataHoraInicio,
                    DataHoraFim = dataHoraFim
                };

                await _repository.AddAlocacaoAsync(novaAlocacao);
                alocacoesRealizadas.Add(novaAlocacao);

                _logger.LogInformation("Alocação realizada com sucesso na cadeira {CadeiraId}", cadeirasDisponiveis.Id);
                return alocacoesRealizadas;
            }

            // Nenhuma cadeira disponível, ajustando horários
            _logger.LogInformation("Nenhuma cadeira disponível. Ajustando alocações...");

            foreach (var cadeira in cadeiras)
            {
                var alocacoesCadeira = alocacoesExistentes.Where(a => a.Cadeira.Id == cadeira.Id)
                    .OrderBy(a => a.DataHoraInicio)
                    .ToList();

                bool horarioAjustado = AjustarHorarios(alocacoesCadeira, dataHoraInicio, dataHoraFim);
                if (horarioAjustado)
                {
                    var novaAlocacao = new Alocacao
                    {
                        Cadeira = cadeira,
                        DataHoraInicio = dataHoraInicio,
                        DataHoraFim = dataHoraFim
                    };

                    await _repository.AddAlocacaoAsync(novaAlocacao);
                    alocacoesRealizadas.Add(novaAlocacao);

                    _logger.LogInformation("Horários ajustados e nova alocação realizada na cadeira {CadeiraId}", cadeira.Id);
                    return alocacoesRealizadas;
                }
            }
            throw new Exception("Não foi possível alocar a cadeira nem ajustar os horários existentes.");
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