using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoScale.Library
{
    public class AutoSize
    {
        private SizeF _formSize;
        private List<int> _dgvColumnsSize;
        private List<Rectangle> _controlList;
        private List<int> _controlFontSize;
        private bool _showRowHeader = false;
        private string _font = "Tahoma";

        public AutoSize(Form form)
        {
            _dgvColumnsSize = new List<int>();
            _controlList = new List<Rectangle>();
            _controlFontSize = new List<int>();

            SetInitialSize(form);
        }

        public void Resize(Form form)
        {
            if (form.ClientSize.Width > 0 || form.ClientSize.Height > 0)
            {
                double ratioWidth = Math.Round((double)form.ClientSize.Width / (double)_formSize.Width, 2);
                double ratioHeight = Math.Round((double)form.ClientSize.Height / (double)_formSize.Height, 2);
                var _controls = GetAllControls(form);
                int _pos = 0;

                foreach (Control control in _controls)
                {
                    Size _controlSize = new Size((int)(_controlList[_pos].Width * ratioWidth),
                        (int)(_controlList[_pos].Height * ratioHeight));

                    Point _controlposition = new Point(
                        (int)(_controlList[_pos].X * ratioWidth),
                        (int)(_controlList[_pos].Y * ratioHeight));

                    control.Bounds = new Rectangle(_controlposition, _controlSize);

                    if (control.GetType() == typeof(DataGridView))
                    {
                        AdjustDgvColumn(((DataGridView)control), _showRowHeader, ratioWidth);
                    }

                    control.Font = new Font(_font,
                     (float)(((Convert.ToDouble(_controlFontSize[_pos]) * ratioWidth) / 3.55) +
                      ((Convert.ToDouble(_controlFontSize[_pos]) * ratioHeight) / 3.55)));
                    _pos += 1;
                }
            }
        }

        private void SetInitialSize(Form form)
        {
            _formSize = form.ClientSize;
            var controls = GetAllControls(form);

            _controlList.Clear();
            _controlFontSize.Clear();

            foreach(Control control in controls)
            {
                _controlList.Add(control.Bounds);
                _controlFontSize.Add((int)control.Font.Height);

                if (controls.GetType() == typeof(DataGridView))
                    _dgvColumnsSize = GetColumnWidth((DataGridView)control);
            }
        }

        private List<int> GetColumnWidth(DataGridView dgv)
        {
            _dgvColumnsSize.Clear();

            foreach (DataGridViewColumn column in dgv.Columns)
                _dgvColumnsSize.Add(column.Width);

            return _dgvColumnsSize;
        }

        private void AdjustDgvColumn(DataGridView dgv, bool showRowHeader, double widthRatio)
        {
            dgv.ScrollBars = ScrollBars.None;
            int verticalScrollBar = 0;

            if (dgv.RowCount != 0)
            {
                if (dgv.RowCount * dgv.Rows[0].Height > dgv.Height)
                {
                    verticalScrollBar = 17;
                    dgv.ScrollBars = ScrollBars.Vertical;
                    dgv.FirstDisplayedScrollingRowIndex = dgv.RowCount - 1;
                }

                for(int i = 0; i < dgv.ColumnCount; i++)
                {
                    if (dgv.Dock == DockStyle.Fill)
                    {
                        dgv.Columns[i].Width = Convert.ToInt32(_dgvColumnsSize[i] * widthRatio);
                    }
                    else
                    {
                        if (i == (dgv.ColumnCount - 1))
                            dgv.Columns[i].Width = Convert.ToInt32(_dgvColumnsSize[i] * widthRatio) - verticalScrollBar;
                        else
                            dgv.Columns[i].Width = Convert.ToInt32(_dgvColumnsSize[i] * widthRatio);
                    }
                }
            }
        }

        private static IEnumerable<Control> GetAllControls(Control control)
        {
            return control.Controls.Cast<Control>()
                .SelectMany(item => GetAllControls(item))
                .Concat(control.Controls.Cast<Control>())
                .Where(item => item.Name != string.Empty);
        }
    }
}
