<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Ajedrez_ASPX.Admin" MasterPageFile="~/Site.Master" %>

<asp:Content ID="EncabezadoExtra" ContentPlaceHolderID="EncabezadoExtra" runat="server">
    <link href="Content/admin.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <div class="pagina">
        <div class="barra-superior">
            <h1 class="titulo">PANEL ADMINISTRATIVO</h1>
            <div class="fila-controles">
                <asp:Button ID="btnSincronizar" runat="server" Text="SINCRONIZAR" CssClass="btn btn-verde" OnClick="btnSincronizar_Click" CausesValidation="false" />
                <asp:Button ID="ir_menu" runat="server" Text="MENU" CssClass="btn btn-azul" OnClick="ir_menu_Click" CausesValidation="false" />
            </div>
        </div>

        <div class="pestanas">
            <asp:LinkButton ID="btnTabPartidas" runat="server" CssClass="pestana activa" OnClick="btnTabPartidas_Click" CausesValidation="false">Partidas</asp:LinkButton>
            <asp:LinkButton ID="btnTabUsuarios" runat="server" CssClass="pestana" OnClick="btnTabUsuarios_Click" CausesValidation="false">Usuarios</asp:LinkButton>
            <asp:LinkButton ID="btnTabMovimientos" runat="server" CssClass="pestana" OnClick="btnTabMovimientos_Click" CausesValidation="false">Movimientos</asp:LinkButton>
        </div>

        <asp:MultiView ID="mvTabs" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewPartidas" runat="server">
                <div class="tarjeta">
                    <div class="fila-filtros">
                        <div class="filtro-grupo">
                            <label>Jugador</label>
                            <asp:TextBox ID="txtPartidasJugador" runat="server" CssClass="campo" AutoPostBack="true" OnTextChanged="FiltroPartidas_Changed" />
                        </div>
                        <div class="filtro-grupo">
                            <label>Resultado</label>
                            <asp:DropDownList ID="cmbPartidasResultado" runat="server" CssClass="campo" AutoPostBack="true" OnSelectedIndexChanged="FiltroPartidas_Changed">
                                <asp:ListItem Text="Todos" Value="Todos" />
                                <asp:ListItem Text="Ganan blancas" Value="Ganan blancas" />
                                <asp:ListItem Text="Ganan negras" Value="Ganan negras" />
                                <asp:ListItem Text="Empate" Value="Empate" />
                                <asp:ListItem Text="En curso" Value="En curso" />
                            </asp:DropDownList>
                        </div>
                        <div class="filtro-grupo">
                            <label>Tipo</label>
                            <asp:DropDownList ID="cmbPartidasTipo" runat="server" CssClass="campo" AutoPostBack="true" OnSelectedIndexChanged="FiltroPartidas_Changed">
                                <asp:ListItem Text="Todos" Value="Todos" />
                            </asp:DropDownList>
                        </div>
                        <div class="filtro-grupo">
                            <label>Desde</label>
                            <asp:TextBox ID="txtPartidasDesde" runat="server" TextMode="Date" CssClass="campo" AutoPostBack="true" OnTextChanged="FiltroPartidas_Changed" />
                        </div>
                        <div class="filtro-grupo">
                            <label>Hasta</label>
                            <asp:TextBox ID="txtPartidasHasta" runat="server" TextMode="Date" CssClass="campo" AutoPostBack="true" OnTextChanged="FiltroPartidas_Changed" />
                        </div>
                        <div class="filtro-grupo">
                            <asp:Button ID="btnPartidasLimpiar" runat="server" Text="Limpiar filtros" CssClass="btn btn-gris" OnClick="btnPartidasLimpiar_Click" CausesValidation="false" />
                        </div>
                    </div>

                    <div class="stats-panel">
                        <asp:Label ID="lblPartidasStats" runat="server" />
                    </div>

                    <div class="tabla-envoltorio">
                        <asp:GridView ID="dgvPartidas" runat="server" CssClass="tabla grid-admin" AutoGenerateColumns="false" DataKeyNames="Id" AllowSorting="false" GridLines="None" HeaderStyle-CssClass="th" RowStyle-CssClass="td" SelectedRowStyle-CssClass="fila-sel">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="Blancas" HeaderText="Blancas" />
                                <asp:BoundField DataField="Negras" HeaderText="Negras" />
                                <asp:BoundField DataField="Ganador" HeaderText="Ganador" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                <asp:BoundField DataField="Inicio" HeaderText="Inicio" />
                                <asp:BoundField DataField="Fin" HeaderText="Fin" />
                            </Columns>
                            <SelectedRowStyle CssClass="fila-sel" />
                        </asp:GridView>
                    </div>

                    <div class="botones-admin">
                        <asp:Button ID="btnPartidasRefrescar" runat="server" Text="Refrescar" CssClass="btn btn-cian" OnClick="btnPartidasRefrescar_Click" CausesValidation="false" />
                        <asp:Button ID="btnPartidasEliminar" runat="server" Text="Eliminar" CssClass="btn btn-rojo" OnClick="btnPartidasEliminar_Click" CausesValidation="false" />
                    </div>
                </div>
            </asp:View>

            <asp:View ID="viewUsuarios" runat="server">
                <div class="tarjeta">
                    <div class="tabla-envoltorio">
                        <asp:GridView ID="dgvUsuarios" runat="server" CssClass="tabla grid-admin" AutoGenerateColumns="false" DataKeyNames="Id" AllowSorting="false" GridLines="None" HeaderStyle-CssClass="th" RowStyle-CssClass="td" SelectedRowStyle-CssClass="fila-sel">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="Contrasena" HeaderText="Contrasena" />
                                <asp:BoundField DataField="ELO" HeaderText="ELO" DataFormatString="{0:0}" />
                                <asp:BoundField DataField="Jugadas" HeaderText="Jugadas" />
                                <asp:BoundField DataField="Ganadas" HeaderText="Ganadas" />
                                <asp:BoundField DataField="Empatadas" HeaderText="Empatadas" />
                                <asp:BoundField DataField="Perdidas" HeaderText="Perdidas" />
                                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                            </Columns>
                            <SelectedRowStyle CssClass="fila-sel" />
                        </asp:GridView>
                    </div>

                    <div class="botones-admin">
                        <asp:Button ID="btnUsuariosRefrescar" runat="server" Text="Refrescar" CssClass="btn btn-cian" OnClick="btnUsuariosRefrescar_Click" CausesValidation="false" />
                        <asp:Button ID="btnUsuariosCrear" runat="server" Text="Crear" CssClass="btn btn-verde" OnClick="btnUsuariosCrear_Click" CausesValidation="false" />
                        <asp:Button ID="btnUsuariosEditar" runat="server" Text="Editar" CssClass="btn btn-azul" OnClick="btnUsuariosEditar_Click" CausesValidation="false" />
                        <asp:Button ID="btnUsuariosEliminar" runat="server" Text="Eliminar" CssClass="btn btn-rojo" OnClick="btnUsuariosEliminar_Click" CausesValidation="false" />
                        <asp:Button ID="btnUsuariosReset" runat="server" Text="Resetear contrasena" CssClass="btn btn-amarillo" OnClick="btnUsuariosReset_Click" CausesValidation="false" />
                    </div>
                </div>
            </asp:View>

            <asp:View ID="viewMovimientos" runat="server">
                <div class="tarjeta">
                    <div class="tabla-envoltorio">
                        <asp:GridView ID="dgvMovimientos" runat="server" CssClass="tabla grid-admin" AutoGenerateColumns="false" DataKeyNames="Id" AllowSorting="false" GridLines="None" HeaderStyle-CssClass="th" RowStyle-CssClass="td" SelectedRowStyle-CssClass="fila-sel">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="IdPartida" HeaderText="IdPartida" />
                                <asp:BoundField DataField="NumMovimientos" HeaderText="NumMovimientos" />
                                <asp:BoundField DataField="TurnoFinal" HeaderText="TurnoFinal" />
                                <asp:BoundField DataField="MovimientoFinal" HeaderText="MovimientoFinal" />
                            </Columns>
                            <SelectedRowStyle CssClass="fila-sel" />
                        </asp:GridView>
                    </div>

                    <div class="botones-admin">
                        <asp:Button ID="btnMovimientosRefrescar" runat="server" Text="Refrescar" CssClass="btn btn-cian" OnClick="btnMovimientosRefrescar_Click" CausesValidation="false" />
                        <asp:Button ID="btnMovimientosEliminar" runat="server" Text="Eliminar" CssClass="btn btn-rojo" OnClick="btnMovimientosEliminar_Click" CausesValidation="false" />
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>

    <!-- Modal para resetear contraseña -->
    <asp:Panel ID="pnlReset" runat="server" CssClass="modal-fondo" Visible="false">
        <div class="modal">
            <h2 class="subtitulo">Resetear contraseña</h2>
            <div class="modal-grupo">
                <span class="etiqueta">Nueva contraseña</span>
                <asp:TextBox ID="txtNuevaContrasena" runat="server" CssClass="campo" />
            </div>
            <div class="modal-botones">
                <asp:Button ID="btnResetAceptar" runat="server" Text="Aceptar" CssClass="btn btn-verde" OnClick="btnResetAceptar_Click" CausesValidation="false" />
                <asp:Button ID="btnResetCancelar" runat="server" Text="Cancelar" CssClass="btn btn-gris" OnClick="btnResetCancelar_Click" CausesValidation="false" />
            </div>
        </div>
    </asp:Panel>

    <!-- Modal para crear/editar usuario -->
    <asp:Panel ID="pnlUsuario" runat="server" CssClass="modal-fondo" Visible="false">
        <div class="modal">
            <h2 class="subtitulo"><asp:Label ID="lblUsuarioTitulo" runat="server" Text="Usuario" /></h2>
            <div class="modal-grupo">
                <span class="etiqueta">Nombre de usuario</span>
                <asp:TextBox ID="txtUsuarioNombre" runat="server" CssClass="campo" />
            </div>
            <div class="modal-grupo">
                <span class="etiqueta">Email</span>
                <asp:TextBox ID="txtUsuarioEmail" runat="server" CssClass="campo" />
            </div>
            <div class="modal-grupo">
                <span class="etiqueta">Contraseña</span>
                <asp:TextBox ID="txtUsuarioContrasena" runat="server" CssClass="campo" TextMode="Password" />
                <asp:CheckBox ID="chkUsuarioVer" runat="server" Text="Ver contraseña" CssClass="opcion" AutoPostBack="true" OnCheckedChanged="chkUsuarioVer_CheckedChanged" />
            </div>
            <div class="modal-grupo">
                <span class="etiqueta">ELO</span>
                <asp:TextBox ID="txtUsuarioElo" runat="server" CssClass="campo" />
            </div>
            <div class="modal-grupo">
                <span class="etiqueta">Partidas jugadas</span>
                <asp:TextBox ID="txtUsuarioJugadas" runat="server" CssClass="campo" />
            </div>
            <div class="modal-grupo">
                <span class="etiqueta">Partidas ganadas</span>
                <asp:TextBox ID="txtUsuarioGanadas" runat="server" CssClass="campo" />
            </div>
            <div class="modal-grupo">
                <span class="etiqueta">Partidas empatadas</span>
                <asp:TextBox ID="txtUsuarioEmpatadas" runat="server" CssClass="campo" />
            </div>
            <div class="modal-grupo">
                <span class="etiqueta">Partidas perdidas</span>
                <asp:TextBox ID="txtUsuarioPerdidas" runat="server" CssClass="campo" />
            </div>
            <div class="modal-grupo">
                <asp:CheckBox ID="chkUsuarioActivo" runat="server" Text="Activo" CssClass="opcion" />
            </div>
            <div class="modal-botones">
                <asp:Button ID="btnUsuarioGuardar" runat="server" Text="Guardar" CssClass="btn btn-verde" OnClick="btnUsuarioGuardar_Click" CausesValidation="false" />
                <asp:Button ID="btnUsuarioCancelar" runat="server" Text="Cancelar" CssClass="btn btn-gris" OnClick="btnUsuarioCancelar_Click" CausesValidation="false" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>