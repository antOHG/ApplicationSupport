--SELECT     s.prop_ref, s.house_ref, s.title, s.forename, s.surname, s.post_preamble, s.post_desig, s.aline1, s.aline2, s.aline3, s.aline4, s.post_code, s.estate, s.la_name, 
--                      s.housing_officer, s.[total jobs], s.num_of_voids, p.sap_rating, t.cur_bal, r.rent, CASE WHEN r.rent = 0 THEN NULL WHEN t .cur_bal = 0 THEN NULL 
--                      ELSE t .cur_bal / r.rent END AS arrears, CASE WHEN (hb.tag_ref IS NOT NULL) THEN 'Yes' WHEN (hb.tag_ref IS NULL) THEN 'No' END AS hb
--FROM         tenagree AS t INNER JOIN
--                      property AS p ON t.prop_ref = p.prop_ref INNER JOIN
--                      scratch.dbo.sustain2 AS s ON p.prop_ref = s.prop_ref INNER JOIN
--                      rent AS r ON s.house_ref = r.house_ref LEFT OUTER JOIN
--                          (SELECT DISTINCT tag_ref ---check this as it might not be correct
--                            FROM          u_inreceiptofhb) AS hb ON t.tag_ref = hb.tag_ref
--WHERE     (t.eot = 0)
--ORDER BY s.prop_ref
  SELECT     a.house_ref, b.prop_ref, t.tag_ref, a.title, a.forename, a.surname, b.post_preamble, b.post_desig, c.aline1, c.aline2, c.aline3, c.post_code, l2.lu_desc AS estate, 
                      e.la_name, t.cur_bal, rent.rent, CASE WHEN rent.rent = 0 THEN NULL WHEN t .cur_bal LIKE '%-%' THEN NULL ELSE t .cur_bal / rent.rent END AS wks_in_arrears, 
                      CASE WHEN (hb.tag_ref IS NOT NULL) THEN '1' WHEN (hb.tag_ref IS NULL) THEN '0' END AS hb, l.pt_prop_desc AS tenure, lu.lu_desc AS housing_officer, 
                      b.sap_rating, v.[number of voids], j.[total jobs], l3.total_cases AS total_asb_cases, l4.avg_hb, l5.avg_wks_in_arrears, l6.avg AS avg_asb
FROM         (SELECT     estate, avg_hb
                       FROM          scratch.dbo.sustain_avg_final1) AS l4 INNER JOIN
                          (SELECT     lu_ref, lu_type, lu_desc
                            FROM          lookup
                            WHERE      (lu_type = 'uES')) AS l2 ON l4.estate = l2.lu_desc INNER JOIN
                          (SELECT     estate, avg_wks_in_arrears
                            FROM          scratch.dbo.sustain_avg_arr) AS l5 ON l2.lu_desc = l5.estate INNER JOIN
                          (SELECT     estate, units, num_asb, avg
                            FROM          scratch.dbo.sustain_avgtest) AS l6 ON l2.lu_desc = l6.estate RIGHT OUTER JOIN
                      tenagree AS t INNER JOIN
                      property AS b ON t.prop_ref = b.prop_ref INNER JOIN
                      rent ON b.prop_ref = rent.prop_ref INNER JOIN
                          (SELECT     pt_prop_code, pt_prop_desc, pt_prop_type
                            FROM          proptype
                            WHERE      (pt_prop_type = 'MI')) AS l ON b.cat_type = l.pt_prop_code LEFT OUTER JOIN
                          (SELECT     tag_ref, prop_ref, total_cases
                            FROM          scratch.dbo.asb_count) AS l3 ON b.prop_ref = l3.prop_ref LEFT OUTER JOIN
                          (SELECT     prop_ref, [total jobs]
                            FROM          scratch.dbo.jobcount) AS j ON b.prop_ref = j.prop_ref ON l2.lu_ref = b.u_estate LEFT OUTER JOIN
                          (SELECT     prop_ref, [number of voids]
                            FROM          scratch.dbo.sustain_void_count) AS v ON b.prop_ref = v.prop_ref LEFT OUTER JOIN
                          (SELECT     la_ref, la_name
                            FROM          lauth AS lauth_1) AS e ON b.la_ref = e.la_ref LEFT OUTER JOIN
                      member AS a ON b.house_ref = a.house_ref RIGHT OUTER JOIN
                      postcode AS c ON b.post_code = c.post_code LEFT OUTER JOIN
                          (SELECT DISTINCT tag_ref
                            FROM          u_inreceiptofhb) AS hb ON t.tag_ref = hb.tag_ref LEFT OUTER JOIN
                          (SELECT     lu_ref, lu_type, lu_desc
                            FROM          lookup AS lookup_1
                            WHERE      (lu_type = 'GHO')) AS lu ON b.housing_officer = lu.lu_ref
WHERE     (a.person_no = 1) AND (b.level_code = '3') AND (b.arr_officer <> '950') AND (t.eot = 0) AND (b.cat_type = 'GN1')
ORDER BY housing_officer, estate
--CASE WHEN r.rent = 0 THEN NULL WHEN t .cur_bal = 0 THEN NULL ELSE t .cur_bal / r.rent END AS arrears