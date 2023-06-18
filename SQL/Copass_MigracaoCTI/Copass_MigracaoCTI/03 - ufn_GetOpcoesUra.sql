USE [AlctelVSS]
GO

/****** Object:  UserDefinedFunction [dbo].[ufn_GetOpcoesUra]    Script Date: 15/03/2023 14:53:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER FUNCTION [dbo].[ufn_GetOpcoesUra](		
	@dataInicio DATETIME = '2023-02-13'
	,@dataFinal DATETIME = '2023-02-14'
	,@tipoResultado VARCHAR(20) = 'NAO_CONCATENADO' --NAO_CONCATENADO OU CONCATENADO
)
RETURNS @tOpcoesUra TABLE (
	tipo_resultado VARCHAR(20)
	,cha_id VARCHAR(50)
	,cha_opcoes_ura VARCHAR(MAX)
	,[data] DATE
	,opcao_1 INT
	,opcao_2 INT
	,opcao_3 INT	
	,sm_op0 INT
	,sm_op1 INT
	,sm_op2 INT
	,sm_op3 INT
	,sm_op4 INT
	,sm_op5 INT
)
--AS
BEGIN
	DECLARE
		@inicio_GMT_dt DATETIME = DATEADD(HOUR, 3, CAST(CAST(@dataInicio AS DATE) AS DATETIME))
		,@fim_GMT_dt DATETIME = DATEADD(HOUR, 3, CAST(CAST(@dataFinal AS DATE) AS DATETIME))
		,@tipo_resultado VARCHAR(20) = @tipoResultado

	SET @fim_GMT_dt = DATEADD(SECOND, 86399, @fim_GMT_dt)

	DECLARE @tOpcoes TABLE (cha_id VARCHAR(50), cha_opcoes VARCHAR(1024), cha_opcoes_nivel INT, [data] DATETIME)

	INSERT INTO @tOpcoes
	SELECT 
		cha_id
		,cha_opcoes
		,MIN(cha_opcoes_nivel) AS cha_opcoes_nivel
		,CAST(opcao_dt AS DATE) AS [data]
	FROM (

		SELECT 
			UPPER(C.CONNID) AS cha_id
			,DATEADD(HOUR, -3, U.ADDED) AS opcao_dt
			,U.KEYNAME
			,U.[VALUE] AS cha_opcoes
			,ROW_NUMBER() OVER(PARTITION BY C.CONNID ORDER BY U.ADDED) AS cha_opcoes_nivel
		FROM [Alctel_Gen_ICON].[dbo].[G_USERDATA_HISTORY] AS U WITH (NOLOCK)
			LEFT JOIN Alctel_Gen_ICON.dbo.G_CALL AS C WITH (NOLOCK) ON U.CALLID = C.CALLID
		WHERE 
			U.KEYNAME = 'Alc_Menu'
			AND U.ADDED BETWEEN @inicio_GMT_dt AND @fim_GMT_dt
		--GROUP BY C.CONNID, U.[VALUE], U.KEYNAME

	) AS S
	GROUP BY S.cha_id, cha_opcoes, CAST(opcao_dt AS DATE)
	ORDER BY S.cha_id	

	IF (@tipo_resultado = 'NAO_CONCATENADO')
	BEGIN

		INSERT INTO @tOpcoesUra(tipo_resultado, [data], opcao_1, opcao_2, opcao_3, sm_op0, sm_op1, sm_op2, sm_op3, sm_op4, sm_op5) 
		SELECT
			@tipo_resultado AS tipo_resultado 
			,CAST(P.[data] AS DATE) AS [data]
			,ISNULL(P.Opcao_1, 0) AS opcao_1
			,ISNULL(P.Opção_2, 0) AS opcao_2
			,ISNULL(P.Opção_3, 0) AS opcao_3
			,ISNULL(P.Sub_Menu_Op0, 0) AS sm_op0
			,ISNULL(P.Sub_Menu_Op1, 0) AS sm_op1
			,ISNULL(P.Sub_Menu_Op2, 0) AS sm_op2
			,ISNULL(P.Sub_Menu_Op3, 0) AS sm_op3
			,ISNULL(P.Sub_Menu_Op4, 0) AS sm_op4
			,ISNULL(P.Sub_Menu_Op5, 0) AS sm_op5
		FROM (

			SELECT 
				[data]
				,cha_opcoes
				,1 AS qtd
			FROM @tOpcoes

		) AS S
		PIVOT (SUM(qtd) FOR cha_opcoes IN([Opcao_1], [Opção_2], [Opção_3], [Sub_Menu_Op0], [Sub_Menu_Op1], [Sub_Menu_Op2], [Sub_Menu_Op3], [Sub_Menu_Op4], [Sub_Menu_Op5])) AS P

	END

	IF (@tipo_resultado = 'CONCATENADO')
	BEGIN

		INSERT INTO @tOpcoesUra(tipo_resultado, cha_id, cha_opcoes_ura)
		SELECT
			@tipo_resultado
			,P.cha_id
			,CONCAT(
				ISNULL(P.[1], ''), CASE WHEN P.[1] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[2], ''), CASE WHEN P.[2] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[3], ''), CASE WHEN P.[3] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[4], ''), CASE WHEN P.[4] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[5], ''), CASE WHEN P.[5] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[6], ''), CASE WHEN P.[6] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[7], ''), CASE WHEN P.[7] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[8], ''), CASE WHEN P.[8] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[9], ''), CASE WHEN P.[9] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[10], ''), CASE WHEN P.[10] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[11], ''), CASE WHEN P.[11] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[12], ''), CASE WHEN P.[12] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[13], ''), CASE WHEN P.[13] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[14], ''), CASE WHEN P.[14] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[15], ''), CASE WHEN P.[15] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[16], ''), CASE WHEN P.[16] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[17], ''), CASE WHEN P.[17] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[18], ''), CASE WHEN P.[18] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[19], ''), CASE WHEN P.[19] IS NOT NULL THEN '; ' ELSE '' END
				,ISNULL(P.[20], ''), CASE WHEN P.[20] IS NOT NULL THEN '; ' ELSE '' END
			) AS cha_opcoes	
		FROM @tOpcoes
		PIVOT(MAX(cha_opcoes) FOR cha_opcoes_nivel IN([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20])) AS P

	END
	RETURN
END
GO


