<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CartFinalizeSale.aspx.cs" Inherits="eBikesWebApp.Sales.CartFinalizeSale" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <h1>Checkout</h1>
    <br />
    <div class="row">
        <uc1:MessageUserControl runat="server" ID="MessageUserControl"  />
    </div>
        <asp:Panel runat="server" ID="ExcludedPanle" Visible="false">
            <blockquote class="alert alert-warning">
                <asp:Label ID="EcludedLabel" runat="server" />
            </blockquote>
        </asp:Panel>

    <div class="row">
        <div class="col-md-4">
            <asp:LinkButton ID="ViewCart" Text="View Cart" runat="server" OnClick="ViewCart_Click"/>
            <asp:BulletedList ID="BulletedList1" runat="server">
                <asp:ListItem Text="Change qty" />
                <asp:ListItem Text="Remove item" />
            </asp:BulletedList>
            <asp:LinkButton ID="ContinueShopping" Text="Continue shopping" runat="server" OnClick="ContinueShopping_Click"/>
        </div>
        <div class="col-md-4">
            <asp:LinkButton ID="PurchaseInfo" Text="Purchase Info" runat="server" OnClick="PurchaseInfo_Click"/>
            <asp:BulletedList ID="BulletedList2" runat="server">
                <asp:ListItem Text="Customer Info" />
                <asp:ListItem Text="Hot to purchase/pay" />
                <asp:ListItem Text="type" />
            </asp:BulletedList>
        </div>
        <div class="col-md-4">
            <asp:LinkButton ID="PlaceOrderLink" Text="Place Order" runat="server" OnClick="PlaceOrderLink_Click" />
            <asp:BulletedList ID="BulletedList3" runat="server">
                <asp:ListItem Text="Coupon info" />
                <asp:ListItem Text="Coupon_Apply" />
                <asp:ListItem Text="subtotal/GST/Total" />
                <asp:ListItem Text="Place Order" />
            </asp:BulletedList>
        </div>
    </div>
    <div class="row">
        <br />
        <br />
        <h1>Your Shopping Cart <i class="glyphicon glyphicon-shopping-cart"></i></h1>
        <hr />
        <div class="row">
            <br />
            <asp:Label ID="InfoRow" runat="server" />

        </div>
        <asp:GridView ID="CartGridView" runat="server"
            AutoGenerateColumns="false"
            Visible="false" GridLines="None" BorderStyle="None" CssClass="table table-condensed">
            <Columns>

                <asp:BoundField DataField="PartDesc" SortExpression="PartDesc"></asp:BoundField>
                <asp:BoundField DataField="Qty" SortExpression="Qty"></asp:BoundField>
                <asp:BoundField DataField="SellPrice" SortExpression="SellPrice" DataFormatString="{0:c}" ItemStyle-CssClass="t-cost"></asp:BoundField>
                <asp:BoundField DataField="itemTotalPrice" SortExpression="itemTotalPrice" DataFormatString="{0:c}" ItemStyle-CssClass="t-cost"></asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField ID="CartItemId" runat="server" Value='<%# Eval("CartItemId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
    <div class="row">
        <div class="col-md-3">
            <asp:TextBox ID="Coupon" runat="server"></asp:TextBox>
            <asp:LinkButton ID="ApplyCoupon" runat="server" OnClick="ApplyCoupon_Click">Apply Coupon</asp:LinkButton>
        </div>
        <div class="col-md-3">
        </div>

        <div class="col-md-3">
            <asp:Label runat="server" Text="Sub Total:"></asp:Label>&nbsp;&nbsp;
                             <asp:Label ID="SubTotal" runat="server"></asp:Label>
            <br />

            <asp:Label ID="DiscountLabel" runat="server" Text="Discount:" Visible="false"></asp:Label>&nbsp;&nbsp;
                             <asp:Label ID="Discount" runat="server" Visible="false"></asp:Label>

            <br />

            <asp:Label runat="server" Text="GST:"></asp:Label>&nbsp;&nbsp;
                             <asp:Label ID="TotalGST" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" Text="Total:"></asp:Label>&nbsp;&nbsp;
                             <asp:Label ID="TotalPrice" runat="server"></asp:Label>

        </div>
        <div class="col-md-3">

            <asp:RadioButtonList ID="PaymentType" runat="server">
                <asp:ListItem Text="Debit" />
                <asp:ListItem Text="Credit" />
            </asp:RadioButtonList>

            <br />
            <asp:LinkButton ID="PlaceOrder" Text="Place Order" runat="server" class="btn btn-primary" OnClick="PlaceOrder_Click" />
            <asp:LinkButton ID="ReturnToShopping" Text="Return To Shop" runat="server" Visible="false" class="btn btn-primary" OnClick="ReturnToShopping_Click"/>
        </div>
    </div>

</asp:Content>
