<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Damas.aspx.cs" Inherits="Ajedrez_ASPX.Damas" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContenidoEncabezado" ContentPlaceHolderID="EncabezadoExtra" runat="server">
    <link href="Content/damas.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <div class="pagina">
        <h1 class="titulo">DAMAS</h1>

        <div class="damas-marco">
            <div class="tarjeta damas-tablero-tarjeta">
                <asp:ScriptManager ID="smPagina" runat="server" />
                <asp:UpdatePanel ID="upDamas" runat="server">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="phTablero" runat="server" />

                        <asp:Label ID="lblAviso" runat="server" CssClass="mensaje damas-aviso" />

                        <!-- Confirmación de "Ir al menú" (igual que el MessageBox del escritorio) -->
                        <asp:Panel ID="pnlMenu" runat="server" CssClass="damas-confirmacion" Visible="false">
                            <div class="subtitulo">Partida en curso</div>
                            <p class="dato damas-confirmacion-texto">
                                <asp:Literal ID="litMenuTexto" runat="server" />
                            </p>
                            <div class="fila-controles">
                                <asp:Button ID="btnMenuFinalizar" runat="server" Text="Sí" CssClass="btn btn-rojo"
                                    OnClick="btnMenuFinalizar_Click" CausesValidation="false" />
                                <asp:Button ID="btnMenuContinuar" runat="server" Text="No" CssClass="btn btn-verde"
                                    OnClick="btnMenuContinuar_Click" CausesValidation="false" />
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tarjeta damas-panel">
                <asp:Label ID="lblPuntosBlancas" runat="server" CssClass="etiqueta" Text="PUNTOS BLANCAS:" />
                <div class="damas-puntos">
                    <asp:Literal ID="litComidasBlancas" runat="server" />
                    <asp:Label ID="pBlancas" runat="server" CssClass="damas-numero" Text="0" />
                </div>

                <asp:Label ID="lblPuntosNegras" runat="server" CssClass="etiqueta" Text="PUNTOS NEGRAS:" />
                <div class="damas-puntos">
                    <asp:Literal ID="litComidasNegras" runat="server" />
                    <asp:Label ID="pNegras" runat="server" CssClass="damas-numero" Text="0" />
                </div>

                <div class="fila-controles espacio-arriba">
                    <asp:Button ID="btnDeshacer" runat="server" Text="Deshacer" CssClass="btn btn-gris"
                        OnClick="btnDeshacer_Click" CausesValidation="false" />
                    <asp:Button ID="btnIrMenu" runat="server" Text="Ir al menú" CssClass="btn btn-azul"
                        OnClick="btnIrMenu_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
