using CadeirasDentistas.models;
using CadeirasDentistas.services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll()
        {
            var alocacoes = await _alocacaoService.GetAllAlocacoesAsync();
            return Ok(alocacoes);
        }

        // POST: api/alocacao/automatico
        [HttpPost("automatico")]
        public async Task<IActionResult> AlocarAutomaticamente([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            var alocacoes = await _alocacaoService.AlocarAutoAsync(inicio, fim);
            return Ok(alocacoes);
        }
    }
    
}