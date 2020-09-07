using LuzesRGB.Services.Controls;
using System;

namespace LuzesRGB {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.waveViewer1 = new NAudio.Gui.WaveViewer();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbStartInvisible = new System.Windows.Forms.CheckBox();
            this.cbLaunchOnStartup = new System.Windows.Forms.CheckBox();
            this.tLimit = new System.Windows.Forms.TrackBar();
            this.lbLights = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNewLight = new System.Windows.Forms.Button();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.buttonRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonShow = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonExit = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.rgbView = new LuzesRGB.Services.Controls.RGBView();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tLimit)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.trayStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // waveViewer1
            // 
            this.waveViewer1.Location = new System.Drawing.Point(358, 101);
            this.waveViewer1.Name = "waveViewer1";
            this.waveViewer1.SamplesPerPixel = 128;
            this.waveViewer1.Size = new System.Drawing.Size(472, 288);
            this.waveViewer1.StartPosition = ((long)(0));
            this.waveViewer1.TabIndex = 2;
            this.waveViewer1.WaveStream = null;
            // 
            // chart
            // 
            chartArea1.BorderWidth = 2;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Amplitudes";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(891, 277);
            this.chart.TabIndex = 3;
            this.chart.Text = "chart1";
            this.chart.Click += new System.EventHandler(this.ChartClicked);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.cbStartInvisible, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.cbLaunchOnStartup, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tLimit, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.lbLights, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(613, 225);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // cbStartInvisible
            // 
            this.cbStartInvisible.AutoSize = true;
            this.cbStartInvisible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbStartInvisible.Location = new System.Drawing.Point(20, 163);
            this.cbStartInvisible.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.cbStartInvisible.Name = "cbStartInvisible";
            this.cbStartInvisible.Size = new System.Drawing.Size(590, 17);
            this.cbStartInvisible.TabIndex = 9;
            this.cbStartInvisible.Text = "Start invisible";
            this.cbStartInvisible.UseVisualStyleBackColor = true;
            this.cbStartInvisible.CheckedChanged += new System.EventHandler(this.StartInvisibleCheckedChanged);
            // 
            // cbLaunchOnStartup
            // 
            this.cbLaunchOnStartup.AutoSize = true;
            this.cbLaunchOnStartup.Location = new System.Drawing.Point(3, 140);
            this.cbLaunchOnStartup.Name = "cbLaunchOnStartup";
            this.cbLaunchOnStartup.Size = new System.Drawing.Size(112, 17);
            this.cbLaunchOnStartup.TabIndex = 10;
            this.cbLaunchOnStartup.Text = "Launch on startup";
            this.cbLaunchOnStartup.UseVisualStyleBackColor = true;
            this.cbLaunchOnStartup.CheckedChanged += new System.EventHandler(this.LaunchOnStartupCheckedChanged);
            // 
            // tLimit
            // 
            this.tLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLimit.Location = new System.Drawing.Point(1, 184);
            this.tLimit.Margin = new System.Windows.Forms.Padding(1);
            this.tLimit.Maximum = 255;
            this.tLimit.Name = "tLimit";
            this.tLimit.Size = new System.Drawing.Size(611, 40);
            this.tLimit.TabIndex = 11;
            this.tLimit.Scroll += new System.EventHandler(this.BrightnessLimitScrolled);
            // 
            // lbLights
            // 
            this.lbLights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLights.FormattingEnabled = true;
            this.lbLights.Location = new System.Drawing.Point(3, 3);
            this.lbLights.Name = "lbLights";
            this.lbLights.Size = new System.Drawing.Size(607, 96);
            this.lbLights.TabIndex = 12;
            this.lbLights.DoubleClick += new System.EventHandler(this.LightListDoubleClicked);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnNewLight);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 105);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(607, 29);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // btnNewLight
            // 
            this.btnNewLight.AutoSize = true;
            this.btnNewLight.Location = new System.Drawing.Point(518, 3);
            this.btnNewLight.Name = "btnNewLight";
            this.btnNewLight.Size = new System.Drawing.Size(86, 23);
            this.btnNewLight.TabIndex = 0;
            this.btnNewLight.Text = "Nova lâmpada";
            this.btnNewLight.UseVisualStyleBackColor = true;
            this.btnNewLight.Click += new System.EventHandler(this.NewLight);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayStrip;
            this.trayIcon.Text = "LuzesRGB";
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += new System.EventHandler(this.TrayIconDoubleClicked);
            // 
            // trayStrip
            // 
            this.trayStrip.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.trayStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonRestart,
            this.buttonShow,
            this.buttonExit});
            this.trayStrip.Name = "trayStrip";
            this.trayStrip.ShowImageMargin = false;
            this.trayStrip.Size = new System.Drawing.Size(86, 70);
            this.trayStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.TrayStripItemClicked);
            // 
            // buttonRestart
            // 
            this.buttonRestart.Name = "buttonRestart";
            this.buttonRestart.Size = new System.Drawing.Size(85, 22);
            this.buttonRestart.Text = "Restart";
            // 
            // buttonShow
            // 
            this.buttonShow.Name = "buttonShow";
            this.buttonShow.Size = new System.Drawing.Size(85, 22);
            this.buttonShow.Text = "Show";
            // 
            // buttonExit
            // 
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(85, 22);
            this.buttonExit.Text = "Exit";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chart);
            this.splitContainer1.Size = new System.Drawing.Size(891, 506);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 8;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.rgbView);
            this.splitContainer2.Size = new System.Drawing.Size(891, 225);
            this.splitContainer2.SplitterDistance = 613;
            this.splitContainer2.TabIndex = 0;
            // 
            // rgbView
            // 
            this.rgbView.Color = System.Drawing.Color.Empty;
            this.rgbView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgbView.Location = new System.Drawing.Point(0, 0);
            this.rgbView.Name = "rgbView";
            this.rgbView.Size = new System.Drawing.Size(274, 225);
            this.rgbView.TabIndex = 7;
            this.rgbView.Text = "rgbView1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 506);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.waveViewer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "LuzesRGB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tLimit)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.trayStrip.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private NAudio.Gui.WaveViewer waveViewer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private RGBView rgbView;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayStrip;
        private System.Windows.Forms.ToolStripMenuItem buttonExit;
        private System.Windows.Forms.ToolStripMenuItem buttonShow;
        private System.Windows.Forms.CheckBox cbStartInvisible;
        private System.Windows.Forms.CheckBox cbLaunchOnStartup;
        private System.Windows.Forms.ToolStripMenuItem buttonRestart;
        private System.Windows.Forms.TrackBar tLimit;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox lbLights;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnNewLight;
    }
}

