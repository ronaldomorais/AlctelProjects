USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[TESTE_Alc_Paae_001_sp_hiPesquisaSatisfacaoDetalheFila_20221107_BKP]    Script Date: 08/11/2022 12:29:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




--DECLARE
--	@dataInicial VARCHAR(10) = '2022-07-25'
--	,@dataFinal VARCHAR(10)  = '2022-07-26'
--	,@parSite INT = 11

CREATE PROCEDURE [dbo].[TESTE_Alc_Paae_001_sp_hiPesquisaSatisfacaoDetalheFila_20221107_BKP] 
	@dataInicial VARCHAR(10) = '2022-03-17'
	,@dataFinal VARCHAR(10)  = '2022-03-17'
	,@parSite INT = 10
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	IF OBJECT_ID('tempdb..#tDadosDasPesquisas') IS NOT NULL
		DROP TABLE #tDadosDasPesquisas

	IF OBJECT_ID('tempdb..#tDadosDasPesquisasDetalhado') IS NOT NULL
		DROP TABLE #tDadosDasPesquisasDetalhado

	DECLARE @dataInicioInput VARCHAR(10) = @dataInicial
		,@dataTerminoInput VARCHAR(10) = @dataFinal
		,@parSiteInput INT = @parSite

	CREATE TABLE #tDadosDasPesquisas (
		cha_id VARCHAR(50) NOT NULL
		,protocolo VARCHAR(100)
		,[data] VARCHAR(10)
		,hora VARCHAR(8)
		,origem VARCHAR(50)
		,fila VARCHAR(50)
		,agentid VARCHAR(50)
		,agente_nome VARCHAR(255)
		,enviado_pesquisa VARCHAR(3)
		,respondida_pesquisa VARCHAR(3)
		,resposta_1 VARCHAR(10)
		,resposta_2 VARCHAR(10)
		)


