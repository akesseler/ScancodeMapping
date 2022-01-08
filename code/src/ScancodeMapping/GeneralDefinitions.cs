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
using System.Text;

namespace ScancodeMapping
{
    public enum TKeyboardLayout
    {
        Standard = 0,
        Enhanced = 1,
        Default = Enhanced
    }

    public enum TKeyboardKeys
    {
        Keys101 = 0,
        Keys102 = 1,
        Keys105 = 2,
        Keys106 = 3,
        Default = Keys106
    }

    public enum TKeyboardAlignment
    {
        Germany = 0,
        USStandard = 1,
        Default = Germany
    }

    public static class Win32Wrapper
    {
        public static String GetKeyboardLayoutName()
        {
            const Int32 KL_NAMELENGTH = 9; // WinUser.h
            String theResult = String.Empty;

            // Create a buffer and call Win32 API to obtain the keyboard layout name.
            StringBuilder lpString = new StringBuilder(KL_NAMELENGTH);
            if (GetKeyboardLayoutName(lpString))
            {
                theResult = lpString.ToString();
            }
            return theResult;
        }

        public static String GetKeyNameText(KeyScan keyScan, String original)
        {
            String theResult = original;

            if (null != keyScan)
            {
                if (VirtualKeys.Display(keyScan.VKeyCode) != String.Empty)
                {
                    theResult = VirtualKeys.Display(keyScan.VKeyCode);
                }
                else
                {
                    theResult = GetKeyNameText(keyScan);
                }
            }
            return theResult;
        }

        public static String GetKeyNameText(KeyScan keyScan)
        {
            String theResult = String.Empty;
            Int32 lParam = 0;

            if (null != keyScan)
            {
                if (keyScan.VKeyCode == VirtualKeys.VK_PRIOR ||
                    keyScan.VKeyCode == VirtualKeys.VK_NEXT ||
                    keyScan.VKeyCode == VirtualKeys.VK_END ||
                    keyScan.VKeyCode == VirtualKeys.VK_HOME ||
                    keyScan.VKeyCode == VirtualKeys.VK_LEFT ||
                    keyScan.VKeyCode == VirtualKeys.VK_UP ||
                    keyScan.VKeyCode == VirtualKeys.VK_RIGHT ||
                    keyScan.VKeyCode == VirtualKeys.VK_DOWN ||
                    keyScan.VKeyCode == VirtualKeys.VK_SNAPSHOT ||
                    keyScan.VKeyCode == VirtualKeys.VK_INSERT ||
                    keyScan.VKeyCode == VirtualKeys.VK_DELETE ||
                    keyScan.VKeyCode == VirtualKeys.VK_LWIN ||
                    keyScan.VKeyCode == VirtualKeys.VK_RWIN ||
                    keyScan.VKeyCode == VirtualKeys.VK_APPS ||
                    keyScan.VKeyCode == VirtualKeys.VK_DIVIDE ||
                    keyScan.VKeyCode == VirtualKeys.VK_NUMLOCK ||
                    keyScan.VKeyCode == VirtualKeys.VK_RCONTROL)
                {
                    lParam = keyScan.Scancode | 0x0100; // Set extended bit            
                }
                else if (keyScan.VKeyCode == VirtualKeys.VK_LCONTROL ||
                         keyScan.VKeyCode == VirtualKeys.VK_RSHIFT ||
                         keyScan.VKeyCode == VirtualKeys.VK_LMENU)
                {
                    lParam = keyScan.Scancode | 0x2000; // Set "don't care" bit.
                }
                else if (keyScan.VKeyCode == VirtualKeys.VK_RMENU)
                {
                    lParam = keyScan.Scancode | 0x2100; // Set "don't care" bit and extended bit.
                }
                else
                {
                    lParam = keyScan.Scancode; // Use scancode only.
                }

                // Create a buffer and call Win32 API to obtain the display text.
                StringBuilder lpString = new StringBuilder(100);
                Int32 nSize = lpString.Capacity;
                if (0 != GetKeyNameText((lParam << 16), lpString, nSize))
                {
                    theResult = lpString.ToString();

                    // German OEM adaption for death keys
                    if (theResult == "ZIRKUMFLEX")
                    {
                        theResult = "^";
                    }
                    else if (theResult == "AKUT")
                    {
                        theResult = "´";
                    }
                }
            }
            return theResult;
        }

