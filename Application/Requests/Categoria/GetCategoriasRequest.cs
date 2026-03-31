using Domain.Enums;

namespace Application.Requests.Categoria
{
    public class GetCategoriasRequest
    {
            public int Page { get; set; } = 1;
            public int Limit { get; set; } = 10;
            public string? Search { get; set; }
            public CategoriaFinalidadeEnum? Finalidade { get; set; }

    }
}
