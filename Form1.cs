using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimMod_4
{
    public partial class Form1 : Form
    {
        int n = 8;
        public List<List<int>> cells = new List<List<int>>();
        bool isInit = false;
        int time = 0;

        void InitCells()
        {
            cells.Clear();
            for (int i = 0; i < n; i++)
            {
                cells.Add(new List<int>());
                for (int j = 0; j < n; j++)
                {
                    cells[i].Add(0);
                }
            }
        }

        void InitDataGrid()
        {
            table.Columns.Clear();
            table.Rows.Clear();

            for (int i = 0; i < n; i++)
            {
                table.Columns.Add(new DataGridViewColumn() { CellTemplate = new DataGridViewTextBoxCell() });
            }
            for (int i = 0; i < n; i++)
            {
                table.Rows.Add();
                for (int j = 0; j < n; j++)
                {
                    table.Rows[i].Cells[j].Value = 0;
                    cells[i][j] = 0;
                }
            }
        }

        void Init()
        {
            if (!isInit)
            {
                InitCells();
                InitDataGrid();
                time = 0;
                timeText.Text = time.ToString();
                isInit = true;
            }
        }

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                isInit = false;
            }
            else
            {
                Init();
                timer1.Enabled = true;
            }
        }

        int GetNeighbourCellsValue(List<List<int>> prevCells, int x, int y)
        {
            int sum = 0;

            for (int i = Math.Max(x - 1, 0); i <= Math.Min(x + 1, n - 1); i++)
            {
                for (int j = Math.Max(y - 1, 0); j <= Math.Min(y + 1, n - 1); j++)
                {
                    if (!(i == x && j == y))
                    {
                        sum += prevCells[i][j];
                    }
                }
            }

            return sum;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<List<int>> prevCells = new List<List<int>>(cells);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (prevCells[i][j] == 0)
                    {
                        if (GetNeighbourCellsValue(prevCells, i, j) == 3)
                        {
                            cells[i][j] = 1;
                            SetCellColor(1, i, j);
                        }
                    }
                    else
                    {
                        switch (GetNeighbourCellsValue(prevCells, i, j))
                        {
                            case 2:
                            case 3:
                                cells[i][j] = prevCells[i][j];
                                SetCellColor(cells[i][j], i, j);
                                break;
                            default:
                                cells[i][j] = 0;
                                SetCellColor(0, i, j);
                                break;
                        }
                    }
                }
            }
            time++;
            timeText.Text = time.ToString();
        }

        void SetCellColor(int value, int x, int y)
        {
            if (value == 1)
            {
                table.Rows[y].Cells[x].Style.BackColor = Color.Red;
            }
            else
            {
                table.Rows[y].Cells[x].Style.BackColor = table.DefaultCellStyle.BackColor;
            }
        }

        private void table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            table.CurrentCell.Selected = false;
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();
            if (cells[e.ColumnIndex][e.RowIndex] == 1)
            {
                cells[e.ColumnIndex][e.RowIndex] = 0;
                CellStyle.BackColor = table.DefaultCellStyle.BackColor;
            }
            else
            {
                cells[e.ColumnIndex][e.RowIndex] = 1;
                CellStyle.BackColor = Color.Red;
            }
            table.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = CellStyle;

        }
    }
}
