using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SZI
{
    /// <summary>
    /// Klasa umożliwiająca kontrolę postępu wydarzeń. Używana do aktualizowania postępu wczytywania rekordów z bazy danych.
    /// </summary>
    public class ProgressStatusStrip : StatusStrip
    {
        #region ProgressStatusStrip Definitions
        private Color _barColor = Color.ForestGreen;
        private Color _barShade = Color.LightGreen;

        private float _progressMin = 0.0F;
        private float _progressMax = 100.0F;
        private float _progressVal = 0.0F;
        #endregion

        #region ProgressStatusStrip Properties
        [Description("The color of the Progress Bar"), Category("Progress Bar"), DefaultValue(typeof(Color), "Color.ForestGreen")]
        public Color ProgressColor
        {
            get { return _barColor; }
            set { _barColor = value; this.Invalidate(); }
        }

        [Description("The shade color of the Progress Bar"), Category("Progress Bar"), DefaultValue(typeof(Color), "Color.LightGreen")]
        public Color ProgressShade
        {
            get { return _barShade; }
            set { _barShade = value; this.Invalidate(); }
        }

        [Description("The lower bound of the range the Progress Bar is working with"), Category("Progress Bar"), DefaultValue(0.0F)]
        public float Minimum
        {
            get { return _progressMin; }
            set
            {
                _progressMin = value;
                if (_progressMin > _progressMax) _progressMax = _progressMin;
                if (_progressMin > _progressVal) _progressVal = _progressMin;
                this.Invalidate();
            }
        }

        [Description("The upper bound of the range the Progress Bar is working with"), Category("Progress Bar"), DefaultValue(100.0F)]
        public float Maximum
        {
            get { return _progressMax; }
            set
            {
                _progressMax = value;
                if (_progressMax < _progressMin) _progressMin = _progressMax;
                if (_progressMax < _progressVal) _progressVal = _progressMax;
                this.Invalidate();
            }
        }

        [Description("The current value for the Progress Bar, in the range specified by the minimum and maximum properties"), Category("Progress Bar"), DefaultValue(0.0F)]
        public float Value
        {
            get { return _progressVal; }
            set
            {
                _progressVal = value;
                if (_progressVal < _progressMin) _progressVal = _progressMin;
                if (_progressVal > _progressMax) _progressVal = _progressMax;
                this.Invalidate();
            }
        }
        #endregion

        #region ProgressStatusStrip Methods
        public ProgressStatusStrip() { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            float progPercent = (float)(_progressVal / (_progressMax - _progressMin));
            if (progPercent > 0)
            {
                RectangleF progRectangle = pe.Graphics.VisibleClipBounds;
                progRectangle.Width *= progPercent;
                LinearGradientBrush progBrush = new LinearGradientBrush(progRectangle, _barColor, _barShade, LinearGradientMode.Horizontal);
                pe.Graphics.FillRectangle(progBrush, progRectangle);
                progBrush.Dispose();
            }

            base.OnPaint(pe);
        }
        #endregion
    }
}
