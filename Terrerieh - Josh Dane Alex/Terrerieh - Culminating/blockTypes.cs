using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace Terrerieh___Culminating
{
    class Block_air : Block
    {

        public Block_air(Point location)
            : base(location, null, true)
        {
            this.Background = new SolidColorBrush(Color.FromRgb(169, 228, 232));
            this.displayName = "Air";
        }
        public Block_air()
        {
            this.Background = new SolidColorBrush(Color.FromRgb(169, 228, 232));
            this.penetrable = false;
            this.displayName = "Air";
        }

    }
    class Block_dirt : Block
    {

        public bool grass = false;

        public Block_dirt(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
            this.displayName = "Dirt";
            if (texture == null) {
                this.Background = new SolidColorBrush(Color.FromRgb(88, 76, 65));
            }
            
        grass = false;
        }

        public Block_dirt() 
        {
            this.displayName = "Dirt";
            this.Background = new SolidColorBrush(Color.FromRgb(88, 76, 65));
        
        }

        public void setGrassState(bool isBlockGrass)
        {

            if (isBlockGrass)
            {
                    this.Background = new SolidColorBrush(Color.FromRgb(42, 109, 39));
            
                    grass = true;
            }
            else
            {
                this.Background = new SolidColorBrush(Color.FromRgb(88, 76, 65));
                grass = false;
            }

        }
    }
    

    class Block_stone : Block 
    {

        public Block_stone(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
            if (texture == null)
            {
                this.Background = new SolidColorBrush(Color.FromRgb(87, 80, 80));
            }
            this.displayName = "Stone";
        }

        public Block_stone()
        {
            //this.Background = new SolidColorBrush(Color.FromRgb(87, 80, 80));
            this.displayName = "Stone";
        }
    
    }

    class Block_scrapMetal : Block
    {

        public Block_scrapMetal(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
                value = 35;
                //this.Background = new SolidColorBrush(Color.FromRgb(43, 23, 1));
                this.displayName = "Scrap Metal";
     
        }

        public Block_scrapMetal()
        {
            value = 35;
            this.Background = new SolidColorBrush(Color.FromRgb(43, 23, 1));
            this.displayName = "Scrap Metal";
        }

    }
   
    class Block_coal : Block
    {

        public Block_coal(Point location, BitmapImage texture)
            : base(location, texture, false)
       
            {
                value = 50;
                //this.Background = new SolidColorBrush(Color.FromRgb(55, 145, 13));
                this.displayName = "Coal";
            }
       

        public Block_coal()
        {
            value = 50;
            //this.Background = new SolidColorBrush(Color.FromRgb(55, 145, 13));
            this.displayName = "Coal";
        }

    }
    class Block_iron : Block
    {

        public Block_iron(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
            value = 75;
            //this.Background = new SolidColorBrush(Color.FromRgb(56, 23, 245));
            this.displayName = "Iron";
        }

        public Block_iron()
        {
            value = 75;
             //this.Background = new SolidColorBrush(Color.FromRgb(56, 23, 245));
             this.displayName = "Iron";
        }

    }
    class Block_bronze : Block
    {

        public Block_bronze(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
            value = 100;
            //this.Background = new SolidColorBrush(Color.FromRgb(87, 45, 123));
            this.displayName = "Bronze";
        }

        public Block_bronze()
        {
            value = 100;
            this.Background = new SolidColorBrush(Color.FromRgb(87, 45, 123));
            this.displayName = "Bronze";
        }

    }
    class Block_gold : Block 
    {

        public Block_gold(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
            value = 175;
            //this.Background = new SolidColorBrush(Color.FromRgb(255, 215, 00));
            this.displayName = "Gold";
        }

        public Block_gold()
        {
            value = 175;
            this.Background = new SolidColorBrush(Color.FromRgb(255, 215, 00));
            this.displayName = "Gold";
        }

    }

    class Block_diamond : Block 
    {
        public Block_diamond(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
            value = 300;
            //this.Background = new SolidColorBrush(Color.FromRgb(56, 199, 207));
            this.displayName = "Diamond";
        }

        public Block_diamond()
        {
            value = 300;
            this.Background = new SolidColorBrush(Color.FromRgb(56, 199, 207));
            this.displayName = "Diamond";
        }
    }
    class Block_varnium : Block
    {
        public Block_varnium(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
            value = 1500;
            //this.Background = new SolidColorBrush(Color.FromRgb(74, 225, 125));
            this.displayName = "Varnium";
        }

        public Block_varnium()
        {
            value = 1500;
            //this.Background = new SolidColorBrush(Color.FromRgb(74, 225, 125));
            this.displayName = "Varnium";
        }
    }
    class Block_bedrock : Block {

        public Block_bedrock(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
            //this.Background = new SolidColorBrush(Color.FromRgb(25, 0, 51));
            this.displayName = "Bedrock";
        }

        public Block_bedrock()
        {
            //this.Background = new SolidColorBrush(Color.FromRgb(25, 0, 51));
            this.displayName = "Bedrock";
        }
    }

    class Block_grass : Block {

        public Block_grass(Point location, BitmapImage texture)
            : base(location, texture, false)
        {
            //this.Background = new SolidColorBrush(Color.FromRgb(46, 153, 54));
            this.displayName = "Grass";
        }

        public Block_grass()
        {
            this.Background = new SolidColorBrush(Color.FromRgb(46, 153, 54));
            this.displayName = "Grass";
        }
    }
}
