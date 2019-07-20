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
            this.updater = new System.Windows.Forms.Timer(this.components);
            this.waveViewer1 = new NAudio.Gui.WaveViewer();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbIp = new System.Windows.Forms.TextBox();
            this.cbStartInvisible = new System.Windows.Forms.CheckBox();
            this.cbLaunchOnStartup = new System.Windows.Forms.CheckBox();
            this.tLimit = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.buttonRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonShow = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonExit = new System.Windows.Forms.ToolStripMenuItem();
            this.rgbView = new LuzesRGB.RGBView();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tLimit)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.trayStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // updater
            // 
            this.updater.Interval = 15;
            this.updater.Tick += new System.EventHandler(this.updater_Tick);
            // 
            // waveViewer1
            // 
            this.waveViewer1.Location = new System.Drawing.Point(477, 124);
            this.waveViewer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.waveViewer1.Name = "waveViewer1";
            this.waveViewer1.SamplesPerPixel = 128;
            this.waveViewer1.Size = new System.Drawing.Size(629, 355);
            this.waveViewer1.StartPosition = ((long)(0));
            this.waveViewer1.TabIndex = 2;
            this.waveViewer1.WaveStream = null;
            // 
            // chart
            // 
            chartArea1.AxisY.Maximum = 0.02D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(4, 113);
            this.chart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Amplitudes";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(802, 339);
            this.chart.TabIndex = 3;
            this.chart.Text = "chart1";
            this.chart.Click += new System.EventHandler(this.chart_Click);
            // 
            // cbMode
            // 
            this.cbMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "Fixed",
            "Rainbow",
            "Music"});
            this.cbMode.Location = new System.Drawing.Point(55, 4);
            this.cbMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(334, 24);
            this.cbMode.TabIndex = 5;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbMode, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbIp, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbStartInvisible, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.cbLaunchOnStartup, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.tLimit, 1, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(393, 174);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(4, 32);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 28);
            this.label5.TabIndex = 7;
            this.label5.Text = "Ip";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(4, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 32);
            this.label4.TabIndex = 6;
            this.label4.Text = "Mode";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbIp
            // 
            this.tbIp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIp.Location = new System.Drawing.Point(54, 35);
            this.tbIp.Name = "tbIp";
            this.tbIp.Size = new System.Drawing.Size(336, 22);
            this.tbIp.TabIndex = 8;
            this.tbIp.TextChanged += new System.EventHandler(this.tbIp_TextChanged);
            this.tbIp.DoubleClick += new System.EventHandler(this.tbIp_DoubleClick);
            // 
            // cbStartInvisible
            // 
            this.cbStartInvisible.AutoSize = true;
            this.cbStartInvisible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbStartInvisible.Location = new System.Drawing.Point(78, 93);
            this.cbStartInvisible.Margin = new System.Windows.Forms.Padding(27, 4, 4, 4);
            this.cbStartInvisible.Name = "cbStartInvisible";
            this.cbStartInvisible.Size = new System.Drawing.Size(311, 21);
            this.cbStartInvisible.TabIndex = 9;
            this.cbStartInvisible.Text = "Start invisible";
            this.cbStartInvisible.UseVisualStyleBackColor = true;
            this.cbStartInvisible.CheckedChanged += new System.EventHandler(this.cbStartInvisible_CheckedChanged);
            // 
            // cbLaunchOnStartup
            // 
            this.cbLaunchOnStartup.AutoSize = true;
            this.cbLaunchOnStartup.Location = new System.Drawing.Point(55, 64);
            this.cbLaunchOnStartup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLaunchOnStartup.Name = "cbLaunchOnStartup";
            this.cbLaunchOnStartup.Size = new System.Drawing.Size(145, 21);
            this.cbLaunchOnStartup.TabIndex = 10;
            this.cbLaunchOnStartup.Text = "Launch on startup";
            this.cbLaunchOnStartup.UseVisualStyleBackColor = true;
            this.cbLaunchOnStartup.CheckedChanged += new System.EventHandler(this.cbLaunchOnStartup_CheckedChanged);
            // 
            // tLimit
            // 
            this.tLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLimit.Location = new System.Drawing.Point(52, 119);
            this.tLimit.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tLimit.Maximum = 255;
            this.tLimit.Name = "tLimit";
            this.tLimit.Size = new System.Drawing.Size(340, 54);
            this.tLimit.TabIndex = 11;
            this.tLimit.Scroll += new System.EventHandler(this.tLimit_Scroll);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.chart, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(810, 456);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.rgbView, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(802, 101);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayStrip;
            this.trayIcon.Text = "LuzesRGB";
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
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
            this.trayStrip.Size = new System.Drawing.Size(100, 76);
            this.trayStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.trayStrip_ItemClicked);
            // 
            // buttonRestart
            // 
            this.buttonRestart.Name = "buttonRestart";
            this.buttonRestart.Size = new System.Drawing.Size(99, 24);
            this.buttonRestart.Text = "Restart";
            // 
            // buttonShow
            // 
            this.buttonShow.Name = "buttonShow";
            this.buttonShow.Size = new System.Drawing.Size(99, 24);
            this.buttonShow.Text = "Show";
            // 
            // buttonExit
            // 
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(99, 24);
            this.buttonExit.Text = "Exit";
            // 
            // rgbView
            // 
            this.rgbView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgbView.Location = new System.Drawing.Point(405, 4);
            this.rgbView.Margin = new System.Windows.Forms.Padding(4);
            this.rgbView.Name = "rgbView";
            this.rgbView.Size = new System.Drawing.Size(393, 174);
            this.rgbView.TabIndex = 7;
            this.rgbView.Text = "rgbView1";
            this.rgbView.Value = System.Drawing.Color.Empty;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 456);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.waveViewer1);
            this.Name = "MainForm";
            this.Text = "LuzesRGB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tLimit)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.trayStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer updater;
        private NAudio.Gui.WaveViewer waveViewer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbIp;
        private RGBView rgbView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayStrip;
        private System.Windows.Forms.ToolStripMenuItem buttonExit;
        private System.Windows.Forms.ToolStripMenuItem buttonShow;
        private System.Windows.Forms.CheckBox cbStartInvisible;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox cbLaunchOnStartup;
        private System.Windows.Forms.ToolStripMenuItem buttonRestart;
        private System.Windows.Forms.TrackBar tLimit;
    }
}

