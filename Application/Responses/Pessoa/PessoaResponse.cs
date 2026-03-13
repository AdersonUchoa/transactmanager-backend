namespace Application.Responses.Pessoa
{
    public class PessoaResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public int Idade { get; set; }
        public decimal? TotalReceitas { get; set; }
        public decimal? TotalDespesas { get; set; }
        public decimal? Saldo { get; set; }
    }
}
