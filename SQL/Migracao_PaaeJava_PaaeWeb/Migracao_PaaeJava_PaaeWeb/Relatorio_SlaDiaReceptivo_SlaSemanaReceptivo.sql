USE AlctelVSS
GO

--Relatórios: Sla Dia Receptivo, Sla Dia Semana Receptivo
CREATE PROCEDURE Alc_PAAE_sp_RCC_SLADiaReceptivo
	@dataInicio VARCHAR(10) = '2021-01-14'
	,@dataFinal VARCHAR(10) = '2021-01-15'
	,@parSite INT
	,@tipoAmostragem VARCHAR(12) = 'POR_SEMANA' --'POR_DIA, POR_SEMANA
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE 
		@data_inicio DATE = @dataInicio
		,@data_final DATE = @dataFinal
		,@par_site INT = @parSite
		,@tipo_amostragem VARCHAR(12) = UPPER(@tipoAmostragem)

	IF OBJECT_ID('tempdb..#tAgentesEstatisticas') IS NOT NULL
		DROP TABLE #tAgentesEstatisticas
	IF OBJECT_ID('tempdb..#tChamadasEstatisticas') IS NOT NULL
		DROP TABLE #tChamadasEstatisticas


	SELECT 
		CAST([data] AS DATE) AS [data]
		,LEFT(agente_id, 3) AS agente_id_sufixo
		,COUNT(DISTINCT(agente_dbid)) AS AgentesLogados
		,SUM(tempoAtendimentoEntrada) AS tempoAtendimentoEntrada
	INTO #tAgentesEstatisticas
	FROM AlctelVSS..ufn_R_122_STAT_RES(@data_inicio, @data_final, @par_site)
	WHERE
		tempoLogado > 0
	GROUP BY CAST([data] AS DATE), LEFT(agente_id, 3)
	ORDER BY CAST([data] AS DATE), LEFT(agente_id, 3)

	SELECT 
		CAST([data] AS DATE) AS [data]
		,NomeGrupo
		,REPLACE(UPPER(LEFT(nomeFila, 3)), 'VEN', 'FVE') AS fila_prefixo --Vendas sendo tratadas com o prefixo FVE
		,SUM(Recebidas) AS Recebidas
		,SUM(Atendidas) AS Atendidas
		,SUM(Abandonadas) AS Abandonadas
		,SUM(TempoEspera) AS TempoEspera
		,SUM(TempoAbandono) AS TempoAbandono
	INTO #tChamadasEstatisticas
	FROM AlctelVSS..ufn_R_ALC_QUEUE_DAY(@data_inicio, @data_final, @par_site)
	GROUP BY CAST([data] AS DATE), NomeGrupo, LEFT(nomeFila, 3)
	ORDER BY CAST([data] AS DATE), NomeGrupo, LEFT(nomeFila, 3)
	
	IF (@tipo_amostragem = 'POR_DIA')
	BEGIN
		SELECT 
			C.[data]
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
		FROM #tChamadasEstatisticas AS C
			INNER JOIN #tAgentesEstatisticas AS A ON C.[data] = A.[data] AND C.fila_prefixo = A.agente_id_sufixo
		ORDER BY C.[data], C.fila_prefixo
	END

	IF (@tipo_amostragem = 'POR_SEMANA')
	BEGIN
		SELECT 
			CASE DATEPART(WEEKDAY, C.[data])
				WHEN 1 THEN 'DOMINGO'
				WHEN 2 THEN 'SEGUNDA'
				WHEN 3 THEN 'TERCA'
				WHEN 4 THEN 'QUARTA'
				WHEN 5 THEN 'QUINTA'
				WHEN 6 THEN 'SEXTA'
				WHEN 7 THEN 'SABADO'
			END AS DiaDaSemana
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
		FROM #tChamadasEstatisticas AS C
			INNER JOIN #tAgentesEstatisticas AS A ON C.[data] = A.[data] AND C.fila_prefixo = A.agente_id_sufixo
		ORDER BY C.[data], C.fila_prefixo
	END

END