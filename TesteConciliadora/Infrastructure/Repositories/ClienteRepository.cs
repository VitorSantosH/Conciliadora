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


    public async Task<Cliente?> getCLientePorTelefone(string telefone)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Telefone == telefone);
    }
    
    public async Task<Cliente?> getClienteMensalista(int  id)
    {
        return await _context.Clientes
            .Include(c => c.Mensalista)
            .FirstOrDefaultAsync(c => c.Id == id);

    }
    
    public async Task<List<Cliente>> GetAlLEntidadeCompletaAsync()
    {
        return await _context.Clientes
            .Include(c => c.Veiculos)
            .Include(c => c.Mensalista)
            .ToListAsync();
    }

}