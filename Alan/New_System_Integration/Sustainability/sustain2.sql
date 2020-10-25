select prop_ref, COUNT(eot) AS [number of voids] into scratch..sustain_void_count from tenagree
where  (cot > GETDATE() - 1826)
                      group by prop_ref