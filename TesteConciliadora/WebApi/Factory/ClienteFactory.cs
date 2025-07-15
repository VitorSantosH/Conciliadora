using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Repositories;
using TesteConciliadora.WebApi.Utils;

namespace TesteConciliadora.WebApi.Factory;

public class ClienteFactory
{
    public static async Task<(Cliente? cliente, string? erro)> CriarSeValido(
        ClienteRepository clienteRepo,
        string nome,
        string telefone)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return (null, "Nome do cliente está vazio.");

        if (!TelefoneHelper.IsValid(telefone))
            return (null, $"Telefone '{telefone}' inválido.");


        var telefoneJaCadastrado = clienteRepo.Where(c
            => c.Ativo
               && c.Telefone.Trim() == telefone.Trim()).FirstOrDefault();

        if (telefoneJaCadastrado is not null)
            return (null, $"Telefone '{telefone}' ja cadastrado em outro cliente ativo.");

        var clienteCadastrado = await clienteRepo
            .AddReturnAsync(new Cliente(nome, telefone));


        return (clienteCadastrado, null);
    }
}