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

        private static List<Marker> ProcessStep(List<string> textData, string name, string startString, string endString, DateTime startTime, Log log)
        {
            int startMarkerIndex = -1;
            int endMarkerIndex = -1;
            string startTimeString = "";
            string endTimeString = "";
            List<Marker> markers = new List<Marker>();

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
            while (index<textData.Count)
            {
                if (textData[index].Contains(endString))
                {
                    endMarkerIndex = index;
                    endTimeString = textData[index].Substring(0, 19);
                    break;
                }
                index++;
            }

            if (startMarkerIndex != -1)
            {
                markers.Add(new Marker(name, DateTime.ParseExact(startTimeString, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US")).Subtract(startTime).TotalMilliseconds));
            }
            else
            {
                log.AddLine("Could not find Start Index for log line '" + name + "'");
            }

            if (endMarkerIndex != -1)
            {
                markers.Add(new Marker("END: " + name, DateTime.ParseExact(endTimeString, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US")).Subtract(startTime).TotalMilliseconds));
            }
            else
            {
                log.AddLine("Could not find End Index for log line '" + name + "'");
            }

            return markers;
        }

        private static List<Marker> ProcessSingleLine(List<string> textData, string name, string startString, DateTime startTime) 
        {
            string startTimeString = "";
            string endTimeString = "";
            List<Marker> markers = new List<Marker>();

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
            return markers;
        } 

        public static List<Marker> FromDelighterRemoveShadingLogFile(string filename, DateTime startTime, Log log)
        {
            log.AddLine("Adding Markers from Delighter Remove Shading Log File...");

            List<string> textData = MarkerBuilder.LoadDataAsStringList(filename);
            List<Marker> markers = new List<Marker>();

            markers.AddRange(ProcessStep(textData, "Remove Shading", "RemoveLightingInteractive: ", "(exit code 1)", startTime, log));
            markers.AddRange(ProcessStep(textData, "Calculating Angular Data", "Calculating angular data...", "Angular data calculated in ", startTime, log));
            markers.AddRange(ProcessStep(textData, "Calculating Scale", "Calculating scale...", "Scale calculated in ", startTime, log));
            markers.AddRange(ProcessStep(textData, "Suppressing Highlights", "Suppressing highlights...", "Highlights suppressed in ", startTime, log));
            markers.AddRange(ProcessSingleLine(textData, "Calculating Ambient Occlusions", "Calculating ambient occlusion...", startTime));
         
            return markers;
        }

        public static List<Marker> FromDelighterRemoveCastShadowsLogFile(string filename, DateTime startTime, Log log)
        {
            log.AddLine("Adding Markers from Delighter Remove Cast Shadows Log File...");

            List<string> textData = MarkerBuilder.LoadDataAsStringList(filename);
            List<Marker> markers = new List<Marker>();

            markers.AddRange(ProcessStep(textData, "Remove Cast Shadows", "RemoveLightingInteractive: ", "(exit code 1)", startTime, log));
            markers.AddRange(ProcessStep(textData, "Repairing Atlas", "Repairing atlas...", "Atlas repaired in ", startTime, log));
            markers.AddRange(ProcessStep(textData, "Baking matt", "Baking matting...", "Matting baked in ", startTime, log));
            markers.AddRange(ProcessStep(textData, "Baking Shadow Masks", "Baking shadow masks...", "Shadow masks baked in ", startTime, log));
            markers.AddRange(ProcessStep(textData, "Inpainting Penumbra", "Inpainting penumbra...", "Penumbra inpainted in ", startTime, log));

            markers.AddRange(ProcessStep(textData, "Supressing Unknown Colors", "Suppressing unknown colors...", "Unknown colors suppressed in ", startTime, log));

            return markers;
        }

        public static List<Marker> FromMetashapeLogFile(string filename, DateTime startTime, Log log)
        {
            log.AddLine("Adding Markers from Metashape Log File...");

            List<string> textData = MarkerBuilder.LoadDataAsStringList(filename);
            List<Marker> markers = new List<Marker>();

            markers.AddRange(ProcessStep(textData, "Build Points", "Matching photos...", "(exit code 1)", startTime, log));
            markers.AddRange(ProcessStep(textData, "Build Dense Cloud", "BuildDenseCloud: ", "(exit code 1)", startTime, log));
            markers.AddRange(ProcessStep(textData, "Build Mesh", "BuildModel: ", "(exit code 1)", startTime, log));
            markers.AddRange(ProcessStep(textData, "Set Tree", "Tree depth:", "Tree set in ", startTime, log));
            markers.AddRange(ProcessStep(textData, "Laplacian Constraints", "Leaves/Nodes: ", "Laplacian constraints set in ", startTime, log));
            markers.AddRange(ProcessStep(textData, "Linear System", "Depth[0/", "Linear system solved in ", startTime, log));
            markers.AddRange(ProcessStep(textData, "Build Texture", "BuildTexture: ", "(exit code 1)", startTime, log));

            markers.AddRange(ProcessSingleLine(textData, "Extracting faces", "faces extracted in ", startTime));
            markers.AddRange(ProcessSingleLine(textData, "Decimating mesh", "Decimating mesh...", startTime));
            markers.AddRange(ProcessSingleLine(textData, "Calculate Colors", "calculating colors...", startTime));

            return markers;
        }

    }
}
