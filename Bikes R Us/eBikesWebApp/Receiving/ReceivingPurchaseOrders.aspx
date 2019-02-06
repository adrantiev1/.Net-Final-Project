<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceivingPurchaseOrders.aspx.cs" Inherits="eBikesWebApp.Receiving.ReceivingPurchaseOrders" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <img src="/Images/DramaticMadBugs.png" alt="Logo" />
        <asp:GridView runat="server"></asp:GridView>
        <p class="lead">Receiving and returns page.</p>
    </div>
    <div class="row">
        <strong>User: <asp:Literal ID="UserName" runat="server" /></strong>
    </div>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
   
    <div class="row">
        <div class="col-md-6">
            <br />
            <asp:GridView ID="OutstandingOrderGridView" runat="server" 
                CssClass="table table-condensed table-hover"
                DataSourceID="OutstandingOrdersDataSource" 
                AutoGenerateColumns="False"
                DataKeyNames="POid">
                <Columns>
                    <asp:BoundField DataField="PONumber" HeaderText="Order" SortExpression="PONumber"></asp:BoundField>
                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date" SortExpression="OrderDate"></asp:BoundField>
                    <asp:BoundField DataField="VendorName" HeaderText="Vendor" SortExpression="VendorName"></asp:BoundField>
                    <asp:BoundField DataField="VendorPhone" HeaderText="Contact" SortExpression="VendorPhone"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="VendorId" runat="server" 
                                Value='<%# Eval("VendorId") %>' />
                            <asp:HiddenField ID="POid" runat="server"
                                Value='<%# Eval("POid") %>' />
                            <asp:LinkButton ID="DisplayOrder" Text="View Order" OnClick="DisplayOrder_Click" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Panel ID="OrderPanel" Enabled="false" Visible="false" runat="server">
                <div class="row">
                    <br />
                    <div class="col-md-2">
                        <asp:Label Text="PO: " AssociatedControlID="PONumLabel" runat="server" />
                        <asp:Label ID="PONumLabel" runat="server" />
                    </div>
                    <div class="col-md-5">
                        <asp:Label Text="Vendor: " AssociatedControlID="Vendor" runat="server" />
                        <asp:Label ID="Vendor" runat="server" />
                    </div>
                    <div class="col-md-5">
                        <asp:Label Text="Contact: " AssociatedControlID="VendorPhone" runat="server" />
                        <asp:Label ID="VendorPhone" runat="server" />
                    </div>
                </div>
                
                <div class="row">
                    <br />
                <asp:GridView ID="PurchaseOrderDetailsGridView" runat="server"
                    CssClass="table table-condensed table-hover"
                    AutoGenerateColumns="False">
                    <Columns>
                        
                        
                        <%--<asp:BoundField DataField="PartID" HeaderText="Part #" SortExpression="PartID"></asp:BoundField>--%>
                        <asp:TemplateField HeaderText="Part #">
                            <ItemTemplate>
                                <asp:Label ID="PartID" Text='<%# Eval("PartID") %>' runat="server"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"></asp:BoundField>
                        <asp:BoundField DataField="QtyOrdered" HeaderText="QtyOrdered" SortExpression="QtyOrdered"></asp:BoundField>
                        <asp:TemplateField HeaderText="QtyOutstanding">
                            <ItemTemplate>
                                <asp:Label ID="QtyOutstanding" Text='<%# Eval("QtyOutstanding") %>' runat="server"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="QtyOutstanding" HeaderText="QtyOutstanding" SortExpression="QtyOutstanding"></asp:BoundField>--%>
                        <asp:TemplateField  HeaderText="Receiving">
                            <ItemTemplate>
                                <asp:TextBox ID="ReceivingQty" TextMode="Number" Text="0" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  HeaderText="Returning">
                            <ItemTemplate>
                                <asp:TextBox ID="ReturningQty" Text="0" TextMode="Number" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  HeaderText="Reason">
                            <ItemTemplate>
                                <asp:TextBox ID="ReturnReason" runat="server" TextMode="SingleLine" />
                                <asp:HiddenField ID="POid" runat="server"
                                    Value='<%# Eval("POid") %>' />
                                <asp:HiddenField ID="PODetailID" runat="server"
                                    Value='<%# Eval("PODetailID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
                <br />
                <asp:ObjectDataSource ID="UnorderedCartDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetUnorderedPart" TypeName="eBikesSystem.BLL.ReceivingReturns.ReceivingController" DataObjectTypeName="eBikesData.Entities.POCOs.Receiving.UnorderedPart" DeleteMethod="RemoveUnorderedItem" InsertMethod="AddUnorderedItem">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="PONumLabel" PropertyName="Text" Name="purchaseOrderNum" Type="Int32"></asp:ControlParameter>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <h3>Unordered Vendor Parts</h3>
                    <asp:ListView ID="UnorderedCart" runat="server"
                        DataKeyNames="CartID"
                        DataSourceID="UnorderedCartDataSource"
                        ItemType="eBikesData.Entities.POCOs.Receiving.UnorderedPart"
                        OnItemInserting="UnorderedCart_ItemInserting" InsertItemPosition="LastItem"
                        OnItemInserted="UnorderedCart_ItemInserted"
                        >
                        
                        <EditItemTemplate>
                            <tr >
                                <td>
                                    <asp:Button runat="server" CommandName="Update" Text="Update" ID="UpdateButton" />
                                    <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" /></td>
                                <%--<td>
                                    <asp:TextBox Text='<%# Bind("PONumber") %>' runat="server" ID="PONumberTextBox" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("CartID") %>' runat="server" ID="CartIDTextBox" /></td>--%>
                                <td>
                                    <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("VendorPartNumber") %>' runat="server" ID="VendorPartNumberTextBox" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" /></td>
                            </tr>
                        </EditItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" style="border-collapse: collapse; border-style: none; border-width: 1px;">
                                <tr>
                                    <td>No data was returned.</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <InsertItemTemplate>
                            <tr style="">
                                <%--<td>
                                    <asp:Button runat="server" CommandName="Insert" Text="Insert" ID="InsertButton" />
                                    <asp:Button runat="server" CommandName="Cancel" Text="Clear" ID="CancelButton" /></td>--%>
                                <td>
                                    <asp:Button runat="server" CommandName="Insert" Text="Insert" ID="InsertButton" CssClass="btn btn-success"/>
                                    <asp:Button runat="server" CommandName="Cancel" Text="Clear" ID="CancelButton" CssClass="btn btn-default"/>
                                    <asp:TextBox Visible="false" Text='<%# Bind("PONumber") %>' runat="server" ID="PONumberTextBox" /></td>
                                <%--<td>
                                    <asp:TextBox Text='<%# Bind("CartID") %>' runat="server" ID="CartIDTextBox" /></td>--%>
                                <td>
                                    
                                    <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" CssClass="form-control"/>
                                    
                                </td>
                                
                                <td>
                                    <asp:TextBox Text='<%# Bind("VendorPartNumber") %>' runat="server" ID="VendorPartNumberTextBox" CssClass="form-control"/>
                                    
                                </td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" CssClass="form-control"/>
                                    
                                </td>
                            </tr>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <tr >
                                <td>
                                    <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" CssClass="btn btn-danger"/></td>
                                <%--<td>
                                    <asp:Label Text='<%# Eval("PONumber") %>' runat="server" ID="PONumberLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("CartID") %>' runat="server" ID="CartIDLabel" /></td>--%>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("VendorPartNumber") %>' runat="server" ID="VendorPartNumberLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server"
                                class="table table-hover table-condensed table-bordered">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" style="border-collapse: collapse; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                            <tr runat="server" >
                                                <th runat="server"></th>
                                                <%--<th runat="server">PONumber</th>
                                                <th runat="server">CartID</th>--%>
                                                <th runat="server">Description</th>
                                                <th runat="server">VendorPartNumber</th>
                                                <th runat="server">Quantity</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td runat="server" style="text-align: center; font-family: Verdana, Arial, Helvetica, sans-serif;"></td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <SelectedItemTemplate>
                            <tr style="font-weight: bold;">
                                <td>
                                    <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" /></td>
                                <%--<td>
                                    <asp:Label Text='<%# Eval("PONumber") %>' runat="server" ID="PONumberLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("CartID") %>' runat="server" ID="CartIDLabel" /></td>--%>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("VendorPartNumber") %>' runat="server" ID="VendorPartNumberLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                            </tr>
                        </SelectedItemTemplate>
                    </asp:ListView>
                    
                    
                    
                    
                </div>
                <div class="row">
                    <asp:LinkButton ID="ReceiveOrder" Text="Receive" OnClick="ReceiveOrder_Click" runat="server" CssClass="btn btn-success" />
                    <br />
                    <asp:LinkButton ID="ForceCloseOrder" Text="Force Close" OnClick="ForceCloseOrder_Click" runat="server" CssClass="btn btn-danger" />
                    <asp:Label Text="Force Close Reason" AssociatedControlID="ForceCloseText" runat="server" />
                    <asp:TextBox ID="ForceCloseText" runat="server" />
                </div>
                
            </asp:Panel>
            <asp:ObjectDataSource runat="server" ID="OutstandingOrdersDataSource" OldValuesParameterFormatString="original_{0}" SelectMethod="ListOutstandingOrder" TypeName="eBikesSystem.BLL.ReceivingReturns.ReceivingController"></asp:ObjectDataSource>
           
        </div>
    </div>
</asp:Content>
