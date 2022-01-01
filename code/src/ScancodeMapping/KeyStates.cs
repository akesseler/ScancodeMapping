﻿/*
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

using System.Runtime.InteropServices;   // DllImport()

namespace ScancodeMapping
{
    public static class KeyStates
    {
        private const int VK_NUMLOCK = 0x90;  // VK=0x0090; SC=0x0045; FL=0x0001; NA=VK_NUMLOCK
        private const int VK_CAPITAL = 0x14;  // VK=0x0014; SC=0x003A; FL=0x0000; NA=VK_CAPITAL
        private const int VK_SCROLL = 0x91;  // VK=0x0091; SC=0x0046; FL=0x0000; NA=VK_SCROLL

        private const int SC_NUMLOCK = 0x45;  // VK=0x0090; SC=0x0045; FL=0x0001; NA=VK_NUMLOCK
        private const int SC_CAPITAL = 0x3A;  // VK=0x0014; SC=0x003A; FL=0x0000; NA=VK_CAPITAL
        private const int SC_SCROLL = 0x46;  // VK=0x0091; SC=0x0046; FL=0x0000; NA=VK_SCROLL

        public static bool NumLock { get { return GetKeyboardState(VK_NUMLOCK); } }
        public static bool CapsLock { get { return GetKeyboardState(VK_CAPITAL); } }
        public static bool ScrollLock { get { return GetKeyboardState(VK_SCROLL); } }

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        //
        // Summary:
        //
        private static bool GetKeyboardState(int vkCode)
        {
            // For those keys the low-order bit is set!
            if ((GetKeyState(vkCode) & 0x00000001) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
