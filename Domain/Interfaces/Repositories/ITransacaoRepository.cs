using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.Repositories
{
    public interface ITransacaoRepository
    {
        Task<Transacao> AddAsync(Transacao transacao);
        Task<Transacao> UpdateAsync(Transacao transacao);
        Task<bool> DeleteAsync(int id);
        Task<Transacao?> GetByIdAsync(int id);
        IQueryable<Transacao> GetAllAsync(int? pessoaId = null, int? categoriaId = null, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
        Task<int> GetTransacoesCountAsync();
        IQueryable<Transacao> GetByPessoaIdAsync(int pessoaId, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
        IQueryable<Transacao> GetByCategoriaIdAsync(int categoriaId, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
    }
}
