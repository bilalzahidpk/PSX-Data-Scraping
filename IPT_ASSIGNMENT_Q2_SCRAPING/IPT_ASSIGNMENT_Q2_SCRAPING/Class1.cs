using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace IPT_ASSIGNMENT_Q2_SCRAPING
{
    public class CreateXML
    {
        public static string path;
        public static string value;
        public static XDocument doc;
        public static int j;

        public static void CreateXMLFile(string filePath)
        {
            j = 0;
            string html = File.ReadAllText(@"" + filePath );
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var CategoryList = htmlDocument.DocumentNode.SelectNodes("//th[contains(@colspan, '8')]").ToList();
            foreach (HtmlNode category in CategoryList)
            {
                path = Path.Combine(@"D:\IPT Assignment#1\Files", category.InnerText.Replace(@"/", @","));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                value = category.InnerText;
            }
            var Companies = htmlDocument.DocumentNode.SelectNodes("//table").Skip(2).ToList();

            foreach (HtmlNode category in CategoryList)
            {
                path = Path.Combine(@"D:\IPT Assignment#2\Files", category.InnerText.Replace(@"/", @","));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                value = category.InnerText;
            }
            foreach (HtmlNode Company in Companies)
            {
                var CompanyName = Company.SelectNodes(".//tr[contains(@class, 'green-text-td') or contains(@class, 'blue-text-td') or contains(@class, 'red-text-td')]/td[1]").ToList();
                var Price = Company.SelectNodes(".//tr[contains(@class, 'green-text-td') or contains(@class, 'blue-text-td') or contains(@class, 'red-text-td')]/td[6]").ToList();
                doc = new XDocument(new XElement("root"));
                for (int i = 0; i < CompanyName.Count; i++)
                {
                    Console.WriteLine(CompanyName[i].InnerText);
                    doc.Root.Add(new XElement("Scripts", new XElement("Script", CompanyName[i].InnerText.Replace("&", "&amp;")),
                                    new XElement("Price", Price[i].InnerText)));

                }
                string dateNow = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
                doc.Save(Path.Combine(@"D:\IPT Assignment#2\Files", CategoryList[j].InnerText.Replace(@"/", @","), dateNow));
                j++;
            }
        }
    }

}
