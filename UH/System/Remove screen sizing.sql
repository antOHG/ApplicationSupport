/****** Resolve missing screen issue in UH first find user code from auser table and add to the query below, Running bottom delete will remove property screen size  ******/
SELECT TOP 1000 [id]
      ,[user_code]
      ,[container_no]
      ,[field_no]
      ,[obj_top]
      ,[obj_left]
      ,[obj_width]
      ,[obj_height]
      ,[user_control]
      ,[tabindex]
      ,[gridrowheight]
      ,[apposf_sid]
      ,[tstamp]
      ,[comp_avail]
      ,[comp_display]
  FROM [uh630live].[dbo].[apposf]
  where user_code = 'C1W'
  
  
  begin tran
  delete from apposf
  where user_code = 'C1W' and id = 'MPROPU'
  commit tran