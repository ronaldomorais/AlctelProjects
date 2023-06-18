USE AlctelVSS
GO


ALTER PROCEDURE Alc_PAAE_sp_RCC_SLAHoraAtivo
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

	IF OBJECT_ID('tempdb..#tAgentesEstatisticas') IS NOT NULL
		DROP TABLE #tAgentesEstatisticas
	IF OBJECT_ID('tempdb..#tChamadasEstatisticas') IS NOT NULL
		DROP TABLE #tChamadasEstatisticas
	IF OBJECT_ID('tempdb..#tAgentesEstatisticasResumido') IS NOT NULL
		DROP TABLE #tAgentesEstatisticasResumido
	IF OBJECT_ID('tempdb..#tChamadasEstatisticasResumido') IS NOT NULL
		DROP TABLE #tChamadasEstatisticasResumido

	SELECT 
		CONCAT(RIGHT(CONVERT(VARCHAR(19), [data], 121), 8), ' - ', RIGHT(CONVERT(VARCHAR(19), DATEADD(HOUR, 1, [data]), 121), 8)) AS hora_intervalo
		,COUNT(DISTINCT(agente_dbid)) AS AgentesLogados
		,SUM(tempoAtendimentoSaida) AS tempoAtendimentoSaida
	INTO #tAgentesEstatisticas
	FROM AlctelVSS..ufn_R_122_STAT_RES(@data_inicio, @data_final, @par_site)
	WHERE
		tempoLogado > 0
	GROUP BY [data]


	SELECT 
		CONCAT(RIGHT(CONVERT(VARCHAR(19), [data], 121), 8), ' - ', RIGHT(CONVERT(VARCHAR(19), DATEADD(HOUR, 1, [data]), 121), 8)) AS hora_intervalo
		,SUM(Discadas) AS Discadas
		,SUM(Atendidas) AS Atendidas
	INTO #tChamadasEstatisticas
	FROM AlctelVSS..ufn_R_ALC_CL_HOUR(@data_inicio, @data_final, @par_site)
	GROUP BY [data]

	SELECT 
		hora_intervalo
		,SUM(Discadas) AS Discadas
		,SUM(Atendidas) AS Atendidas
	INTO #tChamadasEstatisticasResumido
	FROM #tChamadasEstatisticas
	GROUP BY hora_intervalo
	ORDER BY hora_intervalo

	SELECT 
		hora_intervalo
		,SUM(AgentesLogados) AS AgentesLogados
		,SUM(tempoAtendimentoSaida) AS tempoAtendimentoSaida
	INTO #tAgentesEstatisticasResumido
	FROM #tAgentesEstatisticas
	GROUP BY hora_intervalo
	ORDER BY hora_intervalo

	SELECT 
		C.hora_intervalo
		,Discadas
		,Atendidas
		,CASE WHEN C.Discadas > 0
			THEN ROUND((CAST(C.Atendidas AS FLOAT) / CAST(C.Discadas AS FLOAT)) * 100, 2)
			ELSE 0
		END SLA
		,CASE WHEN C.Atendidas > 0
			THEN dbo.ToHHMMSS(A.tempoAtendimentoSaida / C.Atendidas)
			ELSE '00:00:00'
		END AS TempoMedioAtendimento
		,AgentesLogados
	FROM #tChamadasEstatisticasResumido AS C
		INNER JOIN #tAgentesEstatisticasResumido AS A ON C.hora_intervalo = A.hora_intervalo
	ORDER BY C.hora_intervalo


END