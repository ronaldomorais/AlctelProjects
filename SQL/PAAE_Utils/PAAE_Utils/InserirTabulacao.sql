/*


TABULAÇÕES QUE PRECISAM SER CRIADAS
Motivo	Submotivo	Rotulo / Data
DMPAG	INFORMAÇÃO	Solicitar a inclusão do motivo e submotivo

Motivo = tipo
Submotivo = subtipo


*/



--INSERT INTO [AlctelVSS].[dbo].[Alc_WDE_Tabulacao] (tabulacao, tipo, subtipo)
--VALUES('Email', 'DMPAG', 'INFORMAÇÃO')

SELECT *
FROM AlctelVSS..Alc_WDE_Tabulacao
ORDER BY tabulacao, tipo, subtipo



