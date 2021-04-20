using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rockhoppers.scripts;

namespace Rockhoppers
{
    public class Game1 : Game
    { 
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D cursor_sprite;
        public static int counter = 0;



        private static Vector2 screenSize = new Vector2(1200,800);

        public static Vector2 ScreenSize { get => screenSize; }

        


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            

            Content.RootDirectory = "Content";
            IsMouseVisible = false;


           

            
        }
         
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = (int)screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)screenSize.Y;

            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach(string path in SpriteBucket.sprite_paths)
            {
                Texture2D sprite = Content.Load<Texture2D>(path);

                SpriteBucket.Sprites[path] = sprite;
            }

            foreach (string path in SpriteBucket.spriteFont_paths)
            {
                SpriteFont spritef = Content.Load<SpriteFont>(path);

                SpriteBucket.spriteFonts[path] = spritef;

            }


            cursor_sprite = SpriteBucket.Sprites["cursor"];
        }

        protected override void Update(GameTime gameTime)
        {
            if(! SceneManager.has_loaded)
            {
                SceneManager.InitializeScene();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            SceneManager.UpdateScene(gameTime);

           
            
           

            foreach(Entity e in SceneManager.entityList)
            {
                //System.Diagnostics.Debug.WriteLine(e.SpritePath);
            }

            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.BackToFront,null, SamplerState.PointClamp,null,null,null,null);
            
            foreach(Entity e in SceneManager.entityList)
            {
                e.Draw(_spriteBatch);
            }


            _spriteBatch.Draw(cursor_sprite, Input.mousePosition, null, Color.White, 0, Vector2.Zero, new Vector2(4,4), SpriteEffects.None, 0);
            _spriteBatch.End();

            SceneManager.CullEntities();
            //System.Diagnostics.Debug.WriteLine("Entities Drawn: " + counter);
            counter = 0;

            base.Draw(gameTime);
        }
    }
}
