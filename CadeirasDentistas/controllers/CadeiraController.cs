using CadeirasDentistas.models;
using CadeirasDentistas.services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace CadeirasDentistas.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class CadeiraController : ControllerBase
    {
        private readonly ICadeiraService _cadeiraService;

        public CadeiraController(ICadeiraService cadeiraService)
        {
            _cadeiraService = cadeiraService;
        }

        // GET: api/cadeira
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cadeiras = await _cadeiraService.GetAllCadeirasAsync();
            return Ok(cadeiras);
        }

        // GET: api/cadeira/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cadeira = await _cadeiraService.GetCadeiraByIdAsync(id);
            return Ok(cadeira);
        }

        // POST: api/cadeira
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CadeiraDTO cadeiraDTO)
        {
            var cadeira = new Cadeira
            {
               Numero = cadeiraDTO.Numero,
               Descricao = cadeiraDTO.Descricao,
               TotalAlocacoes = 0,
               Alocacoes = [], 
            };
            var createdCadeira = await _cadeiraService.AddCadeiraAsync(cadeira);
            return CreatedAtAction(nameof(GetById), new { id = createdCadeira.Id }, createdCadeira);
        }

        // PUT: api/cadeira/{id}
        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(CadeiraDTO), typeof(CadeiraExample))]
        public async Task<IActionResult> Update(int id, [FromBody] Cadeira cadeira)
        {
            var updatedCadeira = await _cadeiraService.UpdateCadeiraAsync(cadeira);
            return Ok(updatedCadeira);
        }


        // Ajustar o corpo do Swagger para a requisição Put
        public class CadeiraExample : IExamplesProvider<Cadeira>
        {
            public Cadeira GetExamples()
            {
                return new Cadeira
                {
                    Id = 1,
                    Numero = 10,
                    Descricao = "Cadeira confortável",
                    TotalAlocacoes = 1,
                    Alocacoes = new List<Alocacao>
                    {
                        new Alocacao
                        {
                            Id = 1,
                            Cadeira = new Cadeira
                            {
                                Numero = 10,
                                Descricao = "Cadeira confortável"
                            },
                            DataHoraInicio = DateTime.UtcNow,
                            DataHoraFim = DateTime.UtcNow.AddHours(1)
                        }
                    }
                };
            }
        }

        // DELETE: api/cadeira/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cadeiraService.DeleteCadeiraAsync(id);
            return Ok("Cadeira deletada com sucesso.");
        }
    }
    
}