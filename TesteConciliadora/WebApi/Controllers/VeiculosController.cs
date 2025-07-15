using Microsoft.AspNetCore.Mvc;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Repositories;

namespace TesteConciliadora.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController(VeiculoRepository veiculoRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Veiculo>>> Get()
    {
        var veiculos = await veiculoRepository.GetAllAsync();
        return Ok(veiculos);
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<Veiculo>>> GetByClienteId(int clienteId)
    {
        var veiculos = await veiculoRepository.GetByClienteIdAsync(clienteId);
        return Ok(veiculos);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Veiculo veiculo)
    {
        await veiculoRepository.AddAsync(veiculo);
        return CreatedAtAction(nameof(Get), new { id = veiculo.Id }, veiculo);
    }
}