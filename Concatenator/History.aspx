<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="Concatenator.History" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>History</title>
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
                    <td><a href="Start.aspx">home</a></td>
                </tr>
            </table>
            <table align="right">
                <tr>
                    <td>user: <%=loggedInUser %></td>
                </tr>
            </table>
        </div>
        <div align="center">
            <asp:DataGrid ID="Grid" runat="server" PageSize="20" AllowPaging="True" DataKeyField="UserID" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanged="Grid_PageIndexChanged">
                <Columns>
                    <asp:BoundColumn HeaderText="UserName" DataField="FullName"></asp:BoundColumn>
                    <asp:BoundColumn HeaderText="DateTime" DataField="CreatedDate"></asp:BoundColumn>
                </Columns>
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />
                <AlternatingItemStyle BackColor="White" />
                <ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            </asp:DataGrid>
            <br />
            <br />
        </div>
    </form>
</body>
</html>
