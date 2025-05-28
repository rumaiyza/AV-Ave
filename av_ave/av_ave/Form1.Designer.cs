using AxWMPLib;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        List<string> filteredFiles = new List<string>();
        FolderBrowserDialog browser = new FolderBrowserDialog();
        int currentFile = 0;

        public Form1()
        {
            InitializeComponent();
            playlist.BackColor = Color.Tomato;
            this.DoubleBuffered = true;
        }

        private void audi_Load(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Normal;
        }

        private void pictureBox1_Click_1(object sender, EventArgs e) { }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void minimise_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e) { }

        private void play_Enter(object sender, EventArgs e) { }

        private void StateChangeEvent(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 0)
            {
                duration.Text = "ready to be loaded";
            }
            else if (e.newState == 1)
            {
                duration.Text = "stopped";
            }
            else if (e.newState == 3)
            {
                duration.Text = "duration: " + play.currentMedia.durationString;
            }
            else if (e.newState == 8)
            {
                if (currentFile >= filteredFiles.Count - 1)
                {
                    currentFile = 0;
                }
                else
                {
                    currentFile += 1;
                }
                playlist.SelectedIndex = currentFile;
                ShowFile(track);
            }
            else if (e.newState == 9)
            {
                duration.Text = "loading next track";
            }
            else if (e.newState == 10)
            {
                timer1.Start();
            }
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            play.Ctlcontrols.play();
            timer1.Stop();
        }

        private void LoadPlaylist()
        {
            play.currentPlaylist = play.newPlaylist("playlist", "");

            foreach (string v in filteredFiles)
            {
                play.currentPlaylist.appendItem(play.newMedia(v));
                playlist.Items.Add(v);
            }

            if (filteredFiles.Count > 0)
            {
                track.Text = "files found: " + filteredFiles.Count;
                playlist.SelectedIndex = currentFile;
                PlayFile(playlist.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("no compatible files in this folder");
            }
        }

        private void PlayFile(string url)
        {
            play.URL = url;
        }

        private void ShowFile(Label x)
        {
            string file = Path.GetFileName(playlist.SelectedItem.ToString());
            x.Text = "" + file;
        }

        private void browse_Click(object sender, EventArgs e)
        {
            play.Ctlcontrols.stop();

            if (filteredFiles.Count > 1)
            {
                filteredFiles.Clear();
                playlist.Items.Clear();
                currentFile = 0;
            }


            if (browser.ShowDialog() == DialogResult.OK)
            {
                filteredFiles = Directory.GetFiles(browser.SelectedPath, "*.*")
                    .Where(file => file.ToLower().EndsWith(".mp3") || file.ToLower().EndsWith(".mp4") ||
                                   file.ToLower().EndsWith(".mkv") || file.ToLower().EndsWith(".dvr-ms") ||
                                   file.ToLower().EndsWith(".mid") || file.ToLower().EndsWith(".midi") ||
                                   file.ToLower().EndsWith(".rmi") || file.ToLower().EndsWith(".aif") ||
                                   file.ToLower().EndsWith(".aifc") || file.ToLower().EndsWith(".aiff") ||
                                   file.ToLower().EndsWith(".wav") || file.ToLower().EndsWith(".m4a") ||
                    file.ToLower().EndsWith(".flac"))
                    .ToList();

                LoadPlaylist();
            }
        }

        private void playlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentFile = playlist.SelectedIndex;
            PlayFile(playlist.SelectedItem.ToString());
            ShowFile(track);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("click on 'browse for tracks' to select media files" + Environment.NewLine + "happy listening/viewing :]");
        }

        private void logo_Click(object sender, EventArgs e) { }
    }
}
