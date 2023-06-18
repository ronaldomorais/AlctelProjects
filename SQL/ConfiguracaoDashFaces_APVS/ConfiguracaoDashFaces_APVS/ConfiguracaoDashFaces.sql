DECLARE 
	@site VARCHAR(50) = 'SUPORTE-CRA'
	,@site_id INT
	,@dash_nome VARCHAR(15) = 'dashFaces'
	,@dado VARCHAR(20) = 'tma' -- tma, tabandono, tme
	,@seq_dashfacesmeta INT
	,@seq_configdash INT
	,@seq_dashfaces INT
	,@ambiente VARCHAR(10) = 'HOMOL' --HOMOL, --PROD
	,@processar_update BIT = 0 -- 0 - NÃO, 1 - SIM



SELECT @site_id = sequencial FROM AlctelVSS..Alc_mtr001Sites WHERE [site] = @site
SELECT @ambiente AS ambiente, @site_id AS site_id, @site AS [site], @dado AS dado



IF @processar_update = 1
BEGIN

	BEGIN TRANSACTION DASH_FACES_TRAN

	PRINT 'Updating ' + @ambiente

	IF @ambiente = 'HOMOL'
	BEGIN

		--Update Limites
		UPDATE AlctelVSS.dbo.Alc_mtr001ConfigLimites_HOMOL SET lim_inferior = 0, lim_superior = 30 WHERE sequencial = 473
		UPDATE AlctelVSS.dbo.Alc_mtr001ConfigLimites_HOMOL SET lim_inferior = 31, lim_superior = 120 WHERE sequencial = 474
		UPDATE AlctelVSS.dbo.Alc_mtr001ConfigLimites_HOMOL SET lim_inferior = 13.1, lim_superior = 2000 WHERE sequencial = 472

		--Update meta
		UPDATE AlctelVSS..Alc_DashFacesConfigMetas_HOMOL SET imeta = 340, fmeta = 340 WHERE sequencial = 40

	END

	IF @ambiente = 'PROD'
	BEGIN

		--Update Limites
		UPDATE AlctelVSS.dbo.Alc_mtr001ConfigLimites SET lim_inferior = 0, lim_superior = 30 WHERE sequencial = 473
		UPDATE AlctelVSS.dbo.Alc_mtr001ConfigLimites SET lim_inferior = 31, lim_superior = 120 WHERE sequencial = 474
		UPDATE AlctelVSS.dbo.Alc_mtr001ConfigLimites SET lim_inferior = 13.1, lim_superior = 2000 WHERE sequencial = 472

		--Update meta
		UPDATE AlctelVSS..Alc_DashFacesConfigMetas SET imeta = 340, fmeta = 340 WHERE sequencial = 40

	END
END


IF @ambiente = 'HOMOL'
BEGIN

	SELECT @seq_dashfaces = sequencial FROM AlctelVSS.dbo.Alc_mtr001DashFaces_HOMOL WHERE dado = @dado	
	SELECT @seq_configdash = sequencial FROM AlctelVSS.dbo.Alc_mtr001Dashboards_HOMOL WHERE nome = @dash_nome AND [site] = @site_id
	--SELECT @seq_dashfacesmeta = sequencial FROM AlctelVSS.dbo.Alc_mtr001ConfigFaces_HOMOL WHERE seq_dashfaces = @seq_dashfaces AND seq_site = @site_id
	SELECT @seq_dashfacesmeta = sequencial FROM AlctelVSS..Alc_DashFacesMetas_HOMOL WHERE dado = @dado

	SELECT @seq_dashfaces as Seq_DashFaces, @seq_dashfacesmeta AS Seq_DashFacesMeta, @seq_configdash AS Seq_ConfigDash

	--Metas
	SELECT * FROM AlctelVSS.dbo.Alc_mtr001ConfigFaces_HOMOL WHERE seq_site = @site_id AND seq_dashfaces = @seq_dashfaces
	SELECT * FROM AlctelVSS..Alc_DashFacesConfigMetas_HOMOL WHERE parSite = @site_id AND seq_dashfacesmeta = @seq_dashfacesmeta
	
	--Limites
	SELECT * FROM AlctelVSS.dbo.Alc_mtr001ConfigLimites_HOMOL WHERE seq_col_indice = @seq_dashfaces AND seq_configdash = @seq_configdash

END

IF @ambiente = 'PROD'
BEGIN

	SELECT @seq_dashfaces = sequencial FROM AlctelVSS.dbo.Alc_mtr001DashFaces WHERE dado = @dado
	SELECT @seq_configdash = sequencial FROM AlctelVSS.dbo.Alc_mtr001Dashboards WHERE nome = @dash_nome AND [site] = @site_id
	--SELECT @seq_dashfacesmeta = sequencial FROM AlctelVSS.dbo.Alc_mtr001ConfigFaces WHERE seq_dashfaces = @seq_dashfaces AND seq_site = @site_id
	SELECT @seq_dashfacesmeta = sequencial FROM AlctelVSS..Alc_DashFacesMetas_HOMOL WHERE dado = @dado

	SELECT @seq_dashfaces as Seq_DashFaces, @seq_dashfacesmeta AS Seq_DashFacesMeta, @seq_configdash AS Seq_ConfigDash

	--Metas
	SELECT * FROM AlctelVSS.dbo.Alc_mtr001ConfigFaces WHERE seq_dashfaces = @seq_dashfaces AND seq_site = @site_id
	SELECT * FROM AlctelVSS..Alc_DashFacesConfigMetas WHERE parSite = @site_id AND seq_dashfacesmeta = @seq_dashfacesmeta

	--Limites
	SELECT * FROM AlctelVSS.dbo.Alc_mtr001ConfigLimites WHERE seq_col_indice = @seq_dashfaces AND seq_configdash = @seq_configdash
END


RETURN

COMMIT TRANSACTION DASH_FACES_TRAN
ROLLBACK TRANSACTION DASH_FACES_TRAN




