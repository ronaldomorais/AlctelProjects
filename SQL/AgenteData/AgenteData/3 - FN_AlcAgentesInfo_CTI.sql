--DECLARE 
--	@tenant_dbid NUMERIC(10,0) = 111
--	,@data_inicio DATETIME = '2022-12-21 00:00:00'
--	,@data_fim DATETIME = '2022-12-21 23:59:59'
--	,@site_id INT = 0

CREATE FUNCTION [dbo].[ufn_AlcAgentesInfo_CTI] (
	@tenant_dbid NUMERIC(10,0)
	,@data_inicio DATETIME
	,@data_fim DATETIME
	,@site_id INT
)
RETURNS
	@tAgentesInfo TABLE (
		session_id VARCHAR(255)
		,agente_login_seq INT
		,agente_nome VARCHAR(255)
		,agente_id VARCHAR(255)
		,agente_dbid NUMERIC(10,0)
		,agente_place VARCHAR(255)
		,inicio_login DATETIME
		,termino_login DATETIME
		,pausa VARCHAR(255)
		,pausa_cod VARCHAR(255)
		,pausa_inicio DATETIME
		,pausa_fim DATETIME
	)
AS
BEGIN
	DECLARE
		@_data_inicio DATETIME = @data_inicio
		,@_data_fim DATETIME = @data_fim
		,@_site_id INT = @site_id

	DECLARE @tAgenteInfo TABLE (
		agente_nome VARCHAR(255)
		,agente_id VARCHAR(255)
		,agente_dbid NUMERIC(10,0)
	)

	DECLARE @tLoginInfo TABLE (
		sequencial BIGINT
		,agente_id VARCHAR(50)
		,agente_place VARCHAR(50)
		,inicio_login DATETIME
		,termino_login DATETIME
	)


	DECLARE @tPausasInfo TABLE (
		sequencial VARCHAR(255)
		,pausa VARCHAR(255)
		,pausa_cod VARCHAR(255)
		,pausa_inicio DATETIME
		,pausa_fim DATETIME
		,pausaativa INT
		,prepausa INT
	)

	DECLARE @tSkillsInfo TABLE (
		agente_dbid NUMERIC(10, 0)
		,skill VARCHAR(255)
		,skill_dbid NUMERIC(10, 0)
		,skill_level INT
	)

	DECLARE @tSitesFilasRelacionamento TABLE (
		fila VARCHAR(50)
		,[site] VARCHAR(50)
		,site_id INT
	)

	DECLARE @tAgentesFiltradoPorSite TABLE (
		agente_dbid NUMERIC(10, 0)
		,fila VARCHAR(50)
		,[site] VARCHAR(50)
		,site_id INT
	)

	INSERT INTO @tAgenteInfo
	SELECT 
		CONCAT(first_name, ' ', last_name) AS agente_nome
		,employee_id AS agente_id
		,[dbid] AS agente_dbid
	FROM Alctel_Gen_CONFIG.dbo.cfg_person WITH (NOLOCK)
	WHERE
		tenant_dbid = @tenant_dbid


	INSERT INTO @tLoginInfo
	SELECT 
		sequencial
		,agentId AS agente_id
		,placeName AS agente_place
		,[login] AS inicio_login
		,logout AS termino_login
	FROM AlctelSRVCTI_Aeromedica.dbo.Alc_mtrEventosLogin WITH (NOLOCK)
	WHERE
		[login] BETWEEN @_data_inicio AND @_data_fim


	INSERT INTO @tPausasInfo
	SELECT 
		P.eventoLogin AS sequencial
		,C.[name] AS pausa
		,P.reason AS pausa_cod 
		,P.inicio AS pausa_inicio
		,P.fim AS pausa_fim
		,pausaativa
		,prepausa
	FROM AlctelSRVCTI_Aeromedica.dbo.Alc_mtrEventosPausa AS P WITH (NOLOCK)
		LEFT JOIN Alctel_Gen_CONFIG.dbo.cfg_action_code AS C WITH (NOLOCK) ON P.reason = C.code
	WHERE
		eventoLogin IN(SELECT sequencial FROM @tLoginInfo)

	IF (@_site_id > 0)
	BEGIN

		INSERT INTO @tSkillsInfo
		SELECT
			L.person_dbid AS agente_dbid
			,S.[name] AS skill
			,S.[dbid] AS skill_dbid
			,L.level_ AS skill_level
		FROM Alctel_Gen_CONFIG.dbo.cfg_skill AS S WITH (NOLOCK)
			LEFT JOIN Alctel_Gen_CONFIG.dbo.cfg_skill_level AS L WITH (NOLOCK) ON S.[dbid] = L.skill_dbid
		WHERE tenant_dbid = @tenant_dbid

		INSERT INTO @tSitesFilasRelacionamento
		SELECT 
			F.fila
			,F.[site]
			,S.sequencial AS site_id
		FROM AlctelVSS.dbo.Alc_mtr001Filas AS F WITH (NOLOCK)
			INNER JOIN AlctelVSS.dbo.Alc_mtr001Sites AS S WITH (NOLOCK) ON F.[site] = S.[site]
		WHERE	
			S.sequencial = @_site_id

		INSERT INTO @tAgentesFiltradoPorSite
		SELECT 
			S.agente_dbid
			,SF.fila
			,SF.[site]
			,SF.site_id
		FROM @tSitesFilasRelacionamento AS SF
			INNER JOIN @tSkillsInfo AS S ON SF.fila = S.skill
		WHERE
			site_id = @_site_id

	END

	INSERT INTO @tAgentesInfo
	SELECT 
		L.sequencial
		,ROW_NUMBER() OVER(PARTITION BY A.agente_id, L.inicio_login ORDER BY L.inicio_login) AS agente_login_seq
		,A.agente_nome
		,A.agente_id
		,A.agente_dbid
		,L.agente_place
		,L.inicio_login
		,L.termino_login
		,P.pausa
		,P.pausa_cod
		,P.pausa_inicio
		,P.pausa_fim
	FROM @tLoginInfo AS L
		LEFT JOIN @tAgenteInfo AS A ON L.agente_id = A.agente_id
		LEFT JOIN @tPausasInfo AS P ON L.sequencial = P.sequencial
	WHERE
		(@_site_id = 0 OR A.agente_dbid IN(SELECT agente_dbid FROM @tAgentesFiltradoPorSite))
	ORDER BY A.agente_nome, L.inicio_login

	RETURN
END


