namespace Domain.Entities;

public partial class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public int Idade { get; set; }
    public virtual ICollection<Transacao> Transacoes { get; set; } = [];

    public Pessoa() { }

    public Pessoa(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
    }

    public void Update(string? newName, int? newAge)
    {
        Nome = newName ?? Nome;
        Idade = newAge ?? Idade;
    }
}
