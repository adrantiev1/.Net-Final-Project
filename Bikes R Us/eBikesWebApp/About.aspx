<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="eBikesWebApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Your application description page.</h3>
    <p>Use this area to provide additional information.</p>

    <div class="col-md-3">
        <h2>Alex Dvorkin</h2>

        <img src="Images/alex.png" alt="Alternate Text" />
        
    </div>
    <div class="col-md-3">
        <h2>Emily Urdaneta</h2>

        <img src="Images/Em.jpg" alt="Alternate Text" />

    </div>
    <div class="col-md-3">
        <h2>Anton Drantiev</h2>

        <img src="Images/an.png" alt="Alternate Text" />

    </div>
    <div class="col-md-3">
        <h2>Bo Pang</h2>

        <img src="Images/bo.jpg" alt="Alternate Text" />

    </div>
    <div class="col-md-12">
        <h2>Security Roles</h2>
        <p>
            Admin: Webmaster

        </p>
        <h3>Security information</h3>
        <div class="col-md-3">
            <h4>Administrators:</h4>
            <p>UserName: WebMaster</p>
            <p>Email: Webmaster@eBikes.ca</p>
            <p>Password: Pa$$w0rd</p>
           
        </div>
        <div class="col-md-3">
            <h4>Customers:</h4>
            <p>UserName:</p>
            <p>Email: </p>
            <p>Password: Pa$$w0rd</p>
        </div>
        <div class="col-md-3">
            <h4>Reciving:</h4>
            <p>UserName: </p>
            <p>Email: </p>
            <p>Password: Pa$$w0rd</p>
        </div>
        <div class="col-md-3">
            <h4>Services:</h4>
            <p>UserName: </p>
            <p>Email: </p>
            <p>Password: Pa$$w0rd</p>
        </div>
        <p>All users are unregistered to start with. Unregistered users are imported from the employee table. When an employee is registred through the ebikes user registration page by the Webmaster they are automaticly assigned to their roles</p>
    </div>
    <div class="col-md-12">
        <h3>Connection string information</h3>
        <code>  name="eBikesDb" connectionString="Data Source=.;Initial Catalog=eBikes;Integrated Security=True" providerName="System.Data.SqlClient" </code>
    </div>

</asp:Content>
