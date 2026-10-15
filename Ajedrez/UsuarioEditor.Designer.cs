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

            int y = 15;
            Fila("Nombre de usuario:", this.txtNombre, ref y);
            Fila("Email:", this.txtEmail, ref y);
            Fila("Contraseña:", this.txtContrasena, ref y);
            this.txtContrasena.UseSystemPasswordChar = true;

            this.chkVer.AutoSize = true;
            this.chkVer.Location = new System.Drawing.Point(372, 82);
            this.chkVer.Name = "chkVer";
            this.chkVer.Text = "ver";
            this.chkVer.UseVisualStyleBackColor = true;
            this.chkVer.CheckedChanged += new System.EventHandler(this.chkVer_CheckedChanged);
            this.Controls.Add(this.chkVer);

            Fila("ELO:", this.txtElo, ref y);
            Fila("Partidas jugadas:", this.txtJugadas, ref y);
            Fila("Partidas ganadas:", this.txtGanadas, ref y);
            Fila("Partidas empatadas:", this.txtEmpatadas, ref y);
            Fila("Partidas perdidas:", this.txtPerdidas, ref y);

            this.chkActivo.AutoSize = true;
            this.chkActivo.Location = new System.Drawing.Point(178, y + 4);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Text = "Activo";
            this.chkActivo.UseVisualStyleBackColor = true;
            this.Controls.Add(this.chkActivo);

            this.btnGuardar.Location = new System.Drawing.Point(120, y + 40);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(110, 32);
            this.btnGuardar.TabIndex = 20;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            this.Controls.Add(this.btnGuardar);

            this.btnCancelar.Location = new System.Drawing.Point(250, y + 40);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 32);
            this.btnCancelar.TabIndex = 21;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            this.Controls.Add(this.btnCancelar);

            // 
            // UsuarioEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, y + 90);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UsuarioEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Usuario";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Coloca una etiqueta con un campo de texto debajo de la anterior.
        private void Fila(string etiqueta, System.Windows.Forms.TextBox campo, ref int y)
        {
            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
            lbl.AutoSize = true;
            lbl.Location = new System.Drawing.Point(14, y + 4);
            lbl.Text = etiqueta;
            this.Controls.Add(lbl);

            campo.Location = new System.Drawing.Point(178, y);
            campo.Size = new System.Drawing.Size(180, 22);
            this.Controls.Add(campo);

            y += 32;
        }
    }
}
