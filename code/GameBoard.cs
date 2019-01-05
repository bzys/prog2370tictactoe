/*
 * Author:      Bill Sun
 * Class Name: 	GameBoard.cs
 * Description: This class handles drawing out the games board and game pieces, like the 3x3 grid and which X or O is in which grid space
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
    class GameBoard : GameController
    {
        #region const variables
        private const int sizeX = 130;
        private const int sizeY = 130;
        #endregion

        private SpriteBatch spriteBatch;

        private Texture2D gameBoard; //the grid
        private Rectangle gameBoardRectangle; //size and position rectangle for game board

        private Texture2D pieceX; //holds texture for x
        private Texture2D pieceXShadow; //holds shadow texture for x
        private Texture2D pieceO; //holds texture for o
        private Texture2D pieceOShadow; //holds shadow texture for o
        private Texture2D currPiece; //texture for current piece, which is on user mouse position
        private Texture2D currPieceShadow; //texture for current piece's shadow

        private Game game;

        public GameBoard(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            gameBoard = game.Content.Load<Texture2D>("ticTacToeGrid"); //loads stuff into content manager
            gameBoardRectangle = new Rectangle(44, 44, 512, 512); //the game board rectangle and starting position

            pieceX = game.Content.Load<Texture2D>("ticTacToeX"); //loading the x and o's
            pieceXShadow = game.Content.Load<Texture2D>("ticTacToeXShadow");
            pieceO = game.Content.Load<Texture2D>("ticTacToeO");
            pieceOShadow = game.Content.Load<Texture2D>("ticTacToeOShadow");
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //draws the game board
            spriteBatch.Draw(gameBoard, gameBoardRectangle, Color.White);

            if (CurrCollidedPiece != -1) //determines which piece is intersecting, -1 is just some random value, anything from 0-8 means is intersecting
            {
                GamePiece gridHiLi = GamePieces[CurrCollidedPiece]; //which piece to highlight

                //learned how to do this from here: https://stackoverflow.com/questions/5751732/draw-rectangle-in-xna-using-spritebatch
                //the below code is the same one as found in gamecontroller, except this draws a red or green square whereever the user hovers
                //to show if the space is valid or not as a visual aid
                Texture2D rect = new Texture2D(game.GraphicsDevice, 130, 130); //rectangle size
                Color[] data = new Color[130 * 130];
                Vector2 coor = new Vector2(gridHiLi.PieceRectangle.X, gridHiLi.PieceRectangle.Y);

                if (gridHiLi.WhichPiece == PIECES.PIECEEMPTY) //if the hovered over grid is empty, make it green
                {
                    for (int i = 0; i < data.Length; ++i) data[i] = Color.LightGreen;
                    rect.SetData(data);
                }
                else //if there is a piece in that grid, make it red
                {
                    for (int i = 0; i < data.Length; ++i) data[i] = Color.OrangeRed;
                    rect.SetData(data);
                }
                spriteBatch.Draw(rect, coor, Color.White); //draw the grid
            }

            foreach (GamePiece piece in GamePieces) //draws out the list of tiles, if it is empty, it is just not drawn
            {
                Rectangle pieceRectangle;

                //spriteBatch.DrawRectangle(piece.PieceRectangle, Color.Red);

                if (piece.WhichPiece == PIECES.PIECEX) //if the current player held piece is a X
                {
                    //creating a new rectangle to hold the x, smaller than the collision rectangle
                    pieceRectangle = new Rectangle(piece.PieceRectangle.X + 10, piece.PieceRectangle.Y + 10, 110, 110);
                    spriteBatch.Draw(pieceX, pieceRectangle, Color.White);
                }
                else if (piece.WhichPiece == PIECES.PIECEO) //if the current player held piece is a O
                {
                    //creating a new rectangle to hold the o, smaller than the collision rectangle
                    pieceRectangle = new Rectangle(piece.PieceRectangle.X + 10, piece.PieceRectangle.Y + 10, 110, 110);
                    spriteBatch.Draw(pieceO, pieceRectangle, Color.White);
                }
            }

            if (PlayerTurn == PIECES.PIECEX) //sets which texture for the user held piece
            {
                currPiece = pieceX;
                currPieceShadow = pieceXShadow;
            }
            else
            {
                currPiece = pieceO;
                currPieceShadow = pieceOShadow;
            }

            //draws out the actual player held piece, offset so the center of square is where the mouse is
            //the shadow is drawn first, as it needs to appear under the X or O
            spriteBatch.Draw(currPieceShadow, new Rectangle((int)MousePosition.X - 50, (int)MousePosition.Y - 53, 110, 110), Color.White);
            spriteBatch.Draw(currPiece, CurrPieceRectangle, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
