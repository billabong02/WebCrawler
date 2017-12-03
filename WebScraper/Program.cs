using System;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Web Scraper ...");
            Scraper scraper = new Scraper();

            int i = 1;
            while (i < 11)
            {
                string url = "http://www.wegottickets.com/searchresults/page/" + i.ToString()  +  "/adv#paginate";

                scraper.StartAsyncScraping(url);

                i++;
            }

            Console.WriteLine("Web Scrapping Complete ... ");
            Console.ReadLine();            
        }        
    }
}
