using Domain.Enums;

namespace Domain.Entities;

public partial class Categoria
{
    public int Id { get; set; }

    public string Descricao { get; set; } = null!;

    public CategoriaFinalidadeEnum Finalidade { get; set; }

    public virtual ICollection<Transacao> Transacoes { get; set; } = [];
}
