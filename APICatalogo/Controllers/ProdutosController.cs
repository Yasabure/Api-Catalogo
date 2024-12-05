using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uof = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("Produtos/{id}")]
        public ActionResult <IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if(produtos is null)
                return NotFound ();

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters parameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutos(parameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet("Filter/preco/pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco prodututosFilterParameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosFiltroPreco(prodututosFilterParameters);
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);

        }
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get() // Posso retornar um enumerabel ou ActionResult
        {
            var produtos = _uof.ProdutoRepository.GetAll();
            if (produtos is null)
                return NotFound();

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
           var produto = _uof.ProdutoRepository.GetById(c => c.CategoriaId == id);
            if(produto is null)
                return NotFound("Produto não encontrado");

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDto);
        }
        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);
            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();
            var novoProdutoDto = _mapper.Map<ProdutoDTO>(produto);
            return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
            
        }
        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
        {
            if(id != produtoDto.ProdutoId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);
            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);
            return Ok(produtoAtualizado);


        }
        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete (int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p=> p.ProdutoId == id);
            if (produto is null)
                return NotFound();

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            var produtoDeletedoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);
            return Ok(produtoDeletado);
        }
    }
}
