USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[TESTE_Alc_Paae_001_sp_hiSumario001Intervalo]    Script Date: 15/03/2023 14:33:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[TESTE_Alc_Paae_001_sp_hiSumario001Intervalo]   
	@dataInicial datetime = '2023-02-13',  
	@dataFinal datetime   = '2023-02-14',   
	@cha_fila varchar(MAX) = 'VQ_Call_Center'
	--@parSite int = 0
AS 
BEGIN

	DECLARE @site_id INT
	DECLARE @tbl_filas TABLE(fila VARCHAR(100))
	DECLARE @filas VARCHAR(MAX) = ''
	DECLARE @delimiter VARCHAR(2) = ';'
	
	SET @site_id = 0 --Para trazer todas as informaçoes no relatorios de chamadas

	IF OBJECT_ID('tempdb..#tChamadas') IS NOT NULL
		DROP TABLE #tChamadas




	SET @filas = @cha_fila + @delimiter
	WHILE (CHARINDEX(@delimiter, @filas) > 0)
	BEGIN
		INSERT INTO @tbl_filas
		SELECT SUBSTRING(@filas, 0, CHARINDEX(@delimiter, @filas, 0))
		SET @filas = SUBSTRING(@filas, CHARINDEX(@delimiter, @filas, 0) + 1, LEN(@filas))
	END


	SELECT 
		cha_fila
		,fila_dbid
		--,cha_ini
		,CAST(CASE WHEN DATEPART(HOUR, cha_ini) < 10 
			THEN CONCAT(CAST(cha_ini AS DATE), ' 0', DATEPART(HOUR, cha_ini), ':00:00')
			ELSE CONCAT(CAST(cha_ini AS DATE), ' ', DATEPART(HOUR, cha_ini), ':00:00')
		END AS DATETIME) AS horario
		,CASE WHEN DATEPART(HOUR, cha_ini) < 10 
			THEN CONCAT('0', DATEPART(HOUR, cha_ini), ':00 - 0', DATEPART(HOUR, cha_ini), ':59')
			ELSE CONCAT(DATEPART(HOUR, cha_ini), ':00 - ', DATEPART(HOUR, cha_ini), ':59')
		END AS timeslot
		,1 AS Recebidas
		,CASE WHEN status_ligacao = 'ATENDIDA' THEN 1 ELSE 0 END AS Atendidas
		,CASE WHEN status_ligacao = 'ABANDONADA' THEN 1 ELSE 0 END AS Abandonadas
		,CASE WHEN status_ligacao = 'ATENDIDA'
			THEN 
				CASE WHEN cha_inFila > cha_outFila
					THEN 0
					ELSE DATEDIFF(SECOND, cha_inFila, cha_outFila)
				END
			ELSE 0
		END AS tempo_espera
		,CASE WHEN status_ligacao = 'ABANDONADA'
			THEN 
				CASE WHEN cha_inFila > cha_outFila
					THEN 0
					ELSE DATEDIFF(SECOND, cha_inFila, cha_outFila)
				END
			ELSE 0
		END AS tempo_abandono
		,CASE WHEN age_agentid IS NULL
			THEN 0
			ELSE 
				CASE WHEN age_ini > age_fim
					THEN 0
					ELSE DATEDIFF(SECOND, age_ini, age_fim)
				END
		END AS tempo_atendimento
		,status_ligacao
		,[site]
	INTO #tChamadas
	FROM AlctelVSS..ufn_GetChamadas (@dataInicial, @dataFinal, @site_id)
	WHERE
		cha_tipo_chamada = 'ENTRADA'
		AND fila_dbid IS NOT NULL
		AND cha_origem <> '***'
	
	SELECT
		S.cha_fila
		--,S.horario
		,S.timeslot
		--,S.fila_dbid AS [dbid]
		,SUM(S.Atendidas) AS atendidas
		,SUM(S.AtendidasAte10) AS atendidasNS10
		,SUM(S.AtendidasAte20) AS atendidasNS20
		,SUM(S.AtendidasAte30) AS atendidasNS30
		,SUM(S.AtendidasAte40) AS atendidasNS40
		,SUM(S.AtendidasAte50) AS atendidasNS50
		,SUM(S.AtendidasAte60) AS atendidasNS60
		,SUM(S.AtendidasAte70) AS atendidasNS70
		,SUM(S.AtendidasAte80) AS atendidasNS80
		,SUM(S.AtendidasAte90) AS atendidasNS90
		,SUM(S.Recebidas) AS recebidas
		,SUM(S.tempo_abandono) AS tempoAbandono
		,SUM(S.tempo_espera) AS tempoEspera
		--,CASE WHEN SUM(S.Atendidas) = 0 THEN 0 ELSE SUM(S.tempo_atendimento)/SUM(S.Atendidas) END as tma
		,SUM(S.tempo_atendimento) AS tma
		,SUM(S.AbandonadasAte5) AS abandonadasNS05
		,0 AS realizadas
		,0 AS tmar
		,SUM(S.AtendidasAte45) AS atendidasNS45
		,SUM(S.AbandonadasAte30) AS AbandonadasAte30
		,SUM(S.AbandonadasAte45) AS AbandonadasAte45
		,SUM(S.AbandonadasAte60) AS AbandonadasAte60
		,SUM(S.AbandonadasAte90) AS AbandonadasAte90
		,SUM(S.AtendidasAte30) AS atendidasNS
		,SUM(S.Recebidas) AS recebidasNS

		--,SUM(S.Abandonadas) AS qtdAbandonadas
		--,CASE WHEN SUM(S.Abandonadas) = 0 THEN 0 ELSE SUM(S.tempo_abandono)/SUM(S.Abandonadas) END as TMAb
		--,CASE WHEN SUM(S.Atendidas) = 0 THEN 0 ELSE SUM(S.tempo_espera)/SUM(S.Atendidas) END as TME
		--,CAST(CASE WHEN SUM(S.Atendidas) = 0 THEN 0 ELSE 100.0*((1.0 * SUM(S.AtendidasAte30))/SUM(S.Atendidas)) END AS DECIMAL(18,2)) as NS
		--,CAST(CASE WHEN SUM(S.Recebidas) = 0 THEN 0 ELSE (SUM(S.Atendidas)*100.0)/SUM(S.Recebidas) END AS DECIMAL(18,2)) as PorcAtendidas
		--,CAST(CASE WHEN SUM(S.Atendidas) = 0 THEN 0 ELSE 100.0*((1.0 * SUM(S.AtendidasAte30))/SUM(S.Atendidas)) END AS DECIMAL(18,2)) as PorcAtendidasAte30
		--,CAST(CASE WHEN SUM(S.Recebidas) = 0 THEN 0 ELSE SUM(S.Abandonadas)*100.0/SUM(S.Recebidas) END AS DECIMAL(18,2)) AS PorcAbandono
		--,CAST(CASE WHEN SUM(S.Recebidas) = 0 THEN 0 ELSE SUM(S.AbandonadasAte5)*100.0/SUM(S.Recebidas) END AS DECIMAL(18,2)) AS PorcAbandonoAte5
		--,CAST(CASE WHEN SUM(S.Recebidas) = 0 THEN 0 ELSE SUM(S.AbandonadasAte10)*100.0/SUM(S.Recebidas) END AS DECIMAL(18,2)) AS PorcAbandonoAte10
		--,CAST(CASE WHEN SUM(S.Recebidas) = 0 THEN 0 ELSE SUM(S.AbandonadasAte15)*100.0/SUM(S.Recebidas) END AS DECIMAL(18,2)) AS PorcAbandonoAte15
		--,CAST(CASE WHEN SUM(S.Recebidas) = 0 THEN 0 ELSE SUM(S.AbandonadasAte20)*100.0/SUM(S.Recebidas) END AS DECIMAL(18,2)) AS PorcAbandonoAte20
		--,[site]
	FROM (

		SELECT 
			cha_fila
			--,fila_dbid
			--,horario
			,timeslot
			,Recebidas
			,Atendidas
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 10
				THEN 1 
				ELSE 0
			END AS AtendidasAte10
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 20
				THEN 1 
				ELSE 0
			END AS AtendidasAte20
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 30
				THEN 1 
				ELSE 0
			END AS AtendidasAte30
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 40
				THEN 1 
				ELSE 0
			END AS AtendidasAte40
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 45
				THEN 1 
				ELSE 0
			END AS AtendidasAte45
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 50
				THEN 1 
				ELSE 0
			END AS AtendidasAte50
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 60
				THEN 1 
				ELSE 0
			END AS AtendidasAte60
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 70
				THEN 1 
				ELSE 0
			END AS AtendidasAte70
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 80
				THEN 1 
				ELSE 0
			END AS AtendidasAte80
			,CASE WHEN status_ligacao = 'ATENDIDA' AND tempo_espera <= 90
				THEN 1 
				ELSE 0
			END AS AtendidasAte90
			,Abandonadas
			,CASE WHEN status_ligacao = 'ABANDONADA' AND tempo_abandono <= 5
				THEN 1 
				ELSE 0
			END AS AbandonadasAte5
			,CASE WHEN status_ligacao = 'ABANDONADA' AND tempo_abandono <= 30
				THEN 1 
				ELSE 0
			END AS AbandonadasAte30
			,CASE WHEN status_ligacao = 'ABANDONADA' AND tempo_abandono <= 45
				THEN 1 
				ELSE 0
			END AS AbandonadasAte45
			,CASE WHEN status_ligacao = 'ABANDONADA' AND tempo_abandono <= 60
				THEN 1
				ELSE 0
			END AS AbandonadasAte60
			,CASE WHEN status_ligacao = 'ABANDONADA' AND tempo_abandono <= 90
				THEN 1
				ELSE 0
			END AS AbandonadasAte90
			,tempo_atendimento
			,tempo_espera
			,tempo_abandono
			--,[site]
		FROM #tChamadas
		WHERE
			status_ligacao IN('ATENDIDA', 'ABANDONADA')

	) AS S
	WHERE
		S.cha_fila IN (SELECT fila FROM @tbl_filas)
	GROUP BY S.cha_fila, S.timeslot
END








GO


