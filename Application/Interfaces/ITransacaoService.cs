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
        Task<ApiResponse<List<TransacaoResponse>>> GetAllAsync(int page, int limit, int? pessoaId = null, int? categoriaId = null, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<int>> GetTransacoesCountAsync();
        Task<ApiResponse<List<TransacaoResponse>>> GetAllByPessoaIdAsync(int pessoaId, int page, int limit, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
        Task<ApiResponse<List<TransacaoResponse>>> GetAllByCategoriaIdAsync(int categoriaId, int page, int limit, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
    }
}
