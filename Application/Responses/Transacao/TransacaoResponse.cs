using Application.Responses.Categoria;
using Application.Responses.Pessoa;

namespace Application.Responses.Transacao
{
    public class TransacaoResponse
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = null!;
        public decimal Valor { get; set; }
        public string Tipo { get; set; } = null!;
        public int CategoriaId { get; set; }
        public int PessoaId { get; set; }
        public CategoriaBasicResponse? Categoria { get; set; }
        public PessoaBasicResponse? Pessoa { get; set; }
    }
}
