using EduApoyosCommon.Interface;
using EduApoyosDomain.Entities;
using EduApoyosInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduApoyosInfrastructure.Repository
{
    public class EstudianteRepository : IEstudianteRepository
    {
        private readonly AppDbContext _appDbContext;

        public EstudianteRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Task<List<Estudiante>> GetEstudiantesAsync(int page, int pageSize)
        {
            return _appDbContext.Estudiantes
                .OrderBy(x => x.NumeroDocumento)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> CountEstudiantesAsync()
        {
            return await _appDbContext.Estudiantes.CountAsync();
        }
        public Task<Estudiante?> GetEstudianteByIdAsync(Guid id)
        {
            return _appDbContext.Estudiantes.AsTracking().FirstOrDefaultAsync(e => e.Id == id);
        }
        public Task<Estudiante?> GetEstudianteByNumeroDocumentoAsync(string NumeroDocumento)
        {
            return _appDbContext.Estudiantes.AsTracking().FirstOrDefaultAsync(e => e.NumeroDocumento == NumeroDocumento);
        }
        public async Task CrearAsync(Estudiante estudiante)
        {
            await _appDbContext.Estudiantes.AddAsync(estudiante);
        }
        public Task EliminarAsync(Estudiante estudiante)
        {
            _appDbContext.Estudiantes.Remove(estudiante);
            return Task.CompletedTask;
        }
        public async Task GuardarCambiosAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
