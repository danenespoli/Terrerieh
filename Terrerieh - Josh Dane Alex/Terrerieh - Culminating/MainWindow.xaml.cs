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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Terrerieh___Culminating
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //enum interactionMode { Build, Destroy };

        World world;        //this is the variable where all of the world information is stored
        System.Windows.Threading.DispatcherTimer dtGameTick = new System.Windows.Threading.DispatcherTimer();

        Engine.Direction moveDir = Engine.Direction.None;

        Store.items selectedItem;
        Store Store= new Store();

        public MainWindow()
        {

            //sets up a timer used to move the form

            InitializeComponent();

            

        }

        

        private void dtGameTick_Tick(object sender, EventArgs e)
        {
            
            //ticks every 16ms (60 fps)


            //first check to see if the player needs to move to the ground

            //  world.player.CheckGravity();

            //MessageBox.Show("yoloswag");


            //FUEL TANK ticking down
            fuelLabel.Text = world.player.Fuel.ToString();
            world.player.Fuel -= 1;
            fuelPercentageLabel.Text = Math.Floor(world.player.Fuel / world.player.FuelTankCapacity * 100) + "%";


            if (world.player.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X > this.Width - 150 && 
                moveDir == Engine.Direction.Right && world.player.Margin.Left + 300 < world.blockArray.GetLength(0) * 25)
            {
                //MessageBox.Show("Right");
                world.Move(new Movement(new Vector(-(this.Width - 250), 0), 100));
            }
            if (world.player.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X < 150 && world.player.Margin.Left > 300 &&
                moveDir == Engine.Direction.Left)
            {
                world.Move(new Movement(new Vector((this.Width - 250), 0), 100));
            }
            if (world.player.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).Y > this.Height - 150)
            {
                world.Move(new Movement(new Vector(0, -(this.Height - 250)), 100));
            }

            if (world.player.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).Y < 100 && world.player.Margin.Top > 200)
            {
                world.Move(new Movement(new Vector(0, (this.Height - 250)), 100));
            }

            //Losing the game
            if (world.player.Fuel <= 0)
            {
                MessageBox.Show("You died. Nice.");
                this.Close();
            }

            //this.Title = world.player.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).ToString();
            //this.Title = world.player.CargoBayCapacity.ToString();

            //switch (moveDir)
            //{
            //    case Engine.Direction.Left:
            //        //move left
            //        world.player.StartAnimation(new Movement(new Vector(movespeed * -1, 0), timeMs));

            //        previousMoveDir = Engine.Direction.Left;
            //        break;

            //    case Engine.Direction.Right:
            //        //move right
            //        world.player.StartAnimation(new Movement(new Vector(movespeed, 0), timeMs));

            //        previousMoveDir = Engine.Direction.Right;
            //        break;

            //    case Engine.Direction.Up:
            //        //jump
            //        //MessageBox.Show(previousMoveDir.ToString());
            //        world.player.Jump(previousMoveDir);
            //        break;
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //initializes a new world and generates it

            TextureCollection al = new TextureCollection();

            world = new World(Convert.ToInt32(137), Convert.ToInt32(200),gameGrid, al);

            dtGameTick.Tick += new EventHandler(dtGameTick_Tick);
            dtGameTick.Interval = new TimeSpan(0, 0, 0, 0, 16);
            dtGameTick.Start();
            Canvas.SetZIndex(canvas_shop, 98);
            store.Visibility = Visibility.Collapsed;

            store.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            store.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            canvas_shop.Background = new ImageBrush(world.textures.assets["shop"]);
           

        }

        private void gameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //this code can select a block based on the location of a click and then call any method on the block
            int[] clickArrayLoc = Engine.GetArrayLocation(e.GetPosition(gameGrid));

            var distance = Engine.GetDistance(new Point((clickArrayLoc[0] * 25) + 13, (clickArrayLoc[1] * 25) + 13), new Point(world.player.Margin.Left + (world.player.Width / 2), world.player.Margin.Top + (world.player.Height / 2)));

            //isolates the individual block
            var selectedBlock = world.GetBlockAtLocation(e.GetPosition(gameGrid));
            if (distance <= 75)
            {
                if (Engine.GetDrillMiningValidityOfBlocksIfYouBoughtTheRightDrill(world.player.playerDrill, selectedBlock))
                {
                    //this.Title = "";

                    //adds that block to the world.players inventory
                    if (world.player.Cargo < world.player.CargoBayCapacity)
                    {
                        world.player.inventory.Add(selectedBlock.GetType(), 1);

                    }
                    //removes the block from the world

                    //world.TransformToAir(world.blockArray[clickArrayLoc[0], clickArrayLoc[1]], gameGrid);
                    world.PlaceBlock(typeof(Block_air), world.blockArray[clickArrayLoc[0], clickArrayLoc[1]]);
                    cargoHudLabel.Text = world.player.Cargo.ToString() + " / " + world.player.CargoBayCapacity.ToString();
                }
            }
                //else if (mode == interactionMode.Build)
                //{   //place a block
                //    if (world.player.inventory.Items.ContainsKey(selection))
                //    {
                //        if (world.player.inventory.Items[selection] > 0)
                //        {
                //            world.PlaceBlock(selection, selectedBlock);
                //            world.player.inventory.Remove(selection, 1);
                //        }
                //    }
                //}

                //this.Title = "";

                //foreach (var v in world.player.inventory.Items)
                //{
                //    //(testing code) displays the world.player inventory in the title of the window
                //    this.Title += v.Key.ToString() + ": " + v.Value.ToString() + " ";
                //    this.Title = this.Title.Replace("Terrerieh___Culminating.", "");//.Replace("_","");
                //}
            
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //runs when a key is pressed

            switch (e.Key)
            {
                //-------------------------------------------------------
                //Inventory Keys
                //case Key.D1:
                //    selection = world.player.inventory.Items.ElementAt(0).Key;
                //    MessageBox.Show(selection.ToString() + " selected");
                //    break;
                //case Key.D2:
                //    selection = world.player.inventory.Items.ElementAt(1).Key;
                //    MessageBox.Show(selection.ToString() + "selected");
                //    break;
                //case Key.D3:
                //    selection = world.player.inventory.Items.ElementAt(2).Key;
                //    MessageBox.Show(selection.ToString() + "selected");
                //    break;
                //case Key.D4:
                //    selection = world.player.inventory.Items.ElementAt(3).Key;
                //    MessageBox.Show(selection.ToString() + "selected");
                //    break;

                //-------------------------------------------------------
                //Movement Keys

                case Key.A:
                case Key.Left:
                    world.player.Move(Engine.Direction.Left);
                    moveDir = Engine.Direction.Left;
                    //world.player.Background = new ImageBrush(world.textures.assets["playerLeft"]);
                    break;

                case Key.D:
                case Key.Right:
                    world.player.Move(Engine.Direction.Right);
                    moveDir = Engine.Direction.Right;
                    //world.player.Background = new ImageBrush(world.textures.assets["playerRight"]);
                    break;

                case Key.Space:
                case Key.W:
                case Key.Up:
                    //moveDir = Engine.Direction.Up;  //jump
                    world.player.Move(Engine.Direction.Up);
                    moveDir = Engine.Direction.None;
                    break;

                //-------------------------------------------------------
                //Misc Keys

                //case Key.LeftShift:
                //    //switches between placing and destroying blocks
                //    if (mode == interactionMode.Build)
                //    {
                //        mode = interactionMode.Destroy;
                //    }
                //    else
                //    {
                //        mode = interactionMode.Build;
                //    }

                //    break;
                   
                 

                //-------------------------------------------------------
                //debug code

                case Key.T:
                    //MessageBox.Show("turtleman");
                    world.player.SetPosition(new Point(canvas_shop.Margin.Left, canvas_shop.Margin.Top));
                    break;

                case Key.Escape:
                    store.Visibility = Visibility.Collapsed;
                    break;

                case Key.Enter:
                    world.player.Move(Engine.Direction.None);
                    Point playerPoint = new Point(world.player.Margin.Left + (world.player.Width / 2), world.player.Margin.Top + (world.player.Height / 2));
                    Point storePoint = new Point(canvas_shop.Margin.Left + (canvas_shop.Width / 2), canvas_shop.Margin.Top + (canvas_shop.Height / 2));
                    //MessageBox.Show(Engine.GetDistance(playerPoint, storePoint).ToString());
                   if (Engine.GetDistance(playerPoint, storePoint) < 100) 
                   {
                        store.Visibility = Visibility.Visible;
                        Canvas.SetZIndex(store, 100);
                        moneyLabel.Text = world.player.money.ToString();
                        Store.PopulateList(world.player, lbInventory);
                        //MessageBox.Show(store.Visibility.ToString());

                        UpdateStore();
                    }
                    break;
            }
        }

        private void UpdateStore()
        {
            //Updates store values
            cargobayCostLabel.Text = Store.cargobayCost.ToString();
            fueltankCostLabel.Text = Store.fueltankCost.ToString();
            fueltankDescLabel.Text = "+" + ( (world.player.FuelTankCapacity * 0.5).ToString() ) + " Capacity";

            moneyLabel.Text = world.player.money.ToString();
            moneyHudLabel.Text = world.player.money.ToString();

            cargoHudLabel.Text = world.player.Cargo.ToString() + " / " + world.player.CargoBayCapacity.ToString();


            if (Store.drillList.Count > 1)
            {
                drillDescLabel.Text = Store.drillList[1].name;  //shows name of next drill in list
                drillCostLabel.Text = Store.drillCost.ToString();
            }


        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {   
            moveDir = Engine.Direction.None;
            //previousMoveDir = Engine.Direction.None;
            world.player.Move(Engine.Direction.None);
        }


        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            Store.Buy(selectedItem, world.player);
            UpdateStore();            

        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            Store.SellAllItems(world.player);
            Store.PopulateList(world.player, lbInventory);
            UpdateStore();
        }

        

        private void btnRefuel_Click(object sender, RoutedEventArgs e)
        {
            Store.Refuel(world.player);
            UpdateStore();
        }


        //CARGO
        private void rectCargoBay_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selectedItem != Store.items.CargoBay)
            {
                rectCargoBay.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
            }
        }
        private void rectCargoBay_MouseLeave(object sender, MouseEventArgs e)
        {
            if (selectedItem != Store.items.CargoBay)
            {
                rectCargoBay.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
        }
        private void rectCargoBay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rectCargoBay.Fill = new SolidColorBrush(System.Windows.Media.Colors.Chocolate);

            rectFuelTank.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            rectDrill.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            selectedItem = Store.items.CargoBay;
        }

        //FUEL TANK
        private void rectFuelTank_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selectedItem != Store.items.FuelTank)
            {
                rectFuelTank.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
            }
        }
        private void rectFuelTank_MouseLeave(object sender, MouseEventArgs e)
        {
            if (selectedItem != Store.items.FuelTank)
            {
                rectFuelTank.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
        }
        private void rectFuelTank_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rectFuelTank.Fill = new SolidColorBrush(System.Windows.Media.Colors.Chocolate);

            rectCargoBay.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            rectDrill.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            selectedItem = Store.items.FuelTank;
        }


        //DRILL
        private void rectDrill_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selectedItem != Store.items.Drill)
            {
                rectDrill.Fill = new SolidColorBrush(System.Windows.Media.Colors.AliceBlue);
            }
        }
        private void rectDrill_MouseLeave(object sender, MouseEventArgs e)
        {
            if (selectedItem != Store.items.Drill)
            {
                rectDrill.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            }
        }
        private void rectDrill_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rectDrill.Fill = new SolidColorBrush(System.Windows.Media.Colors.Chocolate);

            rectFuelTank.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            rectCargoBay.Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            selectedItem = Store.items.Drill;
        }

      
    }
}
