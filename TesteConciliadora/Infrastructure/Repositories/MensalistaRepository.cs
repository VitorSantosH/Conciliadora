using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Data;

namespace TesteConciliadora.Infrastructure.Repositories;


public class MensalistaRepository
{
    private readonly EstacionamentoDbContext _context;

    public MensalistaRepository(EstacionamentoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Mensalista>> GetAllAsync()
    {
        return await _context.Mensalistas
            .Include(m => m.Cliente)
            .ThenInclude(c => c.Veiculos)
            .ToListAsync();
    }

    public async Task<Mensalista?> GetByClienteIdAsync(int clienteId)
    {
        return await _context.Mensalistas
            .Include(m => m.Cliente)
            .ThenInclude(c => c.Veiculos)
            .FirstOrDefaultAsync(m => m.ClienteId == clienteId);
    }

    public async Task AddAsync(Mensalista mensalista)
    {
        _context.Mensalistas.Add(mensalista);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsForClienteAsync(int clienteId)
    {
        return await _context.Mensalistas.AnyAsync(m => m.ClienteId == clienteId);
    }
}