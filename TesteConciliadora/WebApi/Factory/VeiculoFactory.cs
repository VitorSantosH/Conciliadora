using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Repositories;
using TesteConciliadora.WebApi.DTOs;

namespace TesteConciliadora.WebApi.Factory;

public class VeiculoFactory
{
    public static async Task<(Veiculo? veiculo, string? erro)> CriarSeValido(
        VeiculoRepository veiculoRepository,
        VeiculoCsv veiculoCsv,
        Cliente cliente)
    {
        if (string.IsNullOrEmpty(veiculoCsv.Placa))
            return (null, "Placa invalida");

        if (string.IsNullOrEmpty(veiculoCsv.Modelo))
            return (null, "Modelo invalido");

        var veiculoJaCadastrado = (await veiculoRepository.Where(veiculo
                => veiculo.Ativo
                   && veiculo.Placa == veiculoCsv.Placa))
            .FirstOrDefault();

        if (veiculoJaCadastrado is not null)
            return (null, $"Placa ja cadastrada: {veiculoCsv.Placa.Trim()}");

        var veiculo = new Veiculo()
        {
            Placa = veiculoCsv.Placa,
            Modelo = veiculoCsv.Modelo,
            ClienteId = cliente.Id,
        };

        var vaiculoCadastrado = await veiculoRepository
            .AddReturnAsync(veiculo);


        return (vaiculoCadastrado, null);
    }
}