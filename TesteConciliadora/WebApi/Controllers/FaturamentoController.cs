using Microsoft.AspNetCore.Mvc;
using TesteConciliadora.Infrastructure.Repositories;

namespace TesteConciliadora.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FaturamentoController(MensalistaRepository mensalistaRepository) : ControllerBase
{
    [HttpPost("gerar")]
    public async Task<ActionResult> GerarFaturamento()
    {
        var mensalistas = await mensalistaRepository.GetAllAsync();
        var dataAtual = DateTime.Now;

        var faturamentoSimulado = mensalistas
            .Where(m => m.Ativo)
            .Select(m => new
            {
                Cliente = m.Cliente.Nome,
                Telefone = m.Cliente.Telefone,
                Valor = m.ValorMensal,
                Referente = $"{dataAtual:MMMM/yyyy}"
            });

        return Ok(faturamentoSimulado);
    }
}