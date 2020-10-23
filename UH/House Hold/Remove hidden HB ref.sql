select * from tenambr where tag_ref = 'H0008271/01'

begin tran
update tenambr
set foreign_ref = ''
where tag_ref = 'H0008271/01'

commit tran