using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;

namespace IPT_ASSIGNMENT_Q4_JSON
{
    public class Class1
    {
        
        public static void GetJsonAndRemoveXML(string GetDirectories, string DestinationFolder)
        {
            string[] dirs = Directory.GetDirectories(@"" + GetDirectories);
            foreach (string dir in dirs)
            {
                string lastFolderName = Path.GetFileName(dir);
    
                DirectoryInfo folder = Directory.CreateDirectory(@"" + DestinationFolder + lastFolderName);
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    XDocument xml = XDocument.Load(@"" + file);
                    foreach (XElement element in xml.Root.Elements("Scripts"))
                    {

                        string script = (string)element.Element("Script");
                        string price = (string)element.Element("Price");


                        //Console.WriteLine(@""+ folder.FullName + @"\" + script + ".json");
                        if (!File.Exists(@"" + folder.FullName + @"\" + script + ".json"))
                        {
                            Data data = new Data();
                            var item = new List<Item>
                          {
                             new Item { Date = DateTime.Now, price = price },
                          };
                            ScriptData scriptdata = new ScriptData();
                            scriptdata.script = item;
                            scriptdata.LastUpdatedOn = DateTime.Now;
                            data.ScriptData = scriptdata;
                            string JsonResult = JsonConvert.SerializeObject(data);
                            using (var tw = new StreamWriter(@"" + folder.FullName + @"\" + script + ".json", true))
                            {
                                tw.WriteLine(JsonResult.ToString());
                                tw.Close();
                            }
                            File.Delete(@"" + file);
                        }

                        else
                        {
                            var filePath = @"" + folder.FullName + @"\" + script + ".json";

                            var jsonData = System.IO.File.ReadAllText(filePath);
                            // Read existing json data
                            var message = JsonConvert.DeserializeObject<Data>(jsonData);

                            var item = new Item { Date = DateTime.Now, price = price };
                            message.ScriptData.script.Add(item);

                            string JsonResult = JsonConvert.SerializeObject(message, Newtonsoft.Json.Formatting.Indented);

                            File.WriteAllText(filePath, JsonResult);
                            File.Delete(@"" + file);
                        }

                    }
                }
            }
        }
    }

    class Data
    {
        public ScriptData ScriptData;
    }

    class Item
    {
        public DateTime Date;
        public string price;
    }
    class ScriptData
    {
        public DateTime LastUpdatedOn;
        public List<Item> script;
    }
}
