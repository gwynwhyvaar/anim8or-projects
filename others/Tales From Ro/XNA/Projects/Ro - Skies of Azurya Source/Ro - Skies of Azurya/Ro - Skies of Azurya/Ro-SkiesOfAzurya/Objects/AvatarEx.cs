// ------------------------
// Avatar Class
// (c) kaddiska 2009
// ------------------------

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SkiesOfAzurya.Objects
{
    [Serializable]
    public class AvatarEx
    {
        private Model avatarModel;

        public Boolean IsActive = true;

        public Model AvatarModel
        {
            get
            {
                return avatarModel;
            }
            set
            {
                avatarModel = value;
            }
        }

        public Matrix[] Transforms;

        /// <summary>
        /// Position of the Model in World space
        /// </summary>
        public Vector3 Position = Vector3.Zero;
        /// <summary>
        /// Velocity of the model, Applied each frame to the model's position
        /// </summary>
        public Vector3 Velocity = Vector3.Zero;
        
        //amplifies controller speed input
        private const float VelocityScale = 5.0f;

        /// <summary>
        /// 
        /// </summary>
        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);

        private float rotation;

        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                float newVal = value;
                while (newVal >= MathHelper.TwoPi)
                {
                    newVal -= MathHelper.TwoPi;
                }
                while (newVal < 0)
                {
                    newVal += MathHelper.TwoPi;
                }
                if (rotation != newVal)
                {
                    rotation = newVal;
                    //RotationMatrix = Matrix.CreateRotationY(rotation);
                    RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationZ(rotation);
                }
            }
        }
        public void Update()
        {
            GameConstants gameConstant = new GameConstants();

            if (Position.X > gameConstant.PlayFieldSizeX)
            {
                Position.X -= 2 * gameConstant.PlayFieldSizeX;
            }
            if (Position.X < -gameConstant.PlayFieldSizeX)
            {
                Position.X += 2 * gameConstant.PlayFieldSizeX;
            }
            if (Position.Y > gameConstant.PlayFieldSizeY)
            {
                Position.Y -= 2 * gameConstant.PlayFieldSizeY;
            }
            if (Position.Y < -gameConstant.PlayFieldSizeY)
            {
                Position.Y += 2 * gameConstant.PlayFieldSizeY;
            }
        }
        public void Update(GamePadState gamePadControllerState)
        {
            // Rotate the model using the left thumbstick, and scale it down.
            rotation -= gamePadControllerState.ThumbSticks.Left.X * 0.10f;
            // Finally, add this vector to our velocity.
            Velocity += RotationMatrix.Forward * 1.0f * gamePadControllerState.Triggers.Right;
        }
        public void Update(MouseState mouseControllerState)
        {
            if (mouseControllerState.LeftButton == ButtonState.Pressed)
            {
                Velocity -= RotationMatrix.Backward * VelocityScale;
            }
            //Rotation -=(float) mouseControllerState.X;
        }
        public void Update(KeyboardState keyBoardControllerState)
        {
            //rotate the model using the left arrow, and scale it down
            if (keyBoardControllerState.IsKeyDown(Keys.Left) == true)
            {
                Rotation -= 0.10f;
                //Velocity -= RotationMatrix.Backward * VelocityScale;
            }
            if (keyBoardControllerState.IsKeyDown(Keys.Down) == true)
            {
                Velocity += RotationMatrix.Backward* VelocityScale;
            }
            else if (keyBoardControllerState.IsKeyDown(Keys.Right) == true)
            {
                Rotation += 0.10f;
                //Velocity -= RotationMatrix.Backward * VelocityScale;
            }
        }
    }
}
