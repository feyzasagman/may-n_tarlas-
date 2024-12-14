namespace mayınTarlası
{
    partial class Form1
    {
        /// <summary>
        /// Gerekli designer değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Kullanılan tüm kaynakları temizler.
        /// </summary>
        /// <param name="disposing">Yönetilen kaynaklar silinmeli mi?</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer tarafından oluşturulan kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot.
        /// Bu metodun içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600); // Geniş form
            this.ClientSize = new System.Drawing.Size(600, 400); // Daha küçük form

            this.Name = "Form1";
            this.Text = "Mayın Tarlası - Başarılar!";
            this.Text = "Mayın Tarlası - Zor Seviye";

            this.Load += new System.EventHandler(this.Form1_Load); // Load olayı bağlantısı
            this.ResumeLayout(false);
        }

        #endregion
    }
}
