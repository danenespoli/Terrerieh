using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
namespace Terrerieh___Culminating
{
    class TextureCollection
    {

        public Dictionary<string, BitmapImage> assets = new Dictionary<string, BitmapImage>();
        string baseFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Terrerieh\\textures\\";

        public TextureCollection()
        {

            //MessageBox.Show(baseFilePath);

            try
            {
                foreach(var v in Directory.GetFiles(baseFilePath)){
                    try {
                        if (v.ToString().Contains(".tif"))
                        {
                            string name = v.Replace(baseFilePath, "").Replace(".tif", "").ToLower();
                            assets.Add(name, new BitmapImage(new Uri(v)));
                        }
                        if (v.ToString().Contains(".png"))
                        {
                            string name = v.Replace(baseFilePath, "").Replace(".png", "").ToLower();
                            assets.Add(name, new BitmapImage(new Uri(v)));

                        }
                    }
                    catch{}
                }

                if(assets.Count == 0)
                {
                    MessageBox.Show("Error (0): place textures in 'Documents\\Terrerieh\\textures'");
                }

            }
            catch (Exception e) {

                MessageBox.Show("Error (1): place textures in 'Documents\\Terrerieh\\textures'" + "\r\n" + e.ToString() + "\r\n" + baseFilePath);
            
            }

        }

    }
}
