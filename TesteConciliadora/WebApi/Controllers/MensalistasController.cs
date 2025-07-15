using Microsoft.AspNetCore.Mvc;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Repositories;

namespace TesteConciliadora.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MensalistasController(MensalistaRepository mensalistaRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mensalista>>> Get()
    {
        var lista = await mensalistaRepository.GetAllAsync();
        return Ok(lista);
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<Mensalista>> GetByClienteId(int clienteId)
    {
        var mensalista = await mensalistaRepository.GetByClienteIdAsync(clienteId);
        if (mensalista == null)
            return NotFound();

        return Ok(mensalista);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Mensalista mensalista)
    {
        var existe = await mensalistaRepository.ExistsForClienteAsync(mensalista.ClienteId);
        if (existe)
            return Conflict("Este cliente já é mensalista.");

        await mensalistaRepository.AddAsync(mensalista);
        return CreatedAtAction(nameof(GetByClienteId), new { clienteId = mensalista.ClienteId }, mensalista);
    }
}