using Markets.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBandJSON
{

    public class Console
    {
        public static void Main()
        {
            var marketsContext = new ChainOfSupermarketsContext();

            JsonReporter jsonReport = new JsonReporter(marketsContext, "..\\..\\Json-Reports");
            //JsonReporter jsonReport = new JsonReporter("..\\..\\..\\Helper.Files\\Json-Reports");
            DateTime startDate = DateTime.Now.AddDays(-5);
            DateTime endDate = DateTime.Now.AddDays(-4);

            jsonReport.GenerateReport(startDate, endDate);
        }
    }

}
