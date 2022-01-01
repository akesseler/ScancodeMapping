using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
        SystemMenu result = new SystemMenu();

        result.hSysMenu = GetSystemMenu(form.Handle, false);

        if (result.hSysMenu == IntPtr.Zero)
        {
            throw new MemberAccessException("Call to WIN32API function \"GetSystemMenu()\" returned with an invalid (zero) handle.");
        }

        return result;
    }

    // Returns a menu item's position for given menu item command ID.
    public int FindPositionByCommandID(int commandID)
    {
        int count = GetMenuItemCount(hSysMenu);
        for (int index = 0; index < count; index++)
        {
            if (commandID == GetMenuItemID(hSysMenu, index))
            {
                return index;
            }
        }
        throw new IndexOutOfRangeException("Menu with command ID 0x" + commandID.ToString("X8") + " not found.");
    }

    // Insert a separator at the given position; index starts at zero.
    public bool InsertSeparator(int position)
    {
        return InsertMenu(position, MenuFlags.MF_SEPARATOR | MenuFlags.MF_BYPOSITION, 0, "");
    }

    // Insert a menu at the given position; index starts at zero.
    public bool InsertMenu(int position, int itemID, String itemName)
    {
        return InsertMenu(position, MenuFlags.MF_BYPOSITION | MenuFlags.MF_STRING, itemID, itemName);
    }

    // Insert a menu at the given position using flags; index starts at zero.
    public bool InsertMenu(int position, MenuFlags flags, int itemID, String itemName)
    {
        return InsertMenu(hSysMenu, position, (int)flags, itemID, itemName);
    }

    // Appends a seperator
    public bool AppendSeparator()
    {
        return this.AppendMenu(0, "", MenuFlags.MF_SEPARATOR);
    }

    // Appent a menu (MF_STRING only) with given item ID.
    public bool AppendMenu(int itemID, String itemName)
    {
        return this.AppendMenu(itemID, itemName, MenuFlags.MF_STRING);
    }

    // Appent a menu (MF_STRING only) with given item ID using flags.
    public bool AppendMenu(int itemID, String itemName, MenuFlags flags)
    {
        return AppendMenu(hSysMenu, (int)flags, itemID, itemName);
    }

    // Reset's the window menu to it's default
    public static void ResetSystemMenu(Form form)
    {
        GetSystemMenu(form.Handle, true);
    }

    //
    // DLL import section.
    //

    [DllImport("user32.dll", EntryPoint = "GetSystemMenu", SetLastError = true, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll", EntryPoint = "AppendMenuW", SetLastError = true, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern bool AppendMenu(IntPtr hMenu, int flags, int itemID, String itemName);

    [DllImport("user32.dll", EntryPoint = "InsertMenuW", SetLastError = true, CharSet = CharSet.Unicode,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern bool InsertMenu(IntPtr hMenu, int position, int flags, int itemID, String itemName);

    [DllImport("user32.dll", EntryPoint = "GetMenuItemCount", SetLastError = true,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern int GetMenuItemCount(IntPtr hMenu);

    [DllImport("user32.dll", EntryPoint = "GetMenuItemID", SetLastError = true,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern int GetMenuItemID(IntPtr hMenu, int pos);
}
