using System;
using System.IO;
using System.Text;

namespace Utils.Extentions
{
    public static class ArrayEx
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);

            return result;
        }

        public static void SaveToFile(this byte[] bytes, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        public static byte[] LoadFromFile(this byte[] bytes, string path)
        {
            byte[] tmp = new byte[0];
            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (StreamReader sr = new StreamReader(fs)) 
            {
                tmp = Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }

            return tmp;
        }
    }
}
