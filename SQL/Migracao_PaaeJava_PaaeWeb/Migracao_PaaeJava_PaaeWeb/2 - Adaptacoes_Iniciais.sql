USE AlctelVSS
GO

ALTER TABLE AlctelVSS.dbo.Alc_mtr001ConfigReport
ADD paginate INT
GO

ALTER TABLE AlctelVSS.dbo.Alc_mtr001ConfigReport
ADD timeout_processamento INT
GO


CREATE FUNCTION [dbo].[ToHHMMSS](@Segundos INT) RETURNS VARCHAR(50)	
AS
BEGIN
	DECLARE  
	@Minutos INT,   
	@Horas INT, 
	@FormatarTempo VARCHAR(50)  
  
  
	SELECT    
	@Minutos = @Segundos/60,   
	@Horas = @Segundos/3600  
  
	SELECT    
	@Segundos = @Segundos - ((@Segundos/60) * 60),   
	@Minutos = @Minutos - ((@Minutos/60) * 60) -- Minutos  
  
	SELECT    
	@FormatarTempo =   
	CAST(@Horas AS VARCHAR(30)) + ':' + CASE WHEN @Minutos < 10 THEN '0' ELSE '' END +   
	CAST(@Minutos AS VARCHAR(30)) + ':' + CASE WHEN @Segundos < 10 THEN '0' ELSE '' END +   
	CAST(@Segundos AS VARCHAR(30))
	RETURN (@FormatarTempo)
END
GO







