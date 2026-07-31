using EduApoyosApplication;
using EduApoyosCommon.Interface;
using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;
using Moq;

namespace EduApoyosTest.Application
{
    public class SolicitudApplicationTests
    {
        private readonly Mock<ISolicitudRepository> _repositoryMock;

        private readonly SolicitudApplication _application;

        public SolicitudApplicationTests()
        {
            _repositoryMock = new Mock<ISolicitudRepository>();

            _application = new SolicitudApplication(_repositoryMock.Object);
        }
        [Fact]
        public async Task GetSolicitudesAsync_Should_ReturnPagedResult_When_DataExists()
        {
            var solicitudes = new List<SolicitudApoyo>{
                new(),
                new()
            };
            _repositoryMock.Setup(x => x.GetAsync(1, 10, null)).ReturnsAsync(solicitudes);
            _repositoryMock.Setup(x => x.CountAsync(null)).ReturnsAsync(2);

            var result = await _application.GetSolicitudesAsync(1, 10, null);

            Assert.True(result.Success);
            Assert.Equal(2, result.Data.TotalRecords);
            Assert.Equal(2, result.Data.Items.Count);
        }
        [Fact]
        public async Task GetByIdHistorialAsync_Should_ReturnSolicitud_When_UserIsAsesor()
        {
            var solicitudId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var solicitud = new SolicitudApoyo
            {
                Estudiante = new Estudiante { UsuarioId = Guid.NewGuid() }
            };
            _repositoryMock.Setup(x => x.GetByIdAsync(solicitudId)).ReturnsAsync(solicitud);
            _repositoryMock.Setup(x => x.GetByIdHistorialAsync(solicitudId)).ReturnsAsync(solicitud);

            var result = await _application.GetByIdHistorialAsync(solicitudId, usuarioId, RolUsuario.Asesor);
            Assert.True(result.Success);
        }
        [Fact]
        public async Task GetByIdHistorialAsync_Should_ThrowUnauthorizedAccessException_When_Estudiante_Is_NotOwner()
        {
            var solicitudId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var solicitud = new SolicitudApoyo
            {
                Estudiante = new Estudiante
                {
                    UsuarioId = Guid.NewGuid()
                }
            };
            _repositoryMock.Setup(x => x.GetByIdAsync(solicitudId)).ReturnsAsync(solicitud);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _application.GetByIdHistorialAsync(solicitudId, usuarioId, RolUsuario.Estudiante)
            );
        }
        [Fact]
        public async Task CrearAsync_Should_CreateSolicitud_With_Pendiente_Status()
        {
            var solicitudApoyoDto = new CrearSolicitudApoyoDto
            {
                EstudianteId = Guid.NewGuid(),
                AsesorId = Guid.NewGuid(),
                TipoApoyo = TipoApoyoEnum.Beca,
                MontoSolicitado = 500000
            };
            _repositoryMock.Setup(x => x.ExisteSolicitudActivaAsync(solicitudApoyoDto.EstudianteId, solicitudApoyoDto.TipoApoyo)).ReturnsAsync(false);

            await _application.CrearAsync(solicitudApoyoDto);

            _repositoryMock.Verify(x => x.CrearAsync(
                It.Is<SolicitudApoyo>(s => s.Estado == EstadoSolicitudEnum.Pendiente)),
                Times.Once
            );

            _repositoryMock.Verify(x => x.GuardarCambiosAsync(), Times.Once);
        }
        [Fact]
        public async Task CrearAsync_Should_ThrowException_When_ExisteSolicitudActiva()
        {
            var solicitudApoyoDto = new CrearSolicitudApoyoDto
            {
                EstudianteId = Guid.NewGuid(),
                TipoApoyo = TipoApoyoEnum.Beca
            };
            _repositoryMock.Setup(x => x.ExisteSolicitudActivaAsync(solicitudApoyoDto.EstudianteId, solicitudApoyoDto.TipoApoyo)).ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _application.CrearAsync(solicitudApoyoDto));
        }
        [Fact]
        public async Task CambiarEstadoAsync_Should_AddHistorial_When_StatusChanges()
        {
            var solicitud = new SolicitudApoyo
            {
                Id = Guid.NewGuid(),
                Estado = EstadoSolicitudEnum.Pendiente
            };
            var solicitudDto = new CambiarEstadoSolicitudDto
            {
                Estado = EstadoSolicitudEnum.EnRevision
            };
            _repositoryMock.Setup(x => x.GetByIdAsync(solicitud.Id)).ReturnsAsync(solicitud);

            await _application.CambiarEstadoAsync(solicitud.Id, solicitudDto, Guid.NewGuid());

            _repositoryMock.Verify(x =>
                x.AgregarHistorialAsync(It.IsAny<HistorialEstado>()),
                Times.Once
            );

            _repositoryMock.Verify(x =>
                x.GuardarCambiosAsync(),
                Times.Once
            );
        }
        [Fact]
        public async Task CambiarEstadoAsync_Should_ThrowException_When_Solicitud_NotFound()
        {
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((SolicitudApoyo?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _application.CambiarEstadoAsync(Guid.NewGuid(), new CambiarEstadoSolicitudDto(), Guid.NewGuid())
            );
        }
        [Fact]
        public async Task CambiarEstadoAsync_Should_ThrowException_When_StatusAlreadyExists()
        {
            var solicitud = new SolicitudApoyo
            {
                Estado = EstadoSolicitudEnum.Pendiente
            };
            var solicitudDto = new CambiarEstadoSolicitudDto
            {
                Estado = EstadoSolicitudEnum.Pendiente
            };
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(solicitud);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _application.CambiarEstadoAsync(Guid.NewGuid(), solicitudDto, Guid.NewGuid())
            );
        }
    }
}
