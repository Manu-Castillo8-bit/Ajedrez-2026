<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Creditos.aspx.cs" Inherits="Ajedrez_ASPX.Creditos" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <div class="centrado">
        <div class="tarjeta texto-centrado" style="max-width: 560px;">

            <asp:Label ID="lblTitulo" runat="server" Text="AGRADECIMIENTOS" CssClass="titulo"
                style="display: block; text-align: center; letter-spacing: 2px;" />

            <asp:Image ID="imgLogo" runat="server" ImageUrl="~/imagenes/Logo_final.png"
                AlternateText="Ajedrez 2026"
                style="display: block; width: 113px; height: 104px; object-fit: contain; margin: 16px auto;" />

            <asp:Label ID="lblCuerpo" runat="server"
                style="display: block; text-align: center; white-space: pre-line; font-size: 15px; line-height: 1.5; max-width: 464px; margin: 0 auto; color: var(--text);" />

            <div class="fila-controles" style="justify-content: center; margin-top: 20px; margin-bottom: 0;">
                <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" CssClass="btn btn-azul"
                    OnClick="btnCerrar_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</asp:Content>
