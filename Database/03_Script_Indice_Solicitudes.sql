USE EduApoyos
GO

/*
**summary***
Se define el indice no agrupado para las columnas de Estado y FechaActualización teniendo como referencia que es un punto de consulta normal,
con esto mejora el rendimiento de la respuesta para cada consulta recibida.
*/
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name = 'IX_SolicitudesApoyo_Estado_FechaActualizacion')
BEGIN
    CREATE NONCLUSTERED INDEX IX_SolicitudesApoyo_Estado_FechaActualizacion
    ON SolicitudesApoyo
    (
        Estado,
        FechaActualizacion
    );
    PRINT 'Indice creado correctamente';
END
ELSE
BEGIN
    PRINT 'El indice ya existe';
END