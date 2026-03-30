using Application.Requests.Pessoa;
using Application.Responses;
using Application.Responses.Pessoa;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IPessoaService
    {
        Task<ApiResponse<PessoaResponse>> AddAsync(CreatePessoaRequest request);
        Task<ApiResponse<PessoaResponse>> UpdateAsync(int id, UpdatePessoaRequest request);
        Task<ApiResponse<PessoaByIdResponse>> GetByIdAsync(int id);
        Task<ApiResponse<List<PessoaResponse>>> GetAllAsync(HttpResponse httpResponse, int page, int limit, string? search = null);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<int>> GetPessoasCountAsync();
    }
}
