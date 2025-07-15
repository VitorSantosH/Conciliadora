using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Repositories;
using TesteConciliadora.WebApi.DTOs;
using TesteConciliadora.WebApi.Factory;
using TesteConciliadora.WebApi.Utils;

namespace TesteConciliadora.WebApi.Controllers;

[ApiController]
[Route("api/upload/csv")]
public class UploadCsvController : ControllerBase
{
    private readonly ClienteRepository _clienteRepo;
    private readonly VeiculoRepository _veiculoRepo;

    public UploadCsvController(ClienteRepository clienteRepo, VeiculoRepository veiculoRepo)
    {
        _clienteRepo = clienteRepo;
        _veiculoRepo = veiculoRepo;
    }

    [HttpPost("clientes")]
    public async Task<IActionResult> UploadCLientes(IFormFile? file)
    {
        try
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest("Arquivo inválido");
            }

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            });

            var registros = csv.GetRecords<ClienteCsv>();
            var msgRetorno = new List<CsvClienteDto>();
            var totalCadastrado = 0;
            var totalErros = 0;


            foreach (var registro in registros)
            {
                try
                {
                    var retornoFactory =
                        await   ClienteFactory.CriarSeValido(_clienteRepo, registro.Nome, registro.Telefone);

                    if (retornoFactory.erro is not null)
                    {
                        msgRetorno.Add(new CsvClienteDto(null, false,
                            $"Erro ao salvar cliente '{registro.Nome} - {registro.Telefone}' no banco de dados, exception: {retornoFactory.erro}"));
                        totalErros++;
                    }
                    else
                    {
                        msgRetorno.Add(new CsvClienteDto(retornoFactory.cliente, true,
                            $"Cliente '{retornoFactory.cliente?.Nome}' cadastrado com sucesso. ID: {retornoFactory.cliente?.Id}."));
                        totalCadastrado++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    msgRetorno.Add(new CsvClienteDto(null, false, e.Message));
                    totalErros++;
                }
            }

            return Ok(new ResponseCadClientesCsvDto(totalCadastrado, totalErros, msgRetorno));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}