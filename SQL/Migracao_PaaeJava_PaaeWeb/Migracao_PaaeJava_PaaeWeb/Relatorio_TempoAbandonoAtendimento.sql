USE AlctelVSS
GO



CREATE PROCEDURE Alc_PAAE_sp_RCC_TempoAbandonoAtendimento
	@dataInicio VARCHAR(10) = '2021-01-14'
	,@dataFinal VARCHAR(10) = '2021-01-15'
	,@parSite INT
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE 
		@data_inicio DATE = @dataInicio
		,@data_final DATE = @dataFinal
		,@par_site INT = @parSite


	SELECT 
		NomeGrupo
		,nomeFila
		,SUM(Recebidas) AS Recebidas
		,SUM(Atendidas) AS Atendidas
		,SUM(Atendidas30) AS AtendidasAte30
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Atendidas30) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS AtendidasAte30_Perc
		,SUM(Atendidas31) AS Atendidas31a60
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Atendidas31) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS Atendidas31a60_Perc
		,SUM(Atendidas61) AS Atendidas61a90
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Atendidas61) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS Atendidas61a90_Perc
		,SUM(Atendidas91) AS Atendidas91a120
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Atendidas91) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS Atendidas91a120_Perc
		,SUM(Atendidas121) AS AtendidasMaior120
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Atendidas121) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS AtendidasMaior120_Perc
		,SUM(Abandonadas) AS Abandonadas
		,SUM(Abandonadas30) AS AbandonadasAte30
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Abandonadas30) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS AbandonadasAte30_Perc
		,SUM(Abandonadas31) AS Abandonadas31a60
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Abandonadas31) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS Abandonadas31a60_Perc
		,SUM(Abandonadas61) AS Abandonadas61a90
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Abandonadas61) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS Abandonadas61a90_Perc
		,SUM(Abandonadas91) AS Abandonadas91a120
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Abandonadas91) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS Abandonadas91a120_Perc
		,SUM(Abandonadas121) AS AbandonadasMaior120
		,CASE WHEN SUM(Recebidas) > 0 THEN ROUND((CAST(SUM(Abandonadas121) AS FLOAT) / CAST(SUM(Recebidas) AS FLOAT)) * 100, 2) ELSE 0 END AS AbandonadasMaior120_Perc
	FROM AlctelVSS..ufn_R_ALC_QUEUE_DAY(@data_inicio, @data_final, @par_site)
	GROUP BY NomeGrupo, nomeFila
	ORDER BY NomeGrupo, nomeFila
END