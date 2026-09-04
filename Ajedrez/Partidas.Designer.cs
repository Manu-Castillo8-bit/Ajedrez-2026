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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Partidas));
            this.m_oscuro = new System.Windows.Forms.Button();
            this.m_claro = new System.Windows.Forms.Button();
            this.ir_menu = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ganadas_ = new System.Windows.Forms.Label();
            this.perdidas_ = new System.Windows.Forms.Label();
            this.empates_ = new System.Windows.Forms.Label();
            this.line4 = new System.Windows.Forms.Label();
            this.line3 = new System.Windows.Forms.Label();
            this.line2 = new System.Windows.Forms.Label();
            this.line1 = new System.Windows.Forms.Label();
            this.todas_ = new System.Windows.Forms.Label();
            this.total = new System.Windows.Forms.Label();
            this.empates = new System.Windows.Forms.Label();
            this.derrotas = new System.Windows.Forms.Label();
            this.victorias_ = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.todas = new System.Windows.Forms.Button();
            this.ganadas = new System.Windows.Forms.Button();
            this.perdidas = new System.Windows.Forms.Button();
            this.tablas = new System.Windows.Forms.Button();
            this.fecha = new System.Windows.Forms.Button();
            this.damas = new System.Windows.Forms.RadioButton();
            this.ajedrez = new System.Windows.Forms.RadioButton();
            this.picture_todas = new System.Windows.Forms.PictureBox();
            this.picture_victorias = new System.Windows.Forms.PictureBox();
            this.picture_derrotas = new System.Windows.Forms.PictureBox();
            this.picture_empates = new System.Windows.Forms.PictureBox();
            this.picture_fecha = new System.Windows.Forms.PictureBox();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_todas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_victorias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_derrotas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_empates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_fecha)).BeginInit();
            this.SuspendLayout();
            // 
            // m_oscuro
            // 
            this.m_oscuro.BackgroundImage = global::Ajedrez.Properties.Resources.luna_blanca;
            this.m_oscuro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.m_oscuro.Location = new System.Drawing.Point(94, 19);
            this.m_oscuro.Name = "m_oscuro";
            this.m_oscuro.Size = new System.Drawing.Size(40, 40);
            this.m_oscuro.TabIndex = 13;
            this.m_oscuro.UseVisualStyleBackColor = true;
            this.m_oscuro.Click += new System.EventHandler(this.m_oscuro_Click);
            // 
            // m_claro
            // 
            this.m_claro.BackgroundImage = global::Ajedrez.Properties.Resources.sol_negro;
            this.m_claro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.m_claro.Location = new System.Drawing.Point(19, 19);
            this.m_claro.Name = "m_claro";
            this.m_claro.Size = new System.Drawing.Size(40, 40);
            this.m_claro.TabIndex = 14;
            this.m_claro.UseVisualStyleBackColor = true;
            this.m_claro.Click += new System.EventHandler(this.m_claro_Click);
            // 
            // ir_menu
            // 
            this.ir_menu.BackgroundImage = global::Ajedrez.Properties.Resources.menu_negro;
            this.ir_menu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ir_menu.Location = new System.Drawing.Point(80, 368);
            this.ir_menu.Name = "ir_menu";
            this.ir_menu.Size = new System.Drawing.Size(49, 43);
            this.ir_menu.TabIndex = 12;
            this.ir_menu.UseVisualStyleBackColor = true;
            this.ir_menu.Click += new System.EventHandler(this.ir_menu_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.ir_menu);
            this.groupBox3.Controls.Add(this.m_oscuro);
            this.groupBox3.Controls.Add(this.m_claro);
            this.groupBox3.Location = new System.Drawing.Point(874, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(204, 426);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ganadas_);
            this.groupBox1.Controls.Add(this.perdidas_);
            this.groupBox1.Controls.Add(this.empates_);
            this.groupBox1.Controls.Add(this.line4);
            this.groupBox1.Controls.Add(this.line3);
            this.groupBox1.Controls.Add(this.line2);
            this.groupBox1.Controls.Add(this.line1);
            this.groupBox1.Controls.Add(this.todas_);
            this.groupBox1.Controls.Add(this.total);
            this.groupBox1.Controls.Add(this.empates);
            this.groupBox1.Controls.Add(this.derrotas);
            this.groupBox1.Controls.Add(this.victorias_);
            this.groupBox1.Location = new System.Drawing.Point(19, 79);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 273);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ESTADISTICAS:";
            // 
            // ganadas_
            // 
            this.ganadas_.AutoSize = true;
            this.ganadas_.Location = new System.Drawing.Point(16, 46);
            this.ganadas_.Name = "ganadas_";
            this.ganadas_.Size = new System.Drawing.Size(35, 13);
            this.ganadas_.TabIndex = 11;
            this.ganadas_.Text = "label4";
            // 
            // perdidas_
            // 
            this.perdidas_.AutoSize = true;
            this.perdidas_.Location = new System.Drawing.Point(16, 91);
            this.perdidas_.Name = "perdidas_";
            this.perdidas_.Size = new System.Drawing.Size(35, 13);
            this.perdidas_.TabIndex = 10;
            this.perdidas_.Text = "label3";
            // 
            // empates_
            // 
            this.empates_.AutoSize = true;
            this.empates_.Location = new System.Drawing.Point(16, 143);
            this.empates_.Name = "empates_";
            this.empates_.Size = new System.Drawing.Size(35, 13);
            this.empates_.TabIndex = 9;
            this.empates_.Text = "label2";
            // 
            // line4
            // 
            this.line4.AutoSize = true;
            this.line4.Location = new System.Drawing.Point(16, 214);
            this.line4.Name = "line4";
            this.line4.Size = new System.Drawing.Size(133, 13);
            this.line4.TabIndex = 8;
            this.line4.Text = "_____________________";
            // 
            // line3
            // 
            this.line3.AutoSize = true;
            this.line3.Location = new System.Drawing.Point(16, 162);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(133, 13);
            this.line3.TabIndex = 7;
            this.line3.Text = "_____________________";
            // 
            // line2
            // 
            this.line2.AutoSize = true;
            this.line2.Location = new System.Drawing.Point(16, 111);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(133, 13);
            this.line2.TabIndex = 6;
            this.line2.Text = "_____________________";
            // 
            // line1
            // 
            this.line1.AutoSize = true;
            this.line1.Location = new System.Drawing.Point(16, 60);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(133, 13);
            this.line1.TabIndex = 5;
            this.line1.Text = "_____________________";
            // 
            // todas_
            // 
            this.todas_.AutoSize = true;
            this.todas_.Location = new System.Drawing.Point(16, 193);
            this.todas_.Name = "todas_";
            this.todas_.Size = new System.Drawing.Size(35, 13);
            this.todas_.TabIndex = 4;
            this.todas_.Text = "label6";
            // 
            // total
            // 
            this.total.AutoSize = true;
            this.total.Location = new System.Drawing.Point(103, 193);
            this.total.Name = "total";
            this.total.Size = new System.Drawing.Size(35, 13);
            this.total.TabIndex = 3;
            this.total.Text = "label5";
            // 
            // empates
            // 
            this.empates.AutoSize = true;
            this.empates.Location = new System.Drawing.Point(103, 143);
            this.empates.Name = "empates";
            this.empates.Size = new System.Drawing.Size(35, 13);
            this.empates.TabIndex = 2;
            this.empates.Text = "label4";
            // 
            // derrotas
            // 
            this.derrotas.AutoSize = true;
            this.derrotas.Location = new System.Drawing.Point(103, 91);
            this.derrotas.Name = "derrotas";
            this.derrotas.Size = new System.Drawing.Size(35, 13);
            this.derrotas.TabIndex = 1;
            this.derrotas.Text = "label3";
            // 
            // victorias_
            // 
            this.victorias_.AutoSize = true;
            this.victorias_.Location = new System.Drawing.Point(103, 46);
            this.victorias_.Name = "victorias_";
            this.victorias_.Size = new System.Drawing.Size(35, 13);
            this.victorias_.TabIndex = 0;
            this.victorias_.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(401, 31);
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
            this.dataGridView1.Size = new System.Drawing.Size(856, 312);
            this.dataGridView1.TabIndex = 18;
            // 
            // todas
            // 
            this.todas.Location = new System.Drawing.Point(18, 62);
            this.todas.Name = "todas";
            this.todas.Size = new System.Drawing.Size(133, 43);
            this.todas.TabIndex = 19;
            this.todas.Text = "TODAS";
            this.todas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.todas.UseVisualStyleBackColor = true;
            this.todas.Click += new System.EventHandler(this.button1_Click);
            // 
            // ganadas
            // 
            this.ganadas.Location = new System.Drawing.Point(154, 60);
            this.ganadas.Name = "ganadas";
            this.ganadas.Size = new System.Drawing.Size(168, 43);
            this.ganadas.TabIndex = 20;
            this.ganadas.Text = "VICTORIAS";
            this.ganadas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ganadas.UseVisualStyleBackColor = true;
            this.ganadas.Click += new System.EventHandler(this.button2_Click);
            // 
            // perdidas
            // 
            this.perdidas.Location = new System.Drawing.Point(325, 60);
            this.perdidas.Name = "perdidas";
            this.perdidas.Size = new System.Drawing.Size(170, 43);
            this.perdidas.TabIndex = 21;
            this.perdidas.Text = "DERROTAS";
            this.perdidas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.perdidas.UseVisualStyleBackColor = true;
            this.perdidas.Click += new System.EventHandler(this.button3_Click);
            // 
            // tablas
            // 
            this.tablas.Location = new System.Drawing.Point(500, 60);
            this.tablas.Name = "tablas";
            this.tablas.Size = new System.Drawing.Size(167, 43);
            this.tablas.TabIndex = 22;
            this.tablas.Text = "EMPATES";
            this.tablas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tablas.UseVisualStyleBackColor = true;
            this.tablas.Click += new System.EventHandler(this.button4_Click);
            // 
            // fecha
            // 
            this.fecha.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.fecha.Location = new System.Drawing.Point(668, 60);
            this.fecha.Name = "fecha";
            this.fecha.Size = new System.Drawing.Size(117, 43);
            this.fecha.TabIndex = 23;
            this.fecha.Text = "FECHA";
            this.fecha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.fecha.UseVisualStyleBackColor = true;
            this.fecha.Click += new System.EventHandler(this.button5_Click);
            // 
            // damas
            // 
            this.damas.AutoSize = true;
            this.damas.Location = new System.Drawing.Point(794, 58);
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
            this.ajedrez.Location = new System.Drawing.Point(794, 83);
            this.ajedrez.Name = "ajedrez";
            this.ajedrez.Size = new System.Drawing.Size(74, 17);
            this.ajedrez.TabIndex = 25;
            this.ajedrez.TabStop = true;
            this.ajedrez.Text = "AJEDREZ";
            this.ajedrez.UseVisualStyleBackColor = true;
            // 
            // picture_todas
            // 
            this.picture_todas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.picture_todas.Image = global::Ajedrez.Properties.Resources.batalla;
            this.picture_todas.Location = new System.Drawing.Point(23, 67);
            this.picture_todas.Name = "picture_todas";
            this.picture_todas.Size = new System.Drawing.Size(45, 33);
            this.picture_todas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_todas.TabIndex = 26;
            this.picture_todas.TabStop = false;
            // 
            // picture_victorias
            // 
            this.picture_victorias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(184)))), ((int)(((byte)(40)))));
            this.picture_victorias.Image = global::Ajedrez.Properties.Resources.aprobado;
            this.picture_victorias.Location = new System.Drawing.Point(163, 65);
            this.picture_victorias.Name = "picture_victorias";
            this.picture_victorias.Size = new System.Drawing.Size(45, 33);
            this.picture_victorias.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_victorias.TabIndex = 27;
            this.picture_victorias.TabStop = false;
            // 
            // picture_derrotas
            // 
            this.picture_derrotas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(23)))), ((int)(((byte)(19)))));
            this.picture_derrotas.Image = global::Ajedrez.Properties.Resources.no_aprobado;
            this.picture_derrotas.Location = new System.Drawing.Point(330, 65);
            this.picture_derrotas.Name = "picture_derrotas";
            this.picture_derrotas.Size = new System.Drawing.Size(45, 33);
            this.picture_derrotas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_derrotas.TabIndex = 28;
            this.picture_derrotas.TabStop = false;
            // 
            // picture_empates
            // 
            this.picture_empates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(211)))), ((int)(((byte)(5)))));
            this.picture_empates.Image = global::Ajedrez.Properties.Resources.empate;
            this.picture_empates.Location = new System.Drawing.Point(505, 65);
            this.picture_empates.Name = "picture_empates";
            this.picture_empates.Size = new System.Drawing.Size(53, 33);
            this.picture_empates.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_empates.TabIndex = 29;
            this.picture_empates.TabStop = false;
            // 
            // picture_fecha
            // 
            this.picture_fecha.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(61)))), ((int)(((byte)(219)))));
            this.picture_fecha.Image = global::Ajedrez.Properties.Resources.fecha;
            this.picture_fecha.Location = new System.Drawing.Point(673, 65);
            this.picture_fecha.Name = "picture_fecha";
            this.picture_fecha.Size = new System.Drawing.Size(42, 33);
            this.picture_fecha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture_fecha.TabIndex = 30;
            this.picture_fecha.TabStop = false;
            // 
            // Partidas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 450);
            this.Controls.Add(this.picture_fecha);
            this.Controls.Add(this.picture_empates);
            this.Controls.Add(this.picture_derrotas);
            this.Controls.Add(this.picture_victorias);
            this.Controls.Add(this.picture_todas);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Partidas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PARTIDAS";
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_todas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_victorias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_derrotas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_empates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture_fecha)).EndInit();
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
        private System.Windows.Forms.Label line4;
        private System.Windows.Forms.Label line3;
        private System.Windows.Forms.Label line2;
        private System.Windows.Forms.Label line1;
        private System.Windows.Forms.Label todas_;
        private System.Windows.Forms.Label total;
        private System.Windows.Forms.Label empates;
        private System.Windows.Forms.Label derrotas;
        private System.Windows.Forms.Label victorias_;
        private System.Windows.Forms.PictureBox picture_todas;
        private System.Windows.Forms.PictureBox picture_victorias;
        private System.Windows.Forms.PictureBox picture_derrotas;
        private System.Windows.Forms.PictureBox picture_empates;
        private System.Windows.Forms.PictureBox picture_fecha;
        private System.Windows.Forms.Label ganadas_;
        private System.Windows.Forms.Label perdidas_;
        private System.Windows.Forms.Label empates_;
    }
}