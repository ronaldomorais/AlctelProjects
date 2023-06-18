USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[TESTE_Alc_Paae_001_sp_hiProdutividadeFiltroSup]    Script Date: 23/03/2023 16:27:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









CREATE PROCEDURE [dbo].[TESTE_Alc_Paae_001_sp_hiProdutividadeFiltroSup]   
	@dataInicial datetime = '2023-01-19'
	,@dataFinal datetime   = '2023-01-20'   
	,@parSite int = 0
	,@parUser NVARCHAR(50)
AS 
BEGIN

	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE @site_id INT = @parSite
	DECLARE
		@inicio_dt DATETIME = CAST(@dataInicial AS DATE)
		,@fim_dt DATETIME = CAST(@dataFinal AS DATE)

	SET @fim_dt = DATEADD(SECOND, 86399, @fim_dt)
		
	SET @site_id = 0 --Para trazer todas as informaçoes no relatorios de chamadas

	IF OBJECT_ID('tempdb..#tChamadas') IS NOT NULL
		DROP TABLE #tChamadas

	IF OBJECT_ID('tempdb..#tNavegacao') IS NOT NULL
		DROP TABLE #tNavegacao

	IF OBJECT_ID('tempdb..#tCTIEventosLogin') IS NOT NULL
		DROP TABLE #tCTIEventosLogin

	CREATE TABLE #tCTIEventosLogin
	(
		sequencial INT
		,dbid INT
		,agentid VARCHAR(50)
		,fullname VARCHAR(200)
		,place VARCHAR(50)
		,datalogin DATETIME
		,horalogin DATETIME
		,horalogout DATETIME
		,loginativo INT
	)


	IF OBJECT_ID('tempdb..#tLoginSumario') IS NOT NULL
		DROP TABLE #tLoginSumario

	IF OBJECT_ID('tempdb..#tChamadasSumario') IS NOT NULL
		DROP TABLE #tChamadasSumario

	IF OBJECT_ID('tempdb..#tConsultaSumario') IS NOT NULL
		DROP TABLE #tConsultaSumario

	IF OBJECT_ID('tempdb..#pausas') IS NOT NULL
		DROP TABLE #pausas

	CREATE TABLE #pausas (
		sequencial INT,
		inicio datetime,
		fim datetime,
		duracao int,
		descricao varchar(100),
		reason int,
		prepausa int,
		eventologin int
	)

	IF OBJECT_ID('tempdb..#tPausasSumario') IS NOT NULL
		DROP TABLE #tPausasSumario


	SELECT
		cha_id
		,cha_ini
		,cha_fim
		,age_agentid
		,age_dbid
		,age_fullname
		,age_ini
		,age_fim
		,CASE WHEN cha_tipo_chamada	= 'ENTRADA' THEN DATEDIFF(SECOND, age_ini, age_fim) ELSE 0 END AS tempo_conversacao
		,CASE WHEN cha_tipo_chamada	= 'ENTRADA' THEN 1 ELSE 0 END AS atendida
		,CASE WHEN cha_tipo_chamada	= 'SAIDA' THEN 1 ELSE 0 END AS realizada
		,CASE WHEN cha_tipo_chamada	= 'SAIDA' THEN DATEDIFF(SECOND, age_ini, age_fim) ELSE 0 END AS duracao_saida
	INTO #tChamadas
	FROM AlctelVSS..ufn_GetChamadas(@dataInicial, @dataFinal, @site_id)
	WHERE
		status_ligacao = 'ATENDIDA'
	ORDER BY cha_id, cha_ini



	SELECT 
		P.[data]
		,P.cha_id
		,ISNULL(P.pesquisa_1, 0) AS pesquisa_1
		,ISNULL(P.pesquisa_2, 0) AS pesquisa_2
		,ISNULL(P.pesquisa_3, 0) AS pesquisa_3
		,ISNULL(P.pesquisa_4, 0) AS pesquisa_4
		,ISNULL(P.pesquisa_5, 0) AS pesquisa_5
	INTO #tNavegacao
	FROM (

	SELECT 
		CAST(CAST([data] AS DATE) AS DATETIME) AS [data]
		,cha_id
		,interacao
		--,dado
		,1 AS qtd
	FROM AlctelSRVCTI..Alc_Navegacao WITH (NOLOCK)
	WHERE 
		[data] BETWEEN @inicio_dt AND @fim_dt
		AND interacao LIKE 'pesquisa_%'

	) AS S
	PIVOT(SUM(S.qtd) FOR S.interacao IN([pesquisa_1], [pesquisa_2], [pesquisa_3], [pesquisa_4], [pesquisa_5])) AS P

	EXEC CTI_REL_Alc_Paae_Login_Base @dataInicial, @dataFinal, @site_id

	INSERT INTO #pausas
	EXEC CTI_REL_Alc_Paae_Pausas_Base @dataInicial, @dataFinal


	SELECT 
		P.[data]
		,P.eventologin
		,ISNULL(P.[pausa0], 0) AS [pausa0]
		,ISNULL(P.[pausa1], 0) AS [pausa1]
		,ISNULL(P.[pausa2], 0) AS [pausa2]
		,ISNULL(P.[pausa3], 0) AS [pausa3]
		,ISNULL(P.[pausa4], 0) AS [pausa4]
		,ISNULL(P.[pausa5], 0) AS [pausa5]
		,ISNULL(P.[pausa6], 0) AS [pausa6]
		,ISNULL(P.[pausa7], 0) AS [pausa7]
		,ISNULL(P.[pausa8], 0) AS [pausa8]
		,ISNULL(P.[pausa9], 0) AS [pausa9]
		,ISNULL(P.[pausa10], 0) AS [pausa10]
		,ISNULL(P.[pausa11], 0) AS [pausa11]
		,ISNULL(P.[pausa12], 0) AS [pausa12]
		,ISNULL(P.[pausa13], 0) AS [pausa13]
		,ISNULL(P.[pausa14], 0) AS [pausa14]
		,ISNULL(P.[pausa15], 0) AS [pausa15]
	INTO #tPausasSumario
	FROM (

		SELECT 
			CAST(CAST(inicio AS DATE) AS DATETIME) AS [data]
			--,descricao
			--,reason
			,CASE reason
				WHEN 0 THEN 'pausa0'
				WHEN 1 THEN 'pausa1'
				WHEN 2 THEN 'pausa2'
				WHEN 3 THEN 'pausa3'
				WHEN 4 THEN 'pausa4'
				WHEN 5 THEN 'pausa5'
				WHEN 6 THEN 'pausa6'
				WHEN 7 THEN 'pausa7'
				WHEN 8 THEN 'pausa8'
				WHEN 9 THEN 'pausa9'
				WHEN 10 THEN 'pausa10'
				WHEN 11 THEN 'pausa11'
				WHEN 12 THEN 'pausa12'
				WHEN 13 THEN 'pausa13'
				WHEN 14 THEN 'pausa14'
				WHEN 15 THEN 'pausa15'
			END AS descricao
			,eventologin		
			,SUM(duracao) AS duracao
		FROM #pausas
		WHERE
			prepausa <> 1
		GROUP BY CAST(CAST(inicio AS DATE) AS DATETIME), descricao, reason, eventologin

	) AS S
	PIVOT(SUM(duracao) FOR descricao IN(
		[pausa0]
		,[pausa1]
		,[pausa2]
		,[pausa3]
		,[pausa4]
		,[pausa5]
		,[pausa6]
		,[pausa7]
		,[pausa8]
		,[pausa9]
		,[pausa10]
		,[pausa11]
		,[pausa12]
		,[pausa13]
		,[pausa14]
		,[pausa15]
		)) AS P

	SELECT 
		S.[data]
		,S.[dbid]
		,S.agentid
		,S.fullname
		,SUM(S.tempologado) AS tempologado
		,SUM(S.tempototalpausa) AS tempototalpausa
		,SUM(S.[pausa0]) AS [pausa0]
		,SUM(S.[pausa1]) AS [pausa1]
		,SUM(S.[pausa2]) AS [pausa2]
		,SUM(S.[pausa3]) AS [pausa3]
		,SUM(S.[pausa4]) AS [pausa4]
		,SUM(S.[pausa5]) AS [pausa5]
		,SUM(S.[pausa6]) AS [pausa6]
		,SUM(S.[pausa7]) AS [pausa7]
		,SUM(S.[pausa8]) AS [pausa8]
		,SUM(S.[pausa9]) AS [pausa9]
		,SUM(S.[pausa10]) AS [pausa10]
		,SUM(S.[pausa11]) AS [pausa11]
		,SUM(S.[pausa12]) AS [pausa12]
		,SUM(S.[pausa13]) AS [pausa13]
		,SUM(S.[pausa14]) AS [pausa14]
		,SUM(S.[pausa15]) AS [pausa15]
	INTO #tLoginSumario
	FROM (

		SELECT 
			--L.sequencial
			L.[dbid]
			,L.agentid
			,L.fullname
			--,L.place
			,CAST(CAST(L.datalogin AS DATE) AS DATETIME) AS [data]
			--,L.horalogin
			--,L.horalogout
			,DATEDIFF(SECOND, L.horalogin, L.horalogout) AS tempologado 
			,(P.[pausa0] + P.[pausa1] + P.[pausa2] + P.[pausa3] + P.[pausa4] + P.[pausa5] + P.[pausa6] + P.[pausa7] + P.[pausa8] + P.[pausa9] + P.[pausa10] + P.[pausa11]
			 + P.[pausa12] + P.[pausa13] + P.[pausa14] + P.[pausa15]) AS tempototalpausa
			,P.[pausa0]
			,P.[pausa1]
			,P.[pausa2]
			,P.[pausa3]
			,P.[pausa4]
			,P.[pausa5]
			,P.[pausa6]
			,P.[pausa7]
			,P.[pausa8]
			,P.[pausa9]
			,P.[pausa10]
			,P.[pausa11]
			,P.[pausa12]
			,P.[pausa13]
			,P.[pausa14]
			,P.[pausa15]
		FROM #tCTIEventosLogin AS L
			LEFT JOIN #tPausasSumario AS P ON CAST(CAST(L.datalogin AS DATE) AS DATETIME) = P.[data] AND L.sequencial = P.eventologin
		WHERE loginativo = 0 
	
	) AS S
	GROUP BY S.[dbid], S.agentid, S.fullname, S.[data]



	SELECT 
		S.[data]
		,S.age_agentid
		,SUM(consulta) AS consultas
	INTO #tConsultaSumario
	FROM (

		SELECT 
			CAST(CAST(age_ini AS DATE) AS DATETIME) AS [data]
			,age_agentid
			,1 AS consulta
		FROM AlctelVSS..ufn_GetChamadasEmConsultas(@dataInicial, @dataFinal)

	) AS S
	GROUP BY S.[data], S.age_agentid

	SELECT 
		S.[data]
		,S.age_agentid
		,S.age_dbid
		,S.age_fullname
		,SUM(atendida) AS atendidas
		,SUM(S.tempo_conversacao) AS totalatendidas
		,SUM(S.realizada) AS realizadas
		,SUM(S.duracao_saida) AS totalsaida
		,SUM(S.consultas) AS consultas
		,SUM(S.p1n1) AS p1n1
		,SUM(S.p1n2) AS p1n2
		,SUM(S.p1n3) AS p1n3
		,SUM(S.p1n4) AS p1n4
		,SUM(S.p1n5) AS p1n5
	INTO #tChamadasSumario
	FROM (

		SELECT 
			CAST(CAST(C.cha_ini AS DATE) AS DATETIME) AS [data]
			,C.age_agentid
			,C.age_dbid
			,C.age_fullname
			,C.atendida
			,C.tempo_conversacao
			,C.realizada
			,C.duracao_saida
			,H.consultas
			,ISNULL(N.pesquisa_1, 0) AS p1n1
			,ISNULL(N.pesquisa_2, 0) AS p1n2
			,ISNULL(N.pesquisa_3, 0) AS p1n3
			,ISNULL(N.pesquisa_4, 0) AS p1n4
			,ISNULL(N.pesquisa_5, 0) AS p1n5
		FROM #tChamadas AS C
			LEFT JOIN #tNavegacao AS N ON C.cha_id = N.cha_id AND CAST(CAST(C.cha_ini AS DATE) AS DATETIME) = N.[data]
			LEFT JOIN #tConsultaSumario AS H ON C.age_agentid = H.age_agentid AND CAST(CAST(C.cha_ini AS DATE) AS DATETIME) = H.[data]

	) AS S
	GROUP BY S.[data], S.age_agentid, S.age_dbid, S.age_fullname


	SELECT
		L.fullname
		,L.[dbid]
		,L.agentid
		,L.[data]
		,ISNULL(C.p1n1, 0) AS p1n1
		,ISNULL(C.p1n2, 0) AS p1n2
		,ISNULL(C.p1n3, 0) AS p1n3
		,ISNULL(C.p1n4, 0) AS p1n4
		,ISNULL(C.p1n5, 0) AS p1n5
		,ISNULL(C.atendidas, 0) AS atendidas
		,ISNULL(C.consultas, 0) AS consultas
		,ISNULL(C.totalatendidas, 0) AS totalatendidas
		,CASE WHEN C.atendidas > 0 THEN C.totalatendidas / C.atendidas ELSE 0 END AS tma
		,ISNULL(L.tempototalpausa, 0) AS tempototalpausa
		--,ISNULL(L.[pausa0], 0) AS [pausa0]
		,ISNULL(L.[pausa1], 0) AS [pausa1]
		,ISNULL(L.[pausa2], 0) AS [pausa2]
		,ISNULL(L.[pausa3], 0) AS [pausa3]
		,ISNULL(L.[pausa4], 0) AS [pausa4]
		,ISNULL(L.[pausa5], 0) AS [pausa5]
		,ISNULL(L.[pausa6], 0) AS [pausa6]
		,ISNULL(L.[pausa7], 0) AS [pausa7]
		,ISNULL(L.[pausa8], 0) AS [pausa8]
		,ISNULL(L.[pausa9], 0) AS [pausa9]
		,ISNULL(L.[pausa10], 0) AS [pausa10]
		,ISNULL(L.[pausa11], 0) AS [pausa11]
		,ISNULL(L.[pausa12], 0) AS [pausa12]
		,ISNULL(L.[pausa13], 0) AS [pausa13]
		,ISNULL(L.[pausa14], 0) AS [pausa14]
		,ISNULL(L.[pausa15], 0) AS [pausa15]		
		,L.tempologado
		,ISNULL(C.realizadas, 0) AS chamadassaida
		,0 AS ddiscagem
		,CASE WHEN C.realizadas > 0 THEN C.totalsaida / C.realizadas ELSE 0 END AS tmasaida
		,ISNULL(C.totalsaida, 0) AS totalsaida
		,0 AS natendidas
		,0 AS ntransfer
		,0 AS atendidasD
		,0 AS totalatendidasD
		,0 AS tmaD
		,0 AS natvoz
		,0 AS totalatvoz
		,0 AS natchat
		,0 AS totalatchat
		,0 AS nemailconc
		,0 AS nemailresp
		,0 AS prepausa
		,0 AS emailRecebido
		,0 AS emailRespondido
		,0 AS emailEnviado
		,0 AS emailEncaminhado
		,0 AS emailConcluido
		,0 AS emailTempoTrIn
		,0 AS emailTratado
		,0 AS tmaEmailTempoTrIn
		,0 AS emailTempoTrOut
		,0 AS tmaEmailTempoTrOut
	FROM #tLoginSumario AS L
		LEFT JOIN #tChamadasSumario AS C ON L.[data] = C.[data] AND L.agentid = C.age_agentid AND L.fullname = C.age_fullname AND L.[dbid] = C.age_dbid
	ORDER BY L.fullname, L.[data]

END


GO


