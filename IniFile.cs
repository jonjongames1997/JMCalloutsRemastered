using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace JMCalloutsRemastered
{
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile
    {
        private string filePath;

        public IniFile(string filePath)
        {
            this.filePath = filePath;
        }

        public string ReadValue(string section, string key)
        {
            string value = "";
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    string currentSection = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            currentSection = line.Substring(1, line.Length - 2);
                        }
                        else if (currentSection.Equals(section) && line.StartsWith(key))
                        {
                            int startIndex = line.IndexOf("=") + 1;
                            value = line.Substring(startIndex);
                            break;
                        }
                    }
                }
            }
            return value;
        }

        public void WriteValue(string section, string key, string value)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    bool sectionFound = false;
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            if (line.Substring(1, line.Length - 2).Equals(section))
                            {
                                sectionFound = true;
                            }
                            writer.WriteLine(line);
                        }
                        else if (sectionFound && line.StartsWith(key))
                        {
                            writer.WriteLine(key + "=" + value);
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                    if (!sectionFound)
                    {
                        writer.WriteLine();
                        writer.WriteLine("[" + section + "]");
                        writer.WriteLine(key + "=" + value);
                    }
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("[" + section + "]");
                    writer.WriteLine(key + "=" + value);
                }
            }
        }
    }
}
