<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OnlineShoppingAndCheckout.aspx.cs" Inherits="eBikesWebApp.Sales.OnlineShoppingAndCheckout" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <br />
    <h1>Part Catalog Available <i class="glyphicon glyphicon-wrench"></i> Online and In-Store</h1>


    <div class="row">
        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    </div>
    <div class="col-md-6">
        <h2>Browse by Category</h2>

        <asp:GridView ID="CatGridView" runat="server" AutoGenerateColumns="False" DataSourceID="CategoryDataSource" DataKeyNames="CategoryID"
            GridLines="None" BorderStyle="None" CssClass="table table-hover">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:HyperLink runat="server" NavigateUrl="OnlineShoppingAndCheckout.aspx" Text="All Categories" > </asp:HyperLink>
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField ID="CategoryIdHidden" Value='<%# Eval("CategoryID") %>' runat="server" />
                        <asp:LinkButton OnClick="CategoryDesc_Click" Text='<%# Eval("CategoryDesc") %>' runat="server" ID="CategoryDescLabel" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CategoryCount" SortExpression="CategoryCount"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="col-md-6">

        <h2>Products</h2>
        <asp:LinkButton ID="vieCart"  runat="server" OnClick="viewCart_Click" class="btn btn-primary">View Shopping Cart <i class="glyphicon glyphicon-shopping-cart"></i></asp:LinkButton>
        <asp:GridView ID="PartGridView" runat="server"
            AutoGenerateColumns="False"
            Visible="false" GridLines="None" BorderStyle="None"
            AllowPaging="True" OnPageIndexChanging="PartGridView_PageIndexChanging"
            DataKeyNames="PartID" CssClass="table table-hover">
            <PagerSettings Mode="NumericFirstLast"
                Position="Top"
                FirstPageText="First"
                PageButtonCount="3"
                LastPageText="Last" />
            <PagerStyle
                Height="30px"
                VerticalAlign="Bottom"
                HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField ID="CategoryId" runat="server" Value='<%# Eval("CategoryId") %>' />
                        <asp:HiddenField ID="PartID" runat="server" Value='<%# Eval("PartID") %>' />
                        <asp:Button Text="Add" ID="AddToCart" OnClick="AddToCart_Click" runat="server" />
                        <asp:Label ID="qtyInCart" Text='<%# Eval("QtyInCart") %>' runat="server" />
                        <asp:TextBox ID="QTY" runat="server" Text="1" Width="40px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SellingPrice" SortExpression="SellingPrice" DataFormatString="{0:c}" ItemStyle-CssClass="t-cost"></asp:BoundField>
                <asp:BoundField DataField="PartDesc" SortExpression="PartDesc" ></asp:BoundField>
                <asp:BoundField DataField="QtyOnHand" SortExpression="QtyOnHand" DataFormatString=" {0} in stock"></asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:ObjectDataSource ID="CategoryDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="CategoriesList" TypeName="eBikesSystem.BLL.Sales.SalesController"></asp:ObjectDataSource>
</asp:Content>