--PROC Alc_Paae_001_sp_hiPesquisaSatisfacaoDetalhe transcrita aqui por causa de proecdures nested

	--INSERT INTO #tDadosDasPesquisas
	--EXEC Alc_Paae_001_sp_hiPesquisaSatisfacaoDetalhe @dataInicioInput, @dataTerminoInput, @parSiteInput	

	DECLARE 
		@lparSite int = @parSite,
		@ldataInicial datetime = @dataInicial,
		@ldataFinal datetime = @dataFinal

	IF OBJECT_ID('tempdb..#dadosDetalhados') IS NOT NULL
		DROP TABLE #dadosDetalhados
	IF OBJECT_ID('tempdb..#tLogChamadas') IS NOT NULL
		DROP TABLE #tLogChamadas
	IF OBJECT_ID('tempdb..#tUltimoAtendenteAntesPesquisa') IS NOT NULL
		DROP TABLE #tUltimoAtendenteAntesPesquisa
	IF OBJECT_ID('tempdb..#tOrigem') IS NOT NULL
		DROP TABLE #tOrigem
	IF OBJECT_ID('tempdb..#tResultadoProtocolo') IS NOT NULL
		DROP TABLE #tResultadoProtocolo
	IF OBJECT_ID('tempdb..#tDadosPesquisa') IS NOT NULL
		DROP TABLE #tDadosPesquisa
	IF OBJECT_ID('tempdb..#tInicioPesquisaDT') IS NOT NULL
		DROP TABLE #tInicioPesquisaDT
	IF OBJECT_ID('tempdb..#tResultadoPesquisa') IS NOT NULL
		DROP TABLE #tResultadoPesquisa



	
	
	CREATE TABLE #dadosDetalhados (
		cha_data datetime,
		cha_fila varchar(100),
		filaDbid int,
		tempo_espera int,
		Recebidas varchar(100),
		Atendidas varchar(100),
		AtendidasAte30 int,
		tempo_atendimento int,
		Abandonadas varchar(100),
		tempo_abandono int,
		AbandonadasAte5  int,
		AbandonadasAte10 int,
		AbandonadasAte15 int,
		AbandonadasAte20 int,
		AbandonadasAte30 int
	)

	
	INSERT INTO #dadosDetalhados
	EXEC AlctelVSS.dbo.CTI_REL_Alc_Paae_Sumario_Voz_Base_Aeromedica @ldataInicial, @ldataFinal, @lparSite
	

	SELECT *
	INTO #tLogChamadas
	FROM AlctelSRVCTI_Aeromedica.dbo.Alc_mtr001LogChamadas
	WHERE
		evt_connid IN(
			SELECT DISTINCT Atendidas FROM #dadosDetalhados
		)

	--Coletando última chamada antes de ser enviada à pesquisa.
	SELECT 
		evt_connid
		,evt_agentid
		,CONCAT(P.first_name, ' ', P.last_name) AS agente_nome
	INTO #tUltimoAtendenteAntesPesquisa
	FROM (

		SELECT 
			evt_connid
			,evt_agentid
			,ROW_NUMBER() OVER(PARTITION BY evt_connid ORDER BY evt_hora DESC) AS evt_seq_desc
		FROM #tLogChamadas
		WHERE
			evt_id = 64

	) AS S
		LEFT JOIN Alctel_Gen_CONFIG.dbo.cfg_person AS P ON S.evt_agentid = P.employee_id
	WHERE
		S.evt_seq_desc = 1


	--Coletando a origem da chamada.
	SELECT DISTINCT
		evt_connid
		,evt_origem
	INTO #tOrigem
	FROM (

		SELECT 
			evt_connid
			,evt_origem
		FROM #tLogChamadas
		WHERE evt_origem <> '***'

	) AS S


	--Coletando dados de protocolo
	SELECT evt_connid
		,evt_valor AS protocolo
	INTO #tResultadoProtocolo
	FROM #tLogChamadas
	--WHERE evt_dado = 'Alc_Nav_Protocolo'
	--Alteração para OptouProtPend Roberto
	WHERE evt_dado LIKE 'Alc_Nav_Protocolo%'


	--Coletando dados de pesquisa
	SELECT evt_connid
		,evt_hora
		,REPLACE(evt_dado, 'Alc_Nav_', '') AS pesquisa
		,evt_valor AS resposta
	INTO #tDadosPesquisa
	FROM #tLogChamadas
	WHERE evt_thisDN = '30550'


	--AND evt_dado LIKE 'Alc_Nav_pesquisa%'
	--Tratando dados da pesquisa
	SELECT evt_connid
		,MIN(evt_hora) AS inicio_pesquisa
	INTO #tInicioPesquisaDT
	FROM #tDadosPesquisa
	GROUP BY evt_connid


	SELECT I.evt_connid
		,I.inicio_pesquisa
		,P.pesquisa_1
		,P.pesquisa_2
	INTO #tResultadoPesquisa
	FROM (
		SELECT evt_connid
			,pesquisa
			,resposta
		FROM #tDadosPesquisa
		WHERE pesquisa LIKE 'pesquisa%'
		) AS R
	PIVOT(MAX(resposta) FOR pesquisa IN (
				[pesquisa_1]
				,[pesquisa_2]
				)) AS P
	RIGHT JOIN #tInicioPesquisaDT AS I ON P.evt_connid = I.evt_connid
	
	INSERT INTO #tDadosDasPesquisas
	SELECT 
		C.Atendidas AS cha_id
		,ISNULL(P.protocolo, '') AS protocolo
		,CONVERT(NVARCHAR(10), C.cha_data, 103) AS [data]
		,CONVERT(NVARCHAR(10), C.cha_data, 24) AS hora
		,O.evt_origem AS origem
		,C.cha_fila AS fila
		,U.evt_agentid AS agentid
		,U.agente_nome
		,CASE 
			WHEN R.inicio_pesquisa IS NOT NULL
				THEN 'SIM'
			ELSE 'NÃO'
			END AS enviado_pesquisa
		,CASE 
			WHEN R.inicio_pesquisa IS NOT NULL 
				THEN 
					CASE WHEN R.pesquisa_1 IS NOT NULL AND R.pesquisa_2 IS NOT NULL
						THEN 'SIM'
						ELSE 'NÃO'
					END
			ELSE 'N/A'
			END AS respondida_pesquisa
		,CASE 
			WHEN R.inicio_pesquisa IS NOT NULL 
				THEN 
					CASE WHEN R.pesquisa_1 IS NULL
						THEN 'N/A'
						ELSE CASE WHEN R.pesquisa_1 = 1 THEN 'SIM' ELSE 'NÃO' END
					END
			ELSE 'N/A'
			END AS resposta_1
		,CASE 
			WHEN R.inicio_pesquisa IS NOT NULL 
				THEN ISNULL(R.pesquisa_2, 'N/A')
				ELSE 'N/A'
			END AS resposta_2		
	FROM #dadosDetalhados AS C 
		LEFT JOIN #tUltimoAtendenteAntesPesquisa AS U ON C.Atendidas = U.evt_connid
		LEFT JOIN #tOrigem AS O ON C.Atendidas = O.evt_connid
		LEFT JOIN #tResultadoProtocolo AS P ON C.Atendidas = P.evt_connid
		LEFT JOIN #tResultadoPesquisa AS R ON C.Atendidas = R.evt_connid

	WHERE
		Atendidas IS NOT NULL
		--AND Atendidas = '028A0336108A0C17'
	ORDER BY C.Atendidas

