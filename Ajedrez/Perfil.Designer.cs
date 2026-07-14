namespace Ajedrez
{
    partial class Perfil
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ir_menu = new System.Windows.Forms.Button();
            this.m_claro = new System.Windows.Forms.Button();
            this.m_oscuro = new System.Windows.Forms.Button();
            this.picture_partidas = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.picture_derrotas = new System.Windows.Forms.PictureBox();
            this.label = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.picture_victorias = new System.Windows.Forms.PictureBox();
            this.picture_elo = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.nombre = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_partidas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_derrotas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_victorias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_elo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.picture_elo);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.picture_victorias);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.picture_partidas);
            this.groupBox1.Controls.Add(this.label);
            this.groupBox1.Controls.Add(this.picture_derrotas);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 299);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(595, 139);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.UseCompatibleTextRendering = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ir_menu);
            this.groupBox3.Controls.Add(this.m_claro);
            this.groupBox3.Controls.Add(this.m_oscuro);
            this.groupBox3.Location = new System.Drawing.Point(626, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(165, 426);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            // 
            // ir_menu
            // 
            this.ir_menu.BackgroundImage = global::Ajedrez.Properties.Resources.menu_negro;
            this.ir_menu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ir_menu.Location = new System.Drawing.Point(32, 383);
            this.ir_menu.Name = "ir_menu";
            this.ir_menu.Size = new System.Drawing.Size(101, 35);
            this.ir_menu.TabIndex = 8;
            this.ir_menu.UseVisualStyleBackColor = true;
            this.ir_menu.Click += new System.EventHandler(this.ir_menu_Click);
            // 
            // m_claro
            // 
            this.m_claro.BackgroundImage = global::Ajedrez.Properties.Resources.sol_negro;
            this.m_claro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.m_claro.Location = new System.Drawing.Point(33, 19);
            this.m_claro.Name = "m_claro";
            this.m_claro.Size = new System.Drawing.Size(40, 40);
            this.m_claro.TabIndex = 11;
            this.m_claro.UseVisualStyleBackColor = true;
            this.m_claro.Click += new System.EventHandler(this.m_claro_Click);
            // 
            // m_oscuro
            // 
            this.m_oscuro.BackgroundImage = global::Ajedrez.Properties.Resources.luna_negra;
            this.m_oscuro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.m_oscuro.Location = new System.Drawing.Point(108, 19);
            this.m_oscuro.Name = "m_oscuro";
            this.m_oscuro.Size = new System.Drawing.Size(40, 40);
            this.m_oscuro.TabIndex = 10;
            this.m_oscuro.UseVisualStyleBackColor = true;
            this.m_oscuro.Click += new System.EventHandler(this.m_oscuro_Click);
            // 
            // picture_partidas
            // 
            this.picture_partidas.Image = global::Ajedrez.Properties.Resources.batalla;
            this.picture_partidas.Location = new System.Drawing.Point(16, 78);
            this.picture_partidas.Name = "picture_partidas";
            this.picture_partidas.Size = new System.Drawing.Size(44, 42);
            this.picture_partidas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_partidas.TabIndex = 21;
            this.picture_partidas.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("04b", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(380, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 11);
            this.label5.TabIndex = 19;
            this.label5.Text = "DERROTAS";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("04b", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(93, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 11);
            this.label3.TabIndex = 17;
            this.label3.Text = "PARTIDAS";
            // 
            // picture_derrotas
            // 
            this.picture_derrotas.Image = global::Ajedrez.Properties.Resources.no_aprobado;
            this.picture_derrotas.Location = new System.Drawing.Point(321, 78);
            this.picture_derrotas.Name = "picture_derrotas";
            this.picture_derrotas.Size = new System.Drawing.Size(44, 42);
            this.picture_derrotas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_derrotas.TabIndex = 20;
            this.picture_derrotas.TabStop = false;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("04b", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.Location = new System.Drawing.Point(93, 26);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(35, 11);
            this.label.TabIndex = 16;
            this.label.Text = "ELO";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("04b", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(380, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 11);
            this.label4.TabIndex = 18;
            this.label4.Text = "VICTORIAS";
            // 
            // picture_victorias
            // 
            this.picture_victorias.Image = global::Ajedrez.Properties.Resources.trofeo;
            this.picture_victorias.Location = new System.Drawing.Point(321, 22);
            this.picture_victorias.Name = "picture_victorias";
            this.picture_victorias.Size = new System.Drawing.Size(44, 50);
            this.picture_victorias.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_victorias.TabIndex = 22;
            this.picture_victorias.TabStop = false;
            // 
            // picture_elo
            // 
            this.picture_elo.Image = global::Ajedrez.Properties.Resources.elo;
            this.picture_elo.Location = new System.Drawing.Point(16, 22);
            this.picture_elo.Name = "picture_elo";
            this.picture_elo.Size = new System.Drawing.Size(44, 50);
            this.picture_elo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_elo.TabIndex = 23;
            this.picture_elo.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::Ajedrez.Properties.Resources.lapiz;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Location = new System.Drawing.Point(186, 226);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 53);
            this.button1.TabIndex = 37;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(375, 233);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 13);
            this.label14.TabIndex = 36;
            this.label14.Text = "n";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(360, 196);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "n";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(341, 159);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "n";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(336, 120);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "n";
            // 
            // nombre
            // 
            this.nombre.AutoSize = true;
            this.nombre.Location = new System.Drawing.Point(341, 81);
            this.nombre.Name = "nombre";
            this.nombre.Size = new System.Drawing.Size(13, 13);
            this.nombre.TabIndex = 32;
            this.nombre.Text = "n";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("04b", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(264, 233);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(112, 11);
            this.label10.TabIndex = 31;
            this.label10.Text = "DESCRIPCION:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("04b", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(264, 196);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 11);
            this.label9.TabIndex = 30;
            this.label9.Text = "VICTORIAS:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("04b", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(264, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 11);
            this.label8.TabIndex = 29;
            this.label8.Text = "CORREO:";
            // 
            // label7
            // 
            this.label7.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("04b", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(264, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 11);
            this.label7.TabIndex = 28;
            this.label7.Text = "ELO:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("04b", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(264, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 11);
            this.label6.TabIndex = 27;
            this.label6.Text = "NOMBRE:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(39, 71);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(198, 195);
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("04b", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(278, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 21);
            this.label1.TabIndex = 25;
            this.label1.Text = "PERFIL";
            // 
            // Perfil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.nombre);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "Perfil";
            this.Text = "PERFIL";
            this.Load += new System.EventHandler(this.Perfil_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture_partidas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_derrotas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_victorias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_elo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_oscuro;
        private System.Windows.Forms.Button m_claro;
        private System.Windows.Forms.Button ir_menu;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox picture_partidas;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picture_derrotas;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picture_victorias;
        private System.Windows.Forms.PictureBox picture_elo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label nombre;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
    }
}