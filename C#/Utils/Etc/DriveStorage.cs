using System.IO;
using System.Linq;

namespace Utils.Etc
{
    public class DriveStorage
    {
        /// <summary>
        /// GB 단위
        /// </summary>
        /// <param name="drive"></param>
        /// <returns></returns>
        public static (double percent, string freesize, string usesize, string totalsize) GetAvailableStorage(string drive)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            double percent = 0;
            string freesize = "empty";
            string usesize = "empty";
            string totalsize = "emptry";
            drive += @":\";
            var tmp = allDrives.FirstOrDefault(x => x.Name == drive);
            if (tmp != null)
            {
                double availFreeSpaceGB = (double)tmp.AvailableFreeSpace / 1024 / 1024 / 1024;
                double totalSizeGB = (double)tmp.TotalSize / 1024 / 1024 / 1024;


                percent = 100 - (100 * (double)tmp.AvailableFreeSpace / tmp.TotalSize);
                freesize = (availFreeSpaceGB).ToString("F1");
                totalsize = (totalSizeGB).ToString("F1");
                usesize = (totalSizeGB - availFreeSpaceGB).ToString("F1");

            }

            return (percent, freesize, usesize, totalsize);
        }
    }
}
