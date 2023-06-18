USE AlctelVSS
GO


CREATE PROCEDURE Alc_PAAE_sp_RCC_SLAHoraReceptivo
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
		,LEFT(agente_id, 3) AS agente_id_sufixo
		,COUNT(DISTINCT(agente_dbid)) AS AgentesLogados
		,SUM(tempoAtendimentoEntrada) AS tempoAtendimentoEntrada
	INTO #tAgentesEstatisticas
	FROM AlctelVSS..ufn_R_122_STAT_RES(@data_inicio, @data_final, @par_site)
	WHERE
		tempoLogado > 0
	GROUP BY [data], LEFT(agente_id, 3)
	ORDER BY [data], LEFT(agente_id, 3)

	SELECT 
		CONCAT(RIGHT(CONVERT(VARCHAR(19), [data], 121), 8), ' - ', RIGHT(CONVERT(VARCHAR(19), DATEADD(HOUR, 1, [data]), 121), 8)) AS hora_intervalo
		,NomeGrupo
		,REPLACE(UPPER(LEFT(nomeFila, 3)), 'VEN', 'FVE') AS fila_sufixo --Vendas sendo tratadas com o prefixo FVE
		,SUM(Recebidas) AS Recebidas
		,SUM(Atendidas) AS Atendidas
		,SUM(Abandonadas) AS Abandonadas
		,SUM(TempoEspera) AS TempoEspera
		,SUM(TempoAbandono) AS TempoAbandono
	INTO #tChamadasEstatisticas
	FROM AlctelVSS..ufn_R_ALC_QUEUE_HOUR(@data_inicio, @data_final, @par_site)
	GROUP BY [data], NomeGrupo, LEFT(nomeFila, 3)
	ORDER BY [data], NomeGrupo
	
	SELECT 
		hora_intervalo
		,NomeGrupo
		,fila_sufixo
		,SUM(Recebidas) AS Recebidas
		,SUM(Atendidas) AS Atendidas
		,SUM(Abandonadas) AS Abandonadas
		,SUM(TempoEspera) AS TempoEspera
		,SUM(TempoAbandono) AS TempoAbandono
	INTO #tChamadasEstatisticasResumido
	FROM #tChamadasEstatisticas
	GROUP BY hora_intervalo, NomeGrupo, fila_sufixo
	ORDER BY hora_intervalo, NomeGrupo, fila_sufixo


	SELECT 
		hora_intervalo
		,agente_id_sufixo
		,SUM(AgentesLogados) AS AgentesLogados
		,SUM(tempoAtendimentoEntrada) AS tempoAtendimentoEntrada
	INTO #tAgentesEstatisticasResumido
	FROM #tAgentesEstatisticas
	GROUP BY hora_intervalo, agente_id_sufixo
	ORDER BY hora_intervalo

	SELECT 
		C.hora_intervalo
		,C.NomeGrupo
		,C.Recebidas
		,C.Atendidas
		,C.Abandonadas
		,CASE WHEN C.Recebidas > 0
			THEN ROUND((CAST(C.Atendidas AS FLOAT) / CAST(C.Recebidas AS FLOAT)) * 100, 2)
			ELSE 0
		END AS SLA
		,A.AgentesLogados
		,CASE WHEN C.Atendidas > 0
			THEN dbo.ToHHMMSS(C.TempoEspera / C.Atendidas)
			ELSE '00:00:00'
		END AS TempoMedioEspera
		,CASE WHEN C.Abandonadas > 0
			THEN dbo.ToHHMMSS(C.TempoAbandono / C.Abandonadas)
			ELSE '00:00:00'
		END AS TempoMedioAbandono
		,CASE WHEN C.Atendidas > 0
			THEN dbo.ToHHMMSS(A.tempoAtendimentoEntrada / C.Atendidas)
			ELSE '00:00:00'
		END AS TempoMedioAtendimento
	FROM #tChamadasEstatisticasResumido AS C
		INNER JOIN #tAgentesEstatisticasResumido AS A ON C.hora_intervalo = A.hora_intervalo AND C.fila_sufixo = A.agente_id_sufixo
	ORDER BY C.hora_intervalo, C.fila_sufixo
END