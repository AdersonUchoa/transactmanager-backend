namespace Application.Responses.Categoria
{
    public class CategoriaResponse
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = null!;
        public string Finalidade { get; set; } = null!;
        public decimal? TotalReceitas { get; set; }
        public decimal? TotalDespesas { get; set; }
        public decimal? Saldo { get; set; }
    }
}
