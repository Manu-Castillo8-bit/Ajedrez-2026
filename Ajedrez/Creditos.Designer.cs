namespace Ajedrez
{
    partial class Creditos
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblCuerpo = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(12, 22);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(536, 34);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "AGRADECIMIENTOS";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCuerpo
            // 
            this.lblCuerpo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular);
            this.lblCuerpo.Location = new System.Drawing.Point(48, 78);
            this.lblCuerpo.Name = "lblCuerpo";
            this.lblCuerpo.Size = new System.Drawing.Size(464, 236);
            this.lblCuerpo.TabIndex = 1;
            this.lblCuerpo.Text = "A Jonathan Palacios, por su valiosa ayuda y consejería en el ajedrez.\r\n\r\n" +
    "Al profe Arcides.\r\n\r\n" +
    "Y a cada maestro, por compartirnos un poco de sus conocimientos.\r\n\r\n" +
    "¡Muchas gracias!";
            this.lblCuerpo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(225, 330);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(110, 36);
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // Creditos
            // 
            this.AcceptButton = this.btnCerrar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCerrar;
            this.ClientSize = new System.Drawing.Size(560, 386);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.lblCuerpo);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Creditos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Créditos";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblCuerpo;
        private System.Windows.Forms.Button btnCerrar;
    }
}
