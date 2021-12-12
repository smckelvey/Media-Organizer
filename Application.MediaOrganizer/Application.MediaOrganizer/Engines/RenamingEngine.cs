using System;
using System.Collections.Generic;
using System.IO;

namespace Application.MediaOrganizer.Engines
{
    public class RenamingEngine
    {
        internal static void CleanFileNames(string folder, bool previewOnly)
        {
            // Take a snapshot of the file system.  
            DirectoryInfo dir = new DirectoryInfo(folder);

            // This method assumes that the application has discovery permissions  
            // for all folders under the specified path.  
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);

            Console.WriteLine("** FILES TO RENAME **");

            foreach (var file in fileList)
            {
                var standardizedName = StandardizeFileName(file.Name);
                if (!file.Name.Equals(standardizedName, StringComparison.OrdinalIgnoreCase))
                {
                    if (!File.Exists(standardizedName))
                    {
                        Console.WriteLine($"\t{file.Name}  ->  {standardizedName}");
                        if (!previewOnly)
                        {
                            file.MoveTo(standardizedName);
                        }
                    }
                }
            }
        }

        public static string StandardizeFileName(string name)
        {
            var result = name;

            result = result.Replace(" (1)", "");
            result = result.Replace(" (2)", "");
            result = result.Replace(" (3)", "");
            result = result.Replace(" (4)", "");
            result = result.Replace(" (5)", "");
            result = result.Replace(" (6)", "");
            result = result.Replace("(1)", "");
            result = result.Replace("(2)", "");
            result = result.Replace("(3)", "");
            result = result.Replace("(4)", "");
            result = result.Replace("(5)", "");
            result = result.Replace("(6)", "");
            result = result.Replace(" - copy", "", StringComparison.OrdinalIgnoreCase);
            result = result.Replace("- copy", "", StringComparison.OrdinalIgnoreCase);
            //result = result.Replace("-", "");
            //result = result.Replace(" ", "");

            return result.Trim();
        }

        public static string IncrementFilename(string path)
        {
            if (File.Exists(path))
            {
                if (!path.Contains("("))
                {
                    var nextPath = path.Substring(0, path.IndexOf(".")) + " (1)" + path.Substring(path.IndexOf("."));
                    return IncrementFilename(nextPath);
                }

                int lastNumber;
                var lastChar = path.Substring(path.IndexOf("(") + 1, 1);
                if (int.TryParse(lastChar, out lastNumber))
                {
                    lastNumber++;
                }
                var newName = path.Substring(0, path.IndexOf("(")) + "(" + lastNumber + path.Substring(path.IndexOf(")"));
                return IncrementFilename(newName);
            }
            else
            {
                return path;
            }
        }
    }
}
