<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Domotics.Home" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/HomeStyleSheet.css" rel="stylesheet" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <div id="Wrapper">

        <p>
            The <strong>Domotic inteface</strong> web application is the control point of your home electronical device.
        </p>
        <p>
            You car retrive information on temperature, and luminosity as well as controling your light.
        </p>

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>






        <asp:UpdatePanel ID="UpdatePanel1" runat="server">

            <ContentTemplate>


                <asp:Timer ID="Timer1" runat="server" Interval="6000" OnTick="Timer1_Tick"></asp:Timer>

               

                <asp:Chart ID="Chart1" runat="server" Width="550px"
                    Height="350px" EnableViewState="true">
                    
                </asp:Chart>




            </ContentTemplate>



        </asp:UpdatePanel>






    </div>
</asp:Content>





