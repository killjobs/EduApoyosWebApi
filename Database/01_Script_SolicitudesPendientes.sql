USE EduApoyos
GO
/*
**summary***
Script que realiza la consulta y que lista las solicitudes pendientes con más de 5 días sin actualización,
ordenadas por antigüedad.
*/
SELECT
    Id,
    EstudianteId,
    TipoApoyo,
    Estado,
    FechaSolicitud,
    FechaActualizacion
FROM SolicitudesApoyo
WHERE Estado = 1
AND FechaActualizacion <= DATEADD(DAY, -5, GETUTCDATE())
ORDER BY FechaActualizacion ASC;