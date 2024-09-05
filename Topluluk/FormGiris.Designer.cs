namespace vize_1
{
    partial class FormGiris
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGiris));
            this.bunifuGradientPanel1 = new Bunifu.Framework.UI.BunifuGradientPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonKullaniciGiris = new System.Windows.Forms.Button();
            this.textBoxEposta = new System.Windows.Forms.TextBox();
            this.textBoxSifre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonKucult = new System.Windows.Forms.Button();
            this.buttonKapat = new System.Windows.Forms.Button();
            this.bunifuGradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bunifuGradientPanel1
            // 
            this.bunifuGradientPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuGradientPanel1.BackgroundImage")));
            this.bunifuGradientPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuGradientPanel1.Controls.Add(this.buttonKapat);
            this.bunifuGradientPanel1.Controls.Add(this.buttonKucult);
            this.bunifuGradientPanel1.Controls.Add(this.label1);
            this.bunifuGradientPanel1.Controls.Add(this.label2);
            this.bunifuGradientPanel1.Controls.Add(this.buttonKullaniciGiris);
            this.bunifuGradientPanel1.Controls.Add(this.textBoxEposta);
            this.bunifuGradientPanel1.Controls.Add(this.textBoxSifre);
            this.bunifuGradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bunifuGradientPanel1.GradientBottomLeft = System.Drawing.Color.MidnightBlue;
            this.bunifuGradientPanel1.GradientBottomRight = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.bunifuGradientPanel1.GradientTopLeft = System.Drawing.SystemColors.ActiveCaption;
            this.bunifuGradientPanel1.GradientTopRight = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bunifuGradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.bunifuGradientPanel1.Name = "bunifuGradientPanel1";
            this.bunifuGradientPanel1.Quality = 10;
            this.bunifuGradientPanel1.Size = new System.Drawing.Size(413, 450);
            this.bunifuGradientPanel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(53, 80);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "E-posta:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(53, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "Şifre:";
            // 
            // buttonKullaniciGiris
            // 
            this.buttonKullaniciGiris.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonKullaniciGiris.Location = new System.Drawing.Point(171, 273);
            this.buttonKullaniciGiris.Name = "buttonKullaniciGiris";
            this.buttonKullaniciGiris.Size = new System.Drawing.Size(127, 47);
            this.buttonKullaniciGiris.TabIndex = 4;
            this.buttonKullaniciGiris.Text = "GİRİŞ YAP";
            this.buttonKullaniciGiris.UseVisualStyleBackColor = false;
            this.buttonKullaniciGiris.Click += new System.EventHandler(this.buttonKullaniciGiris_Click);
            // 
            // textBoxEposta
            // 
            this.textBoxEposta.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBoxEposta.Location = new System.Drawing.Point(157, 80);
            this.textBoxEposta.Name = "textBoxEposta";
            this.textBoxEposta.Size = new System.Drawing.Size(175, 29);
            this.textBoxEposta.TabIndex = 2;
            // 
            // textBoxSifre
            // 
            this.textBoxSifre.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBoxSifre.Location = new System.Drawing.Point(157, 182);
            this.textBoxSifre.Name = "textBoxSifre";
            this.textBoxSifre.PasswordChar = '*';
            this.textBoxSifre.Size = new System.Drawing.Size(175, 29);
            this.textBoxSifre.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 412);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 22);
            this.label3.TabIndex = 5;
            // 
            // buttonKucult
            // 
            this.buttonKucult.BackgroundImage = global::vize_1.Properties.Resources._34230_blue_box_icon;
            this.buttonKucult.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonKucult.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonKucult.Location = new System.Drawing.Point(338, 3);
            this.buttonKucult.Name = "buttonKucult";
            this.buttonKucult.Size = new System.Drawing.Size(33, 24);
            this.buttonKucult.TabIndex = 5;
            this.buttonKucult.UseVisualStyleBackColor = true;
            this.buttonKucult.Click += new System.EventHandler(this.buttonKucult_Click);
            // 
            // buttonKapat
            // 
            this.buttonKapat.BackgroundImage = global::vize_1.Properties.Resources._34217_close_delete_remove_icon;
            this.buttonKapat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonKapat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonKapat.Location = new System.Drawing.Point(377, 3);
            this.buttonKapat.Name = "buttonKapat";
            this.buttonKapat.Size = new System.Drawing.Size(33, 24);
            this.buttonKapat.TabIndex = 6;
            this.buttonKapat.UseVisualStyleBackColor = true;
            this.buttonKapat.Click += new System.EventHandler(this.buttonKapat_Click);
            // 
            // FormGiris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(413, 450);
            this.Controls.Add(this.bunifuGradientPanel1);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormGiris";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GİRİŞ";
            this.bunifuGradientPanel1.ResumeLayout(false);
            this.bunifuGradientPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Bunifu.Framework.UI.BunifuGradientPanel bunifuGradientPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonKullaniciGiris;
        private System.Windows.Forms.TextBox textBoxEposta;
        private System.Windows.Forms.TextBox textBoxSifre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonKapat;
        private System.Windows.Forms.Button buttonKucult;
    }
}

