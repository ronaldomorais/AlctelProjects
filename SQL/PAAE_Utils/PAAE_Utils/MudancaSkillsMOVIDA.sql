USE AlctelVSS
GO


DECLARE
	@bInsertSkill BIT = 0
	,@bInsertFila BIT = 0
	,@bUpdateFilaDbid BIT = 0
	,@site VARCHAR(100) = 'Reservas Moover'
	,@fila VARCHAR(100) = 'VQ_Moover_Sem_Parar'
	,@fila_dbid NUMERIC(10,0)
	,@skill VARCHAR(100) = 'ATH_SAC'
	,@skill_dbid NUMERIC(10,0)

SELECT @skill_dbid = [dbid] FROM Alctel_Gen_CONFIG..cfg_skill WITH (NOLOCK) WHERE [name] = @skill
SELECT @fila_dbid = [dbid] FROM Alctel_Gen_CONFIG..cfg_dn WITH (NOLOCK) WHERE [name] = @fila

SELECT @fila AS Fila, @fila_dbid AS Fila_dbid, @skill AS Skill, @skill_dbid AS Skill_dbid

SELECT *
FROM AlctelVSS..Alc_mtr001Filas WITH (NOLOCK)
WHERE
	nome = @fila

SELECT *
FROM AlctelVSS..Alc_mtr001Filas WITH (NOLOCK)
WHERE
	fila = @skill AND nome = @fila


IF @bUpdateFilaDbid = 1
BEGIN

	UPDATE AlctelVSS..Alc_mtr001Filas SET [dbid] = @fila_dbid WHERE fila = @fila

END

IF @bInsertSkill = 1
BEGIN
	INSERT INTO AlctelVSS..Alc_mtr001Filas(
	[dbid]
	,taxa
	,fila
	,agendamento
	,prioridade
	,nome
	,seq_aplicacao
	,tempo
	,filacallback
	,transb_dia
	,transb_hi
	,transb_hf
	,transb_grupo
	,transb_grupo2
	,transb_grupo3
	,transb_grupo4
	,transb_grupo5
	,transb_grupo6
	,parreport
	,[site]
	,fil_visivel
	,fil_callback
	,tolerancia
	,fil_skill
	)
	VALUES(@skill_dbid,0,@skill,1,8,@fila,	0,1,'***', '', '', '', '', '', '', '', '', '','trfilas',@site,1,1,0,0)
	


	INSERT INTO AlctelVSS..Alc_mtr001Filas(
	[dbid]
	,taxa
	,fila
	,agendamento
	,prioridade
	,nome
	,seq_aplicacao
	,tempo
	,filacallback
	,transb_dia
	,transb_hi
	,transb_hf
	,transb_grupo
	,transb_grupo2
	,transb_grupo3
	,transb_grupo4
	,transb_grupo5
	,transb_grupo6
	,parreport
	,[site]
	,fil_visivel
	,fil_callback
	,tolerancia
	,fil_skill
	)
	VALUES(@skill_dbid,0,@skill,1,8,@fila,	0,1,'***', '', '', '', '', '', '', '', '', '','trfilas','Todos',1,1,0,0)

END



IF @bInsertFila = 1
BEGIN
	INSERT INTO AlctelVSS..Alc_mtr001Filas(
	[dbid]
	,taxa
	,fila
	,agendamento
	,prioridade
	,nome
	,seq_aplicacao
	,tempo
	,filacallback
	,transb_dia
	,transb_hi
	,transb_hf
	,transb_grupo
	,transb_grupo2
	,transb_grupo3
	,transb_grupo4
	,transb_grupo5
	,transb_grupo6
	,parreport
	,[site]
	,fil_visivel
	,fil_callback
	,tolerancia
	,fil_skill
	)
	VALUES(@fila_dbid,0,@fila,1,8,@fila,0,1,'***', '', '', '', '', '', '', '', '', '','trfilas',@site,1,1,0,0)
	


	INSERT INTO AlctelVSS..Alc_mtr001Filas(
	[dbid]
	,taxa
	,fila
	,agendamento
	,prioridade
	,nome
	,seq_aplicacao
	,tempo
	,filacallback
	,transb_dia
	,transb_hi
	,transb_hf
	,transb_grupo
	,transb_grupo2
	,transb_grupo3
	,transb_grupo4
	,transb_grupo5
	,transb_grupo6
	,parreport
	,[site]
	,fil_visivel
	,fil_callback
	,tolerancia
	,fil_skill
	)
	VALUES(@fila_dbid,0,@fila,1,8,@fila,0,1,'***', '', '', '', '', '', '', '', '', '','trfilas','Todos',1,1,0,0)

END


IF (@bInsertFila = 1 OR @bInsertSkill = 1 OR @bUpdateFilaDbid = 1)
BEGIN

	SELECT 'RESULTADO INSERÇÃO'

	SELECT *
	FROM AlctelVSS..Alc_mtr001Filas WITH (NOLOCK)
	WHERE
		nome = @fila

	SELECT *
	FROM AlctelVSS..Alc_mtr001Filas WITH (NOLOCK)
	WHERE
		fila = @skill AND nome = @fila

END