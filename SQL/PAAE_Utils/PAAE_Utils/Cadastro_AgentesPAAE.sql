--Adcionar agentes na paae

--Script 01

DECLARE @ageid_espelho INT = 599
DECLARE @ageid_novo INT = 632

INSERT INTO [AlctelVSS].[dbo].[Alc_mtr001ConfigGroupAgentes](gro_sequencial,gro_usu_sequencial,age_dbid,ativo, atualizacao)
select DISTINCT
	gro_sequencial
	,gro_usu_sequencial
	,@ageid_novo
	,ativo
	,GETDATE()
from [AlctelVSS].[dbo].[Alc_mtr001ConfigGroupAgentes] 
where 
	age_dbid = @ageid_espelho
	and gro_sequencial NOT IN (select gro_sequencial from [AlctelVSS].[dbo].[Alc_mtr001ConfigGroupAgentes] where age_dbid = @ageid_novo)
	
	
SELECT * FROM [AlctelVSS].[dbo].[Alc_mtr001ConfigGroupAgentes] WHERE age_dbid IN(@ageid_espelho, @ageid_novo) ORDER BY age_dbid, gro_sequencial	




--Script 02

declare @name varchar(50) = 'THAYNAN'
select * from alctel_gen_config.dbo.cfg_person
where 
	--employee_id = '90137' or 
	first_name like '%' + @name + '%'
	or last_name like '%' + @name + '%'


declare @age_espelho int; declare @age_novo int
set @age_espelho = 408  -- AGENTE ESPELHO
set @age_novo = 626 -- AGENTE NOVO
 
insert into [AlctelVSS].[dbo].[Alc_mtr001ConfigGroupAgentes]
(gro_sequencial, gro_usu_sequencial, age_dbid, ativo)
select [gro_sequencial]
      ,[gro_usu_sequencial]
      ,@age_novo 
      ,[ativo]       
         from [AlctelVSS].[dbo].[Alc_mtr001ConfigGroupAgentes] where age_dbid = @age_espelho 
                              and gro_sequencial not in
         (select gro_sequencial from [AlctelVSS].[dbo].[Alc_mtr001ConfigGroupAgentes] 
                             where age_dbid = @age_novo )
							 
							 
							 
