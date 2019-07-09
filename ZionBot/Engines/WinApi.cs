using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OtClientBot
{
    public static class WinApi
    {

        #region WindowInfo
        [Flags]
        public enum WindowStyles : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,

            WS_CAPTION = WS_BORDER | WS_DLGFRAME,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_CHILDWINDOW = WS_CHILD,
        }

        [Flags]
        public enum ExtendedWindowStyles : uint
        {
            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_MDICHILD = 0x00000040,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_WINDOWEDGE = 0x00000100,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_CONTEXTHELP = 0x00000400,
            WS_EX_RIGHT = 0x00001000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,
            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_APPWINDOW = 0x00040000,
            WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
            WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST)
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public WindowStyles dwStyle;
            public ExtendedWindowStyles dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public static uint GetSize()
            {
                return (uint)Marshal.SizeOf(typeof(WINDOWINFO));
            }
        }


        [DllImport("user32.dll")]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        #endregion














        [DllImport("kernel32.dll")]
        public static extern bool Thread32First(IntPtr hSnapshot, ref THREADENTRY32 lpte);


        [DllImport("kernel32.dll")]
        public static extern bool Thread32Next(IntPtr hSnapshot, out THREADENTRY32 lpte);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateToolhelp32Snapshot(int dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle,
    uint dwThreadId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle,
    uint dwThreadId);

        [DllImport("kernel32.dll")]
        public static extern uint TerminateThread(IntPtr hThread, int dwExitCode);


        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern bool DebugBreakProcess(IntPtr hProcess);


        [DllImport("kernel32.dll")]
        public static extern bool GetThreadContext(IntPtr hThread, ref ThreadContext lpContext);


        [DllImport("kernel32.dll")]
        public static extern bool SetThreadContext(IntPtr hThread,
            [In] ref ThreadContext lpContext);



        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess,out bool isDebuggerPresent);



        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(
          IntPtr hProcess,
          IntPtr lpThreadAttributes,
          uint dwStackSize,
          IntPtr lpStartAddress, // raw Pointer into remote process
          IntPtr lpParameter,
          uint dwCreationFlags,
          out uint lpThreadId
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetThreadPriority(IntPtr handle, int priority);



        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);


        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool DebugActiveProcess(int dwProcessId);
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool WaitForDebugEvent([Out] out DEBUG_EVENT lpDebugEvent, int dwMilliseconds);
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool ContinueDebugEvent(int dwProcessId, int dwThreadId, int dwContinueStatus);
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool IsDebuggerPresent();

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool DebugSetProcessKillOnExit(bool KillProcess);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool DebugActiveProcessStop(int dwProcessId);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeConsole();





        public static void SuspendProcess(Process process)
        {
            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread((uint)ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(Process process)
        {

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread((uint)ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                var suspendCount = 0;
                do
                {
                    suspendCount = (int)ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }


        const int TH32CS_SNAPTHREAD = 0x00000004;

        public static uint GetProcessThreadID(uint ProcessId)
        {
            THREADENTRY32 entry = new THREADENTRY32();

            entry.dwSize = 28;
            IntPtr snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);

            if (Thread32First(snapshot, ref entry) == true)
            {
                while (Thread32Next(snapshot, out entry) == true)
                {
                    if (entry.th32OwnerProcessID == ProcessId)
                    {
                        CloseHandle(snapshot);
                        return entry.th32ThreadID;
                    }
                }
            }
            CloseHandle(snapshot);
            return 0;
        }







        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct DEBUG_EVENT
        {
            public readonly uint dwDebugEventCode;
            public readonly int dwProcessId;
            public readonly int dwThreadId;


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 86, ArraySubType = UnmanagedType.U1)]
            public readonly byte[] debugInfo;


            public EXCEPTION_DEBUG_INFO Exception
            {
                get
                {
                    if (debugInfo == null)
                        return new EXCEPTION_DEBUG_INFO();


                    fixed (byte* ptr = debugInfo)
                    {
                        return *(EXCEPTION_DEBUG_INFO*)ptr;
                    }
                }
            }


            public LOAD_DLL_DEBUG_INFO LoadDll
            {
                get
                {
                    if (debugInfo == null)
                        return new LOAD_DLL_DEBUG_INFO();


                    fixed (byte* ptr = debugInfo)
                    {
                        return *(LOAD_DLL_DEBUG_INFO*)ptr;
                    }
                }
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct EXCEPTION_DEBUG_INFO
        {
            public EXCEPTION_RECORD ExceptionRecord;
            public readonly uint dwFirstChance;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct EXCEPTION_RECORD
        {
            public readonly uint ExceptionCode;
            public readonly uint ExceptionFlags;
            public readonly IntPtr ExceptionRecord;
            public readonly IntPtr ExceptionAddress;
            public readonly uint NumberParameters;


            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.U4)]
            //public readonly uint[] ExceptionInformation;


            public unsafe fixed uint ExceptionInformation[15];
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct FLOATING_SAVE_AREA
        {
            public uint ControlWord;
            public uint StatusWord;
            public uint TagWord;
            public uint ErrorOffset;
            public uint ErrorSelector;
            public uint DataOffset;
            public uint DataSelector;


            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            //public byte[] RegisterArea;


            public unsafe fixed byte RegisterArea[80];


            public uint Cr0NpxState;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct LOAD_DLL_DEBUG_INFO
        {
            public readonly IntPtr hFile;
            public readonly IntPtr lpBaseOfDll;
            public readonly uint dwDebugInfoFileOffset;
            public readonly uint nDebugInfoSize;
            public readonly IntPtr lpImageName;
            public readonly ushort fUnicode;
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct THREADENTRY32
        {
            internal UInt32 dwSize;
            internal readonly UInt32 cntUsage;
            internal readonly UInt32 th32ThreadID;
            internal readonly UInt32 th32OwnerProcessID;
            internal readonly UInt32 tpBasePri;
            internal readonly UInt32 tpDeltaPri;
            internal readonly UInt32 dwFlags;
        }


        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
        }

        [Flags]
        public enum FreeType
        {
            Decommit = 0x4000,
            Release = 0x8000,
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct ThreadContext
        {
            public uint ContextFlags; //set this to an appropriate value 
            // Retrieved by CONTEXT_DEBUG_REGISTERS 
            public uint Dr0;
            public uint Dr1;
            public uint Dr2;
            public uint Dr3;
            public uint Dr6;
            public uint Dr7;
            // Retrieved by CONTEXT_FLOATING_POINT 
            public FLOATING_SAVE_AREA FloatSave;
            // Retrieved by CONTEXT_SEGMENTS 
            public uint SegGs;
            public uint SegFs;
            public uint SegEs;
            public uint SegDs;
            // Retrieved by CONTEXT_INTEGER 
            public uint Edi;
            public uint Esi;
            public uint Ebx;
            public uint Edx;
            public uint Ecx;
            public uint Eax;
            // Retrieved by CONTEXT_CONTROL 
            public uint Ebp;
            public uint Eip;
            public uint SegCs;
            public uint EFlags;
            public uint Esp;
            public uint SegSs;
            // Retrieved by CONTEXT_EXTENDED_REGISTERS 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] ExtendedRegisters;
        }



                [DllImport("uxtheme.dll")]
        public static extern IntPtr SetWindowTheme(IntPtr hWnd, String
        pszSubAppName, String pszSubIdList);

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
        int nLeftRect, // x-coordinate of upper-left corner
        int nTopRect, // y-coordinate of upper-left corner
        int nRightRect, // x-coordinate of lower-right corner
        int nBottomRect, // y-coordinate of lower-right corner
        int nWidthEllipse, // height of ellipse
        int nHeightEllipse // width of ellipse
        );

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr dc);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(
             IntPtr hWndChild,      // handle to window
             IntPtr hWndNewParent   // new parent window
           );

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern void SetWindowText(IntPtr hWnd, string str);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool invert);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, uint hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);




        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder className, int maxCharCount);

        public const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        public const uint PROCESS_VM_READ = 0x0010;
        public const uint PROCESS_VM_WRITE = 0x0020;
        public const uint PROCESS_VM_OPERATION = 0x0008;
        public const uint PAGE_EXECUTE_READ = 0x20;
        public const uint PAGE_EXECUTE_READWRITE = 0x40;
        public const uint PAGE_READWRITE = 0x4;
        public const uint MEM_COMMIT = 0x1000;
        public const uint MEM_RESERVE = 0x2000;
        public const uint MEM_RELEASE = 0x8000;
        public const uint SWP_NOMOVE = 0x2;
        public const uint SWP_NOSIZE = 0x1;
        public const uint HWND_TOPMOST = 0xFFFFFFFF;
        public const uint HWND_NOTOPMOST = 0xFFFFFFFE;
        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;

        public const uint WM_LBUTTONDOWN = 0x201;
        public const uint WM_LBUTTONUP = 0x202;

        public const uint CREATE_SUSPENDED = 0x00000004;

        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        public struct SECURITY_ATTRIBUTES
        {
            public int length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CreateProcess(String imageName,
                String cmdLine,
                IntPtr lpProcessAttributes,
                IntPtr lpThreadAttributes,
                bool boolInheritHandles,
                uint dwCreationFlags,
                IntPtr lpEnvironment,
                String lpszCurrentDir,
                ref STARTUPINFO si,
                out PROCESS_INFORMATION pi);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int32 WaitForSingleObject(IntPtr Handle, UInt32 Wait);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress,
            IntPtr dwSize, uint flNewProtect, ref uint lpflOldProtect);
        
        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);


        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
           uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
           uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess,
           IntPtr lpThreadAttributes, uint dwStackSize, IntPtr
           lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        public static extern int MakeLParam(Int16 LoWord, Int16 HiWord);

         
       


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int X;
            public int Y;
            public int LowerRightX;
            public int LowerRightY;
        }

        public static int _MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }

        //the same function but with another name =D
        // just for understand the code better.
        public static int MakeWParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }





    }
}
