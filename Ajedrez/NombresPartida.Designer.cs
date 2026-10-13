namespace Ajedrez
{
    partial class NombresPartida
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
            this.lblLocal = new System.Windows.Forms.Label();
            this.txtLocal = new System.Windows.Forms.TextBox();
            this.lblOponente = new System.Windows.Forms.Label();
            this.txtOponente = new System.Windows.Forms.TextBox();
            this.lblModo = new System.Windows.Forms.Label();
            this.rbNormal = new System.Windows.Forms.RadioButton();
            this.rbSoplar = new System.Windows.Forms.RadioButton();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLocal
            // 
            this.lblLocal.AutoSize = true;
            this.lblLocal.Location = new System.Drawing.Point(24, 30);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.Size = new System.Drawing.Size(78, 13);
            this.lblLocal.TabIndex = 0;
            this.lblLocal.Text = "Jugador (tú):";
            // 
            // txtLocal
            // 
            this.txtLocal.Location = new System.Drawing.Point(150, 27);
            this.txtLocal.Name = "txtLocal";
            this.txtLocal.Size = new System.Drawing.Size(200, 20);
            this.txtLocal.TabIndex = 1;
            // 
            // lblOponente
            // 
            this.lblOponente.AutoSize = true;
            this.lblOponente.Location = new System.Drawing.Point(24, 70);
            this.lblOponente.Name = "lblOponente";
            this.lblOponente.Size = new System.Drawing.Size(63, 13);
            this.lblOponente.TabIndex = 2;
            this.lblOponente.Text = "Oponente:";
            // 
            // txtOponente
            // 
            this.txtOponente.Location = new System.Drawing.Point(150, 67);
            this.txtOponente.Name = "txtOponente";
            this.txtOponente.Size = new System.Drawing.Size(200, 20);
            this.txtOponente.TabIndex = 3;
            // 
            // lblModo
            // 
            this.lblModo.AutoSize = true;
            this.lblModo.Location = new System.Drawing.Point(24, 103);
            this.lblModo.Name = "lblModo";
            this.lblModo.Size = new System.Drawing.Size(98, 13);
            this.lblModo.TabIndex = 4;
            this.lblModo.Text = "Regla de captura:";
            // 
            // rbNormal
            // 
            this.rbNormal.AutoSize = true;
            this.rbNormal.Checked = true;
            this.rbNormal.Location = new System.Drawing.Point(150, 101);
            this.rbNormal.Name = "rbNormal";
            this.rbNormal.Size = new System.Drawing.Size(163, 17);
            this.rbNormal.TabIndex = 5;
            this.rbNormal.TabStop = true;
            this.rbNormal.Text = "Normal (captura obligatoria)";
            this.rbNormal.UseVisualStyleBackColor = true;
            // 
            // rbSoplar
            // 
            this.rbSoplar.AutoSize = true;
            this.rbSoplar.Location = new System.Drawing.Point(150, 123);
            this.rbSoplar.Name = "rbSoplar";
            this.rbSoplar.Size = new System.Drawing.Size(157, 17);
            this.rbSoplar.TabIndex = 6;
            this.rbSoplar.Text = "Soplar piezas si no comes";
            this.rbSoplar.UseVisualStyleBackColor = true;
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(150, 155);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(95, 30);
            this.btnIniciar.TabIndex = 7;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(255, 155);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(95, 30);
            this.btnCancelar.TabIndex = 8;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // NombresPartida
            // 
            this.AcceptButton = this.btnIniciar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(384, 200);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.rbSoplar);
            this.Controls.Add(this.rbNormal);
            this.Controls.Add(this.lblModo);
            this.Controls.Add(this.txtOponente);
            this.Controls.Add(this.lblOponente);
            this.Controls.Add(this.txtLocal);
            this.Controls.Add(this.lblLocal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NombresPartida";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NUEVA PARTIDA - DAMAS";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblLocal;
        private System.Windows.Forms.TextBox txtLocal;
        private System.Windows.Forms.Label lblOponente;
        private System.Windows.Forms.TextBox txtOponente;
        private System.Windows.Forms.Label lblModo;
        private System.Windows.Forms.RadioButton rbNormal;
        private System.Windows.Forms.RadioButton rbSoplar;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
