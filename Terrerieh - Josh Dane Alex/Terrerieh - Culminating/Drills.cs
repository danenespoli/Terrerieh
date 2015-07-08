using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrerieh___Culminating
{
    public class Drill
    {

        public List<Type> mineableBlocks = new List<Type>();
        public int price;
        public string name;

        public Drill()
        { 
        
            mineableBlocks.Add(typeof(Block_dirt));
            mineableBlocks.Add(typeof(Block_stone));
            mineableBlocks.Add(typeof(Block_scrapMetal));
        }


    }


    public class Drill_Scrapmetal : Drill
    {
        public Drill_Scrapmetal()
        {
            
            
        }
    }

    public class Drill_CoalBronze : Drill
    {
        public Drill_CoalBronze() 
        {
            mineableBlocks.Add(typeof(Block_coal));
            mineableBlocks.Add(typeof(Block_bronze));
            price = 500;
            name = "Coalite-Bronzide Drill";
        }
    }

    public class Drill_IronGold : Drill
    {
        public Drill_IronGold()
        {
            mineableBlocks.Add(typeof(Block_coal));
            mineableBlocks.Add(typeof(Block_bronze));
            mineableBlocks.Add(typeof(Block_iron));
            mineableBlocks.Add(typeof(Block_gold));
            price = 1000;
            name = "Ironium-Goldine Drill";
        }
    }

    public class Drill_DiamondVarnium : Drill
    {
        public Drill_DiamondVarnium()
        {
            mineableBlocks.Add(typeof(Block_coal));
            mineableBlocks.Add(typeof(Block_bronze));
            mineableBlocks.Add(typeof(Block_iron));
            mineableBlocks.Add(typeof(Block_gold));
            mineableBlocks.Add(typeof(Block_diamond));
            mineableBlocks.Add(typeof(Block_varnium));
            price = 2500;
            name = "The Godliest of Drills";
        }
    }

    public class Drill_Max : Drill
    {
        public Drill_Max()
        {
            mineableBlocks.Add(typeof(Block_coal));
            mineableBlocks.Add(typeof(Block_bronze));
            mineableBlocks.Add(typeof(Block_iron));
            mineableBlocks.Add(typeof(Block_gold));
            mineableBlocks.Add(typeof(Block_diamond));
            mineableBlocks.Add(typeof(Block_varnium));
            price = 0;
            name = "You have the best drill";
        }
    }

}
