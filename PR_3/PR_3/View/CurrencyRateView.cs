using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;

namespace GUI
{
    internal class CurrencyRateView : ViewForm, IChartDraw
    {
        private DataTable Dt;

        internal List<List<string>> CurrencyRates =   new List<List<string>>();
        public CurrencyRateView(ref Chart c1, ref RichTextBox r1, ref DataGridView gridView1) : base(ref c1, ref r1, ref gridView1)
        {
            chart1 = c1;
            richTextBox1 = r1;
            this.gridView1 = gridView1;
            Dt = new DataTable();
            
        }

        public DataTable dataTable { get => Dt; set =>Dt = value; }

        public void ShowAdditionalInfo(string Maininfo, string additionalInfo, bool append = false)
        {
            if (!append)
            {
                richTextBox1.Text = $"{Maininfo} {additionalInfo}\n";
                return;
            }
            richTextBox1.AppendText($"{Maininfo} {additionalInfo}\n");
        }

        void IChartDraw.ShowChart(string[] titles)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add(new ChartArea());



            Series series1 = new Series
            {
                Name = titles[1],
                Color = System.Drawing.Color.Blue,
                ChartType = SeriesChartType.Line,
              
            };

            series1.XValueType = ChartValueType.Date;
            series1.YValueType = ChartValueType.Double;
            Series series2 = new Series
            {
                Name = titles[2],
                Color = System.Drawing.Color.Red,
                ChartType = SeriesChartType.Line
            };
            series2.XValueType = ChartValueType.Date;
            series2.YValueType = ChartValueType.Double;
            int i = 0;
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 2;
            chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 2;
            chart1.Series.Add(series1);
            chart1.Series.Add(series2);
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd.MM.yy";


            chart1.Invalidate();
            foreach (DataRow row in dataTable.Rows)
            {
                CurrencyRates.Add(new List<string>() { row[titles[0]].ToString(), row[titles[1]].ToString(), row[titles[2]].ToString() });

                if (row[titles[1]].ToString() == "") break;
                double d1,d2;
                double.TryParse(CurrencyRates[i][1],out d1);
                double.TryParse(CurrencyRates[i][2], out d2);

                series1.Points.AddXY(CurrencyRates[i][0], d1);
                series2.Points.AddXY(CurrencyRates[i][0],d2);
                i++;
        
            }

 


        }



        void IChartDraw.ShowGrid()
        {
            gridView1.DataSource = Dt;
        }
    }
}
