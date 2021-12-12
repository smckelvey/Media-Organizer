using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Application.MediaOrganizer.Engines
{
    public class DedupeEngine
    {
        public static void RemoveDuplicates(string folder, bool previewOnly)
        {
            // Take a snapshot of the file system.  
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folder);

            // This method assumes that the application has discovery permissions  
            // for all folders under the specified path.  
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            var queryDupNames = fileList.Where(f => FileTagsEngine.GetCameraModel(f.FullName) != null)
                .OrderBy(f => f.FullName.Length)
                .GroupBy(f => new { Name = RenamingEngine.StandardizeFileName(f.Name), Camera = FileTagsEngine.GetCameraModel(f.FullName) })
                .Where(fg => fg.Count() > 1);

            ProcessDupes(queryDupNames, previewOnly);
        }

        private static void ProcessDupes<K, V>(IEnumerable<System.Linq.IGrouping<K, V>> groupByExtList, bool previewOnly)
        {
            Console.WriteLine("** STARTING COUNT: " + groupByExtList.Count() + " **");
            var count = 0;
            // Iterate through the outer collection of groups.  
            foreach (var filegroup in groupByExtList)
            {
                Console.WriteLine("Filename = {0}", filegroup.Key.ToString() == String.Empty ? "[none]" : filegroup.Key.ToString());

                var dateTaken = FileTagsEngine.GetDateTaken(filegroup.ElementAt(0).ToString());
                var mainFile = true;
                foreach (var fileName in filegroup)
                {
                    if (mainFile)
                    {
                        Console.WriteLine("\tKEEP: {0}", fileName);
                        count++;
                    }
                    else
                    {
                        if (dateTaken != null && dateTaken == FileTagsEngine.GetDateTaken(fileName.ToString()))
                        {
                            if (previewOnly)
                            {
                                Console.WriteLine("\tTO BE DELETED: {0}", fileName);
                            }
                            else
                            {
                                Console.WriteLine("\tDELETED: {0}", fileName);
                                File.Delete(fileName.ToString());
                            }

                        }
                        else
                        {
                            Console.WriteLine("\tNO DATE MATCH: {0}", fileName);
                        }
                    }

                    mainFile = false; //Just use the first one as the main one since they're sorted by name length
                }
            }
            Console.WriteLine("** FINAL COUNT: " + count + " **");
        }
    }
}
