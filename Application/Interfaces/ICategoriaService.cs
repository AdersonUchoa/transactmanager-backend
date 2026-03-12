using Application.Requests.Categoria;
using Application.Responses;
using Application.Responses.Categoria;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<ApiResponse<CategoriaResponse>> AddAsync(CreateCategoriaRequest request);
        Task<ApiResponse<CategoriaResponse>> UpdateAsync(int id, UpdateCategoriaRequest request);
        Task<ApiResponse<CategoriaByIdResponse>> GetByIdAsync(int id);
        Task<ApiResponse<List<CategoriaResponse>>> GetAllAsync(CategoriaFinalidadeEnum? finalidade = null, string? search = null);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<int>> GetCategoriasCountAsync();
    }
}
