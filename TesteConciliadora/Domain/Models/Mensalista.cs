using System.Text.Json.Serialization;

namespace TesteConciliadora.Domain.Models;

public class Mensalista
{
    public int Id { get; set; } 

    public int ClienteId { get; set; }
    
    [JsonIgnore]
    public Cliente Cliente { get; set; }
    public decimal ValorMensal { get; set; }

    public DateTime DataInicio { get; set; }

    public bool Ativo { get; set; } = true;
}