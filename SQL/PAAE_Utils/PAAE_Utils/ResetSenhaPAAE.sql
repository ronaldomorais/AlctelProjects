--Alteração de Senha


IF OBJECT_ID('tempdb..#tUsuarioTemp') IS NOT NULL
	DROP TABLE #tUsuarioTemp

DECLARE 
	@usuario VARCHAR(50) = 'ronaldo.morais' --Usuário Alvo
	

DECLARE
	@sequencial_usuario INT


SELECT
	*
INTO #tUsuarioTemp
FROM AlctelVSS.dbo.Alc_mtr001Usuarios
WHERE
	nome = @usuario
		

IF (SELECT COUNT(*) FROM #tUsuarioTemp) = 1
BEGIN
	SELECT @sequencial_usuario = sequencial FROM #tUsuarioTemp

	UPDATE AlctelVSS.dbo.Alc_mtr001Usuarios 
		SET senhac = '8f594ece1c1538b531df33b4cf04a2dc85e9a1a02cf0b5e2b5ee5c95d58a2b7c',
		guidref = 'f483679b-73f4-434b-840b-e52946a6eccb'
	WHERE
		sequencial = @sequencial_usuario

		
	PRINT 'Senha atualizada para 1234 do Usuario ' + @usuario
END
ELSE
BEGIN
	PRINT 'Usuário ' + @usuario + ' não encontrado ' 
END


