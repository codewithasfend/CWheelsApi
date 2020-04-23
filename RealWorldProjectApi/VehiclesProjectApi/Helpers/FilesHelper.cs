using System.IO;

namespace VehiclesProjectApi.Helpers
{
    /// <summary>
    /// FilesHelper: allows user to upload photos of vehicles.
    /// </summary>
    public static class FilesHelper
    {
        /// <summary>
        /// Gives user feature to upload vehicle photos.
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool UploadPhoto(MemoryStream memoryStream, string folderName, string fileName)
        {
            try
            {
                memoryStream.Position = 0;
                var path = Path.Combine(folderName, fileName);
                File.WriteAllBytes(path, memoryStream.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}