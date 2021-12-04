using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuzesRGB.Services.Controls
{
    public partial class RGBView : Control, IColorizable
    {
        private int _columnHeight;
        private ColorSelected _colorSelected;
        private Color _value;

        public Color Color { get => GetColor().Result; set => NewColor(value, false); }

        public event EventHandler<Color> OnColorChanged;

        public event EventHandler<Color> OnColorChangedByUser;

        public RGBView()
        {
            InitializeComponent();
            NewColor(Color.OrangeRed, false);
            _colorSelected = ColorSelected.None;
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
            gfx.FillRectangle(new SolidBrush(Color.Red), new RectangleF(0.0F, 0.0F, Width * (_value.R / 255.0F), _columnHeight));
            gfx.FillRectangle(new SolidBrush(Color.Green), new RectangleF(0.0F, _columnHeight, Width * (_value.G / 255.0F), _columnHeight));
            gfx.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(0.0F, _columnHeight * 2, Width * (_value.B / 255.0F), _columnHeight));
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Y <= _columnHeight)
                _colorSelected = ColorSelected.Red;
            else if (e.Y <= _columnHeight * 2)
                _colorSelected = ColorSelected.Green;
            else if (e.Y <= _columnHeight * 3)
                _colorSelected = ColorSelected.Blue;
            OnMouseMove(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int ColorFromMousePos() => Math.Max(0, Math.Min(255, e.X * 255 / Width));
            switch (_colorSelected)
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
            _colorSelected = ColorSelected.None;
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
            if (e.Y <= _columnHeight)
                NewColor(Color.FromArgb(Calculate(_value.R, e.Delta), _value.G, _value.B));
            else if (e.Y <= _columnHeight * 2)
                NewColor(Color.FromArgb(_value.R, Calculate(_value.G, e.Delta), _value.B));
            else if (e.Y <= _columnHeight * 3)
                NewColor(Color.FromArgb(_value.R, _value.G, Calculate(_value.B, e.Delta)));
            base.OnMouseWheel(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            _columnHeight = Height / 4;
        }
    }

    internal enum ColorSelected
    {
        Red,
        Green,
        Blue,
        None
    }
}