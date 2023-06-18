USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[TESTE_Alc_Paae_001_sp_hiAbandonadasX]    Script Date: 22/03/2023 11:07:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[TESTE_Alc_Paae_001_sp_hiAbandonadasX]   
	@dataInicial datetime = '2023-01-10',  
	@dataFinal datetime   = '2023-01-10',   
	@parSite int = 0
AS 
BEGIN


	IF OBJECT_ID('tempdb..#tChamadas') IS NOT NULL
		DROP TABLE #tChamadas

	IF OBJECT_ID('tempdb..#tNovoContato') IS NOT NULL
		DROP TABLE #tNovoContato

	IF OBJECT_ID('tempdb..#tCpf') IS NOT NULL
		DROP TABLE #tCpf


	SELECT 
		cha_id
		,ROW_NUMBER() OVER(ORDER BY cha_ini) AS sequencial
		,cha_ini AS [data]
		,cha_origem AS origem
		,'Copass' AS marca
		,cha_fila AS fila
		,cha_inFila AS infila
		,cha_outFila AS outfila
		,DATEDIFF(SECOND, cha_inFila, cha_outFila) AS tempofila
		,age_agentid
		,age_ini
		,status_ligacao
		,ROW_NUMBER() OVER(PARTITION BY cha_origem ORDER BY cha_ini) AS qtd_contatos
	INTO #tChamadas
	FROM AlctelVSS..ufn_GetChamadas (@dataInicial, @dataFinal, @parSite)
	WHERE
		cha_origem <> '***'
		--cha_origem = '3125213333'


	SELECT 
		cha_id
		,origem
		,status_ligacao
		,CASE WHEN status_ligacao = 'ABANDONADA'
			THEN 
				CASE WHEN LEAD(status_ligacao, 1, '') OVER(PARTITION BY origem ORDER BY qtd_contatos) = 'ATENDIDA'
					THEN LEAD(age_ini, 1, '') OVER(PARTITION BY origem ORDER BY qtd_contatos)
					ELSE NULL
				END
		END AS novocontato
		,CASE WHEN status_ligacao = 'ABANDONADA'
			THEN 
				CASE WHEN LEAD(status_ligacao, 1, '') OVER(PARTITION BY origem ORDER BY qtd_contatos) = 'ATENDIDA'
					THEN LEAD(age_agentid, 1, '') OVER(PARTITION BY origem ORDER BY qtd_contatos)
					ELSE NULL
				END
		END AS age_agentid	
	INTO #tNovoContato
	FROM (

		SELECT
			cha_id 
			,origem
			,status_ligacao
			,age_agentid
			,age_ini
			,qtd_contatos
		FROM #tChamadas
		WHERE
			status_ligacao IN('ABANDONADA', 'ATENDIDA')

	) AS S

	DELETE FROM #tNovoContato WHERE novocontato IS NULL

	SELECT 
		cha_id
		,[dado] AS cpf
	INTO #tCpf
	FROM AlctelSRVCTI..Alc_Navegacao WITH (NOLOCK)
	WHERE
		cha_id IN (
			SELECT DISTINCT cha_id FROM #tChamadas
		)
		AND interacao = 'CPF'


	SELECT 
		C.cha_id
		,C.sequencial
		,C.[data]
		,C.origem
		,C.marca
		,C.fila
		,C.infila
		,C.outfila
		,C.tempofila
		,N.novocontato
		,NULL AS callback
		,N.age_agentid
		,F.cpf
	FROM #tChamadas AS C
		LEFT JOIN #tNovoContato AS N ON C.cha_id = N.cha_id
		LEFT JOIN #tCpf AS F ON C.cha_id = F.cha_id
	WHERE
		C.status_ligacao = 'ABANDONADA'

END


GO


