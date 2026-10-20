<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Partidas.aspx.cs" Inherits="Ajedrez_ASPX.Partidas" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContenidoEncabezado" ContentPlaceHolderID="EncabezadoExtra" runat="server">
    <link href="Content/partidas.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <div class="pagina">
        <div class="barra-superior">
            <h1 class="titulo sin-margen">PARTIDAS</h1>
            <div class="filtros-botones">
                <asp:Button ID="btnActualizar" runat="server" Text="ACTUALIZAR" CssClass="btn btn-verde"
                    OnClick="btnActualizar_Click" CausesValidation="false" />
                <asp:Button ID="btnMenu" runat="server" Text="VOLVER AL MENÚ" CssClass="btn btn-cian"
                    OnClick="btnMenu_Click" CausesValidation="false" />
            </div>
        </div>

        <div class="tarjeta">
            <!-- Filtros con los mismos nombres y colores del escritorio -->
            <div class="filtros">
                <asp:Button ID="btnTodas" runat="server" Text="TODAS" CssClass="btn btn-claro"
                    OnClick="btnTodas_Click" CausesValidation="false" />
                <asp:Button ID="btnGanadas" runat="server" Text="VICTORIAS" CssClass="btn btn-verde-fuerte"
                    OnClick="btnGanadas_Click" CausesValidation="false" />
                <asp:Button ID="btnPerdidas" runat="server" Text="DERROTAS" CssClass="btn btn-rojo-fuerte"
                    OnClick="btnPerdidas_Click" CausesValidation="false" />
                <asp:Button ID="btnTablas" runat="server" Text="EMPATES" CssClass="btn btn-amarillo"
                    OnClick="btnTablas_Click" CausesValidation="false" />
                <asp:Button ID="btnFecha" runat="server" Text="FECHA" CssClass="btn btn-morado"
                    OnClick="btnFecha_Click" CausesValidation="false" />
            </div>

            <!-- Tipo de juego (damas / ajedrez), igual que los RadioButton del escritorio -->
            <div class="grupo">
                <asp:RadioButton ID="rbDamas" runat="server" Text="DAMAS" GroupName="tipo"
                    CssClass="opcion" AutoPostBack="true" />
                <asp:RadioButton ID="rbAjedrez" runat="server" Text="AJEDREZ" GroupName="tipo"
                    CssClass="opcion" AutoPostBack="true" />
            </div>

            <div class="tabla-envoltorio">
                <asp:GridView ID="gvPartidas" runat="server" CssClass="tabla" AutoGenerateColumns="false"
                    DataKeyNames="Id" SelectedRowCssClass="fila-sel"
                    OnSelectedIndexChanged="gvPartidas_SelectedIndexChanged"
                    EmptyDataText="No hay partidas para mostrar.">
                    <Columns>
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                        <asp:BoundField DataField="Jugador" HeaderText="Jugador" />
                        <asp:BoundField DataField="Rival" HeaderText="Rival" />
                        <asp:BoundField DataField="Resultado" HeaderText="Resultado" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                        <asp:CommandField ShowSelectButton="true" SelectText="Ver movimientos" ButtonType="Link" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Estadísticas con winrate, igual que el groupbox del escritorio -->
        <div class="tarjeta">
            <div class="subtitulo sin-margen">
                <asp:Label ID="lblEstadisticas" runat="server" Text="ESTADISTICAS:" />
            </div>
            <div class="stats-linea">
                <div class="stat-caja">
                    <span class="rotulo"><asp:Label ID="ganadas_" runat="server" /></span>
                    <span class="valor"><asp:Label ID="victorias_" runat="server" /></span>
                </div>
                <div class="stat-caja">
                    <span class="rotulo"><asp:Label ID="perdidas_" runat="server" /></span>
                    <span class="valor"><asp:Label ID="derrotas" runat="server" /></span>
                </div>
                <div class="stat-caja">
                    <span class="rotulo"><asp:Label ID="empates_" runat="server" /></span>
                    <span class="valor"><asp:Label ID="empates" runat="server" /></span>
                </div>
                <div class="stat-caja">
                    <span class="rotulo"><asp:Label ID="todas_" runat="server" /></span>
                    <span class="valor"><asp:Label ID="total" runat="server" /></span>
                </div>
            </div>
        </div>

        <!-- Movimientos de la partida seleccionada (equivale a la ventana MOVIMIENTOS) -->
        <asp:Panel ID="pnlMovimientos" runat="server" CssClass="tarjeta" Visible="false">
            <div class="barra-superior">
                <div class="subtitulo sin-margen">MOVIMIENTOS</div>
                <asp:Button ID="btnCerrarMov" runat="server" Text="CERRAR" CssClass="btn btn-azul"
                    OnClick="btnCerrarMov_Click" CausesValidation="false" />
            </div>
            <div class="tabla-envoltorio">
                <asp:GridView ID="gvMovimientos" runat="server" CssClass="tabla" AutoGenerateColumns="false"
                    EmptyDataText="La partida no tiene movimientos registrados.">
                    <Columns>
                        <asp:BoundField DataField="num_movimientos" HeaderText="Nº" />
                        <asp:BoundField DataField="turno_final" HeaderText="Turno" />
                        <asp:BoundField DataField="movimiento_final" HeaderText="Movimiento" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
