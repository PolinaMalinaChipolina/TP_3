using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;


namespace GUI
{
    public class JogginView :ViewForm, IChartDraw
    {
        private DataTable Dtable;
        DataTable IChartDraw.dataTable { get => Dtable; set { Dtable = value; } }

        public JogginView(ref Chart c1, ref RichTextBox r1, ref DataGridView gridView1): base(ref c1,ref r1,ref gridView1)
        {
            chart1 = c1;
            richTextBox1 = r1;
            this.gridView1 = gridView1;
        }



        void IChartDraw.ShowAdditionalInfo(string Maininfo,string additionalInfo, bool append = false)
        {
            if(!append)
            { richTextBox1.Text = $"{Maininfo} {additionalInfo}\n";
                return;
            }
            richTextBox1.AppendText($"{Maininfo} {additionalInfo}\n");
            
        }

        void IChartDraw.ShowChart(string[] titles)
        {
                if (Dtable == null || !Dtable.Columns.Contains(titles[0]) || !Dtable.Columns.Contains(titles[1]) || !Dtable.Columns.Contains(titles[2]))
                {
                    MessageBox.Show("Некорректные параметры для построения графика.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                chart1.Series.Clear();
                chart1.ChartAreas.Clear();
                chart1.ChartAreas.Add(new ChartArea());

                // Первая серия для Distance
                Series series1 = new Series
                {
                    Name = "Distance",
                    Color = System.Drawing.Color.Blue,
                    ChartType = SeriesChartType.Line
                };
                chart1.Series.Add(series1);

                // Вторая серия для Duration
                Series series2 = new Series
                {
                    Name = "Duration",
                    Color = System.Drawing.Color.Red,
                    ChartType = SeriesChartType.Line
                };
                chart1.Series.Add(series2);

                foreach (DataRow row in Dtable.Rows)
                {
                    series1.Points.AddXY(row[titles[0]], row[titles[1]]);
                    series2.Points.AddXY(row[titles[0]], row[titles[2]]);
                }
                chart1.Invalidate();
        }

        void IChartDraw.ShowGrid()
        {
            gridView1.DataSource = Dtable;
        }

      

    }
}
