<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Ajedrez_ASPX.Default" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <div class="centrado">
        <div class="tarjeta">
            <div class="texto-centrado">
                <img src="imagenes/Logo_final.png" alt="Ajedrez 2026" class="logo" />
                <h1 class="titulo texto-centrado">INICIAR SESIÓN</h1>
            </div>

            <span class="etiqueta">Nombre de usuario</span>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="campo" MaxLength="50" autocomplete="off" />

            <span class="etiqueta">Contraseña</span>
            <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="campo" MaxLength="100" />
            <asp:CheckBox ID="chkVer" runat="server" Text="Ver contraseña" CssClass="opcion" />

            <div class="fila-controles espacio-arriba">
                <asp:Button ID="btnIngresar" runat="server" Text="Verificar" CssClass="btn btn-cian"
                    OnClick="btnIngresar_Click" />
                <asp:Button ID="btnRegistrarse" runat="server" Text="Registrarse" CssClass="btn btn-azul"
                    OnClick="btnRegistrarse_Click" CausesValidation="false" />
            </div>

            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje" />
        </div>
    </div>
</asp:Content>
