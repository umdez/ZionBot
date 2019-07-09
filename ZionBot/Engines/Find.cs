using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using static OtClientBot.WinApi;


namespace OtClientBot
{
    public class Find
    {

        public static WindowSnap _snap;


        static DateTime lastTimeRequested = DateTime.Now;

        const double ExpirationTime = 1000;



        public static WindowSnap GetSnap()
        {
            var spanTime = DateTime.Now.Subtract(lastTimeRequested);
            ResetTimer();
            double elapsedTime = spanTime.TotalMilliseconds;
            if (elapsedTime > ExpirationTime || _snap == null )
            {
                UpdateSnap();
                return _snap;
            }
            else
            {
                return _snap;
            }            
        }


        public static void UpdateSnap()
        {
            _snap = WindowSnap.GetWindowSnap(Client.wHandle, true);
        }


        public static Point Position(int X, int Y)
        {
            return Position(new Location(X, Y, Player.Z));
        }
        public static Point Position(Location loc)
        {
            var p = new Point(0,0);
            
            int xOffset = loc.X - Player.X; // At first, these variables represents the difference
            int yOffset = loc.Y - Player.Y;

            if (loc.Z != Player.Z || Math.Abs(xOffset) > 7 || Math.Abs(yOffset) > 5 ) // se estiver fora da tela retorna
            {
                return p;
            }

            var rect = GameScreen();

            // Get size of tile
            double xStep = rect.Width / 15.0;
            double yStep = rect.Height / 11.0;


            // Get self pos
            var selfP = new Point();  
            selfP.X = rect.X + rect.Width / 2;
            selfP.Y = rect.Y + rect.Height / 2;



            p.X = (int)(selfP.X + (xOffset * xStep));
            p.Y = (int)(selfP.Y + (yOffset * yStep));
            


            return p;
        }

        public static Rectangle GameScreen()
        {
            {
                var rect = new Rectangle();

                if (Debugger.GuiObject != 0)
                {
                    rect.X = Memory.ReadInt(Debugger.GuiObject + (uint)Address.GuiObjectOffsets.X);
                    rect.Y = Memory.ReadInt(Debugger.GuiObject + (uint)Address.GuiObjectOffsets.Y);

                    // [[ Verificar se está funcionando ]]
                    rect.Width = Memory.ReadInt(Debugger.GuiObject + (uint)Address.GuiObjectOffsets.Width);
                    rect.Height = Memory.ReadInt(Debugger.GuiObject + (uint)Address.GuiObjectOffsets.Heigth);

                    //int a = Memory.ReadInt(Address.Client.GameGuiBase);
                    //int b = Memory.ReadInt(a);



                    //rect.Width = Memory.ReadInt(b+0x30);
                    //rect.Height = Memory.ReadInt(b+0x34);
                }

                return rect;
            }
        }

        public static Point Self()
        {
            {
                var point = new Point();

                var Rect = GameScreen();

                point.X = Rect.X + Rect.Width / 2;
                point.Y = Rect.Y + Rect.Height / 2;

                return point;
            }
        }







        static void ResetTimer()
        {
            lastTimeRequested = DateTime.Now;

        }

        private static int _xWindowBorder = -1;
        private static int _yWindowBorder = -1;


        public static int xWindowBorder
        {  get
            {
                if (_xWindowBorder == -1)
                {
                    var Winfo = GetWindowInfo();
                    _xWindowBorder = Winfo.rcClient.X - Winfo.rcWindow.X;
                }
                return _xWindowBorder;
            }
        }
        public static int yWindowBorder
        { get
            {
                if (_yWindowBorder == -1)
                {
                    var Winfo = GetWindowInfo();
                    _yWindowBorder = Winfo.rcClient.Y - Winfo.rcWindow.Y;
                }
                return _yWindowBorder;
            }
        }



        private static WINDOWINFO GetWindowInfo()
        {
            var winfo = new WINDOWINFO();
            WinApi.GetWindowInfo(Client.wHandle, ref winfo);
            return winfo;

        }



        static Point lastGameScreenWidthHeight = new Point(0, 0);
        static Point lastSnapLocation = new Point(0, 0);


        public static Color Color(Point p)
        {           
            LockBitmap tempNap = new LockBitmap(_snap.Image);

            tempNap.LockBits();

            Color color = tempNap.GetPixel(p.X, p.Y);

            tempNap.UnlockBits();


            return color;

        }






        public static bool Icon(Bitmap icon, out Point Point)
        {
            Point = Icon(icon);



            if (Point.X != 0 && Point.Y != 0)
            {
                return true;
            }else
            {
                return false;
            }

            
        }


        public static Point Icon(Bitmap icon)
        {
            var p = new Point();

            var snap = GetSnap();

            var Image = snap.Image;


            var IconRect = Utils.findBitmap(icon, Image, 0.8);

            if (IconRect.X == 0 || IconRect.Y == 0) return new Point(0,0); // If not found, return 0 0




            p.X = (snap.Location.X) + IconRect.X + IconRect.Width/2;
            p.Y = (snap.Location.Y) + IconRect.Y + IconRect.Height/2;

            return p;
        }












    }
}
