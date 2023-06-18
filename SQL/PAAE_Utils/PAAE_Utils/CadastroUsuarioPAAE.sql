--Cadastro de usuário na PAAE



DECLARE 
	@usuario_espelho VARCHAR(50) = 'mtorres'
	,@usuario_novo VARCHAR(50) = 'trenata'

DECLARE
	@sequencial_espelho INT
	,@sequencial_novo INT
		

IF (SELECT COUNT(*) FROM AlctelVSS.dbo.Alc_mtr001Usuarios WHERE nome = @usuario_novo) < 1
BEGIN
	--INSERT INTO AlctelVSS.dbo.Alc_mtr001Usuarios(nome, estado, ajustar_senha, grpaplicacoes, senhac, guidref, logado, inclusao, atualizacao, abas)
	INSERT INTO AlctelVSS.dbo.Alc_mtr001Usuarios(nome, estado, ajustar_senha, grpaplicacoes, senhac, guidref, logado, inclusao, atualizacao)
	SELECT 
		@usuario_novo
		,estado
		,ajustar_senha
		,grpaplicacoes
		,'8f594ece1c1538b531df33b4cf04a2dc85e9a1a02cf0b5e2b5ee5c95d58a2b7c' --senha 1234
		,'f483679b-73f4-434b-840b-e52946a6eccb'
		,logado
		,GETDATE()
		,GETDATE()
		--,abas
	FROM
		AlctelVSS.dbo.Alc_mtr001Usuarios 
	WHERE 
		nome = @usuario_espelho
		
	SELECT @sequencial_espelho = sequencial FROM AlctelVSS.dbo.Alc_mtr001Usuarios WHERE nome = @usuario_espelho		
	SELECT @sequencial_novo = sequencial FROM AlctelVSS.dbo.Alc_mtr001Usuarios WHERE nome = @usuario_novo	
	
	INSERT INTO AlctelVSS.dbo.Alc_mtr001ConfigAbas(seq_usuario, seq_aba, ativo)
	SELECT @sequencial_novo, seq_aba, 1 FROM AlctelVSS.dbo.Alc_mtr001ConfigAbas WHERE seq_usuario = @sequencial_espelho
	
	INSERT INTO AlctelVSS.dbo.Alc_mtr001ConfigSites(seq_usuario, seq_site, ativo, [enabled], ordem)
	SELECT @sequencial_novo, seq_site, 1, 1, ordem FROM AlctelVSS.dbo.Alc_mtr001ConfigSites WHERE seq_usuario = @sequencial_espelho
	
	--INSERT INTO AlctelVSS.dbo.Alc_mtr001ConfigReportUsuarios(seq_usuario, seq_report, [enabled])
	--SELECT @sequencial_novo, seq_report, [enabled] FROM AlctelVSS.dbo.Alc_mtr001ConfigReportUsuarios WHERE seq_usuario = @sequencial_espelho

	INSERT INTO AlctelVSS.dbo.Alc_mtr001ConfigReportUser(seq_usuario, seq_report, ativo)
	SELECT @sequencial_novo, seq_report, ativo FROM AlctelVSS.dbo.Alc_mtr001ConfigReportUser WHERE seq_usuario = @sequencial_espelho
	
	PRINT 'Usuario ' + @usuario_novo + ' inserido com sequencial ' + CAST(@sequencial_novo AS VARCHAR(5))
END
ELSE
BEGIN
	SELECT @sequencial_novo = sequencial FROM AlctelVSS.dbo.Alc_mtr001Usuarios WHERE nome = @usuario_novo 
	PRINT 'Login ' + @usuario_novo + ' já existe com o sequencial ' + CAST(@sequencial_novo AS VARCHAR(5))
END