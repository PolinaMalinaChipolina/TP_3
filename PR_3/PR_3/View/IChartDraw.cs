using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace GUI
{
    public interface IChartDraw
    {
        DataTable dataTable { get; set; }

        void ShowChart(string[] titles); // Выводит на экран график [series]

        void ShowAdditionalInfo(string Maininfo, string additionalInfo,bool append = false); // Выводит на экран сообщение [info]

        void ShowGrid();// Выводит на экран таблицу

    }
}
