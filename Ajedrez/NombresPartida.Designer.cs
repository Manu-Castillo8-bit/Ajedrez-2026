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
            this.gbFichas = new System.Windows.Forms.GroupBox();
            this.rbNegras = new System.Windows.Forms.RadioButton();
            this.rbBlancas = new System.Windows.Forms.RadioButton();
            this.lblModo = new System.Windows.Forms.Label();
            this.rbNormal = new System.Windows.Forms.RadioButton();
            this.rbSoplar = new System.Windows.Forms.RadioButton();
            this.gbModo = new System.Windows.Forms.GroupBox();
            this.lblNivel = new System.Windows.Forms.Label();
            this.cmbNivel = new System.Windows.Forms.ComboBox();
            this.rbIA = new System.Windows.Forms.RadioButton();
            this.rbLocal = new System.Windows.Forms.RadioButton();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gbFichas.SuspendLayout();
            this.gbModo.SuspendLayout();
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
            this.txtLocal.Location = new System.Drawing.Point(180, 27);
            this.txtLocal.Name = "txtLocal";
            this.txtLocal.Size = new System.Drawing.Size(170, 20);
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
            this.txtOponente.Location = new System.Drawing.Point(180, 67);
            this.txtOponente.Name = "txtOponente";
            this.txtOponente.Size = new System.Drawing.Size(170, 20);
            this.txtOponente.TabIndex = 3;
            // 
            // gbFichas
            // 
            this.gbFichas.Controls.Add(this.rbNegras);
            this.gbFichas.Controls.Add(this.rbBlancas);
            this.gbFichas.Location = new System.Drawing.Point(24, 100);
            this.gbFichas.Name = "gbFichas";
            this.gbFichas.Size = new System.Drawing.Size(326, 56);
            this.gbFichas.TabIndex = 4;
            this.gbFichas.TabStop = false;
            this.gbFichas.Text = "Mis fichas:";
            // 
            // rbBlancas
            // 
            this.rbBlancas.AutoSize = true;
            this.rbBlancas.Checked = true;
            this.rbBlancas.Location = new System.Drawing.Point(18, 22);
            this.rbBlancas.Name = "rbBlancas";
            this.rbBlancas.Size = new System.Drawing.Size(66, 17);
            this.rbBlancas.TabIndex = 0;
            this.rbBlancas.TabStop = true;
            this.rbBlancas.Text = "Blancas";
            this.rbBlancas.UseVisualStyleBackColor = true;
            // 
            // rbNegras
            // 
            this.rbNegras.AutoSize = true;
            this.rbNegras.Location = new System.Drawing.Point(160, 22);
            this.rbNegras.Name = "rbNegras";
            this.rbNegras.Size = new System.Drawing.Size(61, 17);
            this.rbNegras.TabIndex = 1;
            this.rbNegras.Text = "Negras";
            this.rbNegras.UseVisualStyleBackColor = true;
            // 
            // lblModo
            // 
            this.lblModo.AutoSize = true;
            this.lblModo.Location = new System.Drawing.Point(24, 170);
            this.lblModo.Name = "lblModo";
            this.lblModo.Size = new System.Drawing.Size(98, 13);
            this.lblModo.TabIndex = 5;
            this.lblModo.Text = "Regla de captura:";
            // 
            // rbNormal
            // 
            this.rbNormal.AutoSize = true;
            this.rbNormal.Checked = true;
            this.rbNormal.Location = new System.Drawing.Point(150, 190);
            this.rbNormal.Name = "rbNormal";
            this.rbNormal.Size = new System.Drawing.Size(163, 17);
            this.rbNormal.TabIndex = 6;
            this.rbNormal.TabStop = true;
            this.rbNormal.Text = "Normal (captura obligatoria)";
            this.rbNormal.UseVisualStyleBackColor = true;
            // 
            // rbSoplar
            // 
            this.rbSoplar.AutoSize = true;
            this.rbSoplar.Location = new System.Drawing.Point(150, 212);
            this.rbSoplar.Name = "rbSoplar";
            this.rbSoplar.Size = new System.Drawing.Size(157, 17);
            this.rbSoplar.TabIndex = 7;
            this.rbSoplar.Text = "Soplar piezas si no comes";
            this.rbSoplar.UseVisualStyleBackColor = true;
            // 
            // gbModo
            // 
            this.gbModo.Controls.Add(this.lblNivel);
            this.gbModo.Controls.Add(this.cmbNivel);
            this.gbModo.Controls.Add(this.rbIA);
            this.gbModo.Controls.Add(this.rbLocal);
            this.gbModo.Location = new System.Drawing.Point(24, 162);
            this.gbModo.Name = "gbModo";
            this.gbModo.Size = new System.Drawing.Size(326, 76);
            this.gbModo.TabIndex = 8;
            this.gbModo.TabStop = false;
            this.gbModo.Text = "Modo de juego:";
            // 
            // rbLocal
            // 
            this.rbLocal.AutoSize = true;
            this.rbLocal.Checked = true;
            this.rbLocal.Location = new System.Drawing.Point(18, 20);
            this.rbLocal.Name = "rbLocal";
            this.rbLocal.Size = new System.Drawing.Size(166, 17);
            this.rbLocal.TabIndex = 0;
            this.rbLocal.TabStop = true;
            this.rbLocal.Text = "Dos jugadores (local)";
            this.rbLocal.UseVisualStyleBackColor = true;
            this.rbLocal.CheckedChanged += new System.EventHandler(this.rbLocal_CheckedChanged);
            // 
            // rbIA
            // 
            this.rbIA.AutoSize = true;
            this.rbIA.Location = new System.Drawing.Point(18, 44);
            this.rbIA.Name = "rbIA";
            this.rbIA.Size = new System.Drawing.Size(150, 17);
            this.rbIA.TabIndex = 1;
            this.rbIA.Text = "Jugar contra la IA";
            this.rbIA.UseVisualStyleBackColor = true;
            this.rbIA.CheckedChanged += new System.EventHandler(this.rbIA_CheckedChanged);
            // 
            // lblNivel
            // 
            this.lblNivel.AutoSize = true;
            this.lblNivel.Location = new System.Drawing.Point(190, 22);
            this.lblNivel.Name = "lblNivel";
            this.lblNivel.Size = new System.Drawing.Size(58, 13);
            this.lblNivel.TabIndex = 2;
            this.lblNivel.Text = "Dificultad:";
            // 
            // cmbNivel
            // 
            this.cmbNivel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNivel.FormattingEnabled = true;
            this.cmbNivel.Items.AddRange(new object[] {
            "Fácil",
            "Media",
            "Difícil",
            "Experto"});
            this.cmbNivel.Location = new System.Drawing.Point(190, 42);
            this.cmbNivel.Name = "cmbNivel";
            this.cmbNivel.Size = new System.Drawing.Size(120, 21);
            this.cmbNivel.TabIndex = 3;
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(125, 248);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(100, 30);
            this.btnIniciar.TabIndex = 8;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(235, 248);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(115, 30);
            this.btnCancelar.TabIndex = 9;
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
            this.ClientSize = new System.Drawing.Size(384, 292);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.gbModo);
            this.Controls.Add(this.rbSoplar);
            this.Controls.Add(this.rbNormal);
            this.Controls.Add(this.lblModo);
            this.Controls.Add(this.gbFichas);
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
            this.gbFichas.ResumeLayout(false);
            this.gbFichas.PerformLayout();
            this.gbModo.ResumeLayout(false);
            this.gbModo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblLocal;
        private System.Windows.Forms.TextBox txtLocal;
        private System.Windows.Forms.Label lblOponente;
        private System.Windows.Forms.TextBox txtOponente;
        private System.Windows.Forms.GroupBox gbFichas;
        private System.Windows.Forms.RadioButton rbBlancas;
        private System.Windows.Forms.RadioButton rbNegras;
        private System.Windows.Forms.Label lblModo;
        private System.Windows.Forms.RadioButton rbNormal;
        private System.Windows.Forms.RadioButton rbSoplar;
        private System.Windows.Forms.GroupBox gbModo;
        private System.Windows.Forms.Label lblNivel;
        private System.Windows.Forms.ComboBox cmbNivel;
        private System.Windows.Forms.RadioButton rbIA;
        private System.Windows.Forms.RadioButton rbLocal;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
