using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Terrerieh___Culminating
{

    class World
    {

        public Block[,] blockArray;    //stores all blocks

        public TextureCollection textures;
        
        Grid targetGrid;
        bool targetMoving = false;
        
        public Player player;

        public World(int sizeX, int sizeY, Grid grid, TextureCollection a) 
        {

            blockArray = new Block[sizeX, sizeY];
            targetGrid = grid;
            textures = a;

            Generate();
            AddPlayer();

        }

        public void AddPlayer() { 
        
            player = new Player(this);
            player.SetPosition(new Point(10, 9 * 25));
            player.SetBackground(textures.assets["playerright"]);
            targetGrid.Children.Add(player);
            Canvas.SetZIndex(player, 99);
            
        }

        public void Generate()
        {

            CryptoRandom rnd = new CryptoRandom();

            for (int y = 0; y < blockArray.GetLength(1); y++)
            {

                for (int x = 0; x < blockArray.GetLength(0); x++)
                {

                    Point blockLocation = new Point(x * 25, y * 25);

                    if (y < 12 || y == 199) //no random generation at this level
                    {

                        if (y <= 10)
                        {
                            //sky blocks at levels one to ten
                            addNewBlock(new Block_air(blockLocation), x, y);
                        }

                        if (y == 11)
                        {

                            Block_dirt b = new Block_dirt(blockLocation, textures.assets["grass"]);
                           // b.setGrassState(true);
                            addNewBlock(b, x, y);
                        }

                        if (y == 199) {

                            Block_bedrock b = new Block_bedrock(blockLocation, textures.assets["bedrock"]);
                            addNewBlock(b, x, y);
                        
                        }

                    }
                    else 
                    {
                        //begin probability calculations
                        int blockRnd;

                        Block newBlock = new Block_dirt(blockLocation, textures.assets["dirt"]);

                        if (y > 20)
                        {
                            //stone will start spawning at 20 blocks
                            blockRnd = rnd.Next(0, 100);

                            if (y + (blockRnd / 5) > 35)
                            {

                                newBlock = new Block_stone(blockLocation, textures.assets["stone"]);

                            }
                        }

                        if (y >= 25 && y <= 52)
                        {
                            //7% chance
                            blockRnd = rnd.Next(0, 100);
                            if (blockRnd <= 7)
                            {

                                newBlock = new Block_scrapMetal(blockLocation, textures.assets["scrapmetal"]);
                            
                            }
                        }

                        if (y >= 45 && y <= 75) {

                            //5% chance
                            blockRnd = rnd.Next(0, 125);

                            if (blockRnd <= 5) {

                                newBlock = new Block_coal(blockLocation, textures.assets["coal"]);
                            
                            }                  
                        }

                        if (y >= 60 && y <= 100) { 
                            
                            //5% chance
                            blockRnd = rnd.Next(0, 150);
                            if (blockRnd <= 5)
                            {

                                newBlock = new Block_bronze(blockLocation, textures.assets["bronze"]);
                            
                            }
                        }

                        if (y >= 90 && y <= 130)
                        {
                            //3% chance

                            blockRnd = rnd.Next(0, 200);
                            if (blockRnd <= 3) 
                            { 

                               newBlock = new Block_iron(blockLocation, textures.assets["iron"]);

                            }
                        }

                        if (y > 125) {
                            //2 %
                            blockRnd = rnd.Next(0, 250);
                            if (blockRnd <= 2) 
                            { 
                            
                                newBlock = new Block_gold(blockLocation, textures.assets["gold"]);
                            
                            }
                        }

                        if (y > 150) {
                            
                            blockRnd = rnd.Next(0, 500);
                            if (blockRnd <= 2)
                            {

                                newBlock = new Block_diamond(blockLocation, textures.assets["diamond"]);

                            }
                        
                        }

                        if (y > 175)
                        {

                            blockRnd = rnd.Next(0, 1000);
                            if (blockRnd <= 2)
                            {

                                newBlock = new Block_varnium(blockLocation, textures.assets["varnium"]);

                            }

                        }




                            
                        
                        //add whatever the randomly determined block was

                        addNewBlock(newBlock, x, y);
                    }
                } // end for x
            } // end for y
        }// end method
            

  

        public int GetFloor(Player player)
        {

            //returns the World > BlockArray index of the nearest solid block below the player

            //get the location in the array of the left and right side of the block (since the player can be partially on multiple blocks at once)
            int[] arrayLocationL = Engine.GetArrayLocation(player.Margin.Left + 3, player.Margin.Top);
            int[] arrayLocationR = Engine.GetArrayLocation((player.Margin.Left + player.Width) - 3, player.Margin.Top);

            int floorL = 0;
            int floorR = 0;

            //finds the nearest solid block below the left side of the player
            try
            {
                for (int y = arrayLocationL[1]; y < blockArray.GetLength(1); y++)
                {

                    if (blockArray[arrayLocationL[0], y].penetrable == false)
                    {
                        floorL = y;
                        break;
                    }
                }

                //finds the nearest solid block below the right side of the player

                for (int y = arrayLocationL[1]; y < blockArray.GetLength(1); y++)
                {
                    if (blockArray[arrayLocationR[0], y].penetrable == false)
                    {
                        floorR = y;
                        break;
                    }
                }

                //returns the nearest block of the two

                if (floorL < floorR)
                {
                    return floorL;
                }
                else
                {
                    return floorR;
                }
            }
            catch
            {
                return -1;
            }
        }

        public int FloorFromPoint(double pointX, double pointY)
        {
            //get the location in the array of the left and right side of the block (since the player can be partially on multiple blocks at once)
            int[] arrayLocation = Engine.GetArrayLocation(pointX, pointY);

            //finds the nearest solid block below the point

            try
            {
                for (int y = arrayLocation[1]; y < blockArray.GetLength(1); y++)
                {
                    if (blockArray[arrayLocation[0], y].penetrable == false)
                    {
                        return y;
                    }
                }
            }
            catch
            {
                return -1;
            }
            return -1;
        }

        public int CheckSide(Player player, int side)
        {

            //returns the never exceed X-point for each side of the player, or null

            int[] playerArrayLocationL = Engine.GetArrayLocation(player.Margin.Left, player.Margin.Top);
            int[] playerArrayLocationR = Engine.GetArrayLocation(player.Margin.Left + player.ActualWidth, player.Margin.Top);
            try
            {
                switch (side)
                {

                    case 0:

                        Boolean topLeft_penetrable = blockArray[playerArrayLocationL[0] - 1, playerArrayLocationL[1]].penetrable;
                        Boolean botLeft_penetrable = blockArray[playerArrayLocationL[0] - 1, playerArrayLocationL[1] + 1].penetrable;

                        if (topLeft_penetrable == false || botLeft_penetrable == false)
                        {

                            return Convert.ToInt32(Math.Abs(player.Margin.Left - (playerArrayLocationL[0] * 25)));

                        }

                        break;

                    case 1:

                        Boolean topRight_penetrable = blockArray[playerArrayLocationR[0] + 1, playerArrayLocationR[1]].penetrable;
                        Boolean botRight_penetrable = blockArray[playerArrayLocationR[0] + 1, playerArrayLocationR[1] + 1].penetrable;

                        if (topRight_penetrable == false || botRight_penetrable == false)
                        {
                            // means we can't move into the next block

                            int nextArrayLoc = playerArrayLocationR[0] + 1;
                            int rightBloc_xPx = (nextArrayLoc * 25) - 1;
                            int distance = (rightBloc_xPx - Convert.ToInt32(player.Margin.Left + player.Width)) % 25;

                            return distance;
                        }

                        break;
                }
            }
            catch
            {
                return -2;
            }
            return -1;
        }

        public void PlaceBlock(Type newBlockType, Block target)
        {

            if (player.Cargo < player.CargoBayCapacity || 
                target.GetType() == typeof(Block_dirt) || 
                target.GetType() == typeof(Block_stone) || 
                target.GetType() == typeof(Block_air))
            {

                //places a block of a specified type at a specified location.
                if (target.penetrable || newBlockType == typeof(Block_air))
                {
                    int[] blockArrayLoc = (Engine.GetArrayLocation(target.Margin.Left, target.Margin.Top));

                    //replace block in worldarray and grid

                    targetGrid.Children.Remove(target);
                    blockArray[blockArrayLoc[0], blockArrayLoc[1]] = (Block)Engine.GetNewBlock_ofType(newBlockType);

                    blockArray[blockArrayLoc[0], blockArrayLoc[1]].setPosition(new Point(blockArrayLoc[0] * 25, blockArrayLoc[1] * 25));
                    targetGrid.Children.Add(blockArray[blockArrayLoc[0], blockArrayLoc[1]]);
                    Canvas.SetZIndex(blockArray[blockArrayLoc[0], blockArrayLoc[1]], 10);

                    if (newBlockType == typeof(Block_air))
                    {
                        blockArray[blockArrayLoc[0], blockArrayLoc[1]].penetrable = true;
                    }

                }
            }

            if (target.GetType() != typeof(Block_dirt) &&
                target.GetType() != typeof(Block_stone) &&
                target.GetType() != typeof(Block_air) &&
                player.Cargo < player.CargoBayCapacity)
            {
                player.Cargo += 1;
            }

        }

        public Block GetBlockFromArray(int[] coords)
        {

            return blockArray[coords[0], coords[1]];
        
        }

        public Block GetBlockAtLocation(Point p) 
        {

            try 
            { 
                return blockArray[Convert.ToInt32(Math.Floor(p.X / 25)), Convert.ToInt32(Math.Floor(p.Y / 25))]; 
            }
            catch 
            { 
                return null; 
            }
        
        }

        public Block GetBlockAtLocation(int x, int y)
        {

            return GetBlockAtLocation(new Point(x, y));
        
        }

        public void Move(Movement m)
        {
            if (targetMoving == false)
            {
                targetMoving = true;
                //moves the grid, so that the world.player can never leave the form
                ThicknessAnimation ta = new ThicknessAnimation();
                ta.From = targetGrid.Margin;
                ta.To = new Thickness(targetGrid.Margin.Left + m.Displacement.X, targetGrid.Margin.Top + m.Displacement.Y, 0, 0);
                ta.AccelerationRatio = m.Acceleration;
                ta.DecelerationRatio = m.Deceleration;
                ta.Completed += (o, i) => { targetMoving = false; };
                targetGrid.BeginAnimation(Grid.MarginProperty, ta);
            }
            //comment
        }

        private void addNewBlock(Block b, int arrayLocX, int arrayLocY)
        {
            //adds a new block to the target grid and adds it to the array

            blockArray[arrayLocX, arrayLocY] = b;

            targetGrid.Children.Add(blockArray[arrayLocX, arrayLocY]);
        
        }

    }
}
