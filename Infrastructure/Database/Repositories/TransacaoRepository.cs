using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly TransactManagerContext _context;
        private readonly DbSet<Transacao> _transacao;

        public TransacaoRepository(TransactManagerContext context)
        {
            _context = context;
            _transacao = _context.Set<Transacao>();
        }

        public async Task<Transacao> AddAsync(Transacao transacao)
        {
            _transacao.Add(transacao);
            await _context.SaveChangesAsync();
            return transacao;
        }

        public async Task<Transacao> UpdateAsync(Transacao transacao)
        {
            _transacao.Update(transacao);
            await _context.SaveChangesAsync();
            return transacao;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transacao = await _transacao.FindAsync(id);
            if (transacao == null) return false;
            _transacao.Remove(transacao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Transacao?> GetByIdAsync(int id)
        {
            return await _transacao
                .Include(p => p.Pessoa)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Transacao> GetAllAsync(int? pessoaId = null, int? categoriaId = null, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null)
        {
            var query = _transacao.AsQueryable().AsNoTracking();

            if (pessoaId.HasValue)
                query = query.Where(p => p.PessoaId == pessoaId.Value);

            if (categoriaId.HasValue)
                query = query.Where(p => p.CategoriaId == categoriaId.Value);

            if (valor.HasValue)
                query = query.Where(p => p.Valor == valor.Value);

            if (tipo.HasValue)
                query = query.Where(p => p.Tipo == tipo.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Descricao.Contains(search));

            return query.OrderByDescending(p => p.Id);
        }

        public async Task<int> GetTransacoesCountAsync()
        {
            return await _transacao.CountAsync();
        }
    }
}
