using Application.Interfaces;
using Application.Pagination;
using Application.Requests.Categoria;
using Application.Responses;
using Application.Responses.Categoria;
using Application.Responses.Transacao;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Extensions;
using Domain.Interfaces.Repositories;
using System.Net;

namespace Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CategoriaResponse>> AddAsync(CreateCategoriaRequest request)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(request);

                var created = await _categoriaRepository.AddAsync(categoria);

                var response = new CategoriaResponse
                {
                    Id = created.Id,
                    Descricao = created.Descricao,
                    Finalidade = created.Finalidade.Value(),
                };

                return new ApiResponse<CategoriaResponse>(true, HttpStatusCode.Created, response, "Categoria criada com sucesso", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoriaResponse>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while creating the category: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<CategoriaResponse>> UpdateAsync(int id, UpdateCategoriaRequest request)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdAsync(id);
                if (categoria == null) return new ApiResponse<CategoriaResponse>(false, HttpStatusCode.NotFound, null, "Categoria não encontrada", null, null);

                categoria.Update(request.Descricao, request.Finalidade);

                var updated = await _categoriaRepository.UpdateAsync(categoria);

                var response = _mapper.Map<CategoriaResponse>(updated);

                return new ApiResponse<CategoriaResponse>(true, HttpStatusCode.OK, response, "Categoria atualizada com sucesso", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoriaResponse>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while updating the category: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<CategoriaByIdResponse>> GetByIdAsync(int id)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdAsync(id);
                if (categoria == null) return new ApiResponse<CategoriaByIdResponse>(false, HttpStatusCode.NotFound, null, "Categoria não encontrada", null, null);

                var response = new CategoriaByIdResponse
                {
                    Id = categoria.Id,
                    Descricao = categoria.Descricao,
                    Finalidade = categoria.Finalidade.Value(),
                    Transacoes = [.. categoria.Transacoes.Select(t => new TransacaoResponse
                    {
                        Id = t.Id,
                        Descricao = t.Descricao,
                        Valor = t.Valor,
                        Tipo = t.Tipo.Value(),
                        PessoaId = t.PessoaId,
                        CategoriaId = t.CategoriaId
                    })]
                };
                return new ApiResponse<CategoriaByIdResponse>(true, HttpStatusCode.OK, response, "Categoria retrieved successfully", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoriaByIdResponse>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while retrieving the category: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdAsync(id);
                if (categoria == null) return new ApiResponse<bool>(false, HttpStatusCode.NotFound, false, "Categoria não encontrada", null, null);

                await _categoriaRepository.DeleteAsync(id);

                return new ApiResponse<bool>(true, HttpStatusCode.OK, null, "Categoria deletada com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while retrieving the category: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<List<CategoriaResponse>>> GetAllAsync(int page, int limit, CategoriaFinalidadeEnum? finalidade = null, string? search = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(search)) 
                    search = search.Trim();
                
                var categorias = _categoriaRepository.GetAllAsync(finalidade, search);

                var paginated = await PaginatedResult<Categoria>.CreateAsync(categorias, page, limit);

                var response = categorias.Select(c => new CategoriaResponse
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    Finalidade = c.Finalidade.Value(),
                }).ToList();

                var result = new PaginatedResult<CategoriaResponse>(response, paginated.TotalCount, paginated.PageIndex, paginated.PageSize);

                return new ApiResponse<List<CategoriaResponse>>(true, HttpStatusCode.OK, result, "Categorias obtidas com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CategoriaResponse>>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while retrieving categories: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<int>> GetCategoriasCountAsync()
        {
            try
            {
                var categorias = await _categoriaRepository.GetCategoriasCountAsync();

                return new ApiResponse<int>(true, HttpStatusCode.OK, categorias, "Contagem de categorias obtida com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while counting categories: {ex.Message}", null, null);
            }
        }
    }
}
