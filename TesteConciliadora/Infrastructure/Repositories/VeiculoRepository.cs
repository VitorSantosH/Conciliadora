using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Domain.Models;
using TesteConciliadora.Infrastructure.Data;


namespace TesteConciliadora.Infrastructure.Repositories;

public class VeiculoRepository : GenericRepository<Veiculo>
{
    private readonly EstacionamentoDbContext _context;

    public VeiculoRepository(EstacionamentoDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<Veiculo?> GetByIdAsync(int id)
    {
        return await _context.Veiculos.Include(v => v.Cliente)
            .FirstOrDefaultAsync(v => v.Id == id);
    }


    public async Task<List<Veiculo>> GetByClienteIdAsync(int clienteId)
    {
        return await _context.Veiculos
            .Where(v => v.ClienteId == clienteId)
            .ToListAsync();
    }

    public async Task<Veiculo?> UpdateVariaveisPermitidas(Veiculo veiculo)
    {
        var entidadeOriginal = await _context.Veiculos.FindAsync(veiculo.Id);

        veiculo.Placa = entidadeOriginal.Placa;

        var entidadeAtualizada = await UpdateReturnAsync(veiculo);

        return entidadeAtualizada;
    }
}