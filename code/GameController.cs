/*
 * Author:      Bill Sun
 * Class Name: 	GameController.cs
 * Description: This class handles the game logic, such as player turn, winning conditions, user interface/messages
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BSunAssignment3
{
    class GameController : DrawableGameComponent
    {
        #region const variables
        private const int boardSizeX = 3;
        private const int boardSizeY = 3;
        private const int pieceSizeX = 130;
        private const int pieceSizeY = 130;
        #endregion

        private bool gameOver = false; //is the game over?
        private bool hasPlayerGone = false; //did the player go yet?
        private PIECES playerTurn; //which player is it?

        private List<GamePiece> gamePieces; //game pieces

        private SpriteBatch spriteBatch;
        private SpriteFont fontS16; //font size 16
        private SpriteFont fontS32b; //font size 32, bold
        private string gameMessage = "Welcome to Tic Tac Toe! Player (X) goes first!";

        private Vector2 mousePosition; //position of mouse
        private Rectangle currPieceRectangle; //current rectangle drawn around mmouse
        private int currCollidedPiece; //which piece of the 9 piece grid is intersecting?

        private const int displayEndingFor = 5000; //5 seconds, or 5000 milliseconds
        private int elapsedDisplayTime = 0; //elapsed time counter
        private int movesLeft = 9; //max number of moves possible is 9

        private Game game;

        public GameController(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            fontS16 = game.Content.Load<SpriteFont>("gameFont_s16"); //loading font
            fontS32b = game.Content.Load<SpriteFont>("gameFont_S32b"); //loading font

            playerTurn = PIECES.PIECEX; //begins with player 1, which is piece X

            gamePieces = new List<GamePiece>();

            //adding the 9 tiles for the tic tac toe grid into the list
            gamePieces.Add(new GamePiece(new Rectangle(74, 71, pieceSizeX, pieceSizeY), 0)); 
            gamePieces.Add(new GamePiece(new Rectangle(234, 71, pieceSizeX, pieceSizeY), 1));
            gamePieces.Add(new GamePiece(new Rectangle(394, 71, pieceSizeX, pieceSizeY), 2));
            gamePieces.Add(new GamePiece(new Rectangle(74, 236, pieceSizeX, pieceSizeY), 3));
            gamePieces.Add(new GamePiece(new Rectangle(234, 236, pieceSizeX, pieceSizeY), 4));
            gamePieces.Add(new GamePiece(new Rectangle(394, 236, pieceSizeX, pieceSizeY), 5));
            gamePieces.Add(new GamePiece(new Rectangle(74, 397, pieceSizeX, pieceSizeY), 6));
            gamePieces.Add(new GamePiece(new Rectangle(234, 397, pieceSizeX, pieceSizeY), 7));
            gamePieces.Add(new GamePiece(new Rectangle(394, 397, pieceSizeX, pieceSizeY), 8));
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState(); //grabbing mousestate
            mousePosition.X = mouse.X; //x position
            mousePosition.Y = mouse.Y; //y position

            currPieceRectangle = new Rectangle((int)MousePosition.X - 55, (int)MousePosition.Y - 55, 110, 110);

            if (gameOver == true) //if game has finished
            {
                //resetting the pieces on the board
                foreach (GamePiece piece in GamePieces) //go through list
                {
                    piece.WhichPiece = PIECES.PIECEEMPTY; //and reset all pieces back to empty, this way the draw part will know
                }
                movesLeft = 9;
                playerTurn = PIECES.PIECEX; //x's go first, so player 1
            }
            else
            {
                currCollidedPiece = -1; //set piece to one that can't exist
                foreach (GamePiece piece in GamePieces) //checking each piece to see if theres currently a intersect happening
                {
                    if (piece.PieceRectangle.Intersects(currPieceRectangle)) //if there is, set current piece to the intersecting one
                    {
                        currCollidedPiece = piece.PiecePosition;
                    }
                }

                if (mouse.LeftButton == ButtonState.Pressed) //if player has clicked mouse, this will allow player to drag mouse over grid and randomly draw X or Os
                {
                    if (gamePieces[currCollidedPiece].WhichPiece == PIECES.PIECEEMPTY) //if the selected piece is empty
                    {
                        gamePieces[currCollidedPiece].WhichPiece = playerTurn; //set the grid piece to current player piece
                        hasPlayerGone = true; //yes player has gone
                        movesLeft--; //negatively interate once every time a player has performed a move
                    }
                }

                if (CheckForWinner()) //calls check for winner to see if latest move resulted in a win
                {
                    hasPlayerGone = false; //set to false

                    if (PlayerTurn == PIECES.PIECEX) //which ever player just went and has triggered winning condition, wins
                    {
                        gameMessage = "Player (X) is the winner!";
                        gameOver = true;
                    }
                    else
                    {
                        gameMessage = "Player (O) is the winner!";
                        gameOver = true;
                    }
                }
                else if (movesLeft <= 0)
                {
                    gameMessage = "Boooo, no one won!";
                    gameOver = true;
                    movesLeft = 9; //resetting to 9 moves left
                }
                else
                {
                    if (playerTurn == PIECES.PIECEX && hasPlayerGone) //if it is X/player 1
                    {
                        gameMessage = "It is now Player (O)'s turn!"; //what turn
                        playerTurn = PIECES.PIECEO; //which player, x or o
                        hasPlayerGone = false; //reset has player moved to false
                    }
                    else if (playerTurn == PIECES.PIECEO && hasPlayerGone) //if it is O/player 2
                    {
                        gameMessage = "It is now Player (X)'s turn!";
                        playerTurn = PIECES.PIECEX;
                        hasPlayerGone = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //testing fonts
            spriteBatch.Begin();

            //drawing the game message, of which player's turn it is
            spriteBatch.DrawString(fontS16, gameMessage, new Vector2(64f, 20f), new Color(10, 11, 104));

            if (gameOver) 
            {
                //learned how to do this from here: https://stackoverflow.com/questions/5751732/draw-rectangle-in-xna-using-spritebatch
                Texture2D rect = new Texture2D(game.GraphicsDevice, 600, 600); //makes a new texture of 600x600, will be used to cover display window
                Color[] data = new Color[600 * 600]; //new Color array, of size 600x600 pixels
                Vector2 coor = new Vector2(0, 0); //start coordinates for rectangle
                for (int i = 0; i < data.Length; ++i) data[i] = new Color(239, 239, 231); //setting each pixel in Colo[] data to desired color
                rect.SetData(data); //change the texture's pixels
                spriteBatch.Draw(rect, coor, Color.White); //drawing said texture

                if (elapsedDisplayTime < displayEndingFor) //checking to see if 5 seconds are up, for displaying the winning message
                {
                    string secondsUntilRestart = "";
                    elapsedDisplayTime += gameTime.ElapsedGameTime.Milliseconds; //cumulative elapsed time
                    secondsUntilRestart = (((displayEndingFor - elapsedDisplayTime) / 1000) + 1).ToString(); //converts milliseconds to seconds for display

                    spriteBatch.DrawString(fontS32b, gameMessage, new Vector2(64f, 170f), new Color(10, 11, 104)); //display winner
                    spriteBatch.DrawString(fontS16, "Game is restarting in: ", new Vector2(64f, 230f), new Color(10, 11, 104)); //flavour text
                    //countdown until game restarts, i scaled this text up so it looks ugly
                    spriteBatch.DrawString(fontS32b, secondsUntilRestart + " . . .", new Vector2(250f, 250f), new Color(10, 11, 104), 0f, Vector2.One, 2f, SpriteEffects.None, 0f);
                }
                else //if 5 seconds are up display welcome message
                {
                    gameMessage = "Welcome to Tic Tac Toe! Player (X) goes first!";
                    elapsedDisplayTime = 0;
                    gameOver = false;
                }
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
        //setters/getters for gamepieces list
        public List<GamePiece> GamePieces
        {
            get { return gamePieces; }
            set { gamePieces = value; }
        }
        //getters only for mouse position
        public Vector2 MousePosition
        {
            get { return mousePosition; }
        }
        //getters for only which player turn it is
        public PIECES PlayerTurn
        {
            get { return playerTurn; }
        }
        //getters only for which rectangle
        public Rectangle CurrPieceRectangle
        {
            get { return currPieceRectangle; }
        }
        //getter only for which piece is currently intersecting
        public int CurrCollidedPiece
        {
            get { return currCollidedPiece; }
        }

        //logic for checking if either player won, please don't look at this its ugly and hurts my eyes
        public bool CheckForWinner()
        {
            bool winnerFound = false;

            if (//checking horizontal
                (gamePieces[0].WhichPiece == PIECES.PIECEO &&
                 gamePieces[1].WhichPiece == PIECES.PIECEO &&
                 gamePieces[2].WhichPiece == PIECES.PIECEO) ||
                (gamePieces[0].WhichPiece == PIECES.PIECEX &&
                 gamePieces[1].WhichPiece == PIECES.PIECEX &&
                 gamePieces[2].WhichPiece == PIECES.PIECEX) ||
                (gamePieces[3].WhichPiece == PIECES.PIECEO &&
                 gamePieces[4].WhichPiece == PIECES.PIECEO &&
                 gamePieces[5].WhichPiece == PIECES.PIECEO) ||
                (gamePieces[3].WhichPiece == PIECES.PIECEX &&
                 gamePieces[4].WhichPiece == PIECES.PIECEX &&
                 gamePieces[5].WhichPiece == PIECES.PIECEX) ||
                (gamePieces[6].WhichPiece == PIECES.PIECEO &&
                 gamePieces[7].WhichPiece == PIECES.PIECEO &&
                 gamePieces[8].WhichPiece == PIECES.PIECEO) ||
                (gamePieces[6].WhichPiece == PIECES.PIECEX &&
                 gamePieces[7].WhichPiece == PIECES.PIECEX &&
                 gamePieces[8].WhichPiece == PIECES.PIECEX) ||
                 //checking vertical
                 (gamePieces[0].WhichPiece == PIECES.PIECEO &&
                 gamePieces[3].WhichPiece == PIECES.PIECEO &&
                 gamePieces[6].WhichPiece == PIECES.PIECEO) ||
                (gamePieces[0].WhichPiece == PIECES.PIECEX &&
                 gamePieces[3].WhichPiece == PIECES.PIECEX &&
                 gamePieces[6].WhichPiece == PIECES.PIECEX) ||
                (gamePieces[1].WhichPiece == PIECES.PIECEO &&
                 gamePieces[4].WhichPiece == PIECES.PIECEO &&
                 gamePieces[7].WhichPiece == PIECES.PIECEO) ||
                (gamePieces[1].WhichPiece == PIECES.PIECEX &&
                 gamePieces[4].WhichPiece == PIECES.PIECEX &&
                 gamePieces[7].WhichPiece == PIECES.PIECEX) ||
                (gamePieces[2].WhichPiece == PIECES.PIECEO &&
                 gamePieces[5].WhichPiece == PIECES.PIECEO &&
                 gamePieces[8].WhichPiece == PIECES.PIECEO) ||
                (gamePieces[2].WhichPiece == PIECES.PIECEX &&
                 gamePieces[5].WhichPiece == PIECES.PIECEX &&
                 gamePieces[8].WhichPiece == PIECES.PIECEX) ||
                 //checking horizontal
                (gamePieces[0].WhichPiece == PIECES.PIECEO &&
                 gamePieces[4].WhichPiece == PIECES.PIECEO &&
                 gamePieces[8].WhichPiece == PIECES.PIECEO) ||
                (gamePieces[0].WhichPiece == PIECES.PIECEX &&
                 gamePieces[4].WhichPiece == PIECES.PIECEX &&
                 gamePieces[8].WhichPiece == PIECES.PIECEX) ||
                (gamePieces[2].WhichPiece == PIECES.PIECEO &&
                 gamePieces[4].WhichPiece == PIECES.PIECEO &&
                 gamePieces[6].WhichPiece == PIECES.PIECEO) ||
                (gamePieces[2].WhichPiece == PIECES.PIECEX &&
                 gamePieces[4].WhichPiece == PIECES.PIECEX &&
                 gamePieces[6].WhichPiece == PIECES.PIECEX)
                 )
            {
                winnerFound = true;
            }
            return winnerFound;
        }
    }
}
