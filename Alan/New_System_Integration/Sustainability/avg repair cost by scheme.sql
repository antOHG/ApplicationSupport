SELECT     r.wo_ref, r.prop_ref, p.man_scheme, u.NAME AS scheme_name, derivedtbl_1.pt_prop_desc AS tenure, p.post_preamble, p.post_desig, pc.aline1, pc.aline2, pc.aline3, 
                      pc.aline4, pc.post_code, r.created, r.completed, r.act_cost, lk2.lu_desc AS repair_type, lk3.lu_desc AS wo_status
FROM         (SELECT     lu_ref, lu_type, lu_desc
                       FROM          lookup AS lookup_1
                       WHERE      (lu_type = 'ZWS')) AS lk3 INNER JOIN
                          (SELECT     lu_ref, lu_type, lu_desc
                            FROM          lookup
                            WHERE      (lu_type = 'ZRT')) AS lk2 INNER JOIN
                      rmworder AS r ON lk2.lu_ref = r.rep_type ON lk3.lu_ref = r.wo_status RIGHT OUTER JOIN
                          (SELECT     prop_ref, man_scheme
                            FROM          scratch.dbo.ab) AS z ON r.prop_ref = z.prop_ref LEFT OUTER JOIN
                      postcode AS pc RIGHT OUTER JOIN
                          (SELECT     pt_prop_desc, pt_prop_type, pt_prop_code
                            FROM          proptype
                            WHERE      (pt_prop_type = 'MI')) AS derivedtbl_1 INNER JOIN
                      property AS p ON derivedtbl_1.pt_prop_code = p.cat_type INNER JOIN
                      u_scheme_name AS u ON p.man_scheme = u.ANL_CODE ON pc.post_code = p.post_code ON r.prop_ref = p.prop_ref
WHERE     (r.cancelled_date = 0)
ORDER BY scheme_name



--select distinct man_scheme   into scratch..avg_man_scheme from scratch..avgrepaircost2 
