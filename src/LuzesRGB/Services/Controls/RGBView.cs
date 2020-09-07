using LuzesRGB.Services;
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

namespace LuzesRGB.Services.Controls
{
    public partial class RGBView : Control, IColorizable
    {
        private int columnHeight;
        private ColorSelected colorSelected;
        private Color _value;

        public Color Color { get => GetColor().Result; set => NewColor(value, false); }

        public event EventHandler<Color> OnColorChanged;
        
        public event EventHandler<Color> OnColorChangedByUser;

        public RGBView()
        {
            InitializeComponent();
            NewColor(Color.OrangeRed, false);
            colorSelected = ColorSelected.None;
        }

        public Task SetColor(Color color)
        {
            NewColor(color, false);
            return Task.CompletedTask;
        }

        private void NewColor(Color color, bool user = true)
        {
            _value = color;
            OnColorChanged?.Invoke(this, color);
            if (user)
                OnColorChangedByUser?.Invoke(this, color);
            Invalidate();
        }

        public Task<Color> GetColor() =>
            Task.FromResult(_value);

        protected override void OnPaint(PaintEventArgs pe)
        {
            var gfx = pe.Graphics;
            gfx.Clear(_value);
            gfx.FillRectangle(new SolidBrush(Color.Red), new RectangleF(0.0F, 0.0F, this.Width * (_value.R / 255.0F), columnHeight));
            gfx.FillRectangle(new SolidBrush(Color.Green), new RectangleF(0.0F, columnHeight, this.Width * (_value.G / 255.0F), columnHeight));
            gfx.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(0.0F, columnHeight * 2, this.Width * (_value.B / 255.0F), columnHeight));
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Y <= columnHeight)
                colorSelected = ColorSelected.Red;
            else if (e.Y <= columnHeight * 2)
                colorSelected = ColorSelected.Green;
            else if (e.Y <= columnHeight * 3)
                colorSelected = ColorSelected.Blue;
            OnMouseMove(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int ColorFromMousePos()
            {
                return Math.Max(0, Math.Min(255, e.X * 255 / this.Width));
            }
            switch (colorSelected)
            {
                case ColorSelected.None:
                    return;
                case ColorSelected.Red:
                    NewColor(Color.FromArgb(ColorFromMousePos(), _value.G, _value.B));
                    break;
                case ColorSelected.Green:
                    NewColor(Color.FromArgb(_value.R, ColorFromMousePos(), _value.B));
                    break;
                case ColorSelected.Blue:
                    NewColor(Color.FromArgb(_value.R, _value.G, ColorFromMousePos()));
                    break;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            colorSelected = ColorSelected.None;
            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int Calculate(byte colorChannel, int delta)
            {
                var value = colorChannel + delta / 30;
                if (value < 0) return 0;
                if (value > 255) return 255;
                return value;
            }
            if (e.Y <= columnHeight)
                NewColor(Color.FromArgb(Calculate(_value.R, e.Delta), _value.G, _value.B));
            else if (e.Y <= columnHeight * 2)
                NewColor(Color.FromArgb(_value.R, Calculate(_value.G, e.Delta), _value.B));
            else if (e.Y <= columnHeight * 3)
                NewColor(Color.FromArgb(_value.R, _value.G, Calculate(_value.B, e.Delta)));
            base.OnMouseWheel(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            columnHeight = this.Height / 4;
        }
    }

    enum ColorSelected
    {
        Red,
        Green,
        Blue,
        None
    }
}