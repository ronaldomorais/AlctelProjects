/*


TABULA��ES QUE PRECISAM SER CRIADAS
Motivo	Submotivo	Rotulo / Data
DMPAG	INFORMA��O	Solicitar a inclus�o do motivo e submotivo

Motivo = tipo
Submotivo = subtipo


*/



--INSERT INTO [AlctelVSS].[dbo].[Alc_WDE_Tabulacao] (tabulacao, tipo, subtipo)
--VALUES('Email', 'DMPAG', 'INFORMA��O')

SELECT *
FROM AlctelVSS..Alc_WDE_Tabulacao
ORDER BY tabulacao, tipo, subtipo



