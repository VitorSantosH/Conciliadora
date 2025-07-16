using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Data;

namespace TesteConciliadora.Infrastructure.Repositories;

public class VagaRepository : GenericRepository<Vaga>
{
    private readonly EstacionamentoDbContext _context;

    public VagaRepository(EstacionamentoDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Vaga?> OcupaVagaAsync(int id, int veiculoId)
    {
        var vaga = await _context.Vagas.FindAsync(id);
        if (vaga == null) return null;

        vaga.Ocupada = true;
        vaga.VeiculoId = veiculoId;
        await _context.SaveChangesAsync();
        return vaga;
    }

    public async Task<List<Vaga>> GetAllAsync()
    {
        return await _context.Vagas.Include(v => v.Veiculo).ToListAsync();
    }
    
    public async Task<Vaga?> DesocuparVagaAsync(int id)
    {
        var vaga = await _context.Vagas.FindAsync(id);
        if (vaga == null)
            return null;

        vaga.Ocupada = false;
        vaga.VeiculoId = null;
        await _context.SaveChangesAsync();
        return vaga;
    }

}
