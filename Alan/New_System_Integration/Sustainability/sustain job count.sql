
drop table scratch..jobcount

SELECT     prop_ref, COUNT(wo_ref) AS [total jobs]
into scratch..jobcount FROM         rmworder AS a
WHERE     (created > GETDATE() - 1826) AND (cancelled_date = 0) 
GROUP BY prop_ref