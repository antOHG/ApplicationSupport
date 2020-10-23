begin tran

--first updating where not garage
update h set h.prop_ref = t.prop_ref from househ h , tenagree t where 
t.prop_ref <> '' and h.prop_ref = '' and t.active = 1 and t.present = 1 and t.terminated = 0 
and t.house_ref = h.house_ref and t.tenure <> 'GL'

--updating the rest the remaning with the garage tenure being allowed this time
update h set h.prop_ref = t.prop_ref from househ h , tenagree t where 
t.prop_ref <> '' and h.prop_ref = '' and t.active = 1 and t.present = 1 and t.terminated = 0 
and t.house_ref = h.house_ref
commit
