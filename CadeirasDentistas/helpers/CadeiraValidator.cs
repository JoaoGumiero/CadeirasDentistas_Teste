

using CadeirasDentistas.models;
using CadeirasDentistas.models.context;

namespace CadeirasDentistas.Helper
{
    
    public static class ValidateCadeira
    {

        public static void Validate(Cadeira cadeira, ILogger logger, CadeiraContext context)
        {
            if (cadeira == null)
            {
                logger?.LogError("Erro de validação: Cadeira é nula. Contexto: {@Context}", context);
                throw new ArgumentNullException(nameof(cadeira), "O objeto Cadeira não pode ser nulo.");
            }

            if (cadeira.Numero > 0)
            {
                logger?.LogError("Erro de validação: Número da cadeira necessita de positiva. Contexto: {@Context}", context);
                throw new ArgumentNullException("O número da cadeira é obrigatório e deve ser positiva.");
            }
            
            if (cadeira.Descricao == null)
            {
                logger?.LogError("Erro de validação: Descrição da Cadeira é nula. Contexto: {@Context}", context);
                throw new ValidationException("A Descrição da cadeira é obrigatório.", "Descrição da Cadeira", cadeira.Descricao);
            }

            if (cadeira.Descricao.Length > 255)
            {
                logger?.LogError("Erro de validação: Descrição maior que 255. Contexto: {@Context}", context);
                throw new ValidationException("A Descrição não pode ultrapassar 255 caracteres", "Número de Caracteres: ", cadeira.Descricao.Length);
            }

            if (cadeira.Numero.ToString().Length > 10)
            {
                logger?.LogError("Erro de validação: Número da Cadeira é maior que 10. Contexto: {@Context}", context);
                throw new ValidationException("O número da Cadeira não pode ultrapassar 10 caracteres", "Número da Cadeira", cadeira.Numero);
            }

            if (cadeira.TotalAlocacoes < 0)
            {
                logger?.LogError("Erro de validação: Alocações da Cadeira é menor que 0. Contexto: {@Context}", context);
                throw new ValidationException("A Cadeira não pode ter um número negativo de alocações", "Alocação na Cadeira", cadeira.Alocacoes);
            }
        }

    }
    
}