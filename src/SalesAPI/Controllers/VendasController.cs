using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.Models;
using SalesAPI.Dtos;
using SalesAPI.Services;


[ApiController]
[Route("api/[controller]")]
public class VendasController : ControllerBase
{
    private readonly SalesDbContext _context;
    private readonly ILogger<VendasController> _logger;
    private readonly SalesService _salesService;


    public VendasController(SalesDbContext context, ILogger<VendasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CriarVenda([FromBody] CriarVendaDto vendaDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (vendaDto.ClienteId <= 0)
            return BadRequest("O ClienteId é obrigatório e deve ser válido.");

        if (vendaDto.FilialId <= 0)
            return BadRequest("A FilialId é obrigatória e deve ser válida.");

        var clienteExiste = await _context.Clientes.AnyAsync(c => c.ClienteId == vendaDto.ClienteId);
        if (!clienteExiste)
            return BadRequest("O cliente informado não existe.");

        var filialExiste = await _context.Filiais.AnyAsync(f => f.FilialId == vendaDto.FilialId);
        if (!filialExiste)
            return BadRequest("A Filial informada não existe.");

        var produtos = await _context.Produtos
            .Where(p => vendaDto.Itens.Select(i => i.ProdutoId).Contains(p.ProdutoId))
            .ToListAsync();

        if (produtos.Count != vendaDto.Itens.Count)
            return NotFound("Um ou mais produtos não foram encontrados.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var vendaItensAtualizados = vendaDto.Itens.Select(item => {
                var produto = produtos.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                if (produto == null)
                    throw new Exception($"Produto não encontrado: ID {item.ProdutoId}.");

                if (produto.QuantidadeEstoque < item.Quantidade)
                    throw new Exception($"Estoque insuficiente para {produto.Nome}. Disponível: {produto.QuantidadeEstoque}, Requerido: {item.Quantidade}");

                var desconto = item.Quantidade >= 10 ? 0.20m : item.Quantidade >= 4 ? 0.10m : 0;
                var precoComDesconto = item.Preco * (1 - desconto);

                return new VendaItemDto(item.ProdutoId, item.Quantidade, precoComDesconto, item.Preco, desconto);
            }).ToList();

            var venda = new Venda
            {
                ClienteId = vendaDto.ClienteId,
                DataVenda = DateTime.UtcNow
            };

            // Adiciona os itens usando um método apropriado
            foreach (var item in vendaItensAtualizados)
            {
                _salesService.AdicionarItem(venda, new VendaItem
                {
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    Preco = item.Preco,
                    PrecoOriginal = item.PrecoOriginal,
                    Desconto = item.Desconto
                });
            }

            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return CreatedAtAction(nameof(GetVendaById), new { id = venda.Id }, venda);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar a venda para o cliente {ClienteId}.", vendaDto.ClienteId);
            await transaction.RollbackAsync();
            return StatusCode(500, "Erro interno ao processar a venda.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVendaById(int id)
    {
        var venda = await _context.Vendas
            .Include(v => v.VendaItems)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venda == null)
            return NotFound("Venda não encontrada.");

        return Ok(venda);
    }


}
