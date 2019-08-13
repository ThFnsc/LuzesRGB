using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.Numerics;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Threading;

namespace LuzesRGB {
    public partial class MainForm : Form {
        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);
        public byte ChannelLimit { get { return Properties.Settings.Default.ChannelLimit; } set { Properties.Settings.Default.ChannelLimit = value; Properties.Settings.Default.Save(); }}
        private bool forceClose = false;
        private bool boot = false;

        MagicHome strip;
        WasapiLoopbackCapture audio;
        BufferedWaveProvider bwp;
        Color lastColorSent;
        int lastColorCount = 0;

        bool doGraphUpdate = true;

        float[,] history = new float[3, 750];
        short histPos = 0;

        double hue = 0;
        float MIN_THRESHOLD = 0.04f;

        int BUFFER_SIZE = (int)Math.Pow(2, 13);

        public MainForm(string[] progArgs) {
            foreach (var arg in progArgs)
                if (arg == "-boot") boot = true;
            SystemEvents.SessionEnding += (sender, args) => {
                args.Cancel = true;
                Shutdown();
            };
            InitializeComponent();
            cbStartInvisible.Checked = Properties.Settings.Default.StartInvisible;
            rgbView.ValueChanged += RgbView_ValueChanged;
            tLimit.Value = ChannelLimit;
            Task.Run(() => {
                audio = new WasapiLoopbackCapture();
                bwp = new BufferedWaveProvider(audio.WaveFormat) { DiscardOnBufferOverflow = true, BufferLength = BUFFER_SIZE * 2 };
                audio.DataAvailable += (sndr, args) => bwp.AddSamples(args.Buffer, 0, args.BytesRecorded);
                strip = new MagicHome() { TurnOnWhenConnected = true };
                strip.OnConnecting += (s, ea) => tbIp.ForeColor = Color.Blue;
                strip.OnConnect += (s, ea) => tbIp.ForeColor = Color.DarkGreen;
                strip.OnConnectFail += (s, ea) => tbIp.ForeColor = Color.OrangeRed;
                strip.OnConnectionLost += (s, ea) => tbIp.ForeColor = Color.DarkMagenta;
                SafeCall(() => {
                    tbIp.Text = Properties.Settings.Default.Ip;
                    Connect();
                    cbMode_SelectedIndexChanged(null, null);
                    updater.Enabled = true;
                });
            });
        }

        private void RgbView_ValueChanged(object sender, Color e) {
            SetRGBStrip(rgbView.Value, true);
        }

        private void SetRGBStrip(Color color, bool callback = false) {
            lastColorCount = color == lastColorSent ? lastColorCount + 1 : 0;
            if (lastColorCount < 5)
                strip.SetColor(lastColorSent = color);
            if (!callback && this.Visible && this.WindowState != FormWindowState.Minimized)
                rgbView.Value = color;
        }

        private void updater_Tick(object sender, EventArgs e) {
            switch (cbMode.SelectedIndex) {
                case 0:
                    updater.Enabled = false;
                    break;
                case 1:
                    SetRGBStrip(new HSLColor(hue++, 255.0, 128.0));
                    if (hue >= 256) hue = 0;
                    break;
                case 2:
                    if (bwp.BufferedBytes < BUFFER_SIZE) return;

                    updater.Enabled = false;
                    byte[] buffer = new byte[bwp.BufferLength];

                    bwp.Read(buffer, 0, bwp.BufferLength);
                    float[] leftSamples = new float[bwp.BufferLength / 8];
                    float[] rightSamples = new float[bwp.BufferLength / 8];
                    float[] joinedSamples = new float[bwp.BufferLength / 8];

                    for (int i = 0; i < bwp.BufferLength; i += 8) {
                        leftSamples[i / 8] = BitConverter.ToSingle(buffer, i);
                        rightSamples[i / 8] = BitConverter.ToSingle(buffer, i + 4);
                        joinedSamples[i / 8] = (leftSamples[i / 8] + rightSamples[i / 8]) / 2;
                    }

                    joinedSamples = FFT(joinedSamples);
                    SafeCall(() => {
                        if (doGraphUpdate) {
                            chart.Series["Amplitudes"].Points.Clear();
                            for (int i = 0; i < joinedSamples.Length; i++)
                                chart.Series["Amplitudes"].Points.AddXY(i, Math.Abs(joinedSamples[i]));
                        }

                        float[] freqs = LowsMidsHighs(joinedSamples);
                        for (int i = 0; i < freqs.Length; i++)
                            history[i, histPos++] = freqs[i];
                        if (histPos >= history.GetLength(1)) histPos = 0;
                        SetRGBStrip(Color.FromArgb(
                            MapN(freqs[0], 0, MaxOf(history, 0), 0, ChannelLimit),
                        MapN(freqs[1], 0, MaxOf(history, 1), 0, ChannelLimit),
                        MapN(freqs[2], 0, MaxOf(history, 2), 0, ChannelLimit)));
                    });
                    updater.Enabled = true;
                    break;
            }
        }

