using Application.Responses.Transacao;

namespace Application.Responses.Pessoa
{
    public class PessoaByIdResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public int Idade { get; set; }
        public List<TransacaoResponse> Transacoes { get; set; } = [];
    }
}
