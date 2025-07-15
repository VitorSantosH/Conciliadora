using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Data;


namespace TesteConciliadora.Infrastructure.Repositories;



public class VeiculoRepository
{
    private readonly EstacionamentoDbContext _context;

    public VeiculoRepository(EstacionamentoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Veiculo>> GetAllAsync()
    {
        return await _context.Veiculos.Include(v => v.Cliente).ToListAsync();
    }

    public async Task<Veiculo?> GetByIdAsync(int id)
    {
        return await _context.Veiculos.Include(v => v.Cliente)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task AddAsync(Veiculo veiculo)
    {
        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Veiculo>> GetByClienteIdAsync(int clienteId)
    {
        return await _context.Veiculos
            .Where(v => v.ClienteId == clienteId)
            .ToListAsync();
    }
}
