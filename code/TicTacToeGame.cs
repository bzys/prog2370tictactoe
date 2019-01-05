/*
 *  Author:     Bill Sun
 *  Date:       2018-11-22
 *  Class:      PROG2370
 *  Assignment: Assignment 3
 *  About:      TicTacToe game made using Monogame framework, I do not claim credit for auto-generated code, etc.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BSunAssignment3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TicTacToeGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameController gameManager;
        GameBoard gameBoard;

        Color backColor = new Color(239, 239, 231); //background color

        public TicTacToeGame()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = 600; //window height
            graphics.PreferredBackBufferWidth = 600; //window width
            graphics.ApplyChanges();

            this.Window.Title = "BSunAssignment3 - Tic Tac Toe"; //window name

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //adding game manager class
            gameManager = new GameController(this, spriteBatch);
            this.Components.Add(gameManager);
            //adding game board class
            gameBoard = new GameBoard(this, spriteBatch);
            this.Components.Add(gameBoard);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backColor);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
