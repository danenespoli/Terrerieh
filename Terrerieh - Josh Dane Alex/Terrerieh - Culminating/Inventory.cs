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
    class Inventory
    {
        //stores all of the blocks that a player has collected

        //store the player inventory in a dictionary with a quantity associated with each type of block

        public Dictionary<Type, int> Items = new Dictionary<Type, int>();

        //adds a block to the players inventory

        public void Add(Type t, int amount)
        {
            if (t != typeof(Block_dirt) && t != typeof(Block_stone) && t != typeof(Block_air))
            {
                if (Items.ContainsKey(t))
                {

                    Items[t] += amount; //increases the count of the block in the players inventory by the specified amount if that type already exists int the dictionary

                }
                else
                {

                    Items.Add(t, amount); //adds a new type to the dictionary with the specified value of blocks

                }
            }
        }
        public void Remove(Type t, int amount) {

            if (Items.ContainsKey(t))
            {

                Items[t] -= amount; //decreases the count of the block in the players inventory by the specified amount if that type already exists int the dictionary

            }        
        }
    }
 }