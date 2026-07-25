using System;
using System.Collections.Generic;
//using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using SkiesOfAzurya.Objects;
using System.Configuration;
using System.IO;
using SkiesOfAzurya.Forms;
using SkiesOfAzurya.Enums;

namespace SkiesOfAzurya
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SkiesOfAzuryaClass : Microsoft.Xna.Framework.Game
    {
        private TitleScreen titleScreen = new TitleScreen();

        private GameStateEnum GameStateEnum = GameStateEnum.TitleScreen;

        private bool isPaused = false;
        private bool isPauseKeyDown = false;
        private bool isPausedForguid = false;

        private float MP = 100;//100%
        private float HP = 100;//100%
        private float EXP = (float)decimal.Zero;

        Texture2D hpBar, mpBar;

        private string defaultModel = "nazvhi-flight";

        private GameConstants gameConstants ;//= new GameConstants();

        private float layerDepth = 0f;

        public float LayerDepth { get { return layerDepth; } set { layerDepth = value; } }

        GraphicsDeviceManager graphics;

        KeyboardState lastKeyBoardState = Keyboard.GetState();
        KeyboardState lastTitleKeyBoardState = Keyboard.GetState();

        MouseState lastMouseState = Mouse.GetState();

        SpriteBatch spriteBatch;

        SpriteFont centurygothic;

        Int32 Score =0;

        Vector2 ScorePosition = new Vector2(100, 50);

        public AvatarEx avatarEx = new AvatarEx();

        public Model hillModel;
        public Model forceBallModel;
        public Model scytheModel;
        public Model backgroundModel;

        Matrix[] ForceBallTransforms;
        Matrix[] HillModelTransforms;
        Matrix[] ScytheTransforms;

        ForceBall[] ForceBallList;
        HillSide[] HillSideList;
       
        Random randomVar = new Random();

        //Camera/View information
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, -5000.0f);

        Matrix projectionMatrix;
        Matrix viewMatrix,titleViewMatrix;

        //get-sets the default model to load in this application
        public String DefaultModel { get { return defaultModel; } set { defaultModel = value; } }
        public String DefaultBackground { get; set; }

        public Int32 menuoptions = 1;

        //Audio Components
        #region not used in this demo yet
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;
        #endregion

        #region to keep my mesh spinning in the skies..i think i will need this :P
        private float modelRotation = 0.0f;

        public float ModelRotation { get { return modelRotation; } set { modelRotation = value; } } 
        #endregion

        #region fields for the background - wont the best thing be to create a class for this? oh well :P
        
        public Texture2D Background { get; set; }
        public Vector2 ScreenPosition;
        public Vector2 Origin { get; set; }
        public Vector2 TextureSize { get; set; }

        public Texture2D RoBackgroundTexture { get; set; }

        public Int32 ScreenHeight { get; set; } 
        
        #endregion

        Texture2D nazvhihp40x40, nazvhimp40x40, banner;

        public SkiesOfAzuryaClass()
        {
            try
            {
                graphics = new GraphicsDeviceManager(this);
                Content.RootDirectory = "Content";
                Window.Title = "Nazvhi - Skies of Azurya XNA Demo";
                
                graphics.PreferMultiSampling = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowMultiSampling"]);
                graphics.PreferredDepthStencilFormat = DepthFormat.Depth24;
            }
            catch (Exception exxxxx)
            {
                File.AppendAllText("c:\\ro.azurya.log", DateTime.Now.ToString() + "\n\r#Error :" + exxxxx.Message + "\n\r\tStacktrace :" + exxxxx.StackTrace + "\n\r\n\r");
            }
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            try
            {
                //graphics.GraphicsDevice.RenderState.DepthBufferEnable = false;

                //Splash dialog, will show for 5 seconds
                Splash splash = new Splash();
                splash.ShowDialog();

                gameConstants = new GameConstants();

                HillSideList = new HillSide[gameConstants.NumObstacles];
                ForceBallList = new ForceBall[GameConstants.NumForceBalls];
               
                cameraPosition = new Vector3(0.0f, 0.0f, gameConstants.CameraHeight);

                DefaultModel = ConfigurationManager.AppSettings["DefaultModel"];
                DefaultBackground = ConfigurationManager.AppSettings["DefaultBackground"];
                LayerDepth = (float)Convert.ToDouble(ConfigurationManager.AppSettings["LayerDepth"]);

                float camPosX = (float)Convert.ToDouble(ConfigurationManager.AppSettings["CameraPositionX"]);
                float camPosY = (float)Convert.ToDouble(ConfigurationManager.AppSettings["CameraPositionY"]);
                float camPosZ = (float)Convert.ToDouble(ConfigurationManager.AppSettings["CameraPositionZ"]);

                //set up the camera position for the title screen.
                cameraPosition = new Vector3(camPosX, camPosY, camPosZ);

                //set up the view matrix for the title screen.
                viewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                titleViewMatrix = Matrix.CreateLookAt(new Vector3(0,0,130)/*new Vector3(10f)*/, Vector3.Zero, Vector3.Up);

                //set up the projection matrix for the title screen.
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), GraphicsDevice.DisplayMode.AspectRatio, gameConstants.CameraHeight - 2000.0f, gameConstants.CameraHeight + 1000.0f);
                
                ResetObstacles();

                GraphicsDeviceCapabilities caps = graphics.GraphicsDevice.GraphicsDeviceCapabilities;
                if (caps.MaxPixelShaderProfile < ShaderProfile.PS_2_0)
                {
                    graphics.MinimumPixelShaderProfile = ShaderProfile.PS_1_4;
                    graphics.MinimumVertexShaderProfile = ShaderProfile.VS_1_1;
                    graphics.ApplyChanges();
                }
                else
                {
                    graphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;
                    graphics.MinimumVertexShaderProfile = ShaderProfile.VS_2_0;
                    graphics.ApplyChanges();
                }

                base.Initialize();
            }
            catch (Exception exxxxx)
            {
                File.AppendAllText("c:\\ro.azurya.log", DateTime.Now.ToString() + "\n\r#Error :" + exxxxx.Message + "\n\r\tStacktrace :" + exxxxx.StackTrace + "\n\r\n\r");
            }
        }
        private Matrix[] SetupEffectDefaults(Model AvatarModelParam)
        {
            Matrix[] absoluteTransforms = new Matrix[AvatarModelParam.Bones.Count];
            AvatarModelParam.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in AvatarModelParam.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
            }
            return absoluteTransforms;
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
                
            //load the Assets for the title screen here..
            titleScreen.TitleModels.Add("Background", Content.Load<Model>("Models\\nazvhi-title-background"));
            titleScreen.BackgroundTitleTexture = Content.Load<Texture2D>("Images\\nazvhi-title-background");
            centurygothic = Content.Load<SpriteFont>("Fonts//CenturyGothic");
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) == true)
            {
                this.Exit();
            }
            switch (GameStateEnum)
            {
                case GameStateEnum.GameDemoScreen:
                    #region Gameplay Screen Updates
                    CheckPauseKey();
                    if (!isPaused)
                    {
                        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (MP < 100)
                        {
                            MP += 0.005f;
                        }
                        else
                        {
                            MP = 100.0f;
                        }
                        // Allows the game to exit
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) == true)
                        {
                            this.Exit();
                        }
                        UpdateKeyBoardInput();

                        avatarEx.Update();

                        //add some velocity to the current avatar's position
                        //avatarEx.Position += avatarEx.Velocity;
                        avatarEx.Position.Z += avatarEx.Velocity.Z;
                        avatarEx.Position.X += avatarEx.Velocity.X;
                        avatarEx.Position.Y += avatarEx.Velocity.Y;

                        //bleed off velocity over time
                        avatarEx.Velocity *= 0.95f;

                        for (int x = 0; x < gameConstants.NumObstacles; x++)
                        {
                            HillSideList[x].Update(elapsed);
                        }

                        for (int mp = 0; mp < GameConstants.NumForceBalls; mp++)
                        {
                            if (ForceBallList[mp].IsActive)
                            {
                                ForceBallList[mp].Update(elapsed);
                            }
                        }
                        //forceball - rock/obstacle check..
                        for (int exp = 0; exp < HillSideList.Length; exp++)
                        {
                            if (HillSideList[exp].IsActive)
                            {
                                BoundingSphere rockXSphere = new BoundingSphere(HillSideList[exp].Position, hillModel.Meshes[0].BoundingSphere.Radius * GameConstants.RockBoundingSphereScale);
                                for (int xx = 0; xx < ForceBallList.Length; xx++)
                                {
                                    if (ForceBallList[xx].IsActive)
                                    {
                                        BoundingSphere forceBallSphere = new BoundingSphere(ForceBallList[xx].Position, forceBallModel.Meshes[0].BoundingSphere.Radius);
                                        if (rockXSphere.Intersects(forceBallSphere))
                                        {
                                            //play sound explosion
                                            HillSideList[exp].IsActive = false;
                                            ForceBallList[xx].IsActive = false;
                                            //a hit! hooray!
                                            EXP += 0.5f;
                                            Score += GameConstants.KillBonus;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        //create bounding sphere for avatar
                        if (avatarEx.IsActive)
                        {
                            BoundingSphere avatarSphere = new BoundingSphere(avatarEx.Position, avatarEx.AvatarModel.Meshes[0].BoundingSphere.Radius * GameConstants.AvatarBoundingSphereScale);

                            //loop to visit each asteriod

                            for (int ixrock = 0; ixrock < HillSideList.Length; ixrock++)
                            {
                                //create temp bounding sphere for rocks..
                                if (HillSideList[ixrock].IsActive)
                                {
                                    BoundingSphere rockSphereTemp = new BoundingSphere(HillSideList[ixrock].Position, hillModel.Meshes[0].BoundingSphere.Radius * GameConstants.RockBoundingSphereScale);
                                    if (rockSphereTemp.Intersects(avatarSphere))
                                    {
                                        //BOOM! we are DEAD!!!
                                        HP -= GameConstants.AvatarDeathPenalty;
                                        //is avatar's hp less than or equal to zero
                                        if (HP <= (float)decimal.Zero)
                                        {
                                            //death man...as in death...BOOM!!!
                                            avatarEx.IsActive = false;
                                        }
                                        HillSideList[ixrock].IsActive = false;
                                        break;
                                    }
                                }
                            }
                        }

                        //UpdateBackground(elapsed * 50);

                        modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);
                    }
                    #endregion
                    break;
                case GameStateEnum.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true&&menuoptions==1)
                    {
                        GameStateEnum = GameStateEnum.GameDemoScreen;
                        LoadGameDemoAssets();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true && menuoptions == 3)
                    {
                        this.Exit();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) == true)
                    {
                        this.graphics.ToggleFullScreen();
                    }
                    UpdateTitleKeys();
                    break;
            }
            base.Update(gameTime);
        }
        public void UpdateTitleKeys()
        {
            KeyboardState currentKeyState = Keyboard.GetState();

            if (currentKeyState.IsKeyDown(Keys.Down) == true && lastTitleKeyBoardState.IsKeyUp(Keys.Down) == true)
            {
                    menuoptions += 1;
                    if (menuoptions > 3)
                    {
                        menuoptions = 1;
                    }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true && lastTitleKeyBoardState.IsKeyUp(Keys.Up) == true)
            {
                menuoptions -= 1;
                if (menuoptions <1)
                {
                    menuoptions = 3;
                }
            }
            lastTitleKeyBoardState = currentKeyState;
        }
        public void UpdateBackground(float deltaY)
        {
            ScreenPosition.Y += deltaY;
            ScreenPosition.Y = ScreenPosition.Y % RoBackgroundTexture.Height;
        } 
        public void UpdateKeyBoardInput()
        {
            //we get the current state of the gamepad
            //GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            KeyboardState currentKeyState = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();

            if (currentKeyState != null)
            {
                avatarEx.Update(currentKeyState);
                avatarEx.Update(currentMouseState);

                // todo : add sound effects here..

                // if lost..we press ENTER to reset! neat huh?
                if (currentKeyState.IsKeyDown(Keys.Enter) == true)
                {
                    avatarEx.Position = Vector3.Zero;
                    avatarEx.Velocity = Vector3.Zero;
                    avatarEx.Rotation = 0.0f;
                    //avatarEx.IsActive = true;
                    //shouldnt happen mate..u lose a simple token..
                    HP -= 0.15f;
            
                    MP -= 0.5f;
                    if (MP < 0)
                    {
                        MP = (float)decimal.Zero;
                    }
                    if (HP < 0)
                    {
                        HP = (float)decimal.Zero;
                    }
                }
                //we use this to toggle between fullscreen and window
                if (currentKeyState.IsKeyDown(Keys.Space) == true)
                {
                    this.graphics.ToggleFullScreen();
                }
                //are we shooting?
                if (avatarEx.IsActive && currentMouseState.RightButton == ButtonState.Pressed&&lastMouseState.RightButton== ButtonState.Released)
                {
                    //add another force ball. we find an inactive force ball slot (mana), and use it..
                    //if all force ball slots (mana)are used, ignore the user input
                    for (int mp = 0; mp < GameConstants.NumForceBalls; mp++)
                    {
                        if (!ForceBallList[mp].IsActive)
                        {
                            //penalize him for shooting his... :P
                            if (MP < (float)decimal.Zero || MP == (float)decimal.Zero)
                            {
                                //dont shoot man...
                                break;
                            }
                            MP -= GameConstants.MagicPointsPenalty;

                            ForceBallList[mp].Direction = avatarEx.RotationMatrix.Forward;
                            ForceBallList[mp].Speed = GameConstants.ForceBallSpeedAdjustment;
                            ForceBallList[mp].Position = avatarEx.Position + (1000 * ForceBallList[mp].Direction);
                            ForceBallList[mp].IsActive = true;
                            //ForceBallList[mp].Velocity += avatarEx.Velocity;
                            //play sound file here.. ;-)
                            break;
                        }
                    }
                }
            }
            lastKeyBoardState = currentKeyState;
            lastMouseState = currentMouseState;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.LightBlue);

            //we render the scene..
            switch (GameStateEnum)
            {
                case GameStateEnum.GameDemoScreen:
                    #region Check if it's the GameDemo Screen i.e the Gameplay Screen
                    if (HP <= (float)decimal.Zero)
                    {
                        //kill the ma'fah..!
                        avatarEx.IsActive = false;
                    }

                    //spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
                    //DrawBackground(spriteBatch);
                    //spriteBatch.End();
                    
                    //we try to draw a simple world here..
                    DrawWorld(backgroundModel);

                    Matrix scytheMatrix = Matrix.CreateTranslation(avatarEx.Position);
                    DrawScytheEx(scytheModel, scytheMatrix, ScytheTransforms);

                    //avatarEx.RotationMatrix = Matrix.CreateRotationY(avatarEx.Rotation);
                    Matrix avatarTransformMatrix = Matrix.CreateRotationZ(avatarEx.Rotation) * Matrix.CreateTranslation(avatarEx.Position);
                    //Matrix avatarTransformMatrix = avatarEx.RotationMatrix * Matrix.CreateTranslation(avatarEx.Position);
                    if (avatarEx.IsActive)
                    {
                        DrawAvatarEx(avatarEx.AvatarModel, avatarTransformMatrix, avatarEx.Transforms);//,modelRotation,avatarEx.Position); 
                    }

                    for (int ix = 0; ix < gameConstants.NumObstacles; ix++)
                    {
                        Matrix obstacleTransformEx = Matrix.CreateTranslation(HillSideList[ix].Position);
                        if (HillSideList[ix].IsActive)
                        {
                            DrawAvatarEx(hillModel, obstacleTransformEx, HillModelTransforms, modelRotation, HillSideList[ix].Position);
                        }
                    }

                    for (int mp = 0; mp < GameConstants.NumForceBalls; mp++)
                    {
                        if (ForceBallList[mp].IsActive)
                        {
                            Matrix forceBallTransformMatrix = Matrix.CreateTranslation(ForceBallList[mp].Position);
                            DrawForceballEx(forceBallModel, forceBallTransformMatrix, ForceBallTransforms, modelRotation, ForceBallList[mp].Position);
                        }
                    }

                    //we draw the banner and icons
                    #region Drawing banner and Icons
                  
                    spriteBatch.Begin();
                    spriteBatch.Draw(nazvhihp40x40, new Vector2(110, 55), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, LayerDepth);
                    spriteBatch.End();

                    spriteBatch.Begin();
                    spriteBatch.Draw(nazvhimp40x40, new Vector2(250, 55), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, LayerDepth);
                    spriteBatch.End();

                    //we draw the score last
                    spriteBatch.Begin();//SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
                    spriteBatch.DrawString(centurygothic, "Score : " + Score + "  Exp " + Convert.ToInt32(EXP), new Vector2(110, 110), Color.Black);
                    spriteBatch.End();
                    //we draw the MP
                    spriteBatch.Begin();//SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
                    spriteBatch.DrawString(centurygothic, "MP : " + Convert.ToInt32(MP), new Vector2(300, 55), Color.Black);
                    spriteBatch.End();
                    //we draw the HP
                    spriteBatch.Begin();//SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
                    spriteBatch.DrawString(centurygothic, "HP : " + Convert.ToInt32(HP), new Vector2(160, 55), Color.Black);
                    spriteBatch.End();

                    //we draw the game version here..
                    spriteBatch.Begin();//SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
                    spriteBatch.DrawString(centurygothic, "(c) Kaddiska Skies of Azurya Pre-Aphla Build", new Vector2(240, 150), Color.Maroon);
                    spriteBatch.End();
                    #endregion
                    //drawing life bars
                    #region hp
                    spriteBatch.Begin();
                    //spriteBatch.Draw(hpBar, new Rectangle(160, 70, hpBar.Width, hpBar.Height), new Rectangle(0, 55, hpBar.Width, hpBar.Height), Color.Purple);
                    ////now we draw based on current life..or HP
                    spriteBatch.Draw(hpBar, new Rectangle(160, 70, hpBar.Width, hpBar.Height), new Rectangle(0, 55, hpBar.Width, hpBar.Height), Color.Gray);
                    spriteBatch.Draw(hpBar, new Rectangle(160, 70, (int)(hpBar.Width * ((float)HP / 100)), hpBar.Height), new Rectangle(0, 0, hpBar.Width, hpBar.Height), Color.Purple);
                    spriteBatch.End();
                    #endregion

                    #region mp
                    spriteBatch.Begin();
                    spriteBatch.Draw(mpBar, new Rectangle(300, 70, mpBar.Width, mpBar.Height), new Rectangle(0, 55, mpBar.Width, mpBar.Height), Color.Gray);
                    spriteBatch.Draw(hpBar, new Rectangle(300, 70, (int)(mpBar.Width * ((float)MP / 100)), mpBar.Height), new Rectangle(0, 0, mpBar.Width, mpBar.Height), Color.GreenYellow);
                    spriteBatch.End();
                    #endregion

                    //pause
                    if (isPaused == true)
                    {
                        spriteBatch.Begin();
                        spriteBatch.DrawString(centurygothic, "PAUSED", new Vector2(graphics.GraphicsDevice.DisplayMode.Height / 2.0f, graphics.GraphicsDevice.DisplayMode.Width / 2.0f), Color.Black);
                        spriteBatch.End();
                    }
                    break; 
                    #endregion
                case GameStateEnum.TitleScreen:
                    //check if it's the Title screen
                    DrawTitle(titleScreen.BackgroundTitleTexture);
                    break;
            }
            base.Draw(gameTime);
        }
        public void DrawWorld(Model world)
        {
            //we draw the world...
            foreach (ModelMesh mesh in world.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.FogEnabled = true;
                    effect.FogStart = 30.0f;//gameConstants.CameraHeight - 2000.0f;
                    effect.FogEnd = 200.0f;//gameConstants.CameraHeight + 1000.0f;
                    effect.PreferPerPixelLighting = true;
                    effect.FogColor = Color.Gray.ToVector3();
                    effect.World = Matrix.Identity;
                    effect.View = titleViewMatrix;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 1.1f, 900f);
                }
                graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
                mesh.Draw(SaveStateMode.SaveState);
            }
        }
        public void DrawTitle(Texture2D texture)
        {
            //we draw the world...
            foreach (ModelMesh mesh in titleScreen.TitleModels["Background"].Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    //effect.FogEnabled = true;
                    //effect.FogStart = 30.0f;//gameConstants.CameraHeight - 2000.0f;
                    //effect.FogEnd = 200.0f;//gameConstants.CameraHeight + 1000.0f;
                    effect.PreferPerPixelLighting = true;
                    //effect.FogColor = Color.Gray.ToVector3();
                    effect.World = Matrix.Identity;
                    effect.View = titleViewMatrix;
                    effect.LightingEnabled = true;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, GraphicsDevice.Viewport.AspectRatio, 1.1f, 900f);
                }
                graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
                mesh.Draw(SaveStateMode.SaveState);
            }
            //check if it's first option selected, then we highlight appropriately
            if (menuoptions == 1)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(centurygothic, "Start Single Player Demo", new Vector2(160, 55), Color.Yellow);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(centurygothic, "Start Single Player Demo", new Vector2(160, 55), Color.Black);
                spriteBatch.End();
            }
            //check if it's second option selected, then we highlight appropriately
            if (menuoptions == 2)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(centurygothic, "Options", new Vector2(160, 75), Color.Yellow);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(centurygothic, "Options", new Vector2(160, 75), Color.Black);
                spriteBatch.End();
            }

            //check if it's third option selected, then we highlight appropriately
            if (menuoptions == 3)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(centurygothic, "Exit", new Vector2(160, 95), Color.Yellow);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(centurygothic, "Exit", new Vector2(160, 95), Color.Black);
                spriteBatch.End();
            }
            
            //draw the title mesh
        }
        public void DrawBackground(SpriteBatch spriteBatch)
        {
            // Draw the texture, if it is still onscreen.
            if (ScreenPosition.Y < ScreenHeight)
            {
                spriteBatch.Draw(RoBackgroundTexture, ScreenPosition, null, Color.White, 0, Origin, 1, SpriteEffects.None, LayerDepth);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            spriteBatch.Draw(RoBackgroundTexture, ScreenPosition - TextureSize, null, Color.White, 0, Origin, 1, SpriteEffects.None, LayerDepth);
        }
        public void LoadBackground(GraphicsDevice graphicsDevice, Texture2D backgroundTexture)
        {
            RoBackgroundTexture = backgroundTexture;
            ScreenHeight = graphicsDevice.Viewport.Height;

            int screenWidth = graphicsDevice.Viewport.Width;
            // Set the origin so that we're drawing from the 
            // center of the top edge.
            Origin = new Vector2(RoBackgroundTexture.Width / 2, 0);
            // Set the screen position to the center of the screen.
            ScreenPosition = new Vector2(screenWidth / 2, ScreenHeight / 2);
            // Offset to draw the second texture, when necessary.
            TextureSize = new Vector2(0, RoBackgroundTexture.Height);
        }
        public void DrawScytheEx(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //we draw the model..loop through each mesh of the model too
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationZ(modelRotation) * Matrix.CreateTranslation(avatarEx.Position);
                }
                //draw the mesh :D
                mesh.Draw(SaveStateMode.SaveState);
            }
        }
        public  void DrawAvatarEx(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //we draw the model..loop through each mesh of the model too
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = Color.Azure.ToVector3();
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //draw the mesh :D
                graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
                mesh.Draw(SaveStateMode.SaveState);
            }
        }
        public static void DrawAvatarEx(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms,float rotation,Vector3 position)
        {
            //we draw the model..loop through each mesh of the model too
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(rotation) * Matrix.CreateTranslation(position);
                    //effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //draw the mesh :D
                mesh.Draw(SaveStateMode.SaveState);
            }
        }
        public static void SpinAvatarEx(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms, float rotation, Vector3 position)
        {
            //we draw the model..loop through each mesh of the model too
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position);
                    //effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //draw the mesh :D
                mesh.Draw();
            }
        }
        public static void DrawForceballEx(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms, float rotation, Vector3 position)
        {
            //we draw the model..loop through each mesh of the model too
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Alpha = 0.5f;
                    ////effect.AmbientLightColor = new Vector3(0.8f);
                    //effect.DiffuseColor = new Vector3(1.0f);
                    //effect.SpecularColor = new Vector3(0.2f);
                    effect.EmissiveColor = new Vector3(0.1f);
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(rotation) * Matrix.CreateTranslation(position);
                }
                //draw the mesh :D
                mesh.Draw(SaveStateMode.SaveState);
            }
        }  
        /// <summary>
        /// set up the position and velocity of the different obstacles..
        /// </summary>
        private void ResetObstacles()
        {
            float xStart;
            float yStart;

            for (int i = 0; i < gameConstants.NumObstacles; i++)
            {
                if (randomVar.Next(2) == 0)
                {
                    xStart = (float)-gameConstants.PlayFieldSizeX;
                }
                else
                {
                    xStart = (float)gameConstants.PlayFieldSizeX;
                }
                yStart = (float)randomVar.NextDouble() * gameConstants.PlayFieldSizeY;

                HillSideList[i].IsActive = true;
                HillSideList[i].Position = new Vector3(xStart, yStart, 0.0f);

                double angle = randomVar.NextDouble() * 2 * Math.PI;

                HillSideList[i].Direction.X = -(float)Math.Sin(angle);
                HillSideList[i].Direction.Y = (float)Math.Cos(angle);
                HillSideList[i].Speed = gameConstants.ObstacleMinimumSpeed + (float)randomVar.NextDouble() * gameConstants.ObstacleMaximumSpeed;
            }
        }
        #region old worthless code :P
        ///// <summary>
        ///// set up the position and velocity of the different Scenes...for aesthetic values only
        ///// </summary>
        //private void ResetScene()
        //{
        //    float xStart;
        //    float yStart;

        //    for (int i = 0; i <2; i++)
        //    {
        //        if (randomVar.Next(2) == 0)
        //        {
        //            xStart = (float)-gameConstants.PlayFieldSizeX;
        //        }
        //        else
        //        {
        //            xStart = (float)gameConstants.PlayFieldSizeX;
        //        }
        //        yStart = (float)randomVar.NextDouble() * gameConstants.PlayFieldSizeY;

        //        SceneList[i].IsActive = true;
        //        SceneList[i].Position = new Vector3(xStart, yStart, 0.0f);

        //        double angle = randomVar.NextDouble() * 2 * Math.PI;

        //        SceneList[i].Direction.X = -(float)Math.Sin(angle);
        //        SceneList[i].Direction.Y = (float)Math.Cos(angle);
        //        SceneList[i].Speed = gameConstants.ObstacleMinimumSpeed + (float)randomVar.NextDouble() * gameConstants.ObstacleMaximumSpeed;
        //    }
        //} 
        #endregion
        private void BeginPaused(bool UserInitiated)
        {
            isPaused = true;
            isPausedForguid = !UserInitiated;
        }
        private void EndPause()
        {
            //TODO: Resume audio
            isPausedForguid = false;
            isPaused = false;
        }
        private void CheckPauseKey()
        {
            bool isPauseKeyDownThisFrame = (Keyboard.GetState().IsKeyDown(Keys.P));
            if (!isPauseKeyDown && isPauseKeyDownThisFrame)
            {
                if (!isPaused)
                {
                    BeginPaused(true);
                }
                else
                {
                    EndPause();
                }
            }
            isPauseKeyDown = isPauseKeyDownThisFrame;
        }
        /// <summary>
        /// loads the Assets for the Demo Level
        /// </summary>
        public void LoadGameDemoAssets()
        {
            try
            {
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);
                //load uo the avatar - nazvi
                avatarEx.AvatarModel = Content.Load<Model>("Models//" + DefaultModel);
                //load up the hill model obstacle
                hillModel = Content.Load<Model>("Models//rock");
                //load up the force ball model
                forceBallModel = Content.Load<Model>("Models//forceball-nazvhy");

                //load the scythe
                scytheModel = Content.Load<Model>("Models//scythe");
                
                //load the world
                backgroundModel = Content.Load<Model>("Models//waterfall");

                HillModelTransforms = SetupEffectDefaults(hillModel);
                avatarEx.Transforms = SetupEffectDefaults(avatarEx.AvatarModel);
                ForceBallTransforms = SetupEffectDefaults(forceBallModel);
                ScytheTransforms = SetupEffectDefaults(scytheModel);

                //supposed to load the sound engine after this mafa' :D
                //Background = Content.Load<Texture2D>("Images\\" + DefaultBackground);

                //load the banner
                banner = Content.Load<Texture2D>("Images\\bannerx2c");
                //load the icons
                #region icons
                nazvhihp40x40 = Content.Load<Texture2D>("Images\\nazviico40x40");
                nazvhimp40x40 = Content.Load<Texture2D>("Images\\forceball40x40");
                #endregion

                //load hp-mp
                #region hp/mp bars
                hpBar = Content.Load<Texture2D>("Images\\hpbarx");
                mpBar = Content.Load<Texture2D>("Images\\hpbarx");
                #endregion

                //load the font
                centurygothic = Content.Load<SpriteFont>("Fonts//CenturyGothic");
                //
                //LoadBackground(GraphicsDevice, Background);
            }
            catch (Exception exxxxx)
            {
                File.AppendAllText("c:\\ro.azurya.log", DateTime.Now.ToString() + "\n\r#Error :" + exxxxx.Message + "\n\r\tStacktrace :" + exxxxx.StackTrace + "\n\r\n\r");
            }
        }
    }
}
