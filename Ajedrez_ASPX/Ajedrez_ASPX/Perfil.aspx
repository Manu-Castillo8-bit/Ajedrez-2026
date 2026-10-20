<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="Ajedrez_ASPX.Perfil" MasterPageFile="~/Site.Master" %>

<asp:Content ID="ContenidoEncabezado" ContentPlaceHolderID="EncabezadoExtra" runat="server">
    <link href="Content/perfil.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="ContenidoPrincipal" ContentPlaceHolderID="Contenido" runat="server">
    <div class="pagina">
        <div class="barra-superior">
            <h1 class="titulo sin-margen">PERFIL</h1>
            <asp:Button ID="btnMenu" runat="server" Text="VOLVER AL MENÚ" CssClass="btn btn-cian"
                OnClick="btnMenu_Click" CausesValidation="false" />
        </div>

        <!-- Datos del perfil: foto y campos (como la ventana PERFIL del escritorio) -->
        <div class="tarjeta">
            <div class="perfil-zona">

                <div class="perfil-foto">
                    <asp:Image ID="imgPerfil" runat="server" CssClass="foto-grande" Visible="false" />
                    <asp:Panel ID="pnlFotoVacia" runat="server" CssClass="foto-grande foto-vacia">Sin foto de perfil</asp:Panel>
                    <span class="etiqueta">Subir o cambiar la foto de perfil</span>
                    <asp:FileUpload ID="fuFoto" runat="server" CssClass="campo"
                        accept=".jpg,.jpeg,.png,.bmp,.gif" />
                </div>

                <div class="perfil-datos">
                    <div class="fila-dato">
                        <span class="clave">NOMBRE:</span>
                        <span class="valor"><asp:Label ID="lblNombre" runat="server" /></span>
                    </div>

                    <div class="fila-dato">
                        <span class="clave">CORREO:</span>
                        <span class="valor"><asp:Label ID="lblCorreo" runat="server" /></span>
                        <asp:Button ID="btnEditarCorreo" runat="server" Text="EDITAR DATOS"
                            CssClass="btn btn-azul btn-mini" ToolTip="Editar nombre y correo"
                            OnClick="btnEditarCorreo_Click" CausesValidation="false" />
                    </div>

                    <div class="fila-dato">
                        <span class="clave">ELO:</span>
                        <span class="valor"><asp:Label ID="lblElo" runat="server" Text="0" /></span>
                    </div>

                    <div class="fila-dato">
                        <span class="clave">VICTORIAS:</span>
                        <span class="valor"><asp:Label ID="victorias" runat="server" Text="0" /></span>
                    </div>

                    <div class="fila-dato">
                        <span class="clave">DESCRIPCION:</span>
                        <span class="valor"><asp:Label ID="lblDescripcion" runat="server" /></span>
                        <asp:Button ID="btnEditarDescripcion" runat="server" Text="EDITAR DESCRIPCIÓN"
                            CssClass="btn btn-azul btn-mini" OnClick="btnEditarDescripcion_Click"
                            CausesValidation="false" />
                    </div>

                    <!-- Edición de nombre y correo (aparece al pulsar el lápiz, como el escritorio) -->
                    <asp:Panel ID="pnlEdicion" runat="server" Visible="false">
                        <span class="etiqueta">Nombre</span>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="campo campo-inline"
                            MaxLength="50" autocomplete="off" />
                        <span class="etiqueta">Correo</span>
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="campo campo-inline"
                            MaxLength="100" autocomplete="off" />
                    </asp:Panel>
                </div>
            </div>
        </div>

        <!-- Estadísticas calculadas desde las partidas finalizadas (igual que el escritorio) -->
        <div class="tarjeta">
            <div class="stats-linea">
                <div class="stat-caja">
                    <span class="rotulo">VICTORIAS</span>
                    <span class="valor"><asp:Label ID="statVictorias" runat="server" Text="0" /></span>
                </div>
                <div class="stat-caja">
                    <span class="rotulo">DERROTAS</span>
                    <span class="valor"><asp:Label ID="statDerrotas" runat="server" Text="0" /></span>
                </div>
                <div class="stat-caja">
                    <span class="rotulo">EMPATES</span>
                    <span class="valor"><asp:Label ID="statEmpates" runat="server" Text="0" /></span>
                </div>
                <div class="stat-caja">
                    <span class="rotulo">TOTAL</span>
                    <span class="valor"><asp:Label ID="statTotal" runat="server" Text="0" /></span>
                </div>
            </div>
        </div>

        <div class="tarjeta">
            <div class="fila-controles">
                <asp:Button ID="btnGuardar" runat="server" Text="GUARDAR" CssClass="btn btn-azul"
                    OnClick="btnGuardar_Click" CausesValidation="false" />
                <asp:Button ID="btnCancelar" runat="server" Text="CANCELAR" CssClass="btn btn-azul"
                    OnClick="btnCancelar_Click" CausesValidation="false" Visible="false" />
                <asp:Button ID="btnEliminarFoto" runat="server" Text="ELIMINAR FOTO"
                    CssClass="btn btn-rojo" OnClick="btnEliminarFoto_Click" CausesValidation="false" />
            </div>

            <!-- Edición de la descripción (máximo 50 caracteres, como el diálogo del escritorio) -->
            <asp:Panel ID="pnlDescripcion" runat="server" Visible="false">
                <span class="etiqueta">Escribe una descripción corta para tu perfil (máximo 50 caracteres):</span>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="campo" MaxLength="50" />
                <div class="contador-desc"><asp:Label ID="lblContador" runat="server" /></div>
                <div class="fila-controles">
                    <asp:Button ID="btnGuardarDesc" runat="server" Text="GUARDAR" CssClass="btn btn-azul"
                        OnClick="btnGuardarDesc_Click" CausesValidation="false" />
                    <asp:Button ID="btnCancelarDesc" runat="server" Text="CANCELAR" CssClass="btn btn-azul"
                        OnClick="btnCancelarDesc_Click" CausesValidation="false" />
                </div>
            </asp:Panel>

            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje" />
        </div>
    </div>
</asp:Content>
