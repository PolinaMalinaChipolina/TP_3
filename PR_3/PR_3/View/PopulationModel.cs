using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI
{
    public class PopulationModel:IAnalyser
    {
        public IChartDraw view;
        public PopulationModel(IChartDraw v)
        {
            view = v;

        }
        internal void ReadTextFile(string filePath)
        {
            view.dataTable = new DataTable();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string headerLine = sr.ReadLine();
                if (headerLine == null)
                {
                    return; // Возвращаем null, если файл пуст
                }

                string[] headers = headerLine.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string header in headers)
                {
                    view.dataTable.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        continue;
                    }

                    string[] rows = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (rows.Length != headers.Length)
                    {
                        continue; // Пропускаем строки, которые не соответствуют количеству заголовков
                    }

                    DataRow dr = view.dataTable.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    view.dataTable.Rows.Add(dr);
                }
            }
            return;
        }





        public void CalculateStatistics()
        {
            if (view.dataTable == null)
            {
                return; // Если таблица данных не содержит необходимых колонок
            }

            double maxIncrease = 0;
            string yearWithMaxIncrease = string.Empty;

            for (int i = 1; i < view.dataTable.Rows.Count; i++)
            {
                double previousChildren = Convert.ToDouble(view.dataTable.Rows[i - 1]["Children"]);
                double previousAdults = Convert.ToDouble(view.dataTable.Rows[i - 1]["Adults"]);
                double previousTotalPopulation = previousChildren + previousAdults;

                double currentChildren = Convert.ToDouble(view.dataTable.Rows[i]["Children"]);
                double currentAdults = Convert.ToDouble(view.dataTable.Rows[i]["Adults"]);
                double currentTotalPopulation = currentChildren + currentAdults;

                double increase = ((currentTotalPopulation - previousTotalPopulation) / previousTotalPopulation) * 100;

                if (increase > maxIncrease)
                {
                    maxIncrease = increase;
                    yearWithMaxIncrease = view.dataTable.Rows[i]["Year"].ToString();
                }
            }

            view.ShowAdditionalInfo($"Максимальный процент прироста: {maxIncrease:F2}%", $"Год: {yearWithMaxIncrease}");
        }

   
    }
}