using TesteConciliadora.Domain.Models;

namespace TesteConciliadora.WebApi.DTOs;

public record CadastroVeiculoDtl(Veiculo? veiculo , bool Adicionado, string Error);