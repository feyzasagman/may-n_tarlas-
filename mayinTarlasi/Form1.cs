using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace mayınTarlası
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        // Global değişkenler
        private int toplamGüvenliHücre;
        private int açılanHücreSayısı = 0;
        private int kazanmaSayisi = 0;
        private int kaybetmeSayisi = 0;
        private DateTime oyunBaslangicZamani;
        private int[,] komsuMayinSayilari;
        private bool[,] mayinMatrisi;
        private Timer zamanlayici;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Form başarıyla yüklendi!", "Bilgi");
        }

        private void InitializeUI()
        {
            PanelControl panelTop = new PanelControl
            {
                Name = "panelTop",
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(220, 220, 220),
                BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat
            };
            this.Controls.Add(panelTop);

            LabelControl lblMayinSayisi = new LabelControl
            {
                Name = "lblMayinSayisi",
                Text = "Kaç Mayın Olsun?",
                Location = new Point(10, 15),
                Appearance = { ForeColor = Color.Navy, Font = new Font("Arial", 12, FontStyle.Bold) }
            };
            panelTop.Controls.Add(lblMayinSayisi);

            SpinEdit spinMayinSayisi = new SpinEdit
            {
                Name = "spinMayinSayisi",
                Properties = { MaxValue = 900, MinValue = 1 },
                Value = 10,
                Location = new Point(120, 10)
            };
            panelTop.Controls.Add(spinMayinSayisi);

            LabelControl lblSkor = new LabelControl
            {
                Name = "lblSkor",
                Text = "Kazanç: 0 | Kayıp: 0",
                Location = new Point(400, 15),
                Appearance = { ForeColor = Color.Green, Font = new Font("Arial", 10, FontStyle.Bold) }
            };
            panelTop.Controls.Add(lblSkor);

            LabelControl lblZaman = new LabelControl
            {
                Name = "lblZaman",
                Text = "Geçen Süre: 0 sn",
                Location = new Point(600, 15),
                Appearance = { ForeColor = Color.DarkBlue, Font = new Font("Arial", 10, FontStyle.Bold) }
            };
            panelTop.Controls.Add(lblZaman);

            SimpleButton btnBaslat = new SimpleButton
            {
                Name = "btnBaslat",
                Text = "Oyunu Başlat",
                Location = new Point(200, 10),
                Appearance = { BackColor = Color.Green, ForeColor = Color.White }
            };
            btnBaslat.Click += (s, e) => BtnBaslat_Click(spinMayinSayisi);
            panelTop.Controls.Add(btnBaslat);
        }

        private void BtnBaslat_Click(SpinEdit spinMayinSayisi)
        {
            oyunBaslangicZamani = DateTime.Now;

            if (zamanlayici != null) zamanlayici.Stop();
            zamanlayici = new Timer { Interval = 1000 };
            zamanlayici.Tick += (s, e) =>
            {
                TimeSpan geçenSüre = DateTime.Now - oyunBaslangicZamani;
                LabelControl lblZaman = this.Controls.Find("lblZaman", true)[0] as LabelControl;
                lblZaman.Text = $"Geçen Süre: {geçenSüre.Seconds} sn";
            };
            zamanlayici.Start();

            int mayinSayisi = (int)spinMayinSayisi.Value;

            if (mayinSayisi <= 0 || mayinSayisi > 900)
            {
                MessageBox.Show("Geçerli bir mayın sayısı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InitializeGrid(mayinSayisi);
        }

        private void InitializeGrid(int mayinSayisi)
        {
            this.Controls.Clear();
            InitializeUI();

            toplamGüvenliHücre = 30 * 30 - mayinSayisi;
            açılanHücreSayısı = 0;

            Panel gamePanel = new Panel
            {
                Name = "gamePanel",
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            this.Controls.Add(gamePanel);

            int buttonSize = 20;
            gamePanel.Size = new Size(buttonSize * 30, buttonSize * 30);

            mayinMatrisi = new bool[30, 30];
            komsuMayinSayilari = new int[30, 30];
            Random random = new Random();

            int i = 0;
            while (i < mayinSayisi)
            {
                int row = random.Next(0, 30);
                int col = random.Next(0, 30);

                if (!mayinMatrisi[row, col])
                {
                    mayinMatrisi[row, col] = true;
                    i++;
                }
            }

            // Komşu mayın sayısını hesapla
            for (int row = 0; row < 30; row++)
            {
                for (int col = 0; col < 30; col++)
                {
                    if (mayinMatrisi[row, col]) continue;

                    int komsuMayin = 0;
                    for (int r = -1; r <= 1; r++)
                    {
                        for (int c = -1; c <= 1; c++)
                        {
                            int yeniRow = row + r;
                            int yeniCol = col + c;

                            if (yeniRow >= 0 && yeniRow < 30 && yeniCol >= 0 && yeniCol < 30 && mayinMatrisi[yeniRow, yeniCol])
                            {
                                komsuMayin++;
                            }
                        }
                    }
                    komsuMayinSayilari[row, col] = komsuMayin;
                }
            }

            for (int row = 0; row < 30; row++)
            {
                for (int col = 0; col < 30; col++)
                {
                    Button button = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(col * buttonSize, row * buttonSize),
                        Name = $"btn_{row}_{col}",
                        Tag = mayinMatrisi[row, col],
                        BackColor = Color.LightGray
                    };

                    button.Click += Button_Click;
                    gamePanel.Controls.Add(button);
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button == null) return;

            bool isMine = (bool)button.Tag;

            if (isMine)
            {
                button.Text = "💣";
                button.BackColor = Color.Red;
                kaybetmeSayisi++;
                zamanlayici.Stop();

                foreach (Control ctrl in this.Controls.Find("gamePanel", true)[0].Controls)
                {
                    if (ctrl is Button btn && (bool)btn.Tag)
                    {
                        btn.Text = "💣";
                        btn.BackColor = Color.Red;
                    }
                }

                MessageBox.Show($"Mayına bastınız! Oyun bitti. Toplam kayıp: {kaybetmeSayisi}", "Oyun Bitti", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                UpdateSkor();
            }
            else
            {
                if (button.Text == "")
                {
                    açılanHücreSayısı++;
                    int row = int.Parse(button.Name.Split('_')[1]);
                    int col = int.Parse(button.Name.Split('_')[2]);
                    int komsuMayin = komsuMayinSayilari[row, col];

                    button.Text = komsuMayin > 0 ? komsuMayin.ToString() : "";
                    button.BackColor = Color.LightGreen;

                    if (komsuMayin == 1) button.ForeColor = Color.Blue;
                    else if (komsuMayin == 2) button.ForeColor = Color.Green;
                    else if (komsuMayin == 3) button.ForeColor = Color.Red;
                    else if (komsuMayin > 3) button.ForeColor = Color.DarkMagenta;
                    else button.ForeColor = Color.Black;

                    if (açılanHücreSayısı == toplamGüvenliHücre)
                    {
                        kazanmaSayisi++;
                        zamanlayici.Stop();
                        TimeSpan oyunSuresi = DateTime.Now - oyunBaslangicZamani;
                        MessageBox.Show($"Tebrikler! Oyunu kazandınız.\nToplam Süre: {oyunSuresi.TotalSeconds:F2} saniye\nToplam Kazanç: {kazanmaSayisi}", "Kazandınız!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UpdateSkor();
                    }
                }
            }
        }

        private void UpdateSkor()
        {
            LabelControl lblSkor = this.Controls.Find("lblSkor", true)[0] as LabelControl;
            lblSkor.Text = $"Kazanç: {kazanmaSayisi} | Kayıp: {kaybetmeSayisi}";
        }
    }
}







