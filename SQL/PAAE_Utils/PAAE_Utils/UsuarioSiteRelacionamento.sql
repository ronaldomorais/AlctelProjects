DECLARE
	@usuario_id INT
	,@usuario_nome VARCHAR(100)
	,@usuario_ultima_atualizacao DATETIME 
	,@site_id INT
	,@site_nome VARCHAR(100)
	,@site_ativo BIT
	,@ultimo_login DATETIME = '2021-10-01'

IF OBJECT_ID('tempdb..#tListaUsuariosSiteRelacao_Temp') IS NOT NULL
	DROP TABLE #tListaUsuariosSiteRelacao_Temp

CREATE TABLE #tListaUsuariosSiteRelacao_Temp (
	seq_usuario INT
	,nome VARCHAR(100)
	,ultima_atualizacao DATETIME
	,seq_site INT
	,[site] VARCHAR(100)	
	,ativo BIT
)

DECLARE usuario_cursor CURSOR FOR
	SELECT sequencial, nome, atualizacao FROM AlctelVSS..Alc_mtr001Usuarios --WHERE nome IN('adriano.vinicius', 'poliana.souza')

OPEN usuario_cursor

FETCH NEXT FROM usuario_cursor INTO @usuario_id, @usuario_nome, @usuario_ultima_atualizacao

WHILE @@FETCH_STATUS = 0
BEGIN

	DECLARE site_cursor CURSOR FOR
		SELECT sequencial, [site] FROM AlctelVSS..Alc_mtr001Sites WHERE [site] <> 'Todos' AND ativo = 1

	OPEN site_cursor

	FETCH NEXT FROM site_cursor INTO @site_id, @site_nome

	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		SET @site_ativo = 0

		IF (SELECT COUNT(*) FROM AlctelVSS..Alc_mtr001ConfigSites WHERE seq_usuario = @usuario_id AND seq_site = @site_id AND ativo = 1) > 0
		BEGIN
			
			SET @site_ativo = 1

		END

		INSERT INTO #tListaUsuariosSiteRelacao_Temp VALUES(@usuario_id, @usuario_nome, @usuario_ultima_atualizacao, @site_id, @site_nome, @site_ativo)
		
		FETCH NEXT FROM site_cursor INTO @site_id, @site_nome

	END

	CLOSE site_cursor
	DEALLOCATE site_cursor

	FETCH NEXT FROM usuario_cursor INTO @usuario_id, @usuario_nome, @usuario_ultima_atualizacao
END

CLOSE usuario_cursor
DEALLOCATE usuario_cursor


SELECT *
FROM #tListaUsuariosSiteRelacao_Temp
WHERE
	ativo = 0
	AND ultima_atualizacao >= @ultimo_login
ORDER BY nome, [site]


