using LuzesRGB.Helpers;
using LuzesRGB.Services;
using LuzesRGB.Services.Controls;
using LuzesRGB.Services.Lights;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuzesRGB
{
    public partial class MainForm : Form
    {
        public byte ChannelLimit { get => Properties.Settings.Default.ChannelLimit; set { Properties.Settings.Default.ChannelLimit = value; Properties.Settings.Default.Save(); } }
        private bool _forceClose = false;
        private readonly bool _boot = false;
        private bool _doGraphUpdate = true;
        private readonly AudioToColorService _audioToColorService;

        public MainForm(string[] progArgs)
        {
            foreach (var arg in progArgs)
                if (arg == "-boot") _boot = true;

            SystemEvents.SessionEnding += (sender, args) =>
            {
                args.Cancel = true;
                Shutdown();
            };

            _audioToColorService = new AudioToColorService(new LoopbackAudio(), new HistoriedAudioToColorConverter());
            _audioToColorService.Start();
            _audioToColorService.OnAudioData += OnAudioData;
            _audioToColorService.OnColorChanged += NewColor;

            InitializeComponent();

            lbLights.Items.AddRange(Properties.Settings.Default.SavedLamps.AsJson<List<SmartLight>>().ToArray());

            _ = UpdateLights();

            cbStartInvisible.Checked = Properties.Settings.Default.StartInvisible;
            rgbView.OnColorChangedByUser += RgbView_ValueChanged;
            tLimit.Value = ChannelLimit;
        }

        private void NewColor(object sender, Color e) =>
            rgbView.Color = e;

        private void OnAudioData(object sender, float[] spectrum)
        {
            if (_doGraphUpdate)
                SafeCall(() =>
                {
                    chart?.Series["Amplitudes"].Points.Clear();
                    foreach (var sample in spectrum)
                        chart?.Series["Amplitudes"].Points.AddY(sample);
                });
        }

        public async Task UpdateLights()
        {
            Properties.Settings.Default.SavedLamps = lbLights.Items.OfType<SmartLight>().ToJson();
            Properties.Settings.Default.Save();
            await _audioToColorService.RemoveAll();
            _audioToColorService.SmartLights.AddRange(lbLights.Items.OfType<SmartLight>().Select(s => s.Instantiate()));
            await _audioToColorService.ConnectAll();
        }

        private void RgbView_ValueChanged(object sender, Color color) =>
            _ = _audioToColorService.SetColor(color);

        private void SafeCall(Action action)
        {
            if (InvokeRequired)
                try
                {
                    Invoke(action);
                }
                catch (ObjectDisposedException) { }
            else action.Invoke();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_forceClose)
            {
                e.Cancel = true;
                Visible = false;
            }
        }

        private void Shutdown() =>
            _audioToColorService.Dispose();

        private void ChartClicked(object sender, EventArgs e) =>
            _doGraphUpdate = !_doGraphUpdate;

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.StartInvisible && _boot)
                BeginInvoke(new MethodInvoker(() => Hide()));
            cbLaunchOnStartup.Checked = WindowsStartup.Get();
            cbStartInvisible.Enabled = cbLaunchOnStartup.Checked;
            Icon = Properties.Resources.icon_ico;
            trayIcon.Icon = Icon;
        }

        private void TrayIconDoubleClicked(object sender, EventArgs e) =>
            Task.Delay(250).ContinueWith(_ => SafeCall(() => Visible = true));

        private void TrayStripItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == buttonExit)
            {
                _forceClose = true;
                Close();
            }
            else if (e.ClickedItem == buttonShow)
                Visible = true;
            else if (e.ClickedItem == buttonRestart)
            {
                _forceClose = true;
                Application.Restart();
            }
        }

        private void StartInvisibleCheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartInvisible = cbStartInvisible.Checked;
            Properties.Settings.Default.Save();
        }

        private void LaunchOnStartupCheckedChanged(object sender, EventArgs e) =>
            WindowsStartup.Set(cbStartInvisible.Enabled = cbLaunchOnStartup.Checked);

        private void BrightnessLimitScrolled(object sender, EventArgs e) =>
            ChannelLimit = _audioToColorService.BrightnessCap = Convert.ToByte(tLimit.Value);

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) =>
            Shutdown();

        private void NewLight(object sender, EventArgs e)
        {
            var newLampForm = new EditLight(null);
            if (newLampForm.ShowDialog() == DialogResult.OK)
            {
                lbLights.Items.Add(newLampForm.SmartLight);
                _ = UpdateLights();
            }
        }

        private void LightListDoubleClicked(object sender, EventArgs e)
        {
            var index = lbLights.IndexFromPoint((e as MouseEventArgs).Location);
            if (index != -1)
            {
                var selected = lbLights.SelectedItem as SmartLight;
                var edit = new EditLight(selected);
                edit.ShowDialog();
                switch (edit.DialogResult)
                {
                    case DialogResult.OK:
                        var lights = lbLights.Items.OfType<SmartLight>().ToArray();
                        lbLights.Items.Clear();
                        lbLights.Items.AddRange(lights);
                        break;
                    case DialogResult.Abort:
                        lbLights.Items.Remove(selected);
                        break;
                }
                _ = UpdateLights();
            }
        }
    }
}