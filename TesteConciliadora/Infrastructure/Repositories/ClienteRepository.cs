using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Data;

namespace TesteConciliadora.Infrastructure.Repositories;


public class ClienteRepository
{
    private readonly EstacionamentoDbContext _context;

    public ClienteRepository(EstacionamentoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> GetAllAsync()
    {
        return await _context.Clientes
            .Include(c => c.Veiculos)
            .Include(c => c.Mensalista)
            .ToListAsync();
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
        return await _context.Clientes
            .Include(c => c.Veiculos)
            .Include(c => c.Mensalista)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
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

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Clientes.AnyAsync(c => c.Id == id);
    }
    
    public List<Cliente?> Where(Expression<Func<Cliente, bool>> condicao)
    {
        try
        {
            var ret = _context.Set<Cliente>().Where(condicao).ToList();
            return ret;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

}