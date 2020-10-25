USE uh630live
/*
UPDATE THE FOLLOWING IN THE BELOW QUERY AND EXECUTE IN TEST WITH ROLLBACK MODE
#Property references#
#DISPOSAL Date#
#DISPOSAL Reason#

VERIFY EVERYTHING, BEFORE EXECUTING IN LIVE WITH COMMIT MODE
*/

BEGIN TRAN

DECLARE @Property table ([ID] [int] IDENTITY(1,1) NOT NULL, prop_ref nvarchar(50))

INSERT INTO @Property --## ENTER THE PROPERTY REFERENCES BELOW UNION -ACCORDINGLY ##
SELECT 'XSUTTON21120'
UNION
SELECT 'XSUTTON21787'
UNION 
SELECT 'XSUTTON21788' 
UNION 
SELECT 'XSUTTON21789' 
UNION 
SELECT 'XSUTTON21790' 
UNION 
SELECT 'XSUTTON21791' 
UNION 
SELECT 'XSUTTON21792' 
UNION 
SELECT 'XSUTTON21794' 
UNION 
SELECT 'XSUTTON21795' 
UNION 
SELECT 'XSUTTON21796'

--select * from @Property order by ID

DECLARE @DisposalDate datetime
DECLARE @DisposalReason nvarchar(10) -- RUN FOR REASON LIST --> select * from lookup where lu_type = 'uDI'

SET @DisposalDate = '2017-12-11 00:00:00.000' --## ENTER THE DISPOSAL DATE ##
SET @DisposalReason = '019' --## ENTER THE DISPOSAL REASON ##

DECLARE @PropertyCounter INT
DECLARE @PropertyMax INT
SET @PropertyCounter = 1
SET @PropertyMax = (select count(*) from @Property)

WHILE (@PropertyCounter <= @PropertyMax)
BEGIN
	
	select prop_ref from @Property where ID=@PropertyCounter
	------------
	--UPDATE PROPERTY DETAILS
	UPDATE property SET occ_stat='DEM', housing_officer='950', arr_officer='950', maintresp='DEM', area_office='999', rep_area='099', rep_subarea='099', u_dem_disp='1'
	,u_dispose_reason=@DisposalReason, u_dispose_date=@DisposalDate
	WHERE prop_ref = (select prop_ref from @Property where ID=@PropertyCounter)
	
	--UPDATE RENT DETAILS
	UPDATE rent SET occ_status = 'DEM' WHERE prop_ref = (select prop_ref from @Property where ID=@PropertyCounter)
	------------
	
	SELECT @PropertyCounter = @PropertyCounter + 1
	
	WAITFOR DELAY '00:00:01'
END

SELECT * FROM auditlog WHERE field='u_dispose_date' and keyval in (select prop_ref from @Property)

ROLLBACK





/*
--OLD QUERY, JUST FOR REFERENCE

--Update Property Details
begin tran
update property
set 
housing_officer = '950', 
ownership = '014',
arr_officer = '950',
maintresp = 'DEM',
area_office = '999',
rep_area = '099', 
rep_subarea = '099',
u_dispose_reason = '003', -- RUN FOR REASON LIST --> select * from lookup where lu_type = 'uDI'
u_dispose_date = '2014-04-01 00:00:00'
u_dem_disp = '1'
Where prop_ref = 'XHIGHRO21455'


--Update Rent Details
begin tran
update rent
set occ_status = 'DEM'
Where prop_ref = 'XHIGHRO21455'

commit


-- Use these to query a property a like to copy details from to restore above
select housing_officer, ownership, arr_officer, maintresp, area_office, rep_area, rep_subarea, u_dispose_reason, u_dispose_date, major_ref  from property
where prop_ref = 'COLLEG001800'

select occ_status from rent
where prop_ref = 'COLLEG001800'

select u_dispose_date from property
where prop_ref in ('COLLEG001800')

select u_dispose_reason from property
where prop_ref in ('COLLEG001800')

*/

