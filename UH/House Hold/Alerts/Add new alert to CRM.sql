

BEGIN TRAN 

select * from [TBL_UH_ENTITY_ALERTS] where [EntityReference] like 'CAXTON%' 
INSERT INTO [DocumotiveCRM_V2].[dbo].[TBL_UH_ENTITY_ALERTS]  ([AlertId],[EntityReference],[EntityTypeId],[Notes],[AuthorisedBy]) VALUES ('28','CAXTON000023','2','Ask@onehousing.co.uk','1')

select * from [TBL_UH_ENTITY_ALERTS] where [EntityReference] like 'CAXTON%' 
commit tran

