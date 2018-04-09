using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Numerics;
using System.Drawing;
using System.Windows.Forms;

namespace Rts_project_base
{
    
    sealed class GameWorld
    {
        //only allow one instance of the gameworld class. useing Simpleton Pattern
        #region Simpleton
        public static object kGameworldInstance = new object();
        private static GameWorld _instance;
        public GameWorld(Graphics draws, Rectangle displayRectangle) // change to private to make Simpletom work as intended by for testing purpose leave it public 
        {
            this.backBuffer = BufferedGraphicsManager.Current.Allocate(draws, displayRectangle);
            this.draws = backBuffer.Graphics;
            this.displayRect = displayRectangle;
            unitRect = new RectangleF(0,0,(displayRect.Width/50),(displayRect.Height/50));
            gameObjectList = new List<GameObject>();
            addGameObject = new List<GameObject>();
            removeGameObject = new List<GameObject>();
            Setup();
            Draw();
        }
        /*
        public static GameWorld Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (kGameworldInstance)
                    {

                        //Creates the Gameworld Instance
                        _instance = new GameWorld();
                    }
                }
                return _instance;
            }
        }
        */
        #endregion
        #region Fields
        private Rectangle displayRect;
        private RectangleF unitRect;
        private Graphics draws;
        private BufferedGraphics backBuffer;
        private float currentFps;
        private DateTime endTime;
        private static List<GameObject> gameObjectList;
        private static Mutex gameListKey= new Mutex();
        private static List<GameObject> addGameObject;
        private static List<GameObject> removeGameObject;
        #endregion
        #region Properties
        public static List<GameObject> GameObjectList
        {
            get { return gameObjectList; }
            set { gameObjectList = value; }
        }
        public  static List<GameObject> AddGameObject
        {
            get { return addGameObject; }
            set { addGameObject = value; }
        }
        public static List<GameObject> RemoveGameObject
        {
            get { return removeGameObject; }
            set { removeGameObject = value; }
        }
        #endregion

        private void Setup()
        {
            //intialize the componets of the gameworld
            GameObjectList.Add(new Mine(new Vector2(200, 1), @"Images\Mine_Test1..png", 1, "CoalMinene", Ressources.Coal));
            gameObjectList.Add(new Mine(new Vector2(1, 1), @"Images\Mine_Test1..png", 1,"GoldMinene",Ressources.Gold));
            gameObjectList.Add(new Bank(new Vector2(600, 200), @"Images\Bank.png", 0.4f, "Bank"));
            GameForm.runGame = true;
        }
        public void Draw()
        {
            DrawContent();
       
        }
        public void DrawContent()
        {
            
            ///<summary>
            ///Draws the games ui
            /// </summary>
            //Draw the Graphics of the Game
            draws.Clear(Color.White);
            gameListKey.WaitOne();
            foreach (GameObject drawable in gameObjectList)
            {
                drawable.Draw(draws);
                DrawUi();
                
            }
            gameListKey.ReleaseMutex();

            backBuffer.Render();
        }
        public void DrawUi()
        {
            Font f = new Font("Arial", 16);
            draws.DrawString(string.Format("FPS: {0}", currentFps), f, Brushes.Black, 550, 0);

            Font counter = new Font("Arial Black", 14);
            string gold = Bank.GoldCount.ToString();
            draws.DrawString(string.Format("Gold: {0}", gold), counter, Brushes.Black, 680, 10);

            string coal = Bank.CoalCount.ToString();
            draws.DrawString(string.Format("Coal: {0}", coal), counter, Brushes.Black, 800, 10);

            //string gold = Bank.goldCount.ToString();
            //draws.DrawString(string.Format("Gold:{0}", gold), counter, Brushes.Black, 700, 10);

        }
        public void Update()
        {
            ///<summary>
            ///Updates the state of the gameobjects
            /// </summary>
            gameListKey.WaitOne();
             foreach(GameObject gO in gameObjectList)
             {
                gO.Update(currentFps);
            };
            foreach (GameObject item in AddGameObject)
            {
            
                gameObjectList.Add(item);
               
            }
            foreach (GameObject item in RemoveGameObject)
            {
            
                GameObjectList.Remove(item);
               
            }
            ClearTempLists();
            gameListKey.ReleaseMutex();
        }
        private void ClearTempLists()
        {
            RemoveGameObject.Clear();
            AddGameObject.Clear();
        }
        public void Gameloop()
        {
            DateTime startime = DateTime.Now;
            TimeSpan deltaTime = startime - endTime;
            int miliSecond = deltaTime.Milliseconds > 0 ? deltaTime.Milliseconds : 1;
            currentFps = 1000 / miliSecond;
            endTime = DateTime.Now;
            Draw();
            Update();
        }
    }
}