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

namespace ScancodeMapping
{
    public class VirtualKey
    {
        private readonly Int32 code;
        private readonly String name;
        private readonly String display;
        private readonly String fulltext;

        public VirtualKey(Int32 code, String name, String display, String fulltext)
        {
            this.code = code;
            this.name = name;
            this.display = display;
            this.fulltext = fulltext;
        }

        public Int32 Code
        {
            get { return this.code; }
        }

        public String Name
        {
            get { return this.name; }
        }

        public String Display
        {
            get { return this.display; }
        }

        public String Fulltext
        {
            get { return this.fulltext; }
        }

        public static implicit operator Int32(VirtualKey key)
        {
            return key.code;
        }

        public static implicit operator String(VirtualKey key)
        {
            return key.name;
        }

        public override String ToString()
        {
            return "VK:" + this.code.ToString("D3") + ", 0x" + this.code.ToString("X2") + ", \"" + this.name + "\", \"" + this.display + "\"";
        }
    }

    public class VirtualKeys
    {
        private static readonly VirtualKey vkUnknown = new VirtualKey(VK_UNKNOWN, "VK_UNKNOWN", "Unknown", "Unknown Virtual Key");

        #region Definition of available virtual key codes.

        public const Int32 VK_UNKNOWN = -1;
        public const Int32 VK_UNUSED = 0x00;
        public const Int32 VK_LBUTTON = 0x01;
        public const Int32 VK_RBUTTON = 0x02;
        public const Int32 VK_CANCEL = 0x03;
        public const Int32 VK_MBUTTON = 0x04;             // NOT contiguous with L & RBUTTON 
        public const Int32 VK_XBUTTON1 = 0x05;            // NOT contiguous with L & RBUTTON 
        public const Int32 VK_XBUTTON2 = 0x06;            // NOT contiguous with L & RBUTTON 
        public const Int32 VK_UNASSIGNED_0x07 = 0x07;     // 0x07 : unassigned
        public const Int32 VK_BACK = 0x08;
        public const Int32 VK_TAB = 0x09;
        public const Int32 VK_RESERVED_0x0A = 0x0A;       // 0x0A : reserved
        public const Int32 VK_RESERVED_0x0B = 0x0B;       // 0x0B : reserved
        public const Int32 VK_CLEAR = 0x0C;
        public const Int32 VK_RETURN = 0x0D;
        public const Int32 VK_RESERVED_0x0E = 0x0E;       // 0x0E : reserved (not listed in winuser.h)
        public const Int32 VK_RESERVED_0x0F = 0x0F;       // 0x0F : reserved (not listed in winuser.h)
        public const Int32 VK_SHIFT = 0x10;
        public const Int32 VK_CONTROL = 0x11;
        public const Int32 VK_MENU = 0x12;
        public const Int32 VK_PAUSE = 0x13;
        public const Int32 VK_CAPITAL = 0x14;
        public const Int32 VK_KANA = 0x15;                // other names => VK_HANGEUL, VK_HANGUL
        public const Int32 VK_RESERVED_0x16 = 0x16;       // 0x16 : reserved (not listed in winuser.h)
        public const Int32 VK_JUNJA = 0x17;
        public const Int32 VK_FINAL = 0x18;
        public const Int32 VK_KANJI = 0x19;               // other name => VK_HANJA
        public const Int32 VK_RESERVED_0x1A = 0x1A;       // 0x1A : reserved (not listed in winuser.h)
        public const Int32 VK_ESCAPE = 0x1B;
        public const Int32 VK_CONVERT = 0x1C;
        public const Int32 VK_NONCONVERT = 0x1D;
        public const Int32 VK_ACCEPT = 0x1E;
        public const Int32 VK_MODECHANGE = 0x1F;
        public const Int32 VK_SPACE = 0x20;
        public const Int32 VK_PRIOR = 0x21;
        public const Int32 VK_NEXT = 0x22;
        public const Int32 VK_END = 0x23;
        public const Int32 VK_HOME = 0x24;
        public const Int32 VK_LEFT = 0x25;
        public const Int32 VK_UP = 0x26;
        public const Int32 VK_RIGHT = 0x27;
        public const Int32 VK_DOWN = 0x28;
        public const Int32 VK_SELECT = 0x29;
        public const Int32 VK_PRINT = 0x2A;
        public const Int32 VK_EXECUTE = 0x2B;
        public const Int32 VK_SNAPSHOT = 0x2C;
        public const Int32 VK_INSERT = 0x2D;
        public const Int32 VK_DELETE = 0x2E;
        public const Int32 VK_HELP = 0x2F;
        public const Int32 VK_0 = 0x30;                   // The same as ASCII '0'
        public const Int32 VK_1 = 0x31;                   // The same as ASCII '1'
        public const Int32 VK_2 = 0x32;                   // The same as ASCII '2'
        public const Int32 VK_3 = 0x33;                   // The same as ASCII '3'
        public const Int32 VK_4 = 0x34;                   // The same as ASCII '4'
        public const Int32 VK_5 = 0x35;                   // The same as ASCII '5'
        public const Int32 VK_6 = 0x36;                   // The same as ASCII '6'
        public const Int32 VK_7 = 0x37;                   // The same as ASCII '7'
        public const Int32 VK_8 = 0x38;                   // The same as ASCII '8'
        public const Int32 VK_9 = 0x39;                   // The same as ASCII '9'
        public const Int32 VK_UNDEFINED_0x3A = 0x3A;
        public const Int32 VK_UNDEFINED_0x3B = 0x3B;
        public const Int32 VK_UNDEFINED_0x3C = 0x3C;
        public const Int32 VK_UNDEFINED_0x3D = 0x3D;
        public const Int32 VK_UNDEFINED_0x3E = 0x3E;
        public const Int32 VK_UNDEFINED_0x3F = 0x3F;
        public const Int32 VK_UNASSIGNED_0x40 = 0x40;     // 0x40 : unassigned
        public const Int32 VK_A = 0x41;                   // The same as ASCII 'A'
        public const Int32 VK_B = 0x42;                   // The same as ASCII 'B'
        public const Int32 VK_C = 0x43;                   // The same as ASCII 'C'
        public const Int32 VK_D = 0x44;                   // The same as ASCII 'D'
        public const Int32 VK_E = 0x45;                   // The same as ASCII 'E'
        public const Int32 VK_F = 0x46;                   // The same as ASCII 'F'
        public const Int32 VK_G = 0x47;                   // The same as ASCII 'G'
        public const Int32 VK_H = 0x48;                   // The same as ASCII 'H'
        public const Int32 VK_I = 0x49;                   // The same as ASCII 'I'
        public const Int32 VK_J = 0x4A;                   // The same as ASCII 'J'
        public const Int32 VK_K = 0x4B;                   // The same as ASCII 'K'
        public const Int32 VK_L = 0x4C;                   // The same as ASCII 'L'
        public const Int32 VK_M = 0x4D;                   // The same as ASCII 'M'
        public const Int32 VK_N = 0x4E;                   // The same as ASCII 'N'
        public const Int32 VK_O = 0x4F;                   // The same as ASCII 'O'
        public const Int32 VK_P = 0x50;                   // The same as ASCII 'P'
        public const Int32 VK_Q = 0x51;                   // The same as ASCII 'Q'
        public const Int32 VK_R = 0x52;                   // The same as ASCII 'R'
        public const Int32 VK_S = 0x53;                   // The same as ASCII 'S'
        public const Int32 VK_T = 0x54;                   // The same as ASCII 'T'
        public const Int32 VK_U = 0x55;                   // The same as ASCII 'U'
        public const Int32 VK_V = 0x56;                   // The same as ASCII 'V'
        public const Int32 VK_W = 0x57;                   // The same as ASCII 'W'
        public const Int32 VK_X = 0x58;                   // The same as ASCII 'X'
        public const Int32 VK_Y = 0x59;                   // The same as ASCII 'Y'
        public const Int32 VK_Z = 0x5A;                   // The same as ASCII 'Z'
        public const Int32 VK_LWIN = 0x5B;
        public const Int32 VK_RWIN = 0x5C;
        public const Int32 VK_APPS = 0x5D;
        public const Int32 VK_RESERVED_0x5E = 0x5E;       // 0x5E : reserved
        public const Int32 VK_SLEEP = 0x5F;
        public const Int32 VK_NUMPAD0 = 0x60;
        public const Int32 VK_NUMPAD1 = 0x61;
        public const Int32 VK_NUMPAD2 = 0x62;
        public const Int32 VK_NUMPAD3 = 0x63;
        public const Int32 VK_NUMPAD4 = 0x64;
        public const Int32 VK_NUMPAD5 = 0x65;
        public const Int32 VK_NUMPAD6 = 0x66;
        public const Int32 VK_NUMPAD7 = 0x67;
        public const Int32 VK_NUMPAD8 = 0x68;
        public const Int32 VK_NUMPAD9 = 0x69;
        public const Int32 VK_MULTIPLY = 0x6A;
        public const Int32 VK_ADD = 0x6B;
        public const Int32 VK_SEPARATOR = 0x6C;
        public const Int32 VK_SUBTRACT = 0x6D;
        public const Int32 VK_DECIMAL = 0x6E;
        public const Int32 VK_DIVIDE = 0x6F;
        public const Int32 VK_F1 = 0x70;
        public const Int32 VK_F2 = 0x71;
        public const Int32 VK_F3 = 0x72;
        public const Int32 VK_F4 = 0x73;
        public const Int32 VK_F5 = 0x74;
        public const Int32 VK_F6 = 0x75;
        public const Int32 VK_F7 = 0x76;
        public const Int32 VK_F8 = 0x77;
        public const Int32 VK_F9 = 0x78;
        public const Int32 VK_F10 = 0x79;
        public const Int32 VK_F11 = 0x7A;
        public const Int32 VK_F12 = 0x7B;
        public const Int32 VK_F13 = 0x7C;
        public const Int32 VK_F14 = 0x7D;
        public const Int32 VK_F15 = 0x7E;
        public const Int32 VK_F16 = 0x7F;
        public const Int32 VK_F17 = 0x80;
        public const Int32 VK_F18 = 0x81;
        public const Int32 VK_F19 = 0x82;
        public const Int32 VK_F20 = 0x83;
        public const Int32 VK_F21 = 0x84;
        public const Int32 VK_F22 = 0x85;
        public const Int32 VK_F23 = 0x86;
        public const Int32 VK_F24 = 0x87;
        public const Int32 VK_UNASSIGNED_0x88 = 0x88;     // 0x88 : unassigned
        public const Int32 VK_UNASSIGNED_0x89 = 0x89;     // 0x89 : unassigned
        public const Int32 VK_UNASSIGNED_0x8A = 0x8A;     // 0x8A : unassigned
        public const Int32 VK_UNASSIGNED_0x8B = 0x8B;     // 0x8B : unassigned
        public const Int32 VK_UNASSIGNED_0x8C = 0x8C;     // 0x8C : unassigned
        public const Int32 VK_UNASSIGNED_0x8D = 0x8D;     // 0x8D : unassigned
        public const Int32 VK_UNASSIGNED_0x8E = 0x8E;     // 0x8E : unassigned
        public const Int32 VK_UNASSIGNED_0x8F = 0x8F;     // 0x8F : unassigned
        public const Int32 VK_NUMLOCK = 0x90;
        public const Int32 VK_SCROLL = 0x91;
        public const Int32 VK_OEM_FJ_JISHO = 0x92;        // 'Dictionary' key  => VK_OEM_NEC_EQUAL "NEC PC-9800 kbd definitions"
        public const Int32 VK_OEM_FJ_MASSHOU = 0x93;      // 'Unregister word' key 
        public const Int32 VK_OEM_FJ_TOUROKU = 0x94;      // 'Register word' key 
        public const Int32 VK_OEM_FJ_LOYA = 0x95;         // 'Left OYAYUBI' key 
        public const Int32 VK_OEM_FJ_ROYA = 0x96;         // 'Right OYAYUBI' key 
        public const Int32 VK_UNASSIGNED_0x97 = 0x97;     // 0x97 : unassigned
        public const Int32 VK_UNASSIGNED_0x98 = 0x98;     // 0x98 : unassigned
        public const Int32 VK_UNASSIGNED_0x99 = 0x99;     // 0x99 : unassigned
        public const Int32 VK_UNASSIGNED_0x9A = 0x9A;     // 0x9A : unassigned
        public const Int32 VK_UNASSIGNED_0x9B = 0x9B;     // 0x9B : unassigned
        public const Int32 VK_UNASSIGNED_0x9C = 0x9C;     // 0x9C : unassigned
        public const Int32 VK_UNASSIGNED_0x9D = 0x9D;     // 0x9D : unassigned
        public const Int32 VK_UNASSIGNED_0x9E = 0x9E;     // 0x9E : unassigned
        public const Int32 VK_UNASSIGNED_0x9F = 0x9F;     // 0x9F : unassigned
        public const Int32 VK_LSHIFT = 0xA0;
        public const Int32 VK_RSHIFT = 0xA1;
        public const Int32 VK_LCONTROL = 0xA2;
        public const Int32 VK_RCONTROL = 0xA3;
        public const Int32 VK_LMENU = 0xA4;
        public const Int32 VK_RMENU = 0xA5;
        public const Int32 VK_BROWSER_BACK = 0xA6;
        public const Int32 VK_BROWSER_FORWARD = 0xA7;
        public const Int32 VK_BROWSER_REFRESH = 0xA8;
        public const Int32 VK_BROWSER_STOP = 0xA9;
        public const Int32 VK_BROWSER_SEARCH = 0xAA;
        public const Int32 VK_BROWSER_FAVORITES = 0xAB;
        public const Int32 VK_BROWSER_HOME = 0xAC;
        public const Int32 VK_VOLUME_MUTE = 0xAD;
        public const Int32 VK_VOLUME_DOWN = 0xAE;
        public const Int32 VK_VOLUME_UP = 0xAF;
        public const Int32 VK_MEDIA_NEXT_TRACK = 0xB0;
        public const Int32 VK_MEDIA_PREV_TRACK = 0xB1;
        public const Int32 VK_MEDIA_STOP = 0xB2;
        public const Int32 VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const Int32 VK_LAUNCH_MAIL = 0xB4;
        public const Int32 VK_LAUNCH_MEDIA_SELECT = 0xB5;
        public const Int32 VK_LAUNCH_APP1 = 0xB6;
        public const Int32 VK_LAUNCH_APP2 = 0xB7;
        public const Int32 VK_RESERVED_0xB8 = 0xB8;       // 0xB8 : reserved
        public const Int32 VK_RESERVED_0xB9 = 0xB9;       // 0xB9 : reserved
        public const Int32 VK_OEM_1 = 0xBA;               // ';:' for US
        public const Int32 VK_OEM_PLUS = 0xBB;            // '+' any country
        public const Int32 VK_OEM_COMMA = 0xBC;           // ',' any country
        public const Int32 VK_OEM_MINUS = 0xBD;           // '-' any country
        public const Int32 VK_OEM_PERIOD = 0xBE;          // '.' any country
        public const Int32 VK_OEM_2 = 0xBF;               // '/?' for US
        public const Int32 VK_OEM_3 = 0xC0;               // '`~' for US
        public const Int32 VK_RESERVED_0xC1 = 0xC1;       // 0xC1 : reserved
        public const Int32 VK_RESERVED_0xC2 = 0xC2;       // 0xC2 : reserved
        public const Int32 VK_RESERVED_0xC3 = 0xC3;       // 0xC3 : reserved
        public const Int32 VK_RESERVED_0xC4 = 0xC4;       // 0xC4 : reserved
        public const Int32 VK_RESERVED_0xC5 = 0xC5;       // 0xC5 : reserved
        public const Int32 VK_RESERVED_0xC6 = 0xC6;       // 0xC6 : reserved
        public const Int32 VK_RESERVED_0xC7 = 0xC7;       // 0xC7 : reserved
        public const Int32 VK_RESERVED_0xC8 = 0xC8;       // 0xC8 : reserved
        public const Int32 VK_RESERVED_0xC9 = 0xC9;       // 0xC9 : reserved
        public const Int32 VK_RESERVED_0xCA = 0xCA;       // 0xCA : reserved
        public const Int32 VK_RESERVED_0xCB = 0xCB;       // 0xCB : reserved
        public const Int32 VK_RESERVED_0xCC = 0xCC;       // 0xCC : reserved
        public const Int32 VK_RESERVED_0xCD = 0xCD;       // 0xCD : reserved
        public const Int32 VK_RESERVED_0xCE = 0xCE;       // 0xCE : reserved
        public const Int32 VK_RESERVED_0xCF = 0xCF;       // 0xCF : reserved
        public const Int32 VK_RESERVED_0xD0 = 0xD0;       // 0xD0 : reserved
        public const Int32 VK_RESERVED_0xD1 = 0xD1;       // 0xD1 : reserved
        public const Int32 VK_RESERVED_0xD2 = 0xD2;       // 0xD2 : reserved
        public const Int32 VK_RESERVED_0xD3 = 0xD3;       // 0xD3 : reserved
        public const Int32 VK_RESERVED_0xD4 = 0xD4;       // 0xD4 : reserved
        public const Int32 VK_RESERVED_0xD5 = 0xD5;       // 0xD5 : reserved
        public const Int32 VK_RESERVED_0xD6 = 0xD6;       // 0xD6 : reserved
        public const Int32 VK_RESERVED_0xD7 = 0xD7;       // 0xD7 : reserved
        public const Int32 VK_UNASSIGNED_0xD8 = 0xD8;     // 0xD8 : unassigned
        public const Int32 VK_UNASSIGNED_0xD9 = 0xD9;     // 0xD9 : unassigned
        public const Int32 VK_UNASSIGNED_0xDA = 0xDA;     // 0xDA : unassigned
        public const Int32 VK_OEM_4 = 0xDB;               //  '[{' for US
        public const Int32 VK_OEM_5 = 0xDC;               //  '\|' for US
        public const Int32 VK_OEM_6 = 0xDD;               //  ']}' for US
        public const Int32 VK_OEM_7 = 0xDE;               //  ''"' for US
        public const Int32 VK_OEM_8 = 0xDF;
        public const Int32 VK_RESERVED_0xE0 = 0xE0;       // 0xE0 : reserved
        public const Int32 VK_OEM_AX = 0xE1;              // 'AX' key on Japanese AX kbd (Various extended or enhanced keyboards)
        public const Int32 VK_OEM_102 = 0xE2;             // "<>" or "\|" on RT 102-key kbd (Various extended or enhanced keyboards).
        public const Int32 VK_ICO_HELP = 0xE3;            // Help key on ICO (Various extended or enhanced keyboards)
        public const Int32 VK_ICO_00 = 0xE4;              // 00 key on ICO (Various extended or enhanced keyboards)
        public const Int32 VK_PROCESSKEY = 0xE5;          // (Various extended or enhanced keyboards)
        public const Int32 VK_ICO_CLEAR = 0xE6;           // (Various extended or enhanced keyboards)
        public const Int32 VK_PACKET = 0xE7;              // (Various extended or enhanced keyboards)
        public const Int32 VK_UNASSIGNED_0xE8 = 0xE8;     // 0xE8 : unassigned
        public const Int32 VK_OEM_RESET = 0xE9;           // Nokia/Ericsson definitions
        public const Int32 VK_OEM_JUMP = 0xEA;            // Nokia/Ericsson definitions
        public const Int32 VK_OEM_PA1 = 0xEB;             // Nokia/Ericsson definitions
        public const Int32 VK_OEM_PA2 = 0xEC;             // Nokia/Ericsson definitions
        public const Int32 VK_OEM_PA3 = 0xED;             // Nokia/Ericsson definitions
        public const Int32 VK_OEM_WSCTRL = 0xEE;          // Nokia/Ericsson definitions
        public const Int32 VK_OEM_CUSEL = 0xEF;           // Nokia/Ericsson definitions
        public const Int32 VK_OEM_ATTN = 0xF0;            // Nokia/Ericsson definitions
        public const Int32 VK_OEM_FINISH = 0xF1;          // Nokia/Ericsson definitions
        public const Int32 VK_OEM_COPY = 0xF2;            // Nokia/Ericsson definitions
        public const Int32 VK_OEM_AUTO = 0xF3;            // Nokia/Ericsson definitions
        public const Int32 VK_OEM_ENLW = 0xF4;            // Nokia/Ericsson definitions
        public const Int32 VK_OEM_BACKTAB = 0xF5;         // Nokia/Ericsson definitions
        public const Int32 VK_ATTN = 0xF6;                // Nokia/Ericsson definitions
        public const Int32 VK_CRSEL = 0xF7;               // Nokia/Ericsson definitions
        public const Int32 VK_EXSEL = 0xF8;               // Nokia/Ericsson definitions
        public const Int32 VK_EREOF = 0xF9;               // Nokia/Ericsson definitions
        public const Int32 VK_PLAY = 0xFA;                // Nokia/Ericsson definitions
        public const Int32 VK_ZOOM = 0xFB;                // Nokia/Ericsson definitions
        public const Int32 VK_NONAME = 0xFC;              // Nokia/Ericsson definitions
        public const Int32 VK_PA1 = 0xFD;                 // Nokia/Ericsson definitions
        public const Int32 VK_OEM_CLEAR = 0xFE;           // Nokia/Ericsson definitions
        public const Int32 VK_RESERVED_0xFF = 0xFF;       // 0xFF : reserved/disabled

