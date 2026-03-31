using Domain.Entities;
using Domain.Enums;
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
                .AsTracking()
                .Include(p => p.Transacoes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Categoria?> GetByIdNoTrackingAsync(int id)
        {
            return await _categorias
                .AsNoTracking()
                .Include(p => p.Transacoes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Categoria> GetAllAsync(CategoriaFinalidadeEnum? finalidade = null, string? search = null)
        {
            var query = _categorias.AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => EF.Functions.ILike(p.Descricao, $"%{search}%")); //Melhor tratado para PostgreSQL, que é o banco de dados utilizado. Com MySQL ou SQL Server, pode usar Contains normalmente.

            if (finalidade.HasValue)
                query = query.Where(p => p.Finalidade == finalidade.Value);

            return query.OrderByDescending(p => p.Id);
        }

        public async Task<int> GetCategoriasCountAsync()
        {
            return await _categorias.CountAsync();
        }
    }
}
