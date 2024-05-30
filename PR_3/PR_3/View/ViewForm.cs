using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace GUI
{
    public class ViewForm
    {
        protected Chart chart1;
        protected RichTextBox richTextBox1;
        protected DataGridView gridView1;

        public  ViewForm(ref Chart c1, ref RichTextBox r1, ref DataGridView gridView1)
        {
            chart1 = c1;
            richTextBox1 = r1;
            this.gridView1 = gridView1;
        }

    }
}
