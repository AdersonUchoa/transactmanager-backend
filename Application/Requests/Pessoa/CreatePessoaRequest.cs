using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Pessoa
{
    public class CreatePessoaRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome deve conter no máximo 200 caracteres.")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "A idade é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A idade não pode ser menor que 0.")]
        public int Idade { get; set; }
    }
}
