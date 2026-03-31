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
using Domain.Interfaces.SeedWorks;
using System.Net;

namespace Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PessoaService(IPessoaRepository pessoaRepository, ITransacaoRepository transacaoRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _pessoaRepository = pessoaRepository;
            _transacaoRepository = transacaoRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PessoaResponse>> AddAsync(CreatePessoaRequest request)
        {
            try
            {
                var pessoa = _mapper.Map<Pessoa>(request);

                var created = await _pessoaRepository.AddAsync(pessoa);

                await _unitOfWork.SaveChangesAsync();

                var response = new PessoaResponse
                {
                    Id = created.Id,
                    Nome = created.Nome,
                    Idade = created.Idade
                };

                return new ApiResponse<PessoaResponse>(true, HttpStatusCode.Created, response, "Pessoa criada com sucesso.", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PessoaResponse>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", ex.Message);
            }
        }

        public async Task<ApiResponse<PessoaResponse>> UpdateAsync(int id, UpdatePessoaRequest request)
        {
            try
            {
                var pessoa = await _pessoaRepository.GetByIdAsync(id);
                if (pessoa == null) return new ApiResponse<PessoaResponse>(false, HttpStatusCode.NotFound, null, "Pessoa não encontrada.", null);

                pessoa.Update(request.Nome, request.Idade);

                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<PessoaResponse>(pessoa);

                return new ApiResponse<PessoaResponse>(true, HttpStatusCode.OK, response, "Pessoa atualizada com sucesso.", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PessoaResponse>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", ex.Message);
            }
        }

        public async Task<ApiResponse<PessoaByIdResponse>> GetByIdAsync(int id)
        {
            try
            {
                var pessoa = await _pessoaRepository.GetByIdNoTrackingAsync(id);
                if (pessoa == null) return new ApiResponse<PessoaByIdResponse>(false, HttpStatusCode.NotFound, null, "Pessoa não encontrada.", null);

                var (receitas, despesas) = await _transacaoRepository.GetTotalsByPessoaIdAsync(pessoa.Id);

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
                    })],
                    TotalReceitas = receitas,
                    TotalDespesas = despesas,
                    Saldo = receitas - despesas
                };

                return new ApiResponse<PessoaByIdResponse>(true, HttpStatusCode.OK, response, "Pessoa recuperada com sucesso.", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PessoaByIdResponse>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", ex.Message);
            }
        }

        public async Task<ApiResponse<PaginatedResult<PessoaResponse>>> GetAllAsync(GetPessoasRequest request)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(request.Search))
                    request.Search = request.Search.Trim();

                var pessoas = _pessoaRepository.GetAllAsync(request.Search);

                var paginated = await PaginatedResult<Pessoa>.CreateAsync(pessoas, request.Page, request.Limit);

                var ids = paginated.Items.Select(p => p.Id);
                var totaisDict = await _transacaoRepository.GetTotalsByPessoaIdsAsync(ids);

                var response = paginated.Items.Select(p =>
                {
                    var (receitas, despesas) = totaisDict[p.Id];
                    return new PessoaResponse
                    {
                        Id = p.Id,
                        Nome = p.Nome,
                        Idade = p.Idade,
                        TotalReceitas = receitas,
                        TotalDespesas = despesas,
                        Saldo = receitas - despesas
                    };
                }).ToList();

                var result = new PaginatedResult<PessoaResponse>(response, paginated.TotalCount, paginated.PageIndex, paginated.PageSize);

                return new ApiResponse<PaginatedResult<PessoaResponse>>(true, HttpStatusCode.OK, result, "Pessoas recuperadas com sucesso.", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaginatedResult<PessoaResponse>>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", ex.Message);
            }
        }

        public async Task<ApiResponse<bool?>> DeleteAsync(int id)
        {
            try
            {
                var pessoa = await _pessoaRepository.GetByIdAsync(id);
                if (pessoa == null) return new ApiResponse<bool?>(false, HttpStatusCode.NotFound, false, "Pessoa não encontrada.", null);

                await _pessoaRepository.DeleteAsync(id);

                await _unitOfWork.SaveChangesAsync();

                return new ApiResponse<bool?>(true, HttpStatusCode.OK, null, "Pessoa deletada com sucesso.", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool?>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", ex.Message);
            }
        }

        public async Task<ApiResponse<int?>> GetPessoasCountAsync()
        {
            try
            {
                var count = await _pessoaRepository.GetPessoasCountAsync();

                return new ApiResponse<int?>(true, HttpStatusCode.OK, count, "Contagem de pessoas recuperada com sucesso.", null);
            }
            catch (Exception ex)
            {
                return new ApiResponse<int?>(false, HttpStatusCode.InternalServerError, null, "Erro interno do servidor. Tente novamente mais tarde.", ex.Message);
            }
        }
    }
}