        #endregion

        #region Definition of the virtual key table.

        private static readonly VirtualKey[] vkTable = new VirtualKey[]
        {
            new VirtualKey( VK_UNUSED,              "VK_UNUSED",               "",          ""),
            new VirtualKey( VK_LBUTTON,             "VK_LBUTTON",              "",          ""),
            new VirtualKey( VK_RBUTTON,             "VK_RBUTTON",              "",          ""),
            new VirtualKey( VK_CANCEL,              "VK_CANCEL",               "",          ""),
            new VirtualKey( VK_MBUTTON,             "VK_MBUTTON",              "",          ""),
            new VirtualKey( VK_XBUTTON1,            "VK_XBUTTON1",             "",          ""),
            new VirtualKey( VK_XBUTTON2,            "VK_XBUTTON2",             "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x07,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_BACK,                "VK_BACK",                 "backspace", "Backspace"),
            new VirtualKey( VK_TAB,                 "VK_TAB",                  "tab",       "Tabulator"),
            new VirtualKey( VK_RESERVED_0x0A,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0x0B,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_CLEAR,               "VK_CLEAR",                "",          ""),
            new VirtualKey( VK_RETURN,              "VK_RETURN",               "ent",       "Enter"),
            new VirtualKey( VK_RESERVED_0x0E,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0x0F,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_SHIFT,               "VK_SHIFT",                "shift",     "Shift"),
            new VirtualKey( VK_CONTROL,             "VK_CONTROL",              "ctrl",      "Control"),
            new VirtualKey( VK_MENU,                "VK_MENU",                 "alt",       "Alt"),
            new VirtualKey( VK_PAUSE,               "VK_PAUSE",                "brk",       "Break"),
            new VirtualKey( VK_CAPITAL,             "VK_CAPITAL",              "caps",      "Caps Lock"),
            new VirtualKey( VK_KANA,                "VK_KANA",                 "",          ""),
            new VirtualKey( VK_RESERVED_0x16,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_JUNJA,               "VK_JUNJA",                "",          ""),
            new VirtualKey( VK_FINAL,               "VK_FINAL",                "",          ""),
            new VirtualKey( VK_KANJI,               "VK_KANJI",                "",          ""),
            new VirtualKey( VK_RESERVED_0x1A,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_ESCAPE,              "VK_ESCAPE",               "esc",       "Escape"),
            new VirtualKey( VK_CONVERT,             "VK_CONVERT",              "",          ""),
            new VirtualKey( VK_NONCONVERT,          "VK_NONCONVERT",           "",          ""),
            new VirtualKey( VK_ACCEPT,              "VK_ACCEPT",               "",          ""),
            new VirtualKey( VK_MODECHANGE,          "VK_MODECHANGE",           "",          ""),
            new VirtualKey( VK_SPACE,               "VK_SPACE",                "space",     "Space"),
            new VirtualKey( VK_PRIOR,               "VK_PRIOR",                "pup",       "Page Up"),
            new VirtualKey( VK_NEXT,                "VK_NEXT",                 "pdn",       "Page Down"),
            new VirtualKey( VK_END,                 "VK_END",                  "end",       "End"),
            new VirtualKey( VK_HOME,                "VK_HOME",                 "hm",        "Home"),
            new VirtualKey( VK_LEFT,                "VK_LEFT",                 "lft",       "Left Arrow"),
            new VirtualKey( VK_UP,                  "VK_UP",                   "up",        "Up Arrow"),
            new VirtualKey( VK_RIGHT,               "VK_RIGHT",                "rgh",       "Right Arrow"),
            new VirtualKey( VK_DOWN,                "VK_DOWN",                 "dn",        "Down Arrow"),
            new VirtualKey( VK_SELECT,              "VK_SELECT",               "",          ""),
            new VirtualKey( VK_PRINT,               "VK_PRINT",                "",          ""),
            new VirtualKey( VK_EXECUTE,             "VK_EXECUTE",              "",          ""),
            new VirtualKey( VK_SNAPSHOT,            "VK_SNAPSHOT",             "prn",       "Print Screen"),
            new VirtualKey( VK_INSERT,              "VK_INSERT",               "ins",       "Insert"),
            new VirtualKey( VK_DELETE,              "VK_DELETE",               "del",       "Delete"),
            new VirtualKey( VK_HELP,                "VK_HELP",                 "",          ""),
            new VirtualKey( VK_0,                   "VK_0",                    "",          ""),
            new VirtualKey( VK_1,                   "VK_1",                    "",          ""),
            new VirtualKey( VK_2,                   "VK_2",                    "",          ""),
            new VirtualKey( VK_3,                   "VK_3",                    "",          ""),
            new VirtualKey( VK_4,                   "VK_4",                    "",          ""),
            new VirtualKey( VK_5,                   "VK_5",                    "",          ""),
            new VirtualKey( VK_6,                   "VK_6",                    "",          ""),
            new VirtualKey( VK_7,                   "VK_7",                    "",          ""),
            new VirtualKey( VK_8,                   "VK_8",                    "",          ""),
            new VirtualKey( VK_9,                   "VK_9",                    "",          ""),
            new VirtualKey( VK_UNDEFINED_0x3A,      "VK_UNDEFINED",            "",          ""),
            new VirtualKey( VK_UNDEFINED_0x3B,      "VK_UNDEFINED",            "",          ""),
            new VirtualKey( VK_UNDEFINED_0x3C,      "VK_UNDEFINED",            "",          ""),
            new VirtualKey( VK_UNDEFINED_0x3D,      "VK_UNDEFINED",            "",          ""),
            new VirtualKey( VK_UNDEFINED_0x3E,      "VK_UNDEFINED",            "",          ""),
            new VirtualKey( VK_UNDEFINED_0x3F,      "VK_UNDEFINED",            "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x40,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_A,                   "VK_A",                    "",          ""),
            new VirtualKey( VK_B,                   "VK_B",                    "",          ""),
            new VirtualKey( VK_C,                   "VK_C",                    "",          ""),
            new VirtualKey( VK_D,                   "VK_D",                    "",          ""),
            new VirtualKey( VK_E,                   "VK_E",                    "",          ""),
            new VirtualKey( VK_F,                   "VK_F",                    "",          ""),
            new VirtualKey( VK_G,                   "VK_G",                    "",          ""),
            new VirtualKey( VK_H,                   "VK_H",                    "",          ""),
            new VirtualKey( VK_I,                   "VK_I",                    "",          ""),
            new VirtualKey( VK_J,                   "VK_J",                    "",          ""),
            new VirtualKey( VK_K,                   "VK_K",                    "",          ""),
            new VirtualKey( VK_L,                   "VK_L",                    "",          ""),
            new VirtualKey( VK_M,                   "VK_M",                    "",          ""),
            new VirtualKey( VK_N,                   "VK_N",                    "",          ""),
            new VirtualKey( VK_O,                   "VK_O",                    "",          ""),
            new VirtualKey( VK_P,                   "VK_P",                    "",          ""),
            new VirtualKey( VK_Q,                   "VK_Q",                    "",          ""),
            new VirtualKey( VK_R,                   "VK_R",                    "",          ""),
            new VirtualKey( VK_S,                   "VK_S",                    "",          ""),
            new VirtualKey( VK_T,                   "VK_T",                    "",          ""),
            new VirtualKey( VK_U,                   "VK_U",                    "",          ""),
            new VirtualKey( VK_V,                   "VK_V",                    "",          ""),
            new VirtualKey( VK_W,                   "VK_W",                    "",          ""),
            new VirtualKey( VK_X,                   "VK_X",                    "",          ""),
            new VirtualKey( VK_Y,                   "VK_Y",                    "",          ""),
            new VirtualKey( VK_Z,                   "VK_Z",                    "",          ""),
            new VirtualKey( VK_LWIN,                "VK_LWIN",                 "win",       "Left Windows Key"),
            new VirtualKey( VK_RWIN,                "VK_RWIN",                 "win",       "Right Windows Key"),
            new VirtualKey( VK_APPS,                "VK_APPS",                 "apps",      "Context Menu"),
            new VirtualKey( VK_RESERVED_0x5E,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_SLEEP,               "VK_SLEEP",                "",          ""),
            new VirtualKey( VK_NUMPAD0,             "VK_NUMPAD0",              "n0",        "NUM 0"),
            new VirtualKey( VK_NUMPAD1,             "VK_NUMPAD1",              "n1",        "NUM 1"),
            new VirtualKey( VK_NUMPAD2,             "VK_NUMPAD2",              "n2",        "NUM 2"),
            new VirtualKey( VK_NUMPAD3,             "VK_NUMPAD3",              "n3",        "NUM 3"),
            new VirtualKey( VK_NUMPAD4,             "VK_NUMPAD4",              "n4",        "NUM 4"),
            new VirtualKey( VK_NUMPAD5,             "VK_NUMPAD5",              "n5",        "NUM 5"),
            new VirtualKey( VK_NUMPAD6,             "VK_NUMPAD6",              "n6",        "NUM 6"),
            new VirtualKey( VK_NUMPAD7,             "VK_NUMPAD7",              "n7",        "NUM 7"),
            new VirtualKey( VK_NUMPAD8,             "VK_NUMPAD8",              "n8",        "NUM 8"),
            new VirtualKey( VK_NUMPAD9,             "VK_NUMPAD9",              "n9",        "NUM 9"),
            new VirtualKey( VK_MULTIPLY,            "VK_MULTIPLY",             "mul",       "Multiply"),
            new VirtualKey( VK_ADD,                 "VK_ADD",                  "add",       "Add"),
            new VirtualKey( VK_SEPARATOR,           "VK_SEPARATOR",            "",          ""),
            new VirtualKey( VK_SUBTRACT,            "VK_SUBTRACT",             "sub",       "Subtract"),
            new VirtualKey( VK_DECIMAL,             "VK_DECIMAL",              "dec",       "Decimal"),
            new VirtualKey( VK_DIVIDE,              "VK_DIVIDE",               "div",       "Divide"),
            new VirtualKey( VK_F1,                  "VK_F1",                   "",          ""),
            new VirtualKey( VK_F2,                  "VK_F2",                   "",          ""),
            new VirtualKey( VK_F3,                  "VK_F3",                   "",          ""),
            new VirtualKey( VK_F4,                  "VK_F4",                   "",          ""),
            new VirtualKey( VK_F5,                  "VK_F5",                   "",          ""),
            new VirtualKey( VK_F6,                  "VK_F6",                   "",          ""),
            new VirtualKey( VK_F7,                  "VK_F7",                   "",          ""),
            new VirtualKey( VK_F8,                  "VK_F8",                   "",          ""),
            new VirtualKey( VK_F9,                  "VK_F9",                   "",          ""),
            new VirtualKey( VK_F10,                 "VK_F10",                  "",          ""),
            new VirtualKey( VK_F11,                 "VK_F11",                  "",          ""),
            new VirtualKey( VK_F12,                 "VK_F12",                  "",          ""),
            new VirtualKey( VK_F13,                 "VK_F13",                  "",          ""),
            new VirtualKey( VK_F14,                 "VK_F14",                  "",          ""),
            new VirtualKey( VK_F15,                 "VK_F15",                  "",          ""),
            new VirtualKey( VK_F16,                 "VK_F16",                  "",          ""),
            new VirtualKey( VK_F17,                 "VK_F17",                  "",          ""),
            new VirtualKey( VK_F18,                 "VK_F18",                  "",          ""),
            new VirtualKey( VK_F19,                 "VK_F19",                  "",          ""),
            new VirtualKey( VK_F20,                 "VK_F20",                  "",          ""),
            new VirtualKey( VK_F21,                 "VK_F21",                  "",          ""),
            new VirtualKey( VK_F22,                 "VK_F22",                  "",          ""),
            new VirtualKey( VK_F23,                 "VK_F23",                  "",          ""),
            new VirtualKey( VK_F24,                 "VK_F24",                  "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x88,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x89,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x8A,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x8B,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x8C,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x8D,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x8E,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x8F,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_NUMLOCK,             "VK_NUMLOCK",              "num",       "NUM Lock"),
            new VirtualKey( VK_SCROLL,              "VK_SCROLL",               "scr",       "Scroll Lock"),
            new VirtualKey( VK_OEM_FJ_JISHO,        "VK_OEM_FJ_JISHO",         "",          ""),
            new VirtualKey( VK_OEM_FJ_MASSHOU,      "VK_OEM_FJ_MASSHOU",       "",          ""),
            new VirtualKey( VK_OEM_FJ_TOUROKU,      "VK_OEM_FJ_TOUROKU",       "",          ""),
            new VirtualKey( VK_OEM_FJ_LOYA,         "VK_OEM_FJ_LOYA",          "",          ""),
            new VirtualKey( VK_OEM_FJ_ROYA,         "VK_OEM_FJ_ROYA",          "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x97,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x98,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x99,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x9A,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x9B,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x9C,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x9D,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x9E,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0x9F,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_LSHIFT,              "VK_LSHIFT",               "shift",     "Left Shift"),
            new VirtualKey( VK_RSHIFT,              "VK_RSHIFT",               "shift",     "Right Shift"),
            new VirtualKey( VK_LCONTROL,            "VK_LCONTROL",             "ctrl",      "Left Control"),
            new VirtualKey( VK_RCONTROL,            "VK_RCONTROL",             "ctrl",      "Right Control"),
            new VirtualKey( VK_LMENU,               "VK_LMENU",                "alt",       "Left Alt"),
            new VirtualKey( VK_RMENU,               "VK_RMENU",                "alt",       "Right Alt"),
            new VirtualKey( VK_BROWSER_BACK,        "VK_BROWSER_BACK",         "",          ""),
            new VirtualKey( VK_BROWSER_FORWARD,     "VK_BROWSER_FORWARD",      "",          ""),
            new VirtualKey( VK_BROWSER_REFRESH,     "VK_BROWSER_REFRESH",      "",          ""),
            new VirtualKey( VK_BROWSER_STOP,        "VK_BROWSER_STOP",         "",          ""),
            new VirtualKey( VK_BROWSER_SEARCH,      "VK_BROWSER_SEARCH",       "",          ""),
            new VirtualKey( VK_BROWSER_FAVORITES,   "VK_BROWSER_FAVORITES",    "",          ""),
            new VirtualKey( VK_BROWSER_HOME,        "VK_BROWSER_HOME",         "",          ""),
            new VirtualKey( VK_VOLUME_MUTE,         "VK_VOLUME_MUTE",          "",          ""),
            new VirtualKey( VK_VOLUME_DOWN,         "VK_VOLUME_DOWN",          "",          ""),
            new VirtualKey( VK_VOLUME_UP,           "VK_VOLUME_UP",            "",          ""),
            new VirtualKey( VK_MEDIA_NEXT_TRACK,    "VK_MEDIA_NEXT_TRACK",     "",          ""),
            new VirtualKey( VK_MEDIA_PREV_TRACK,    "VK_MEDIA_PREV_TRACK",     "",          ""),
            new VirtualKey( VK_MEDIA_STOP,          "VK_MEDIA_STOP",           "",          ""),
            new VirtualKey( VK_MEDIA_PLAY_PAUSE,    "VK_MEDIA_PLAY_PAUSE",     "",          ""),
            new VirtualKey( VK_LAUNCH_MAIL,         "VK_LAUNCH_MAIL",          "",          ""),
            new VirtualKey( VK_LAUNCH_MEDIA_SELECT, "VK_LAUNCH_MEDIA_SELECT",  "",          ""),
            new VirtualKey( VK_LAUNCH_APP1,         "VK_LAUNCH_APP1",          "",          ""),
            new VirtualKey( VK_LAUNCH_APP2,         "VK_LAUNCH_APP2",          "",          ""),
            new VirtualKey( VK_RESERVED_0xB8,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xB9,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_OEM_1,               "VK_OEM_1",                "",          ""),
            new VirtualKey( VK_OEM_PLUS,            "VK_OEM_PLUS",             "",          ""),
            new VirtualKey( VK_OEM_COMMA,           "VK_OEM_COMMA",            "",          ""),
            new VirtualKey( VK_OEM_MINUS,           "VK_OEM_MINUS",            "",          ""),
            new VirtualKey( VK_OEM_PERIOD,          "VK_OEM_PERIOD",           "",          ""),
            new VirtualKey( VK_OEM_2,               "VK_OEM_2",                "",          ""),
            new VirtualKey( VK_OEM_3,               "VK_OEM_3",                "",          ""),
            new VirtualKey( VK_RESERVED_0xC1,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xC2,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xC3,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xC4,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xC5,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xC6,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xC7,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xC8,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xC9,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xCA,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xCB,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xCC,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xCD,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xCE,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xCF,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xD0,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xD1,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xD2,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xD3,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xD4,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xD5,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xD6,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_RESERVED_0xD7,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_UNASSIGNED_0xD8,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0xD9,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_UNASSIGNED_0xDA,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_OEM_4,               "VK_OEM_4",                "",          ""),
            new VirtualKey( VK_OEM_5,               "VK_OEM_5",                "",          ""),
            new VirtualKey( VK_OEM_6,               "VK_OEM_6",                "",          ""),
            new VirtualKey( VK_OEM_7,               "VK_OEM_7",                "",          ""),
            new VirtualKey( VK_OEM_8,               "VK_OEM_8",                "",          ""),
            new VirtualKey( VK_RESERVED_0xE0,       "VK_RESERVED",             "",          ""),
            new VirtualKey( VK_OEM_AX,              "VK_OEM_AX",               "",          ""),
            new VirtualKey( VK_OEM_102,             "VK_OEM_102",              "",          ""),
            new VirtualKey( VK_ICO_HELP,            "VK_ICO_HELP",             "",          ""),
            new VirtualKey( VK_ICO_00,              "VK_ICO_00",               "",          ""),
            new VirtualKey( VK_PROCESSKEY,          "VK_PROCESSKEY",           "",          ""),
            new VirtualKey( VK_ICO_CLEAR,           "VK_ICO_CLEAR",            "",          ""),
            new VirtualKey( VK_PACKET,              "VK_PACKET",               "",          ""),
            new VirtualKey( VK_UNASSIGNED_0xE8,     "VK_UNASSIGNED",           "",          ""),
            new VirtualKey( VK_OEM_RESET,           "VK_OEM_RESET",            "",          ""),
            new VirtualKey( VK_OEM_JUMP,            "VK_OEM_JUMP",             "",          ""),
            new VirtualKey( VK_OEM_PA1,             "VK_OEM_PA1",              "",          ""),
            new VirtualKey( VK_OEM_PA2,             "VK_OEM_PA2",              "",          ""),
            new VirtualKey( VK_OEM_PA3,             "VK_OEM_PA3",              "",          ""),
            new VirtualKey( VK_OEM_WSCTRL,          "VK_OEM_WSCTRL",           "",          ""),
            new VirtualKey( VK_OEM_CUSEL,           "VK_OEM_CUSEL",            "",          ""),
            new VirtualKey( VK_OEM_ATTN,            "VK_OEM_ATTN",             "",          ""),
            new VirtualKey( VK_OEM_FINISH,          "VK_OEM_FINISH",           "",          ""),
            new VirtualKey( VK_OEM_COPY,            "VK_OEM_COPY",             "",          ""),
            new VirtualKey( VK_OEM_AUTO,            "VK_OEM_AUTO",             "",          ""),
            new VirtualKey( VK_OEM_ENLW,            "VK_OEM_ENLW",             "",          ""),
            new VirtualKey( VK_OEM_BACKTAB,         "VK_OEM_BACKTAB",          "",          ""),
            new VirtualKey( VK_ATTN,                "VK_ATTN",                 "",          ""),
            new VirtualKey( VK_CRSEL,               "VK_CRSEL",                "",          ""),
            new VirtualKey( VK_EXSEL,               "VK_EXSEL",                "",          ""),
            new VirtualKey( VK_EREOF,               "VK_EREOF",                "",          ""),
            new VirtualKey( VK_PLAY,                "VK_PLAY",                 "",          ""),
            new VirtualKey( VK_ZOOM,                "VK_ZOOM",                 "",          ""),
            new VirtualKey( VK_NONAME,              "VK_NONAME",               "",          ""),
            new VirtualKey( VK_PA1,                 "VK_PA1",                  "",          ""),
            new VirtualKey( VK_OEM_CLEAR,           "VK_OEM_CLEAR",            "",          ""),
            new VirtualKey( VK_RESERVED_0xFF,       "VK_RESERVED",             "",          "")
        };

        #endregion

        public static VirtualKey Entry(Int32 code)
        {
            if (0 <= code && code <= VK_RESERVED_0xFF)
            {
                return vkTable[code];
            }
            else if (code == VK_UNKNOWN)
            {
                return vkUnknown;
            }
            else
            {
                throw new IndexOutOfRangeException("Index " + code + " is out of table bounds.");
            }
        }

        public static String Name(Int32 code)
        {
            return Entry(code).Name;
        }

        public static String Display(Int32 code)
        {
            return Entry(code).Display;
        }

        public static String Fulltext(Int32 code)
        {
            return Entry(code).Fulltext;
        }

        public static Int32 Code(String name)
        {
            for (Int32 idx = 0; idx < vkTable.Length; idx++)
            {
                if (vkTable[idx].Name == name)
                {
                    return vkTable[idx].Code;
                }
            }
            return -1;
        }

        public static Int32 Count
        {
            get { return vkTable.Length; }
        }
    }
}
