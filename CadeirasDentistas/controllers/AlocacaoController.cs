using CadeirasDentistas.models;
using CadeirasDentistas.services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CadeirasDentistas.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class AlocacaoController : ControllerBase
    {
        private readonly IAlocacaoService _alocacaoService;

        public AlocacaoController(IAlocacaoService alocacaoService)
        {
            _alocacaoService = alocacaoService;
        }

        // GET: api/alocacao
        [HttpGet]
        [SwaggerOperation(
            Summary = "Retorna todas as Alocações."
        )]
        public async Task<IActionResult> GetAll()
        {
            var alocacoes = await _alocacaoService.GetAllAlocacoesAsync();
            return Ok(alocacoes);
        }

        // POST: api/alocacao/automatico
        [HttpPost("automatico")]
        [SwaggerOperation(
            Summary = "Alocação automática de cadeiras",
            Description = "Realiza a alocação automática de cadeiras para um período específico."
        )]
        public async Task<IActionResult> AlocarAutomaticamente(
            [FromQuery, SwaggerParameter(Description = "Data e hora de início do período para alocação automática. Exemplo: 2025-01-09T10:00:00Z")] DateTime inicio, 
            [FromQuery, SwaggerParameter(Description = "Data e hora de término do período para alocação automática. Exemplo: 2025-01-09T12:00:00Z")] DateTime fim)
        {
            var alocacoes = await _alocacaoService.AlocarAutoAsync(inicio, fim);
            return Ok(alocacoes);
        }
    }
    
}