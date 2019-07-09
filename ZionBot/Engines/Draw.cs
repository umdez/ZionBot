using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OtClientBot
{
    public class Draw
    {

        static Board DrawBoard; 



        public static void LoadBoard()
        {
            if (DrawBoard != null) { DrawBoard.g.Clear(Color.Lime); return; }

            DrawBoard = new Board(Client.wHandle);

            DrawBoard.Show();

            DrawBoard.Visible = true;

        }

        public static void ClearBoard()
        {
            DrawBoard.g.Clear(Color.Lime);

        }
        

        public static void Point(Point p)
        {
            Pen selPen = new Pen(Color.Blue);
            DrawBoard.g.DrawRectangle(selPen, p.X - 1, p.Y - 1, 3, 3);

        }


        public static void GameRectangle(bool drawTileLines)
        {
            if (DrawBoard == null || DrawBoard.IsDisposed) LoadBoard();
           
            var GameWindow = Find.GameScreen();

            Pen selPen = new Pen(Color.Red);
            DrawBoard.g.DrawRectangle(selPen, GameWindow);


            Pen WhitePen = new Pen(Color.White);


            double xStep = GameWindow.Width / 15.0;

            double yStep = GameWindow.Height / 11.0;



            int index = 1;

            if (drawTileLines)
            {
                while (index < 15) // Draw vertical lines
                {
                    double xOffset = GameWindow.X + index * xStep;

                    int yTop = GameWindow.Top;
                    int yBot = GameWindow.Bottom;



                    DrawBoard.g.DrawLine(WhitePen, (float)xOffset, yTop, (float)xOffset, yBot);

                    index++;
                }


                index = 1;

                while (index < 11) // Draw Horizontal lines
                {
                    double yOffset = GameWindow.Y + index * yStep;

                    int xLeft = GameWindow.Left;
                    int xRight = GameWindow.Right;

                    DrawBoard.g.DrawLine(WhitePen, xLeft, (float)yOffset, xRight, (float)yOffset);

                    index++;

                }


            }



        }





    }
}
