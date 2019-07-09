using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtClientBot
{
    public partial class Board : Form
    {

        public Graphics g;

        IntPtr thisHandle;

        //IntPtr parentHandle;

        IntPtr targetHandle;

       // Form parentObject;

        public Thread FocusChecker;

        bool did=false;


        public bool CanDraw = true;


        //// TEST

        //private System.Windows.Forms.Button button1;



        //private void IniComponent()
        //{
        //    this.button1 = new System.Windows.Forms.Button();

        //    // 
        //    // button1
        //    // 
        //    this.button1.Location = new System.Drawing.Point(12, 38);
        //    this.button1.Name = "button1";
        //    this.button1.Size = new System.Drawing.Size(75, 23);
        //    this.button1.TabIndex = 0;
        //    this.button1.Text = "Send";
        //    this.button1.UseVisualStyleBackColor = true;
        //    this.button1.BackColor = Color.Blue;

        //}

        ////TEST


        public Board(IntPtr targetWindowHandle)//, Form parentObject)
        {
            //this.parentObject = parentObject;
            thisHandle = this.Handle;

            //parentHandle = parentObject.Handle;

            var screenRect = Screen.PrimaryScreen.WorkingArea;

            this.ShowInTaskbar = false;

            this.FormBorderStyle = FormBorderStyle.None;
            this.Width = screenRect.Width;
            this.Height = screenRect.Height;
            this.Location = new Point(0, 20);
            this.TopMost = true;            
            var transparentColor = Color.Lime;
            this.BackColor = transparentColor;
            this.TransparencyKey = transparentColor;

            this.g = this.CreateGraphics();

            targetHandle = targetWindowHandle;

            FocusChecker = new Thread(FocusLoop) { IsBackground = true };
            //FocusChecker.Start();

            this.thisHandle = this.Handle;

        }




        delegate void _action();

        delegate void _SetProp(object Instance, object Value);



        PropertyInfo _Visible = PropertyHelper<Form>.GetProperty(x => x.Visible);

        private void FocusLoop()
        {
            bool Activated = true;
            bool waitTarget=true;

            this.Invoke(new _action(this.Show));
            while (Activated)
            {
                Thread.Sleep(500);                
                IntPtr FocusedHandle = WinApi.GetForegroundWindow();

                if (waitTarget)
                {
                    if (FocusedHandle == targetHandle)
                        waitTarget = false;


                    continue;
                }

                if ((FocusedHandle == this.targetHandle || FocusedHandle == this.thisHandle))
                {
                    this.CanDraw = true;          
                }
                else
                {
                    this.CanDraw = false;
                    this.g.Clear(Color.Lime);
                }
            }
        }

    }





    public static class PropertyHelper<T>
    {
        public static PropertyInfo GetProperty<TValue>(
            Expression<Func<T, TValue>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }



}
