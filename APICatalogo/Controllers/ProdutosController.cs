﻿using APICatalogo.Context;
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
        public async Task<ActionResult <IEnumerable<ProdutoDTO>>> GetProdutosCategoria(int id)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorCategoriaAsync(id);

            if(produtos is null)
                return NotFound ();

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters parameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosAsync(parameters); 

            var metadata = new
            {
                produtos.Count,
                produtos.PageSize,
                produtos.PageCount,
                produtos.TotalItemCount,
                produtos.HasNextPage,
                produtos.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        [HttpGet("Filter/preco/pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco prodututosFilterParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosFiltroPrecoAsync(prodututosFilterParameters);
            var metadata = new
            {
                produtos.Count,
                produtos.PageSize,
                produtos.PageCount,
                produtos.TotalItemCount,
                produtos.HasNextPage,
                produtos.HasPreviousPage
            };
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get() // Posso retornar um enumerabel ou ActionResult
        {
            var produtos = await _uof.ProdutoRepository.GetAllAsync();
            if (produtos is null)
                return NotFound();

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
           var produto = await _uof.ProdutoRepository.GetByIdAsync(c => c.CategoriaId == id);
            if(produto is null)
                return NotFound("Produto não encontrado");

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDto);
        }
        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);
            var novoProduto = _uof.ProdutoRepository.Create(produto);
            await _uof.CommitAsync();
            var novoProdutoDto = _mapper.Map<ProdutoDTO>(produto);
            return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
            
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDto)
        {
            if(id != produtoDto.ProdutoId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);
            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            await _uof.CommitAsync();
            var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);
            return Ok(produtoAtualizado);


        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete (int id)
        {
            var produto = await _uof.ProdutoRepository.GetByIdAsync(p=> p.ProdutoId == id); // Usamos Await aqui pois estamos lidadno diretametne com o banco de dados
            if (produto is null)
                return NotFound();

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            await _uof.CommitAsync();
            var produtoDeletedoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);
            return Ok(produtoDeletado);
        }
    }
}
