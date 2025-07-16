using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteConciliadora.Domain.Models;

public class Vaga
{
    [Key]
    public int Id { get; set; }

    public bool Ocupada { get; set; } = false;

    public int? VeiculoId { get; set; }

    [ForeignKey("VeiculoId")]
    public Veiculo? Veiculo { get; set; }
}