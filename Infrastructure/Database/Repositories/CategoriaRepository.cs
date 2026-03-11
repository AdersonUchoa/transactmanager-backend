using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly TransactManagerContext _context;
        private readonly DbSet<Categoria> _categorias;

        public CategoriaRepository(TransactManagerContext context)
        {
            _context = context;
            _categorias = _context.Set<Categoria>();
        }

        public async Task<Categoria> AddAsync(Categoria categoria)
        {
            _categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<Categoria> UpdateAsync(Categoria categoria)
        {
            _categorias.Update(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var categoria = await _categorias.FindAsync(id);
            if (categoria == null) return false;
            _categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _categorias
                .Include(p => p.Transacoes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Categoria>> GetAllAsync(string? search = null)
        {
            var query = _categorias.AsQueryable().AsNoTracking();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Descricao.Contains(search));
            }
            return await query
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task<int> GetCategoriasCountAsync()
        {
            return await _categorias.CountAsync();
        }
    }
}
