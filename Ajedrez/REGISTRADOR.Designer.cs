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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(REGISTRADOR));
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_claro = new System.Windows.Forms.Button();
            this.m_oscuro = new System.Windows.Forms.Button();
            this.g_usuario = new System.Windows.Forms.Button();
            this.contraseña = new System.Windows.Forms.TextBox();
            this.nombre = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.ingresar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(79, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Contraseña";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Nombre de usuario";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.ingresar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.m_claro);
            this.groupBox1.Controls.Add(this.m_oscuro);
            this.groupBox1.Location = new System.Drawing.Point(416, -28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(161, 379);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "TEMA DE LA APP";
            // 
            // m_claro
            // 
            this.m_claro.BackgroundImage = global::Ajedrez.Properties.Resources.sol_negro;
            this.m_claro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.m_claro.Location = new System.Drawing.Point(30, 76);
            this.m_claro.Name = "m_claro";
            this.m_claro.Size = new System.Drawing.Size(40, 38);
            this.m_claro.TabIndex = 5;
            this.m_claro.UseVisualStyleBackColor = true;
            this.m_claro.Click += new System.EventHandler(this.m_claro_Click);
            // 
            // m_oscuro
            // 
            this.m_oscuro.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("m_oscuro.BackgroundImage")));
            this.m_oscuro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.m_oscuro.Location = new System.Drawing.Point(99, 76);
            this.m_oscuro.Name = "m_oscuro";
            this.m_oscuro.Size = new System.Drawing.Size(40, 38);
            this.m_oscuro.TabIndex = 2;
            this.m_oscuro.UseVisualStyleBackColor = true;
            this.m_oscuro.Click += new System.EventHandler(this.m_oscuro_Click);
            // 
            // g_usuario
            // 
            this.g_usuario.Location = new System.Drawing.Point(123, 261);
            this.g_usuario.Name = "g_usuario";
            this.g_usuario.Size = new System.Drawing.Size(169, 39);
            this.g_usuario.TabIndex = 14;
            this.g_usuario.Text = "GUARDAR USUARIO";
            this.g_usuario.UseVisualStyleBackColor = true;
            this.g_usuario.Click += new System.EventHandler(this.g_usuario_Click);
            // 
            // contraseña
            // 
            this.contraseña.Location = new System.Drawing.Point(72, 142);
            this.contraseña.Name = "contraseña";
            this.contraseña.Size = new System.Drawing.Size(220, 20);
            this.contraseña.TabIndex = 13;
            this.contraseña.UseSystemPasswordChar = true;
            // 
            // nombre
            // 
            this.nombre.Location = new System.Drawing.Point(71, 76);
            this.nombre.Name = "nombre";
            this.nombre.Size = new System.Drawing.Size(220, 20);
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
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Ajedrez.Properties.Resources.persona_negra;
            this.pictureBox1.Location = new System.Drawing.Point(29, 61);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(36, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Ajedrez.Properties.Resources.candado_blanco;
            this.pictureBox2.Location = new System.Drawing.Point(29, 127);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(36, 35);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 19;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Ajedrez.Properties.Resources.ver_contra_negro;
            this.pictureBox3.Location = new System.Drawing.Point(281, 135);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(36, 35);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 20;
            this.pictureBox3.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(317, 145);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(98, 17);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "Ver contraseña";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Ajedrez.Properties.Resources.persona_blanca;
            this.pictureBox4.Location = new System.Drawing.Point(29, 60);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(36, 35);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 22;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::Ajedrez.Properties.Resources.candado_negro;
            this.pictureBox5.Location = new System.Drawing.Point(29, 127);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(36, 35);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 23;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::Ajedrez.Properties.Resources.ver_contra;
            this.pictureBox6.Location = new System.Drawing.Point(281, 135);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(36, 35);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox6.TabIndex = 24;
            this.pictureBox6.TabStop = false;
            // 
            // ingresar
            // 
            this.ingresar.Location = new System.Drawing.Point(28, 304);
            this.ingresar.Name = "ingresar";
            this.ingresar.Size = new System.Drawing.Size(111, 35);
            this.ingresar.TabIndex = 9;
            this.ingresar.Text = "INGRESAR";
            this.ingresar.UseVisualStyleBackColor = true;
            this.ingresar.Click += new System.EventHandler(this.ingresar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 289);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "¿Ya tienes cuenta, ingresa?";
            // 
            // REGISTRADOR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 323);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.g_usuario);
            this.Controls.Add(this.contraseña);
            this.Controls.Add(this.nombre);
            this.Controls.Add(this.label1);
            this.Name = "REGISTRADOR";
            this.Text = "REGISTRADOR";
            this.Load += new System.EventHandler(this.REGISTRADOR_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
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
        private System.Windows.Forms.Button g_usuario;
        private System.Windows.Forms.TextBox contraseña;
        private System.Windows.Forms.TextBox nombre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Button ingresar;
        private System.Windows.Forms.Label label5;
    }
}