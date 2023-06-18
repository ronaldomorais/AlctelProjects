USE AlctelVSS
GO



CREATE PROCEDURE JAVA_hiCampanhas
	@parUser VARCHAR(50) = ''
	,@dataInicial NVARCHAR(20) = '2021-01-14'
	,@dataFinal NVARCHAR(20) = '2021-01-15'
	,@parAplicacao INT = 1
	,@parSite INT
AS
BEGIN
	SET NOCOUNT ON
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE 
		@par_user VARCHAR(50) = @parUser
		,@data_inicio DATE = @dataInicial
		,@data_final DATE = @dataFinal
		,@par_aplicacao INT = @parAplicacao
		,@par_site INT = @parSite

	IF OBJECT_ID('tempdb..#tCampanhas') IS NOT NULL
		DROP TABLE #tCampanhas

	SELECT 
		CONFSERVER_OBJ_ID
		,[OBJECT_ID]
		,[OBJECT_NAME]
	INTO #tCampanhas
	FROM DB_GENESYS_DM..O_ALC_CL_DAY


	SELECT 
		C.[OBJECT_NAME] AS ColunaTipoStr01
		,CONVERT(DATETIME, D.TIME_KEY, 103) AS ColunaTipoDateTime01
		,0 AS ColunaTipoInt01
		,SUM(D.N_ANSWER) AS ColunaTipoInt02
		,0 AS ColunaTipoDouble01
		,SUM(D.N_SCHEDULED) AS ColunaTipoInt03
		,SUM(D.N_NOANSWER) AS ColunaTipoInt04
		,0 AS ColunaTipoDouble02
		--,D.N_DIALMADE
		,SUM(D.N_BUSY) AS ColunaTipoInt05
		,SUM(D.N_INVALIDNUM) AS ColunaTipoInt06
		,SUM(D.N_ANSMACHINE) AS ColunaTipoInt07
		,SUM(D.N_FAX) AS ColunaTipoInt08
		,SUM(D.N_DIALERROR) AS ColunaTipoInt09
		,SUM(D.N_ABAND) AS ColunaTipoInt10
	FROM #tCampanhas AS C 
		INNER JOIN DB_GENESYS_DM..R_ALC_CL_DAY AS D ON C.[OBJECT_ID] = D.[OBJECT_ID]
	WHERE
		TIME_KEY BETWEEN @data_inicio AND @data_final
	GROUP BY C.[OBJECT_NAME], CONVERT(DATETIME, D.TIME_KEY, 103)
	ORDER BY C.[OBJECT_NAME], CONVERT(DATETIME, D.TIME_KEY, 103)
END