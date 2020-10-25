SELECT     a.house_ref, b.prop_ref, t.tag_ref, a.title, a.forename, a.surname, a.age, a.gender, RTRIM(b.post_preamble) + ' ' + RTRIM(b.post_desig) + ', ' + RTRIM(c.aline1) 
                      + ', ' + RTRIM(c.aline2) + ', ' + c.aline3 AS address, c.post_code, l2.lu_desc AS estate, e.la_name, CONVERT(varchar(10), t.cot, 103) AS tenancy_start, t.cur_bal, 
                      CASE WHEN rent.rent = 0 THEN NULL WHEN t .cur_bal LIKE '%-%' THEN NULL ELSE t .cur_bal / rent.rent END AS wks_in_arrears, 
                      CASE WHEN (hb.tag_ref IS NOT NULL) THEN 'Yes' WHEN (hb.tag_ref IS NULL) THEN 'No' END AS hb, l.pt_prop_desc AS tenure
FROM         tenagree AS t INNER JOIN
                      property AS b ON t.prop_ref = b.prop_ref INNER JOIN
                      rent ON b.prop_ref = rent.prop_ref INNER JOIN
                          (SELECT     pt_prop_code, pt_prop_desc, pt_prop_type
                            FROM          proptype
                            WHERE      (pt_prop_type = 'MI')) AS l ON b.cat_type = l.pt_prop_code LEFT OUTER JOIN
                          (SELECT DISTINCT tag_ref
                            FROM          u_inreceiptofhb) AS hb ON t.tag_ref = hb.tag_ref LEFT OUTER JOIN
                          (SELECT     lu_ref, lu_type, lu_desc
                            FROM          lookup
                            WHERE      (lu_type = 'uES')) AS l2 ON b.u_estate = l2.lu_ref LEFT OUTER JOIN
                          (SELECT     la_ref, la_name
                            FROM          lauth AS lauth_1) AS e ON b.la_ref = e.la_ref LEFT OUTER JOIN
                      member AS a ON b.house_ref = a.house_ref RIGHT OUTER JOIN
                      postcode AS c ON b.post_code = c.post_code
WHERE     (b.level_code = '3') AND (b.arr_officer <> '950') AND (t.eot = 0)
ORDER BY estate