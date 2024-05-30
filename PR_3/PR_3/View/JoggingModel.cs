using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GUI
{
    // класс для формирования таблицы и графика для бега
    public class JoggingModel:IAnalyser
    {
        public IChartDraw view;
        public JoggingModel(IChartDraw v)
        {
            view = v;
        
        }
        internal  void ReadTextFile(string filePath)
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
     void IAnalyser.CalculateStatistics()
        {
            double totalWeekendDistance = 0;
            CultureInfo culture = new CultureInfo("en-US"); // Используем "en-US" культуру для точечного разделителя

            foreach (DataRow row in view.dataTable.Rows)
            {
                if (DateTime.TryParse(row["Date"].ToString(), out DateTime date))
                {
                    // Проверка на выходной день
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (double.TryParse(row["Distance"].ToString(), NumberStyles.Any, culture, out double distance))
                        {
                            totalWeekendDistance += distance;
                        }
                        else
                        {
                            view.ShowAdditionalInfo($"Ошибка преобразования Distance:",$"{row["Distance"]}\n");
                        }
                    }
                }
                else
                {
                    view.ShowAdditionalInfo($"Ошибка преобразования Date:",$"{row["Date"]}\n");
                }
            }

            view.ShowAdditionalInfo($"Сумма пройденных километров за все выходные дни:",$"{totalWeekendDistance} км\n");
        }

     
    }
}
