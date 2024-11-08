using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            if (categorias is null)
                return NotFound("Não existem categorias");

            var categoriasDTO = new List<CategoriaDTO>();
            foreach (var categoria in categorias)
            {
                var categoriaDTO = new CategoriaDTO()
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl,
                };
            }
            return Ok(categoriasDTO);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria is null)
            {
                    _logger.LogWarning($"Categoria com Id = {id} não encontrada");
                    return NotFound($"Categoria com Id = {id} não encontrada");
            }
            var categoriaDTO = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,

            };
                return Ok(categoriaDTO);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
        {
            
                if (categoriaDTO is  null)
                {
                    _logger.LogWarning($"Dados Inválidos...");
                    return BadRequest($"Dados Inválidos");
                }
            var categoria = new Categoria()
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl
            };
          
                var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
                _uof.Commit();
            var novaCategoriaDTO = new CategoriaDTO()
            {
                CategoriaId = categoriaCriada.CategoriaId,
                Nome = categoriaCriada.Nome,
                ImagemUrl = categoriaCriada.ImagemUrl
            };
            return new CreatedAtRouteResult("ObterProduto", new { id = novaCategoriaDTO.CategoriaId }, novaCategoriaDTO);

        }
        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning($"Dados Inválidos...");
                return BadRequest($"Dados Inválidos...");
            }
            var categoria = new Categoria()
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl
            };

            var categoriaAtualizada =_uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            var categoriaAtualizadaDTO = new CategoriaDTO()
            {
                CategoriaId = categoriaAtualizada.CategoriaId,
                Nome = categoriaAtualizada.Nome,
                ImagemUrl = categoriaAtualizada.ImagemUrl
            };
            return Ok(categoriaAtualizadaDTO);
        }
        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id {id} não encontrada");
                return BadRequest($"Categoria com id {id} não encontrada");
            }
            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            var categoriaExcluidaDTO = new CategoriaDTO()
            {
                CategoriaId = categoriaExcluida.CategoriaId,
                Nome = categoriaExcluida.Nome,
                ImagemUrl = categoriaExcluida.ImagemUrl
            };
            return Ok(categoriaExcluidaDTO);
        }
    }
}
