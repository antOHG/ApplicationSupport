/*
STEPS:

1. UPDATE the @PropRef with an appropriate reference, say 'THRALE00%'

2. Execute the 'MAIN SCRIPT', if the result shows several successful messages - then the activity is completed

3. If the result shows "Components already exists", then DELETE the components by running the following with appropriate @PropRef:

BEGIN TRAN
USE [One.Components]
DECLARE @PropRef nvarchar(50) = 'THRALE00%'
DELETE FROM ComponentSupplier where ComponentId in (select Id from Component where PropertyReference in (select PropertyRef from Property where PropertyRef like @PropRef))
DELETE FROM Component where PropertyReference in (select PropertyRef from Property where PropertyRef like @PropRef)
ROLLBACK

4. If the above is successful, execute the script # 3 with COMMIT action

5. Now, execute the 'MAIN SCRIPT' to complete this activity
	
*/

--MAIN SCRIPT

USE [One.Components]
DECLARE @PropRef nvarchar(50) = 'THRALE00%'
IF (select count(*) from Component where PropertyReference in (select PropertyRef from Property where propertyRef like @PropRef))>0
BEGIN
	PRINT 'Components already exists'
	RETURN
END

DECLARE @Property table ([ID] [int] IDENTITY(1,1) NOT NULL, prop_ref nvarchar(50), level_code int)
insert into @Property select replace(propertyRef, ' ', ''), levelCode from property where propertyRef like @PropRef order by PropertyRef
SELECT * FROM @Property

DECLARE @code INT
DECLARE @PropertyCounter INT
DECLARE @PropertyMax INT
DECLARE @TempEstateBlockCOmponentID INT
DECLARE @ComponentSupplierCount INT
DECLARE @Output nvarchar(max)
DECLARE @ComponentEstateCount	INT = 18	--CURRENT NO.OF COMPONENTS
DECLARE @ComponentUnitCount		INT = 18	--CURRENT NO.OF COMPONENTS

SET @code = (select max(Code)+1 from Component)
SET @PropertyCounter = 1
SET @PropertyMax = (select count(*) from @Property)
SET @Output = ''
SET @ComponentSupplierCount = 0

WHILE (@PropertyCounter <= @PropertyMax)
BEGIN
	SELECT @Output = @Output + dbo.fn_GetNonDefectInsertScript((Select prop_ref from @Property where ID=@PropertyCounter), @code, (Select level_code from @Property where ID=@PropertyCounter))

	IF ((Select level_code from @Property where ID=@PropertyCounter) < 3)
	BEGIN
		SELECT @code = @code + @ComponentEstateCount
		SELECT @ComponentSupplierCount = @ComponentSupplierCount + @ComponentEstateCount
	END
	ELSE
	BEGIN
		SELECT @code = @code + @ComponentUnitCount
		SELECT @ComponentSupplierCount = @ComponentSupplierCount + @ComponentUnitCount
	END

	SELECT @PropertyCounter = @PropertyCounter + 1
END

SELECT @Output
SELECT @ComponentSupplierCount

EXEC sp_executesql @Output
INSERT INTO ComponentAdditionLogs values (@Output, GETDATE())
-----------------------------------

DECLARE @ComponentSupplierOutput nvarchar(max)
DECLARE @ComponentID INT
Set @ComponentID = (select max(Id) from Component)
SET @ComponentSupplierOutput = ''
SELECT @ComponentSupplierOutput = dbo.fn_GetComponentSupplierInsertScript(@ComponentID, @ComponentSupplierCount, 320, 1)
SELECT @ComponentSupplierOutput

EXEC sp_executesql @ComponentSupplierOutput
INSERT INTO ComponentAdditionLogs values (@ComponentSupplierOutput, GETDATE())
PRINT 'Successfully added the NonDefect Components'
--------------------------------------------------------------------

DECLARE @CompID INT
SET @CompID = (SELECT MIN(Id) from Component where PropertyReference in (select PropertyRef from Property where PropertyRef like @PropRef))
UPDATE A
set A.PropertyId = P.PropertyId
FROM Component A
INNER JOIN Property P ON P.PropertyRef=A.PropertyReference
where A.Id>=@CompID
PRINT 'Successfully added the PropertyID'

Delete from ComponentSupplier 
where ComponentId in 
(select Id from Component where PropertyReference in (select PropertyRef from Property where propertyRef like @PropRef) AND ComponentType_Id IN (13,15,18))
PRINT 'Successfully deleted the Components IN - (13,15,18)'

DECLARE @OutComponentSupplier nvarchar(max)=''
select @OutComponentSupplier = @OutComponentSupplier + (CASE WHEN ComponentType_Id=13 THEN dbo.fn_GetMandESupplier(Id, 13, 0) WHEN ComponentType_Id=18 THEN dbo.fn_GetMandESupplier(Id, 18, 0) ELSE dbo.fn_GetMandESupplier(Id, 15, 0)
END) from Component where PropertyReference in (select PropertyRef from Property 
where propertyRef like @PropRef) 
AND ComponentType_Id in (13,15,18) order by ComponentType_Id
EXEC sp_executesql @OutComponentSupplier

PRINT 'Successfully added all supplier for Components IN - (13,15,18)'

PRINT 'Successfully completed the NonDefect Components Process'



