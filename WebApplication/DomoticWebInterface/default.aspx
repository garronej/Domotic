<%@ Page Title="Domotic" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="DomoticWebInterface.WebForm" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="css/WebFormStyleSheet.css" rel="stylesheet" type="text/css" media="screen" />

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Timer ID="Timer1" runat="server" Interval="5000" OnTick="Timer1_Tick" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
        <ContentTemplate>


            <div id="realTimeValues" style="float: right; margin-right: 15px" class="shadow frame">

                <p style="text-align: center">
                    Control panel ( last refresh :&nbsp;
                        <asp:Label ID="Label3" runat="server" Text="Not yet" />
                    &nbsp;)&nbsp;
                    <span style="display: inline-block; vertical-align: middle">
                        <asp:Image ID="LoadingGif" ImageUrl="~/img/loading.gif" runat="server" CssClass="hidden" />
                    </span>

                </p>


                <ul>
                    <li>
                        <p>Lumosity : </p>
                    </li>

                    <li>
                        <p>
                            <span style="display: inline-block; vertical-align: middle">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/img/lum16.gif" CssClass="luminosity" />
                            </span>
                        </p>
                    </li>


                    <li>
                        <p>
                            &nbsp;&nbsp;&nbsp;Light 
                                    <span style="display: inline-block; vertical-align: middle">
                                        <asp:ImageButton ID="Image2" runat="server" ImageUrl="~/img/on.gif" CssClass="interruptor" OnClientClick="ShowLoading()" OnClick="Image2_Click" />
                                    </span>
                        </p>
                    </li>


                    <li>
                        <p>
                            Automatic
                                    <span style="display: inline-block; vertical-align: middle">
                                        <asp:ImageButton ID="Image4" runat="server" ImageUrl="~/img/onAlt.gif" CssClass="interruptor" OnClick="Image4_Click" OnClientClick="ShowLoading()" />
                                    </span>
                        </p>
                    </li>
                </ul>


                <ul>
                    <li>
                        <p>Temperature :</p>
                    </li>

                    <li>
                        <p>
                            <span style="display: inline-block; vertical-align: middle">
                                <asp:Label ID="Label2" runat="server" Text="25" Font-Bold="true" />
                            </span>
                            C°
                        </p>
                    </li>


                    <li>
                        <p>
                            Heating 
                                    <span style="display: inline-block; vertical-align: middle">
                                        <asp:ImageButton ID="Image3" runat="server" ImageUrl="~/img/off.gif" CssClass="interruptor" OnClick="Image3_Click" OnClientClick="ShowLoading()" />
                                    </span>
                        </p>
                    </li>


                    <li>
                        <p>
                            Automatic  
                                    <span style="display: inline-block; vertical-align: middle">
                                        <asp:ImageButton ID="Image5" runat="server" ImageUrl="~/img/offAlt.gif" CssClass="interruptor" OnClick="Image5_Click" OnClientClick="ShowLoading()" />
                                    </span>
                        </p>
                    </li>
                </ul>

                <p style="text-align: center">
                    There is currently
                    <asp:Label ID="Label4" runat="server" Text="a presence in the room" Font-Bold="true" />
                    in the room.
                </p>

            </div>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <div id="accordion">

        <h3>Temperature record</h3>
        <div>


            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>





                    <div style="width: 100%; text-align: center;">

                        <p>
                            Chart of temperature :

        

         <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true">
             <asp:ListItem Text="Last 12 hours" Value="0" Selected="True"></asp:ListItem>
             <asp:ListItem Text="Last 24 hours" Value="1"></asp:ListItem>
             <asp:ListItem Text="Last week" Value="2"></asp:ListItem>
         </asp:DropDownList>
                            <asp:Image ID="LoadingGif2" ImageUrl="~/img/loading.gif" runat="server" CssClass="hidden" />

                        </p>


                    </div>




                    <div style="width: 100%; text-align: center;">

                        <asp:Chart ID="Chart1" runat="server" Width="1000px"
                            Height="300px" EnableViewState="true" CssClass="centerChart">

                            <Series>
                                <asp:Series Name="Series1" />
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                            </ChartAreas>

                        </asp:Chart>

                    </div>
                </ContentTemplate>

            </asp:UpdatePanel>






        </div>
        

    </div>


</asp:Content>



