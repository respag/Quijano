<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notificacion.aspx.cs" Inherits="ULA.Quijano.SendMails.Notificacion" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
</head>
<body>
    <form id="form1" runat="server" role="form">
        <div>

            <p>Estimado(a)</p>
            <p>Se ha generado un solicitud de Mesa de Operaciones, con incidente número <asp:Label ID="txtIncidente" runat="server"></asp:Label>.</p>
            <p>Para acceder a la tarea ingrese <asp:HyperLink id="txtEnlace" runat="server">AQUÍ</asp:HyperLink>.</p>
            <p>Nota: Este es un mensaje automático, por favor no responder.</p>

            <br />

            <h3>
                <asp:Label id="txtTitulo" runat="server"></asp:Label>
            </h3>

            <div id="divPasante" runat="server" visible="false">

                <table>
                    <tr>
                        <td>Solicitador por:</td>
                        <td>
                            <asp:Label id="TxtSolicitadoPor" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Entidad:</td>
                        <td>
                            <asp:Label id="txtEntidad" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Dirección:</td>
                        <td>
                            <asp:Label id="txtDireccion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Trámite:</td>
                        <td>
                            <asp:Label id="txtTramite" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Detalles:</td>
                        <td>
                            <asp:Label id="txtDetalles" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>

            </div>

            <div id="divMensajero" runat="server" visible="false">

                <table>
                    <tr>
                        <td>Solicitador por:</td>
                        <td>
                            <asp:Label id="TxtSolicitadoPor2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Entidad:</td>
                        <td>
                            <asp:Label id="txtEntidad2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Para:</td>
                        <td>
                            <asp:Label id="txtPara" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>De:</td>
                        <td>
                            <asp:Label id="txtDe" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Dirección:</td>
                        <td>
                            <asp:Label id="txtDireccion2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Piso:</td>
                        <td>
                            <asp:Label id="txtPiso" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Calle:</td>
                        <td>
                            <asp:Label id="txtCalle" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Referencias del lugar de envío:</td>
                        <td>
                            <asp:Label id="txtReferenciaLugarEnvio" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Horarios:</td>
                        <td>
                            <asp:Label id="txtHorarios" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Teléfonos:</td>
                        <td>
                            <asp:Label id="txtTelefonos" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Asunto:</td>
                        <td>
                            <asp:Label id="txtAsunto" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Acciones:</td>
                        <td>
                            <asp:Label id="txtAcciones" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>

            </div>

            <br />

            <table>
                <tr>
                    <td>Estado:</td>
                    <td>
                        <asp:Label id="txtEstado" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Responsable:</td>
                    <td>
                        <asp:Label id="txtResponsable" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Instrucciones:</td>
                    <td>
                        <asp:Label id="txtComentario" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>

        </div>
    </form>

</body>
</html>
