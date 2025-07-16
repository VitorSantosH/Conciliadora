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
    public async Task<IActionResult> UploadClientes(IFormFile? file)
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
            var msgRetorno = new List<CadastroCLienteDtl>();
            var totalCadastrado = 0;
            var totalErros = 0;


            foreach (var registro in registros)
            {
                try
                {
                    var retornoFactory =
                        await ClienteFactory.CriarSeValido(_clienteRepo, registro.Nome, registro.Telefone);

                    if (retornoFactory.erro is not null)
                    {
                        msgRetorno.Add(new CadastroCLienteDtl(null, false,
                            $"Erro ao salvar cliente '{registro.Nome} - {registro.Telefone}' no banco de dados, exception: {retornoFactory.erro}"));
                        totalErros++;
                    }
                    else
                    {
                        msgRetorno.Add(new CadastroCLienteDtl(retornoFactory.cliente, true,
                            $"Cliente '{retornoFactory.cliente?.Nome}' cadastrado com sucesso. ID: {retornoFactory.cliente?.Id}."));
                        totalCadastrado++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    msgRetorno.Add(new CadastroCLienteDtl(null, false, e.Message));
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

    [HttpPost("veiculos")]
    public async Task<IActionResult> UploadCsv(IFormFile? file)
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

            var registros = csv.GetRecords<VeiculoCsv>();
            var msgRetorno = new List<CadastroVeiculoDtl>();
            var totalCadastrado = 0;
            var totalErros = 0;


            foreach (var registro in registros)
            {
                try
                {
                    Cliente? cliente = null;

                    if (registro.ClienteId is not null && registro.ClienteId > 0)
                    {
                        cliente = await _clienteRepo.GetByIdAsync((int)registro.ClienteId);
                    }
                    else if (!String.IsNullOrEmpty(registro.ClienteTelefone))
                    {
                        cliente = await _clienteRepo.getCLientePorTelefone(registro.ClienteTelefone.Trim());
                    }

                    if (cliente is null)
                    {
                        msgRetorno.Add(new CadastroVeiculoDtl(null, false,
                            $"Erro ao salvar veiculo placa '{registro.Placa}' no banco de dados, exception: cliente não encontrado"));
                        totalErros++;
                    }

                    var retornoFactory =
                        await VeiculoFactory.CriarSeValido(_veiculoRepo, registro, cliente);

                    if (retornoFactory.erro is not null)
                    {
                        msgRetorno.Add(new CadastroVeiculoDtl(null, false,
                            $"Erro ao salvar veiculo '{registro.Placa}' no banco de dados, exception: {retornoFactory.erro}"));
                        totalErros++;
                    }
                    else
                    {
                        msgRetorno.Add(new CadastroVeiculoDtl(retornoFactory.veiculo, true,
                            $"Veiculo '{retornoFactory.veiculo?.Placa}' cadastrado com sucesso. ID: {retornoFactory.veiculo?.Id}."));
                        totalCadastrado++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    msgRetorno.Add(new CadastroVeiculoDtl(null, false, e.Message));
                    totalErros++;
                }
            }

            return Ok(new ResponseCadVeiculosCsvDto(totalCadastrado, totalErros, msgRetorno));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}