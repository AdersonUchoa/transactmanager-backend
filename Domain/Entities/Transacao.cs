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
}
