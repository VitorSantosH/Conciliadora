using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Data;
using TesteConciliadora.Infrastructure.Repositories;

namespace TesteConciliadora.WebApi.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController(ClienteRepository clienteRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> Get()
    {
        var clientes = await clienteRepository.GetAllAsync();
        return Ok(clientes);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Post(Cliente cliente)
    {
        await clienteRepository.AddAsync(cliente);
        return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
    }
}