<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Ajedrez_ASPX.Registro" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <div class="centrado">
        <div class="tarjeta">
            <div class="texto-centrado">
                <img src="imagenes/Logo_final.png" alt="Ajedrez 2026" class="logo" />
                <h1 class="titulo texto-centrado">REGISTRARSE</h1>
            </div>

            <span class="etiqueta">Nombre de usuario</span>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="campo" MaxLength="50" autocomplete="off" />

            <span class="etiqueta">Contraseña</span>
            <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="campo" MaxLength="100" />
            <asp:CheckBox ID="chkVer" runat="server" Text="Ver contraseña" CssClass="opcion" />

            <div class="fila-controles espacio-arriba">
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btn btn-cian"
                    OnClick="btnRegistrar_Click" />
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-azul"
                    OnClick="btnIngresar_Click" CausesValidation="false" />
            </div>

            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje" />
        </div>
    </div>
</asp:Content>
