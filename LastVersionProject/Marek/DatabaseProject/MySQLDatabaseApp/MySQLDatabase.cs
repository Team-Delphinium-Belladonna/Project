namespace MySQLDatabaseApp
{
    using System;
    using System.IO;
    using System.Linq;

    using MySql.Data.MySqlClient;
    using Markets.Data;

    public class MySQLDatabase
    {
        private const string CONNECTION_STRING = "Server=localhost;Port=3306;Uid=softuni;Pwd=1234567;";
        private const string MYSQL_DATABASE_SCHEMA_FILE = "../../../Resources/mysql_schema.sql";

        private MySqlConnection mysqlConnection;
        private ChainOfSupermarketsContext sqlServerContext;

        public MySQLDatabase(ChainOfSupermarketsContext context)
        {
            this.mysqlConnection = new MySqlConnection(CONNECTION_STRING);
            this.sqlServerContext = context;
        }

        public void Migrate()
        {
            this.CreateDbSchema();
            this.ImportDataFromSqlServer();
        }

        private void CreateDbSchema()
        {
            var queries = File.ReadAllText(MYSQL_DATABASE_SCHEMA_FILE);

            this.mysqlConnection.Open();

            using (this.mysqlConnection)
            {
                using (var command = new MySqlCommand(queries, mysqlConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void ImportDataFromSqlServer()
        {
            this.mysqlConnection.Open();

            using (this.mysqlConnection)
            {
                using (var sqlServerDb = this.sqlServerContext)
                {
                    this.ImportVendors();
                    this.ImportMeasures();
                    this.ImportLocations();
                    this.ImportProducts();
                    this.ImportSales();
                    this.ImportExpenses();
                }
            }
        }

        private void ExecuteMySQLQuery(string query)
        {
            using (var command = new MySqlCommand(query, this.mysqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void ImportVendors()
        {
            var vendorNames = this.sqlServerContext.Vendors.Select(v => "('" + v.VendorName + "')");
            var query = string.Format(
                @"USE `chain_of_supermarkets`;
                INSERT IGNORE INTO `vendors` (`name`) VALUES {0};",
                string.Join(",", vendorNames));

            this.ExecuteMySQLQuery(query);
        }

        private void ImportMeasures()
        {
            var measureNames = this.sqlServerContext.Measures.Select(m => "('" + m.MeasureName + "')");
            var query = string.Format(
                @"USE `chain_of_supermarkets`;
                INSERT IGNORE INTO `measures` (`name`) VALUES {0};",
                string.Join(",", measureNames));

            this.ExecuteMySQLQuery(query);
        }

        private void ImportLocations()
        {
            var locationNames = this.sqlServerContext.Locations.Select(l => "('" + l.Name + "')");
            var query = string.Format(
                @"USE `chain_of_supermarkets`;
                INSERT IGNORE INTO `locations` (`name`) VALUES {0};",
                string.Join(",", locationNames));

            this.ExecuteMySQLQuery(query);
        }

        private void ImportProducts()
        {
            var products = this.sqlServerContext.Products.Select(p => new
            {
                p.ProductName,
                p.Price,
                VendorName = p.Vendors.VendorName,
                MeasureName = p.Measures.MeasureName
            });

            foreach (var product in products)
            {
                int vendorId = GetVendorId(product.VendorName);
                int measureId = GetMeasureId(product.MeasureName);

                var query = string.Format(
                    @"USE `chain_of_supermarkets`;
                    INSERT INTO `products` (`name`, `price`, `vendor_id`, `measure_id`)
                    VALUES('{0}', {1}, {2}, {3});",
                    product.ProductName,
                    product.Price,
                    vendorId,
                    measureId);

                this.ExecuteMySQLQuery(query);
            }
        }

        private void ImportSales()
        {
            var sales = this.sqlServerContext.Sales.Select(s => new
            {
                s.Quantity,
                s.DateOfSale,
                productName = s.Product.ProductName,
                locationName = s.Location.Name
            });

            foreach (var sale in sales)
            {
                var productId = this.GetProductId(sale.productName);
                var locationId = this.GetLocationId(sale.locationName);

                var query = string.Format(
                    @"USE `chain_of_supermarkets`;
                    INSERT INTO `sales` (`quantity`, `date_of_sale`, `product_id`, `location_id`)
                    VALUES({0}, '{1}', {2}, {3});",
                    sale.Quantity,
                    sale.DateOfSale.ToString("yyyy-MM-dd"),
                    productId,
                    locationId);

                this.ExecuteMySQLQuery(query);
            }
        }

        private void ImportExpenses()
        {
            var expenses = this.sqlServerContext.Expenses.Select(e => new
            {
                e.Value,
                e.Month,
                vendorName = e.Vendor.VendorName
            });

            foreach (var expense in expenses)
            {

                var query = string.Format(
                    @"USE `chain_of_supermarkets`;
                    INSERT INTO `expenses` (`expense_value`, `expense_month`)
                    VALUES({0}, '{1}');",
                    expense.Value,
                    expense.Month.ToString("yyyy-MM-dd"));

                this.ExecuteMySQLQuery(query);

                var expenseIdQuery = string.Format(
                    @"USE `chain_of_supermarkets`;
                    SELECT `id` FROM `expenses` WHERE `expense_value` = {0}",
                    expense.Value);
                using (var command = new MySqlCommand(expenseIdQuery, this.mysqlConnection))
                {
                    var vendorId = this.GetVendorId(expense.vendorName);
                    int expenseId = Convert.ToInt32(command.ExecuteScalar());

                    var insertVendorsExpensesQuery = string.Format(
                        @"USE `chain_of_supermarkets`;
                        INSERT INTO `vendors_expenses` (`vendor_id`, `expense_id`)
                        VALUES({0}, {1});",
                        vendorId,
                        expenseId);
                    this.ExecuteMySQLQuery(insertVendorsExpensesQuery);
                }
            }
        }

        private int GetVendorId(string vendorName)
        {
            var query = string.Format(
                @"USE `chain_of_supermarkets`;
                SELECT `id` FROM `vendors` WHERE `name`='{0}'",
                vendorName);

            using (var command = new MySqlCommand(query, this.mysqlConnection))
            {
                return (int)command.ExecuteScalar();
            }
        }

        private int GetMeasureId(string measureName)
        {
            var query = string.Format(
                @"USE `chain_of_supermarkets`;
                SELECT `id` FROM `measures` WHERE `name`='{0}'",
                measureName);

            using (var command = new MySqlCommand(query, this.mysqlConnection))
            {
                return (int)command.ExecuteScalar();
            }
        }

        private int GetLocationId(string locationName)
        {
            var query = string.Format(
                @"USE `chain_of_supermarkets`;
                SELECT `id` FROM `locations` WHERE `name`='{0}'",
                locationName);

            using (var command = new MySqlCommand(query, this.mysqlConnection))
            {
                return (int)command.ExecuteScalar();
            }
        }

        private int GetProductId(string productName)
        {
            var query = string.Format(
                @"USE `chain_of_supermarkets`;
                SELECT `id` FROM `products` WHERE `name`='{0}'",
                productName);

            using (var command = new MySqlCommand(query, this.mysqlConnection))
            {
                return (int)command.ExecuteScalar();
            }
        }
    }
}