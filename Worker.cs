using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace Rts_project_base
{
    class Worker : GameObject
    {
        #region Fields
        private float speed;
        private bool working;
        private bool carryingResource;
        private bool active;
        private bool moving;
        Vector2 destination;
        Mine currentMine;
        Ressources carry;
        #endregion
        #region Property

        public float Speed { get { return speed; } set { speed = value; } }
        public bool Working
        {
            get { return working; }
            set { working = value; }
        }
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
        }
        public Vector2 Destination
        {
            get { return destination; }
            set { destination = value; }
        }
        public Mine CurrentMine { get { return currentMine; } set { currentMine = value; } }
        #endregion
        #region Constructor
        public Worker(Vector2 position, string spritePath, float scaleFactor, string name) : base(position, spritePath, scaleFactor, name)
        {
            //starts the workers thread
            InitWorkerThread();
            speed = 1;
        }
        #endregion
        #region Methods

        private void InitWorkerThread()
        {
            // creates a new thread the worker will be useing
            Thread WorkerThread = new Thread(Work);
            //starts the thread
            WorkerThread.Start();
            WorkerThread.IsBackground = true;
            //sets the state of the worker to be active
            active = true;
        }
        public void Work()
        {
            //as long the worker is on duty he will awit a command Maybe this is bussy working.
            while (active)
            {
                if (carryingResource)
                {
                    MoveToPosition(currentFPS, destination);
                    if (destination == position)
                    {
                        DepositResource();
                        carryingResource = false;
                    }
                }
                else if (working)
                {
                    MoveToPosition(currentFPS, destination);
                    if (position == destination)
                    {
                        Mine();
                        currentMine = null;
                        working = false;
                    }


                }
                else if (moving)
                {
                    MoveToPosition(currentFPS, destination);
                    if (position == destination)
                    {
                        moving = false;
                    }

                }
            }
        }
        public void Mine()
        {
            // wait until acess to the mine is granted
            currentMine.EnteranceKey.WaitOne();
            GameWorld.RemoveGameObject.Add(this);
            if (Bank.UpgradeTwo)
            {
                Thread.Sleep(2000);
            }
            else
            {
                Thread.Sleep(3000);//simulates the worker mining
            }
            foreach (GameObject item in GameWorld.GameObjectList)
            {
                if (item is Bank)
                {
                    // i the case the was expanded to hold more than one bank add some code that desides the bank which bank is the neares
                    GetBankDestination(item as Bank);
                }
            }
            carryingResource = true;
            carry = currentMine.Resources;
            position.X = currentMine.OriginPoint.X;
            position.Y = currentMine.OriginPoint.Y + (currentMine.OriginPoint.Y - currentMine.Position.Y);
            GameWorld.AddGameObject.Add(this);
            // releaser key so ohter members ca acces the mine

            currentMine.EnteranceKey.Release();
        }
        public void GetBankDestination(Bank bank)
        {
            destination = bank.OriginPoint;
        }
        public void MoveToPosition(float fps, Vector2 deposition)
        {

            Vector2 velosity = Vector2.Normalize(deposition - this.position);

            {
                this.position.X += (1 * (velosity.X * speed)) / (fps * 120);
                this.position.Y += (1 * (velosity.Y * speed)) / (fps * 120);
            }

        }
        public void DepositResource()
        {
            if (carry == Ressources.Gold)
            {
                Bank.GoldCount += 300;
            }
            else
            {
                Bank.CoalCount += 15;
            }

        }
        #endregion

    }
}
