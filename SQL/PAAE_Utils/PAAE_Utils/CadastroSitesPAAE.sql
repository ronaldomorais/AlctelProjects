
DECLARE
--ALTER PROCEDURE Alc_PAAE_sp_CadastroSite
	@novo_site VARCHAR(50) = 'Celula Contratos'
	,@fila_lista VARCHAR(255) = 'fila01;fila02'
	,@usuarios_lista VARCHAR(255) = 'usuario01;usuario02'
--AS
BEGIN
	--1 - Verificando Site
	IF OBJECT_ID('tempdb..#tSites') IS NOT NULL
		DROP TABLE #tSites

	SELECT *
	INTO #tSites
	FROM AlctelVSS.dbo.Alc_mtr001Sites
	WHERE
		[site] = @novo_site

	DECLARE 
		@site VARCHAR(50) = NULL
		,@ativo INT
		,@site_id INT
		,@site_filaid INT
		,@site_filaativo INT
		,@filas VARCHAR(MAX) = ''
		,@usuarios VARCHAR(MAX) = ''
		,@delimiter VARCHAR(2) = ';'

	DECLARE @tUsuarios TABLE (usuario VARCHAR(100))
	DECLARE @tFilas TABLE (fila VARCHAR(100))


	IF (SELECT COUNT(sequencial) FROM #tSites) = 0
	BEGIN
		INSERT INTO AlctelVSS.dbo.Alc_mtr001Sites([site], filiacao, ativo) VALUES(@novo_site, 0, 1)

		SELECT 
			@site = [site], @ativo = ativo, @site_id = sequencial
		FROM AlctelVSS.dbo.Alc_mtr001Sites
		WHERE
			[site] = @novo_site 

		PRINT 'Site ' + @site + ' criado com sucesso. Ativo = ' + CAST(@ativo AS VARCHAR(2))
	END
	ELSE 
	BEGIN	
		SELECT @site = [site], @ativo = ativo, @site_id = sequencial FROM #tSites

		PRINT 'Site ' + @site + ' já existe. Ativo = ' + CAST(@ativo AS VARCHAR(2))
	END
	
	--2 - Verificando site no tempo real filas
	SELECT @site_filaid = sequencial, @site_filaativo = ativo
	FROM AlctelVSS.dbo.Alc_mtr001ConfigFilas
	WHERE
		[site] = @site_id

	IF ISNULL(@site_filaid, -1) < 1
	BEGIN
		INSERT INTO AlctelVSS.dbo.Alc_mtr001ConfigFilas(grpaplicacoes, nome, titulo, ordem, ativo, [site])
			VALUES(1, 'trfilas', 'VOZ (inbound)', 1, 1, @site_id)
		PRINT 'Site ' + @site + ' configurado no tempo real filas com sucesso'		
	END
	ELSE
	BEGIN
		PRINT 'Site ' + @site + ' já configurado no tempo real filas'
	END

	--3 - Verificando a associação do site com a fila

	IF ISNULL(@fila_lista, '') <> ''
	BEGIN
		SET @filas = @fila_lista + @delimiter

		WHILE (CHARINDEX(@delimiter, @filas) > 0)
		BEGIN
			INSERT INTO @tFilas
			SELECT SUBSTRING(@filas, 0, CHARINDEX(@delimiter, @filas, 0))

			SET @filas = SUBSTRING(@filas, CHARINDEX(@delimiter, @filas, 0) + 1, LEN(@filas))
		END
		
	END


	IF (SELECT COUNT(fila) FROM @tFilas) > 0 AND ISNULL(@site, '') <> ''
	BEGIN 
		
		DECLARE 
			@fila_dalista VARCHAR(100)
			,@fila_id INT
			,@fila VARCHAR(100)
			,@fila_site VARCHAR(100)
			,@fila_dbid INT

		DECLARE fila_cursor CURSOR FOR SELECT fila FROM @tFilas WHERE fila <> ''

		OPEN fila_cursor
		FETCH NEXT FROM fila_cursor INTO @fila_dalista

		WHILE @@FETCH_STATUS = 0
		BEGIN
			SELECT @fila_id = sequencial, @fila = fila, @fila_site = [site] FROM AlctelVSS.dbo.Alc_mtr001Filas WHERE fila = @fila_dalista AND UPPER([site]) <> 'TODOS'

			IF ISNULL(@fila_id, 0) > 0
			BEGIN
				UPDATE F SET F.[site] = @site
				FROM AlctelVSS.dbo.Alc_mtr001Filas AS F
				WHERE sequencial = @fila_id

				PRINT 'Fila ' + @fila + ' atualizada para site ' + @site + ' com sucesso!'
			END
			ELSE 
			BEGIN
				SELECT @fila_dbid = [dbid] FROM Alctel_Gen_CONFIG.dbo.cfg_skill WHERE [name] = @fila_lista

				IF ISNULL(@fila_dbid, 0) > 0
				BEGIN
					INSERT INTO AlctelVSS.dbo.Alc_mtr001Filas (
							[dbid], taxa, fila, agendamento, prioridade, nome, seq_aplicacao, tempo, filacallback
							,transb_dia, transb_hi, transb_hf, transb_grupo, transb_grupo2, transb_grupo3, transb_grupo4
							,transb_grupo5, transb_grupo6, parreport, [site], fil_visivel, fil_callback, tolerancia, fil_skill
						)
						VALUES(@fila_dbid, 0, @fila_dalista, 1, 8, @fila_dalista, 0, 1, '***', '', '', '', '', '', '', '', '', '', 'trfilas', @site, 1, 1, 0, 0)

					INSERT INTO AlctelVSS.dbo.Alc_mtr001Filas (
							[dbid], taxa, fila, agendamento, prioridade, nome, seq_aplicacao, tempo, filacallback
							,transb_dia, transb_hi, transb_hf, transb_grupo, transb_grupo2, transb_grupo3, transb_grupo4
							,transb_grupo5, transb_grupo6, parreport, [site], fil_visivel, fil_callback, tolerancia, fil_skill
						)
						VALUES(@fila_dbid, 0, @fila_dalista, 1, 8, @fila_dalista, 0, 1, '***', '', '', '', '', '', '', '', '', '', 'trfilas', 'Todos', 1, 1, 0, 0)

					PRINT 'Fila ' + @fila_lista + ' configurada para site ' + @site + ' e Todos com sucesso!'
				END
				ELSE
				BEGIN
					PRINT 'Fila ' + @fila_lista + ' não localida na tabela de skills'
				END
			END

			FETCH NEXT FROM fila_cursor INTO @fila_dalista
		END
		
		CLOSE fila_cursor
		DEALLOCATE fila_cursor
		
	END

	--4 - Verificar se site associado ao tempo Real Agentes

	SELECT @site_filaid = sequencial, @site_filaativo = ativo
	FROM AlctelVSS.dbo.Alc_mtr001ConfigAgentes
	WHERE
		[site] = @site_id

	IF ISNULL(@site_filaid, -1) < 1
	BEGIN
		INSERT INTO AlctelVSS.dbo.Alc_mtr001ConfigAgentes(grpaplicacoes, nome, titulo, ordem, ativo, [site])
			VALUES(1, 'tragentes', 'VOZ (inbound)', 1, 1, @site_id)
		PRINT 'Site ' + @site + ' configurado no tempo real de agentes com sucesso'		
	END
	ELSE
	BEGIN
		PRINT 'Site ' + @site + ' já configurado no tempo real agentes'
	END



	--5 - Verificando usuário se inserindo em Site

	IF ISNULL(@usuarios_lista, '') <> ''
	BEGIN
		SET @usuarios = @usuarios_lista + @delimiter

		WHILE (CHARINDEX(@delimiter, @usuarios) > 0)
		BEGIN
			INSERT INTO @tUsuarios
			SELECT SUBSTRING(@usuarios, 0, CHARINDEX(@delimiter, @usuarios, 0))

			SET @usuarios = SUBSTRING(@usuarios, CHARINDEX(@delimiter, @usuarios, 0) + 1, LEN(@usuarios))
		END

	END


	IF (SELECT COUNT(usuario) FROM @tUsuarios) > 0 AND ISNULL(@site, '') <> ''
	BEGIN

		DECLARE 
			@usuario_nome VARCHAR(20) = NULL
			,@usuario_id INT
			,@usuario_dalista VARCHAR(MAX)
			,@usuario_confsite INT

		IF OBJECT_ID('tempdb..#tRelatoriosSequencia') IS NOT NULL
			DROP TABLE #tRelatoriosSequencia

		IF OBJECT_ID('tempdb..#tRelatoriosNaoConfigurados') IS NOT NULL
			DROP TABLE #tRelatoriosNaoConfigurados

		SELECT 		
			sequencial
		INTO #tRelatoriosSequencia
		FROM AlctelVSS.dbo.Alc_mtr001ConfigReport
		WHERE ativo = 1 AND tipo = 2


		DECLARE usuario_cursor CURSOR FOR SELECT usuario FROM @tUsuarios WHERE usuario <> ''

		OPEN usuario_cursor
		FETCH NEXT FROM usuario_cursor INTO @usuario_dalista

		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			SELECT 
				@usuario_id = sequencial
				,@usuario_nome = nome
			FROM AlctelVSS.dbo.Alc_mtr001Usuarios
			WHERE
				nome = @usuario_dalista

			SELECT @usuario_confsite = sequencial
			FROM AlctelVSS.dbo.Alc_mtr001ConfigSites
			WHERE
				seq_usuario = @usuario_id AND seq_site = @site_id

			IF ISNULL(@usuario_confsite, -1) < 1
			BEGIN
				INSERT INTO AlctelVSS.dbo.Alc_mtr001ConfigSites(seq_usuario, seq_site, ativo, [enabled], ordem)
					VALUES(@usuario_id, @site_id, 1, 1, 0)
				PRINT 'Usuário ' + @usuario_nome + ' configurado no site ' + @site + ' com sucesso!'
			END
			ELSE
			BEGIN
				PRINT 'Usuário ' + @usuario_nome + ' já está configurado no site ' + @site
			END


			-- Configurando relatórios

			IF (SELECT COUNT(sequencial) FROM #tRelatoriosSequencia) > 0
			BEGIN
				SELECT * 
				INTO #tRelatoriosNaoConfigurados
				FROM AlctelVSS.dbo.Alc_mtr001ConfigReportUsuarios 
				WHERE
					seq_usuario = @usuario_id AND seq_report NOT IN(SELECT sequencial FROM #tRelatoriosSequencia)

				IF (SELECT COUNT(*) FROM #tRelatoriosNaoConfigurados) > 0
				BEGIN
					INSERT INTO AlctelVSS.dbo.Alc_mtr001ConfigReportUsuarios(seq_usuario, seq_report, [enabled])
					SELECT
						@usuario_id AS seq_usuario
						,seq_report
						,1 AS [enabled]
					FROM #tRelatoriosNaoConfigurados

					PRINT 'Relatórios configurados para o usuário ' + @usuario_nome + ' (' + CAST(@usuario_id AS VARCHAR(255)) + ')'
				END
				ELSE
				BEGIN
					PRINT 'Relatórios já configurados para o usuário ' + @usuario_nome + ' (' + CAST(@usuario_id AS VARCHAR(255)) + ')'
				END
			END
			ELSE
			BEGIN
				PRINT 'Nenhum relatório ativo para configurar no site ' + @site
			END

			FETCH NEXT FROM usuario_cursor INTO @usuario_dalista
		END

		CLOSE usuario_cursor
		DEALLOCATE usuario_cursor
	


	END
	ELSE
	BEGIN
		PRINT 'Nenhum usuário informado para validar/cadastrar no site ' + @site
	END


END

