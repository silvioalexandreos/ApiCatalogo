using APICatalago.Context;
using APICatalago.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalago.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                return _context.Produtos.AsNoTracking().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Erro ao tentar obter produtos.");
            }
            
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> GetId(int id)
        {
            try
            {
                var produto = _context.Produtos.AsNoTracking()
                .FirstOrDefault(x => x.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound($"Produto com id : { id } não foi encontrado.");
                }
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Erro ao tentar obter produto.");
            }
            
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            try
            {
                _context.Produtos.Add(produto);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                     "Erro ao tentar cadastrar novo produto.");
            }

            
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest($"Não foi possível atualizar a produto com id = {id}.");
                }
                _context.Produtos.Update(produto);
                _context.SaveChanges();

                return Ok($"Produto com id = {id} atualizada com sucesso.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Erro ao tentar atualizar produto.");
            }
            
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound();
                }

                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                        "Erro ao tentar remover produto.");
            }
        }
    }
}
