using Application.Interfaces;
using Application.Requests.Transacao;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/transacao")]
    public class TransacaoController : BaseController
    {
        private readonly ITransacaoService _transacaoService;

        public TransacaoController(ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        /// <summary>
        /// Adiciona uma nova transação.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] CreateTransacaoRequest request)
        {
            var result = await _transacaoService.AddAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Obtém uma transação por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            var result = await _transacaoService.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Atualiza uma transação existente por ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateTransacaoRequest request)
        {
            var result = await _transacaoService.UpdateAsync(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Remove uma transação por ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var result = await _transacaoService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Obtém uma lista de transações com paginação e filtros opcionais.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAllAsync(int page = 1, int limit = 10, int? pessoaId = null, int? categoriaId = null, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null)
        {
            var result = await _transacaoService.GetAllAsync(page, limit, pessoaId, categoriaId, valor, tipo, search);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Obtém a contagem total de transações.
        /// </summary>
        [HttpGet("count")]
        public async Task<ActionResult> GetTransacoesCountAsync()
        {
            var result = await _transacaoService.GetTransacoesCountAsync();
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
