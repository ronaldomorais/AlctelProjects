USE [AlctelVSS]
GO

/****** Object:  StoredProcedure [dbo].[Alc_BancoAlfa_sp_relatorioAbandonadas]    Script Date: 1/13/2023 10:47:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[JAVA_hiAbandonadas] 
	-- Add the parameters for the stored procedure here
	@parUser VARCHAR(50) = ''
	,@dataInicial NVARCHAR(20) = '2021-01-14'
	,@dataFinal NVARCHAR(20) = '2021-01-15'
	,@parAplicacao INT = 1
	,@parSite INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	select  
		mtri.sequencial as isequencial,
		mtri.data,
		mtri.origem,
		mtri.fila,
		mtri.inFila,
		mtri.outFila,
		mtri.agente
	from 
		alctelvss.dbo.Alc_mtrInbound as mtri
			
	where
		mtri.processamento = 2 and 
		dateadd(dd,0,datediff(dd,0,mtri.data)) between @dataInicial and @dataFinal
		and mtri.fila = 'FIN_VENDAS'  and mtri.data < '2017-08-11'
	
	union all

	--select 
	--	alfa.sequencial as isequencial, 
	--	alfa.cha_outFila as data,
	--	alfa.cha_origem as origem, 
	--	alfa.cha_fila as fila, 
	--	(case when a.cha_infila is null 
	--			then (case when c.cha_infila is null then alfa.cha_outFila else c.cha_inFila end) 
	--			else a.cha_infila end) as inFila,
	--	alfa.cha_outFila as outFila,
	--	'***' as agente 
	--from 
	--	AlctelVSS.dbo.Alc_Alfa_Abandonadas as alfa
	--left outer join 
	--	AlctelVSS.dbo.Alc_mtrChamadasGeral as c
	--on c.cha_id = alfa.cha_id and c.age_agentid = '***'
	--left outer join 
	--	AlctelVSS.dbo.Alc_mtrChamadasGeralAgentes as a
	--on a.cha_id = alfa.cha_id and a.age_agentid = '***'
	--where 
	--	dateadd(dd,0,datediff(dd,0,alfa.cha_outFila)) between @dataInicial and @dataFinal 
	--	and alfa.cha_fila = 'Vendas_Financeiras' 
	--order by data

	select 
		c.sequencial as isequencial, 
		c.cha_outFila as ColunaTipoDateTime01,
		c.cha_origem as ColunaTipoStr01, 
		c.cha_fila as ColunaTipoStr02, 
		(case when a.cha_infila is null 
				then (case when c.cha_infila is null then c.cha_outFila else c.cha_inFila end) 
				else a.cha_infila end) as ColunaTipoDateTime02,
		c.cha_outFila as ColunaTipoDateTime03,
		'***' as agente 
	from
		AlctelVSS.dbo.Alc_mtrChamadasGeral as c
	left outer join 
		AlctelVSS.dbo.Alc_mtrChamadasGeralAgentes as a
	on a.cha_id = c.cha_id and a.age_agentid = '***'
	where 
		dateadd(dd,0,datediff(dd,0,c.cha_outFila)) between @dataInicial and @dataFinal 
		and c.cha_fila = 'Vendas_Financeiras'  and a.age_agentid ='***'
	order by data
END











GO


