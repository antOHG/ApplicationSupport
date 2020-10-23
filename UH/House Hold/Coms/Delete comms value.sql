select * from comms where comms_value = '01628419437'

begin tran
delete from comms 
where comms_sid = '66421180' and comms_value = '01628419437'
rollback

commit tran