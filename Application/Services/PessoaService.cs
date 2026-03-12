using Application.Interfaces;
using Application.Pagination;
using Application.Requests.Pessoa;
using Application.Responses;
using Application.Responses.Pessoa;
using Application.Responses.Transacao;
using AutoMapper;
using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces.Repositories;
using System.Net;

namespace Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IMapper _mapper;

        public PessoaService(IPessoaRepository pessoaRepository, IMapper mapper)
        {
            _pessoaRepository = pessoaRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PessoaResponse>> AddAsync(CreatePessoaRequest request)
        {
            try
            {
                var pessoa = _mapper.Map<Pessoa>(request);

                var created = await _pessoaRepository.AddAsync(pessoa);

                var response = new PessoaResponse
                {
                    Id = created.Id,
                    Nome = created.Nome,
                    Idade = created.Idade
                };

                return new ApiResponse<PessoaResponse>(true, HttpStatusCode.Created, response, "Pessoa criada com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PessoaResponse>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while creating the person: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<PessoaResponse>> UpdateAsync(int id, UpdatePessoaRequest request)
        {
            try
            {
                var existing = await _pessoaRepository.GetByIdAsync(id);
                if (existing == null) return new ApiResponse<PessoaResponse>(false, HttpStatusCode.NotFound, null, "Pessoa não encontrada.", null, null);

                _mapper.Map(request, existing);
                var updated = await _pessoaRepository.UpdateAsync(existing);

                var response = new PessoaResponse
                {
                    Id = updated.Id,
                    Nome = updated.Nome,
                    Idade = updated.Idade
                };

                //TODO: Verify pessoa.UpdateFields(), porque o Mapper Profile não lida bem com os nullables do update request
                return new ApiResponse<PessoaResponse>(true, HttpStatusCode.OK, response, "Pessoa atualizada com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PessoaResponse>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while updating the person: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<PessoaByIdResponse>> GetByIdAsync(int id)
        {
            try
            {
                var pessoa = await _pessoaRepository.GetByIdAsync(id);
                if (pessoa == null) return new ApiResponse<PessoaByIdResponse>(false, HttpStatusCode.NotFound, null, "Pessoa não encontrada.", null, null);

                var response = new PessoaByIdResponse
                {
                    Id = pessoa.Id,
                    Nome = pessoa.Nome,
                    Idade = pessoa.Idade,
                    Transacoes = [.. pessoa.Transacoes.Select(t => new TransacaoResponse
                    {
                        Id = t.Id,
                        Descricao = t.Descricao,
                        Valor = t.Valor,
                        Tipo = t.Tipo.Value(),
                        CategoriaId = t.CategoriaId,
                        PessoaId = t.PessoaId
                    })]
                };

                return new ApiResponse<PessoaByIdResponse>(true, HttpStatusCode.OK, response, "Pessoa recuperada com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PessoaByIdResponse>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while retrieving the person: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<List<PessoaResponse>>> GetAllAsync(int page, int limit, string? search = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(search))
                    search = search.Trim();

                var pessoas = _pessoaRepository.GetAllAsync(search);

                var paginated = await PaginatedResult<Pessoa>.CreateAsync(pessoas, page, limit);

                var response = pessoas.Select(p => new PessoaResponse
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Idade = p.Idade
                }).ToList();

                var result = new PaginatedResult<PessoaResponse>(response, paginated.TotalCount, paginated.PageIndex, paginated.PageSize);

                return new ApiResponse<List<PessoaResponse>>(true, HttpStatusCode.OK, result, "Pessoas recuperadas com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PessoaResponse>>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while retrieving people: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var pessoa = await _pessoaRepository.GetByIdAsync(id);
                if (pessoa == null) return new ApiResponse<bool>(false, HttpStatusCode.NotFound, false, "Pessoa não encontrada.", null, null);

                await _pessoaRepository.DeleteAsync(id);

                return new ApiResponse<bool>(true, HttpStatusCode.OK, null, "Pessoa deletada com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while deleting the person: {ex.Message}", null, null);
            }
        }

        public async Task<ApiResponse<int>> GetPessoasCountAsync()
        {
            try
            {
                var count = await _pessoaRepository.GetPessoasCountAsync();

                return new ApiResponse<int>(true, HttpStatusCode.OK, count, "Contagem de pessoas recuperada com sucesso.", null, null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>(false, HttpStatusCode.InternalServerError, null, $"An error occurred while counting people: {ex.Message}", null, null);
            }
        }
    }
}
