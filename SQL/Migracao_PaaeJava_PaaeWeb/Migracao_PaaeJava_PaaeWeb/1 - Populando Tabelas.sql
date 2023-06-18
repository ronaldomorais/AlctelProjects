USE AlctelVSS
GO

INSERT INTO dbo.Alc_NomeGrupos VALUES (1, 'ALFALINE')
GO
INSERT INTO dbo.Alc_NomeGrupos VALUES (1, 'JAGUAR')
GO
INSERT INTO dbo.Alc_NomeGrupos VALUES (1, 'LAND ROVER')
GO
INSERT INTO dbo.Alc_NomeGrupos VALUES (1, 'SAC-FIN')
GO
INSERT INTO dbo.Alc_NomeGrupos VALUES (1, 'VENDAS')
GO
INSERT INTO dbo.Alc_NomeGrupos VALUES (2, 'MASSIFICADOS')
GO
INSERT INTO dbo.Alc_NomeGrupos VALUES (2, 'OUVIDORIA')
GO
INSERT INTO dbo.Alc_NomeGrupos VALUES (2, 'SAC-SEG')
GO
INSERT INTO dbo.Alc_NomeGrupos VALUES (2, 'SINISTRO')
GO


INSERT INTO dbo.Alc_CodFila VALUES ('ALFALINE', '0_5_293')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('ALFALINE', '0_5_294')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('ALFALINE', '0_5_295')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('ALFALINE', '0_5_362')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('JAGUAR', '0_5_210')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('JAGUAR', '0_5_356')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('JAGUAR', '0_5_357')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('JAGUAR', '0_5_358')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('LAND ROVER', '0_5_211')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('LAND ROVER', '0_5_359')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('LAND ROVER', '0_5_360')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('LAND ROVER', '0_5_361')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('MASSIFICADOS', '0_5_237')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('OUVIDORIA', '0_5_238')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('SAC-FIN', '0_5_290')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('SAC-FIN', '0_5_291')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('SAC-FIN', '0_5_292')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('SAC-SEG', '0_5_239')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('SINISTRO', '0_5_236')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('SINISTRO', '0_5_240')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('SINISTRO', '0_5_241')
GO
INSERT INTO dbo.Alc_CodFila VALUES ('VENDAS', '0_5_197')
GO

INSERT INTO AlctelVSS..Alc_mtr001Sites([site], filiacao) 
	VALUES
		('ALFALINE', 0)
		,('JAGUAR', 0)
		,('LAND ROVER', 0)
		,('SAC-FIN', 0)
		,('VENDAS', 0)
		,('MASSIFICADOS', 0)
		,('OUVIDORIA', 0)
		,('SAC-SEG', 0)
		,('SINISTRO', 0)
GO

INSERT INTO AlctelVSS..Alc_mtr001ConfigSites (seq_usuario, seq_site, ativo, [enabled], ordem)
	VALUES
		(1, 1, 1, 1, 0)
		,(1, 2, 1, 1, 0)
		,(1, 3, 1, 1, 0)
		,(1, 4, 1, 1, 0)
		,(1, 5, 1, 1, 0)
		,(1, 6, 1, 1, 0)
		,(1, 7, 1, 1, 0)
		,(1, 8, 1, 1, 0)
		,(1, 9, 1, 1, 0)




