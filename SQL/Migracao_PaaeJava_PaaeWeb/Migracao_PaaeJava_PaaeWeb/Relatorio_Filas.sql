USE AlctelVSS
GO


CREATE PROCEDURE Alc_PAAE_sp_RCC_BuscaFila
	@dataInicio VARCHAR(10) = '2021-01-14'
	,@dataFinal VARCHAR(10) = '2021-01-15'
	,@parSite INT
	,@aplicacoes INT = 1
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE 
		@data_inicio DATE = @dataInicio
		,@data_final DATE = @dataFinal
		,@par_site INT = @parSite

	SELECT 
		R.NomeGrupo
		,R.nomeFila
		,R.[data]
		,SUM(R.Recebidas) AS Recebidas
		,SUM(R.Atendidas) AS Atendidas
		,CASE WHEN SUM(R.Recebidas) > 0
			THEN ROUND((CAST(SUM(R.Atendidas) AS FLOAT) / CAST(SUM(R.Recebidas) AS FLOAT)) * 100, 2)
			ELSE 0
		END AS Atendidas_Perc
		,SUM(R.Abandonadas) AS Abandonadas
		,CASE WHEN SUM(R.Recebidas) > 0
			THEN ROUND((CAST(SUM(R.Abandonadas) AS FLOAT) / CAST(SUM(R.Recebidas) AS FLOAT)) * 100, 2)
			ELSE 0
		END AS Abandonadas_Perc
	FROM AlctelVSS.dbo.ufn_R_ALC_QUEUE_DAY(@data_inicio, @data_final, @par_site) AS R
	WHERE
		aplicacoes = @aplicacoes
	GROUP BY R.NomeGrupo, R.nomeFila, R.[data]
	ORDER BY R.NomeGrupo, R.nomeFila, R.[data]

END