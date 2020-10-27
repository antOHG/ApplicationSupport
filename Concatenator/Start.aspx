<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="Concatenator.Start" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SAF Concatenator</title>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <table>
                <tr>
                    <td>
                        <h1>SAF Concatenator</h1>
                    </td>
                </tr>
            </table>
        </div>
        
        <hr />

        <div align="center">
            <table align="left">
                <tr>
                    <td><a href="History.aspx">history</a></td>
                </tr>
            </table>
            <table align="right">
                <tr>
                    <td>user: <%=loggedInUser %></td>
                </tr>
            </table>
        </div>

        
        <div align="center">
            <table>
                <tr>
                    <td>Click this button to start the process: </td>
                    <td><asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="<< Start Process >>" /></td>
                </tr>
            </table>
        </div>

        <div align="center">
            <table>
                <tr>
                    <td>
                        <br />
                        <asp:Label ID="Label1" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
