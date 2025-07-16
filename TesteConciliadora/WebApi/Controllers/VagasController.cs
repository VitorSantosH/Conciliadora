using Microsoft.AspNetCore.Mvc;
using TesteConciliadora.Infrastructure.Repositories;
using TesteConciliadora.WebApi.DTOs;

namespace TesteConciliadora.WebApi.Controllers;

[ApiController]
[Route("api/vagas")]
public class VagasController(VagaRepository vagaRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var vagas = await vagaRepository.GetAllAsync();
        return Ok(vagas);
    }

    [HttpPost("{id}/ocupar")]
    public async Task<ActionResult> OcupaVaga(int id, [FromBody] VagaDto dto)
    {
        if (dto.VeiculoId == null)
            return BadRequest("VeiculoId é obrigatório.");

        var vaga = await vagaRepository.OcupaVagaAsync(id, dto.VeiculoId.Value);
        if (vaga == null)
            return NotFound("Vaga não encontrada.");

        return Ok(vaga);
    }

    [HttpPost("{id}/desocupar")]
    public async Task<ActionResult> DesocuparVaga(int id)
    {
        var vaga = await vagaRepository.DesocuparVagaAsync(id);
        if (vaga == null)
            return NotFound("Vaga não encontrada.");

        return Ok(vaga);
    }
}