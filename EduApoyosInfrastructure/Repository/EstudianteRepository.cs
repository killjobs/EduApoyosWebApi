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
        public Task<List<Estudiante>> GetEstudiantesAsync()
        {
            return _appDbContext.Estudiantes.ToListAsync();
        }
    }
}
