<%@ Page Title="Temperature" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Temperature.aspx.cs" Inherits="Domotics.Temperature" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/TemperatureStyleSheet.css" rel="stylesheet" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">


    <div id="Wrapper">


    
        <table class="center">
            <tr>
                <td id="title" colspan="3" >
                    <h1 style="text-align:center">Temperature managment</h1>
                </td>
            </tr>

            <tr>
                <td colspan="2">

                    <table class="center">
                        <tr>
                            <td>
                                <p>Last retrived temperature</p>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Loading ..." Visible="true" Font-Size="X-Large"/>
                            </td>
                        </tr>

                    </table>

                    </td>

                 <td colspan="2">

                    <asp:Button runat="server" ID="Button1" Text="Refresh" CssClass="center"/>

                    </td>
              </tr>
                    
         

            <tr>
                <td colspan="1">
                    
                    <table class="center">
                        <tr>
                            <td>
                                <p>Mean all time</p>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Loading ..." Visible="true" Font-Size="Small"/>
                            </td>
                        </tr>

                    </table>

                </td>
                <td colspan="1">
                    <table class="center">
                        <tr>
                            <td>
                                <p>Max all time</p>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Loading ..." Visible="true" Font-Size="Small"/>
                            </td>
                        </tr>

                    </table>
                </td>
                <td colspan="1">
                    <table class="center">
                        <tr>
                            <td>
                                <p>Min all time </p>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Loading ..." Visible="true" Font-Size="Small"/>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>

        </table>

        </div>

</asp:Content>
