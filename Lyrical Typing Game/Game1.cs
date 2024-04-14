using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Lyrical_Typing_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static GameWindow gameWindow;

        List<Level> levels = new List<Level>();
        Level currentLevel;

        private bool start = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gameWindow = Window;

            // Create level for each detected song csv
            foreach (string songPath in System.IO.Directory.GetFiles("songs"))
            {
                if (System.IO.Path.GetExtension(songPath).Equals(".csv"))
                {
                    levels.Add(new Level(new Song(System.IO.Path.GetFileNameWithoutExtension(songPath),songPath, Content), "Bob", Content));
                }
            }

            // Level selector is WIP
            currentLevel = levels.First();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Currently this is just used to start the music, this should instead be a switch statement for various game states
            if (start)
            {
                currentLevel.Start();
                start = false;
            }

            currentLevel.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            currentLevel.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
