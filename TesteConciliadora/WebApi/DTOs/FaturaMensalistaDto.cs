namespace TesteConciliadora.WebApi.DTOs;

public record FaturaMensalistaDto(
    int MensalistaId,
    string ClienteNome,
    decimal Valor,
    DateTime Competencia
);