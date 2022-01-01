using System;
using System.Text;
using System.Runtime.InteropServices;

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
        // Summary:
        //
        public static string GetKeyboardLayoutName()
        {
            const int KL_NAMELENGTH = 9; // WinUser.h
            string theResult = "";

            // Create a buffer and call Win32 API to obtain the keyboard layout name.
            StringBuilder lpString = new StringBuilder(KL_NAMELENGTH);
            if ( GetKeyboardLayoutName( lpString ))
            {
                theResult = lpString.ToString();
            }
            return theResult;
        }

        // Summary:
        //
        public static string GetKeyNameText(KeyScan keyScan, string original)
        {
            string theResult = original;

            if (null != keyScan)
            {
                if (VirtualKeys.Display(keyScan.VKeyCode) != "")
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

        // Summary:
        //
        public static string GetKeyNameText(KeyScan keyScan)
        {
            string theResult = "";
            int lParam = 0;

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
                int nSize = lpString.Capacity;
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

            private const int KLF_NOTELLSHELL = 0x00000080;  // winuser.h
            private const int MAPVK_VK_TO_VSC = 0;  // winuser.h

            private static int hLayout = 0; // Handle to loaded layout.


            public static string GetKeyTextAtOnce(TKeyboardAlignment alignment, KeyScan keyScan, string original)
            {
                string result;
                Load(alignment);

                result = GetKeyText(keyScan);

                if (result == "")
                {
                    result = GetKeyNameText(keyScan, original);
                }

                Unload();

                return result;
            }

            // Summary:
            //
            public static void Load(TKeyboardAlignment alignment)
            {
                string layout = "";

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

            // Summary:
            //
            public static string GetKeyText(KeyScan keyScan)
            {
                if (hLayout == 0)
                {
                    throw new ArgumentException("The keyboard layout is not yet loaded!");
                }

                string theResult = "";
                ushort theValue  = 0;
                char   theReturn = '\0';
                byte[] keyState  = new byte[256];

                int result = ToAsciiEx(keyScan.VKeyCode, keyScan.Scancode, keyState, ref theValue, 0, hLayout);

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
                        if ( result < 0 )
                        {
                            ToAsciiEx(VirtualKeys.VK_SPACE, 0, keyState, ref theValue, 0, hLayout);
                        }
                    }
                }
                return theResult;//.ToUpper();
            }

            // Summary:
            //
            public static void Unload()
            {
                if (hLayout != 0)
                {
                    UnloadKeyboardLayout(hLayout);
                    hLayout = 0;
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetKeyNameText(int lParam, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString, int nSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetKeyboardLayoutName([Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int LoadKeyboardLayout(string pwszKLID, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnloadKeyboardLayout(int hLayout);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int ToAsciiEx(int uVirtKey, int uScanCode, byte[] lpKeyState, ref ushort lpChar, int flags, int hLayout);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int MapVirtualKeyEx(int uCode, int uMapType, int dwhLayout);
    }
}
