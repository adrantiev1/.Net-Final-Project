<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CartPreview.aspx.cs" Inherits="eBikesWebApp.Sales.CartPreview" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Checkout</h1>
    <br />
    <div class="row">
        <div class="col-md-4">
            <asp:LinkButton ID="ViewCart" Text="View Cart" runat="server" OnClick="ViewCart_Click" />
            <asp:BulletedList ID="BulletedList1" runat="server">
                <asp:ListItem Text="Change qty" />
                <asp:ListItem Text="Remove item" />
            </asp:BulletedList>
            <asp:LinkButton ID="ContinueShopping" Text="Continue shopping" runat="server" OnClick="ContinueShopping_Click" />
        </div>
        <div class="col-md-4">
            <asp:LinkButton ID="PurchaseInfo" Text="Purchase Info" runat="server" OnClick="PurchaseInfo_Click" />
            <asp:BulletedList ID="BulletedList2" runat="server">
                <asp:ListItem Text="Customer Info" />
                <asp:ListItem Text="Hot to purchase/pay" />
                <asp:ListItem Text="type" />
            </asp:BulletedList>
        </div>
        <div class="col-md-4">
            <asp:LinkButton ID="PlaceOrder" Text="Place Order" runat="server" OnClick="PlaceOrder_Click" />
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
        <div class="row">
            <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
        </div>
        <hr />
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
                        <asp:Label ID="OutOfStock" runat="server"  CssClass="text-danger" Text='<%# Eval("StockAvailabillty") %>'/>
                        <asp:HiddenField ID="CartItemId" runat="server" Value='<%# Eval("CartItemId") %>' />
                        <asp:Button Text="Delete" ID="DeleteItem" OnClick="DeleteItem_Click" runat="server" />
                        <asp:TextBox ID="QtyTextBox" runat="server"  Width="50px" Text='<%# Eval("Qty") %>'/>
                        <asp:Button Text="Update" ID="UpdateItemQty" OnClick="UpdateItemQty_Click" runat="server" />
                    </ItemTemplate>

                </asp:TemplateField>

                <%--<asp:BoundField DataField="QtyOnHand" SortExpression="QtyOnHand" DataFormatString=" {0} in stock"></asp:BoundField>--%>
            </Columns>

        </asp:GridView>


    </div>
    <div class="row">
        <div class="col-md-3">

        </div>
        <div class="col-md-3">

        </div>
        <div class="col-md-6">
            <asp:Label ID="Total" runat="server" Text="Total:"></asp:Label>&nbsp;&nbsp;
                             <asp:Label ID="TotalPrice" runat="server"></asp:Label>
        <br />
        <asp:Label ID="InfoRow" runat="server"/>
        <br />
        <asp:LinkButton ID="ContinueToCustDetails" Text="Continue" runat="server" OnClick="ContinueToCustDetails_Click" class="btn btn-primary"/>
        </div>
        
        
    </div>
    
</asp:Content>
