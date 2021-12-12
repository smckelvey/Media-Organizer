using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Linq;

namespace Application.MediaOrganizer.Engines
{
    public class FileTagsEngine
    {
        public static DateTime? GetDateTaken(string filename)
        {
            try
            {
                // Read all metadata from the image
                var directories = ImageMetadataReader.ReadMetadata(filename);

                // Find the so-called Exif "SubIFD" (which may be null)
                var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

                // Read the DateTime tag value
                var dateTime = subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);

                return dateTime;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetCameraModel(string filename)
        {
            try
            {
                // Read all metadata from the image
                var directories = ImageMetadataReader.ReadMetadata(filename);

                // Find the so-called Exif "SubIFD" (which may be null)
                var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

                // Read the DateTime tag value
                var camera = subIfdDirectory?.Parent?.Tags.Where(t => t.Name == "Model").Select(t => t.Description).FirstOrDefault();

                return camera;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
