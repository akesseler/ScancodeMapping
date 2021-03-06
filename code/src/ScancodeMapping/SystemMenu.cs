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
using System.Windows.Forms;

public enum WindowMessage // Taken from file "winuser.h"
{
    WM_SYSCOMMAND = 0x0112
}

public enum MenuFlags // Taken from file "winuser.h"
{
    MF_INSERT = 0x00000000,
    MF_CHANGE = 0x00000080,
    MF_APPEND = 0x00000100,
    MF_DELETE = 0x00000200,
    MF_REMOVE = 0x00001000,
    MF_BYCOMMAND = 0x00000000,
    MF_BYPOSITION = 0x00000400,
    MF_SEPARATOR = 0x00000800,
    MF_ENABLED = 0x00000000,
    MF_GRAYED = 0x00000001,
    MF_DISABLED = 0x00000002,
    MF_UNCHECKED = 0x00000000,
    MF_CHECKED = 0x00000008,
    MF_USECHECKBITMAPS = 0x00000200,
    MF_STRING = 0x00000000,
    MF_BITMAP = 0x00000004,
    MF_OWNERDRAW = 0x00000100,
    MF_POPUP = 0x00000010,
    MF_MENUBARBREAK = 0x00000020,
    MF_MENUBREAK = 0x00000040,
    MF_UNHILITE = 0x00000000,
    MF_HILITE = 0x00000080,
    MF_DEFAULT = 0x00001000,
    MF_SYSMENU = 0x00002000,
    MF_HELP = 0x00004000,
    MF_RIGHTJUSTIFY = 0x00004000,
    MF_MOUSESELECT = 0x00008000
}

public enum SystemMenuCommand // Taken from file "winuser.h"
{
    SC_SIZE = 0xF000,
    SC_MOVE = 0xF010,
    SC_MINIMIZE = 0xF020,
    SC_MAXIMIZE = 0xF030,
    SC_NEXTWINDOW = 0xF040,
    SC_PREVWINDOW = 0xF050,
    SC_CLOSE = 0xF060,
    SC_VSCROLL = 0xF070,
    SC_HSCROLL = 0xF080,
    SC_MOUSEMENU = 0xF090,
    SC_KEYMENU = 0xF100,
    SC_ARRANGE = 0xF110,
    SC_RESTORE = 0xF120,
    SC_TASKLIST = 0xF130,
    SC_SCREENSAVE = 0xF140,
    SC_HOTKEY = 0xF150,
    SC_DEFAULT = 0xF160,
    SC_MONITORPOWER = 0xF170,
    SC_CONTEXTHELP = 0xF180,
    SC_SEPARATOR = 0xF00F
}

public class SystemMenu
{
    private IntPtr hSysMenu = IntPtr.Zero; // Handle to the System Menu

    // Avoid "direct new"
    private SystemMenu() { }

    // Retrieves a new object from a Form object
    public static SystemMenu GetSystemMenu(Form form)
    {
        SystemMenu result = new SystemMenu()
        {
            hSysMenu = SystemMenu.GetSystemMenu(form.Handle, false)
        };

        if (result.hSysMenu == IntPtr.Zero)
        {
            throw new MemberAccessException("Call to WIN32API function \"GetSystemMenu()\" returned with an invalid (zero) handle.");
        }

        return result;
    }

    // Returns a menu item's position for given menu item command ID.
    public Int32 FindPositionByCommandID(Int32 commandId)
    {
        Int32 count = SystemMenu.GetMenuItemCount(this.hSysMenu);
        for (Int32 index = 0; index < count; index++)
        {
            if (commandId == SystemMenu.GetMenuItemID(this.hSysMenu, index))
            {
                return index;
            }
        }
        throw new IndexOutOfRangeException("Menu with command ID 0x" + commandId.ToString("X8") + " not found.");
    }

    // Insert a separator at the given position; index starts at zero.
    public Boolean InsertSeparator(Int32 position)
    {
        return this.InsertMenu(position, MenuFlags.MF_SEPARATOR | MenuFlags.MF_BYPOSITION, 0, "");
    }

    // Insert a menu at the given position; index starts at zero.
    public Boolean InsertMenu(Int32 position, Int32 itemID, String itemName)
    {
        return this.InsertMenu(position, MenuFlags.MF_BYPOSITION | MenuFlags.MF_STRING, itemID, itemName);
    }

    // Insert a menu at the given position using flags; index starts at zero.
    public Boolean InsertMenu(Int32 position, MenuFlags flags, Int32 itemID, String itemName)
    {
        return SystemMenu.InsertMenu(this.hSysMenu, position, (Int32)flags, itemID, itemName);
    }

    // Appends a seperator
    public Boolean AppendSeparator()
    {
        return this.AppendMenu(0, "", MenuFlags.MF_SEPARATOR);
    }

    // Appent a menu (MF_STRING only) with given item ID.
    public Boolean AppendMenu(Int32 itemID, String itemName)
    {
        return this.AppendMenu(itemID, itemName, MenuFlags.MF_STRING);
    }

    // Appent a menu (MF_STRING only) with given item ID using flags.
    public Boolean AppendMenu(Int32 itemID, String itemName, MenuFlags flags)
    {
        return SystemMenu.AppendMenu(this.hSysMenu, (Int32)flags, itemID, itemName);
    }

    // Reset's the window menu to it's default
    public static void ResetSystemMenu(Form form)
    {
        SystemMenu.GetSystemMenu(form.Handle, true);
    }

    [DllImport("user32.dll", EntryPoint = "GetSystemMenu", SetLastError = true, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, Boolean bRevert);

    [DllImport("user32.dll", EntryPoint = "AppendMenuW", SetLastError = true, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern Boolean AppendMenu(IntPtr hMenu, Int32 flags, Int32 itemID, String itemName);

    [DllImport("user32.dll", EntryPoint = "InsertMenuW", SetLastError = true, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern Boolean InsertMenu(IntPtr hMenu, Int32 position, Int32 flags, Int32 itemID, String itemName);

    [DllImport("user32.dll", EntryPoint = "GetMenuItemCount", SetLastError = true,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern Int32 GetMenuItemCount(IntPtr hMenu);

    [DllImport("user32.dll", EntryPoint = "GetMenuItemID", SetLastError = true,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern Int32 GetMenuItemID(IntPtr hMenu, Int32 pos);
}
