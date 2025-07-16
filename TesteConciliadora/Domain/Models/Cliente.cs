using System.Text.Json.Serialization;

namespace TesteConciliadora.Domain.Models;

public class Cliente
{
    public Cliente () {}
    public Cliente(string nome, string telefone)
    {
        this.Nome = nome;
        this.Telefone = telefone;
    }
    
    public int Id { get; set; } 

    public string Nome { get; set; }

    public string Telefone { get; set; }

    public bool Ativo { get; set; } = true;

    public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();

    public Mensalista? Mensalista { get; set; }  // Associação opcional
}