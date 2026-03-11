using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ITransacaoRepository
    {
        Task<Transacao> AddAsync(Transacao transacao);
        Task<Transacao> UpdateAsync(Transacao transacao);
        Task<bool> DeleteAsync(int id);
        Task<Transacao?> GetByIdAsync(int id);
        Task<List<Transacao>> GetAllAsync(string? search = null);
        Task<int> GetTransacoesCountAsync();
    }
}
