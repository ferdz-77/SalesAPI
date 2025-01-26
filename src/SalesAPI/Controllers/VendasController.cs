using System.Globalization; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class VendasController : ControllerBase
{
    private readonly SalesDbContext _context;
    private readonly ILogger<VendasController> _logger;

    public VendasController(SalesDbContext context, ILogger<VendasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CriarVenda([FromBody] CriarVendaDto vendaDto)
    {
        // 1. Validar os dados recebidos.
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 2. Verificar se os produtos existem e se há estoque suficiente.
            var produtos = await _context.Produtos
                .Where(p => vendaDto.Itens.Select(i => i.ProdutoId).Contains(p.ProdutoId))
                .ToListAsync();

            if (produtos.Count != vendaDto.Itens.Count)
                return NotFound("Um ou mais produtos não foram encontrados.");

            foreach (var item in vendaDto.Itens)
            {
                var produto = produtos.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                if (produto == null)
                {
                    _logger.LogWarning("Produto ID {ProdutoId} não encontrado.", item.ProdutoId);
                    return BadRequest($"Produto não encontrado: ID {item.ProdutoId}.");
                }

                // Verificar estoque suficiente
                if (produto.QuantidadeEstoque < item.Quantidade)
                {
                    _logger.LogWarning("Estoque insuficiente para o produto {Produto}. Requerido: {Requerido}, Disponível: {Disponivel}.",
                        produto.Nome, item.Quantidade, produto.QuantidadeEstoque);
                    return BadRequest(new
                    {
                        Mensagem = $"Estoque insuficiente para o produto: {produto.Nome}.",
                        Disponivel = produto.QuantidadeEstoque,
                        Requerido = item.Quantidade
                    });
                }

                // Regras de desconto:
                if (item.Quantidade < 4)
                {
                    item.PrecoOriginal = item.Preco;
                    item.Desconto = 0;
                }
                else if (item.Quantidade >= 4 && item.Quantidade < 10)
                {
                    item.Desconto = 0.10m;
                    item.PrecoOriginal = item.Preco;
                    item.Preco = item.Preco * (1 - item.Desconto);
                }
                else if (item.Quantidade >= 10 && item.Quantidade <= 20)
                {
                    item.Desconto = 0.20m;
                    item.PrecoOriginal = item.Preco;
                    item.Preco = item.Preco * (1 - item.Desconto);
                }
                else if (item.Quantidade > 20)
                {
                    return BadRequest($"Não é possível vender mais de 20 unidades do produto {produto.Nome}.");
                }
            }

            // 3. Criar a venda sem salvar os itens
            var venda = new Venda
            {
                ClienteId = vendaDto.ClienteId,
                DataVenda = DateTime.UtcNow,
                Total = vendaDto.Itens.Sum(i => i.Quantidade * i.Preco),
                VendaItems = new List<VendaItem>() // Deixe os itens vazios
            };

            _context.Vendas.Add(venda); // Salva apenas a venda
            await _context.SaveChangesAsync(); // Salva a venda

            // 4. Adicionar os itens à tabela VendaItems
            foreach (var itemDto in vendaDto.Itens)
            {
                var novoItem = new VendaItem
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    Preco = itemDto.Preco,
                    PrecoOriginal = itemDto.PrecoOriginal,
                    Desconto = itemDto.Desconto,
                    VendaId = venda.Id // Associe à nova venda
                };
            // Exibe o ID da venda para validação antes de salvar
            Console.WriteLine($"VendaId enviada: {venda.Id}");

            // Adicione o novo item à lista de itens da venda
            _context.VendaItens.Add(novoItem);
            }

            await _context.SaveChangesAsync(); // Salva os itens na tabela VendaItems

            await transaction.CommitAsync();

            // 5. Retornar o resultado
            return CreatedAtAction(nameof(GetVendaById), new { id = venda.Id }, new
            {
                venda.Id,
                venda.DataVenda,
                venda.Total
            });
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
            .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venda == null)
            return NotFound();

        return Ok(new
        {
            venda.Id,
            venda.DataVenda,
            venda.Total,
            Itens = venda.VendaItems.Select(i => new
            {
                i.ProdutoId,
                i.Produto.Nome,
                i.Quantidade,
                i.Preco
            })
        });
    }

    [HttpGet]
   public async Task<IActionResult> GetVendas([FromQuery] string dataInicial, [FromQuery] string dataFinal, [FromQuery] int? clienteId)
{
    // Variáveis para armazenar as datas convertidas
    DateTime? dataInicialParsed = null, dataFinalParsed = null;

    // Validar e converter 'dataInicial' se fornecida
    if (!string.IsNullOrWhiteSpace(dataInicial))
    {
        if (!DateTime.TryParseExact(dataInicial, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out var parsedInicial))
        {
            return BadRequest("O parâmetro 'dataInicial' deve estar no formato 'dd/MM/yyyy'.");
        }
        dataInicialParsed = parsedInicial.ToUniversalTime(); // Convertendo para UTC
    }

    // Validar e converter 'dataFinal' se fornecida
    if (!string.IsNullOrWhiteSpace(dataFinal))
    {
        if (!DateTime.TryParseExact(dataFinal, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out var parsedFinal))
        {
            return BadRequest("O parâmetro 'dataFinal' deve estar no formato 'dd/MM/yyyy'.");
        }
        dataFinalParsed = parsedFinal.ToUniversalTime(); // Convertendo para UTC
    }

    // Construir a consulta com os filtros opcionais
    var query = _context.Vendas.AsQueryable();

    if (dataInicialParsed.HasValue)
        query = query.Where(v => v.DataVenda >= dataInicialParsed.Value);

    if (dataFinalParsed.HasValue)
        query = query.Where(v => v.DataVenda <= dataFinalParsed.Value);

    if (clienteId.HasValue)
        query = query.Where(v => v.ClienteId == clienteId.Value);

    // Consultar e incluir os relacionamentos necessários
    var vendas = await query
        .Include(v => v.VendaItems)
        .ThenInclude(i => i.Produto)
        .ToListAsync();

    // Retornar o resultado no formato desejado
    return Ok(vendas.Select(v => new
    {
        v.Id,
        v.DataVenda,
        v.Total,
        Itens = v.VendaItems.Select(i => new
        {
            i.ProdutoId,
            i.Produto.Nome,
            i.Quantidade,
            i.Preco
        })
    }));
}

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarVenda(int id, [FromBody] AtualizarVendaDto vendaDto)
    {
        // 1. Validar os dados recebidos
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // 2. Buscar a venda existente
        var venda = await _context.Vendas
            .Include(v => v.VendaItems)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venda == null)
            return NotFound("Venda não encontrada.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 3. Atualizar cliente, se necessário
            if (venda.ClienteId != vendaDto.ClienteId)
            {
                venda.ClienteId = vendaDto.ClienteId;
            }

            // 4. Atualizar os itens da venda
            foreach (var item in vendaDto.Itens)
            {
                var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                if (produto == null)
                {
                    return BadRequest($"Produto {item.ProdutoId} não encontrado.");
                }

                var vendaItem = venda.VendaItems.FirstOrDefault(i => i.ProdutoId == item.ProdutoId);
                if (vendaItem != null)
                {
                    // Remover a quantidade antiga e atualizar estoque
                    produto.QuantidadeEstoque += vendaItem.Quantidade;
                    vendaItem.Quantidade = item.Quantidade;
                    produto.QuantidadeEstoque -= item.Quantidade;
                }
                else
                {
                    // Adicionar novo item
                    venda.VendaItems.Add(new VendaItem
                    {
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade,
                        Preco = item.Preco
                    });
                    produto.QuantidadeEstoque -= item.Quantidade;
                }
            }

            // 5. Recalcular o total da venda
            venda.Total = venda.VendaItems.Sum(i => i.Quantidade * i.Preco);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new
            {
                venda.Id,
                venda.DataVenda,
                venda.Total,
                Itens = venda.VendaItems.Select(i => new
                {
                    i.ProdutoId,
                    i.Produto.Nome,
                    i.Quantidade,
                    i.Preco
                })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar a venda.");
            await transaction.RollbackAsync();
            return StatusCode(500, "Erro interno ao atualizar a venda.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelarVenda(int id)
    {
        // 1. Buscar a venda existente
        var venda = await _context.Vendas
            .Include(v => v.VendaItems)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venda == null)
            return NotFound("Venda não encontrada.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 2. Atualizar o estoque de volta
            foreach (var item in venda.VendaItems)
            {
                var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                if (produto != null)
                {
                    produto.QuantidadeEstoque += item.Quantidade;
                }
            }

            // 3. Excluir ou marcar a venda como cancelada (depende de como preferir tratar isso)
            _context.Vendas.Remove(venda);
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok("Venda cancelada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cancelar a venda.");
            await transaction.RollbackAsync();
            return StatusCode(500, "Erro interno ao cancelar a venda.");
        }
    }

}