--PROC Alc_Paae_001_sp_hiPesquisaSatisfacaoDetalhe transcrita aqui por causa de proecdures nested


	SELECT [data]
		,fila
		,1 AS atendida
		,CASE respondida_pesquisa 
			WHEN 'SIM' THEN 1
			ELSE 0
		END AS respondida_pesquisa
		,CASE enviado_pesquisa 
			WHEN 'SIM' THEN 1
			ELSE 0
		END AS enviado_pesquisa
		,CASE 
			WHEN resposta_1 = 'SIM'
				THEN 1
			ELSE 0
			END AS resposta_sim
		,CASE 
			WHEN resposta_1 = 'NÃO'
				THEN 1
			ELSE 0
			END AS resposta_nao
		,CASE 
			WHEN resposta_2 = '5'
				THEN 1
			ELSE 0
			END AS resposta_nota5
		,CASE 
			WHEN resposta_2 = '4'
				THEN 1
			ELSE 0
			END AS resposta_nota4
		,CASE 
			WHEN resposta_2 = '3'
				THEN 1
			ELSE 0
			END AS resposta_nota3
		,CASE 
			WHEN resposta_2 = '2'
				THEN 1
			ELSE 0
			END AS resposta_nota2
		,CASE 
			WHEN resposta_2 = '1'
				THEN 1
			ELSE 0
			END AS resposta_nota1
	INTO #tDadosDasPesquisasDetalhado
	FROM #tDadosDasPesquisas

	
	SELECT 
		S.[data]
		--,CASE WHEN S.[data] = 'SubTotal' THEN '' ELSE S.fila END AS fila
		,fila
		,S.atendidas
		,S.enviados_pesquisa
		,S.nota_pergunta_1
		,S.nota_pergunta_1_per
		,S.nota_pergunta_2
		,S.nota_pergunta_2_per
		,S.nota_geral_per
	FROM (

	SELECT 'TOTAL' AS [data]
		,'' AS fila
		,SUM(atendida) AS atendidas
		,SUM(enviado_pesquisa) AS enviados_pesquisa
		,CONCAT (
			'SIM: '
			,SUM(resposta_sim)
			,' / NÃO: '
			,SUM(resposta_nao)
			) AS nota_pergunta_1
		,CONCAT (
			'SIM: '
			,CASE 
				WHEN SUM(respondida_pesquisa) > 0
					THEN ROUND(CAST(SUM(resposta_sim) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
				ELSE 0
				END
			,'% / NÃO: '
			,CASE 
				WHEN SUM(respondida_pesquisa) > 0
					THEN ROUND(CAST(SUM(resposta_nao) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
				ELSE 0
				END
			, '%') AS nota_pergunta_1_per
		,CONCAT (
			'NOTA 1: '
			,SUM(resposta_nota1)
			,' / NOTA 2: '
			,SUM(resposta_nota2)
			,' / NOTA 3: '
			,SUM(resposta_nota3)
			,' / NOTA 4: '
			,SUM(resposta_nota4)
			,' / NOTA 5: '
			,SUM(resposta_nota5)
			) AS nota_pergunta_2
		,CONCAT (
			' NOTA 1: '
			,CASE 
				WHEN SUM(respondida_pesquisa) > 0
					THEN ROUND(CAST(SUM(resposta_nota1) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
				ELSE 0
				END
			,'% / NOTA 2: '
			,CASE 
				WHEN SUM(respondida_pesquisa) > 0
					THEN ROUND(CAST(SUM(resposta_nota2) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
				ELSE 0
				END
			,'% / NOTA 3: '
			,CASE 
				WHEN SUM(respondida_pesquisa) > 0
					THEN ROUND(CAST(SUM(resposta_nota3) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
				ELSE 0
				END
			,'% / NOTA 4: '
			,CASE 
				WHEN SUM(respondida_pesquisa) > 0
					THEN ROUND(CAST(SUM(resposta_nota4) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
				ELSE 0
				END
			,'% / NOTA 5: '
			,CASE 
				WHEN SUM(respondida_pesquisa) > 0
					THEN ROUND(CAST(SUM(resposta_nota5) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
				ELSE 0
				END
			, '%') AS nota_pergunta_2_per
		,CASE WHEN CAST((SUM(resposta_nota1) + SUM(resposta_nota2) + SUM(resposta_nota3) + SUM(resposta_nota4) + SUM(resposta_nota5)) AS FLOAT) > 0
			THEN
				ROUND((
					CAST((SUM(resposta_nota3) + SUM(resposta_nota4) + SUM(resposta_nota5)) AS FLOAT) / 
					CAST((SUM(resposta_nota1) + SUM(resposta_nota2) + SUM(resposta_nota3) + SUM(resposta_nota4) + SUM(resposta_nota5)) AS FLOAT)  ) * 100, 2) 
			ELSE 0
			END AS nota_geral_per
			,0 AS ordem
	FROM #tDadosDasPesquisasDetalhado
	WHERE
		fila IS NOT NULL AND fila <> '***'

	UNION ALL

		SELECT [data]
			,fila
			,SUM(atendida) AS atendidas
			,SUM(enviado_pesquisa) AS enviados_pesquisa
			,CONCAT (
				'SIM: '
				,SUM(resposta_sim)
				,' / NÃO: '
				,SUM(resposta_nao)
				) AS nota_pergunta_1
			,CONCAT (
				'SIM: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_sim) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NÃO: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nao) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				, '%') AS nota_pergunta_1_per
			,CONCAT (
				'NOTA 1: '
				,SUM(resposta_nota1)
				,' / NOTA 2: '
				,SUM(resposta_nota2)
				,' / NOTA 3: '
				,SUM(resposta_nota3)
				,' / NOTA 4: '
				,SUM(resposta_nota4)
				,' / NOTA 5: '
				,SUM(resposta_nota5)
				) AS nota_pergunta_2
			,CONCAT (
				' NOTA 1: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota1) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NOTA 2: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota2) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NOTA 3: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota3) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NOTA 4: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota4) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NOTA 5: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota5) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				, '%') AS nota_pergunta_2_per
			,CASE WHEN CAST((SUM(resposta_nota1) + SUM(resposta_nota2) + SUM(resposta_nota3) + SUM(resposta_nota4) + SUM(resposta_nota5)) AS FLOAT) > 0
				THEN
					ROUND((
						CAST((SUM(resposta_nota3) + SUM(resposta_nota4) + SUM(resposta_nota5)) AS FLOAT) / 
						CAST((SUM(resposta_nota1) + SUM(resposta_nota2) + SUM(resposta_nota3) + SUM(resposta_nota4) + SUM(resposta_nota5)) AS FLOAT)  ) * 100, 2) 
				ELSE 0
				END AS nota_geral_per
				,2 AS Ordem
		FROM #tDadosDasPesquisasDetalhado
		GROUP BY [data]
			,fila
		HAVING fila IS NOT NULL AND fila <> '***'
	
		UNION ALL

		SELECT 'SubTotal' AS [data]
			,fila
			,SUM(atendida) AS atendidas
			,SUM(enviado_pesquisa) AS enviados_pesquisa
			,CONCAT (
				'SIM: '
				,SUM(resposta_sim)
				,' / NÃO: '
				,SUM(resposta_nao)
				) AS nota_pergunta_1
			,CONCAT (
				'SIM: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_sim) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NÃO: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nao) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				, '%') AS nota_pergunta_1_per
			,CONCAT (
				'NOTA 1: '
				,SUM(resposta_nota1)
				,' / NOTA 2: '
				,SUM(resposta_nota2)
				,' / NOTA 3: '
				,SUM(resposta_nota3)
				,' / NOTA 4: '
				,SUM(resposta_nota4)
				,' / NOTA 5: '
				,SUM(resposta_nota5)
				) AS nota_pergunta_2
			,CONCAT (
				' NOTA 1: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota1) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NOTA 2: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota2) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NOTA 3: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota3) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NOTA 4: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota4) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				,'% / NOTA 5: '
				,CASE 
					WHEN SUM(respondida_pesquisa) > 0
						THEN ROUND(CAST(SUM(resposta_nota5) AS FLOAT) / CAST(SUM(respondida_pesquisa) AS FLOAT) * 100, 2)
					ELSE 0
					END
				, '%') AS nota_pergunta_2_per
			,CASE WHEN CAST((SUM(resposta_nota1) + SUM(resposta_nota2) + SUM(resposta_nota3) + SUM(resposta_nota4) + SUM(resposta_nota5)) AS FLOAT) > 0
				THEN
					ROUND((
						CAST((SUM(resposta_nota3) + SUM(resposta_nota4) + SUM(resposta_nota5)) AS FLOAT) / 
						CAST((SUM(resposta_nota1) + SUM(resposta_nota2) + SUM(resposta_nota3) + SUM(resposta_nota4) + SUM(resposta_nota5)) AS FLOAT)  ) * 100, 2) 
				ELSE 0
				END AS nota_geral_per
				,1 AS Ordem
		FROM #tDadosDasPesquisasDetalhado
		GROUP BY fila
		HAVING fila IS NOT NULL AND fila <> '***'
	) AS S
	
	ORDER BY S.fila, S.ordem, [Data]
END
GO


