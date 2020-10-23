SELECT tenagree.tag_ref, tenagree.prop_ref, tenagree.house_ref, 'Missing Household prop_ref' as Action
FROM  tenagree LEFT OUTER JOIN
               househ ON tenagree.house_ref = househ.house_ref
WHERE (tenagree.eot = '')
and househ.prop_ref = '' and tenagree.prop_ref > '' and tag_ref <>'999998'