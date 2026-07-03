namespace Ajedrez
{
    partial class Partidas
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
            this.m_oscuro = new System.Windows.Forms.Button();
            this.m_claro = new System.Windows.Forms.Button();
            this.ir_menu = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_oscuro
            // 
            this.m_oscuro.Location = new System.Drawing.Point(698, 120);
            this.m_oscuro.Name = "m_oscuro";
            this.m_oscuro.Size = new System.Drawing.Size(75, 40);
            this.m_oscuro.TabIndex = 13;
            this.m_oscuro.Text = "MODO OSCURO";
            this.m_oscuro.UseVisualStyleBackColor = true;
            // 
            // m_claro
            // 
            this.m_claro.Location = new System.Drawing.Point(698, 66);
            this.m_claro.Name = "m_claro";
            this.m_claro.Size = new System.Drawing.Size(75, 40);
            this.m_claro.TabIndex = 14;
            this.m_claro.Text = "MODO CLARO";
            this.m_claro.UseVisualStyleBackColor = true;
            // 
            // ir_menu
            // 
            this.ir_menu.Location = new System.Drawing.Point(698, 26);
            this.ir_menu.Name = "ir_menu";
            this.ir_menu.Size = new System.Drawing.Size(75, 23);
            this.ir_menu.TabIndex = 12;
            this.ir_menu.Text = "MENU";
            this.ir_menu.UseVisualStyleBackColor = true;
            this.ir_menu.Click += new System.EventHandler(this.ir_menu_Click);
            // 
            // Partidas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.m_oscuro);
            this.Controls.Add(this.m_claro);
            this.Controls.Add(this.ir_menu);
            this.Name = "Partidas";
            this.Text = "PARTIDAS";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_oscuro;
        private System.Windows.Forms.Button m_claro;
        private System.Windows.Forms.Button ir_menu;
    }
}