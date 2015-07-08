using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace Terrerieh___Culminating
{
    abstract class Block : Canvas //this class must be inherited by another block
    {
        public int weight = 20;
        public string displayName;
        public int value;
        public Boolean penetrable; //affects if the player can enter the block

        public Block() 
        {
            
            this.Width = 26;
            this.Height = 26;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            penetrable = false;
        }

        public Block(Point location, BitmapImage texture, Boolean isPenetrable) 
        {
            this.Width = 26;
            this.Height = 26; 
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            //this.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(click);
            this.Margin = new Thickness(location.X, location.Y,0,0);
            penetrable = isPenetrable;
           
            if (texture != null) {

                this.Background = new ImageBrush(texture);            
            }
        }

        public void setPosition(Point location) 
        {

            this.Margin = new Thickness(location.X, location.Y, 0, 0);
        
        }
    }
}
