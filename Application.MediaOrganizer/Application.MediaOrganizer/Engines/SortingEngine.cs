using System;
using System.Collections.Generic;
using System.IO;

namespace Application.MediaOrganizer.Engines
{
    public class SortingEngine
    {
        public static void SortFilesByDateTaken(string folder, string sortedPictureFolder, string sortedVideosFolder)
        {
            //check to make sure inputs are different so we don't loop infinitely

            // Take a snapshot of the file system.  
            DirectoryInfo dir = new DirectoryInfo(folder);

            // This method assumes that the application has discovery permissions  
            // for all folders under the specified path.  
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (var file in fileList)
            {
                //get the date taken for the image
                var dateTaken = FileTagsEngine.GetDateTaken(file.FullName);

                var targetFolder = sortedPictureFolder;

                if (file.Name.Contains(".mov", StringComparison.CurrentCultureIgnoreCase) || file.Name.Contains(".mp4", StringComparison.CurrentCultureIgnoreCase))
                {
                    targetFolder = sortedVideosFolder;
                }

                if (dateTaken == null)
                {
                    targetFolder += "\\No_Date_Taken";
                    dateTaken = (DateTime?)((file.CreationTime < file.LastWriteTime) ? file.CreationTime : file.LastWriteTime);
                }

                var year = dateTaken.Value.Year;
                var month = dateTaken.Value.ToString("MM");
                var fullTargetFolder = targetFolder + "\\" + year + "\\" + month;

                if (!Directory.Exists(fullTargetFolder))
                {
                    Directory.CreateDirectory(fullTargetFolder);
                }

                var newPath = fullTargetFolder + "\\" + file.Name;

                var newName = RenamingEngine.IncrementFilename(newPath);
                file.MoveTo(newName);
            }
        }

        public static void SortOutVideos(string folder, string sortedVideosFolder, bool previewOnly)
        {
            //check to make sure inputs are different so we don't loop infinitely

            // Take a snapshot of the file system.  
            DirectoryInfo dir = new DirectoryInfo(folder);

            // This method assumes that the application has discovery permissions  
            // for all folders under the specified path.  
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (var file in fileList)
            {
                if (file.Name.Contains(".mov", StringComparison.CurrentCultureIgnoreCase) || file.Name.Contains(".mp4", StringComparison.CurrentCultureIgnoreCase))
                {
                    var newFilePath = file.FullName.Replace(folder, sortedVideosFolder);
                    newFilePath = RenamingEngine.IncrementFilename(newFilePath);
                    Console.WriteLine($"OLD: {file.FullName}");
                    Console.WriteLine($"NEW: {newFilePath}");

                    if (!previewOnly)
                    {
                        var directoryName = Path.GetDirectoryName(newFilePath);
                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        file.MoveTo(newFilePath); 
                    }
                }
            }
        }
    }
}
