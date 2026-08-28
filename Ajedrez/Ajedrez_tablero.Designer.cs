namespace Ajedrez
{
    partial class Ajedrez_tablero
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ajedrez_tablero));
            this.G = new System.Windows.Forms.Panel();
            this.puntos_n = new System.Windows.Forms.PictureBox();
            this.puntos_b = new System.Windows.Forms.PictureBox();
            this.p_negras = new System.Windows.Forms.Label();
            this.p_blancas = new System.Windows.Forms.Label();
            this.m_oscuro = new System.Windows.Forms.Button();
            this.m_claro = new System.Windows.Forms.Button();
            this.deshacer = new System.Windows.Forms.Button();
            this.ir_menu = new System.Windows.Forms.Button();
            this.puntosn = new System.Windows.Forms.Label();
            this.puntosb = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.puntos_n)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.puntos_b)).BeginInit();
            this.SuspendLayout();
            // 
            // G
            // 
            this.G.BackColor = System.Drawing.Color.Gray;
            this.G.Location = new System.Drawing.Point(10, 10);
            this.G.Name = "G";
            this.G.Size = new System.Drawing.Size(484, 484);
            this.G.TabIndex = 0;
            // 
            // puntos_n
            // 
            this.puntos_n.Image = global::Ajedrez.Properties.Resources.peon_negro_pixel;
            this.puntos_n.Location = new System.Drawing.Point(612, 298);
            this.puntos_n.Name = "puntos_n";
            this.puntos_n.Size = new System.Drawing.Size(49, 50);
            this.puntos_n.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.puntos_n.TabIndex = 1;
            this.puntos_n.TabStop = false;
            // 
            // puntos_b
            // 
            this.puntos_b.Image = global::Ajedrez.Properties.Resources.peon;
            this.puntos_b.Location = new System.Drawing.Point(612, 166);
            this.puntos_b.Name = "puntos_b";
            this.puntos_b.Size = new System.Drawing.Size(49, 50);
            this.puntos_b.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.puntos_b.TabIndex = 2;
            this.puntos_b.TabStop = false;
            // 
            // p_negras
            // 
            this.p_negras.AutoSize = true;
            this.p_negras.Font = new System.Drawing.Font("Californian FB", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p_negras.Location = new System.Drawing.Point(673, 317);
            this.p_negras.Name = "p_negras";
            this.p_negras.Size = new System.Drawing.Size(28, 31);
            this.p_negras.TabIndex = 3;
            this.p_negras.Text = "0";
            // 
            // p_blancas
            // 
            this.p_blancas.AutoSize = true;
            this.p_blancas.Font = new System.Drawing.Font("Californian FB", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p_blancas.Location = new System.Drawing.Point(673, 185);
            this.p_blancas.Name = "p_blancas";
            this.p_blancas.Size = new System.Drawing.Size(28, 31);
            this.p_blancas.TabIndex = 4;
            this.p_blancas.Text = "0";
            // 
            // m_oscuro
            // 
            this.m_oscuro.BackgroundImage = global::Ajedrez.Properties.Resources.luna_blanca;
            this.m_oscuro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.m_oscuro.Location = new System.Drawing.Point(728, 22);
            this.m_oscuro.Name = "m_oscuro";
            this.m_oscuro.Size = new System.Drawing.Size(40, 40);
            this.m_oscuro.TabIndex = 8;
            this.m_oscuro.UseVisualStyleBackColor = true;
            this.m_oscuro.Click += new System.EventHandler(this.m_oscuro_Click);
            // 
            // m_claro
            // 
            this.m_claro.BackgroundImage = global::Ajedrez.Properties.Resources.sol_negro;
            this.m_claro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.m_claro.ForeColor = System.Drawing.Color.Transparent;
            this.m_claro.Location = new System.Drawing.Point(667, 22);
            this.m_claro.Name = "m_claro";
            this.m_claro.Size = new System.Drawing.Size(40, 40);
            this.m_claro.TabIndex = 9;
            this.m_claro.UseVisualStyleBackColor = true;
            this.m_claro.Click += new System.EventHandler(this.m_claro_Click);
            // 
            // deshacer
            // 
            this.deshacer.BackgroundImage = global::Ajedrez.Properties.Resources.regresar_negro;
            this.deshacer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deshacer.Location = new System.Drawing.Point(667, 449);
            this.deshacer.Name = "deshacer";
            this.deshacer.Size = new System.Drawing.Size(43, 43);
            this.deshacer.TabIndex = 11;
            this.deshacer.UseVisualStyleBackColor = true;
            // 
            // ir_menu
            // 
            this.ir_menu.BackgroundImage = global::Ajedrez.Properties.Resources.menu_negro;
            this.ir_menu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ir_menu.Location = new System.Drawing.Point(728, 449);
            this.ir_menu.Name = "ir_menu";
            this.ir_menu.Size = new System.Drawing.Size(43, 43);
            this.ir_menu.TabIndex = 10;
            this.ir_menu.UseVisualStyleBackColor = true;
            this.ir_menu.Click += new System.EventHandler(this.menu_Click_1);
            // 
            // puntosn
            // 
            this.puntosn.AutoSize = true;
            this.puntosn.Location = new System.Drawing.Point(603, 282);
            this.puntosn.Name = "puntosn";
            this.puntosn.Size = new System.Drawing.Size(103, 13);
            this.puntosn.TabIndex = 12;
            this.puntosn.Text = "PUNTOS NEGRAS:";
            // 
            // puntosb
            // 
            this.puntosb.AutoSize = true;
            this.puntosb.Location = new System.Drawing.Point(603, 150);
            this.puntosb.Name = "puntosb";
            this.puntosb.Size = new System.Drawing.Size(107, 13);
            this.puntosb.TabIndex = 13;
            this.puntosb.Text = "PUNTOS BLANCAS:";
            // 
            // Ajedrez_tablero
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 517);
            this.Controls.Add(this.puntosb);
            this.Controls.Add(this.puntosn);
            this.Controls.Add(this.deshacer);
            this.Controls.Add(this.ir_menu);
            this.Controls.Add(this.m_oscuro);
            this.Controls.Add(this.m_claro);
            this.Controls.Add(this.p_blancas);
            this.Controls.Add(this.p_negras);
            this.Controls.Add(this.puntos_b);
            this.Controls.Add(this.puntos_n);
            this.Controls.Add(this.G);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Ajedrez_tablero";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AJEDREZ";
            this.Load += new System.EventHandler(this.Ajedrez_tablero_Load);
            ((System.ComponentModel.ISupportInitialize)(this.puntos_n)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.puntos_b)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel G;
        private System.Windows.Forms.PictureBox puntos_n;
        private System.Windows.Forms.PictureBox puntos_b;
        private System.Windows.Forms.Label p_negras;
        private System.Windows.Forms.Label p_blancas;
        private System.Windows.Forms.Button m_oscuro;
        private System.Windows.Forms.Button m_claro;
        private System.Windows.Forms.Button deshacer;
        private System.Windows.Forms.Button ir_menu;
        private System.Windows.Forms.Label puntosn;
        private System.Windows.Forms.Label puntosb;
    }
}