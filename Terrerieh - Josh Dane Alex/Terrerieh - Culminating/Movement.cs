using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace Terrerieh___Culminating
{
    class Movement
    {
        //this class stores all of the information that a player needs to move, such as an amount, direction, and speed
        //the data allows us to package all of the information a player needs to move and then pass it along to the player to be run later

        public Vector Displacement;
        public int timeMs;
        public double Acceleration, Deceleration;
    
        public Movement(Vector mDisplacement, int mTimeMs, double mAcceleration = 0.0, double mDeceleration = 0.0)
        {

            Displacement = mDisplacement;
            timeMs = mTimeMs;
            Acceleration = mAcceleration;
            Deceleration = mDeceleration;
    
        }
    }
}
    