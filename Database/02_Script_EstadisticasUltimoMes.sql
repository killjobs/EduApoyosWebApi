USE EduApoyos
GO
/*
**summary***
Script que realiza la consulta y que cuenta el total de solicitudes agrupadas por estado y tipo de apoyo en el último
mes.
*/
SELECT
    CASE Estado
        WHEN 1 THEN 'Pendiente'
        WHEN 2 THEN 'En Revision'
        WHEN 3 THEN 'Aprobada'
        WHEN 4 THEN 'Rechazada'
        ELSE 'No definido'
    END AS Estado,
    CASE TipoApoyo
        WHEN 1 THEN 'Beca'
        WHEN 2 THEN 'Credito'
        WHEN 3 THEN 'Subsidio'
        ELSE 'No definido'
    END AS TipoApoyo,
    COUNT(*) AS TotalSolicitudes
FROM SolicitudesApoyo
WHERE FechaSolicitud >= DATEADD(MONTH,-1,GETUTCDATE())
GROUP BY Estado, TipoApoyo
ORDER BY Estado, TipoApoyo;