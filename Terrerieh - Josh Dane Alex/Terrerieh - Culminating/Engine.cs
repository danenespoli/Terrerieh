using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Terrerieh___Culminating
{
    static class Engine
    {
        //various methods used in the game logic

        public enum Direction { None, Left, Right, Up, Down}

        public static int[] GetArrayLocation(double x = 0, double y = 0) {

            //returns the array location of the block that occupies a specific x/y coordinate pair as an array
            //output would look like {int(xLocation), int(yLocation)}
            //
            //the output can be used in the following manner:
            //      int[] arrayLocation = getArrayLocation(190, 250) // sample coordinates
            //      world.blockArray[arrayLocation[0], arrayLocation[1]].doSomething();

            int[] array = {Convert.ToInt32(Math.Floor(x / 25)), Convert.ToInt32(Math.Floor(y / 25))};

            return array;
                
        }

        public static int[] GetArrayLocation(Point location)
        {

            //overload of the first GetArrayLocation method

            return GetArrayLocation(location.X, location.Y);

        }

        public static object GetNewBlock_ofType(Type blockType) {

            //returns a block of a specific block type  

            return Activator.CreateInstance(blockType);

        }

        public static Drill GetNewDrill_ofType(Type drillType)
        {

            return (Drill) Activator.CreateInstance(drillType);
        
        }

       public static double GetDistance(Point a, Point b){

           //returns the distance, in relation to whatever units are provided between two points
           //used for unit collision.

           return Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2));

        }

       public static bool GetDrillMiningValidityOfBlocksIfYouBoughtTheRightDrill(Drill thisIsTheDrillThatThePlayerHas, Block thisIsTheBlockThatThePlayerIsCurrentlyAttemptingToRemoveFromTheWorldExceptTheyMightNotHaveTheRightDrill)
       {
           foreach (var thisIsOneOfTheBlockOptionsInTheListOfValidBlocksInTheDrill in thisIsTheDrillThatThePlayerHas.mineableBlocks) {
               if (thisIsTheBlockThatThePlayerIsCurrentlyAttemptingToRemoveFromTheWorldExceptTheyMightNotHaveTheRightDrill.GetType() == thisIsOneOfTheBlockOptionsInTheListOfValidBlocksInTheDrill)
               {
                   return true;
               }
           }
           return false;
       }
    }
}
