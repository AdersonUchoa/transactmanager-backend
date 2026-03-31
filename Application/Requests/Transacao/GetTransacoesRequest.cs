using Domain.Enums;

namespace Application.Requests.Transacao
{
    public class GetTransacoesRequest
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public int? PessoaId { get; set; }
        public int? CategoriaId { get; set; }
        public decimal? Valor { get; set; }
        public TransacoesTipoEnum? Tipo { get; set; }
        public string? Search { get; set; }
    }
}
