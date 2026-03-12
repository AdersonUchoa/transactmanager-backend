using Application.Interfaces;
using Application.Requests.Categoria;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/categorias")]
    public class CategoriaController : BaseController
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        /// <summary>
        /// Cadastra uma nova categoria.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddAsync([FromQuery] CreateCategoriaRequest request)
        {
            var result = await _categoriaService.AddAsync(request);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Obtém uma categoria por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            var result = await _categoriaService.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Obtém uma lista de categorias com paginação e filtros opcionais.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAllAsync(int page = 1, int limit = 10, string? search = null, CategoriaFinalidadeEnum? finalidade = null)
        {
            var result = await _categoriaService.GetAllAsync(page, limit, finalidade, search);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Atualiza uma categoria existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromQuery] UpdateCategoriaRequest request)
        {
            var result = await _categoriaService.UpdateAsync(id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Deleta uma categoria por ID e suas transações.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var result = await _categoriaService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Obtém a contagem total de categorias.
        /// </summary>
        [HttpGet("count")]
        public async Task<ActionResult> GetCategoriasCountAsync()
        {
            var result = await _categoriaService.GetCategoriasCountAsync();
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
