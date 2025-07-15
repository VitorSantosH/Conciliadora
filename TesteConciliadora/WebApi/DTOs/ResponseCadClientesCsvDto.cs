namespace TesteConciliadora.WebApi.DTOs;

public record ResponseCadClientesCsvDto( List<CsvClienteDto> ListaRetorno, int TotalCadastrado , int TotalErros);