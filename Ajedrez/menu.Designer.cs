namespace Ajedrez
{
    partial class menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ajedrez = new System.Windows.Forms.Button();
            this.ir_damas = new System.Windows.Forms.Button();
            this.ir_perfil = new System.Windows.Forms.Button();
            this.ir_partidas = new System.Windows.Forms.Button();
            this.ir_login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ajedrez
            // 
            this.ajedrez.Location = new System.Drawing.Point(48, 80);
            this.ajedrez.Name = "ajedrez";
            this.ajedrez.Size = new System.Drawing.Size(113, 68);
            this.ajedrez.TabIndex = 9;
            this.ajedrez.Text = "AJEDREZ";
            this.ajedrez.UseVisualStyleBackColor = true;
            this.ajedrez.Click += new System.EventHandler(this.ajedrez_Click);
            // 
            // ir_damas
            // 
            this.ir_damas.Location = new System.Drawing.Point(181, 80);
            this.ir_damas.Name = "ir_damas";
            this.ir_damas.Size = new System.Drawing.Size(113, 68);
            this.ir_damas.TabIndex = 10;
            this.ir_damas.Text = "DAMAS";
            this.ir_damas.UseVisualStyleBackColor = true;
            this.ir_damas.Click += new System.EventHandler(this.ir_damas_Click);
            // 
            // ir_perfil
            // 
            this.ir_perfil.Location = new System.Drawing.Point(48, 158);
            this.ir_perfil.Name = "ir_perfil";
            this.ir_perfil.Size = new System.Drawing.Size(113, 68);
            this.ir_perfil.TabIndex = 11;
            this.ir_perfil.Text = "PERFIL";
            this.ir_perfil.UseVisualStyleBackColor = true;
            this.ir_perfil.Click += new System.EventHandler(this.ir_perfil_Click);
            // 
            // ir_partidas
            // 
            this.ir_partidas.Location = new System.Drawing.Point(181, 158);
            this.ir_partidas.Name = "ir_partidas";
            this.ir_partidas.Size = new System.Drawing.Size(113, 68);
            this.ir_partidas.TabIndex = 12;
            this.ir_partidas.Text = "REGISTRO DE PARTIDAS";
            this.ir_partidas.UseVisualStyleBackColor = true;
            this.ir_partidas.Click += new System.EventHandler(this.ir_movimientos_Click);
            // 
            // ir_login
            // 
            this.ir_login.Location = new System.Drawing.Point(12, 12);
            this.ir_login.Name = "ir_login";
            this.ir_login.Size = new System.Drawing.Size(75, 23);
            this.ir_login.TabIndex = 13;
            this.ir_login.Text = "LOGIN";
            this.ir_login.UseVisualStyleBackColor = true;
            this.ir_login.Click += new System.EventHandler(this.ir_login_Click);
            // 
            // menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 299);
            this.Controls.Add(this.ir_login);
            this.Controls.Add(this.ir_partidas);
            this.Controls.Add(this.ir_perfil);
            this.Controls.Add(this.ir_damas);
            this.Controls.Add(this.ajedrez);
            this.Name = "menu";
            this.Text = "MENU";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ajedrez;
        private System.Windows.Forms.Button ir_damas;
        private System.Windows.Forms.Button ir_perfil;
        private System.Windows.Forms.Button ir_partidas;
        private System.Windows.Forms.Button ir_login;
    }
}