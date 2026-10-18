namespace Ajedrez
{
    partial class UsuarioEditor
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

        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblContrasena;
        private System.Windows.Forms.Label lblElo;
        private System.Windows.Forms.Label lblJugadas;
        private System.Windows.Forms.Label lblGanadas;
        private System.Windows.Forms.Label lblEmpatadas;
        private System.Windows.Forms.Label lblPerdidas;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtContrasena;
        private System.Windows.Forms.CheckBox chkVer;
        private System.Windows.Forms.TextBox txtElo;
        private System.Windows.Forms.TextBox txtJugadas;
        private System.Windows.Forms.TextBox txtGanadas;
        private System.Windows.Forms.TextBox txtEmpatadas;
        private System.Windows.Forms.TextBox txtPerdidas;
        private System.Windows.Forms.CheckBox chkActivo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsuarioEditor));
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblContrasena = new System.Windows.Forms.Label();
            this.lblElo = new System.Windows.Forms.Label();
            this.lblJugadas = new System.Windows.Forms.Label();
            this.lblGanadas = new System.Windows.Forms.Label();
            this.lblEmpatadas = new System.Windows.Forms.Label();
            this.lblPerdidas = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtContrasena = new System.Windows.Forms.TextBox();
            this.chkVer = new System.Windows.Forms.CheckBox();
            this.txtElo = new System.Windows.Forms.TextBox();
            this.txtJugadas = new System.Windows.Forms.TextBox();
            this.txtGanadas = new System.Windows.Forms.TextBox();
            this.txtEmpatadas = new System.Windows.Forms.TextBox();
            this.txtPerdidas = new System.Windows.Forms.TextBox();
            this.chkActivo = new System.Windows.Forms.CheckBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(14, 19);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(99, 13);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre de usuario:";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(14, 51);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email:";
            // 
            // lblContrasena
            // 
            this.lblContrasena.AutoSize = true;
            this.lblContrasena.Location = new System.Drawing.Point(14, 83);
            this.lblContrasena.Name = "lblContrasena";
            this.lblContrasena.Size = new System.Drawing.Size(64, 13);
            this.lblContrasena.TabIndex = 4;
            this.lblContrasena.Text = "Contrasena:";
            // 
            // lblElo
            // 
            this.lblElo.AutoSize = true;
            this.lblElo.Location = new System.Drawing.Point(14, 115);
            this.lblElo.Name = "lblElo";
            this.lblElo.Size = new System.Drawing.Size(31, 13);
            this.lblElo.TabIndex = 7;
            this.lblElo.Text = "ELO:";
            // 
            // lblJugadas
            // 
            this.lblJugadas.AutoSize = true;
            this.lblJugadas.Location = new System.Drawing.Point(14, 147);
            this.lblJugadas.Name = "lblJugadas";
            this.lblJugadas.Size = new System.Drawing.Size(88, 13);
            this.lblJugadas.TabIndex = 9;
            this.lblJugadas.Text = "Partidas jugadas:";
            // 
            // lblGanadas
            // 
            this.lblGanadas.AutoSize = true;
            this.lblGanadas.Location = new System.Drawing.Point(14, 179);
            this.lblGanadas.Name = "lblGanadas";
            this.lblGanadas.Size = new System.Drawing.Size(92, 13);
            this.lblGanadas.TabIndex = 11;
            this.lblGanadas.Text = "Partidas ganadas:";
            // 
            // lblEmpatadas
            // 
            this.lblEmpatadas.AutoSize = true;
            this.lblEmpatadas.Location = new System.Drawing.Point(14, 211);
            this.lblEmpatadas.Name = "lblEmpatadas";
            this.lblEmpatadas.Size = new System.Drawing.Size(103, 13);
            this.lblEmpatadas.TabIndex = 13;
            this.lblEmpatadas.Text = "Partidas empatadas:";
            // 
            // lblPerdidas
            // 
            this.lblPerdidas.AutoSize = true;
            this.lblPerdidas.Location = new System.Drawing.Point(14, 243);
            this.lblPerdidas.Name = "lblPerdidas";
            this.lblPerdidas.Size = new System.Drawing.Size(91, 13);
            this.lblPerdidas.TabIndex = 15;
            this.lblPerdidas.Text = "Partidas perdidas:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(250, 15);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(180, 20);
            this.txtNombre.TabIndex = 1;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(250, 47);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(180, 20);
            this.txtEmail.TabIndex = 3;
            // 
            // txtContrasena
            // 
            this.txtContrasena.Location = new System.Drawing.Point(250, 79);
            this.txtContrasena.Name = "txtContrasena";
            this.txtContrasena.Size = new System.Drawing.Size(180, 20);
            this.txtContrasena.TabIndex = 5;
            this.txtContrasena.UseSystemPasswordChar = true;
            // 
            // chkVer
            // 
            this.chkVer.AutoSize = true;
            this.chkVer.Location = new System.Drawing.Point(444, 82);
            this.chkVer.Name = "chkVer";
            this.chkVer.Size = new System.Drawing.Size(41, 17);
            this.chkVer.TabIndex = 6;
            this.chkVer.Text = "ver";
            this.chkVer.UseVisualStyleBackColor = true;
            this.chkVer.CheckedChanged += new System.EventHandler(this.chkVer_CheckedChanged);
            // 
            // txtElo
            // 
            this.txtElo.Location = new System.Drawing.Point(250, 111);
            this.txtElo.Name = "txtElo";
            this.txtElo.Size = new System.Drawing.Size(180, 20);
            this.txtElo.TabIndex = 8;
            // 
            // txtJugadas
            // 
            this.txtJugadas.Location = new System.Drawing.Point(250, 143);
            this.txtJugadas.Name = "txtJugadas";
            this.txtJugadas.Size = new System.Drawing.Size(180, 20);
            this.txtJugadas.TabIndex = 10;
            // 
            // txtGanadas
            // 
            this.txtGanadas.Location = new System.Drawing.Point(250, 175);
            this.txtGanadas.Name = "txtGanadas";
            this.txtGanadas.Size = new System.Drawing.Size(180, 20);
            this.txtGanadas.TabIndex = 12;
            // 
            // txtEmpatadas
            // 
            this.txtEmpatadas.Location = new System.Drawing.Point(250, 207);
            this.txtEmpatadas.Name = "txtEmpatadas";
            this.txtEmpatadas.Size = new System.Drawing.Size(180, 20);
            this.txtEmpatadas.TabIndex = 14;
            // 
            // txtPerdidas
            // 
            this.txtPerdidas.Location = new System.Drawing.Point(250, 239);
            this.txtPerdidas.Name = "txtPerdidas";
            this.txtPerdidas.Size = new System.Drawing.Size(180, 20);
            this.txtPerdidas.TabIndex = 16;
            // 
            // chkActivo
            // 
            this.chkActivo.AutoSize = true;
            this.chkActivo.Location = new System.Drawing.Point(250, 275);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(56, 17);
            this.chkActivo.TabIndex = 17;
            this.chkActivo.Text = "Activo";
            this.chkActivo.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(192, 311);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 32);
            this.btnGuardar.TabIndex = 18;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(322, 311);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(130, 32);
            this.btnCancelar.TabIndex = 19;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // UsuarioEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 361);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblContrasena);
            this.Controls.Add(this.txtContrasena);
            this.Controls.Add(this.chkVer);
            this.Controls.Add(this.lblElo);
            this.Controls.Add(this.txtElo);
            this.Controls.Add(this.lblJugadas);
            this.Controls.Add(this.txtJugadas);
            this.Controls.Add(this.lblGanadas);
            this.Controls.Add(this.txtGanadas);
            this.Controls.Add(this.lblEmpatadas);
            this.Controls.Add(this.txtEmpatadas);
            this.Controls.Add(this.lblPerdidas);
            this.Controls.Add(this.txtPerdidas);
            this.Controls.Add(this.chkActivo);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UsuarioEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Usuario";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
