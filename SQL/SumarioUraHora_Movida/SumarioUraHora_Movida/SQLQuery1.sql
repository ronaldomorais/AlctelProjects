
IF OBJECT_ID('tempdb..#tChamadasUra') IS NOT NULL
	DROP TABLE #tChamadasUra
IF OBJECT_ID('tempdb..#tFila') IS NOT NULL
	DROP TABLE #tFila

SELECT     
	U.cha_id 
	,ROW_NUMBER() OVER(PARTITION BY U.cha_id ORDER BY U.cha_ini) AS cha_seq
	,U.cha_ini AS [data] 
	,D.DN
	,U.cha_origem AS origem 
	,D.ura
	,U.cha_ini AS ura_inicio 
	,U.cha_fim AS ura_fim
	--,fila
	--,cha_infila
	--,cha_outfila
	--,agentid
	--,desconexao_inicio
	,CASE WHEN U.evt_desconexao = -1 THEN 1 ELSE 0 END AS desconexao
	--,[site] 
	--,siteid	
INTO #tChamadasUra	
FROM [AlctelSRVCTI].[dbo].[Alc_chamadasGeralUra] AS U WITH (NOLOCK)
	LEFT JOIN AlctelVSS.dbo.Alc_UraDN AS D ON U.evt_thisDN = D.DN
	--LEFT JOIN AlctelSRVCTI.dbo.Alc_chamadasGeralFilas AS F ON U.cha_id = F.cha_id
WHERE
	U.cha_ini BETWEEN '2023-01-09 00:00:00' AND '2023-01-09 23:59:59'


SELECT 
	S.cha_id
	,S.cha_fila
	,S.cha_inFila
	,S.cha_outFila
INTO #tFila
FROM (

	SELECT 
		cha_id
		,cha_fila
		,cha_inFila
		,cha_outFila
		,ROW_NUMBER() OVER(PARTITION BY cha_id ORDER BY cha_inFila) AS cha_seq
	FROM AlctelSRVCTI.dbo.Alc_chamadasGeralFilas WITH (NOLOCK)
	WHERE
		cha_inFila BETWEEN '2023-01-09 00:00:00' AND '2023-01-09 23:59:59'

) AS S
WHERE S.cha_seq = 1

SELECT 
	U.cha_id 
	,U.cha_seq
	,U.[data] 
	,U.DN
	,U.origem 
	,U.ura
	,U.ura_inicio 
	,U.ura_fim
	,F.cha_fila 
	,F.cha_infila
	,F.cha_outfila
	--,agentid
	--,desconexao_inicio
	,U.desconexao
	,FL.[site] 
	,S.sequencial AS siteid	
FROM #tChamadasUra AS U
	LEFT JOIN #tFila AS F ON U.cha_id = F.cha_id
	LEFT JOIN AlctelVSS..Alc_mtr001Filas AS FL ON F.cha_fila = FL.fila
	LEFT JOIN AlctelVSS..Alc_mtr001Sites AS S ON FL.[site] = S.[site]
