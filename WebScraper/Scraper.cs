using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace WebScraper
{
    public class Scraper
    {
        public async void StartAsyncScraping(string url)
        {
            var httpclient = new HttpClient();

            var html = await httpclient.GetStringAsync(url);
            var htmlPage = new HtmlDocument();

            htmlPage.LoadHtml(html);
            var scrapedEvents = htmlPage.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Equals("content block-group chatterbox-margin")).ToList();

            var eventList = CreateEventList(scrapedEvents);
            ExportCSV(eventList);
        }

        public List<Event> CreateEventList(List<HtmlNode> events)
        {
            var eventList = new List<Event>();
            
            foreach (var item in events)
            {
               
                var artistOrEventDetails = item?.Descendants("h2").FirstOrDefault()?.InnerText;
                var artistOrEvent = artistOrEventDetails.Replace(",", "");

                var venueDetails = item?.Descendants("h4").FirstOrDefault()?.InnerText;
                var city = Regex.Match(venueDetails, @"\A\w+").ToString().Replace(",", "");
                var venue = Regex.Replace(venueDetails, @":", "").Replace(",", "");
                
                var dateAndTimeDetails = item?.Descendants("h4").Skip(1).FirstOrDefault()?.InnerText;
                var dateAndTimeDetailsArray = dateAndTimeDetails.Split(',');

                var dateAndTimeDetailsArrayLenth = dateAndTimeDetailsArray.Count();

                var date = "Not specified";
                var time = "Not specified";

                if (dateAndTimeDetailsArrayLenth > 1)
                {
                     date = dateAndTimeDetailsArray[0] + dateAndTimeDetailsArray[1];
                     time = dateAndTimeDetailsArray[2];
                }                

                var price = item?.Descendants("strong").FirstOrDefault()?.InnerText;
                var specialGuests = item?.Descendants("i")?.FirstOrDefault()?.InnerText.Replace(",", "");

                var availability = item?.Descendants("span")?.FirstOrDefault()?.InnerText.Replace(",", "");

                if (price == null)
                {
                    price = "Not specified";
                }

                if (specialGuests == null)
                {
                    specialGuests = "No special guests";
                }

                if (availability == null || availability == "NUS")
                {
                    availability = "Available";
                }

                var scrapedEvent = new Event
                {
                    ArtistorEvent = artistOrEvent,
                    Venue = venue,
                    City = city,
                    Date = date,
                    Time = time,
                    Price = price,
                    Availability = availability,
                    SpecialGuests = specialGuests
                };

                eventList.Add(scrapedEvent);
            }

            return eventList;
        }

        public bool ExportCSV(List<Event> scrapedEvents)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Artist or Event,Venue,City,Date,Time,Price,Availability,Special Guests");

                foreach (var scrapedEvent in scrapedEvents)
                {
                    csvContent.AppendLine($"{scrapedEvent.ArtistorEvent},{scrapedEvent.Venue},{scrapedEvent.City},{scrapedEvent.Date},{scrapedEvent.Time},{scrapedEvent.Price},{scrapedEvent.Availability},{scrapedEvent.SpecialGuests}");
                }

                csvContent.AppendLine();

                string csvPath = "C:\\Scraped Events\\Scraped Events.csv";
                File.AppendAllText(csvPath, csvContent.ToString());

                return true;
            }

            catch(Exception e)
            {
                return false;
            }
        }
    }
}
