<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SSOApp.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br />
    <h3>Live Achieve Link:</h3>
    <a href="http://ohgint01/Achieve/SSODefault.aspx?env=Production">Achieve Live</a>
    <br />
   <h3>Project link - Using this page to launch SSODefault.aspx</h3>
    <a href ="SSODefault.aspx">SSODefault</a>
     <br />
    <h3>Project link - Using this page to launch SSODefault.aspx WITH Env parameter - Pilot</h3>
    <a href ="SSODefault.aspx?env=Pilot">SSODefault - Env = Pilot</a>
       <br />
    <h3> Pilot</h3>
    <a href ="http://ohgint01/Achieve/SSODefault.aspx?env=Pilot">REAL live Pilot</a>
    </div>
    </form>
</body>
</html>
