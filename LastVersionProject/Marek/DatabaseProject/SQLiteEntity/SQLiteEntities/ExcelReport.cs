using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Reflection;
using MySql.Data.MySqlClient;
using SQL = System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQLiteEntities
{
    class ExcelReport
    {
        static void Main()
        {
            string sqlSelect = "SELECT * FROM Taxes";
            string sqliteDataSourcePath = @"..\..\..\..\Resources\SQLiteDb.db";
            var sqliteConnection = new SQLiteConnection("Data Source=" + sqliteDataSourcePath);
            var sqliteSelectCommand = new SQLiteCommand(sqlSelect, sqliteConnection);

            var sales = new List<Sales>();
            SQLiteDataReader sqLiteDataReader = null;

            var productTaxes = new SQL.DataTable("Report");

            productTaxes.Columns.Add("Vendor");
            productTaxes.Columns.Add("Incomes");
            productTaxes.Columns.Add("Expenses");
            productTaxes.Columns.Add("TotalTaxes");
            productTaxes.Columns.Add("Financial result");

            String mysqlDataSource = "server=localhost;Database=chain_of_supermarkets;uid=softuni;pwd=1234567;";
            MySqlConnection mySqlConnection = new MySqlConnection(mysqlDataSource);
            String selectData =
                "SELECT " +
                    "v.name AS Vendor," +
                    "p.name AS Product, " +
                    "IFNULL((SELECT SUM(p.price) " +
                            "FROM products p " +
                                "JOIN vendors ve " +
                                    "ON ve.id = p.vendor_id " +
                                "JOIN sales s " +
                                    "ON s.product_id = p.id " +
                            "WHERE ve.id = v.id), 0) AS Incomes, " +
                    "IFNULL((SELECT SUM(e.expense_value) " +
                            "FROM vendors ven " +
                                "JOIN vendors_expenses vex " +
                                    "ON vex.vendor_id = ven.id " +
                                "JOIN expenses e " +
                                    "ON e.id = vex.expense_id " +
                            "WHERE ven.id = v.id), 0) AS Expenses " +
                "FROM vendors v " +
                    "JOIN products p " +
                        "ON p.vendor_id = v.id " +
                "GROUP BY v.id, v.name, p.name";

            MySqlCommand mysqlSelectCommand = new MySqlCommand(selectData, mySqlConnection);
            MySqlDataReader mySqlDataReader = null;

            try
            {
                sqliteConnection.Open();
                sqLiteDataReader = sqliteSelectCommand.ExecuteReader();
                mySqlConnection.Open();
                mySqlDataReader = mysqlSelectCommand.ExecuteReader();

                while (sqLiteDataReader.Read() && mySqlDataReader.Read())
                {
                    var tax = Convert.ToDecimal(sqLiteDataReader["Tax"].ToString());
                    var income = mySqlDataReader.GetDecimal(2);
                    var taxPercent = tax / 100;
                    var currentTax = income * taxPercent;
                    currentTax = System.Math.Round(currentTax, 2);
                    var sale = new Sales(mySqlDataReader.GetString(0), income, mySqlDataReader.GetDecimal(3), currentTax);
                    sales.Add(sale);
                }

                var groupSales = sales.GroupBy(s => new
                {
                    s.Name,
                    s.Expense,
                    s.Income
                }).Select(s => new
                {
                    VendorName = s.Key.Name,
                    Expense = s.Key.Expense,
                    Income = s.Key.Income,

                    FinancialResult =
                        (s.Key.Income - s.Key.Expense - s.Sum(se => se.Tax)
                        ).ToString(new CultureInfo("en-US")),
                    Tax = s.Sum(se => se.Tax)
                        .ToString(new CultureInfo("en-US"))
                }).ToList();


                groupSales.ForEach(
                    gs => productTaxes.Rows.Add(gs.VendorName, gs.Income, gs.Expense, gs.Tax, gs.FinancialResult));
            }
            catch (Exception)
            {
                Console.WriteLine("Exception!");
            }
            finally
            {
                if (sqLiteDataReader != null)
                {
                    sqLiteDataReader.Close();
                }
                if (sqLiteDataReader != null)
                {
                    mySqlDataReader.Close();
                }
                sqliteConnection.Close();
            }

            SQL.DataSet dataSet = new SQL.DataSet();
            dataSet.Tables.Add(productTaxes);
            ExportDataToExcel(dataSet);

        }

        private static void ExportDataToExcel(SQL.DataSet dataSet)
        {
            var excelApplication = new Excel.Application();
            String excelReportPath = @"C:\Users\Alexander\Desktop\teamWork\Project\Marek\DatabaseProject\ExcelReport\Report.xlsx";
            String excelReportPathBackup = String.Format(@"C:\Users\Alexander\Desktop\teamWork\Project\Marek\DatabaseProject\ExcelReport\Report-{0}-{1}-{2}-{3}-{4}-{5}.xlsx",
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour,
                DateTime.Now.Minute,
                DateTime.Now.Second);

            if (System.IO.File.Exists(excelReportPath))
            {
                System.IO.File.Move(excelReportPath, excelReportPathBackup);
            }

            var excelWorkBook = excelApplication.Workbooks.Add(Missing.Value);

            const int VendorsIndex = 1;
            const int IncomesIndex = 2;
            const int ExpensesIndex = 3;
            const int TotalTaxesIndex = 4;
            const int FinancialResultIndex = 5;

            foreach (SQL.DataTable table in dataSet.Tables)
            {
                Excel.Worksheet excelWorksheet = excelWorkBook.Sheets.Add();

                excelWorksheet.Name = table.TableName;

                excelWorksheet.Cells[1, VendorsIndex] = table.Columns[VendorsIndex - 1].ColumnName;
                excelWorksheet.Cells[1, IncomesIndex] = table.Columns[IncomesIndex - 1].ColumnName;
                excelWorksheet.Cells[1, ExpensesIndex] = table.Columns[ExpensesIndex - 1].ColumnName;
                excelWorksheet.Cells[1, TotalTaxesIndex] = table.Columns[TotalTaxesIndex - 1].ColumnName;
                excelWorksheet.Cells[1, FinancialResultIndex] = table.Columns[FinancialResultIndex - 1].ColumnName;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        excelWorksheet.Cells[i + 2, j + 1] = table.Rows[i].ItemArray[j].ToString();
                    }
                }
            }

            excelWorkBook.SaveAs(excelReportPath);
            excelWorkBook.Save();
            excelWorkBook.Close();
            excelApplication.Quit();
            if (System.IO.File.Exists(excelReportPath))
            {
                RunExcelReport(excelReportPath);
            }

        }

        private static void RunExcelReport(string path)
        {
            System.Diagnostics.Process.Start(path);
        }
    }
}
