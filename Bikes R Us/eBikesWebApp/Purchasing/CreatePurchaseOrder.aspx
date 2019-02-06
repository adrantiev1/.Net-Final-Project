<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreatePurchaseOrder.aspx.cs" Inherits="eBikesWebApp.Purchasing.CreatePurchaseOrder" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <div class="page-header">
        <h1>Purchasing <small>modify and place a current order.</small></h1>
         <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
         <asp:Label ID="EmployeeName" runat="server" />
         
       <div>
        <div class="col-md-6">
            <%--vendor drop down datasource--%>
            <asp:ObjectDataSource ID="VendorDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListVendorNames" TypeName="eBikesSystem.BLL.Purchasing.PurchasingController"></asp:ObjectDataSource>
            
            <asp:DropDownList ID="VendorDropDown" runat="server" DataSourceID="VendorDataSource" DataTextField="Name" DataValueField="VendorID" CssClass="form-control" style="display: inline-block;" AppendDataBoundItems="true">
                <asp:ListItem Value=" ">Select a vendor</asp:ListItem>
            </asp:DropDownList> 
           
            <asp:LinkButton ID="GetPurchaseOrder" runat="server" OnClick="GetPurchaseOrder_Click" CssClass="btn btn-primary">Get/Create PO</asp:LinkButton>
            </div>
       </div>
       <div class="col-md-6">
            <asp:Label ID="Vendor" runat="server">Vendor Name: </asp:Label>
            <asp:TextBox ID ="VendorName" runat="server" ReadOnly="true" CssClass="form-control" style="display: inline-block;"></asp:TextBox> <br /> &nbsp; &nbsp; &nbsp; &nbsp; 
            <asp:Label ID="Label1" runat="server">Location: </asp:Label>
            <asp:TextBox ID ="Location" runat="server" ReadOnly="true" CssClass="form-control" style="display: inline-block;"></asp:TextBox><br />&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            <asp:Label ID="Label2" runat="server">Phone: </asp:Label>
            <asp:TextBox ID="Phone" runat="server" ReadOnly="true" CssClass="form-control" style="display: inline-block;"></asp:TextBox>
       </div>
    </div>
     <asp:Panel ID="OrderDetailsPanel" visible="false" Enabled="false" runat="server">
     <div class="col-md-12">
        <h4> Current Order Details</h4>
     </div>
<%--       current order grid view--%>
         <div class="col-md-12">
          <asp:GridView ID="CurrentOrderGridView" runat="server" AutoGenerateColumns="false" ItemType="eBikesData.Entities.POCOs.Purchasing.CurrentOrder" OnRowCommand="CurrentOrderGridView_RowCommand" ViewStateMode="Enabled"  CssClass="table table table-hover table-condensed">
              <Columns>
                   <asp:TemplateField HeaderText="PartID" SortExpression="PartID">
                     <ItemTemplate>
                       <asp:HiddenField runat="server" Value='<%# Bind("PurchaseOrderID") %>' ID="PurchaseOrderID"></asp:HiddenField>
                       <asp:HiddenField runat="server" Value='<%# Bind("PurchaseOrderDetailID") %>' ID="PurchaseOrderDetailID"></asp:HiddenField>
                       <asp:Label runat="server" Text='<%# Bind("PartID") %>' ID="PartID"></asp:Label>
                   </ItemTemplate>
                </asp:TemplateField>
               <asp:TemplateField HeaderText="Description" SortExpression="Description">
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="Description"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="QOH" SortExpression="QOH">
                   
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("QOH") %>' ID="QOH"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="QOO" SortExpression="QOO">
                  
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("QOO") %>' ID="QOO"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="ROL" SortExpression="ROL">
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("ROL") %>' ID="ROL"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Qty" >
                   <ItemTemplate>
                       <asp:Textbox runat="server" Text='<%# Bind("Qty") %>' ID="Qty"></asp:Textbox>
                       <asp:HiddenField runat="server" Value='<%# Bind("Buffer") %>' ID="Buffer"></asp:HiddenField>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Price" SortExpression="Price">
                   <ItemTemplate>
                       <asp:Textbox runat="server" Text='<%# Bind("Price") %>' ID="Price"></asp:Textbox>
                   </ItemTemplate>
               </asp:TemplateField>
                  <asp:ButtonField CommandName="Remove" Text="Remove" />
              </Columns>
          </asp:GridView>
      </div>
     <div class="col-md-12">
          <div class="col-md-4">
             
          </div>
          <div class="col-md-4">
                <asp:LinkButton ID="Update" runat="server" OnClick="Update_Click" CssClass="btn btn-primary">Update</asp:LinkButton>
                <asp:LinkButton ID="Place" runat="server" OnClick="Place_Click" CssClass="btn btn-primary">Place</asp:LinkButton>
                <asp:LinkButton ID="Clear" runat="server" OnClick="Clear_Click" CssClass="btn btn-primary">Clear</asp:LinkButton>
                <asp:LinkButton ID="Delete" runat="server" OnClick="Delete_Click" CssClass="btn btn-primary">Delete</asp:LinkButton>
          </div>
          <div class="col-md-4">
               
          </div>
      </div>
      <div class="col-md-12">
          <div class="col-md-4">
              <asp:Label ID="SubLabel" runat="server">Subtotal</asp:Label>
              <asp:TextBox ID="Subtotal" runat="server" CssClass="form-control" style="display: inline-block;" ReadOnly="true"></asp:TextBox> 
          </div>
          <div class="col-md-4">
                <asp:Label ID="Label8" runat="server">GST</asp:Label>
               <asp:TextBox ID="GST" runat="server" CssClass="form-control" style="display: inline-block;" ReadOnly="true"></asp:TextBox>
          </div>
          <div class="col-md-4">
                <asp:Label ID="Label9" runat="server">Total</asp:Label>
               <asp:TextBox ID="Total" runat="server" CssClass="form-control" style="display: inline-block;" ReadOnly="true"></asp:TextBox>
          </div>
      </div>
   

     <%--  inventory grid view--%>
    <div class="col-md-12">
        <h4>Current Inventory Details</h4>
    </div>
     <div class="col-md-12">
           <asp:GridView ID="CurrentInventoryGridView" runat="server"  AutoGenerateColumns="false" CssClass="table table table-hover table-condensed" ItemType="eBikesData.Entities.POCOs.Purchasing.CurrentOrder" OnRowCommand="CurrentInventoryGridView_RowCommand" ViewStateMode="Enabled">
           <Columns>
               <asp:TemplateField HeaderText="PartID" SortExpression="PartID">
                   <ItemTemplate>
                       <asp:HiddenField runat="server" Value='<%# Bind("PurchaseOrderID") %>' ID="PurchaseOrderID"></asp:HiddenField>
                       <asp:Label runat="server" Text='<%# Bind("PartID") %>' ID="PartID"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Description" SortExpression="Description">
                  
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="Description"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="QOH" SortExpression="QOH">
                   
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("QOH") %>' ID="QOH"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="QOO" SortExpression="QOO">
                  
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("QOO") %>' ID="QOO"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="ROL" SortExpression="ROL">
                  
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("ROL") %>' ID="ROL"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Buffer" SortExpression="Buffer">
                
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("Buffer") %>' ID="Buffer"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="Price" SortExpression="Price">
                  
                   <ItemTemplate>
                       <asp:Label runat="server" Text='<%# Bind("Price") %>' ID="Price"></asp:Label>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:ButtonField CommandName="Add" Text="Add" />
           </Columns>
       </asp:GridView>
     </div>
     </asp:Panel>
       
</asp:Content>
