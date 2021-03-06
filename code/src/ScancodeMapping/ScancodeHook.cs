/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Runtime.InteropServices;

namespace ScancodeHook
{
    namespace LowLevel
    {
        public interface IKeyScanSink
        {
            void HandleKeyScan(Int32 vkeycode, Int32 scancode, Int32 flags, Int32 time);
        }

        public class Keyboard
        {
            public const Int32 STANDARD = 0x00;
            public const Int32 EXTENDED = 0xE0;

            // Declare hook instance member variable.
            private static Keyboard theInstance = null;

            //Declare some hook constants.
            private const Int32 WH_KEYBOARD_LL = 13;
#pragma warning disable IDE0051 // Remove unused private members
            private const Int32 WM_KEYDOWN = 0x0100;
            private const Int32 WM_SYSKEYDOWN = 0x0104;
            private const Int32 WM_KEYUP = 0x0101;
            private const Int32 WM_SYSKEYUP = 0x0105;
#pragma warning restore IDE0051 // Remove unused private members

            // Declare further member variables.
            private Int32 hookHandle = 0;
            private HookProc hookProcedure = null;
            private IKeyScanSink scancodeSink = null;
            private Boolean disableNextHook = false;

            public delegate Int32 HookProc(Int32 nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern Int32 SetWindowsHookEx(Int32 idHook, HookProc lpHookProc, IntPtr hInstance, Int32 threadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern Boolean UnhookWindowsHookEx(Int32 idHook);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern Int32 CallNextHookEx(Int32 idHook, Int32 nCode, IntPtr wParam, IntPtr lParam);

            //Declare a wrapper managed KBDLLHOOKSTRUCT class.
            [StructLayout(LayoutKind.Sequential)]
            private struct KBDLLHOOKSTRUCT
            {
                public Int32 vkeycode;
                public Int32 scancode;
                public Int32 flags;
                public Int32 time;
                public IntPtr dwExtraInfo;
            }

            private Keyboard()
            {
            }

            public static Keyboard Instance
            {
                get
                {
                    if (null == Keyboard.theInstance)
                    {
                        Keyboard.theInstance = new Keyboard();
                    }

                    return Keyboard.theInstance;
                }
            }

            public static Boolean IsKeyUp(Int32 flags)
            {
                const Int32 KF_UP = 0x8000; // Taken from file 'winuser.h'

                if ((flags & (KF_UP >> 8)) == 0x80)
                {
                    return true; // Key is released.
                }
                else
                {
                    return false; // Key is pressed.
                }
            }

            public static Boolean IsExtended(Int32 flags)
            {
                const Int32 KF_EXTENDED = 0x0100; // Taken from file 'winuser.h'

                if ((flags & (KF_EXTENDED >> 8)) == 0x01)
                {
                    return true;  // Key needs the extended flag.
                }
                else
                {
                    return false;  // Key doesn't need an extended flag.
                }
            }

            public static Boolean IsSpecialLControl(Int32 vkeycode, Int32 scancode)
            {
                const Int32 VK_LCONTROL = 0xA2;   // Taken from file 'winuser.h'

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

            public Boolean Hooked
            {
                get { return this.hookHandle != 0; }
            }

            public void Hook(IKeyScanSink scancodeSink, Boolean disableNextHook)
            {
                this.disableNextHook = disableNextHook;
                this.Hook(scancodeSink);
            }

            public void Hook(IKeyScanSink scancodeSink)
            {
                if (null != scancodeSink)
                {
                    if (this.hookHandle == 0)
                    {
                        this.hookProcedure = new HookProc(Keyboard.HookCallback);

                        // REMARK: We need changes on the project setting to get this code running!
                        //         * Right click project in the solution explorer. 
                        //         * Go to properties and open Debug settings
                        //         * Uncheck "Enable visual studio hosting process"
                        this.hookHandle = Keyboard.SetWindowsHookEx(
                            WH_KEYBOARD_LL,
                            this.hookProcedure,
                            Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]),
                            0);

                        if (0 != this.hookHandle)
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

            public void Unhook()
            {
                if (this.hookHandle != 0)
                {
                    if (Keyboard.UnhookWindowsHookEx(this.hookHandle))
                    {
                        this.hookHandle = 0;
                    }
                    else
                    {
                        throw new InvalidOperationException("Could not execute function UnhookWindowsHookEx()!");
                    }
                }
            }

            private static Int32 HookCallback(Int32 nCode, IntPtr wParam, IntPtr lParam)
            {
                // REMARK: Do not distinguish between key press and key release!

                // Call scancode sink to publish current scancode.
                if (null != Keyboard.theInstance.scancodeSink)
                {
                    //Marshall data from wParam.
                    KBDLLHOOKSTRUCT llHook = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

                    // Update scancode mapper sink.
                    Keyboard.theInstance.scancodeSink.HandleKeyScan(
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
                        return Keyboard.CallNextHookEx(Keyboard.theInstance.hookHandle, nCode, wParam, lParam);
                    }

                    if (Keyboard.theInstance.disableNextHook)
                    {
                        return 1;   // Suppress call to next hook in the chain.
                    }
                }

                // Call next hook in the chain if it's call isn't suppressed.
                return Keyboard.CallNextHookEx(Keyboard.theInstance.hookHandle, nCode, wParam, lParam);
            }
        }
    }
}
