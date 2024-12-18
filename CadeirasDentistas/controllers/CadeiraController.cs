using CadeirasDentistas.models;
using CadeirasDentistas.services;
using Microsoft.AspNetCore.Mvc;

namespace CadeirasDentistas.Controller
{

    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Add([FromBody] Cadeira cadeira)
        {
            var createdCadeira = await _cadeiraService.AddCadeiraAsync(cadeira);
            return CreatedAtAction(nameof(GetById), new { id = createdCadeira.Id }, createdCadeira);
        }

        // PUT: api/cadeira/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cadeira cadeira)
        {
            var updatedCadeira = await _cadeiraService.UpdateCadeiraAsync(cadeira);
            return Ok(updatedCadeira);
        }

        // DELETE: api/cadeira/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cadeiraService.DeleteCadeiraAsync(id);
            return NoContent();
        }
    }
    
}