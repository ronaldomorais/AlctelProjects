USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[Alc_Paae_sp_RCC_SLADiaAtivo]    Script Date: 1/10/2023 5:59:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--Relatórios: Sla Dia Ativo - Outbound, Sla Dia Semana - Outbound
CREATE PROCEDURE [dbo].[Alc_Paae_sp_RCC_SLADiaAtivo]
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
		[data]
		,SUM(TempoAtendimentoSaida) AS TempoAtendimentoSaida
		,COUNT(DISTINCT (agente_dbid)) AS AgentesLogados
	INTO #tAgentesEstatisticas
	FROM AlctelVSS..ufn_R_123_STAT_RES(@data_inicio, @data_final, @par_site)
	WHERE
		tempoLogado > 0
	GROUP BY [data]



	SELECT 
		[data]
		,SUM(Discadas) AS Discadas
		,SUM(Atendidas) AS Atendidas
		,CASE WHEN SUM(Discadas) > 0
			THEN ROUND((CAST(SUM(Atendidas) AS FLOAT) / CAST(SUM(Discadas) AS FLOAT)) * 100, 2)
			ELSE 0
		END AS SLA
	INTO #tChamadasEstatisticas
	FROM AlctelVSS..ufn_R_ALC_CL_DAY(@data_inicio, @data_final, @par_site)
	GROUP BY [data]

	IF @tipo_amostragem = 'POR_DIA'
	BEGIN
		
		SELECT 
			C.[data]
			,C.Discadas
			,C.Atendidas
			,C.SLA
			,A.AgentesLogados
			--,A.TempoAtendimentoSaida
			,CASE WHEN C.Atendidas > 0
				THEN dbo.ToHHMMSS(A.TempoAtendimentoSaida / C.Atendidas)
				ELSE '00:00:00'
			END AS TempoMedioAtendimento
		FROM #tChamadasEstatisticas AS C
			INNER JOIN #tAgentesEstatisticas AS A ON C.[data] = A.[data]

	END

	IF @tipo_amostragem = 'POR_SEMANA'
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
			,C.Discadas
			,C.Atendidas
			,C.SLA
			,A.AgentesLogados
			--,A.TempoAtendimentoSaida
			,CASE WHEN C.Atendidas > 0
				THEN dbo.ToHHMMSS(A.TempoAtendimentoSaida / C.Atendidas)
				ELSE '00:00:00'
			END AS TempoMedioAtendimento
		FROM #tChamadasEstatisticas AS C
			INNER JOIN #tAgentesEstatisticas AS A ON C.[data] = A.[data]

	END
END
GO


