using Application.Pagination;
using Application.Requests.Transacao;
using Application.Responses;
using Application.Responses.Transacao;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<ApiResponse<TransacaoResponse>> AddAsync(CreateTransacaoRequest request);
        Task<ApiResponse<TransacaoResponse>> UpdateAsync(int id, UpdateTransacaoRequest request);
        Task<ApiResponse<TransacaoByIdResponse>> GetByIdAsync(int id);
        Task<ApiResponse<PaginatedResult<TransacaoResponse>>> GetAllAsync(GetTransacoesRequest request);
        Task<ApiResponse<bool?>> DeleteAsync(int id);
        Task<ApiResponse<int?>> GetTransacoesCountAsync();
        Task<ApiResponse<PaginatedResult<TransacaoResponse>>> GetAllByPessoaIdAsync(int pessoaId, int page, int limit, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
        Task<ApiResponse<PaginatedResult<TransacaoResponse>>> GetAllByCategoriaIdAsync(int categoriaId, int page, int limit, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
    }
}
