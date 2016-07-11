<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attachment.aspx.cs" Inherits="AttachmentPage.Attachment" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link href="Css/Style.css" rel="stylesheet" />
    <style>
        .modal-vertical-centered {
            transform: translate(0, 50%) !important;
            -ms-transform: translate(0, 50%) !important;
            -webkit-transform: translate(0, 50%) !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">

            <nav id="navTitle" runat="server" class="navbar navbar-default navbar-fixed-top" role="navigation">
                <div class="navbar-header">
                    <img class="navbar-brand" src="Images/ultimus.png" />
                    <p class="navbar-text">Proceso: <strong><span id="LblPprocessName" runat="server" /></strong>, Incidente: <strong><span id="LblIncident" runat="server" /></strong></p>
                </div>
            </nav>

            <div class="panel panel-default">

                <div class="panel-heading">
                    <h3 class="panel-title">Documentos Adjuntos
                    <span id="LblCount" runat="server" class="badge pull-right" />
                    </h3>
                </div>

                <div class="panel-body">

                    <div id="view1" runat="server" class="container-fluid">
                        <asp:Repeater ID="RptFiles1" runat="server">
                            <ItemTemplate>
                                <div class="col-xs-6 col-sm-4 col-md-3 text-center">
                                    <div class="thumbnail">
                                        <asp:ImageButton ID="BtnDeleteFile" CssClass="pull-left" runat="server" ImageUrl="Images/trash-16.png" data-file-name='<%# Eval("FileName") %>' Visible='<%# Eval("EnableDelete") %>' OnClick="BtnDeleteFile_Click" />
                                        <asp:ImageButton ID="BtnGetFile" CssClass="pull-right" runat="server" ImageUrl="Images/download-16.png" data-file-name='<%# Eval("FileName") %>' OnClick="BtnGetFile_Click" />
                                        <div class="center-block">
                                            <img src="Images/Icons/<%# Eval("FileType") %>.png" onerror="this.src='Images/Icons/_blank.png';" />
                                        </div>
                                        <h6><%# Eval("FileName") %></h6>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <div id="view2" runat="server">
                        <table class="table table-striped">
                            <asp:Repeater ID="RptFiles2" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <img src="Images/Icons/<%# Eval("FileType") %>.png" onerror="this.src='Images/Icons/_blank.png';" />
                                        </td>
                                        <td>
                                            <asp:Label ID="LblFileName" runat="server" Text='<%# Eval("FileName") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="BtnGetFile" runat="server" ImageUrl="Images/download.png" data-file-name='<%# Eval("FileName") %>' OnClick="BtnGetFile_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="BtnDeleteFile" runat="server" ImageUrl="Images/trash.png" data-file-name='<%# Eval("FileName") %>' Visible='<%# Eval("EnableDelete") %>' OnClick="BtnDeleteFile_Click" /> 
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>

                </div>

            </div>

            <div id="LblError" runat="server" class="alert alert-danger">
                <button type="button" class="close" aria-hidden="true">&times;</button>
            </div>

            <table id="navUpload" runat="server" class="navbar navbar-default navbar-fixed-bottom table table-bordered">
                <tr>
                    <td>
                        <asp:FileUpload ID="FupAttach" runat="server" CssClass="filestyle" data-buttonText="Examinar" />
                    </td>
                    <td>
                        <asp:Button ID="BtnUpload" runat="server" Text="Subir" OnClick="BtnUpload_Click" CssClass="btn btn-primary btn-block" />
                    </td>
                </tr>
            </table>

            <div id="modalDialog" tabindex="-1" role="dialog" class="modal">
                <div class="modal-dialog modal-sm modal-vertical-centered">
                    <div class="modal-content">
                        <div class="modal-body">
                            <p id="LblDialogMessage"></p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>


        </div>
    </form>

    <script src="Scripts/jquery.min.js"></script>
    <script src="bootstrap/js/bootstrap.min.js"></script>
    <script src="Scripts/bootstrap-filestyle.min.js"></script>

    <script>
        function alert(message) {
            $('#LblDialogMessage').html(message);
            $('#modalDialog').modal('show');
        }
    </script>

</body>
</html>
