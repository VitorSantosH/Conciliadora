using Microsoft.AspNetCore.Mvc;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Repositories;
using TesteConciliadora.WebApi.DTOs;

namespace TesteConciliadora.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MensalistasController(MensalistaRepository mensalistaRepository, ClienteRepository clienteRepository)
    : ControllerBase
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
    public async Task<ActionResult> Post([FromBody] MensalistaDto mensalista)
    {
        var cliente = await clienteRepository.getClienteMensalista(mensalista.ClienteId);

        if (cliente == null)
        {
            return NotFound("Cliente não encontrado");
        }

        if (cliente?.Mensalista != null)
            return Conflict("Este cliente já é mensalista.");

        var novoMensalista = new Mensalista()
        {
            ClienteId = mensalista.ClienteId,
            ValorMensal = mensalista.ValorMensal,
            DataInicio = DateTime.UtcNow
        };


        var mensalistaCadastrado = await mensalistaRepository.AddReturnAsync(novoMensalista);

        if (mensalistaCadastrado == null)
        {
            return Problem("Erro ao cadastrar");
        }

        return CreatedAtAction(nameof(GetByClienteId), new { clienteId = mensalista.ClienteId }, mensalistaCadastrado);
    }
    
    [HttpPost("update")]
    public async Task<ActionResult> Post([FromBody] Mensalista mensalista)
    {
        try
        {
            var entidadeAtualizada = await mensalistaRepository.UpdateReturnAsync(mensalista);

            if (entidadeAtualizada == null)
            {
                return Problem("Erro ao atualizar o registro do mensalista.");
            }

            return Ok(entidadeAtualizada);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem($"Erro interno ao processar a requisição: {ex.Message}");
        }
    }

}