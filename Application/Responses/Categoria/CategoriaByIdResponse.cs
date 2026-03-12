using Application.Responses.Transacao;

namespace Application.Responses.Categoria
{
    public class CategoriaByIdResponse
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = null!;
        public string Finalidade { get; set; } = null!;
        public List<TransacaoResponse> Transacoes { get; set; } = [];
    }
}
