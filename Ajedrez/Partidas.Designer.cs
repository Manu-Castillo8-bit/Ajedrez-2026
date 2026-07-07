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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.todas = new System.Windows.Forms.Button();
            this.ganadas = new System.Windows.Forms.Button();
            this.perdidas = new System.Windows.Forms.Button();
            this.tablas = new System.Windows.Forms.Button();
            this.fecha = new System.Windows.Forms.Button();
            this.damas = new System.Windows.Forms.RadioButton();
            this.ajedrez = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_oscuro
            // 
            this.m_oscuro.Location = new System.Drawing.Point(94, 19);
            this.m_oscuro.Name = "m_oscuro";
            this.m_oscuro.Size = new System.Drawing.Size(55, 40);
            this.m_oscuro.TabIndex = 13;
            this.m_oscuro.Text = "MODO OSCURO";
            this.m_oscuro.UseVisualStyleBackColor = true;
            // 
            // m_claro
            // 
            this.m_claro.Location = new System.Drawing.Point(19, 19);
            this.m_claro.Name = "m_claro";
            this.m_claro.Size = new System.Drawing.Size(55, 40);
            this.m_claro.TabIndex = 14;
            this.m_claro.Text = "MODO CLARO";
            this.m_claro.UseVisualStyleBackColor = true;
            // 
            // ir_menu
            // 
            this.ir_menu.Location = new System.Drawing.Point(38, 376);
            this.ir_menu.Name = "ir_menu";
            this.ir_menu.Size = new System.Drawing.Size(101, 35);
            this.ir_menu.TabIndex = 12;
            this.ir_menu.Text = "MENU";
            this.ir_menu.UseVisualStyleBackColor = true;
            this.ir_menu.Click += new System.EventHandler(this.ir_menu_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.ir_menu);
            this.groupBox3.Controls.Add(this.m_oscuro);
            this.groupBox3.Controls.Add(this.m_claro);
            this.groupBox3.Location = new System.Drawing.Point(626, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(165, 426);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(276, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "PARTIDAS";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 111);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(589, 312);
            this.dataGridView1.TabIndex = 18;
            // 
            // todas
            // 
            this.todas.Location = new System.Drawing.Point(12, 62);
            this.todas.Name = "todas";
            this.todas.Size = new System.Drawing.Size(75, 33);
            this.todas.TabIndex = 19;
            this.todas.Text = "TODAS";
            this.todas.UseVisualStyleBackColor = true;
            this.todas.Click += new System.EventHandler(this.button1_Click);
            // 
            // ganadas
            // 
            this.ganadas.Location = new System.Drawing.Point(93, 62);
            this.ganadas.Name = "ganadas";
            this.ganadas.Size = new System.Drawing.Size(75, 33);
            this.ganadas.TabIndex = 20;
            this.ganadas.Text = "VICTORIAS";
            this.ganadas.UseVisualStyleBackColor = true;
            this.ganadas.Click += new System.EventHandler(this.button2_Click);
            // 
            // perdidas
            // 
            this.perdidas.Location = new System.Drawing.Point(174, 63);
            this.perdidas.Name = "perdidas";
            this.perdidas.Size = new System.Drawing.Size(75, 33);
            this.perdidas.TabIndex = 21;
            this.perdidas.Text = "DERROTAS";
            this.perdidas.UseVisualStyleBackColor = true;
            this.perdidas.Click += new System.EventHandler(this.button3_Click);
            // 
            // tablas
            // 
            this.tablas.Location = new System.Drawing.Point(256, 63);
            this.tablas.Name = "tablas";
            this.tablas.Size = new System.Drawing.Size(75, 33);
            this.tablas.TabIndex = 22;
            this.tablas.Text = "EMPATES";
            this.tablas.UseVisualStyleBackColor = true;
            this.tablas.Click += new System.EventHandler(this.button4_Click);
            // 
            // fecha
            // 
            this.fecha.Location = new System.Drawing.Point(337, 63);
            this.fecha.Name = "fecha";
            this.fecha.Size = new System.Drawing.Size(75, 33);
            this.fecha.TabIndex = 23;
            this.fecha.Text = "FECHA";
            this.fecha.UseVisualStyleBackColor = true;
            this.fecha.Click += new System.EventHandler(this.button5_Click);
            // 
            // damas
            // 
            this.damas.AutoSize = true;
            this.damas.Location = new System.Drawing.Point(461, 54);
            this.damas.Name = "damas";
            this.damas.Size = new System.Drawing.Size(63, 17);
            this.damas.TabIndex = 24;
            this.damas.TabStop = true;
            this.damas.Text = "DAMAS";
            this.damas.UseVisualStyleBackColor = true;
            // 
            // ajedrez
            // 
            this.ajedrez.AutoSize = true;
            this.ajedrez.Location = new System.Drawing.Point(461, 79);
            this.ajedrez.Name = "ajedrez";
            this.ajedrez.Size = new System.Drawing.Size(74, 17);
            this.ajedrez.TabIndex = 25;
            this.ajedrez.TabStop = true;
            this.ajedrez.Text = "AJEDREZ";
            this.ajedrez.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(19, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(129, 273);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ESTADISTICAS:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(72, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(72, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "label5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(72, 246);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "label6";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "_______________";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "_______________";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 162);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "_______________";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 214);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "_______________";
            // 
            // Partidas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ajedrez);
            this.Controls.Add(this.damas);
            this.Controls.Add(this.fecha);
            this.Controls.Add(this.tablas);
            this.Controls.Add(this.perdidas);
            this.Controls.Add(this.ganadas);
            this.Controls.Add(this.todas);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Name = "Partidas";
            this.Text = "PARTIDAS";
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_oscuro;
        private System.Windows.Forms.Button m_claro;
        private System.Windows.Forms.Button ir_menu;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button todas;
        private System.Windows.Forms.Button ganadas;
        private System.Windows.Forms.Button perdidas;
        private System.Windows.Forms.Button tablas;
        private System.Windows.Forms.Button fecha;
        private System.Windows.Forms.RadioButton damas;
        private System.Windows.Forms.RadioButton ajedrez;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}