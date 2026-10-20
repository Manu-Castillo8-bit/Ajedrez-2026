<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ajedrez.aspx.cs" Inherits="Ajedrez_ASPX.Ajedrez" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContenidoEncabezado" ContentPlaceHolderID="EncabezadoExtra" runat="server">
    <link href="Content/ajedrez.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Retardo antes de que la IA responda, para que el usuario vea su jugada -->
            <asp:Timer ID="TimerIA" runat="server" Enabled="false" OnTick="TimerIA_Tick" />

            <div class="pagina-juego">
                <div class="zona-tablero">
                    <table id="tablero" runat="server" class="tablero"></table>

                    <!-- Equivalente al diálogo de promoción del escritorio -->
                    <asp:Panel ID="pnlPromocion" runat="server" Visible="false" CssClass="panel-promocion">
                        <span class="subtitulo">Promoción</span>
                        <div class="fila-controles">
                            <asp:Button ID="btnPromReina" runat="server" Text="Reina" CssClass="btn btn-cian"
                                OnClick="btnPromReina_Click" />
                            <asp:Button ID="btnPromTorre" runat="server" Text="Torre" CssClass="btn btn-cian"
                                OnClick="btnPromTorre_Click" />
                            <asp:Button ID="btnPromAlfil" runat="server" Text="Alfil" CssClass="btn btn-cian"
                                OnClick="btnPromAlfil_Click" />
                            <asp:Button ID="btnPromCaballo" runat="server" Text="Caballo" CssClass="btn btn-cian"
                                OnClick="btnPromCaballo_Click" />
                        </div>
                    </asp:Panel>
                </div>

                <div class="panel-lateral">
                    <asp:Label ID="puntosb" runat="server" CssClass="titulo-lateral" />
                    <div class="fila-puntos">
                        <asp:Label ID="puntos_b" runat="server" CssClass="tira-piezas" />
                        <asp:Label ID="p_blancas" runat="server" CssClass="cuenta-puntos" Text="0" />
                    </div>

                    <asp:Label ID="puntosn" runat="server" CssClass="titulo-lateral" />
                    <div class="fila-puntos">
                        <asp:Label ID="puntos_n" runat="server" CssClass="tira-piezas" />
                        <asp:Label ID="p_negras" runat="server" CssClass="cuenta-puntos" Text="0" />
                    </div>

                    <asp:Label ID="consejo" runat="server" CssClass="consejo etiqueta-consejo" />

                    <div class="fila-controles espacio-arriba">
                        <asp:Button ID="deshacer" runat="server" Text="Deshacer" CssClass="btn btn-gris"
                            OnClick="deshacer_Click" />
                        <asp:Button ID="ir_menu" runat="server" Text="Menú" CssClass="btn btn-azul"
                            OnClick="ir_menu_Click" />
                    </div>

                    <asp:Label ID="lblAviso" runat="server" CssClass="mensaje" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
