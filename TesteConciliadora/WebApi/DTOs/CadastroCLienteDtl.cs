using TesteConciliadora.Domain.Models;

namespace TesteConciliadora.WebApi.DTOs;

public record CadastroCLienteDtl(Cliente? cliente , bool adicionado, string error);