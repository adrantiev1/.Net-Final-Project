<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BikeInHouseServicing.aspx.cs" Inherits="eBikesWebApp.Jobing.BikeInHouseServicing" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="jumbotron">
         <img src="/Images/DramaticMadBugs.png" alt="Logo" />
       
        <p class="lead">Jobing and returns page.</p> 
         <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    </div>   

    
         <asp:Panel ID="NewJobPanel" Enabled="false" Visible="false" runat="server">
                 <div class="col-md-12">
                 <h2>Add New Jobs</h2>
               
                <asp:HiddenField ID="UserNameNewJob"  runat="server" Value=""/>
                <asp:DropDownList ID="ListAllCustomers" runat="server" DataSourceID="CustomerListDB" DataTextField="FullName" DataValueField="CustomerID" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">[Select a customer]</asp:ListItem>
                </asp:DropDownList>

                 <asp:ObjectDataSource ID="CustomerListDB" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListAllCustomers" TypeName="eBikesSystem.BLL.Jobing.JobingController"></asp:ObjectDataSource>&nbsp; &nbsp; &nbsp; 

                <asp:Label ID="ShopRateLabel" AssociatedControlID="ShopRate" runat="server" >Shop Rate:</asp:Label>
                <asp:TextBox ID="ShopRate" runat="server" Text="80"></asp:TextBox>

                <asp:Label ID="VehicleIDLabel" AssociatedControlID="VehicleID" runat="server" >Vehicle ID:</asp:Label>
                <asp:TextBox ID="VehicleID" runat="server"></asp:TextBox>

                 <asp:LinkButton ID="AddNewJob" runat="server" Text="Add Job" OnClick="AddNewJob_Click" class="btn btn-primary"></asp:LinkButton>&nbsp; &nbsp; &nbsp; 
                
                 <asp:LinkButton ID="Clear1" runat="server" Text="Go Back" OnClick="Clear1_Click" class="btn btn-primary"></asp:LinkButton>&nbsp; &nbsp; &nbsp;                      
                </div>

             <div class="col-md-12">
                <asp:Label ID="Label1" runat="server" Text="Description" AssociatedControlID="DescriptionAddJob"></asp:Label>&nbsp;&nbsp;&nbsp;  
                <asp:TextBox ID="DescriptionAddJob" runat="server" Width="800"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  
                          
            </div>
            <div class="col-md-12">
                <asp:Label ID="Label2" runat="server" AssociatedControlID="CouponList"> Coupon: </asp:Label>&nbsp;&nbsp;&nbsp;  
                <asp:DropDownList ID="DropDownListAddJob" runat="server" AppendDataBoundItems="true" DataSourceID="CouponListDB" DataTextField="CouponIDValue" DataValueField="CouponID">
                 <asp:ListItem Value="0">[Select a coupon]</asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp; 
                <asp:ObjectDataSource ID="DropDownListAddJobDB" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListAllCoupon" TypeName="eBikesSystem.BLL.Jobing.JobingController"></asp:ObjectDataSource>

            <asp:Label ID="HoursAddJobLabel" runat="server" AssociatedControlID="HoursAddJob" Text="Hours: "></asp:Label>&nbsp;
                <asp:TextBox ID="HoursAddJob" runat="server" Width="50px"></asp:TextBox>
            </div>

               <div class="col-md-12">
                <asp:Label ID="CommentAddJobLabel" runat="server" AssociatedControlID="CommentAddJob" Text="Comments "></asp:Label>&nbsp;
                <asp:TextBox ID="CommentAddJob" runat="server" Width="1000px" Height="100px"></asp:TextBox>
             </div>
            </asp:Panel>
      
        <asp:Panel ID="CurrentJobList" Enabled="true" Visible="true" runat="server">
        <div class="col-md-12" >
              <h2>Current Jobs List</h2>
    
                 <strong>User: <asp:Literal ID="UserName" runat="server" /></strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;       
                <asp:LinkButton ID="NewJob" runat="server" Text="New Job" OnClick="NewJob_Click" class="btn btn-primary"></asp:LinkButton>
            <br />
        </div>
        <%--List all Jobs--%>
        <div class="col-md-12">
             <br />
            <asp:ListView ID="ShowAllJobs" runat="server" ItemType="eBikesData.Entities.POCOs.Jobbing.POCOJob" DataSourceID="AllJobsDB" OnItemCommand="ShowAllJobs_ItemCommand">
                <AlternatingItemTemplate>
                    <tr style="background-color: #FFF8DC;">
                        <td>
                            <asp:Label Text='<%# Item.JobID %>' runat="server"  ID="JobIDLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("JobDateIn") %>' runat="server" ID="JobDateInLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("JobDateStarted") %>' runat="server" ID="JobDateStartedLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("JobDateDone") %>' runat="server" ID="JobDateDoneLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("CustomerFullName") %>' runat="server" ID="CustomerFullName" /></td>
                        

                         <td>
                            <asp:Label Text='<%# Eval("Phone") %>' runat="server" ID="Phone" /></td>
                        <td>                             
                           <asp:LinkButton ID="ViewJob" runat="server" Text="View" CommandName="View" ></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="ViewService" runat="server" Text="Serving" CommandName="Serving"></asp:LinkButton>
                        </td>                    
                    </tr>
                </AlternatingItemTemplate>
                            
                <ItemTemplate>
                    <tr style="background-color: #DCDCDC; color: #000000;">
                        <td>
                            <asp:Label Text='<%# Eval("JobID") %>' runat="server" ID="JobIDLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("JobDateIn") %>' runat="server" ID="JobDateInLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("JobDateStarted") %>' runat="server" ID="JobDateStartedLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("JobDateDone") %>' runat="server" ID="JobDateDoneLabel" /></td>
                          

                         <td>
                            <asp:Label Text='<%# Eval("CustomerFullName") %>' runat="server" ID="CustomerFullName" /></td>
                        

                         <td>
                            <asp:Label Text='<%# Eval("Phone") %>' runat="server" ID="Phone" /></td>
                         <td>
                           <asp:LinkButton ID="ViewJob" runat="server" Text="View" CommandName="View" ></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="ViewService" runat="server" Text="Serving" CommandName="Serving"></asp:LinkButton>
                        </td>
                      
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                    <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                        <th runat="server">JobID</th>
                                        <th runat="server">JobDateIn</th>
                                        <th runat="server">JobDateStarted</th>
                                        <th runat="server">JobDateDone</th>
                                        <th runat="server">Customer</th>
                                        <th runat="server">Contact Number</th>

                                        <th runat="server"></th>
                                        <th runat="server"></th>
                                      
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                <asp:DataPager runat="server" ID="DataPager1">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True"></asp:NextPreviousPagerField>
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
              
            </asp:ListView>

            <asp:ObjectDataSource ID="AllJobsDB" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListAllJobs" TypeName="eBikesSystem.BLL.Jobing.JobingController"></asp:ObjectDataSource>
            
        <br />
        <br />

        </div>
        </asp:Panel>

 

        <asp:Panel ID="ViewJobPanel" Enabled="false" Visible="false" runat="server">
          
             <div class="col-md-12">
                 <h2>Current Job</h2>
    
                 <strong>User: <asp:Literal ID="UserName2" runat="server" /></strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                  <asp:Label ID="JobIDLabel1" runat="server" Text="Job:"></asp:Label>&nbsp;&nbsp;&nbsp;
                 <asp:Label ID="JobIDLabel2" runat="server" Text="Label"></asp:Label>&nbsp;&nbsp;&nbsp;
                 <asp:Label ID="CustomerLabel" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                 <asp:Label ID="ContactLabel" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;         
                 <br />
                 <br />
            </div>
            <div class="col-md-12">
                <asp:Label ID="DescriptionLabel" runat="server" Text="Description" AssociatedControlID="Description"></asp:Label>&nbsp;&nbsp;&nbsp;  
                <asp:TextBox ID="Description" runat="server" Width="800"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  
                          
                 <asp:LinkButton ID="AddServiceButton" runat="server" Text="Add Service" OnClick="AddServiceButton_Click" class="btn btn-primary"></asp:LinkButton>

                <asp:LinkButton ID="GoBack" runat="server" Text="Go Back" OnClick="GoBack_Click" class="btn btn-primary"></asp:LinkButton>
            </div>
            <div class="col-md-12">
                <asp:Label ID="CouponLabel" runat="server" AssociatedControlID="CouponList"> Coupon: </asp:Label>&nbsp;&nbsp;&nbsp;  
                <asp:DropDownList ID="CouponList" runat="server" AppendDataBoundItems="true" DataSourceID="CouponListDB" DataTextField="CouponIDValue" DataValueField="CouponID">
                 <asp:ListItem Value="0">[Select a coupon]</asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp; 
                <asp:ObjectDataSource ID="CouponListDB" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListAllCoupon" TypeName="eBikesSystem.BLL.Jobing.JobingController"></asp:ObjectDataSource>

            <asp:Label ID="HoursLabel" runat="server" AssociatedControlID="Hours" Text="Hours: "></asp:Label>&nbsp;
                <asp:TextBox ID="Hours" runat="server" Width="50px"></asp:TextBox>
            </div>

             <div class="col-md-12">
                <asp:Label ID="CommentLabel" runat="server" AssociatedControlID="Comment" Text="Comments "></asp:Label>&nbsp;
                <asp:TextBox ID="Comment" runat="server" Width="1000px" Height="100px"></asp:TextBox>
             </div>

             <div class="col-md-12">
                 <asp:ListView ID="ServiceDetails" runat="server" DataSourceID="ServiceDetailsDB" DataKeyNames="ServiceDetailID" OnItemDeleting="ServiceDetails_ItemDeleting">
                     <AlternatingItemTemplate>
                         <tr style="background-color: #FFF8DC;">
                             <td>
                                 <asp:LinkButton runat="server" CommandName="Delete" Text="Remove" ID="DeleteButton" />
                             </td>
                             <td>
                                 <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                             <td>
                                 <asp:Label Text='<%# Eval("JobHours") %>' runat="server" ID="JobHoursLabel" /></td>

                             <td>
                                 <asp:Label Text='<%# Eval("CouponIDValue") %>' runat="server" ID="CouponIDLabel" /></td>
                             <td>
                                 <asp:Label Text='<%# Eval("Comments") %>' runat="server" ID="CommentsLabel" /></td>
                             
                            
                         </tr>
                     </AlternatingItemTemplate>
                    
                     <EmptyDataTemplate>
                         <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                             <tr>
                                 <td>No data was returned.</td>
                             </tr>
                         </table>
                     </EmptyDataTemplate>
                   
                     <ItemTemplate>
                         <tr style="background-color: #DCDCDC; color: #000000;">
                             <td>
                                 <asp:LinkButton runat="server" CommandName="Delete" Text="Remove" ID="DeleteButton" />
                             </td>
                             <td>
                                 <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                             <td>
                                 <asp:Label Text='<%# Eval("JobHours") %>' runat="server" ID="JobHoursLabel" /></td>

                             <td>
                                 <asp:Label Text='<%# Eval("CouponIDValue") %>' runat="server" ID="CouponIDLabel" /></td>
                             <td>
                                 <asp:Label Text='<%# Eval("Comments") %>' runat="server" ID="CommentsLabel" /></td>
                             
                            
                         </tr>
                     </ItemTemplate>
                     <LayoutTemplate>
                         <table runat="server">
                             <tr runat="server">
                                 <td runat="server">
                                     <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                         <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                             <th runat="server"></th>
                                             <th runat="server">Description</th>
                                             <th runat="server">Hours</th>
                                             <th runat="server">Coupon</th>
                                             <th runat="server">Comments</th>
                                             
                                            
                                         </tr>
                                         <tr runat="server" id="itemPlaceholder"></tr>
                                     </table>
                                 </td>
                             </tr>
                             <tr runat="server">
                                 <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                     <asp:DataPager runat="server" ID="DataPager2">
                                         <Fields>
                                             <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True"></asp:NextPreviousPagerField>
                                         </Fields>
                                     </asp:DataPager>
                                 </td>
                             </tr>
                         </table>
                     </LayoutTemplate>                   
                 </asp:ListView>
                 <asp:ObjectDataSource ID="ServiceDetailsDB" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListServiceDetailsByJobID" TypeName="eBikesSystem.BLL.Jobing.JobingController" onDeleted="CheckForExceptions" DataObjectTypeName="eBikesData.Entities.POCOs.Jobbing.POCOServiceDetail" DeleteMethod="RemoveServiceDetail">
                     <SelectParameters>
                         <asp:ControlParameter ControlID="JobIDLabel2" PropertyName="Text" Name="jobID" Type="String"></asp:ControlParameter>

                     </SelectParameters>
                 </asp:ObjectDataSource>
              </div>

        </asp:Panel>

         <asp:Panel ID="ViewServicePanel" Enabled="false" Visible="false" runat="server">
              <div class="col-md-12">
                 <h2>Current Job Service Details</h2>
            
                 <strong>User: <asp:Literal ID="UserName3" runat="server" /></strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                  <asp:Label ID="JobIDLabel3" runat="server" Text="Job:"></asp:Label>&nbsp;&nbsp;&nbsp;
                 <asp:Label ID="JobIDLabel4" runat="server" Text="Label"></asp:Label>&nbsp;&nbsp;&nbsp;
                 <asp:Label ID="CustomerLabel2" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
                 <asp:Label ID="ContactLabel2" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;   
                  <asp:LinkButton ID="ServingGoBack" runat="server" OnClick="GoBack_Click1" Text="GoBack"  class="btn btn-primary"></asp:LinkButton>
                 <br />
            </div>

              <div class="col-md-12">
                <h3>Services</h3>
                  <asp:ListView ID="ServicesList" runat="server" DataSourceID="ServiceListDB" ItemType="eBikesData.Entities.POCOs.Jobbing.POCOServiceDetail"  OnItemCommand="ServicesList_ItemCommand">
                      <AlternatingItemTemplate>
                          <tr style="background-color: #FFF8DC;">
                            
                              <td>
                                  <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>                             
                              <td>
                                  <asp:Label Text='<%# Eval("Status") %>' runat="server" ID="StatusLabel" /></td>
                                                             
                              <td>
                                   <asp:LinkButton runat="server" CommandName="View" Text="View" ID="ServiceView" />
                              </td>
                               <td>
                                   <asp:LinkButton runat="server" CommandName="Start" Text="Start" ID="ServiceStart" />
                              </td>
                              <td>
                                   <asp:LinkButton runat="server" CommandName="Done" Text="Done" ID="ServiceDone" />
                              </td>
                                <td>
                                   <asp:LinkButton runat="server" CommandName="Remove" Text="Remove" ID="ServiceRemove" />
                              </td>
                              <asp:HiddenField Value='<%# Eval("JobHours") %>' runat="server" ID="HiddenFieldHours" />
                               
                              <asp:HiddenField Value='<%# Eval("Comments") %>' runat="server" ID="HiddenFieldComments" />
                               <asp:HiddenField Value='<%# Eval("ServiceDetailID") %>' runat="server" ID="HiddenFieldSDID" />
                              
                          </tr>
                      </AlternatingItemTemplate>
                     
                      <EmptyDataTemplate>
                          <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                              <tr>
                                  <td>No data was returned.</td>
                              </tr>
                          </table>
                      </EmptyDataTemplate>
                   
                      <ItemTemplate>
                          <tr style="background-color: #DCDCDC; color: #000000;">
                              
                              <td>
                                  <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                            
                              <td>
                                  <asp:Label Text='<%# Eval("Status") %>' runat="server" ID="StatusLabel" /></td>
                             
                             <td>
                                   <asp:LinkButton runat="server" CommandName="View" Text="View" ID="ServiceView" />
                              </td>
                               <td>
                                   <asp:LinkButton runat="server" CommandName="Start" Text="Start" ID="ServiceStart" />
                              </td>
                              <td>
                                   <asp:LinkButton runat="server" CommandName="Done" Text="Done" ID="ServiceDone" />
                              </td>
                                <td>
                                   <asp:LinkButton runat="server" CommandName="Remove" Text="Remove" ID="ServiceRemove" />
                              </td>
                              <asp:HiddenField Value='<%# Eval("JobHours") %>' runat="server" ID="HiddenFieldHours" />
                               
                              <asp:HiddenField Value='<%# Eval("Comments") %>' runat="server" ID="HiddenFieldComments" />
                               <asp:HiddenField Value='<%# Eval("ServiceDetailID") %>' runat="server" ID="HiddenFieldSDID" />
                          </tr>
                      </ItemTemplate>
                      <LayoutTemplate>
                          <table runat="server">
                              <tr runat="server">
                                  <td runat="server">
                                      <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                          <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                              
                                              <th runat="server">Description</th>                                          
                                              <th runat="server">Status</th>
                                              <th runat="server"></th>
                                              <th runat="server"></th>
                                              <th runat="server"></th>
                                              <th runat="server"></th>
                                          </tr>
                                          <tr runat="server" id="itemPlaceholder"></tr>
                                      </table>
                                  </td>
                              </tr>
                              <tr runat="server">
                                  <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                      <asp:DataPager runat="server" ID="DataPager3">
                                          <Fields>
                                              <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True"></asp:NextPreviousPagerField>
                                          </Fields>
                                      </asp:DataPager>
                                  </td>
                              </tr>
                          </table>
                      </LayoutTemplate>
                    
                  </asp:ListView>
                  <asp:ObjectDataSource ID="ServiceListDB" runat="server" OnDeleted="CheckForExceptions" OldValuesParameterFormatString="original_{0}" SelectMethod="ListServiceDetailsByJobID" TypeName="eBikesSystem.BLL.Jobing.JobingController">
                      <SelectParameters>
                          <asp:ControlParameter ControlID="JobIDLabel4" PropertyName="Text" Name="jobID" Type="String"></asp:ControlParameter>
                      </SelectParameters>
                  </asp:ObjectDataSource>
                  <br />
            </div>

             <asp:Panel ID="ViewServiceDetail" runat="server" Enabled="false" Visible="false">
                 <div class="col-md-12">
                     <asp:Label ID="ViewDescription" runat="server">Description </asp:Label>&nbsp;&nbsp;&nbsp;  
                     <asp:Label ID="ViewDescriptionText" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                     <asp:Label ID="ViewHours" runat="server">Hours </asp:Label>&nbsp;&nbsp;&nbsp;  
                     <asp:Label ID="ViewHoursText" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                     <asp:Label ID="ViewComments" runat="server">Comments </asp:Label>&nbsp;&nbsp;&nbsp;  
                     <asp:Label ID="ViewCommentsText" runat="server"></asp:Label>
                     <asp:HiddenField ID="ViewServiceDetailID" runat="server" Value="0" />                   
                </div>
                  <br />
                     <br />
             

              <div class="col-md-12">
                  <asp:LinkButton ID="AddComments" runat="server" OnClick="AddComments_Click" class="btn btn-primary" Text="Add"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                  
                  <asp:TextBox ID="EditComment" runat="server" Width="1000px"  PlaceHolder="Add any additional comments discovered while doing the service"></asp:TextBox>
                   <br />
                 <br />
              </div>

               <div class="col-md-12">
                   <asp:ListView ID="ServiceDetailPartList" runat="server" DataKeyNames="ServiceDetailPartID" ItemType="eBikesData.Entities.POCOs.Jobbing.POCOSDPart"  DataSourceID="ServiceDetailPartListDB" InsertItemPosition="LastItem" OnItemInserting="ServiceDetailPartList_ItemInserting" OnItemDeleting="ServiceDetailPartList_ItemDeleting" OnItemUpdating="ServiceDetailPartList_ItemUpdating">

                       <AlternatingItemTemplate>
                           <tr style="background-color: #FFF8DC;">
                               <td>
                                   <asp:LinkButton runat="server" CommandName="Edit" Text="Edit" ID="Button1" />
                                                                    
                               </td>
                               <td>
                                   <asp:Label Text='<%# Eval("PartID") %>' runat="server" ID="PartIDLabel" /></td>
                               <td>
                                   <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                               <td>
                                   <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                               <td> 
                                   <asp:LinkButton runat="server" CommandName="Delete" Text="Remove" ID="DeleteButton" /> </td>
                              <asp:HiddenField Value='<%# Eval("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" />
                           </tr>
                       </AlternatingItemTemplate>
                       <EditItemTemplate>
                           <tr style="background-color: #FFF8DC;">
                               <td>
                                   <asp:Button runat="server" CommandName="Update" Text="Update" ID="UpdateButton" />
                                   <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" />
                               </td>
                               <td>
                                   <asp:Label Text='<%# Bind("PartID") %>' runat="server" ID="PartIDTextBox" /></td>
                               <td>
                                   <asp:Label Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" /></td>
                               <td>
                                   <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" /></td>
                               <td> 
                                   <asp:LinkButton runat="server" CommandName="Delete" Text="Remove" ID="DeleteButton" /> </td>
                                                                                                                               
                                   <asp:HiddenField Value='<%# Bind("ServiceDetailPartID") %>' runat="server" ID="ServiceDetailPartIDTextBox" />
                           </tr>
                       </EditItemTemplate>
                       <EmptyDataTemplate>
                           <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                               <tr>
                                   <td>No data was returned.</td>
                               </tr>
                           </table>
                       </EmptyDataTemplate>
                       <InsertItemTemplate>
                           <tr style="">
                                <td>
                                   <asp:Button runat="server" CommandName="Insert" Text="Insert" ID="UpdateButton" />                                   
                               </td>
                               <td>
                                   <asp:TextBox Text='<%# Bind("PartID") %>' runat="server" ID="PartIDTextBox" /></td>
                               <td>
                                   <asp:Label Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" /></td>
                               <td>
                                   <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" /></td>
                               <td>
                                   <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="Button2" />
                               </td>
                               
                                   <asp:HiddenField Value='<%# Bind("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDTextBox" />
                                                              
                                   <asp:HiddenField Value='<%# Bind("ServiceDetailPartID") %>' runat="server" ID="ServiceDetailPartIDTextBox" />
                           </tr>
                       </InsertItemTemplate>
                       <ItemTemplate>
                           <tr style="background-color: #DCDCDC; color: #000000;">
                              <td>
                                   <asp:LinkButton runat="server" CommandName="Edit" Text="Edit" ID="Button1" />
                                                                    
                               </td>
                               <td>
                                   <asp:Label Text='<%# Eval("PartID") %>' runat="server" ID="PartIDLabel" /></td>
                               <td>
                                   <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                               <td>
                                   <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                               <td> 
                                   <asp:LinkButton runat="server" CommandName="Delete" Text="Remove" ID="DeleteButton" /> </td>
                              <asp:HiddenField Value='<%# Eval("ServiceDetailID") %>' runat="server" ID="ServiceDetailIDLabel" />

                           </tr>
                       </ItemTemplate>
                       <LayoutTemplate>
                           <table runat="server">
                               <tr runat="server">
                                   <td runat="server">
                                       <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                           <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                               <th runat="server"></th>
                                               <th runat="server">Part ID</th>
                                               <th runat="server">Description</th>
                                               <th runat="server">Qty</th>
                                               <th runat="server"></th>
                                           </tr>
                                           <tr runat="server" id="itemPlaceholder"></tr>
                                       </table>
                                   </td>
                               </tr>
                               <tr runat="server">
                                   <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;"></td>
                               </tr>
                           </table>
                       </LayoutTemplate>                       
                   </asp:ListView>

                   <asp:ObjectDataSource ID="ServiceDetailPartListDB" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListAllSDPartsBySDID" TypeName="eBikesSystem.BLL.Jobing.JobingController" OnDeleted="CheckForExceptions" OnUpdated="CheckForExceptions" OnInserted="CheckForExceptions" DataObjectTypeName="eBikesData.Entities.POCOs.Jobbing.POCOSDPart" DeleteMethod="RemoveSDPart" InsertMethod="AddNewSDPart" UpdateMethod="UpdateSDPart">

                       <SelectParameters>
                           <asp:ControlParameter ControlID="ViewServiceDetailID" PropertyName="Value" Name="SDID" Type="Int32"></asp:ControlParameter>


                       </SelectParameters>
                   </asp:ObjectDataSource>
          
               </div>
             </asp:Panel>

        </asp:Panel>
 
   
</asp:Content>
