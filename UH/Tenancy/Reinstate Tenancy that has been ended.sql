-- This script is used to reinstate a tenency, ONLY USE THIS IF THERE IS NO NEW TENANCY STARTED IF THIS IS THE CASE LOG WITH CIVICA!
-- To start replace all prop refs, house hold and tenancy refs in this code.

begin tran

Update tenagree set eot = '01/01/1900'  where tag_ref = 'H0008875/01'
Update tenagree set present = 'True' where tag_ref = 'H0008875/01'
Update tenagree set terminated = 'False' where tag_ref = 'H0008875/01'
Update tenagree set occ_status = 'OCC' where tag_ref = 'H0008875/01'
update tenagree set u_real_reason = '' where tag_ref = 'H0008875/01'
update tenagree set reason_term = '' where tag_ref = 'H0008875/01'
update tenagree set keysrecd_dt = '' where tag_ref = 'H0008875/01'
update tenagree set potentialenddate = '' where tag_ref = 'H0008875/01'

--Delete out the vmrecord table entry for the property, run the select query and find the Vmrecord reference to match the tenancy you are reinstating. 
--once found amend the /000 reference on the end. 
select * from vmrecord where prop_ref = 'STPANC003013'
begin tran
delete from vmrecord where vm_propref = 'STPANC003013/002'


--Delete out the vmstats table entry for the void record for the property. run the select query and find the Vmstats reference to match the tenancy you are reinstating. 
--once found amend the /000 reference on the end. 
select * from vmstats where vm_propref like 'STPANC003013%'
begin tran
delete from vmstats where vm_propref = 'STPANC003013/002'

-- You should have already updated this in the 1st step so run this to reny and occ status fields. 
begin tran
Update rent set house_ref = 'H0008875' where prop_ref = 'STPANC003013'
Update rent set occ_status = 'OCC' where prop_ref = 'STPANC003013'

--Check that avail field in rent = 0 if this is not already the case run the code below.
select avail from rent where prop_ref = 'STPANC003013'
begin tran
Update rent set avail = 0 where prop_ref = 'STPANC003013'

-- Run all 3 of these together providing all refrences have been updatedin the 1st step.
begin tran
Update property set house_ref = 'H0008875' where prop_ref = 'STPANC003013'

Update househ set prop_ref = 'STPANC003013' where house_ref = 'H0008875'

Update debcharg set tag_ref = 'H0008875/01' where prop_ref = 'STPANC003013'

Commit tran
