using System.Collections.Generic;
using System.Management;

namespace Utils.Grapics
{
    public class NVIDIA
    {

        public static List<string> GetDevices()
        {
            List<string> devices = new();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (ManagementObject mo in searcher.Get())
                {
                    PropertyData currentBitsPerPixel = mo.Properties["CurrentBitsPerPixel"];
                    PropertyData description = mo.Properties["Description"];
                    if (currentBitsPerPixel != null && description != null)
                    {
                        var desc = description.Value.ToString();
                        if (desc != null && desc.ToLower().Contains(nameof(NVIDIA).ToLower()))
                            devices.Add(description.Value.ToString());
                    }
                }
            }
            catch { }

            return devices;
        }
    }
}
