using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Controls;

namespace Terrerieh___Culminating
{
    class Store : Canvas
    {
        public enum items { FuelTank, CargoBay, Drill }

        public List<Drill> drillList = new List<Drill>();

        public Store()
        {
            drillList.Add(new Drill_Scrapmetal());
            drillList.Add(new Drill_CoalBronze());
            drillList.Add(new Drill_IronGold());
            drillList.Add(new Drill_DiamondVarnium());
            drillList.Add(new Drill_Max());
        }


        public double cargobayCost = 1000;
        public double fueltankCost = 750;
        public double drillCost = 500;



        public void Refuel(Player player)
        {
            //0.05 per unit of fuel
            double fuelCost = 0.05;
            double fuelToBuy = player.FuelTankCapacity - player.Fuel;

            if (player.money > fuelToBuy * fuelCost)
            {
                player.Fuel += fuelToBuy;
                player.money -= Convert.ToInt32(fuelToBuy * fuelCost);
            }
            else
            {
                player.Fuel += player.money / fuelCost;
                player.money = 0;
            }
        }
        
        
        
        public void Buy(items selectedItem, Player player)
        {
            switch (selectedItem)
            {

                case items.CargoBay: //Cargo Bay
                    if (player.money >= cargobayCost)
                    {
                        player.CargoBayCapacity += 10;

                        player.money -= Convert.ToInt32(cargobayCost);
                        cargobayCost = Math.Floor(cargobayCost * 1.5);
                    }
                    break;

                case items.FuelTank: //Fuel Tank
                    if (player.money >= fueltankCost)
                    {
                        player.FuelTankCapacity += Math.Floor(player.FuelTankCapacity * 0.5);

                        player.money -= Convert.ToInt32(fueltankCost);
                        fueltankCost = Math.Floor(fueltankCost * 1.3);
                    }
                    break;

                case items.Drill: //Drill
                    if (player.money >= drillCost &&
                        drillList.Count > 2)
                    {
                        drillList.Remove(drillList[0]);
                        player.playerDrill = drillList[0];
                         
                        player.money -= Convert.ToInt32(drillCost);
                        drillCost = drillList[1].price;
                    }
                    break;

            }       
        }


        public static void SellAllItems(Player p)
        {
            
            foreach (var v in p.inventory.Items) 
            {

                var key = v.Key;

                var block = (Block)Engine.GetNewBlock_ofType(key);

                int blockValue = block.value;

                p.money += blockValue * v.Value;
            
            }

            p.inventory.Items.Clear();
            p.Cargo = 0;
        
        }

        public static void PopulateList(Player p, ListBox l)
        {
            l.Items.Clear();

            foreach (var v in p.inventory.Items)
            {

                l.Items.Add(((Block)Engine.GetNewBlock_ofType(v.Key)).displayName + ":   " + v.Value.ToString());
            
            }
        
        
        }

    }
}
