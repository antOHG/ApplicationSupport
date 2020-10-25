--select estate, round(avg(wks_in_arrears * 1.0), 1) as avg_wks_in_arrears from scratch..sustain_main group by estate

---without rounding up: 

--select estate, avg(wks_in_arrears * 1.0)as avg_wks_in_arrears   from scratch..sustain_main group by estate order by estate


--select estate, avg(hb * 1.0) as avg_hb from scratch..sustain_main group by estate order by estate

--select estate, avg(total_asb_cases) as avg_asb from scratch..sustain_main group by estate order by estate --number of incidents divided number of GN properties.