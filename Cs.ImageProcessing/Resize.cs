using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Cs.ImageProcessing
{
    public class Resize
    {
        public static void ResizeAlgorithm(out int newWidth, out int newHeight, out int posX, out int posY,
            int width, int height, int imageWidth, int imageHeight)
        {
            double ratioX = (double)width / (double)imageWidth;
            double ratioY = (double)height / (double)imageHeight;
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            newWidth = Convert.ToInt32(imageWidth * ratio);
            newHeight = Convert.ToInt32(imageHeight * ratio);
            posX = Convert.ToInt32((width - (imageWidth * ratio)) / 2);
            posY = Convert.ToInt32((height - (imageHeight * ratio)) / 2);
        }

        /// <summary>
        /// Resizes an image
        /// </summary>
        /// <param name="path">path to file</param>
        /// <param name="originalFile">file to resize</param>
        /// <param name="width">desired width</param>
        /// <param name="height">desired height</param>
        /// <param name="format">image format</param>
        /// <returns>returns string equivalent to image</returns>
        public static String64 ResizeImage(string path, string originalFile, int width, int height, ImageFormat format)
        {
            Image image = Image.FromFile(originalFile);
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            System.Drawing.Image thumbnail = new Bitmap(width, height);
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            int newWidth;
            int newHeight;
            int posX;
            int posY;
            ResizeAlgorithm(out newWidth, out newHeight, out posX, out posY, width, height, imageWidth, imageHeight);

            graphic.Clear(Color.Transparent);
            graphic.DrawImage(image, posX, posY, newWidth, newHeight);

            System.Drawing.Imaging.ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            EncoderParameters encoderParameters;
            encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            //string saveFile = String.Format("{0}thumbnail_{1}.bmp", path, Path.GetFileNameWithoutExtension(originalFile));
            //thumbnail.Save(saveFile, info[1], encoderParameters);

            return ConvertToByteArray(thumbnail, format);
        }

        public static String64 ConvertToByteArray(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] data = ms.ToArray();

                return (String64)Convert.ToBase64String(data);
            }
        }

        public static Image ToImage(String64 str)
        {
            byte[] data = Convert.FromBase64String((string)str);
            MemoryStream ms = new MemoryStream(data, 0, data.Length);
            ms.Write(data, 0, data.Length);
            Image image = Image.FromStream(ms, true);

            return image;
        }
    }
}
