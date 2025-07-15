using TesteConciliadora.Domain.Models;

namespace TesteConciliadora.WebApi.DTOs;

public record CsvClienteDto(Cliente? cliente , bool adicionado, string error);