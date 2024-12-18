
using Microsoft.Data.SqlClient;

namespace CadeirasDentistas.Helper
{
        public static class ExceptionHandler
    {
        public static T Handle<T>(Func<T> action, ILogger logger, string errorMessage, object context)
        {
            try
            {
                return action();
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Erro no banco de dados: {Message}. Contexto: {@Context}", errorMessage, context);
                throw new Exception(errorMessage, ex); // Relança uma exceção genérica com mais contexto
            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Erro de validação: {Message}. Contexto: {@Context}", errorMessage, context);
                throw; // Relança a exceção de validação diretamente
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro inesperado: {Message}. Contexto: {@Context}", errorMessage, context);
                throw new Exception("Erro inesperado ao processar a solicitação.", ex);
            }
        }

        public static async Task<T> HandleAsync<T>(Func<Task<T>> action, ILogger logger, string errorMessage, object context = null)
        {
            try
            {
                return await action();
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Erro no banco de dados: {Message}. Contexto: {@Context}", errorMessage, context);
                throw new Exception(errorMessage, ex);
            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Erro de validação: {Message}. Contexto: {@Context}", errorMessage, context);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro inesperado: {Message}; Contexto: {@Context}", errorMessage, context);
                throw new Exception("Erro inesperado ao processar a solicitação.", ex);
            }
        }
    }
    
}