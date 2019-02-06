<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="eBikesWebApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <img src="Images/DramaticMadBugs.png" alt="Logo" />
        
        <p class="lead">The Team Project Web Application for DMIT 2018.</p>
    </div>
    
    <div class="row">

         <div class="col-md-12">
            <h2>Shared</h2>
            
                <ul>
                    <li>Create Solution - Alex</li>
                    <li>Setup Navigation - Emily</li>
                    <li>Reverse Engineer from DB - Bo</li>
                    <li>Add User Controls & About Page - Anton</li>
                    <li>Documentaion of Default Page - Alex</li>
                </ul>
        </div>

        <div class="col-md-3">
            <h2>Purchasing <span class="glyphicon glyphicon-tags"></span></h2>
            <p>
                Responsibility - Emily
            </p>
            <div class="progress">
                <div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 100%;">
                    100%
                </div>
            </div>
           
            
        </div>
        <div class="col-md-3">
            <h2>Sales <span class="glyphicon glyphicon-barcode"></span></h2>
            
            <p>
                Responsibility - Anton
            </p>
             <div class="progress">
                <div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 100%;">
                    100%
                </div>
            </div>
          
        </div>
        <div class="col-md-3">
            <h2>Receiving <span class="glyphicon glyphicon-send"></span></h2>
            <p>
                Responsibility - Alex
            </p>
             <div class="progress">
                <div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 100%;">
                    100%
                </div>
            </div>
           
            
        </div>
        <div class="col-md-3">
            <h2>Jobing <span class="glyphicon glyphicon-user"></span></h2>
            <p>
                Responsibility - Bo
            </p>
             <div class="progress">
                <div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 100%;">
                    100%
                </div>
            </div>
          
            
        </div>
       
    </div>
    <div class="row">
        <div class="col-md-6">
            <h1>Known Bugs</h1>
            <h3>V2.0</h3>
            <p>Database Bug in Parts: QtyOnOrder is incorrect.</p>
        </div>
    </div>

</asp:Content>
