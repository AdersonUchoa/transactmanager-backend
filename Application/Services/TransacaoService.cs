using Application.Interfaces;
using Application.Pagination;
using Application.Requests.Transacao;
using Application.Responses;
using Application.Responses.Categoria;
using Application.Responses.Pessoa;
using Application.Responses.Transacao;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Extensions;
using Domain.Interfaces.Repositories;
using System.Net;

namespace Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public TransacaoService(ITransacaoRepository transacaoRepository, IMapper mapper, ICategoriaRepository categoriaRepository, IPessoaRepository pessoaRepository)
        {
            _transacaoRepository = transacaoRepository;
            _mapper = mapper;
            _categoriaRepository = categoriaRepository;
            _pessoaRepository = pessoaRepository;
        }

        public async Task<ApiResponse<TransacaoResponse>> AddAsync(CreateTransacaoRequest request)
        {
            try
            {
                var validation = await ValidateTransacaoCreation(request);
                if (!validation.Success) return validation;

                var transacao = _mapper.Map<Transacao>(request);

                var created = await _transacaoRepository.AddAsync(transacao);

                var response = new TransacaoResponse
                {
                    Id = created.Id,
                    Descricao = created.Descricao,
                    Valor = created.Valor,
                    Tipo = created.Tipo.Value(),
                    CategoriaId = created.CategoriaId,
                    PessoaId = created.PessoaId
                };

                return new ApiResponse<TransacaoResponse>(true, HttpStatusCode.Created, response, "Transação criada com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TransacaoResponse>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while creating the transaction: {ex.Message}", null, null);
            }
        }

        private async Task<ApiResponse<TransacaoResponse>> ValidateTransacaoCreation(CreateTransacaoRequest request)
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(request.PessoaId);
            if (pessoa is null) return new ApiResponse<TransacaoResponse>(false, HttpStatusCode.NotFound, null, "Pessoa não encontrada", null, null);

            var categoria = await _categoriaRepository.GetByIdAsync(request.CategoriaId);
            if (categoria is null) return new ApiResponse<TransacaoResponse>(false, HttpStatusCode.NotFound, null, "Categoria não encontrada", null, null);

            if (pessoa.Idade < 18 && request.Tipo != TransacoesTipoEnum.Despesa)
                return new ApiResponse<TransacaoResponse>(false, HttpStatusCode.BadRequest, null, "Pessoas menores de 18 anos só podem criar transações do tipo despesa", null, null);

            var categoriaIncompativel = (categoria.Finalidade == CategoriaFinalidadeEnum.Despesa && request.Tipo != TransacoesTipoEnum.Despesa)
                || (categoria.Finalidade == CategoriaFinalidadeEnum.Receita && request.Tipo != TransacoesTipoEnum.Receita);

            if (categoriaIncompativel)
                return new ApiResponse<TransacaoResponse>(false, HttpStatusCode.BadRequest, null, "A categoria informada é incompatível com o tipo da transação", null, null);

            return new ApiResponse<TransacaoResponse>(true, HttpStatusCode.OK, null, "Validação concluída com sucesso", null, null);
        }

        public async Task<ApiResponse<TransacaoResponse>> UpdateAsync(int id, UpdateTransacaoRequest request)
        {
            try
            {
                var transacao = await _transacaoRepository.GetByIdAsync(id);
                if (transacao == null) return new ApiResponse<TransacaoResponse>(false, HttpStatusCode.NotFound, null, "Transação não encontrada", null, null);

                _mapper.Map(request, transacao);
                var updated = await _transacaoRepository.UpdateAsync(transacao);

                var response = new TransacaoResponse
                {
                    Id = updated.Id,
                    Descricao = updated.Descricao,
                    Valor = updated.Valor,
                    Tipo = updated.Tipo.Value(),
                    CategoriaId = updated.CategoriaId,
                    PessoaId = updated.PessoaId
                };

                //TODO: Verify transacao.UpdateFields(), porque o Mapper Profile não lida bem com os nullables do update request

                return new ApiResponse<TransacaoResponse>(true, HttpStatusCode.OK, response, "Transação atualizada com sucesso", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TransacaoResponse>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while updating the transaction: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<TransacaoByIdResponse>> GetByIdAsync(int id)
        {
            try
            {
                var transacao = await _transacaoRepository.GetByIdAsync(id);
                if (transacao == null) return new ApiResponse<TransacaoByIdResponse>(false, HttpStatusCode.NotFound, null, "Transação não encontrada", null, null);

                var response = new TransacaoByIdResponse
                {
                    Id = transacao.Id,
                    Descricao = transacao.Descricao,
                    Valor = transacao.Valor,
                    Tipo = transacao.Tipo.Value(),
                    Categoria = new CategoriaResponse
                    {
                        Id = transacao.Categoria.Id,
                        Descricao = transacao.Categoria.Descricao,
                        Finalidade = transacao.Categoria.Finalidade.Value()
                    },
                    Pessoa = new PessoaResponse
                    {
                        Id = transacao.Pessoa.Id,
                        Nome = transacao.Pessoa.Nome,
                        Idade = transacao.Pessoa.Idade
                    }
                };

                return new ApiResponse<TransacaoByIdResponse>(true, HttpStatusCode.OK, response, "Transação encontrada com sucesso", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TransacaoByIdResponse>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while retrieving the transaction: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var transacao = await _transacaoRepository.GetByIdAsync(id);
                if (transacao is null) return new ApiResponse<bool>(false, HttpStatusCode.NotFound, null, "Transação não encontrada.", null, null);

                await _transacaoRepository.DeleteAsync(id);

                return new ApiResponse<bool>(true, HttpStatusCode.OK, null, "Transação deletada com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while deleting the transaction: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<List<TransacaoResponse>>> GetAllAsync(int page, int limit, int? pessoaId = null, int? categoriaId = null, decimal? valor = null, TransacoesTipoEnum? tipo = null, string? search = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(search))
                    search = search.Trim();

                var transacoes = _transacaoRepository.GetAllAsync(pessoaId, categoriaId, valor, tipo, search);

                var paginated = await PaginatedResult<Transacao>.CreateAsync(transacoes, page, limit);

                var response = transacoes.Select(t => new TransacaoResponse
                {
                    Id = t.Id,
                    Descricao = t.Descricao,
                    Valor = t.Valor,
                    Tipo = t.Tipo.Value(),
                    CategoriaId = t.CategoriaId,
                    PessoaId = t.PessoaId
                }).ToList();

                var result = new PaginatedResult<TransacaoResponse>(response, paginated.TotalCount, paginated.PageIndex, paginated.PageSize);

                return new ApiResponse<List<TransacaoResponse>>(true, HttpStatusCode.OK, result, "Transações encontradas com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TransacaoResponse>>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while retrieving transactions: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<int>> GetTransacoesCountAsync()
        {
            try
            {
                var count = await _transacaoRepository.GetTransacoesCountAsync();

                return new ApiResponse<int>(true, HttpStatusCode.OK, count, "Contagem de transações recuperada com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while counting transactions: {ex.Message}", null, null);
            }
        }
    }
}
