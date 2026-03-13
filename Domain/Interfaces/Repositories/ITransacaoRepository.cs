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
        Task<(decimal Receitas, decimal Despesas)> GetTotalsByPessoaIdAsync(int pessoaId);
        Task<(decimal Receitas, decimal Despesas)> GetTotalsByCategoriaIdAsync(int categoriaId);
        Task<Dictionary<int, (decimal Receitas, decimal Despesas)>> GetTotalsByCategoriaIdsAsync(IEnumerable<int> categoriaIds);
        Task<Dictionary<int, (decimal Receitas, decimal Despesas)>> GetTotalsByPessoaIdsAsync(IEnumerable<int> pessoaIds);
        IQueryable<Transacao> GetByPessoaIdAsync(int pessoaId, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
        IQueryable<Transacao> GetByCategoriaIdAsync(int categoriaId, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null);
    }
}
