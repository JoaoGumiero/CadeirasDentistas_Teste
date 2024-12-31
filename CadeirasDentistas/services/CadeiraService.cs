


using System.ComponentModel.DataAnnotations;
using CadeirasDentistas.Repository;
using CadeirasDentistas.Helper;
using CadeirasDentistas.models;
using Microsoft.Data.SqlClient;
using ValidationException = CadeirasDentistas.Helper.ValidationException;
using CadeirasDentistas.models.context;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CadeirasDentistas.services
{
    public class CadeiraService : ICadeiraService
    {
        // instância do repositório na classe service
        private readonly ICadeiraRepository _repository;

        private readonly ILogger<CadeiraService> _logger;

        public CadeiraService(ICadeiraRepository repository, ILogger<CadeiraService> logger)
        {
            _repository = repository;
             _logger = logger;

        }

        public async Task<IEnumerable<Cadeira>> GetAllCadeirasAsync()
        {
            return await _repository.GetAllCadeirasAsync();
        }

        public async Task DeleteCadeiraAsync(int id)
        {
            var cadeira = await _repository.GetCadeiraByIdAsync(id);
            if (cadeira.Numero <= 0 )
            {
                throw new KeyNotFoundException("Essa cadeira não existe.");
            }
            await ExceptionHandler.HandleAsync(async () =>
            {
                return await _repository.DeleteCadeiraAsync(id);
            }, _logger, "Erro ao deletar a cadeira.", new { Numero = cadeira.Numero, Descricao = cadeira.Descricao });
        }

        public async Task<Cadeira> UpdateCadeiraAsync(Cadeira cadeira)
        {
            var context = new CadeiraContext("UpdateCadeiraAsync", cadeira);
            Helper.ValidateCadeira.Validate(cadeira, _logger, context);
            return await ExceptionHandler.HandleAsync(async () =>{
                return await _repository.UpdateCadeiraAsync(cadeira);
            }, _logger, "Erro ao atualizar a cadeira,", new {Cadeira = cadeira});
        }


        public async Task<Cadeira> GetCadeiraByIdAsync(int id)
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                var cadeira = await _repository.GetCadeiraByIdAsync(id);
                if(cadeira == null)
                {
                    throw new KeyNotFoundException($"Cadeira com o id: {id}, não foi encontrada.");
                }
                return cadeira;
            }, _logger, "Erro ao buscar a cadeira pelo ID.", new { Id = id });
        }

        public async Task<Cadeira> GetCadeiraByNumberAsync(int number)
        {
            return await ExceptionHandler.HandleAsync(async () =>
            {
                var cadeira = await _repository.GetCadeiraByNumberAsync(number);
                if(cadeira == null)
                {
                    throw new KeyNotFoundException($"Cadeira com o número: {number}, não foi encontrada.");
                }
                return cadeira;
            }, _logger, "Erro ao buscar a cadeira pelo Número.", new { Numero = number});
        }

        public async Task<Cadeira> AddCadeiraAsync(Cadeira cadeira)
        {
            _logger.LogInformation("Entrou no Service");
            _logger.LogInformation("criar context");
            var context = new CadeiraContext("AddCadeiraAsync", cadeira);
            _logger.LogInformation("validação helper");
            Helper.ValidateCadeira.Validate(cadeira, _logger,context);
            _logger.LogInformation("passou, agora vai entrar no handleasync");
            return await ExceptionHandler.HandleAsync(async () =>
            {
                _logger.LogInformation("entroy");
                // Verifica duplicidade no banco
                _logger.LogInformation("verificar duplicidade no banco");
                var existente = await _repository.GetCadeiraByNumberAsync(cadeira.Numero);
                _logger.LogInformation("fez verificação");
                if (existente != null)
                {
                    _logger.LogInformation("Existe numero");
                    throw new ValidationException("O número da cadeira já está em uso.", "Numero", cadeira.Numero);
                }
                _logger.LogInformation("Não existe numero");
                _logger.LogInformation("adicionaro ao banco");
                // Adiciona no banco
                await _repository.AddCadeiraAsync(cadeira);
                _logger.LogInformation("Adicionou e vai retornar cadeira");
                return cadeira;
            }, _logger, "Erro ao adicionar a cadeira.", new { Numero = cadeira.Numero });
        }

    }
 
}