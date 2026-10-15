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

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.ir_menu = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabUsuarios = new System.Windows.Forms.TabPage();
            this.tabPartidas = new System.Windows.Forms.TabPage();
            this.tabMovimientos = new System.Windows.Forms.TabPage();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.dgvPartidas = new System.Windows.Forms.DataGridView();
            this.dgvMovimientos = new System.Windows.Forms.DataGridView();
            this.btnUsuariosRefrescar = new System.Windows.Forms.Button();
            this.btnUsuariosCrear = new System.Windows.Forms.Button();
            this.btnUsuariosEditar = new System.Windows.Forms.Button();
            this.btnUsuariosEliminar = new System.Windows.Forms.Button();
            this.btnUsuariosReset = new System.Windows.Forms.Button();
            this.btnPartidasRefrescar = new System.Windows.Forms.Button();
            this.btnPartidasEliminar = new System.Windows.Forms.Button();
            this.btnMovimientosRefrescar = new System.Windows.Forms.Button();
            this.btnMovimientosEliminar = new System.Windows.Forms.Button();
            this.tabs.SuspendLayout();
            this.tabUsuarios.SuspendLayout();
            this.tabPartidas.SuspendLayout();
            this.tabMovimientos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(16, 14);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(260, 22);
            this.lblTitulo.Text = "PANEL ADMINISTRATIVO";
            // 
            // ir_menu
            // 
            this.ir_menu.Location = new System.Drawing.Point(830, 12);
            this.ir_menu.Name = "ir_menu";
            this.ir_menu.Size = new System.Drawing.Size(94, 30);
            this.ir_menu.Text = "MENU";
            this.ir_menu.UseVisualStyleBackColor = true;
            this.ir_menu.Click += new System.EventHandler(this.ir_menu_Click);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabUsuarios);
            this.tabs.Controls.Add(this.tabPartidas);
            this.tabs.Controls.Add(this.tabMovimientos);
            this.tabs.Location = new System.Drawing.Point(12, 52);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(912, 520);
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
            this.tabUsuarios.Size = new System.Drawing.Size(904, 494);
            this.tabUsuarios.Text = "Usuarios";
            this.tabUsuarios.UseVisualStyleBackColor = true;
            // 
            // dgvUsuarios
            // 
            ConfigurarGrid(this.dgvUsuarios, new System.Drawing.Point(10, 10), new System.Drawing.Size(884, 430));
            // 
            // btnUsuariosRefrescar
            // 
            ConfigurarBoton(this.btnUsuariosRefrescar, "Refrescar", new System.Drawing.Point(10, 450));
            this.btnUsuariosRefrescar.Click += new System.EventHandler(this.btnUsuariosRefrescar_Click);
            // 
            // btnUsuariosCrear
            // 
            ConfigurarBoton(this.btnUsuariosCrear, "Crear", new System.Drawing.Point(130, 450));
            this.btnUsuariosCrear.Click += new System.EventHandler(this.btnUsuariosCrear_Click);
            // 
            // btnUsuariosEditar
            // 
            ConfigurarBoton(this.btnUsuariosEditar, "Editar", new System.Drawing.Point(250, 450));
            this.btnUsuariosEditar.Click += new System.EventHandler(this.btnUsuariosEditar_Click);
            // 
            // btnUsuariosEliminar
            // 
            ConfigurarBoton(this.btnUsuariosEliminar, "Eliminar", new System.Drawing.Point(370, 450));
            this.btnUsuariosEliminar.Click += new System.EventHandler(this.btnUsuariosEliminar_Click);
            // 
            // btnUsuariosReset
            // 
            ConfigurarBoton(this.btnUsuariosReset, "Resetear contraseña", new System.Drawing.Point(490, 450));
            this.btnUsuariosReset.Size = new System.Drawing.Size(170, 32);
            this.btnUsuariosReset.Click += new System.EventHandler(this.btnUsuariosReset_Click);
            // 
            // tabPartidas
            // 
            this.tabPartidas.Controls.Add(this.dgvPartidas);
            this.tabPartidas.Controls.Add(this.btnPartidasEliminar);
            this.tabPartidas.Controls.Add(this.btnPartidasRefrescar);
            this.tabPartidas.Location = new System.Drawing.Point(4, 22);
            this.tabPartidas.Name = "tabPartidas";
            this.tabPartidas.Size = new System.Drawing.Size(904, 494);
            this.tabPartidas.Text = "Partidas";
            this.tabPartidas.UseVisualStyleBackColor = true;
            // 
            // dgvPartidas
            // 
            ConfigurarGrid(this.dgvPartidas, new System.Drawing.Point(10, 10), new System.Drawing.Size(884, 430));
            // 
            // btnPartidasRefrescar
            // 
            ConfigurarBoton(this.btnPartidasRefrescar, "Refrescar", new System.Drawing.Point(10, 450));
            this.btnPartidasRefrescar.Click += new System.EventHandler(this.btnPartidasRefrescar_Click);
            // 
            // btnPartidasEliminar
            // 
            ConfigurarBoton(this.btnPartidasEliminar, "Eliminar", new System.Drawing.Point(130, 450));
            this.btnPartidasEliminar.Click += new System.EventHandler(this.btnPartidasEliminar_Click);
            // 
            // tabMovimientos
            // 
            this.tabMovimientos.Controls.Add(this.dgvMovimientos);
            this.tabMovimientos.Controls.Add(this.btnMovimientosEliminar);
            this.tabMovimientos.Controls.Add(this.btnMovimientosRefrescar);
            this.tabMovimientos.Location = new System.Drawing.Point(4, 22);
            this.tabMovimientos.Name = "tabMovimientos";
            this.tabMovimientos.Size = new System.Drawing.Size(904, 494);
            this.tabMovimientos.Text = "Movimientos";
            this.tabMovimientos.UseVisualStyleBackColor = true;
            // 
            // dgvMovimientos
            // 
            ConfigurarGrid(this.dgvMovimientos, new System.Drawing.Point(10, 10), new System.Drawing.Size(884, 430));
            // 
            // btnMovimientosRefrescar
            // 
            ConfigurarBoton(this.btnMovimientosRefrescar, "Refrescar", new System.Drawing.Point(10, 450));
            this.btnMovimientosRefrescar.Click += new System.EventHandler(this.btnMovimientosRefrescar_Click);
            // 
            // btnMovimientosEliminar
            // 
            ConfigurarBoton(this.btnMovimientosEliminar, "Eliminar", new System.Drawing.Point(130, 450));
            this.btnMovimientosEliminar.Click += new System.EventHandler(this.btnMovimientosEliminar_Click);
            // 
            // AdminPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 584);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.ir_menu);
            this.Controls.Add(this.lblTitulo);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "AdminPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PANEL ADMINISTRATIVO";
            this.Load += new System.EventHandler(this.AdminPanel_Load);
            this.tabs.ResumeLayout(false);
            this.tabUsuarios.ResumeLayout(false);
            this.tabPartidas.ResumeLayout(false);
            this.tabMovimientos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPartidas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ConfigurarGrid(System.Windows.Forms.DataGridView g, System.Drawing.Point loc, System.Drawing.Size size)
        {
            g.Location = loc;
            g.Size = size;
            g.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                     | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.ReadOnly = true;
            g.RowHeadersVisible = false;
            g.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ConfigurarBoton(System.Windows.Forms.Button b, string texto, System.Drawing.Point loc)
        {
            b.Location = loc;
            b.Size = new System.Drawing.Size(110, 32);
            b.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            b.Text = texto;
            b.UseVisualStyleBackColor = true;
        }
    }
}
