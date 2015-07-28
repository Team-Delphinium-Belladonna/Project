namespace Markets.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleClient
    {
        class EntryPoint
        {
            private const int MainMenu = 0;
            private const int ReportMenu = 5;
            public delegate void ExecuteController(string fileName);
            public delegate void ExecuteControllerForDatePeriod(string fileName, DateTime stertDate, DateTime endDate);

            static void Main()
            {
                var nextProcess = MainMenu;
                var isError = false;

                while (true)
                {
                    if (nextProcess == MainMenu)
                    {
                        if (!isError)
                        {
                            Console.Clear();
                        }

                        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
                        Console.WriteLine("+                 Supermarkets Chain              +");
                        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
                        Console.WriteLine();
                        Console.WriteLine(" 1. Transfer Data ");
                        Console.WriteLine(" 2. Create reports");
                        Console.WriteLine(" 3. Exit");
                        Console.WriteLine();

                        Console.Write("Input number (1 or 3): ");
                        var number = Console.ReadLine();

                        switch (number)
                        {
                            case "1":
                                nextProcess = 1;
                                break;
                            case "2":
                                nextProcess = 5;
                                break;
                            case "3":
                                return;
                               // break;
                            default:
                                nextProcess = MainMenu;
                                break;
                        }
                    }

                    try
                    {
                        if (nextProcess >= 1 && nextProcess <= 4)
                        {
                            if (!isError)
                            {
                                Console.Clear();
                            }

                            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
                            Console.WriteLine("+                 Supermarkets Chain              +");
                            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
                            Console.WriteLine();
                            Console.WriteLine("                Transfer Data ");
                            Console.WriteLine(" 1. Data from Oracle database to MSSQL Database");
                            Console.WriteLine(" 2. Data from Zip files to MSSQL Database");
                            Console.WriteLine(" 3. Data from XML files to MSSQL database");
                            Console.WriteLine(" 4. Data from MSSQL database to MySQL Database");
                            Console.WriteLine(" 5. Exit");
                            Console.WriteLine();

                            Console.Write("Choose number of report (1 - 5): ");
                            nextProcess = int.Parse(Console.ReadLine());
                            if (nextProcess == 5)
                            {
                                nextProcess = MainMenu;
                            }

                            Console.Clear();
                        }
                        /*
                        ProcessController(1,
                            "1. Data from Oracle database to MSSQL Database\n Do You want to transfer data /yes (Y), no (N)/: ",
                            ref nextProcess,
                            delegate (string inputInfo)
                            {
                                MarketsController.OracleToMsSql();
                            });

                        ProcessController(2,
                            "2. Data from Zip files to MSSQL Database\n\nDo You want to transfer data /yes (Y), no (N)/: ",
                            ref nextProcess,
                            MarketsController.ZipExcelToMsSql,
                            "Input file name: ");


                        ProcessController(3,
                            "3. Data from XML files to MSSQL database\n\nDo You want to transfer data /yes (Y), no (N)/: ",
                            ref nextProcess,
                            MarketsController.XmlToMsSql,
                            "Input file name: ");

                        ProcessController(4,
                            "4. Data from MSSQL database to MySQL Database\n\nDo You want to transfer data /yes (Y), no (N)/: ",
                            ref nextProcess,
                            delegate (string inputInfo)
                            {
                                MarketsController.MsSqlToMySql();
                            },
                            null,
                            ReportMenu); */

                        if (nextProcess == ReportMenu)
                        {
                            if (!isError)
                            {
                                Console.Clear();
                            }

                            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
                            Console.WriteLine("+                 Supermarkets Chain              +");
                            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
                            Console.WriteLine();
                            Console.WriteLine("             Create reports");
                            Console.WriteLine();
                            Console.WriteLine(" 1. Reports for the sales of each product for given period (output in MongoDB) ");
                            Console.WriteLine(" 2. Report in XML format holding the sales by vendor for given period");
                            Console.WriteLine(" 3. Reports summarizing the sales information for given period to PDF format");
                            Console.WriteLine(" 4. Financial results report to Excel file");
                            Console.WriteLine(" 5. Exit");
                            Console.WriteLine();

                            Console.Write("Choose number of report (1 - 5): ");
                            nextProcess = int.Parse(Console.ReadLine()) + 5;
                            if (nextProcess == 10)
                            {
                                nextProcess = MainMenu;
                            }

                            Console.Clear();
                        }

                        ProcessController(6,
                            "1. Create reports for the sales of each product for given period in JSON format. \n\nDo You want create report? /yes (Y) or no (N)/: ",
                            ref nextProcess,
                            delegate (string directory, DateTime starDate, DateTime endDate)
                            {
                                MarketsController.MsSqlToJson(directory, starDate, endDate);
                                MarketsController.JsonFilesToMongoDb(directory);
                            },
                            "Enter repository's directory",
                            ReportMenu);
                        /*
                        ProcessController(7,
                            "2. Report in XML format holding the sales by vendor for given period. \n\nDo You want create report? /yes (Y) or no (N)/: ",
                            ref nextProcess,
                            MarketsController.MsSqlToXml,
                            "Enter file name: ",
                            ReportMenu);

                        ProcessController(8,
                            "3. Reports summarizing the sales information for given period to PDF format. \n\nDo You want create report? /yes (Y) or no (N)/: ",
                            ref nextProcess,
                            MarketsController.MsSqlToPdf,
                            "Enter file name: ",
                            ReportMenu);

                        ProcessController(9,
                            "4. Financial results report to Excel file \n\nDo You want create report? /yes (Y) or no (N)/: ",
                            ref nextProcess,
                            MarketsController.SqliteMySqlToXlsx,
                            "Enter file name: ",
                            ReportMenu); */
                    }
                    catch (Exception ex)
                    {
                        if (!(ex is TaskCanceledException))
                        {
                            Console.Clear();
                            Console.WriteLine("Error: " + ex.Message);
                            Console.WriteLine();
                            isError = true;
                        }
                    }
                }

            }

            private static void ProcessController(int processNumber, string message, ref int nextProcess,
                ExecuteController controller, string inputInfo = null, int returnNumberProcess = -1)
            {
                if (processNumber != nextProcess)
                {
                    return;
                }

                string input = null;
                Console.Write(message);
                var userChoose = Console.ReadLine();
                if (userChoose == null || userChoose.ToUpper() != "Y")
                {
                    processNumber = returnNumberProcess;
                    throw new TaskCanceledException();
                }


                if (inputInfo != null)
                {
                    Console.Write(inputInfo + " ");
                    input = Console.ReadLine();
                }

                Console.WriteLine("\nPlease wait...");
                controller(input);
                nextProcess++;
                if (returnNumberProcess == -1)
                {
                    nextProcess++;
                }
                else
                {
                    nextProcess = returnNumberProcess;
                }

                Console.Clear();
            }

            private static void ProcessController(int processNumber, string message, ref int nextProcess,
                ExecuteControllerForDatePeriod controller, string inputInfo, int returnNumberProcess = -1)
            {
                if (processNumber != nextProcess)
                {
                    return;
                }

                Console.Write(message);
                var userChoose = Console.ReadLine();
                if (userChoose == null || userChoose.ToUpper() != "Y")
                {
                    nextProcess = returnNumberProcess;
                    throw new TaskCanceledException();
                };

                string input = null;
                if (inputInfo != null)
                {
                    Console.Write(inputInfo + " ");
                    input = Console.ReadLine();
                }

                try
                {
                    Console.Write("Input start date (dd-mm-yyyy): ");
                    var startDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Input end date (dd-mm-yyyy): ");
                    var endDate = DateTime.Parse(Console.ReadLine());
                    Console.WriteLine("\nPlease wait...");
                    controller(input, startDate, endDate);
                }
                catch (InvalidDataException)
                {
                    throw new Exception("Invalid date format.");
                }

                if (returnNumberProcess == -1)
                {
                    nextProcess++;
                }
                else
                {
                    nextProcess = returnNumberProcess;
                }

                Console.Clear();
            }
        }
    }


    //using System;
    //using Markets.Data;
    ////using Markets.DataManipulation;
    //using Markets.Model;

    //internal class Program
    //{
    //    internal static void Main()
    //    {
    //        //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChainOfSupermarketsContext, Configuration>());

    //        var db = new ChainOfSupermarketsContext();

    //        //var measure = new Measure { MeasureName = "liters" };
    //        //db.Measures.Add(measure);
    //        //var measure2 = new Measure { MeasureName = "pieces" };
    //        //db.Measures.Add(measure2);
    //        //var measure3 = new Measure { MeasureName = "kg" };
    //        //db.Measures.Add(measure3);

    //        //var v = new Vendor { VendorName = "Nestle Sofia Corp." };
    //        //db.Vendors.Add(v);
    //        //v = new Vendor { VendorName = "Zagorka Corp." };
    //        //db.Vendors.Add(v);
    //        //v = new Vendor { VendorName = "Targovishte Bottling Company Ltd." };
    //        //db.Vendors.Add(v);
    //        //v = new Vendor { VendorName = "Coca-Cola" };
    //        //db.Vendors.Add(v);
    //        //v = new Vendor { VendorName = "Simid A&D" };
    //        //db.Vendors.Add(v);
    //        //v = new Vendor { VendorName = "Bella AD" };
    //        //db.Vendors.Add(v);

    //        //var pr = new Product
    //        //{
    //        //    ProductName = "Beer \"Zagorka\"",
    //        //    Price = 0.86m,
    //        //    VendorId = 2,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Vodka \"Targovishte\"",
    //        //    Price = 7.56m,
    //        //    VendorId = 3,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Beer \"Beck's\"",
    //        //    Price = 1.03m,
    //        //    VendorId = 2,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Chocolate \"Milka\"",
    //        //    Price = 2.80m,
    //        //    VendorId = 1,
    //        //    MeasureId = 3
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Soda \"Coca - Cola\"",
    //        //    Price = 1.20m,
    //        //    VendorId = 4,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Bread \"Rustic Bread\"",
    //        //    Price = 1.00m,
    //        //    VendorId = 5,
    //        //    MeasureId = 3
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Cereal \"Nesquick\"",
    //        //    Price = 4.60m,
    //        //    VendorId = 1,
    //        //    MeasureId = 3
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Bread \"White Bread\"",
    //        //    Price = 0.80m,
    //        //    VendorId = 5,
    //        //    MeasureId = 3
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Soda \"Fanta\"",
    //        //    Price = 1.10m,
    //        //    VendorId = 4,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Bread \"Brown Bread\"",
    //        //    Price = 0.90m,
    //        //    VendorId = 5,
    //        //    MeasureId = 3
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Ice Cream \"Magnum\"",
    //        //    Price = 2.99m,
    //        //    VendorId = 1,
    //        //    MeasureId = 2
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Soda \"Sprite\"",
    //        //    Price = 1.15m,
    //        //    VendorId = 4,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Cider \"Strongbow\"",
    //        //    Price = 1.60m,
    //        //    VendorId = 2,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Chocolate \"LZ\"",
    //        //    Price = 1.40m,
    //        //    VendorId = 1,
    //        //    MeasureId = 3
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Ham \"Leki\"",
    //        //    Price = 3.49m,
    //        //    VendorId = 6,
    //        //    MeasureId = 3
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Ice Tea \"Nestea\"",
    //        //    Price = 1.59m,
    //        //    VendorId = 4,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Beer \"Amstel\"",
    //        //    Price = 0.90m,
    //        //    VendorId = 2,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Mince \"Naroden\"",
    //        //    Price = 1.20m,
    //        //    VendorId = 6,
    //        //    MeasureId = 3
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Biscuits \"Zhiten Dar\"",
    //        //    Price = 2.25m,
    //        //    VendorId = 1,
    //        //    MeasureId = 2
    //        //};
    //        //db.Products.Add(pr);
    //        //pr = new Product
    //        //{
    //        //    ProductName = "Juice \"Cappy\"",
    //        //    Price = 1.19m,
    //        //    VendorId = 4,
    //        //    MeasureId = 1
    //        //};
    //        //db.Products.Add(pr);

    //        //var loc = new Location { Name = "Sofia" };
    //        //db.Locations.Add(loc);
    //        //loc = new Location { Name = "Plovdiv" };
    //        //db.Locations.Add(loc);
    //        //loc = new Location { Name = "Varna" };
    //        //db.Locations.Add(loc);
    //        //loc = new Location { Name = "Burgas" };
    //        //db.Locations.Add(loc);
    //        //loc = new Location { Name = "Ruse" };
    //        //db.Locations.Add(loc);
    //        //loc = new Location { Name = "Pleven" };
    //        //db.Locations.Add(loc);

    //        //var s = new Sale
    //        //{
    //        //    Quantity = 6,
    //        //    ProductId = 12,
    //        //    PricePerUnit = 1.15m,
    //        //    DateOfSale = DateTime.Now.AddDays(-1),
    //        //    LocationId = 4
    //        //};
    //        //db.Sales.Add(s);
    //        //s = new Sale
    //        //{
    //        //    Quantity = 15,
    //        //    ProductId = 4,
    //        //    PricePerUnit = 2.80m,
    //        //    DateOfSale = DateTime.Now.AddDays(-1),
    //        //    LocationId = 5
    //        //};
    //        //db.Sales.Add(s);
    //        //s = new Sale
    //        //{
    //        //    Quantity = 13,
    //        //    ProductId = 12,
    //        //    PricePerUnit = 1.15m,
    //        //    DateOfSale = DateTime.Now.AddDays(-2),
    //        //    LocationId = 2
    //        //};
    //        //db.Sales.Add(s);
    //        //s = new Sale
    //        //{
    //        //    Quantity = 12,
    //        //    ProductId = 8,
    //        //    PricePerUnit = 0.80m,
    //        //    DateOfSale = DateTime.Now.AddDays(-2),
    //        //    LocationId = 3
    //        //};
    //        //db.Sales.Add(s);
    //        //s = new Sale
    //        //{
    //        //    Quantity = 9,
    //        //    ProductId = 19,
    //        //    PricePerUnit = 1.20m,
    //        //    DateOfSale = DateTime.Now.AddDays(-2),
    //        //    LocationId = 1
    //        //};
    //        //db.Sales.Add(s);

    //        //db.SaveChanges();

    //        //// JSON Reports
    //        //string reportsDirectoryPath = @"..\..\Json-Reports";
    //        //var json = new JsonReport(db, reportsDirectoryPath);
    //        //DateTime start = new DateTime(27/7/2015);
    //        //DateTime end = new DateTime(28/7/2015);
    //        //json.ProductSalesForPeriod(start, end);
    //        //Console.WriteLine("JSON reports generate.");

    //        //Console.WriteLine("Ready");
    //        Console.ReadLine();
    //    }
    //}
}
