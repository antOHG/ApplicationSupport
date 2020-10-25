<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AvailableForms.aspx.cs" Inherits="GenericForms2.AvailableForms" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

   

   
        <div>
          Logged in as   <%=User.Identity.Name%>
        </div>
    <div>
    
        
   <table>
       <tr>
           <th>Forum</th>
           <th>FormType</th>
           <th></th>
       </tr>
       <tr>
           <td>
               <asp:DropdownList ID="RefGroupDropDown" runat="server"></asp:DropdownList>
           </td>
           <td>
               <asp:DropdownList ID="FormNameDropDown" runat="server"></asp:DropdownList>
           </td>
           <td>
               <asp:Button ID="RefreshButton" runat="server" Text="Refresh" OnClick="RefreshButton_Click" />
           </td>
       </tr>
   </table>
    
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    
    </div>
 
</asp:Content>