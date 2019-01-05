/*
 * Author:      Bill Sun
 * Class Name: 	GamePieces
 * Description: Custom class GamePiece which holds information about the particular game piece
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BSunAssignment3
{
    class GamePiece
    {
        private PIECES whichPiece;
        private Rectangle pieceRectangle;
        private int piecePosition;
        
        public GamePiece(Rectangle pieceRectangle, int piecePosition)
        {
            this.pieceRectangle = pieceRectangle; //initializes with a rectangle
            this.piecePosition = piecePosition; //in the 3x3 grid, which spot does this belong to?
            whichPiece = PIECES.PIECEEMPTY; //initalizes with empty game piece on board
        }
        
        //which piece is currently in this grid? will be used to determine whether to draw X or O
        public PIECES WhichPiece
        {
            get { return whichPiece; }
            set { whichPiece = value; }
        }

        //rectangle containing
        public Rectangle PieceRectangle
        {
            get { return pieceRectangle; }
            set { pieceRectangle = value; }
        }

        //getter only for position, as this will be set upon initialization and cannot be changed
        public int PiecePosition
        {
            get { return piecePosition; }
        }
    }
}
