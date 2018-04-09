using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using System.Threading;

namespace Rts_project_base
{
    class Bank : GameObject
    {
        #region Fields
        private static int coalCount;
        private static int goldCount;
        private static bool upgradeOne = true;
        private static bool upgradeTwo = false;
        private static bool Blocker = true;
        #endregion

        #region Property
        public static int CoalCount { get { return coalCount; } set { coalCount = value; } }
        public static int GoldCount { get { return goldCount; } set { goldCount = value; } }
        public static bool UpgradeTwo { get { return upgradeTwo; } set { upgradeTwo = value; } }
        #endregion

        #region Constructor
        public Bank(Vector2 position,string spritePath, float scaleFactor, string name) : base(position,spritePath,scaleFactor,name)
        {
            goldCount = 200;
        }
        #endregion
        #region Methods
        public static void Upgrade()
        {
            if (Bank.goldCount >= 500 && upgradeOne == true)
            {
                //Adds new worker plus new thread for it
                Worker worker = (new Worker(new System.Numerics.Vector2(300, 200), @"Images\worker_test..png", 0.2f, "Carl"));
                GameWorld.AddGameObject.Add(worker);

                goldCount -= 500;

                upgradeOne = false;
                
            }
            if (upgradeOne == false && Blocker == true && Bank.goldCount >= 750)
            {
                upgradeTwo = true;
                goldCount -= 750;
                Blocker = false;
            }
            
            #endregion
        }
    }
}
