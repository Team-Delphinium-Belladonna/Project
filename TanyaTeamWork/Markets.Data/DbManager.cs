namespace Markets.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Collections.ObjectModel;
    using Migrations;

    using Markets.Model;

    public class DbManager
    {
        /*
        public static void SaveData(IMarketData marketData)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChainOfSupermarketsContext, Configuration>());
            try
            {
                var db = new ChainOfSupermarketsContext();
                /*
                var newVendorIds = marketData.Vendors.Select(v => v.Id).ToList()
                    .Except(db.Vendors.Select(ve => ve.Id).ToList()).ToList();
                var newVendorEntityes = marketData.Vendors.Where(x => newVendorIds.Contains(x.Id)).ToList();
                db.Vendors.AddRange(newVendorEntityes);

                var newSupermarketsId = marketData.Supermarkets.Select(v => v.Id).ToList()
                    .Except(db.Supermarkets.Select(ve => ve.Id).ToList()).ToList();
                var newSupermarketsEntityes = marketData.Supermarkets.Where(x => newSupermarketsId.Contains(x.Id)).ToList();
                db.Supermarkets.AddRange(newSupermarketsEntityes);

                var newMeasuresId = marketData.Measures.Select(v => v.Id).ToList()
                    .Except(db.Measures.Select(ve => ve.Id).ToList()).ToList();
                var newMeasuresEntityes = marketData.Measures.Where(x => newMeasuresId.Contains(x.Id)).ToList();
                db.Measures.AddRange(newMeasuresEntityes);

                var newProductsId = marketData.Products.Select(v => v.Id).ToList()
                    .Except(db.Products.Select(ve => ve.Id).ToList()).ToList();
                var newProductsEntityes = marketData.Products.Where(x => newProductsId.Contains(x.Id)).ToList();
                db.Products.AddRange(newProductsEntityes);

                var newSales = SaleDuplicateChecker(marketData.Sales);
                db.Sales.AddRange(newSales);

                var newExpenses = ExpenseDuplicateChecker(marketData.VendorExpenses);
                db.VendorExpenses.AddRange(newExpenses);
                

                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var a = ex;
            }
            catch (Exception exe)
            {
                var a = exe;
            }
        }

        public static ICollection<IEntity> FilterById(ICollection<IEntity> newEntity, ICollection<IEntity> oldEntity)
        {
            var newId = newEntity.Select(v => v.Id).ToList()
            .Except(oldEntity.Select(ve => ve.Id).ToList()).ToList();
            var filteredEntityes = newEntity.Where(x => newId.Contains(x.Id)).ToList();

            return filteredEntityes;
        }

        private static ICollection<Sale> SaleDuplicateChecker(ICollection<Sale> newSales)
        {
            var db = new ChainOfSupermarketsContext();
            var salesToAdd = new List<Sale>() { };
            foreach (var newSale in newSales)
            {
                var existInDatabase = db.Sales.Any(s => (s.Date == newSale.Date) &&
                                                  (s.Supermarket.Name == newSale.Supermarket.Name) &&
                                                  (s.Product.Name == newSale.Product.Name)).ToString();
                if (existInDatabase == "False")
                {
                    Sale sale = new Sale()
                    {
                        ProductId = newSale.Product.Id,
                        SupermarketId = newSale.Supermarket.Id,
                        Quantity = newSale.Quantity,
                        Date = newSale.Date
                    };
                    salesToAdd.Add(sale);
                }
            }

            return salesToAdd;
        }
        */

        /*
        private static ICollection<VendorExpenses> ExpenseDuplicateChecker(ICollection<VendorExpenses> newVendorExpenses)
        {
            var db = new ChainOfSupermarketsContext();
            var result = new List<VendorExpenses>() { };
            foreach (var newExpense in newVendorExpenses)
            {
                var existInDatabase = db.VendorExpenses.Any(s => (s.Date == newExpense.Date) &&
                                                  (s.VendorId == newExpense.VendorId)).ToString();
                if (existInDatabase == "False")
                {
                    result.Add(newExpense);
                }
            }
            return result;
        }
        */

        public static IList<ReportContainer> GetSalesGroupByVendorAndDate(DateTime startDate, DateTime endDate)
        {
            var db = new ChainOfSupermarketsContext();

            var sales = db.Sales
                .Where(s => s.DateOfSale >= startDate && s.DateOfSale <= endDate)
                .Select(s => new { Suppermarket = s.Vendor.VendorName, s.DateOfSale, TotalPrice = (s.Product.Price * s.Quantity) })
                .GroupBy(s => s.Suppermarket)
                .Select(g => new ReportContainer
                {
                    SupermarkeName = g.Key,
                    SaleReport = g.GroupBy(s => s.DateOfSale)
                        .Select(gd => new ReportData { Date = gd.Key, TotalSum = gd.Sum(s => s.TotalPrice) })
                        .ToList()
                })
                .ToList();

            return sales;
        }

        public static IList<ReportContainer> GetSalesOfEachProductForPeriod(DateTime startDate, DateTime endDate)
        {
            var db = new ChainOfSupermarketsContext();

            var sales = db.Sales
                //.Where(s => s.Date >= startDate && s.Date <= endDate)
                .GroupBy(s => s.Product.ProductName)
                .Select(g => new ReportContainer
                {
                    PrductName = g.Key,
                    SaleReport = g.Select(s => new ReportData
                    {
                        Id = s.ProductId,
                        Price = s.Product.Price,
                        Quantity = s.Quantity,
                        VendorName = s.Vendor.VendorName
                    })
                    .ToList()
                })
                .ToList();

            return sales;
        }
        /*
        public static IMarketData LoadData()
        {
            var data = new MarketData();

            using (var MsDb = new ChainOfSupermarketsContext())
            {
                MsDb.Measures.ForEachAsync(m => data.Measures.Add(m)).Wait();
                MsDb.Products.ToList().ForEach(p => data.Products.Add(p));
                MsDb.Sales.ForEachAsync(s => data.Sales.Add(s)).Wait();
                MsDb.Supermarkets.ForEachAsync(sup => data.Supermarkets.Add(sup)).Wait();
                MsDb.Vendors.ForEachAsync(v => data.Vendors.Add(v)).Wait();
                MsDb.VendorExpenses.ForEachAsync(ve => data.VendorExpenses.Add(ve)).Wait();
            }


            return data;
        }
        */

        public static IList<ReportContainer> GetSalesForPeriod(DateTime startDate, DateTime endDate)
        {
            var db = new ChainOfSupermarketsContext();

            var sales = db.Sales
                .Where(s => s.DateOfSale >= startDate && s.DateOfSale <= endDate)
                .GroupBy(s => s.DateOfSale)
                .Select(g => new ReportContainer
                {
                    SaleReport = g.Select(s => new ReportData
                    {
                        ProductName = s.Product.ProductName,
                        Quantity = s.Quantity,
                        Measure = s.Product.Measure.MeasureName,
                        Price = s.Product.Price,
                        VendorName = s.Vendor.VendorName,
                        TotalSum = s.Product.Price * s.Quantity,
                        Date = s.DateOfSale,
                        Id = s.ProductId,
                    })
                    .ToList()
                })
                .ToList();

            return sales;
        }
        /*
        public static Supermarket GetSupermarketByName(string name)
        {
            var db = new DbMarketContext();
            Supermarket supermarket = db.Supermarkets.Where(s => s.Name.Equals(name)).FirstOrDefault();

            return supermarket;
        }

        public static Product GetProductByName(string name)
        {
            var db = new DbMarketContext();
            Product product = db.Products.Where(p => p.Name.Equals(name)).FirstOrDefault();

            return product;
        }

        public static Vendor GetVendorByName(string name)
        {
            var db = new DbMarketContext();
            Vendor vendor = db.Vendors.Where(v => v.Name.Equals(name)).FirstOrDefault();

            return vendor;
        } */
    }
}