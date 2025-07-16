using Microsoft.AspNetCore.Mvc;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Repositories;
using TesteConciliadora.WebApi.DTOs;
using TesteConciliadora.WebApi.Factory;

namespace TesteConciliadora.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController(ClienteRepository _clienteRepo, VeiculoRepository veiculoRepository) : ControllerBase
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
    public async Task<ActionResult> Post([FromBody] VeiculoCsv veiculoCsv)
    {
        Cliente? cliente = null;

        if (veiculoCsv.ClienteId is not null && veiculoCsv.ClienteId > 0)
        {
            cliente = await _clienteRepo.GetByIdAsync((int)veiculoCsv.ClienteId);
        }
        else if (!String.IsNullOrEmpty(veiculoCsv.ClienteTelefone))
        {
            cliente = await _clienteRepo.getCLientePorTelefone(veiculoCsv.ClienteTelefone.Trim());
        }

        if (cliente is null)
        {
            return Problem(
                $"Erro ao salvar veiculo placa '{veiculoCsv.Placa}' no banco de dados, exception: cliente não encontrado");
        }

        var (veiculoCadastrado, erro) = await VeiculoFactory.CriarSeValido(veiculoRepository, veiculoCsv, cliente);

        if (erro != null)
        {
            return Problem(erro);
        }

        return CreatedAtAction(nameof(Get), new { id = veiculoCadastrado?.Id }, veiculoCadastrado);
    }
    
    
    [HttpPost("update")]
    public async Task<ActionResult> Update([FromBody] VeiculoCsv veiculoCsv)
    {
        try
        {
            // var entidadeAtualizada = await veiculoRepository.UpdateReturnAsync(veiculoCsv);
            //
            // if (entidadeAtualizada == null)
            // {
            //     return Problem("Erro ao atualizar o registro do mensalista.");
            // }
            //
            // return Ok(entidadeAtualizada);

            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem($"Erro interno ao processar a requisição: {ex.Message}");
        }
    }

    
}