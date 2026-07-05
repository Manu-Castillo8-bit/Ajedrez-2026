namespace Ajedrez
{
    partial class REGISTRADOR
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_claro = new System.Windows.Forms.Button();
            this.m_oscuro = new System.Windows.Forms.Button();
            this.continuar = new System.Windows.Forms.Button();
            this.contraseña = new System.Windows.Forms.TextBox();
            this.ir_login = new System.Windows.Forms.LinkLabel();
            this.nombre = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Contraseña";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Nombre de usuario";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.m_claro);
            this.groupBox1.Controls.Add(this.m_oscuro);
            this.groupBox1.Location = new System.Drawing.Point(408, -28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(169, 379);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "TEMA DE LA APP";
            // 
            // m_claro
            // 
            this.m_claro.Location = new System.Drawing.Point(21, 76);
            this.m_claro.Name = "m_claro";
            this.m_claro.Size = new System.Drawing.Size(51, 38);
            this.m_claro.TabIndex = 5;
            this.m_claro.UseVisualStyleBackColor = true;
            // 
            // m_oscuro
            // 
            this.m_oscuro.Location = new System.Drawing.Point(90, 76);
            this.m_oscuro.Name = "m_oscuro";
            this.m_oscuro.Size = new System.Drawing.Size(51, 38);
            this.m_oscuro.TabIndex = 2;
            this.m_oscuro.UseVisualStyleBackColor = true;
            // 
            // continuar
            // 
            this.continuar.Location = new System.Drawing.Point(123, 261);
            this.continuar.Name = "continuar";
            this.continuar.Size = new System.Drawing.Size(169, 39);
            this.continuar.TabIndex = 14;
            this.continuar.Text = "CONTINUAR";
            this.continuar.UseVisualStyleBackColor = true;
            // 
            // contraseña
            // 
            this.contraseña.Location = new System.Drawing.Point(36, 151);
            this.contraseña.Name = "contraseña";
            this.contraseña.Size = new System.Drawing.Size(323, 20);
            this.contraseña.TabIndex = 13;
            // 
            // ir_login
            // 
            this.ir_login.AutoSize = true;
            this.ir_login.Location = new System.Drawing.Point(123, 183);
            this.ir_login.Name = "ir_login";
            this.ir_login.Size = new System.Drawing.Size(187, 13);
            this.ir_login.TabIndex = 12;
            this.ir_login.TabStop = true;
            this.ir_login.Text = "¿Ya posees una cuenta? Inicia sesión";
            // 
            // nombre
            // 
            this.nombre.Location = new System.Drawing.Point(35, 85);
            this.nombre.Name = "nombre";
            this.nombre.Size = new System.Drawing.Size(323, 20);
            this.nombre.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(164, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "REGISTRARSE";
            // 
            // REGISTRADOR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 323);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.continuar);
            this.Controls.Add(this.contraseña);
            this.Controls.Add(this.ir_login);
            this.Controls.Add(this.nombre);
            this.Controls.Add(this.label1);
            this.Name = "REGISTRADOR";
            this.Text = "REGISTRADOR";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_claro;
        private System.Windows.Forms.Button m_oscuro;
        private System.Windows.Forms.Button continuar;
        private System.Windows.Forms.TextBox contraseña;
        private System.Windows.Forms.LinkLabel ir_login;
        private System.Windows.Forms.TextBox nombre;
        private System.Windows.Forms.Label label1;
    }
}