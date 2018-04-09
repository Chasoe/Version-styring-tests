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
    public abstract class GameObject
    {
        // Create a general base for all objects game.
        #region Fields
        protected Image sprite;
        protected Vector2 position;
        protected Vector2 originPoint;
        protected float scaleFactor;
        protected List<Image> animationFrames;
        private string objectName;
        private RectangleF collisionBox;
        protected float currentFPS;
        #endregion

        #region Property
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 OriginPoint
        {
            get { return originPoint; }
            set { originPoint = value; }
        }
        public string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        public RectangleF CollisionBox
        {
            get { return collisionBox; }
            set { collisionBox = value; }
        }
        /*  
public RectangleF CollisionBox
{
   get
   {
           return new RectangleF(position.X, position.Y, sprite.Width * scaleFactor, sprite.Height * scaleFactor);
       }
}
*/
        #endregion
        #region Constructor
        public GameObject(Vector2 position, string spritePath, float scaleFactor, string name)
        {
            this.position = position;
            this.scaleFactor = scaleFactor;
            string[] Imagepaths = spritePath.Split(';');
            this.objectName = name;
            this.animationFrames = new List<Image>();
            //in case of animation strips add the images to a list.
            foreach (string path in Imagepaths)
            {
                Image img = Image.FromFile(path);
                animationFrames.Add(img);
            }

            this.sprite = this.animationFrames[0];
            // define the collisionBox
            collisionBox = new RectangleF(position.X, position.Y, sprite.Width * scaleFactor, sprite.Height * scaleFactor);
        }

        #endregion


        #region Methods
        public virtual void Draw(Graphics dc)
        {
            //Draws the sprite
            dc.DrawImage(sprite, position.X, position.Y, sprite.Width * scaleFactor, sprite.Height * scaleFactor);
#if DEBUG
            dc.DrawRectangle(new Pen(Brushes.Green), CollisionBox.X, CollisionBox.Y, CollisionBox.Width, CollisionBox.Height);
#endif
        }
        // hmm probaly an unecesary feature
        public bool ishovered;
        public void UpdateColisionBox()
        {
            collisionBox.X = position.X;
            collisionBox.Y = position.Y;
        }
        public virtual void Update(float fps)
        {
            // Gets the center of an object
            CalcCenterPoint();
            //Update the CollisionBox of dynamic object. this should properly be in the worker class insted.
            UpdateColisionBox();
            currentFPS = fps;
        }
        public void CalcCenterPoint()
        {
            originPoint.X = position.X + ((sprite.Width * scaleFactor) / 2);
            originPoint.Y = position.Y + ((sprite.Height * scaleFactor) / 2);
        }
        public bool CheckCords(float x, float y)
        {
            //check if a object is clicked on by a mouse 
            if (CollisionBox.Contains(x, y))
            {
                return true;
            }
            return false;
            //return CollisionBox.Contains(x, y);
        }
        #endregion

    }
}

