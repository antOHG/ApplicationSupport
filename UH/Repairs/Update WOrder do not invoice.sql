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
