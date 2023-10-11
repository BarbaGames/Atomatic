using System.IO;
using System.Text;
using UnityEngine;

namespace Progress
{
    public class FileHandler
    {
        #region CONSTANT_FIELDS
        private const string dataFolderName = "/data";
        private const string fileSuffix = ".dat";
        private static readonly string directory = Application.persistentDataPath + dataFolderName;
        #endregion

        #region PUBLIC_METHODS
        public static void SaveFile(string id, string jsonData)
        {
            string path = GetPath(id);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            File.WriteAllBytes(path, data);
        }
    
        public static bool TryLoadFileRaw(string id, out string data, string subDirectory = null)
        {
            string path = GetPath(id);
            if (File.Exists(path))
            {
                byte[] bytesData = File.ReadAllBytes(path);
                data = Encoding.UTF8.GetString(bytesData);
                return true;
            }

            data = string.Empty;
            return false;
        }

        public static void DeleteAllFiles()
        {
            foreach (string directory in Directory.GetDirectories(Application.persistentDataPath))
            {
                DirectoryInfo data_dir = new DirectoryInfo(directory);
                data_dir.Delete(true);
            }

            foreach (string file in Directory.GetFiles(Application.persistentDataPath))
            {
                FileInfo fileInfo = new FileInfo(file);
                fileInfo.Delete();
            }
        }

        public static void DeleteFile(string id, string subDirectory = null)
        {
            string path = GetPath(id);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static bool TryDeleteDirectory(string path)
        {
            bool directoryExist = Directory.Exists(path);

            if (directoryExist)
            {
                Directory.Delete(Application.persistentDataPath + path, true);
            }

            return directoryExist;
        }

        public static bool FileExist(string id) { return File.Exists(GetPath(id)); }
        #endregion

        #region PRIVATE_METHODS
        private static string GetPath(string id)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = directory;

            path += "/" + id;
            if (!path.Contains(fileSuffix))
            {
                path += fileSuffix;
            }

            return path;
        }
        #endregion
    }
}