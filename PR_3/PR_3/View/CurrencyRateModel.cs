using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;
 

namespace GUI
{
    internal class CurrencyRateModel: IAnalyser
    {
     
        public IChartDraw view;

        public CurrencyRateModel(IChartDraw view)
        {   
            this.view = view;
        }
        internal void ReadExelFile(string filePath)
        {
            // Проверяем существование файла
            if (!File.Exists(filePath))
            {
                view.ShowAdditionalInfo("Файл не найден.", "");
                return;
            }

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Читаем данные из Excel файла
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Получаем таблицу из первого листа
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Добавляем столбцы в DataTable
                view.dataTable.Columns.Add("Дата");
                view.dataTable.Columns.Add("Лари");
                view.dataTable.Columns.Add("Кроны");

                // Проходимся по строкам и добавляем данные в DataTable
                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    DataRow dataRow = view.dataTable.NewRow();
                    dataRow["Дата"] = worksheet.Cells[row, 1].GetValue<string>(); // Считываем дату
                    dataRow["Лари"] = worksheet.Cells[row, 2].GetValue<string>(); // Считываем значение1
                    dataRow["Кроны"] = worksheet.Cells[row, 3].GetValue<string>(); // Считываем значение2
                    view.dataTable.Rows.Add(dataRow);
                }

        
            }
        }

        public void CalculateStatistics()
        {
            
            byte indexValute = 1;
            for (; indexValute < 3; indexValute++)
            {
                double max = 0, min = 99999;
                string DateMax = "", DateMin = "";
                short index = 0;

                CurrencyRateView rv = (CurrencyRateView)view;
                while (index < rv.CurrencyRates.Count() - 2)
                {
                    double valueCur; double.TryParse(rv.CurrencyRates[index][indexValute], out valueCur);
                    double valueNext; double.TryParse(rv.CurrencyRates[index + 1][indexValute], out valueNext);


                    if ((valueNext - valueCur) > max)
                    {
                        max = valueNext - valueCur;
                        DateMax = rv.CurrencyRates[index + 1][0];
                    }

                    if ((valueNext - valueCur) < min)
                    {
                        min = valueNext - valueCur;
                        DateMin = rv.CurrencyRates[index + 1][0];
                    }

                    index++;

                }

                view.ShowAdditionalInfo(indexValute.ToString(), " - Номер валюты",true);
                view.ShowAdditionalInfo($"Максимальный положительный скачок {max}, Максимальный отрицательный скачок валюты составил {min} ", $"Дата максимального роста :{DateMax}, дата максимальной девальвации: {DateMin} ",true);
            }
        }
    }
}

