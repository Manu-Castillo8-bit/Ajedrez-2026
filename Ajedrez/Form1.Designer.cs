namespace Ajedrez
{
    partial class Form1
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
            this.G = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.p_negras = new System.Windows.Forms.Label();
            this.p_blancas = new System.Windows.Forms.Label();
            this.perfil = new System.Windows.Forms.Button();
            this.partidas = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
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
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Ajedrez.Properties.Resources.peon_negro;
            this.pictureBox1.Location = new System.Drawing.Point(510, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Ajedrez.Properties.Resources.peon_blanco;
            this.pictureBox2.Location = new System.Drawing.Point(510, 435);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // p_negras
            // 
            this.p_negras.AutoSize = true;
            this.p_negras.Font = new System.Drawing.Font("Californian FB", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p_negras.Location = new System.Drawing.Point(566, 36);
            this.p_negras.Name = "p_negras";
            this.p_negras.Size = new System.Drawing.Size(28, 31);
            this.p_negras.TabIndex = 3;
            this.p_negras.Text = "0";
            // 
            // p_blancas
            // 
            this.p_blancas.AutoSize = true;
            this.p_blancas.Font = new System.Drawing.Font("Californian FB", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p_blancas.Location = new System.Drawing.Point(571, 450);
            this.p_blancas.Name = "p_blancas";
            this.p_blancas.Size = new System.Drawing.Size(28, 31);
            this.p_blancas.TabIndex = 4;
            this.p_blancas.Text = "0";
            // 
            // perfil
            // 
            this.perfil.Location = new System.Drawing.Point(713, 183);
            this.perfil.Name = "perfil";
            this.perfil.Size = new System.Drawing.Size(75, 23);
            this.perfil.TabIndex = 5;
            this.perfil.Text = "PERFIL";
            this.perfil.UseVisualStyleBackColor = true;
            // 
            // partidas
            // 
            this.partidas.Location = new System.Drawing.Point(713, 273);
            this.partidas.Name = "partidas";
            this.partidas.Size = new System.Drawing.Size(75, 23);
            this.partidas.TabIndex = 6;
            this.partidas.Text = "PARTIDAS";
            this.partidas.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 517);
            this.Controls.Add(this.partidas);
            this.Controls.Add(this.perfil);
            this.Controls.Add(this.p_blancas);
            this.Controls.Add(this.p_negras);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.G);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Ajedrez";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel G;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label p_negras;
        private System.Windows.Forms.Label p_blancas;
        private System.Windows.Forms.Button perfil;
        private System.Windows.Forms.Button partidas;
    }
}

