
-- use the following select to find the alert required
select * from lookup where lu_type = 'uAW' 

-- update the below u_awareness with the alert code found above

begin tran
update househ
set u_awareness = '015'
where house_ref = 'A032840'
commit


-- use the following to search for the property reference based on the short address
select * from property
where short_address like '%Attebrouche Court%'

-- use the following to find the household reference based on the property reference found with the above
select * from househ
where prop_ref = 'ATTEBR000100'

-- Connect to OHGEDMSQL01 then use the above household reference to add CRM-OneZone alert with the below

BEGIN TRAN 

select * from [TBL_UH_ENTITY_ALERTS] where [EntityReference] like 'A032840%' 
INSERT INTO [DocumotiveCRM_V2].[dbo].[TBL_UH_ENTITY_ALERTS]  ([AlertId],[EntityReference],[EntityTypeId],[Notes],[AuthorisedBy]) VALUES ('6','A032840','1','Property in Self Isolation - Do not visit until 07/04/2020','1')

select * from [TBL_UH_ENTITY_ALERTS] where [EntityReference] like 'A032840%' 
commit tran
