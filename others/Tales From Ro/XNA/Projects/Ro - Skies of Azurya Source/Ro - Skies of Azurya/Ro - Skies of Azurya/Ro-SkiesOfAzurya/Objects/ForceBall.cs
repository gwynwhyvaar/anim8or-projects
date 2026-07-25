using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SkiesOfAzurya.Objects
{
    [Serializable]
    public struct ForceBall
    {
        /// <summary>
        /// active flag for forceball..
        /// </summary>
        public Boolean IsActive;
        
        public Vector3 Position;
        public Vector3 Direction;

        public Vector3 Velocity;

        public float Speed;

        public void Update(float delta)
        {
            GameConstants gameConstants=new GameConstants();
            Position += Direction * Speed * GameConstants.ForceBallSpeedAdjustment * delta;
            if (Position.X > gameConstants.PlayFieldSizeX || Position.X < -gameConstants.PlayFieldSizeX || Position.Y > gameConstants.PlayFieldSizeY || Position.Y < -gameConstants.PlayFieldSizeY)
            {
                IsActive = false;
            }
        }
    }
}
