using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Configuration;

namespace SkiesOfAzurya.Objects
{
    [Serializable]
    public class GameConstants
    {
        /// <summary>
        /// number of forceballs Nazvhi should have at start up
        /// </summary>
        public const Int32 NumForceBalls = 30;
        /// <summary>
        /// floating point Speed tweak for the forceball...
        /// </summary>
        public const float ForceBallSpeedAdjustment = 100.0f;

        public const Int32 MagicPointsPenalty = 1;
        public const Int32 AvatarDeathPenalty = 10;
        public const Int32 WarpPenalty = 50;
        public const Int32 KillBonus = 25;

        public const float RockBoundingSphereScale = 0.95f; //95% of the rock's size
        public const float AvatarBoundingSphereScale = 0.5f; //50% of the avatar's size

        private float speedAdjustment = 5.0f;
        public float SpeedAdjustment
        {
            get
            {
                return speedAdjustment;
            }
            set
            {
                speedAdjustment = value;
            }
        }

        private float obstacleMinimumSpeed = 100.0f;
        public float ObstacleMinimumSpeed
        {
            get
            {
                return obstacleMinimumSpeed;
            }
            set
            {
                obstacleMinimumSpeed = value;
            }
        }

        private float obstacleMaximumSpeed = 300.0f;
        public float ObstacleMaximumSpeed
        {
            get
            {
                return obstacleMaximumSpeed;
            }
            set
            {
                obstacleMaximumSpeed = value;
            }
        }

        private float cameraHeight = 25000.0f;
        public float CameraHeight
        {
            get
            {
                return cameraHeight;
            }
            set
            {
                cameraHeight = value;
            }
        }

        private float playFieldSizeX = 16000.0f;
        public float PlayFieldSizeX
        {
            get
            {
                return playFieldSizeX;
            }
            set
            {
                playFieldSizeX = value;
            }
        }

        private float playFieldSizeY = 12500.0f;
        public float PlayFieldSizeY
        {
            get
            {
                return playFieldSizeY;
            }
            set
            {
                playFieldSizeY = value;
            }
        }

        private Int32 numObstacles = 10;
        public Int32 NumObstacles
        {
            get
            {
                return numObstacles;
            }
            set
            {
                numObstacles = value;
            }
        }

        public GameConstants()
        {
            CameraHeight = (float)Convert.ToDouble(ConfigurationManager.AppSettings["CameraHeight"]);
            PlayFieldSizeX = (float)Convert.ToDouble(ConfigurationManager.AppSettings["PlayFieldSizeX"]);
            PlayFieldSizeY = (float)Convert.ToDouble(ConfigurationManager.AppSettings["PlayFieldSizeY"]);
            ObstacleMinimumSpeed = (float)Convert.ToDouble(ConfigurationManager.AppSettings["ObstacleMinimuSpeed"]);
            ObstacleMaximumSpeed = (float)Convert.ToDouble(ConfigurationManager.AppSettings["ObstacleMaximumSpeed"]);
            SpeedAdjustment = (float)Convert.ToDouble(ConfigurationManager.AppSettings["SpeedAdjustment"]);

            NumObstacles = Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfObstacles"]);
        }
    }
}
