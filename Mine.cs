using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Numerics;

namespace Rts_project_base
{
    public enum Ressources { Gold, Coal}

    class Mine : GameObject
    {
        #region Fields
        private int resourceAmount;
        public Semaphore EnteranceKey;
        Ressources resource;
        #endregion

        #region Property

        public Ressources Resources { get { return resource; } set { resource = value; } }
        public int ResourceAmount { get { return resourceAmount; } set { resourceAmount = value; } }

        #endregion
        #region Constructor
        public Mine(Vector2 position, string spritePath, float scaleFactor, string name, Ressources ressources) : base(position, spritePath, scaleFactor, name)
        {
            // define Semaphore
            EnteranceKey = new Semaphore(1, 1);
            this.resource = ressources;
        }

        #endregion

        #region Methods


        #endregion
    }
}
