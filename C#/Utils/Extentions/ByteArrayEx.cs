using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Utils.Extentions
{
    public static class ByteArrayEx
    {

        public static byte[] ToByteArray(this Bitmap bitmap)
        {
            BitmapData bmpdata = null;
            try
            {
                bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                int numbytes = bmpdata.Stride * bitmap.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;

                Marshal.Copy(ptr, bytedata, 0, numbytes);

                return bytedata;
            }
            finally
            {
                if (bmpdata != null)
                    bitmap.UnlockBits(bmpdata);
            }

        }

        public static Bitmap ToImage(this byte[] data, int ImageWidth, int ImageHeight, PixelFormat pixelformat = PixelFormat.Format24bppRgb)
        {
            int addWidth = ImageWidth % 4;
            int addHeight = ImageHeight % 4;
            int convertWidth = ImageWidth;
            int convertHeight = ImageHeight;
            byte[] imageBytes = data;

            //이미지 사이즈 조정이 필요한 경우
            if (addWidth != 0 || addHeight != 0)
            {
                //변환 사이즈 적용
                convertWidth -= addWidth;
                convertHeight -= addHeight;

                int bit = pixelformat == PixelFormat.Format24bppRgb ? 3 : 4;
                int stride = convertWidth * bit;
                int totalPixel = ImageWidth * ImageHeight * bit;
                byte[] outputPixels = new byte[stride * convertHeight];
                for (int line = 0; line < convertHeight - 1; line++)
                {
                    int startIndex = line * ImageWidth * bit;
                    var endIndex = line * stride;
                    if (totalPixel > startIndex + stride)
                        Array.Copy(data, startIndex, outputPixels, endIndex, stride);
                }
                imageBytes = outputPixels;
            }

            Bitmap bitmap = new Bitmap(convertWidth, convertHeight, pixelformat);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, convertWidth, convertHeight), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(imageBytes, 0, bitmapData.Scan0, imageBytes.Length);
            bitmap.UnlockBits(bitmapData);

            // parameter가 다를경우엔 아래에서 새로 만들어서 반환
            if (pixelformat != PixelFormat.Format24bppRgb)
            {
                return ConvertTo24bpp(bitmap);
            }
            
            return bitmap;
        }

        public static Bitmap ConvertTo24bpp(Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }

        public static Bitmap ResizeImage(this Bitmap image, Rectangle to)
        {
            if (image.Width == to.Width && image.Height == to.Height) return image;

            var destImage = new Bitmap(to.Width, to.Height, image.PixelFormat);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, to, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// value가 낮을수록 화질이 구려짐 (100L 해도 7배 줄어듬)
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="orgWidth"></param>
        /// <param name="orgHeight"></param>
        /// <param name="value"> defaul 30L </param>
        /// <returns></returns>
        public static byte[] EncodingJpeg(this byte[] bytes, int orgWidth, int orgHeight, long value = 50L)
        {
            byte[] output = null;
            using (var bmp = bytes.ToImage(orgWidth, orgHeight))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                using (EncoderParameters myEncoderParameters = new EncoderParameters(1))
                using (myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, value))
                {
                    using (var mem = new MemoryStream())
                    {
                        int filename = DateTime.Now.Second;
                        //bmp.Save($@"C:\Users\shjun\Documents\Fax\{filename}.jpg", jpgEncoder, myEncoderParameters);
                        //Console.WriteLine($"{filename} : {orgWidth}x{orgHeight}");
                        bmp.Save(mem, jpgEncoder, myEncoderParameters);
                        output = mem.ToArray();
                    }
                }
            }

            return output;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static Bitmap JpegBytesToBitmap(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;

            using (var ms = new MemoryStream(bytes))
            {
                var bmp = Image.FromStream(ms) as Bitmap;
                if (bmp != null)
                {
                    using var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp);
                    var convertedBitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);

                    bmp.Dispose();
                    return convertedBitmap;
                }

                return bmp;
            }


        }
    }
}

