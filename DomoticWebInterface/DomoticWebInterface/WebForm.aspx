<%@ Page Title="WebForm" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="DomoticWebInterface.WebForm" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="css/WebFormStyleSheet.css" rel="stylesheet" type="text/css" media="screen" />

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

            <ContentTemplate>


                <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick"></asp:Timer>
                              
               
                <div id ="realTimeValues" style="float:right" class="frame">

                    <p style="text-align:center">Room real time :</p>
                    
                    
                    <ul>
                            <li>
                                <p>Lumosity</p>                         
                            </li>

                            <li>
                                 <p> value :
                                     <span style="display:inline-block; vertical-align:middle">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/img/lum16.gif" CssClass="luminosity" />
                                     </span>
                                 </p>
                            </li>


                            <li>
                                <p>Light is : 
                                    <span style="display:inline-block; vertical-align:middle">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/img/on.gif" CssClass="interruptor"/>
                                    </span>
                                </p>
                            </li>
                            

                            <li>
                                <p>Auto managment is 
                                    <span style="display:inline-block; vertical-align:baseline">
                                        <asp:Label ID="Label1" runat="server" Text="On" Font-Bold="true" ForeColor="Red" />
                                    </span>
                                </p>
                            </li>                                                  
                    </ul>


                    <ul>
                            <li>
                                <p>Temperature : </p>                         
                            </li>

                            <li>
                                 <p> value :
                                     <span style="display:inline-block; vertical-align:middle">
                                        <asp:Label ID="Label2" runat="server" Text="30" Font-Bold="true" />
                                     </span>
                                  C°</p>
                            </li>


                            <li>
                                <p>Heating system is : 
                                    <span style="display:inline-block; vertical-align:middle">
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/img/off.gif" CssClass="interruptor"/>
                                    </span>
                                </p>
                            </li>
                            

                            <li>
                                <p>Auto managment is 
                                    <span style="display:inline-block; vertical-align:baseline">
                                        <asp:Label ID="Label3" runat="server" Text="On" Font-Bold="true" ForeColor="Red" />
                                    </span>
                                </p>
                            </li>                                                  
                    </ul>

                       <p style="text-align:center">There is <asp:Label ID="Label4" runat="server" Text="a presence in the room" Font-Bold="true" /> in the room.</p>

                </div>



            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <div style="width:100%;text-align:center;" >

        <p>Chart of temperature :</p>

        

         <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                <asp:ListItem Text="Last 12 hours" Value="0" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Last 24 hours" Value="1"></asp:ListItem>
                <asp:ListItem Text="Last week" Value="2"></asp:ListItem>
    </asp:DropDownList>


    </div>

    

    <asp:Chart ID="Chart1" runat="server" Width="550px" 
                    Height="350px" EnableViewState="true" CssClass="centerChart">
                    
    </asp:Chart>

    

   


</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
</asp:Content>
