using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Repositories;
using TesteConciliadora.WebApi.DTOs;
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
                    //regex telefone 
                    if (!TelefoneHelper.IsValid(registro.Telefone) || String.IsNullOrEmpty(registro.Nome))
                    {
                        msgRetorno.Add(new CsvClienteDto(null, false,
                            $"Erro ao salvar cliente '{registro.Nome} - {registro.Telefone}' no banco de dados, dados invalidos "));
                        totalErros++;
                    }
                    else
                    {
                        var novoCliente = new Cliente(registro.Nome, registro.Telefone) { };
                        var clienteCadastrado = await _clienteRepo.AddReturnAsync(novoCliente);

                        if (clienteCadastrado is null)
                        {
                            msgRetorno.Add(new CsvClienteDto(novoCliente, false,
                                $"Erro ao salvar cliente '{registro.Nome} - {registro.Telefone}' no banco de dados."));
                            totalErros++;
                        }
                        else
                        {
                            msgRetorno.Add(new CsvClienteDto(clienteCadastrado, true,
                                $"Cliente '{clienteCadastrado.Nome}' cadastrado com sucesso. ID: {clienteCadastrado.Id}."));
                            totalCadastrado++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return Ok(new ResponseCadClientesCsvDto(msgRetorno, totalCadastrado, totalErros));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

}
