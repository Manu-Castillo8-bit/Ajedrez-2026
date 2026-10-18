namespace Ajedrez
{
    partial class AdminPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button ir_menu;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabUsuarios;
        private System.Windows.Forms.TabPage tabPartidas;
        private System.Windows.Forms.TabPage tabMovimientos;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.DataGridView dgvPartidas;
        private System.Windows.Forms.DataGridView dgvMovimientos;
        private System.Windows.Forms.Button btnUsuariosRefrescar;
        private System.Windows.Forms.Button btnUsuariosCrear;
        private System.Windows.Forms.Button btnUsuariosEditar;
        private System.Windows.Forms.Button btnUsuariosEliminar;
        private System.Windows.Forms.Button btnUsuariosReset;
        private System.Windows.Forms.Button btnPartidasRefrescar;
        private System.Windows.Forms.Button btnPartidasEliminar;
        private System.Windows.Forms.Button btnMovimientosRefrescar;
        private System.Windows.Forms.Button btnMovimientosEliminar;
        private System.Windows.Forms.TextBox txtPartidasJugador;
        private System.Windows.Forms.ComboBox cmbPartidasResultado;
        private System.Windows.Forms.ComboBox cmbPartidasTipo;
        private System.Windows.Forms.DateTimePicker dtpPartidasDesde;
        private System.Windows.Forms.DateTimePicker dtpPartidasHasta;
        private System.Windows.Forms.Label lblPartidasJugador;
        private System.Windows.Forms.Label lblPartidasResultado;
        private System.Windows.Forms.Label lblPartidasTipo;
        private System.Windows.Forms.Label lblPartidasDesde;
        private System.Windows.Forms.Label lblPartidasHasta;
        private System.Windows.Forms.Label lblPartidasStats;
        private System.Windows.Forms.Button btnPartidasLimpiar;
        private System.Windows.Forms.Button btnSincronizar;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminPanel));
            this.lblTitulo = new System.Windows.Forms.Label();
            this.ir_menu = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabUsuarios = new System.Windows.Forms.TabPage();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.btnUsuariosReset = new System.Windows.Forms.Button();
            this.btnUsuariosEliminar = new System.Windows.Forms.Button();
            this.btnUsuariosEditar = new System.Windows.Forms.Button();
            this.btnUsuariosCrear = new System.Windows.Forms.Button();
            this.btnUsuariosRefrescar = new System.Windows.Forms.Button();
            this.tabPartidas = new System.Windows.Forms.TabPage();
            this.dgvPartidas = new System.Windows.Forms.DataGridView();
            this.lblPartidasStats = new System.Windows.Forms.Label();
            this.btnPartidasLimpiar = new System.Windows.Forms.Button();
            this.dtpPartidasHasta = new System.Windows.Forms.DateTimePicker();
            this.dtpPartidasDesde = new System.Windows.Forms.DateTimePicker();
            this.cmbPartidasTipo = new System.Windows.Forms.ComboBox();
            this.cmbPartidasResultado = new System.Windows.Forms.ComboBox();
            this.txtPartidasJugador = new System.Windows.Forms.TextBox();
            this.lblPartidasHasta = new System.Windows.Forms.Label();
            this.lblPartidasDesde = new System.Windows.Forms.Label();
            this.lblPartidasTipo = new System.Windows.Forms.Label();
            this.lblPartidasResultado = new System.Windows.Forms.Label();
            this.lblPartidasJugador = new System.Windows.Forms.Label();
            this.btnPartidasEliminar = new System.Windows.Forms.Button();
            this.btnPartidasRefrescar = new System.Windows.Forms.Button();
            this.tabMovimientos = new System.Windows.Forms.TabPage();
            this.dgvMovimientos = new System.Windows.Forms.DataGridView();
            this.btnMovimientosEliminar = new System.Windows.Forms.Button();
            this.btnMovimientosRefrescar = new System.Windows.Forms.Button();
            this.btnSincronizar = new System.Windows.Forms.Button();
            this.tabs.SuspendLayout();
            this.tabUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.tabPartidas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).BeginInit();
            this.tabMovimientos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(24, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(245, 22);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "PANEL ADMINISTRATIVO";
            // 
            // ir_menu
            // 
            this.ir_menu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ir_menu.Location = new System.Drawing.Point(838, 18);
            this.ir_menu.Name = "ir_menu";
            this.ir_menu.Size = new System.Drawing.Size(94, 36);
            this.ir_menu.TabIndex = 1;
            this.ir_menu.Text = "MENU";
            this.ir_menu.UseVisualStyleBackColor = true;
            this.ir_menu.Click += new System.EventHandler(this.ir_menu_Click);
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabUsuarios);
            this.tabs.Controls.Add(this.tabPartidas);
            this.tabs.Controls.Add(this.tabMovimientos);
            this.tabs.Location = new System.Drawing.Point(16, 74);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(912, 570);
            this.tabs.TabIndex = 2;
            // 
            // tabUsuarios
            // 
            this.tabUsuarios.Controls.Add(this.dgvUsuarios);
            this.tabUsuarios.Controls.Add(this.btnUsuariosReset);
            this.tabUsuarios.Controls.Add(this.btnUsuariosEliminar);
            this.tabUsuarios.Controls.Add(this.btnUsuariosEditar);
            this.tabUsuarios.Controls.Add(this.btnUsuariosCrear);
            this.tabUsuarios.Controls.Add(this.btnUsuariosRefrescar);
            this.tabUsuarios.Location = new System.Drawing.Point(4, 22);
            this.tabUsuarios.Name = "tabUsuarios";
            this.tabUsuarios.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsuarios.Size = new System.Drawing.Size(904, 544);
            this.tabUsuarios.TabIndex = 0;
            this.tabUsuarios.Text = "Usuarios";
            this.tabUsuarios.UseVisualStyleBackColor = true;
            // 
            // dgvUsuarios
            // 
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsuarios.Location = new System.Drawing.Point(20, 24);
            this.dgvUsuarios.MultiSelect = false;
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.RowHeadersVisible = false;
            this.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Size = new System.Drawing.Size(864, 420);
            this.dgvUsuarios.TabIndex = 0;
            // 
            // btnUsuariosReset
            // 
            this.btnUsuariosReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUsuariosReset.Location = new System.Drawing.Point(574, 472);
            this.btnUsuariosReset.Name = "btnUsuariosReset";
            this.btnUsuariosReset.Size = new System.Drawing.Size(190, 36);
            this.btnUsuariosReset.TabIndex = 5;
            this.btnUsuariosReset.Text = "Resetear contrasena";
            this.btnUsuariosReset.UseVisualStyleBackColor = true;
            this.btnUsuariosReset.Click += new System.EventHandler(this.btnUsuariosReset_Click);
            // 
            // btnUsuariosEliminar
            // 
            this.btnUsuariosEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUsuariosEliminar.Location = new System.Drawing.Point(438, 472);
            this.btnUsuariosEliminar.Name = "btnUsuariosEliminar";
            this.btnUsuariosEliminar.Size = new System.Drawing.Size(120, 36);
            this.btnUsuariosEliminar.TabIndex = 4;
            this.btnUsuariosEliminar.Text = "Eliminar";
            this.btnUsuariosEliminar.UseVisualStyleBackColor = true;
            this.btnUsuariosEliminar.Click += new System.EventHandler(this.btnUsuariosEliminar_Click);
            // 
            // btnUsuariosEditar
            // 
            this.btnUsuariosEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUsuariosEditar.Location = new System.Drawing.Point(302, 472);
            this.btnUsuariosEditar.Name = "btnUsuariosEditar";
            this.btnUsuariosEditar.Size = new System.Drawing.Size(120, 36);
            this.btnUsuariosEditar.TabIndex = 3;
            this.btnUsuariosEditar.Text = "Editar";
            this.btnUsuariosEditar.UseVisualStyleBackColor = true;
            this.btnUsuariosEditar.Click += new System.EventHandler(this.btnUsuariosEditar_Click);
            // 
            // btnUsuariosCrear
            // 
            this.btnUsuariosCrear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUsuariosCrear.Location = new System.Drawing.Point(166, 472);
            this.btnUsuariosCrear.Name = "btnUsuariosCrear";
            this.btnUsuariosCrear.Size = new System.Drawing.Size(120, 36);
            this.btnUsuariosCrear.TabIndex = 2;
            this.btnUsuariosCrear.Text = "Crear";
            this.btnUsuariosCrear.UseVisualStyleBackColor = true;
            this.btnUsuariosCrear.Click += new System.EventHandler(this.btnUsuariosCrear_Click);
            // 
            // btnUsuariosRefrescar
            // 
            this.btnUsuariosRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUsuariosRefrescar.Location = new System.Drawing.Point(20, 472);
            this.btnUsuariosRefrescar.Name = "btnUsuariosRefrescar";
            this.btnUsuariosRefrescar.Size = new System.Drawing.Size(130, 36);
            this.btnUsuariosRefrescar.TabIndex = 1;
            this.btnUsuariosRefrescar.Text = "Refrescar";
            this.btnUsuariosRefrescar.UseVisualStyleBackColor = true;
            this.btnUsuariosRefrescar.Click += new System.EventHandler(this.btnUsuariosRefrescar_Click);
            // 
            // tabPartidas
            // 
            this.tabPartidas.Controls.Add(this.dgvPartidas);
            this.tabPartidas.Controls.Add(this.lblPartidasStats);
            this.tabPartidas.Controls.Add(this.btnPartidasLimpiar);
            this.tabPartidas.Controls.Add(this.dtpPartidasHasta);
            this.tabPartidas.Controls.Add(this.dtpPartidasDesde);
            this.tabPartidas.Controls.Add(this.cmbPartidasTipo);
            this.tabPartidas.Controls.Add(this.cmbPartidasResultado);
            this.tabPartidas.Controls.Add(this.txtPartidasJugador);
            this.tabPartidas.Controls.Add(this.lblPartidasHasta);
            this.tabPartidas.Controls.Add(this.lblPartidasDesde);
            this.tabPartidas.Controls.Add(this.lblPartidasTipo);
            this.tabPartidas.Controls.Add(this.lblPartidasResultado);
            this.tabPartidas.Controls.Add(this.lblPartidasJugador);
            this.tabPartidas.Controls.Add(this.btnPartidasEliminar);
            this.tabPartidas.Controls.Add(this.btnPartidasRefrescar);
            this.tabPartidas.Location = new System.Drawing.Point(4, 22);
            this.tabPartidas.Name = "tabPartidas";
            this.tabPartidas.Padding = new System.Windows.Forms.Padding(3);
            this.tabPartidas.Size = new System.Drawing.Size(904, 544);
            this.tabPartidas.TabIndex = 1;
            this.tabPartidas.Text = "Partidas";
            this.tabPartidas.UseVisualStyleBackColor = true;
            // 
            // dgvPartidas
            // 
            this.dgvPartidas.AllowUserToAddRows = false;
            this.dgvPartidas.AllowUserToDeleteRows = false;
            this.dgvPartidas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPartidas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPartidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPartidas.Location = new System.Drawing.Point(20, 196);
            this.dgvPartidas.MultiSelect = false;
            this.dgvPartidas.Name = "dgvPartidas";
            this.dgvPartidas.ReadOnly = true;
            this.dgvPartidas.RowHeadersVisible = false;
            this.dgvPartidas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPartidas.Size = new System.Drawing.Size(864, 256);
            this.dgvPartidas.TabIndex = 12;
            // 
            // lblPartidasStats
            // 
            this.lblPartidasStats.Location = new System.Drawing.Point(20, 122);
            this.lblPartidasStats.Name = "lblPartidasStats";
            this.lblPartidasStats.Size = new System.Drawing.Size(864, 56);
            this.lblPartidasStats.TabIndex = 10;
            this.lblPartidasStats.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPartidasLimpiar
            // 
            this.btnPartidasLimpiar.Location = new System.Drawing.Point(610, 74);
            this.btnPartidasLimpiar.Name = "btnPartidasLimpiar";
            this.btnPartidasLimpiar.Size = new System.Drawing.Size(150, 34);
            this.btnPartidasLimpiar.TabIndex = 11;
            this.btnPartidasLimpiar.Text = "Limpiar filtros";
            this.btnPartidasLimpiar.UseVisualStyleBackColor = true;
            this.btnPartidasLimpiar.Click += new System.EventHandler(this.btnPartidasLimpiar_Click);
            // 
            // dtpPartidasHasta
            // 
            this.dtpPartidasHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPartidasHasta.Location = new System.Drawing.Point(374, 76);
            this.dtpPartidasHasta.Name = "dtpPartidasHasta";
            this.dtpPartidasHasta.ShowCheckBox = true;
            this.dtpPartidasHasta.Size = new System.Drawing.Size(150, 20);
            this.dtpPartidasHasta.TabIndex = 9;
            this.dtpPartidasHasta.ValueChanged += new System.EventHandler(this.FiltroPartidas_Changed);
            // 
            // dtpPartidasDesde
            // 
            this.dtpPartidasDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPartidasDesde.Location = new System.Drawing.Point(104, 76);
            this.dtpPartidasDesde.Name = "dtpPartidasDesde";
            this.dtpPartidasDesde.ShowCheckBox = true;
            this.dtpPartidasDesde.Size = new System.Drawing.Size(150, 20);
            this.dtpPartidasDesde.TabIndex = 7;
            this.dtpPartidasDesde.ValueChanged += new System.EventHandler(this.FiltroPartidas_Changed);
            // 
            // cmbPartidasTipo
            // 
            this.cmbPartidasTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPartidasTipo.Location = new System.Drawing.Point(670, 26);
            this.cmbPartidasTipo.Name = "cmbPartidasTipo";
            this.cmbPartidasTipo.Size = new System.Drawing.Size(150, 21);
            this.cmbPartidasTipo.TabIndex = 5;
            this.cmbPartidasTipo.SelectedIndexChanged += new System.EventHandler(this.FiltroPartidas_Changed);
            // 
            // cmbPartidasResultado
            // 
            this.cmbPartidasResultado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPartidasResultado.Location = new System.Drawing.Point(414, 26);
            this.cmbPartidasResultado.Name = "cmbPartidasResultado";
            this.cmbPartidasResultado.Size = new System.Drawing.Size(150, 21);
            this.cmbPartidasResultado.TabIndex = 3;
            this.cmbPartidasResultado.SelectedIndexChanged += new System.EventHandler(this.FiltroPartidas_Changed);
            // 
            // txtPartidasJugador
            // 
            this.txtPartidasJugador.Location = new System.Drawing.Point(104, 26);
            this.txtPartidasJugador.Name = "txtPartidasJugador";
            this.txtPartidasJugador.Size = new System.Drawing.Size(150, 20);
            this.txtPartidasJugador.TabIndex = 1;
            this.txtPartidasJugador.TextChanged += new System.EventHandler(this.FiltroPartidas_Changed);
            // 
            // lblPartidasHasta
            // 
            this.lblPartidasHasta.AutoSize = true;
            this.lblPartidasHasta.Location = new System.Drawing.Point(300, 80);
            this.lblPartidasHasta.Name = "lblPartidasHasta";
            this.lblPartidasHasta.Size = new System.Drawing.Size(38, 13);
            this.lblPartidasHasta.TabIndex = 8;
            this.lblPartidasHasta.Text = "Hasta:";
            // 
            // lblPartidasDesde
            // 
            this.lblPartidasDesde.AutoSize = true;
            this.lblPartidasDesde.Location = new System.Drawing.Point(20, 80);
            this.lblPartidasDesde.Name = "lblPartidasDesde";
            this.lblPartidasDesde.Size = new System.Drawing.Size(41, 13);
            this.lblPartidasDesde.TabIndex = 6;
            this.lblPartidasDesde.Text = "Desde:";
            // 
            // lblPartidasTipo
            // 
            this.lblPartidasTipo.AutoSize = true;
            this.lblPartidasTipo.Location = new System.Drawing.Point(610, 30);
            this.lblPartidasTipo.Name = "lblPartidasTipo";
            this.lblPartidasTipo.Size = new System.Drawing.Size(31, 13);
            this.lblPartidasTipo.TabIndex = 4;
            this.lblPartidasTipo.Text = "Tipo:";
            // 
            // lblPartidasResultado
            // 
            this.lblPartidasResultado.AutoSize = true;
            this.lblPartidasResultado.Location = new System.Drawing.Point(300, 30);
            this.lblPartidasResultado.Name = "lblPartidasResultado";
            this.lblPartidasResultado.Size = new System.Drawing.Size(58, 13);
            this.lblPartidasResultado.TabIndex = 2;
            this.lblPartidasResultado.Text = "Resultado:";
            // 
            // lblPartidasJugador
            // 
            this.lblPartidasJugador.AutoSize = true;
            this.lblPartidasJugador.Location = new System.Drawing.Point(20, 30);
            this.lblPartidasJugador.Name = "lblPartidasJugador";
            this.lblPartidasJugador.Size = new System.Drawing.Size(48, 13);
            this.lblPartidasJugador.TabIndex = 0;
            this.lblPartidasJugador.Text = "Jugador:";
            // 
            // btnPartidasEliminar
            // 
            this.btnPartidasEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPartidasEliminar.Location = new System.Drawing.Point(166, 484);
            this.btnPartidasEliminar.Name = "btnPartidasEliminar";
            this.btnPartidasEliminar.Size = new System.Drawing.Size(120, 36);
            this.btnPartidasEliminar.TabIndex = 14;
            this.btnPartidasEliminar.Text = "Eliminar";
            this.btnPartidasEliminar.UseVisualStyleBackColor = true;
            this.btnPartidasEliminar.Click += new System.EventHandler(this.btnPartidasEliminar_Click);
            // 
            // btnPartidasRefrescar
            // 
            this.btnPartidasRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPartidasRefrescar.Location = new System.Drawing.Point(20, 484);
            this.btnPartidasRefrescar.Name = "btnPartidasRefrescar";
            this.btnPartidasRefrescar.Size = new System.Drawing.Size(130, 36);
            this.btnPartidasRefrescar.TabIndex = 13;
            this.btnPartidasRefrescar.Text = "Refrescar";
            this.btnPartidasRefrescar.UseVisualStyleBackColor = true;
            this.btnPartidasRefrescar.Click += new System.EventHandler(this.btnPartidasRefrescar_Click);
            // 
            // tabMovimientos
            // 
            this.tabMovimientos.Controls.Add(this.dgvMovimientos);
            this.tabMovimientos.Controls.Add(this.btnMovimientosEliminar);
            this.tabMovimientos.Controls.Add(this.btnMovimientosRefrescar);
            this.tabMovimientos.Location = new System.Drawing.Point(4, 22);
            this.tabMovimientos.Name = "tabMovimientos";
            this.tabMovimientos.Padding = new System.Windows.Forms.Padding(3);
            this.tabMovimientos.Size = new System.Drawing.Size(904, 544);
            this.tabMovimientos.TabIndex = 2;
            this.tabMovimientos.Text = "Movimientos";
            this.tabMovimientos.UseVisualStyleBackColor = true;
            // 
            // dgvMovimientos
            // 
            this.dgvMovimientos.AllowUserToAddRows = false;
            this.dgvMovimientos.AllowUserToDeleteRows = false;
            this.dgvMovimientos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMovimientos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovimientos.Location = new System.Drawing.Point(20, 24);
            this.dgvMovimientos.MultiSelect = false;
            this.dgvMovimientos.Name = "dgvMovimientos";
            this.dgvMovimientos.ReadOnly = true;
            this.dgvMovimientos.RowHeadersVisible = false;
            this.dgvMovimientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMovimientos.Size = new System.Drawing.Size(864, 420);
            this.dgvMovimientos.TabIndex = 0;
            // 
            // btnMovimientosEliminar
            // 
            this.btnMovimientosEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMovimientosEliminar.Location = new System.Drawing.Point(166, 472);
            this.btnMovimientosEliminar.Name = "btnMovimientosEliminar";
            this.btnMovimientosEliminar.Size = new System.Drawing.Size(120, 36);
            this.btnMovimientosEliminar.TabIndex = 2;
            this.btnMovimientosEliminar.Text = "Eliminar";
            this.btnMovimientosEliminar.UseVisualStyleBackColor = true;
            this.btnMovimientosEliminar.Click += new System.EventHandler(this.btnMovimientosEliminar_Click);
            // 
            // btnMovimientosRefrescar
            // 
            this.btnMovimientosRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMovimientosRefrescar.Location = new System.Drawing.Point(20, 472);
            this.btnMovimientosRefrescar.Name = "btnMovimientosRefrescar";
            this.btnMovimientosRefrescar.Size = new System.Drawing.Size(130, 36);
            this.btnMovimientosRefrescar.TabIndex = 1;
            this.btnMovimientosRefrescar.Text = "Refrescar";
            this.btnMovimientosRefrescar.UseVisualStyleBackColor = true;
            this.btnMovimientosRefrescar.Click += new System.EventHandler(this.btnMovimientosRefrescar_Click);
            // 
            // btnSincronizar
            // 
            this.btnSincronizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSincronizar.Location = new System.Drawing.Point(594, 18);
            this.btnSincronizar.Name = "btnSincronizar";
            this.btnSincronizar.Size = new System.Drawing.Size(228, 36);
            this.btnSincronizar.TabIndex = 3;
            this.btnSincronizar.Text = "SINCRONIZAR";
            this.btnSincronizar.UseVisualStyleBackColor = true;
            this.btnSincronizar.Click += new System.EventHandler(this.btnSincronizar_Click);
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 660);
            this.Controls.Add(this.btnSincronizar);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.ir_menu);
            this.Controls.Add(this.lblTitulo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(960, 640);
            this.Name = "AdminPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PANEL ADMINISTRATIVO";
            this.Load += new System.EventHandler(this.AdminPanel_Load);
            this.tabs.ResumeLayout(false);
            this.tabUsuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.tabPartidas.ResumeLayout(false);
            this.tabPartidas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).EndInit();
            this.tabMovimientos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
