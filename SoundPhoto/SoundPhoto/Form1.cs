using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SoundPhoto
{
    public partial class Form1 : Form
    {
        Core.SPhoto photo = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string photoPath = null, audio = null;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    photo = new Core.SPhoto(dialog.FileName);
                }
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    photoPath = dialog.FileName;
                }
            }
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    audio = dialog.FileName;
                }
            }

            photo.CreateBasicSPhoto(photoPath, audio);

        }

        private void sPhotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = photo.GetImage();
            var mp3 = new Mp3FileReader(new MemoryStream(photo.GetAudio()));
            var waveout = new WaveOut();
            waveout.Init(mp3);
            waveout.Play();
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            photo = null;
            using(OpenFileDialog dialog = new OpenFileDialog())
            {
                if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    photo = new Core.SPhoto(dialog.FileName);
                }
            }
            photo.OpenExistingSPhoto();
        }
    }
}
