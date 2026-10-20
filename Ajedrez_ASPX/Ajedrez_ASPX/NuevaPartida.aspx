<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevaPartida.aspx.cs" Inherits="Ajedrez_ASPX.NuevaPartida" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <div class="centrado">
        <div class="tarjeta">
            <h1 class="titulo">
                NUEVA PARTIDA -
                <asp:Label ID="lblTipo" runat="server" Text="AJEDRÉZ" />
            </h1>

            <span class="etiqueta">Jugador local</span>
            <asp:TextBox ID="txtLocal" runat="server" CssClass="campo" MaxLength="50" />

            <span class="etiqueta">Oponente</span>
            <asp:TextBox ID="txtOponente" runat="server" CssClass="campo" MaxLength="50" />

            <div class="grupo">
                <span class="subtitulo">Color</span>
                <asp:RadioButton ID="rbBlancas" runat="server" Text="Juegas con blancas" GroupName="color"
                    Checked="true" CssClass="opcion" />
                <asp:RadioButton ID="rbNegras" runat="server" Text="Juegas con negras" GroupName="color"
                    CssClass="opcion" />
            </div>

            <!-- Modo soplar: solo damas -->
            <asp:Panel ID="pnlModoDamas" runat="server">
                <div class="grupo">
                    <span class="subtitulo">Modo</span>
                    <asp:RadioButton ID="rbNormal" runat="server" Text="Normal" GroupName="modo"
                        Checked="true" CssClass="opcion" />
                    <asp:RadioButton ID="rbSoplar" runat="server" Text="Soplar (capturas obligatorias)"
                        GroupName="modo" CssClass="opcion" />
                </div>
            </asp:Panel>

            <!-- Modo de juego: solo ajedrez (contra IA o local) -->
            <asp:Panel ID="pnlModoAjedrez" runat="server">
                <div class="grupo">
                    <span class="subtitulo">Modo de juego</span><br />
                    <asp:RadioButton ID="rbLocal" runat="server" Text="2 jugadores (local)" GroupName="modoia"
                        Checked="true" CssClass="opcion" AutoPostBack="true" OnCheckedChanged="Modo_CheckedChanged" />
                    <asp:RadioButton ID="rbIA" runat="server" Text="Contra la IA (Stockfish)" GroupName="modoia"
                        CssClass="opcion" AutoPostBack="true" OnCheckedChanged="Modo_CheckedChanged" />
                    <br />
                    <span class="etiqueta">Nivel de la IA</span>
                    <asp:DropDownList ID="cmbNivel" runat="server" CssClass="campo" Style="max-width: 220px;">
                        <asp:ListItem Text="Fácil" Value="0" />
                        <asp:ListItem Text="Media" Value="1" Selected="True" />
                        <asp:ListItem Text="Difícil" Value="2" />
                        <asp:ListItem Text="Experto" Value="3" />
                    </asp:DropDownList>
                </div>
            </asp:Panel>

            <div class="fila-controles espacio-arriba">
                <asp:Button ID="btnIniciar" runat="server" Text="Iniciar partida" CssClass="btn btn-cian"
                    OnClick="btnIniciar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-gris"
                    OnClick="btnCancelar_Click" CausesValidation="false" />
            </div>

            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje" />
        </div>
    </div>
</asp:Content>
