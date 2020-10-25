/*
STEPS:

1. UPDATE the 
@PostCode
@DefaultSupplier --GET THE SUPPLIER NUMBER USING THE BELOW MENTIONED QUERY

2. Execute the 'MAIN SCRIPT', if the result shows several successful messages - then the activity is completed

3. If the result shows "Components already exists", then DELETE the components by running the following with appropriate @PropRef:

BEGIN TRAN
USE [One.Components]
DECLARE @Postcode nvarchar(50) = 'CM2 7TL%'
DELETE FROM ComponentSupplier where ComponentId in (select Id from Component where PropertyReference in (select PropertyRef from Property where Postcode like @Postcode))
DELETE FROM Component where PropertyReference in (select PropertyRef from Property where Postcode like @Postcode)
ROLLBACK

4. If the above is successful, execute the script # 3 with COMMIT action

5. Now, execute the 'MAIN SCRIPT' to complete this activity


--QUERY TO GET THE SUPPLIER NUMBER
select * from Supplier where name like 'david%'
*/

--MAIN SCRIPT

USE [One.Components]
DECLARE @PostCode nvarchar(50) = 'CM2 7TL%'
Declare @DefaultSupplier INT = 576 --@DEFAULT SUPPLIER@

IF (select count(*) from Component where PropertyReference in (select PropertyRef from Property where postcode like @PostCode))>0
BEGIN
	PRINT 'Components already exists'
	RETURN
END

DECLARE @Property table ([ID] [int] IDENTITY(1,1) NOT NULL, prop_ref nvarchar(50), level_code int)
insert into @Property select replace(propertyRef, ' ', ''), levelCode from property where Postcode like @PostCode order by PropertyRef
select * from @Property

DECLARE @code INT
DECLARE @PropertyCounter INT
DECLARE @PropertyMax INT
DECLARE @TempEstateBlockCOmponentID INT
DECLARE @ComponentSupplierCount INT
DECLARE @Output nvarchar(max)

SET @code = (select max(Code)+1 from Component)
SET @PropertyCounter = 1
SET @PropertyMax = (select count(*) from @Property)
SET @Output = ''
SET @ComponentSupplierCount = 0

WHILE (@PropertyCounter <= @PropertyMax)
BEGIN
	SELECT @Output = @Output + dbo.fn_GetDefectInsertScript((Select prop_ref from @Property where ID=@PropertyCounter), @code, (Select level_code from @Property where ID=@PropertyCounter))

	IF ((Select level_code from @Property where ID=@PropertyCounter) < 3)
	BEGIN
		SELECT @code = @code + 6
		SELECT @ComponentSupplierCount = @ComponentSupplierCount + 6
	END
	ELSE
	BEGIN
		SELECT @code = @code + 7
		SELECT @ComponentSupplierCount = @ComponentSupplierCount + 7
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

SELECT @ComponentSupplierOutput = dbo.fn_GetComponentSupplierInsertScript(@ComponentID, @ComponentSupplierCount, 320, 0) --ALTERNATIVE SUPPLIER
SELECT @ComponentSupplierOutput = @ComponentSupplierOutput + dbo.fn_GetComponentSupplierInsertScript(@ComponentID, @ComponentSupplierCount, @DefaultSupplier, 1) --DEFAULT SUPPLIER

SELECT @ComponentSupplierOutput
EXEC sp_executesql @ComponentSupplierOutput
INSERT INTO ComponentAdditionLogs values (@ComponentSupplierOutput, GETDATE())
PRINT 'Successfully added the Defect Components'
--------------------------------------------------------------------

DECLARE @CompID INT
SET @CompID = (SELECT MIN(Id) from Component where PropertyReference in (select PropertyRef from Property where postcode like @PostCode))

update A
set A.PropertyId = P.PropertyId
FROM Component A
INNER JOIN Property P ON P.PropertyRef=A.PropertyReference
where A.Id>=@CompID

PRINT 'Successfully completed the Defect Components Process'


