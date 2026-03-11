using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> AddAsync(Categoria categoria);
        Task<Categoria> UpdateAsync(Categoria categoria);
        Task<bool> DeleteAsync(int id);
        Task<Categoria?> GetByIdAsync(int id);
        Task<List<Categoria>> GetAllAsync(string? search = null);
        Task<int> GetCategoriasCountAsync();
    }
}
