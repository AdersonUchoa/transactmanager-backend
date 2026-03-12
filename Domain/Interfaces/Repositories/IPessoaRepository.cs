using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IPessoaRepository
    {
        Task<Pessoa> AddAsync(Pessoa pessoa);
        Task<Pessoa> UpdateAsync(Pessoa pessoa);
        Task<bool> DeleteAsync(int id);
        Task<Pessoa?> GetByIdAsync(int id);
        IQueryable<Pessoa> GetAllAsync(string? search = null);
        Task<int> GetPessoasCountAsync();
    }
}
