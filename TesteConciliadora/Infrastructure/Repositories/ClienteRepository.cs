using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Data;

namespace TesteConciliadora.Infrastructure.Repositories;


public class ClienteRepository : GenericRepository<Cliente>
{
    private readonly EstacionamentoDbContext _context;

    public ClienteRepository(EstacionamentoDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<Cliente?> AddReturnAsync(Cliente cliente)
    {
        try
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar cliente: {ex.Message}");
            return null;
        }
    }

}