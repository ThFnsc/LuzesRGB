using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace LuzesRGB {
    public partial class MainForm : Form {
        public byte ChannelLimit { get { return Properties.Settings.Default.ChannelLimit; } set { Properties.Settings.Default.ChannelLimit = value; Properties.Settings.Default.Save(); }}
        private bool forceClose = false;
        private bool boot = false;

        IColorizable strip;
        public int Mode { get; set; }
        bool doGraphUpdate = true;
        double hue = 0;
        IAudioProvider audioProvider;
        IAudioToColorConverter audioToColorConverter;
        List<IColorizable> audioResponsiveElements;

        public MainForm(string[] progArgs) {
            foreach (var arg in progArgs)
                if (arg == "-boot") boot = true;

            SystemEvents.SessionEnding += (sender, args) => {
                args.Cancel = true;
                Shutdown();
            };

            strip = new YeelightManager() { TurnOnWhenConnected = true };
            strip.OnConnecting += (s, ea) => tbIp.ForeColor = Color.Blue;
            strip.OnConnect += (s, ea) => tbIp.ForeColor = Color.DarkGreen;
            strip.OnConnectFail += (s, ea) => tbIp.ForeColor = Color.OrangeRed;
            strip.OnConnectionLost += (s, ea) => tbIp.ForeColor = Color.DarkMagenta;

            audioProvider = new LoopbackAudio();
            audioToColorConverter = new HistoriedAudioToColorConverter();
            audioProvider.OnAudioData += audioToColorConverter.NewSpectrum;
            audioProvider.OnAudioData += AudioProvider_OnAudioData;
            audioToColorConverter.OnColorAvailable += AudioToColorConverter_OnColorAvailable;
            InitializeComponent();

            audioResponsiveElements = new List<IColorizable>()
            {
                rgbView,
                strip
            };

            cbStartInvisible.Checked = Properties.Settings.Default.StartInvisible;
            rgbView.ValueChanged += RgbView_ValueChanged;
            tLimit.Value = ChannelLimit;
            Task.Run(() => {
                SafeCall(async () => {
                    tbIp.Text = Properties.Settings.Default.Ip;
                    await Connect();
                    cbMode_SelectedIndexChanged(null, null);
                });
            });
        }

        private void AudioToColorConverter_OnColorAvailable(object sender, Color color) =>
            audioResponsiveElements.ForEach(audioResponsiveElement => audioResponsiveElement.SetColor(color));

        private void AudioProvider_OnAudioData(object sender, float[] spectrum)
        {
            if (doGraphUpdate)
                SafeCall(() =>
                {
                    chart?.Series["Amplitudes"].Points.Clear();
                    foreach (var sample in spectrum)
                        chart?.Series["Amplitudes"].Points.AddY(sample);
                });
        }

        private void RgbView_ValueChanged(object sender, Color color) =>
            audioResponsiveElements
                .Where(audioResponsiveElement => audioResponsiveElement != sender)
                .ToList()
                .ForEach(audioResponsiveElement => audioResponsiveElement.SetColor(color));

        private void Calculate()
        {
            switch (Mode)
            {
                case 0:
                    break;
                case 1:
                    var rainbow = new HSLColor(hue++, 255.0, 128.0);
                    audioResponsiveElements.ForEach(audioResponsiveElement => audioResponsiveElement.SetColor(rainbow));
                    if (hue >= 256)
                        hue = 0;
                    break;
                case 2:
                    break;
            }
        }

        private void updater_Tick(object sender, EventArgs e) =>
            Calculate();

        private void SafeCall(Action action) {
            if (this.InvokeRequired)
                try {
                    this.Invoke(action);
                } catch (ObjectDisposedException) { } else action.Invoke();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (!forceClose) {
                e.Cancel = true;
                this.Visible = false;
            }
        }

        private void Shutdown() {
            audioProvider.Dispose();
            foreach(var audioResponsiveElement in audioResponsiveElements)
            {
                audioResponsiveElement.SetColor(Color.Black);
                if (audioResponsiveElement is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SafeCall(() =>
            {
                if (cbMode.SelectedIndex == -1)
                    cbMode.SelectedIndex = Properties.Settings.Default.Mode;
                else
                {
                    Properties.Settings.Default.Mode = cbMode.SelectedIndex;
                    Properties.Settings.Default.Save();
                    audioProvider.Stop();
                    switch (cbMode.SelectedIndex)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            audioProvider.Start();
                            break;
                    }
                }
                Mode = cbMode.SelectedIndex;
            });
        }

        private void chart_Click(object sender, EventArgs e) =>
            doGraphUpdate = !doGraphUpdate;

        private void tbIp_TextChanged(object sender, EventArgs e) {
            Properties.Settings.Default.Ip = tbIp.Text;
            Properties.Settings.Default.Save();
        }

        private void tbIp_DoubleClick(object sender, EventArgs e) =>
            _ = Connect();

        private void MainForm_Load(object sender, EventArgs e) {
            if (Properties.Settings.Default.StartInvisible && boot)
                BeginInvoke(new MethodInvoker(() => Hide()));
            cbLaunchOnStartup.Checked = Properties.Settings.Default.StartWithWindows;
            Icon = Properties.Resources.icon_ico;
            trayIcon.Icon = Icon;
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e) =>
            Task.Delay(250).ContinueWith(_ => SafeCall(() => this.Visible = true));

        private void trayStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            if (e.ClickedItem == buttonExit) {
                forceClose = true;
                this.Close();
            } else if (e.ClickedItem == buttonShow)
                this.Visible = true;
            else if (e.ClickedItem == buttonRestart) {
                forceClose = true;
                Application.Restart();
            }
        }

        private void cbStartInvisible_CheckedChanged(object sender, EventArgs e) {
            Properties.Settings.Default.StartInvisible = cbStartInvisible.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbLaunchOnStartup_CheckedChanged(object sender, EventArgs e) {
            var keys = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (cbLaunchOnStartup.Checked)
                keys.SetValue(Text, $"\"{Application.ExecutablePath}\" -boot");
            else keys.SetValue(Text, false);
            Properties.Settings.Default.StartWithWindows = cbLaunchOnStartup.Checked;
            Properties.Settings.Default.Save();
            cbStartInvisible.Enabled = cbLaunchOnStartup.Checked;
        }

        private void tLimit_Scroll(object sender, EventArgs e) =>
            ChannelLimit = Convert.ToByte(tLimit.Value);

        public async Task Connect() {
            if (IPAddress.TryParse(tbIp.Text, out IPAddress ipAddress))
            {
                strip.IPAddress = ipAddress;
                await strip.Connect();
            }
            else
                tbIp.ForeColor = Color.OrangeRed;
        }

        private void TbIp_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
                _ = Connect();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) =>
            Shutdown();
    }
}