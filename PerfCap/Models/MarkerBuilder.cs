using Newtonsoft.Json;
using PerfCap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using PerfCap.Model;

namespace jPerf
{
    static class MarkerBuilder
    {
        public static string LoadData(string filename)
        {
            string data;
            FileStream Stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (StreamReader Reader = new StreamReader(Stream, Encoding.UTF8))
            {
                data = Reader.ReadToEnd();
            }
            return data;
        }

        public static List<string> LoadDataAsStringList(string filename)
        {
            string[] data = File.ReadAllLines(filename);
            return new List<string>(data);
        }

        public static List<Marker> FromMarkerFile(string filename, DateTime startTime)
        {
            string textData = MarkerBuilder.LoadData(filename);

            List<Marker> markers = new List<Marker>();

            dynamic jsonData = JsonConvert.DeserializeObject<dynamic>(textData);

            foreach (dynamic marker in jsonData)
            {
                DateTime time = DateTime.ParseExact((string)marker.time, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
                if (time >= startTime)
                {
                    markers.Add(new Marker((string)marker.title, time.Subtract(startTime).TotalMilliseconds));
                }
            }
            return markers;
        }

        public static List<Marker> FromMetashapeLogFile(string filename, DateTime startTime, Log log)
        {
            log.AddLine("Adding Markers from Metashape Log File...");

            List<string> textData = MarkerBuilder.LoadDataAsStringList(filename);
            List<Marker> markers = new List<Marker>();

            Func<string, string, int> processSingleLine = (string name, string startString) =>
            {
                string startTimeString = "";
                string endTimeString = "";

                var index = 0;
                while (index < textData.Count)
                {
                    if (textData[index].Contains(startString))
                    {
                        startTimeString = textData[index].Substring(0, 19);
                        if (index < textData.Count())
                        {
                            endTimeString = textData[index + 1].Substring(0, 19);
                        }
                        break;
                    }
                    index++;
                }

                markers.Add(new Marker(name, DateTime.ParseExact(startTimeString, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US")).Subtract(startTime).TotalMilliseconds));
                markers.Add(new Marker("END: " + name, DateTime.ParseExact(endTimeString, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US")).Subtract(startTime).TotalMilliseconds));
                return 0;
            };

            Func<string, string, string, int> processStep = (string name, string startString, string endString) => 
            {
                int startMarkerIndex = -1;
                int endMarkerIndex = -1;
                string startTimeString = "";
                string endTimeString = "";

                foreach (var line in textData.Select((x, i) => new { Value = x, Index = i }))
                {
                    if (line.Value.Contains(startString))
                    {
                        startMarkerIndex = line.Index;
                        startTimeString = line.Value.Substring(0, 19);
                        break;
                    }
                }

                var index = startMarkerIndex;
                while (index < textData.Count)
                {
                    if (textData[index].Contains(endString))
                    {
                        endMarkerIndex = index;
                        endTimeString = textData[index].Substring(0, 19);
                        Console.WriteLine(endTimeString);
                        break;
                    }
                    index++;
                }


                if(startMarkerIndex == -1) { return -1; }
                if(endMarkerIndex == -1) { return -1; }

                markers.Add(new Marker(name, DateTime.ParseExact(startTimeString, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US")).Subtract(startTime).TotalMilliseconds));
                markers.Add(new Marker("END: " + name, DateTime.ParseExact(endTimeString, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US")).Subtract(startTime).TotalMilliseconds));
                return 0;
            };

            if(processStep("Build Points", "Matching photos...", "(exit code 1)") == -1)
            {
                log.AddLine("Could not find Build Points markers");
                return null;
            }

            if (processStep("Build Dense Cloud", "BuildDenseCloud: ", "(exit code 1)") == -1)
            {
                log.AddLine("Could not find Build Dense Cloud markers");
                return null;
            }
            if(processStep("Build Mesh", "BuildModel: ", "(exit code 1)") == -1)
            {
                log.AddLine("Could not find Build Mesh markers");
                return null;
            }
                if (processStep("Set Tree", "Tree depth:", "Tree set in ") == -1)
                {
                    log.AddLine("Could not find Set Tree markers");
                    return null;
                }
                if (processStep("Laplacian Constraints", "Leaves/Nodes: ", "Laplacian constraints set in ") == -1)
                {
                    log.AddLine("Could not find Laplace markers");
                    return null;
                }
                if (processStep("Linear System", "Depth[0/", "Linear system solved in ") == -1)
                {
                    log.AddLine("Could not find Linear System markers");
                    return null;
                }
                processSingleLine("Extracting faces", "faces extracted in ");
                processSingleLine("Decimating mesh", "Decimating mesh...");


            if (processStep("Build Texture", "BuildTexture: ", "(exit code 1)") == -1)
            {
                log.AddLine("Could not find Build Texture markers");
                return null;
            }
            
            processSingleLine("Calculate Colors", "calculating colors...");
            
            return markers;
        }

    }
}
