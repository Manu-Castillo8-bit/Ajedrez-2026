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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_claro = new System.Windows.Forms.Button();
            this.m_oscuro = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ajedrez
            // 
            this.ajedrez.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.ajedrez.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ajedrez.Location = new System.Drawing.Point(44, 66);
            this.ajedrez.Name = "ajedrez";
            this.ajedrez.Size = new System.Drawing.Size(160, 83);
            this.ajedrez.TabIndex = 9;
            this.ajedrez.Text = "AJEDREZ";
            this.ajedrez.UseVisualStyleBackColor = false;
            this.ajedrez.Click += new System.EventHandler(this.ajedrez_Click);
            // 
            // ir_damas
            // 
            this.ir_damas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(228)))), ((int)(((byte)(240)))));
            this.ir_damas.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ir_damas.Location = new System.Drawing.Point(292, 66);
            this.ir_damas.Name = "ir_damas";
            this.ir_damas.Size = new System.Drawing.Size(160, 83);
            this.ir_damas.TabIndex = 10;
            this.ir_damas.Text = "DAMAS";
            this.ir_damas.UseVisualStyleBackColor = false;
            this.ir_damas.Click += new System.EventHandler(this.ir_damas_Click);
            // 
            // ir_perfil
            // 
            this.ir_perfil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(228)))), ((int)(((byte)(240)))));
            this.ir_perfil.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ir_perfil.Location = new System.Drawing.Point(44, 201);
            this.ir_perfil.Name = "ir_perfil";
            this.ir_perfil.Size = new System.Drawing.Size(160, 83);
            this.ir_perfil.TabIndex = 11;
            this.ir_perfil.Text = "PERFIL";
            this.ir_perfil.UseVisualStyleBackColor = false;
            this.ir_perfil.Click += new System.EventHandler(this.ir_perfil_Click);
            // 
            // ir_partidas
            // 
            this.ir_partidas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.ir_partidas.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ir_partidas.Location = new System.Drawing.Point(292, 201);
            this.ir_partidas.Name = "ir_partidas";
            this.ir_partidas.Size = new System.Drawing.Size(160, 83);
            this.ir_partidas.TabIndex = 12;
            this.ir_partidas.Text = "REGISTRO DE PARTIDAS";
            this.ir_partidas.UseVisualStyleBackColor = false;
            this.ir_partidas.Click += new System.EventHandler(this.ir_movimientos_Click);
            // 
            // ir_login
            // 
            this.ir_login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(228)))), ((int)(((byte)(240)))));
            this.ir_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ir_login.Location = new System.Drawing.Point(51, 372);
            this.ir_login.Name = "ir_login";
            this.ir_login.Size = new System.Drawing.Size(94, 33);
            this.ir_login.TabIndex = 13;
            this.ir_login.Text = "LOGIN";
            this.ir_login.UseVisualStyleBackColor = false;
            this.ir_login.Click += new System.EventHandler(this.ir_login_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ir_login);
            this.groupBox1.Controls.Add(this.m_claro);
            this.groupBox1.Controls.Add(this.m_oscuro);
            this.groupBox1.Location = new System.Drawing.Point(532, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(185, 426);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("04b", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(62, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 14);
            this.label2.TabIndex = 8;
            this.label2.Text = "TEMA";
            // 
            // m_claro
            // 
            this.m_claro.BackgroundImage = global::Ajedrez.Properties.Resources.sol_negro;
            this.m_claro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.m_claro.Location = new System.Drawing.Point(41, 49);
            this.m_claro.Name = "m_claro";
            this.m_claro.Size = new System.Drawing.Size(40, 38);
            this.m_claro.TabIndex = 5;
            this.m_claro.UseVisualStyleBackColor = true;
            this.m_claro.Click += new System.EventHandler(this.m_claro_Click);
            // 
            // m_oscuro
            // 
            this.m_oscuro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(138)))), ((int)(((byte)(244)))));
            this.m_oscuro.BackgroundImage = global::Ajedrez.Properties.Resources.luna_blanca;
            this.m_oscuro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.m_oscuro.Location = new System.Drawing.Point(105, 49);
            this.m_oscuro.Name = "m_oscuro";
            this.m_oscuro.Size = new System.Drawing.Size(40, 38);
            this.m_oscuro.TabIndex = 2;
            this.m_oscuro.UseVisualStyleBackColor = false;
            this.m_oscuro.Click += new System.EventHandler(this.m_oscuro_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("04b", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(161, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 18);
            this.label1.TabIndex = 17;
            this.label1.Text = "BIENVENIDOS";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(9, 305);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 101);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("04b", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(414, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Victorias";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("04b", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(321, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "ELO";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("04b", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(177, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Bienvenido";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 418);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ir_partidas);
            this.Controls.Add(this.ir_perfil);
            this.Controls.Add(this.ir_damas);
            this.Controls.Add(this.ajedrez);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "menu";
            this.Text = "MENU";
            this.Load += new System.EventHandler(this.menu_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ajedrez;
        private System.Windows.Forms.Button ir_damas;
        private System.Windows.Forms.Button ir_perfil;
        private System.Windows.Forms.Button ir_partidas;
        private System.Windows.Forms.Button ir_login;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_claro;
        private System.Windows.Forms.Button m_oscuro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}