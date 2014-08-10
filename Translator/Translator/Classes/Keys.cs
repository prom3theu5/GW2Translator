using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Utilities
{
    public static class Keys
    {
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lp1, string lp2);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void SendToGame(string text)
        {
            Clipboard.SetText(text);
            IntPtr handle = FindWindow(null, "Guild Wars 2");
            
            if (!handle.Equals(IntPtr.Zero))
            {
                if (SetForegroundWindow(handle))
                {

                    SendEnter(handle);
                    SendPaste(handle);
                    SendEnter(handle);
                }
            }
        }

        static void SendEnter(IntPtr Handle)
        {
            //Enter Key Down
            PostMessage(Handle, WM_KEYDOWN, 0x0D, 0x101c0001);
            Thread.Sleep(100);
            //Enter Key Up
            PostMessage(Handle, WM_KEYUP, 0x0D, 0x101c0001);
            Thread.Sleep(100);
        }

        static void SendPaste(IntPtr Handle)
        {
            //Press and Hold Control
            keybd_event(0xA2, 0, 0, 0);
            //Send V
            PostMessage(Handle, WM_KEYDOWN, 0x56, 0x002f0001);
            Thread.Sleep(100);
            //Release Control
            keybd_event(0xA2, 0, 2, 0);
            Thread.Sleep(100);
            //Simulate backspace to remove the V from the paste :/
            PostMessage(Handle, WM_KEYDOWN, 0x08, 0xe0001);
            Thread.Sleep(100);
            PostMessage(Handle, WM_KEYUP, 0x08, 0xe0001);
        }
    
    }
}
