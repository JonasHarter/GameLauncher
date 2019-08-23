using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Xwt;

namespace Launcher.src
{
    public class Game
    {
        public String name { get; set; }
        public String command { get; set; }
        public byte[] image { get; set; }
        public List<String> tags { get; set; }

        public Game()
        {
            tags = new List<string>();
        }

        internal void SetImage(String path)
        {
            image = ImgToByteArray(Image.FromFile(path));
        }

        internal Xwt.Drawing.Image GetImage()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ByteArrayToImage(image).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return Xwt.Drawing.Image.FromStream(ms);
            }
        }

        private byte[] ImgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, System.Drawing.Imaging.ImageFormat.Png);
                return mStream.ToArray();
            }
        }

        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(mStream);
            }
        }
    }
}
