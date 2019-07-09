using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OtClientBot
{
    public class Post
    {

        public static IntPtr hWindow;




        #region Consts
        const int VK_ESCAPE = 0x1B;
        const int VK_RETURN = 0x0D;
         const int F1 = 0x70;
         const int F2 = 0x71;
         const int F3 = 0x72;
         const int F4 = 0x73;
         const int F5 = 0x74;
         const int F6 = 0x75;
         const int F7 = 0x76;
         const int F8 = 0x77;
         const int F9 = 0x78;
         const int F10 = 0x79;
         const int F11 = 0x7A;
         const int F12 = 0x7B;
        const int VK_LCONTROL = 162;
        const int VK_RCONTROL = 163;
        const int VK_LSHIFT = 160;
        const int VK_RSHIFT = 161;
         const int WM_CHAR = 0x0102;
         const int WM_KEYDOWN = 0x0100;
         const int WM_KEYUP = 0x0101;
         const int WM_SETTEXT = 0x0C;
         const int CONTROL = 0x11;
         const int SHIFT = 0x10;
         const int VK_LEFT = 0x25;
         const int VK_UP = 0x26;
         const int VK_RIGHT = 0x27;
         const int VK_DOWN = 0x28;

        const int VK_LBUTTON = 0x01;
        const int VK_RBUTTON = 0x02;

         const int WM_LBUTTONDOWN = 0x201;
         const int WM_LBUTTONUP = 0x202;
         const int WM_LBUTTONDBLCLK = 0x203;
         const int WM_RBUTTONDOWN = 0x204;
         const int WM_RBUTTONUP = 0x205;
         const int WM_RBUTTONDBLCLK = 0x206;



         const int WM_MOUSEMOVE = 0x200;

         const int MK_LBUTTON = 0x001;
         const int MK_RBUTTON = 0x002;

         const int MAPVK_VK_TO_VSC = 0x0;


        #endregion



        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ScreenToClient(IntPtr hWnd, out Point lpPoint);

        [DllImport("user32.dll")]
        static extern int MapVirtualKey(int uCode, int uMapType);



        public static Point getCurrentPos()
        {
            Point cpos;
            GetCursorPos(out cpos);
            ScreenToClient(hWindow, out cpos);
            return (cpos);
        }

        public static int MAKELPARAM(int LoWord, int HiWord)
        {
            //LoWord = LoWord + (Find.GetSnap().Location.X + Find.xWindowBorder); // 8);

            //HiWord = HiWord + (Find.GetSnap().Location.Y + Find.yWindowBorder); //31);


            return ((HiWord << 16) | (LoWord & 0xffff));
        }

        public static void rightClick( Point k)
        {
            //Program.Log(Find.GetSnap().Location.ToString());
            
            Program.Log("Right Click at: " + k.ToString());
            Point currentPos = getCurrentPos();
            int coordinates = MAKELPARAM(k.X, k.Y);
            PostMessage(hWindow, WM_MOUSEMOVE, 0, coordinates);
            PostMessage(hWindow, WM_RBUTTONDOWN, MK_RBUTTON, coordinates);
            PostMessage(hWindow, WM_MOUSEMOVE, MK_RBUTTON, coordinates);
            PostMessage(hWindow, WM_RBUTTONUP, 0, coordinates); // It works with WM_LBUTTONUP
            coordinates = MAKELPARAM(currentPos.X, currentPos.X);
            PostMessage(hWindow, WM_MOUSEMOVE, 0, coordinates);
        }



        public static void Esc()
        {
            KeyPress(VK_ESCAPE);
        }


        const int WM_SYSKEYUP = 0x105;
        const int WM_SYSKEYDOWN = 0x0104;

        const int VK_CONTROL = 0x11;
        const int VK_MENU = 0x12;
        public static void AltClick(Point k)
        {
            //Program.Log(Find.GetSnap().Location.ToString());

            Program.Log("Alt Click at: " + k.ToString());
            Point currentPos = getCurrentPos();

            _PostMessage(hWindow, WM_SYSKEYUP, VK_CONTROL, 0xE01D0001); // Important one

            // Key down
            PostMessage(hWindow, WM_KEYDOWN, VK_CONTROL, 0x001D0001);
            PostMessage(hWindow, WM_KEYDOWN, VK_MENU, 0x21380001);
            PostMessage(hWindow, WM_SYSKEYDOWN, VK_MENU, (MapVirtualKey(VK_MENU, MAPVK_VK_TO_VSC)) * 0x10000 + 1); // Important One
            
            int coordinates = MAKELPARAM(k.X, k.Y);
            PostMessage(hWindow, WM_MOUSEMOVE, 0, coordinates);

            PostMessage(hWindow, WM_LBUTTONDOWN, MK_LBUTTON, coordinates);


            PostMessage(hWindow, WM_MOUSEMOVE, MK_LBUTTON, coordinates);
            
            PostMessage(hWindow, WM_LBUTTONUP, 0, coordinates); 
            coordinates = MAKELPARAM(currentPos.X, currentPos.X);

            PostMessage(hWindow, WM_MOUSEMOVE, 0, coordinates);


            // Key Up
            _PostMessage(hWindow, WM_SYSKEYUP, VK_CONTROL, 0xE01D0001);
            _PostMessage(hWindow, WM_KEYUP, VK_MENU, 0xC1380001);
            Thread.Sleep(75); // This is important!
            PostMessage(hWindow, WM_SYSKEYUP, VK_MENU, (MapVirtualKey(VK_MENU, MAPVK_VK_TO_VSC)) * 0x10000 + 1); // When this is out, the game keeps the alt pressed.


        }

        public static void PressKeyWithAlt(int key, int lParamDown, int lParamUp)
        {
            PostMessage(hWindow, WM_KEYDOWN, VK_CONTROL, 0x001D0001);
            PostMessage(hWindow, WM_KEYDOWN, VK_MENU, 0x21380001);
            PostMessage(hWindow, WM_KEYDOWN, key, lParamDown);
            Thread.Sleep(1000);
            PostMessage(hWindow, WM_KEYUP, key, lParamUp);
            _PostMessage(hWindow, WM_SYSKEYUP, VK_CONTROL, 0xE01D0001);
            _PostMessage(hWindow, WM_KEYUP, VK_MENU, 0xC1380001);
        }




        static void _PostMessage(IntPtr hWnd, uint Msg, int wParam, uint lParam)
        {
            PostMessage(hWnd, Msg, wParam, unchecked((int)(lParam)));
        }


        public static void leftClick( Point k)
        {
            //Program.Log(Find.GetSnap().Location.ToString());
           

            //Program.Log("Click at: " + k.ToString());
            Point currentPos = getCurrentPos();
            int coordinates = MAKELPARAM(k.X, k.Y);
            PostMessage(hWindow, WM_MOUSEMOVE, 0, coordinates);
            PostMessage(hWindow, WM_LBUTTONDOWN, MK_LBUTTON, coordinates);
            PostMessage(hWindow, WM_MOUSEMOVE, MK_LBUTTON, coordinates);
            PostMessage(hWindow, WM_LBUTTONUP, 0, coordinates); // It works with WM_RBUTTONUP
            coordinates = MAKELPARAM(currentPos.X, currentPos.X);
            PostMessage(hWindow, WM_MOUSEMOVE, 0, coordinates);
        }

        public static void dragDrop(Point from, Point to)
        {
            Point currentPos = getCurrentPos();
            int coordinates = MAKELPARAM(from.X, from.X);

            PostMessage(hWindow, WM_MOUSEMOVE, 0, coordinates);

            PostMessage(hWindow, WM_LBUTTONDOWN, MK_LBUTTON, coordinates);
            coordinates = MAKELPARAM(to.X, to.Y);

            PostMessage(hWindow, WM_MOUSEMOVE, MK_LBUTTON, coordinates);

            PostMessage(hWindow, WM_LBUTTONUP, 0, coordinates);
            coordinates = MAKELPARAM(currentPos.X, currentPos.Y);

            PostMessage(hWindow, WM_MOUSEMOVE, 0, coordinates);
        }


        public static void DragMouse(Point from, Point to)
        {
            int LPFrom = MAKELPARAM(from.X, from.Y);
            int LPTo = MAKELPARAM(to.X, to.Y);
            Point PreCursor = new Point();
            GetCursorPos(out PreCursor);

            Point currentPos = getCurrentPos();

            int LPre = MAKELPARAM(currentPos.X, currentPos.Y);


            PostMessage(hWindow, WM_MOUSEMOVE, 0, LPFrom);
            PostMessage(hWindow, WM_LBUTTONDOWN, MK_LBUTTON, LPFrom);


            PostMessage(hWindow, WM_MOUSEMOVE, MK_LBUTTON, LPTo);
            PostMessage(hWindow, WM_LBUTTONUP, 0, LPTo);

            GetCursorPos(out PreCursor);

            PostMessage(hWindow, WM_MOUSEMOVE, 0, LPre);

        }




        public static void KeyUp(int vk_key)
        {
            int VirtualKey = (MapVirtualKey(vk_key, MAPVK_VK_TO_VSC) * 0x10000);
            int lparam = (int)((uint)(VirtualKey + 3221225473));
            PostMessage(hWindow, WM_KEYUP, vk_key, lparam);
        }

        public static void KeyDown(int vk_key)
        {
            PostMessage(hWindow, WM_KEYDOWN, vk_key,(MapVirtualKey(vk_key, MAPVK_VK_TO_VSC)) * 0x10000 + 1);
        }

        //public static void SuperKeyUp(int vk_key)
        //{
        //    int VirtualKey = (MapVirtualKey(vk_key, MAPVK_VK_TO_VSC) * 0x10000);
        //    int lparam = (int)((uint)(VirtualKey + 3221225473));
        //    PostMessage(hWindow, WM_KEYUP, vk_key, lparam);
        //}

        //public static void SuperKeyDown(int vk_key)
        //{
        //    PostMessage(hWindow, WM_KEYDOWN, vk_key, (MapVirtualKey(vk_key, MAPVK_VK_TO_VSC)) * 0x10000 + 1);
        //    PostMessage(hWindow, WM_SYSKEYDOWN, vk_key, (MapVirtualKey(vk_key, MAPVK_VK_TO_VSC)) * 0x10000 + 1);
        //}

        public static void KeyPress(int vk_key)
        {
            KeyDown( vk_key);
            KeyUp(vk_key);
        }






    }

}
