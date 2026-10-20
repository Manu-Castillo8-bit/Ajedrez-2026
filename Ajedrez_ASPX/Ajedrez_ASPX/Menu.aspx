<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Ajedrez_ASPX.Menu" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <div class="centrado">
        <div class="tarjeta texto-centrado">

            <img src="imagenes/Logo_final.png" alt="Ajedrez 2026" class="logo" />

            <div style="display: flex; align-items: center; justify-content: center; gap: 14px; margin-bottom: 10px;">
                <asp:Image ID="imgUsuario" runat="server" CssClass="imagen-perfil" Visible="false" />
                <div style="text-align: left;">
                    <div class="dato">Usuario: <b><asp:Label ID="lblUsuario" runat="server" /></b></div>
                    <div class="dato">ELO: <b><asp:Label ID="lblElo" runat="server" Text="0" /></b></div>
                    <div class="dato">Victorias: <b><asp:Label ID="lblVictorias" runat="server" Text="0" /></b></div>
                </div>
            </div>

            <asp:Button ID="btnAjedrez" runat="server" Text="AJEDREZ" CssClass="btn btn-menu btn-cian btn-nav"
                OnClick="btnAjedrez_Click" />
            <asp:Button ID="btnDamas" runat="server" Text="DAMAS" CssClass="btn btn-menu btn-azul btn-nav btn-nav-par"
                OnClick="btnDamas_Click" />
            <asp:Button ID="btnPerfil" runat="server" Text="PERFIL" CssClass="btn btn-menu btn-azul btn-nav btn-nav-par"
                OnClick="btnPerfil_Click" />
            <asp:Button ID="btnPartidas" runat="server" Text="PARTIDAS" CssClass="btn btn-menu btn-cian btn-nav"
                OnClick="btnPartidas_Click" />
            <asp:Button ID="btnCreditos" runat="server" Text="CRÉDITOS" CssClass="btn btn-menu btn-azul btn-nav btn-nav-par"
                OnClick="btnCreditos_Click" />
            <asp:Button ID="btnAdmin" runat="server" Text="ADMINISTRACIÓN" CssClass="btn btn-menu btn-rojo btn-admin"
                OnClick="btnAdmin_Click" Visible="false" />
            <asp:Button ID="btnCambiarUsuario" runat="server" Text="CAMBIAR USUARIO" CssClass="btn btn-menu btn-verde btn-nav"
                OnClick="btnCambiarUsuario_Click" />

            <!-- Continuar partida pendiente (equivale al MessageBox del escritorio) -->
            <asp:Panel ID="pnlContinuar" runat="server" Visible="false" CssClass="tarjeta" Style="text-align: left;">
                <p class="sin-margen" id="pnlContinuarTexto" runat="server"></p>
                <div class="fila-controles espacio-arriba">
                    <asp:Button ID="btnContinuarSi" runat="server" Text="Sí, continuar" CssClass="btn btn-verde"
                        OnClick="btnContinuarSi_Click" />
                    <asp:Button ID="btnContinuarNo" runat="server" Text="No, cerrarla" CssClass="btn btn-rojo"
                        OnClick="btnContinuarNo_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
