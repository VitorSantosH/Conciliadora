namespace TesteConciliadora.WebApi.DTOs;

public record ResponseCadVeiculosCsvDto(int TotalCadastrado , int TotalErros, List<CadastroVeiculoDtl> ListaRetorno);