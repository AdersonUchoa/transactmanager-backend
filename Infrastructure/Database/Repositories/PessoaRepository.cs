using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly TransactManagerContext _context;
        private readonly DbSet<Pessoa> _pessoas;

        public PessoaRepository(TransactManagerContext context)
        {
            _context = context;
            _pessoas = _context.Set<Pessoa>();
        }

        public async Task<Pessoa> AddAsync(Pessoa pessoa)
        {
            _pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
            return pessoa;
        }

        public async Task<Pessoa> UpdateAsync(Pessoa pessoa)
        {
            _pessoas.Update(pessoa);
            await _context.SaveChangesAsync();
            return pessoa;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pessoa = await _pessoas.FindAsync(id);
            if (pessoa == null) return false;
            _pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Pessoa?> GetByIdAsync(int id)
        {
            return await _pessoas
                .Include(p => p.Transacoes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Pessoa> GetAllAsync(string? search = null)
        {
            var query = _pessoas.AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Nome.Contains(search));

            return query.OrderByDescending(p => p.Id);

        }

        public async Task<int> GetPessoasCountAsync()
        {
            return await _pessoas.CountAsync();
        }
    }
}
