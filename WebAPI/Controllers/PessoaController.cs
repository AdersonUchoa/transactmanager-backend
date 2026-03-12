using Application.Interfaces;
using Application.Requests.Pessoa;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/pessoa")]
    public class PessoaController : BaseController
    {
        private readonly IPessoaService _pessoaService;

        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        /// <summary>
        /// Adiciona uma nova pessoa.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CreatePessoaRequest request)
        {
            var result = await _pessoaService.AddAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Obtém uma pessoa por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            var result = await _pessoaService.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Obtém uma lista de pessoas com paginação e filtro de busca.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAllAsync(int page = 1, int limit = 10, string? search = null)
        {
            var result = await _pessoaService.GetAllAsync(page, limit, search);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Atualiza uma pessoa existente por ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdatePessoaRequest request)
        {
            var result = await _pessoaService.UpdateAsync(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Remove uma pessoa por ID e suas transações.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var result = await _pessoaService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Obtém a contagem total de pessoas.
        /// </summary>
        [HttpGet("count")]
        public async Task<ActionResult> GetPessoasCountAsync()
        {
            var result = await _pessoaService.GetPessoasCountAsync();
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
