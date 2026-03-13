using Domain.Enums;

namespace Domain.Entities;

public partial class Transacao
{
    public int Id { get; set; }
    public string Descricao { get; set; } = null!;
    public decimal Valor { get; set; }
    public TransacoesTipoEnum Tipo { get; set; }
    public int CategoriaId { get; set; }
    public int PessoaId { get; set; }
    public virtual Categoria Categoria { get; set; } = null!;
    public virtual Pessoa Pessoa { get; set; } = null!;

    public Transacao() { }

    public Transacao(string descricao, decimal valor, TransacoesTipoEnum tipo, int categoriaId, int pessoaId)
    {
        Descricao = descricao;
        Valor = valor;
        Tipo = tipo;
        CategoriaId = categoriaId;
        PessoaId = pessoaId;
    }

    public void Update(string? newDescricao, decimal? newValor, TransacoesTipoEnum? newTipo, int? newCategoriaId, int? newPessoaId)
    {
        Descricao = newDescricao ?? Descricao;
        Valor = newValor ?? Valor;
        Tipo = newTipo ?? Tipo;
        CategoriaId = newCategoriaId ?? CategoriaId;
        PessoaId = newPessoaId ?? PessoaId;
    }
}
