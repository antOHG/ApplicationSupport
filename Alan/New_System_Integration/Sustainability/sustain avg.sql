--SELECT     sustain_main.estate, COUNT(sustain_main.prop_ref) AS units, sustain_num_asb.num_asb
--into scratch..sustain_asb_by_unit
--FROM         sustain_main INNER JOIN
--                      sustain_num_asb ON sustain_main.estate = sustain_num_asb.estate
--GROUP BY sustain_main.estate, sustain_num_asb.num_asb

select *, cast(num_asb as decimal) / units as avg
into scratch..sustain_avgtest
from sustain_asb_by_unit
group by estate, units, num_asb