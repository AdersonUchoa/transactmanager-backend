using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Pessoa
{
    public class UpdatePessoaRequest
    {
        [StringLength(200, ErrorMessage = "O nome deve conter no máximo 200 caracteres.")]
        public string? Nome { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A idade não pode ser menor que 0.")]
        public int? Idade { get; set; }
    }
}
