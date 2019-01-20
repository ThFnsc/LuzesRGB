﻿using System;

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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rgbView = new LuzesRGB.RGBView();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.buttonExit = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonShow = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRestart = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
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
            chartArea2.AxisY.Maximum = 0.02D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea2);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart.Legends.Add(legend2);
            this.chart.Location = new System.Drawing.Point(3, 156);
            this.chart.Name = "chart";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Amplitudes";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(719, 482);
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
            this.cbMode.Location = new System.Drawing.Point(43, 3);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(307, 21);
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
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(353, 141);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 24);
            this.label5.TabIndex = 7;
            this.label5.Text = "Ip";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 27);
            this.label4.TabIndex = 6;
            this.label4.Text = "Mode";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbIp
            // 
            this.tbIp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbIp.Location = new System.Drawing.Point(42, 29);
            this.tbIp.Margin = new System.Windows.Forms.Padding(2);
            this.tbIp.Name = "tbIp";
            this.tbIp.Size = new System.Drawing.Size(309, 20);
            this.tbIp.TabIndex = 8;
            this.tbIp.TextChanged += new System.EventHandler(this.tbIp_TextChanged);
            this.tbIp.DoubleClick += new System.EventHandler(this.tbIp_DoubleClick);
            // 
            // cbStartInvisible
            // 
            this.cbStartInvisible.AutoSize = true;
            this.cbStartInvisible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbStartInvisible.Location = new System.Drawing.Point(60, 77);
            this.cbStartInvisible.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.cbStartInvisible.Name = "cbStartInvisible";
            this.cbStartInvisible.Size = new System.Drawing.Size(290, 17);
            this.cbStartInvisible.TabIndex = 9;
            this.cbStartInvisible.Text = "Start invisible";
            this.cbStartInvisible.UseVisualStyleBackColor = true;
            this.cbStartInvisible.CheckedChanged += new System.EventHandler(this.cbStartInvisible_CheckedChanged);
            // 
            // cbLaunchOnStartup
            // 
            this.cbLaunchOnStartup.AutoSize = true;
            this.cbLaunchOnStartup.Location = new System.Drawing.Point(43, 54);
            this.cbLaunchOnStartup.Name = "cbLaunchOnStartup";
            this.cbLaunchOnStartup.Size = new System.Drawing.Size(112, 17);
            this.cbLaunchOnStartup.TabIndex = 10;
            this.cbLaunchOnStartup.Text = "Launch on startup";
            this.cbLaunchOnStartup.UseVisualStyleBackColor = true;
            this.cbLaunchOnStartup.CheckedChanged += new System.EventHandler(this.cbLaunchOnStartup_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.chart, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.02496F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.97504F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(725, 641);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.rgbView, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(719, 147);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // rgbView
            // 
            this.rgbView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgbView.Location = new System.Drawing.Point(362, 3);
            this.rgbView.Name = "rgbView";
            this.rgbView.Size = new System.Drawing.Size(354, 141);
            this.rgbView.TabIndex = 7;
            this.rgbView.Text = "rgbView1";
            this.rgbView.Value = System.Drawing.Color.Empty;
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
            this.trayStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonRestart,
            this.buttonShow,
            this.buttonExit});
            this.trayStrip.Name = "trayStrip";
            this.trayStrip.ShowImageMargin = false;
            this.trayStrip.Size = new System.Drawing.Size(86, 70);
            this.trayStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.trayStrip_ItemClicked);
            // 
            // buttonExit
            // 
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(155, 22);
            this.buttonExit.Text = "Exit";
            // 
            // buttonShow
            // 
            this.buttonShow.Name = "buttonShow";
            this.buttonShow.Size = new System.Drawing.Size(155, 22);
            this.buttonShow.Text = "Show";
            // 
            // buttonRestart
            // 
            this.buttonRestart.Name = "buttonRestart";
            this.buttonRestart.Size = new System.Drawing.Size(155, 22);
            this.buttonRestart.Text = "Restart";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 641);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.waveViewer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "LuzesRGB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.trayStrip.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}
