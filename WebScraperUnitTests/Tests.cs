using WebScraper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace WebScraperUnitTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ShouldReturnTrueUponCreatingCsv()
        {
            Scraper scraper = new Scraper();

            var eventList = new List<Event>
            {
                new Event
                {
                  ArtistorEvent = "Billy Soomro's Interview",
                  City = "London",
                  Venue = "SongKick",
                  Time = "12:30pm",
                  Price = "Not specified",
                  Availability = "Available",
                  Date = "Fri 8th Dec 2017",
                  SpecialGuests = "Bring Me The Horizon"
                }
            };

            var result = scraper.ExportCSV(eventList);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ShouldReturnListofEvents()
        {
            Scraper scraper = new Scraper();

            var scrapedEvents = new List<HtmlNode>();

            var eventList = new List<Event>();      
            
            var result = scraper.CreateEventList(scrapedEvents);

            Assert.AreEqual(result.Count, eventList.Count);
        }
    }
}
