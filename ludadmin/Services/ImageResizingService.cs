using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace AdminPages.Services
{
    public class ImageResizingService
    {
        public byte[] ImageBytes(Stream fs)
        {
            //----------------------ready Image file--------------------
            byte[] imageBytes;
            using (fs)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((int)fs.Length);
                    imageBytes = ResizeImage(bytes, 350, 400);
                }
            }
            return imageBytes;
            //-----------------------------------------------------------
        }
        private byte[] ResizeImage(byte[] bytes, int new_width, int FileLimitKB)
        {
            int imgSize = bytes.Length / 1024;
            //====================================================================
            if (imgSize > FileLimitKB)
            {
                System.Drawing.Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = System.Drawing.Image.FromStream(ms);
                }
                //==============width and height =========================
                int wt = new_width;
                double x = new_width / Convert.ToDouble(image.Width);
                int ht = Convert.ToInt32(image.Height * x);
                //===============Change image Size========================
                Bitmap new_image = new Bitmap(wt, ht);
                Graphics g = Graphics.FromImage(new_image);
                g.InterpolationMode = InterpolationMode.High;
                g.DrawImage(image, 0, 0, wt, ht);
                //=================== Convert Bitmat to Base64 ============
                Bitmap bmp = new_image;
                string base64String = string.Empty;

                MemoryStream memoryStream = new MemoryStream();
                bmp.Save(memoryStream, ImageFormat.Bmp);

                memoryStream.Position = 0;
                byte[] byteBuffer = memoryStream.ToArray();
                int newSiz = byteBuffer.Length / 1024;
                //==========================================================
                return byteBuffer;
            }
            else
            {
                return bytes;
            }

        }

    }
}