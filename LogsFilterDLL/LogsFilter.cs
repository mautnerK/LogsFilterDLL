using System.Runtime.CompilerServices;

namespace LogsFilterDLL
{
    public static class LogsFilter
    {
        public static void filterLogs(string logLocation, string type, string vault,
               string modul, string start="", string end="")
        {
            int startLocation;
            int endLocation;
            string[] lines = File.ReadAllLines(logLocation);
            List<string> filteredList = new List<string>();
            DateTime startDateTime = DateTime.Now;
            DateTime endDateTime = DateTime.Now;
            if (!String.IsNullOrEmpty(start)) { startDateTime = Convert.ToDateTime(start); }
            if (!String.IsNullOrEmpty(end)) { endDateTime = Convert.ToDateTime(end); }

            foreach (string line in lines)
            {
                if (type != "" && type != null)
                {
                    startLocation = line.IndexOf("[", StringComparison.Ordinal);
                    endLocation = line.IndexOf("]", StringComparison.Ordinal);
                    if (filterLog(line, startLocation, endLocation, type.ToUpper()) != null) filteredList.Add(line);
                }
                if (vault != "" && vault != null)
                {
                    startLocation = line.IndexOf("{", StringComparison.Ordinal);
                    endLocation = line.IndexOf("}", StringComparison.Ordinal);
                    if (type != "") filterSublist(filteredList, line, startLocation, endLocation, vault);
                    else if (type == "" && filterLog(line, startLocation, endLocation, vault.ToUpper()) != null) filteredList.Add(line);
                }
                if (modul != "" && modul != null)
                {
                    startLocation = line.IndexOf("[", line.IndexOf("[") + 1);
                    endLocation = line.IndexOf("]", line.IndexOf("]") + 1);
                    if (type != "" || vault != "") filterSublist(filteredList, line, startLocation, endLocation, modul);
                    else if (type == "" && vault == "" && filterLog(line, startLocation, endLocation, modul) != null) filteredList.Add(line);
                }
                if (start != end)
                {
                    startLocation = line.IndexOf(" +", StringComparison.Ordinal);
                    if (type == "" && vault == "" && modul == "")
                    {
                        if (Convert.ToDateTime(line.Substring(0, startLocation)) >= startDateTime && Convert.ToDateTime(line.Substring(0, startLocation)) <= endDateTime)
                            filteredList.Add(line);
                    }
                    else
                    {
                        if (Convert.ToDateTime(line.Substring(0, startLocation)) < startDateTime || Convert.ToDateTime(line.Substring(0, startLocation)) > endDateTime)
                            filteredList.Remove(line);
                    }
                }
            }
            foreach(var line in filteredList)
            {
                Console.WriteLine(line);
            }
        }
        private static string? filterLog(string line, int startLocation, int endLocation, string keyword)
        {
            if (line.Substring(startLocation, endLocation - startLocation).Contains(keyword)) return line;
            else return null;
        }
        private static void filterSublist(List<string> sublist, string line, int startLocation, int endLocation, string type)
        {
            if (filterLog(line, startLocation, endLocation, type) == null) sublist.Remove(line);
        }
       
    }
}
