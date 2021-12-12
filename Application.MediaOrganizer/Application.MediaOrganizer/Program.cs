using Application.MediaOrganizer.Engines;

namespace Application.MediaOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFolder = "C:\\OneDrive\\Pictures\\UNSORTED";
            var sortedPictureFolder = "C:\\OneDrive\\Pictures\\FAMILY";
            var sortedVideosFolder = "C:\\OneDrive\\Videos\\Family Movies";
            SortingEngine.SortFilesByDateTaken(inputFolder, sortedPictureFolder, sortedVideosFolder);

            var startFolder = "C:\\OneDrive\\Pictures\\FAMILY"; //Note: must be at the same level as the target folder
            var videosFolder = "C:\\OneDrive\\Videos\\Family Movies";
            var previewMovesOnly = true;
            SortingEngine.SortOutVideos(startFolder, videosFolder, previewMovesOnly);

            var dedupeFolder = "C:\\OneDrive\\Pictures\\FAMILY\\2020\\12";
            var previewDeletesOnly = true;
            DedupeEngine.RemoveDuplicates(dedupeFolder, previewDeletesOnly);

            //rename files to remove (X)
            var renamingFolder = "C:\\OneDrive\\Pictures\\FAMILY";
            var previewRenamesOnly = true;
            RenamingEngine.CleanFileNames(renamingFolder, previewRenamesOnly);
        }
    }
}
