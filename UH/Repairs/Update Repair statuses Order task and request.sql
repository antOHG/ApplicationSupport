-- 
begin tran
update rmworder set wo_status = '710' where 
wo_ref in (**enter WOref here **)



update rmtask
set task_status = '710'
where wo_ref in (**enter WOref here **)

update rmreqst
set rq_status = '500'
where rq_ref in (select rq_ref from rmworder where wo_ref in (**enter WOref here **))

commit tran 

-- To find a list of WO order status codes use this 
select * from lookup
where lu_type = 'ZWS'
--------------------------------------------

-- To find a list of WO Task status codes use this 
select * from lookup
where lu_type = 'ZTS'
--------------------------------------------

-- To find a list of WO Request status codes use this 
select * from lookup
where lu_type = 'ZRS'