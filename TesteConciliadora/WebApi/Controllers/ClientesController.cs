using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Data;
using TesteConciliadora.Infrastructure.Repositories;
using TesteConciliadora.WebApi.DTOs;
using TesteConciliadora.WebApi.Factory;

namespace TesteConciliadora.WebApi.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController(ClienteRepository clienteRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> Get()
    {
        var clientes = await clienteRepository.GetAlLEntidadeCompletaAsync();
        return Ok(clientes);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Post(ClienteCsv cliente)
    {
        var (clienteSalvo, erro) =
            await  ClienteFactory.CriarSeValido(clienteRepository, cliente.Nome, cliente.Telefone);

        if (erro is not null)
        {
            return Problem(erro);
        }
        else
        {
            return CreatedAtAction(nameof(Post), new { id = clienteSalvo.Id }, clienteSalvo);
        }
    }
}