
using CadeirasDentistas.models;
using CadeirasDentistas.models.context;

namespace CadeirasDentistas.Helper
{
    public static class AlocacaoValidacao
    {
        public static void Validate(Alocacao alocacao, ILogger logger, AlocacaoContext context) 
        {
            if (alocacao == null)
            {
                logger?.LogError("Erro de validação: Alocação é nula. Contexto: {@Context}", context);
                throw new ArgumentNullException(nameof(alocacao), "O objeto Cadeira não pode ser nulo.");
            }

            if (alocacao.Cadeira == null)
            {
                logger?.LogError("Erro de validação: Alocação sem cadeira. Contexto: {@Context}", context);
                throw new ArgumentNullException("A alocação precisar ser adjunta a uma cadeira.");
            }
            
            if (alocacao.DataHoraInicio <= DateTime.Today)
            {
                logger?.LogWarning("Erro de validação: Data de início inválida. Contexto: {@Context}", context);
                throw new ValidationException("A Data e Hora de início deve ser futura", "Data de Inicio", alocacao.DataHoraInicio);
            }

            if (alocacao.DataHoraFim <= DateTime.Today)
            {
                logger?.LogWarning("Erro de validação: Data de fim inválida. Contexto: {@Context}", context);
                throw new ValidationException("A Data e Hora de Fim deve ser futura", "Data Fim", alocacao.DataHoraFim);
            }

            if (alocacao.DataHoraInicio <= alocacao.DataHoraInicio)
            {
                logger?.LogWarning("Erro de validação: Data e hora de início deve ser anterior a Data e hora fim. Contexto: {@Context}", context);
                throw new ValidationException(" Data e hora de início deve ser anterior a Data e hora fim.", "Data de Inicio", alocacao.DataHoraInicio);
            }

        }

    }
    
}