using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
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

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
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
            var categoriaDto = categoria.ToCategoriaDTO();
                return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
        {
            
                if (categoriaDTO is  null)
                {
                    _logger.LogWarning($"Dados Inválidos...");
                    return BadRequest($"Dados Inválidos");
                }
            var categoria = categoriaDTO.ToCategoria();
          
                var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
                _uof.Commit();
            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();
            return new CreatedAtRouteResult("ObterProduto", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);

        }
        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning($"Dados Inválidos...");
                return BadRequest($"Dados Inválidos...");
            }
            var categoria = categoriaDTO.ToCategoria();

            var categoriaAtualizada =_uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            var categoriaAtualizadaDTO = categoriaAtualizada.ToCategoriaDTO();
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
            var categoriaExcluidaDTO = categoria.ToCategoriaDTO();
            return Ok(categoriaExcluidaDTO);
        }
    }
}