        private float MaxOf(float[,] matrix, int index) {
            float max = 0;
            for (int i = 0; i < matrix.GetLength(1); i++)
                if (matrix[index, i] > max) max = matrix[index, i];
            return max;
        }

        private int MapN(float x, float minX, float maxX, float minY, float maxY) {
            try {
                return Convert.ToInt32(Math.Max(MIN_THRESHOLD, (x - minX) / (maxX - minX) * (maxY - minY) + minY));
            } catch (Exception) { return 0; }
        }

        private float[] LowsMidsHighs(float[] samples) {
            float[] freqs = new float[3];
            for (int i = 1; i < samples.Length / 2; i++) {
                if (i < 10) {
                    if (samples[i] > freqs[0]) freqs[0] = samples[i];
                } else if (i < 200) {
                    if (samples[i] > freqs[1]) freqs[1] = samples[i];
                } else {
                    if (samples[i] > freqs[2]) freqs[2] = samples[i];
                }
            }
            return freqs;
        }

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
            if (strip != null && strip.IsConnected)
                for (byte i = 0; i < 3; Thread.Sleep(100), i++)
                    strip?.Turn(false);
            if (audio.CaptureState == NAudio.CoreAudioApi.CaptureState.Capturing)
                audio.StopRecording();
            strip?.Dispose();
        }

        public float[] FFT(float[] data) {
            float[] fft = new float[data.Length];
            Complex[] fftComplex = new Complex[data.Length];
            for (int i = 0; i < data.Length; i++)
                fftComplex[i] = new Complex(data[i], 0.0);
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < data.Length; i++)
                fft[i] = (float)fftComplex[i].Magnitude;
            return fft;
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbMode.SelectedIndex == -1) cbMode.SelectedIndex = Properties.Settings.Default.Mode;
            else {
                Properties.Settings.Default.Mode = cbMode.SelectedIndex;
                Properties.Settings.Default.Save();
                audio.StopRecording();
                switch (cbMode.SelectedIndex) {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        audio.StartRecording();
                        break;
                }
                updater.Enabled = true;
            }
        }

        private void chart_Click(object sender, EventArgs e) {
            doGraphUpdate = !doGraphUpdate;
        }

        private void tbIp_TextChanged(object sender, EventArgs e) {
            Properties.Settings.Default.Ip = tbIp.Text;
            Properties.Settings.Default.Save();
        }

        private void tbIp_DoubleClick(object sender, EventArgs e) {
            Connect();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            if (Properties.Settings.Default.StartInvisible && boot)
                BeginInvoke(new MethodInvoker(() => Hide()));
            cbLaunchOnStartup.Checked = Properties.Settings.Default.StartWithWindows;
            Icon = Properties.Resources.icon_ico;
            trayIcon.Icon = Icon;
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e) {
            Task.Run(() => {
                Thread.Sleep(250);
                SafeCall(() => this.Visible = true);
            });
        }

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

        private void tLimit_Scroll(object sender, EventArgs e) {
            ChannelLimit = Convert.ToByte(tLimit.Value);
        }

        public void Connect() {
            if (IPAddress.TryParse(tbIp.Text, out strip.Ip))
                strip.Connect();
            else
                tbIp.ForeColor = Color.OrangeRed;
        }

        private void TbIp_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
                Connect();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            Shutdown();
        }
    }
}