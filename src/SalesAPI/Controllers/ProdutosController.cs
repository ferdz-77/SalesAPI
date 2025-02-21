using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly SalesDbContext _context;

    public ProdutosController(SalesDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
    {
        return await _context.Produtos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetProduto(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return NotFound();

        return produto;
    }

    [HttpPost]
    public async Task<ActionResult<Produto>> CreateProduto(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduto), new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduto(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
            return BadRequest();

        _context.Entry(produto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return NotFound();

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

