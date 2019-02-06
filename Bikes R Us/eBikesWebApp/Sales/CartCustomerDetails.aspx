<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CartCustomerDetails.aspx.cs" Inherits="eBikesWebApp.Sales.CartCustomerDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Checkout</h1>
    <br />
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
            <asp:LinkButton ID="PurchaseInfo" Text="Purchase Info" runat="server"  OnClick="PurchaseInfo_Click"/>
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
        <h1>Purchase Details <i class="glyphicon glyphicon-shopping-cart"></i></h1>
        <hr />
        <p>Enter your information for shipping and billing here.</p>
        <br />

        <div class="col-md-6">
            <h2>Billing Details</h2>

            <hr />
            <asp:Label ID="Label1" runat="server" Text="Name"></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server" Columns="60"  readonly="true"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server" Columns="60"  readonly="true"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Address"></asp:Label>
            <asp:TextBox ID="TextBox3" runat="server" Columns="60"  readonly="true"></asp:TextBox>
            <br />
            <asp:Label ID="Label4" runat="server" Text="phone"></asp:Label>
            <asp:TextBox ID="TextBox4" runat="server" Columns="60" readonly="true"></asp:TextBox>
            <br />
            <br />
        </div>
        <div class="col-md-6">

            <h2>Shipping Details</h2>
            <hr />
            <asp:Label ID="Label5" runat="server" Text="Name"></asp:Label>
            <asp:TextBox ID="TextBox5" runat="server" Columns="60"  readonly="true"></asp:TextBox>
            <br />
            <asp:Label ID="Label6" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="TextBox6" runat="server" Columns="60"  readonly="true"></asp:TextBox>
            <br />
            <asp:Label ID="Label7" runat="server" Text="Address"></asp:Label>
            <asp:TextBox ID="TextBox7" runat="server" Columns="60"  readonly="true"></asp:TextBox>
            <br />
            <asp:Label ID="Label8" runat="server" Text="phone"></asp:Label>
            <asp:TextBox ID="TextBox8" runat="server" Columns="60"  readonly="true"></asp:TextBox>
            <br />
            <br />
        </div>
        
       
        
        

    </div>
    <div class="row">
        <div class="col-md-3">
             <asp:LinkButton ID="BackToReview" Text="Back" runat="server" OnClick="BackToReview_Click" class="btn btn-primary"/>
        </div>
        <div class="col-md-3">

        </div>
        <div class="col-md-3">

        </div>
        <div class="col-md-3">
            <asp:LinkButton ID="ContinueToPurchase" Text="Continue" runat="server" OnClick="ContinueToPurchase_Click" class="btn btn-primary"/>
        </div>
    </div>


</asp:Content>
