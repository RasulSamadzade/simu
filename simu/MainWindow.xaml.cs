using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OfficeOpenXml;
using System.Data.SQLite;

namespace simu
{
    public partial class MainWindow : Window
    {
        InterCrack interCrack;
        SQLiteConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()) + "TechnoProbe");
            string connectionString = "Data Source=" + Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 9) + "technoprob.db;Version=3;New=False;Compress=True;";
            sqlConnection = new SQLiteConnection(connectionString);
            Type.ItemsSource = new List<String>() {"MLO", "Direct Attach", "Interposer", "Hybrid", "Space Transformer" };
            Department.ItemsSource = new List<String>() {"Soldering", "Board BE", "PC Testing" };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (checkFields()) {
                interCrack = new InterCrack(Double.Parse(Components.Text));
                interCrack.calculateValues(Type.Text, InterCost.Text);

                switch (Department.SelectedItem.ToString())
                {
                    case "Soldering": CrackCost.Text = interCrack.interCrackValues.solderingModel.ToString(); break;
                    case "Board BE": CrackCost.Text = interCrack.interCrackValues.boardModel.ToString(); break;
                    case "PC Testing": CrackCost.Text = interCrack.interCrackValues.pcModel.ToString(); break;
                    default: break;
                }
                saveTodB();
            } else
            {
                MessageBox.Show("Please enter all fields!");
            }

        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            generateListFromDatabase();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");
                excel.Workbook.Worksheets.Add("Worksheet3");
                var data = generateListFromDatabase();
                var headerRow = new List<string[]>() { new string[] { "Date", "Name", "Surname", "Type", "Department", "Cost", "CompNum", "Crack Cost", "Note" } };
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                string borderRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + (data.Count + 1).ToString();
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                setExcelStyle(worksheet, headerRange, borderRange);
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                worksheet.Cells[2, 1].LoadFromArrays(data);
                var dir = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()) + "TechnoProbe";
                FileInfo excelFile = new FileInfo(dir + "\\TechnoProbSimu.xlsx");
                excel.SaveAs(excelFile);
            }
        }

        private void saveTodB()
        {
            try
            {
                string query = "INSERT INTO Results (Date, Name, Surname, Type, Department, Cost, CompNum, CrackCost, Note) values (@Date, @Name, @Surname, @Type, @Department, @Cost, @CompNum, @CrackCost, @Note)";
                SQLiteCommand sqLiteCommand = new SQLiteCommand(query, sqlConnection);
                sqlConnection.Open();
                sqLiteCommand.Parameters.AddWithValue("@Date", (DateTime.Today.Date + DateTime.Now.TimeOfDay).ToString());
                sqLiteCommand.Parameters.AddWithValue("@Name", Name.Text);
                sqLiteCommand.Parameters.AddWithValue("@Surname", Surname.Text);
                sqLiteCommand.Parameters.AddWithValue("@Type", Type.SelectedValue);
                sqLiteCommand.Parameters.AddWithValue("@Department", Department.SelectedValue);
                sqLiteCommand.Parameters.AddWithValue("@Cost", InterCost.Text);
                sqLiteCommand.Parameters.AddWithValue("@CompNum", Components.Text);
                sqLiteCommand.Parameters.AddWithValue("@CrackCost", CrackCost.Text);
                sqLiteCommand.Parameters.AddWithValue("@Note", Note.Text);
                sqLiteCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public List<string[]> generateListFromDatabase()
        {
            var cellData = new List<string[]>();
            try
            {
                string query = "SELECT Date, Name, Surname, Type, Department, Cost, CompNum, CrackCost, Note FROM Results";
                sqlConnection.Open();

                var cmd = new SQLiteCommand(query, sqlConnection);
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var cell = new string[] { rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6), rdr.GetString(7), rdr.GetString(8) };
                        cellData.Add(cell);
                    }
                }
                return cellData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
            return cellData;
        }

        public void setExcelStyle(OfficeOpenXml.ExcelWorksheet worksheet, String headerRange, String borderRange)
        {
            worksheet.Cells[headerRange].Style.Font.Bold = true;
            worksheet.Cells[headerRange].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(0, 150, 150, 150);
            worksheet.Cells.AutoFitColumns();
            worksheet.Column(1).Width = 20;
            worksheet.Column(2).Width = 20;
            worksheet.Column(3).Width = 20;
            worksheet.Column(4).Width = 20;
            worksheet.Column(5).Width = 20;
            worksheet.Column(6).Width = 20;
            worksheet.Column(7).Width = 20;
            worksheet.Column(8).Width = 20;
            worksheet.Column(9).Width = 20;
            worksheet.Cells[borderRange].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[borderRange].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[borderRange].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[borderRange].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        }

        private bool checkFields() {
            if (Name.Text == "" || Surname.Text == "" || Note.Text == "" || InterCost.Text == "" || Components.Text == "" || Type.SelectedIndex == -1 || Department.SelectedIndex == -1)
            {
                return false;
            }
            else {
                return true;
            }
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Type.SelectedValue.ToString() == "MLO")
                Department.ItemsSource = new List<String>() { "Soldering", "Board BE", "PC Testing" };
            else
                Department.ItemsSource = new List<String>() { "Board BE", "PC Testing" };
        }
    }
}
