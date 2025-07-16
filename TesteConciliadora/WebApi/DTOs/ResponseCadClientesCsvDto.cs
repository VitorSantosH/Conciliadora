namespace TesteConciliadora.WebApi.DTOs;

public record ResponseCadClientesCsvDto( int TotalCadastrado , int TotalErros, List<CadastroCLienteDtl> ListaRetorno);