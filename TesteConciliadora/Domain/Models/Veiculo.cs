using System.Text.Json.Serialization;

namespace TesteConciliadora.Domain.Models;

public class Veiculo
{
    public int Id { get; set; } 

    public string Placa { get; set; }

    public string Modelo { get; set; }

    public int ClienteId { get; set; }
    
    [JsonIgnore]
    public Cliente Cliente { get; set; }

    public bool Ativo { get; set; } = true; 
}