        public static class KeyText
        {
            // Some useful links of different keyboard layouts!
            // http://www.microsoft.com/resources/msdn/goglobal/keyboards/kbdus.htm  (US)
            // http://www.microsoft.com/resources/msdn/goglobal/keyboards/kbdusa.htm (US, IBM Arabic 238_L)
            // http://www.microsoft.com/resources/msdn/goglobal/keyboards/kbdgr1.htm (DE, IBM)
            // http://www.microsoft.com/resources/msdn/goglobal/keyboards/kbdgr.htm  (DE)

            private const Int32 KLF_NOTELLSHELL = 0x00000080;  // winuser.h
#pragma warning disable IDE0051 // Remove unused private members
            private const Int32 MAPVK_VK_TO_VSC = 0;  // winuser.h
#pragma warning restore IDE0051 // Remove unused private members

            private static Int32 hLayout = 0; // Handle to loaded layout.

            public static String GetKeyTextAtOnce(TKeyboardAlignment alignment, KeyScan keyScan, String original)
            {
                String result;

                KeyText.Load(alignment);

                result = GetKeyText(keyScan);

                if (result == String.Empty)
                {
                    result = GetKeyNameText(keyScan, original);
                }

                KeyText.Unload();

                return result;
            }

            public static void Load(TKeyboardAlignment alignment)
            {
                String layout = String.Empty;

                if (hLayout == 0)
                {
                    // Check given layout parameter.
                    switch (alignment)
                    {
                        case TKeyboardAlignment.Germany:
                            layout = "00000407";
                            break;
                        case TKeyboardAlignment.USStandard:
                            layout = "00000409";
                            break;
                        default:
                            throw new NotSupportedException("Given alignment " + alignment + " is not supported!");
                    }

                    // Load requested keyboard layout.
                    hLayout = LoadKeyboardLayout(layout, KLF_NOTELLSHELL);
                }
            }

            public static String GetKeyText(KeyScan keyScan)
            {
                if (hLayout == 0)
                {
                    throw new ArgumentException("The keyboard layout is not yet loaded!");
                }

                String theResult = String.Empty;
                UInt16 theValue = 0;
                Char theReturn = '\0';
                Byte[] keyState = new Byte[256];

                Int32 result = Win32Wrapper.ToAsciiEx(keyScan.VKeyCode, keyScan.Scancode, keyState, ref theValue, 0, hLayout);

                if (result != 0)
                {
                    if (result == 2) // Two characters have been returned.
                    {
                        theReturn = Convert.ToChar(theValue & 0x00FF);
                        theResult = theReturn.ToString();
                        theReturn = Convert.ToChar(theValue >> 8);
                        theResult += theReturn.ToString();
                    }
                    else // Either a dead character (-1) or a normal character (1) has been returned.
                    {
                        theReturn = Convert.ToChar(theValue);
                        theResult = theReturn.ToString();

                        // In case of a dead character empty keyboard buffer in the shown way!
                        if (result < 0)
                        {
                            Win32Wrapper.ToAsciiEx(VirtualKeys.VK_SPACE, 0, keyState, ref theValue, 0, hLayout);
                        }
                    }
                }
                return theResult;
            }

            public static void Unload()
            {
                if (hLayout != 0)
                {
                    Win32Wrapper.UnloadKeyboardLayout(hLayout);
                    hLayout = 0;
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetKeyNameText(Int32 lParam, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString, Int32 nSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean GetKeyboardLayoutName([Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 LoadKeyboardLayout(String pwszKLID, Int32 flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean UnloadKeyboardLayout(Int32 hLayout);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ToAsciiEx(Int32 uVirtKey, Int32 uScanCode, Byte[] lpKeyState, ref UInt16 lpChar, Int32 flags, Int32 hLayout);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 MapVirtualKeyEx(Int32 uCode, Int32 uMapType, Int32 dwhLayout);
    }
}
