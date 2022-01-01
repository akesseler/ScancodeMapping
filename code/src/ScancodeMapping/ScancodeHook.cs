using System;
using System.Runtime.InteropServices;   // DllImport()

namespace ScancodeHook
{
    namespace LowLevel
    {
        /// <summary>
        /// 
        /// </summary>
        public interface IKeyScanSink
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="keyPad"></param>
            /// <param name="vkeycode"></param>
            /// <param name="scancode"></param>
            /// <param name="flags"></param>
            void HandleKeyScan(
                int vkeycode,
                int scancode, 
                int flags,
                int time
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public class Keyboard
        {
            public const int STANDARD = 0x00;
            public const int EXTENDED = 0xE0;

            // Declare hook instance member variable.
            private static Keyboard theInstance = null;

            //Declare some hook constants.
            private const int WH_KEYBOARD_LL = 13;
            private const int WM_KEYDOWN = 0x0100;
            private const int WM_SYSKEYDOWN = 0x0104;
            private const int WM_KEYUP = 0x0101;
            private const int WM_SYSKEYUP = 0x0105;

            // Declare further member variables.
            private int hookHandle = 0;
            private HookProc hookProcedure = null;
            private IKeyScanSink scancodeSink = null;
            private bool disableNextHook = false;

            /// <summary>
            /// Define hook callback function layout.
            /// </summary>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

            /// <summary>
            /// This is the Import for the SetWindowsHookEx function.
            /// Use this function to install a thread-specific hook.
            /// </summary>
            /// <param name="idHook"></param>
            /// <param name="lpHookProc"></param>
            /// <param name="hInstance"></param>
            /// <param name="threadId"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int SetWindowsHookEx(int idHook, HookProc lpHookProc, IntPtr hInstance, int threadId);

            /// <summary>
            /// This is the Import for the UnhookWindowsHookEx function.
            /// Call this function to uninstall the hook.
            /// </summary>
            /// <param name="idHook"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern bool UnhookWindowsHookEx(int idHook);

            /// <summary>
            /// This is the Import for the CallNextHookEx function.
            /// Use this function to pass the hook information to the next hook procedure in chain.
            /// </summary>
            /// <param name="idHook"></param>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

            //Declare a wrapper managed KBDLLHOOKSTRUCT class.
            [StructLayout(LayoutKind.Sequential)]
            private class KBDLLHOOKSTRUCT
            {
                public int    vkeycode;
                public int    scancode;
                public int    flags;
                public int    time;
                public IntPtr dwExtraInfo;
            }

            /// <summary>
            /// Singelton construction. 
            /// </summary>
            private Keyboard()
            {
            }

            /// <summary>
            /// Singleton instance getter. 
            /// </summary>
            public static Keyboard Instance
            {
                get
                {
                    if (null == theInstance)
                    {
                        theInstance = new Keyboard();
                    }
                    return theInstance;
                }
            }

            //
            // Summary:
            //
            public static bool IsKeyUp(int flags)
            {
                const int KF_UP = 0x8000; // Taken from file 'winuser.h'

                if ((flags & (KF_UP >> 8)) == 0x80)
                {
                    return true; // Key is released.
                }

                else
                {
                    return false; // Key is pressed.
                }
            }

            //
            // Summary:
            //
            public static bool IsExtended(int flags)
            {
                const int KF_EXTENDED = 0x0100; // Taken from file 'winuser.h'

                if ((flags & (KF_EXTENDED >> 8)) == 0x01)
                {
                    return true;  // Key needs the extended flag.
                }
                else
                {
                    return false;  // Key doesn't need an extended flag.
                }
            }

            //
            // Summary:
            //
            public static bool IsSpecialLControl(int vkeycode, int scancode)
            {
                const int VK_LCONTROL = 0xA2;   // Taken from file 'winuser.h'

                // ATTENTION: Problem with scancode of VK_RMENU!
                //            With right alt key (VK_RMENU) could be seen that beforehand 
                //            a VK_LCONTROL with flags set to 0x20 is alwasy send. In that 
                //            case the scancode was set to 0x021D! The expected right alt 
                //            key (VK_RMENU, flags=0x21) arrived only in the second step!

                // Ignore this according to explanations above!
                if ((VK_LCONTROL == vkeycode) && (0x021D == scancode))
                {
                    // 0x021D means "Don't care bit" is used and this means don't distinguish 
                    // between left and right keyboard keys. This bit is not only set 
                    // for VK_LCONTROL message. It is also set for VK_RMENU message!
                    // In that case parameter 'flags' is set to 0x0021 for VK_RMENU 
                    // message! VK_LCONTROL message uses instead the 'flags' parameter 
                    // set to 0x0020.
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //
            // Summary:
            //
            public bool Hooked
            {
                get { return (hookHandle != 0); }
            }

            //
            // Summary:
            //
            public void Hook(IKeyScanSink scancodeSink, bool disableNextHook)
            {
                this.disableNextHook = disableNextHook;
                this.Hook(scancodeSink);
            }

            /// <summary>
            /// Hooks low-level keyboard input. 
            /// </summary>
            /// <param name="scancodeSink"></param>
            /// <param name="keyPad"></param>
            public void Hook(IKeyScanSink scancodeSink)
            {
                if (null != scancodeSink)
                {
                    if (hookHandle == 0)
                    {
                        hookProcedure = new HookProc(Keyboard.HookCallback);

                        // REMARK: We need changes on the project setting to get this code running!
                        //         * Right click project in the solution explorer. 
                        //         * Go to properties and open Debug settings
                        //         * Uncheck "Enable visual studio hosting process"
                        hookHandle = SetWindowsHookEx(
                            WH_KEYBOARD_LL,
                            hookProcedure,
                            Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]),
                            0);

                        if (0 != hookHandle)
                        {
                            // Save given scancode sink and key pad to be used.
                            this.scancodeSink = scancodeSink;
                        }
                        else
                        {
                            throw new InvalidOperationException("Could not execute function SetWindowsHookEx()!");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Hook is already in use!");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Given parameter \"Scancode Sink\" is invalid!");
                }
            }

            /// <summary>
            /// Unhook low-level keyboard input. 
            /// </summary>
            public void Unhook()
            {
                if (hookHandle != 0)
                {
                    if (UnhookWindowsHookEx(hookHandle))
                    {
                        hookHandle = 0;
                    }
                    else
                    {
                        throw new InvalidOperationException("Could not execute function UnhookWindowsHookEx()!");
                    }
                }
            }

            /// <summary>
            /// Hook callback implementation. 
            /// </summary>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            private static int HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
            {
                // REMARK: Do not distinguish between key press and key release!

                // Call scancode sink to publish current scancode.
                if (null != theInstance.scancodeSink)
                {
                    //Marshall data from wParam.
                    KBDLLHOOKSTRUCT llHook = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

                    // Update scancode mapper sink.
                    theInstance.scancodeSink.HandleKeyScan(
                        llHook.vkeycode,
                        llHook.scancode,
                        llHook.flags,
                        llHook.time
                    );

                    // It is highly recommended to reflect those keyboard keys to the system!
                    if (llHook.vkeycode == ScancodeMapping.VirtualKeys.VK_NUMLOCK ||
                        llHook.vkeycode == ScancodeMapping.VirtualKeys.VK_CAPITAL ||
                        llHook.vkeycode == ScancodeMapping.VirtualKeys.VK_SCROLL)
                    {
                        // Call next hook in the chain if it's call isn't suppressed.
                        return CallNextHookEx(theInstance.hookHandle, nCode, wParam, lParam);
                    }

                    if (theInstance.disableNextHook)
                    {
                        return 1;   // Suppress call to next hook in the chain.
                    }
                }

                // Call next hook in the chain if it's call isn't suppressed.
                return CallNextHookEx(theInstance.hookHandle, nCode, wParam, lParam);
            }
        }
    }
}
