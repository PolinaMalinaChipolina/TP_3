using System;
using System.Linq;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GUI;


namespace PR_3
{
    public partial class Controller : Form/*, IChartDraw будущий интерфейс для отрисовки графиков и таблиц*/
    {

        JoggingModel joggingModel;
        CurrencyRateModel currencyRateModel;
        PopulationModel populationModel;
        IAnalyser analyser;

        public Controller()
        {
            InitializeComponent();
            joggingModel = new JoggingModel(new JogginView(ref chart1, ref richTextBox1, ref dataGridView1));
            currencyRateModel = new CurrencyRateModel(new CurrencyRateView(ref chart1, ref richTextBox1, ref dataGridView1));
            populationModel = new PopulationModel(new PopulationView(ref chart1, ref richTextBox1, ref dataGridView1));
            comboBox1.Items.Add("Пробежка");
            comboBox1.Items.Add("Валюты");
            comboBox1.Items.Add("Популяция");
        }

      

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "(*.xlsx) | *.xlsx|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Select a text file"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                if (comboBox1.SelectedItem.ToString() == "Пробежка")
                {
                    joggingModel.ReadTextFile(filePath);

                    if (joggingModel.view.dataTable != null)
                    {
                        joggingModel.view.ShowGrid();
                        joggingModel.view.ShowChart(new string[] { "Date", "Distance", "Duration" });
                        analyser = joggingModel;


                    }


                    else
                    {
                        MessageBox.Show("Ошибка при чтении файла. Проверьте формат данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (comboBox1.SelectedItem.ToString() == "Валюты")
                {

                    currencyRateModel.ReadExelFile(filePath);
                    currencyRateModel.view.ShowGrid();
                    currencyRateModel.view.ShowChart(new string[] { "Дата", "Лари", "Кроны" });
                    analyser = currencyRateModel;
                }


                else if (comboBox1.SelectedItem.ToString().Equals("Популяция"))
                {

                    populationModel.ReadTextFile(filePath);
                    populationModel.view.ShowGrid();
                    populationModel.view.ShowChart(new string[] { "Year", "Children", "Adults" });
                    analyser = populationModel;
                }

                analyser.CalculateStatistics();
            }
        }

      
    }
}
