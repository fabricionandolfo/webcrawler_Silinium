using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC_Selinium
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var scraper = new WebScraper();
            var log = scraper.GetDados("https://proxyservers.pro/proxy/list/order/updated/order_dir/desc%22", "/html/body/div[1]/div/div[2]/div/div/div/div[1]/div/table/tbody/tr[position()>0]");
        }
    }
}
