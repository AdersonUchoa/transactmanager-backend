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

        public IQueryable<Transacao> GetByPessoaIdAsync(int pessoaId, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null)
        {
            var query = _transacao.AsQueryable().Where(p => p.PessoaId == pessoaId).AsNoTracking();

            if (valor.HasValue)
                query = query.Where(p => p.Valor == valor.Value);

            if (tipo.HasValue)
                query = query.Where(p => p.Tipo == tipo.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Descricao.Contains(search));

            return query.OrderByDescending(p => p.Id);
        }

        public IQueryable<Transacao> GetByCategoriaIdAsync(int categoriaId, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null)
        {
            var query = _transacao.AsQueryable().Where(p => p.CategoriaId == categoriaId).AsNoTracking();

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

        public async Task<(decimal Receitas, decimal Despesas)> GetTotalsByPessoaIdAsync(int pessoaId)
        {
            var receitas = await _transacao
                .Where(t => t.PessoaId == pessoaId && t.Tipo == TransacoesTipoEnum.Receita)
                .SumAsync(t => t.Valor);

            var despesas = await _transacao
                .Where(t => t.PessoaId == pessoaId && t.Tipo == TransacoesTipoEnum.Despesa)
                .SumAsync(t => t.Valor);

            return (receitas, despesas);
        }

        public async Task<(decimal Receitas, decimal Despesas)> GetTotalsByCategoriaIdAsync(int categoriaId)
        {
            var receitas = await _transacao
                .Where(t => t.CategoriaId == categoriaId && t.Tipo == TransacoesTipoEnum.Receita)
                .SumAsync(t => t.Valor);

            var despesas = await _transacao
                .Where(t => t.CategoriaId == categoriaId && t.Tipo == TransacoesTipoEnum.Despesa)
                .SumAsync(t => t.Valor);

            return (receitas, despesas);
        }

        public async Task<Dictionary<int, (decimal Receitas, decimal Despesas)>> GetTotalsByCategoriaIdsAsync(IEnumerable<int> categoriaIds)
        {
            var resultado = await _transacao
                .Where(t => categoriaIds.Contains(t.CategoriaId))
                .GroupBy(t => new { t.CategoriaId, t.Tipo })
                .Select(g => new
                {
                    g.Key.CategoriaId,
                    g.Key.Tipo,
                    Total = g.Sum(t => t.Valor)
                })
                .ToListAsync();

            return categoriaIds.ToDictionary(
                id => id,
                id => (
                    Receitas: resultado.FirstOrDefault(r => r.CategoriaId == id && r.Tipo == TransacoesTipoEnum.Receita)?.Total ?? 0,
                    Despesas: resultado.FirstOrDefault(r => r.CategoriaId == id && r.Tipo == TransacoesTipoEnum.Despesa)?.Total ?? 0
                )
            );
        }

        public async Task<Dictionary<int, (decimal Receitas, decimal Despesas)>> GetTotalsByPessoaIdsAsync(IEnumerable<int> pessoaIds)
        {
            var resultado = await _transacao
                .Where(t => pessoaIds.Contains(t.PessoaId))
                .GroupBy(t => new { t.PessoaId, t.Tipo })
                .Select(g => new
                {
                    g.Key.PessoaId,
                    g.Key.Tipo,
                    Total = g.Sum(t => t.Valor)
                })
                .ToListAsync();

            return pessoaIds.ToDictionary(
                id => id,
                id => (
                    Receitas: resultado.FirstOrDefault(r => r.PessoaId == id && r.Tipo == TransacoesTipoEnum.Receita)?.Total ?? 0,
                    Despesas: resultado.FirstOrDefault(r => r.PessoaId == id && r.Tipo == TransacoesTipoEnum.Despesa)?.Total ?? 0
                )
            );
        }
    }
}
