using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Utils.Extentions
{
    public static class BitmapEx
    {
        public static byte[] GetBitmapData(this Bitmap bmp, PixelFormat pixcelFormat)
        {
            var bitmapData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, pixcelFormat);
            try
            {
                var length = bitmapData.Stride * bitmapData.Height;
                var data = new byte[length];
                Marshal.Copy(bitmapData.Scan0, data, 0, length);
                return data;
            }
            finally
            {
                bmp.UnlockBits(bitmapData);
            }
        }

        public static byte[] ConvertBitmapToByteArray(this Bitmap bitmap)
        {
            byte[] result = null; if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, bitmap.RawFormat);
                result = stream.ToArray();
            }
            else
            {
                Console.WriteLine("Bitmap is null.");
            }

            return result;
        }



        public static string ToSaveImage(this Bitmap bmp, string path)
        {
            string savedPath = "";
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var imgPath = $@"{path}\{DateTime.Now.Ticks}.jpg";
                
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                bmp.Save(imgPath, jgpEncoder, encoderParams);

                savedPath = imgPath;
            }
            catch (Exception e)
            {

            }

            return savedPath;
        }


        public static byte[] EncodingJpeg(this Bitmap bmp, long value = 50L)
        {
            byte[] output = null;
            using (var mem = new MemoryStream())
            {
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, value);
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                //bmp.Save($@"C:\Users\shjun\Pictures\tmp\dd\sampleSaveImage.jpg", jpgEncoder, encoderParams);
                bmp.Save(mem, jpgEncoder, encoderParams);
                output = mem.ToArray();
            }

            return output;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static bool CompareBitmapsFast(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            int bytes = bmp1.Width * bmp1.Height * (Image.GetPixelFormatSize(bmp1.PixelFormat) / 8);

            bool result = true;
            byte[] b1bytes = new byte[bytes];
            byte[] b2bytes = new byte[bytes];

            BitmapData bitmapData1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, bmp1.PixelFormat);
            BitmapData bitmapData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            Marshal.Copy(bitmapData1.Scan0, b1bytes, 0, bytes);
            Marshal.Copy(bitmapData2.Scan0, b2bytes, 0, bytes);

            for (int n = 0; n <= bytes - 1; n++)
            {
                if (b1bytes[n] != b2bytes[n])
                {
                    result = false;
                    break;
                }
            }

            bmp1.UnlockBits(bitmapData1);
            bmp2.UnlockBits(bitmapData2);

            return result;
        }

    }
}
