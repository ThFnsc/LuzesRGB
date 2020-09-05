using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuzesRGB {
    public partial class RGBView : Control, IColorizable {
        private int columnHeight;
        private ColorSelected colorSelected;
        private Color _value;

        public Color Value { get => GetColor().Result; set => SetColor(value).Wait(); }

        public Task SetColor(Color color)
        {
            _value = color;
            if (ValueChanged != null) 
                ValueChanged.Invoke(this, _value);
            Invalidate();
            return Task.CompletedTask;
        }

        public Task<Color> GetColor() =>
            Task.FromResult(_value);

        public bool TurnOnWhenConnected { get; set; }
        public IPAddress IPAddress { get; set; }

        public bool Connected => true;

        public event EventHandler<Color> ValueChanged;
        public event EventHandler OnConnecting;
        public event EventHandler OnConnect;
        public event EventHandler OnConnectionLost;
        public event EventHandler OnConnectFail;

        public RGBView() {
            InitializeComponent();
            SetColor(Color.OrangeRed).Wait();
            colorSelected = ColorSelected.None;
        }

        protected override void OnPaint(PaintEventArgs pe) {
            var gfx = pe.Graphics;
            gfx.Clear(_value);
            gfx.FillRectangle(new SolidBrush(Color.Red), new RectangleF(0.0F, 0.0F, this.Width * (_value.R/255.0F), columnHeight));
            gfx.FillRectangle(new SolidBrush(Color.Green), new RectangleF(0.0F, columnHeight, this.Width * (_value.G / 255.0F), columnHeight));
            gfx.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(0.0F, columnHeight * 2, this.Width * (_value.B / 255.0F), columnHeight));
        }

        protected override void OnPaintBackground(PaintEventArgs pevent) {

        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Y <= columnHeight)
                colorSelected = ColorSelected.Red;
            else if (e.Y <= columnHeight * 2)
                colorSelected = ColorSelected.Green;
            else if (e.Y <= columnHeight * 3)
                colorSelected = ColorSelected.Blue;
            OnMouseMove(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            int ColorFromMousePos() {
                return Math.Max(0, Math.Min(255, e.X * 255 / this.Width));
            }
            switch (colorSelected) {
                case ColorSelected.None:
                    return;
                case ColorSelected.Red:
                    SetColor(Color.FromArgb(ColorFromMousePos(), _value.G, _value.B)).Wait();
                    break;
                case ColorSelected.Green:
                    SetColor(Color.FromArgb(_value.R, ColorFromMousePos(), _value.B)).Wait();
                    break;
                case ColorSelected.Blue:
                    SetColor(Color.FromArgb(_value.R, _value.G, ColorFromMousePos())).Wait();
                    break;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            colorSelected = ColorSelected.None;
            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            int Calculate(byte colorChannel, int delta) {
                var value = colorChannel + delta/30;
                if (value < 0) return 0;
                if (value > 255) return 255;
                return value;
            }
            if (e.Y <= columnHeight)
                SetColor(Color.FromArgb(Calculate(_value.R, e.Delta), _value.G, _value.B)).Wait();
            else if (e.Y <= columnHeight * 2)
                SetColor(Color.FromArgb(_value.R, Calculate(_value.G,e.Delta), _value.B)).Wait(); 
            else if (e.Y <= columnHeight * 3)
                SetColor(Color.FromArgb(_value.R, _value.G, Calculate(_value.B, e.Delta))).Wait();
            base.OnMouseWheel(e);
        }

        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            columnHeight = this.Height / 4;
        }

        public Task Connect() =>
            Task.CompletedTask;

        public Task Turn(bool state) =>
            Task.CompletedTask;

        public Task Disconnect() =>
            Task.CompletedTask;
    }

    enum ColorSelected {
        Red,
        Green,
        Blue,
        None
    }
}