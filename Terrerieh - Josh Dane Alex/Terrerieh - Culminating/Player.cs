using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace Terrerieh___Culminating
{


    class Player : Canvas
    {
        public double money = 50;

        World parentWorld;

        private bool isMoving = false;

        private List<Movement> moveQueue = new List<Movement>();

        public Inventory inventory;

        Vector movementVector = new Vector(0,0);
        int maxMoveSpeed_X = 6;
        int maxMoveSpeed_Y = -7; //negative since a negative movement == upwards
        Engine.Direction currentMoveDir = Engine.Direction.None;

        public double playerWeight = 300;
        public readonly double baseWeight = 300; 
        double engineStrength = 800;

        double accelerationRatio = 1.7;
        
        private DispatcherTimer moveTimer = new DispatcherTimer();

        public Drill playerDrill = new Drill_Scrapmetal();

        public int CargoBayCapacity = 10;
        public int Cargo = 0;
        public double FuelTankCapacity = 2500;
        public double Fuel = 2500;
        

        public Player(World parent)
        {
            inventory = new Inventory();

            
            //sets the initial conditions for the player
            this.Width = 23;
            this.Height = 40;
            this.Background = new SolidColorBrush(Color.FromRgb(65, 128, 200));
            this.VerticalAlignment = VerticalAlignment.Top;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.parentWorld = parent;

            moveTimer.Interval = TimeSpan.FromMilliseconds(16);
            moveTimer.Tick += new EventHandler(moveTimer_Tick);

            moveTimer.Start();
            //this.MouseDown += (o, i) => { StartAnimation(new Movement(new Vector(-1, 0), 1)); }; //debug
        }

        public void SetBackground(BitmapImage i)
        {

            this.Background = new ImageBrush(i);
        
        }

        private void moveTimer_Tick(object sender, EventArgs e)
        {
            accelerationRatio = engineStrength / playerWeight;
            //currentMoveDir = Engine.Direction.None;
            
            Vector moveVector = new Vector();
            
            switch (currentMoveDir)
            { 
                case Engine.Direction.None:

                    moveVector = new Vector(CalculateX(accelerationRatio, Engine.Direction.None).X, CalculateY(false, accelerationRatio).Y);
                    break;
                case Engine.Direction.Up:
                    moveVector = new Vector(CalculateX(accelerationRatio, Engine.Direction.None).X, CalculateY(true, accelerationRatio).Y);
                    break;
                case Engine.Direction.Left:
                    moveVector = new Vector(CalculateX(accelerationRatio, Engine.Direction.Left).X, CalculateY(false, accelerationRatio).Y);
                    this.SetBackground(parentWorld.textures.assets["playerleft"]);
                    break;
                case Engine.Direction.Right:
                    this.SetBackground(parentWorld.textures.assets["playerright"]);
                    moveVector = new Vector(CalculateX(accelerationRatio, Engine.Direction.Right).X, CalculateY(false, accelerationRatio).Y);
                    break;
            }

            StartAnimation(new Movement(moveVector, 16));

            //Application.Current.MainWindow.Title = moveVector.ToString();
            //System.Threading.Thread.SpinWait(100000);
            //MessageBox.Show("movement fired: " + movementVector.ToString());
        }

        public Vector CalculateY(bool enginePower, double accelerationRatio) {

            if (enginePower == false)
            {
                //if (Math.Abs(movementVector.X) > 0)
                //{
                //    movementVector.X *= 0.7;
                //}

                if (GetMaxMovement(Engine.Direction.Down, 2) == 0)
                {
                    movementVector.Y = 0;
                }
                else
                {
                    movementVector.Y += 0.5;
                }

                if (GetMaxMovement(Engine.Direction.Down, 7) < 5)
                {

                    movementVector.Y = 0;
                    SetPosition(new Point(0, 0));
                    return new Vector(0, GetMaxMovement(Engine.Direction.Down, 10));
                }
                else
                {

                    int maxMovementY = GetMaxMovement(Engine.Direction.Down, Convert.ToInt32(movementVector.Y));
                    return new Vector(0, maxMovementY);
                }
            }
            else {

                if (movementVector.Y > maxMoveSpeed_Y) 
                {

                    if (movementVector.Y == 0 && accelerationRatio > 1.0)
                    {
                        movementVector.Y = -0.1;
                    }

                    movementVector.Y += accelerationRatio;

                    if (movementVector.Y > maxMoveSpeed_Y)
                    {
                        movementVector.Y = maxMoveSpeed_Y;
                    }
                    else if (movementVector.Y < maxMoveSpeed_Y * -1)
                    {
                        movementVector.Y = maxMoveSpeed_Y * -1;
                    }
                    
                }

                return new Vector(0, GetMaxMovement(Engine.Direction.Up, Convert.ToInt32(movementVector.Y)) * -1);
            
            }
            
            return new Vector(0,0);
        
        }

        public Vector CalculateX(double accelerationRatio, Engine.Direction d) 
        {
            switch (d)
            {
                case Engine.Direction.None:

                    movementVector.X = 0;
                    return new Vector(0, 0);

                case Engine.Direction.Left:
                    if (accelerationRatio >= 1)
                    {
                        if (movementVector.X > maxMoveSpeed_X * -1)
                        {
                            movementVector.X -= (Math.Abs(accelerationRatio));
                        }

                        return new Vector(GetMaxMovement(Engine.Direction.Left, Convert.ToInt32(movementVector.X))*-1, 0);
                    }

                    return new Vector(0, 0);
                    break;

                case Engine.Direction.Right:
                    if (accelerationRatio >= 1)
                    {
                        if (movementVector.X < maxMoveSpeed_X)
                        {
                            movementVector.X += (Math.Abs(accelerationRatio));
                        }

                        return new Vector(GetMaxMovement(Engine.Direction.Right, Convert.ToInt32(movementVector.X)), 0);
                    }
                    return new Vector(0, 0);
            }
            return new Vector(0, 0);
        }

        public void SetPosition(Point position)
        {
            //moves the player to a specified pont instantly
            this.Margin = new Thickness(position.X, position.Y, 0, 0);

        }

        public void StartAnimation(Movement m, Boolean toggleQueue = false)
        {

            //Moves the player, accepts a Movement object, can only run one movement at once


            if (isMoving == false )//&& VerifyMovement(m))
            {

                isMoving = true; //forces an animation to complete before the next one can toggle

                ThicknessAnimation ta = new ThicknessAnimation();
                ta.From = this.Margin;
                ta.To = new Thickness(this.Margin.Left + m.Displacement.X, this.Margin.Top + m.Displacement.Y, 0, 0);
                ta.Duration = TimeSpan.FromMilliseconds(m.timeMs);
                ta.AccelerationRatio = m.Acceleration;
                ta.DecelerationRatio = m.Deceleration;

                ta.Completed += (o, i) => { isMoving = false; };

                if (toggleQueue)
                {
                    ta.Completed += (o, i) => { ProcessNextQueueItem(toggleQueue); };
                }

                this.BeginAnimation(MarginProperty, ta);

            }
        }

        public void AddToQueue(Movement m) 
        {
            //adds a Movement object to the list of movements to be run

            moveQueue.Add(m);
        
        }

        public void ProcessEntireQueue()
        {
            //proceeses all of the movements in the queue sequentially

            ProcessNextQueueItem(true);

        }

        public void ProcessNextQueueItem(Boolean inSeries = false)
        {

            //runs the top movement in the queue and then deletes it
            
            if (moveQueue.Count > 0)
            {
                StartAnimation(moveQueue[0], inSeries);
                moveQueue.Remove(moveQueue[0]);
            }
        }

        //private bool VerifyMovement(Movement m) 
        //{

        //    int[] newLocL = Engine.GetArrayLocation(this.Margin.Left + m.Displacement.X, this.Margin.Top + m.Displacement.Y);
        //    int[] newLocR = Engine.GetArrayLocation((this.Margin.Left + this.Width) + m.Displacement.X, this.Margin.Top + m.Displacement.Y);
        //    try
        //    {
        //        if (parentWorld.blockArray[newLocL[0], newLocL[1]].penetrable && parentWorld.blockArray[newLocL[0], newLocL[1] + 1].penetrable)
        //        {
        //            if (parentWorld.blockArray[newLocR[0], newLocR[1]].penetrable && parentWorld.blockArray[newLocR[0], newLocR[1] + 1].penetrable)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    catch { }

        //    return false;
        //}

        //public void CheckGravity()
        //{

        //    int floor = parentWorld.GetFloor(this);    //location in the array of the next lower solidblock
        //    int[] playerFloor = Engine.GetArrayLocation(this.Margin.Left + 1, this.Margin.Top + this.Height); //location in the array of the bottom left corner of the world.player

        //    if (playerFloor[1] < floor)
        //    {

        //        int deltaY = ((floor * 25) - Convert.ToInt32((this.Margin.Top + this.Height - 1)) - 1);
        //        Movement m = new Movement(new Vector(0, deltaY), 3 * deltaY, mAcceleration: 0.999);
        //        this.StartAnimation(m);

        //    }
        //}

        public void Move(Engine.Direction d) 
        { 
            currentMoveDir = d;
        }

        //public void Jump(Engine.Direction d) {
        //    if (isMoving != true) 
        //    {
        //        int deltaY = GetMaxMovement(Engine.Direction.Up, 30);
        //        switch (d)
        //        {

        //            case Engine.Direction.Left:
        //                break;

        //            case Engine.Direction.Right:
        //                AddToQueue(new Movement(new Vector(0, deltaY * -1), deltaY * 10, 0, 0.999));
        //                AddToQueue(new Movement(new Vector(3,0), 1));
        //                ProcessEntireQueue();
        //                break;

        //            case Engine.Direction.None:

        //                //MessageBox.Show("jump");


        //                StartAnimation(new Movement(new Vector(0, deltaY * -1), deltaY * 10, 0, 0.999));
        //                break;
        //        }
        //    }
        //}

        private int GetMaxMovement(Engine.Direction dir, int maxDistance) {

            //returns the max number of pixels that the player can travel in a specific direction, with an upper limit of a specified number of pixels (maxDistance)

            int playerX = Convert.ToInt32(this.Margin.Left);
            int playerY = Convert.ToInt32(this.Margin.Top);

            maxDistance = Math.Abs(maxDistance);

            switch (dir)
            {
                case Engine.Direction.Up:

                    int maxL_Up = maxDistance;
                    int maxR_Up = maxDistance;

                    try
                    {
                        for (int l = 0; l <= maxDistance; l++)
                        {
                            if (parentWorld.GetBlockAtLocation(playerX + 2, playerY - l).penetrable == false)
                            {
                                maxL_Up = l;
                                break;
                            }
                        }

                        for (int r = 0; r <= maxDistance; r++)
                        {
                            if (parentWorld.GetBlockAtLocation(Convert.ToInt32(playerX + this.Width - 2), playerY - r).penetrable == false)
                            {
                                maxR_Up = r;
                                break;
                            }
                        }

                        if (maxL_Up <= maxR_Up)
                        {
                            if (maxL_Up == 0)
                            {
                                return 0;
                            }
                            else
                            {
                                return maxL_Up - 1;
                            }
                        }
                        else
                        {
                            if (maxR_Up == 0)
                            {
                                return 0;
                            }
                            else
                            {
                                return maxR_Up - 1;
                            }
                        }
                    }
                    catch
                    {
                        return 0;
                    }

                case Engine.Direction.Down:

                    int maxL_Down = maxDistance;
                    int maxR_Down = maxDistance;
                    try
                    {
                        for (int l = 0; l <= maxDistance; l++)
                        {
                            if (parentWorld.GetBlockAtLocation(playerX + 2, Convert.ToInt32(playerY + this.Height)).penetrable == false)
                            {
                                maxL_Down = l;
                                break;
                            }
                        }


                        for (int r = 0; r <= maxDistance; r++)
                        {
                            if (parentWorld.GetBlockAtLocation(Convert.ToInt32(playerX + this.Width - 2), Convert.ToInt32(playerY + this.Height) + r).penetrable == false)
                            {
                                maxR_Down = r;
                                break;
                            }
                        }

                        if (maxL_Down <= maxR_Down)
                        {
                            if (maxL_Down == 0)
                            {
                                return 0;
                            }
                            else
                            {
                                return maxL_Down - 1;
                            }
                        }
                        else
                        {
                            if (maxR_Down == 0)
                            {
                                return 0;
                            }
                            else
                            {
                                return maxR_Down - 1;
                            }
                        }
                    }
                    catch
                    {
                        return 0;
                    }

                case Engine.Direction.Left:

                    int maxU_Left = maxDistance;
                    int maxD_Left = maxDistance;
                    try
                    {
                        for (int u = 0; u <= maxDistance; u++)
                        {
                            if (parentWorld.GetBlockAtLocation(playerX - u + 2, playerY).penetrable == false)
                            {
                                maxU_Left = u;
                                break;
                            }

                        }

                        for (int d = 0; d <= maxDistance; d++)
                        {
                            if (parentWorld.GetBlockAtLocation(playerX - d + 2, Convert.ToInt32((playerY - 1) + this.Height)).penetrable == false)
                            {
                                maxD_Left = d;
                                break;
                            }
                        }

                        if (maxU_Left <= maxD_Left)
                        {
                            if (maxU_Left == 0)
                            {
                                return 0;
                            }
                            else
                            {
                                return maxU_Left - 1;
                            }
                        }
                        else
                        {
                            if (maxD_Left == 0)
                            {
                                return 0;
                            }
                            else
                            {
                                return maxD_Left - 1;
                            }
                        }
                    }
                    catch
                    {
                        return 0;
                    }
                case Engine.Direction.Right:
                    try
                    {
                        int maxU_Right = maxDistance;
                        int maxD_Right = maxDistance;

                        for (int u = 0; u <= maxDistance; u++)
                        {

                            if (parentWorld.GetBlockAtLocation(Convert.ToInt32((playerX - 2) + this.Width + u), playerY).penetrable == false)
                            {
                                maxU_Right = u;
                                break;
                            }
                        }

                        for (int d = 0; d <= maxDistance; d++)
                        {
                            if (parentWorld.GetBlockAtLocation(Convert.ToInt32((playerX - 2) + this.Width + d), Convert.ToInt32((playerY - 1) + this.Height)).penetrable == false)
                            {
                                maxD_Right = d;
                                break;
                            }
                        }

                        if (maxU_Right <= maxD_Right)
                        {
                            if (maxU_Right == 0)
                            {
                                return 0;
                            }
                            else
                            {
                                return maxU_Right - 1;
                            }
                        }
                        else
                        {
                            if (maxD_Right == 0)
                            {
                                return 0;
                            }
                            else
                            {
                                return maxD_Right - 1;
                            }
                        }
                    }
                    catch
                    {
                        return 0;
                    }
            }
            return 0;
        }
    }
}
