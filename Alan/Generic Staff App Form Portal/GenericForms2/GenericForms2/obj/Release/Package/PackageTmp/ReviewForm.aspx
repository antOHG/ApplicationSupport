<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewForm.aspx.cs" Inherits="GenericForms2.ReviewForm" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
        <asp:Panel ID="QAPanel" runat="server" >
            <asp:Table ID="QATable" runat="server" style="margin-bottom: 0px" HorizontalAlign="Left" Width="1000px">
            </asp:Table>
        </asp:Panel>
    
        <asp:Button ID="Button1" runat="server" Text="Approve" OnClick="Button1_Click" />
   </asp:Content>