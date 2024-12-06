using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }
        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasparameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasparameters);

            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }
        [HttpGet("Filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFilterNome([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasNomeAsync(categoriasFiltroNome);
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var categoriasDTO = categorias.ToCategoriaDTOList();


            return Ok(categoriasDTO);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync(); // Necessário utilizar o await, pois sem ele tratamos diretamente com a task e não com o resultado

            if (categorias is null)
                return NotFound("Não existem categorias");

            var categoriasDto = categorias.ToCategoriaDTOList(); // Impossibilitando essa operação por falta de informação

            return Ok(categoriasDto);
        }



        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetByIdAsync(c => c.CategoriaId == id);
            if (categoria is null)
            {
                    _logger.LogWarning($"Categoria com Id = {id} não encontrada");
                    return NotFound($"Categoria com Id = {id} não encontrada");
            }
            var categoriaDto = categoria.ToCategoriaDTO();
                return Ok(categoriaDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
        {
            
                if (categoriaDTO is  null)
                {
                    _logger.LogWarning($"Dados Inválidos...");
                    return BadRequest($"Dados Inválidos");
                }
            var categoria = categoriaDTO.ToCategoria();
          
                var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
                await _uof.CommitAsync();
            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();
            return new CreatedAtRouteResult("ObterProduto", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);

        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning($"Dados Inválidos...");
                return BadRequest($"Dados Inválidos...");
            }
            var categoria = categoriaDTO.ToCategoria();

            var categoriaAtualizada =_uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();
            var categoriaAtualizadaDTO = categoriaAtualizada.ToCategoriaDTO();
            return Ok(categoriaAtualizadaDTO);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetByIdAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id {id} não encontrada");
                return BadRequest($"Categoria com id {id} não encontrada");
            }
            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();
            var categoriaExcluidaDTO = categoria.ToCategoriaDTO();
            return Ok(categoriaExcluidaDTO);
        }
    }
}
