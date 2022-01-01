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

using ScancodeHook.LowLevel;
using System.Drawing;

namespace ScancodeMapping
{
    public class StandardKeyPanel : KeyPanel
    {
        private const int LEFT = 0;
        private const int TOP = 60;
        private const int WIDTH = 629;
        private const int HEIGHT = 210;

        private enum TLayout
        {
            DE = 0,
            US = 1,
        }

        #region Buttons' location, size and VK assignments.
        private KeyButtonData[] buttonData = new KeyButtonData[]{
            new KeyButtonData(0,   0,   42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_5,      0x29, Keyboard.STANDARD)),
            new KeyButtonData(42,  0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_1,          0x02, Keyboard.STANDARD)),
            new KeyButtonData(84,  0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_2,          0x03, Keyboard.STANDARD)),
            new KeyButtonData(126, 0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_3,          0x04, Keyboard.STANDARD)),
            new KeyButtonData(168, 0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_4,          0x05, Keyboard.STANDARD)),
            new KeyButtonData(210, 0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_5,          0x06, Keyboard.STANDARD)),
            new KeyButtonData(252, 0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_6,          0x07, Keyboard.STANDARD)),
            new KeyButtonData(294, 0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_7,          0x08, Keyboard.STANDARD)),
            new KeyButtonData(336, 0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_8,          0x09, Keyboard.STANDARD)),
            new KeyButtonData(378, 0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_9,          0x0A, Keyboard.STANDARD)),
            new KeyButtonData(420, 0,   42,  42, true,  false, new KeyScan(VirtualKeys.VK_0,          0x0B, Keyboard.STANDARD)),
            new KeyButtonData(462, 0,   42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_4,      0x0C, Keyboard.STANDARD)),
            new KeyButtonData(504, 0,   42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_6,      0x0D, Keyboard.STANDARD)),
            new KeyButtonData(546, 0,   83,  42, true,  false, new KeyScan(VirtualKeys.VK_BACK,       0x0E, Keyboard.STANDARD)),
            new KeyButtonData(0,   42,  62,  42, true,  false, new KeyScan(VirtualKeys.VK_TAB,        0x0F, Keyboard.STANDARD)),
            new KeyButtonData(62,  42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_Q,          0x10, Keyboard.STANDARD)),
            new KeyButtonData(104, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_W,          0x11, Keyboard.STANDARD)),
            new KeyButtonData(146, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_E,          0x12, Keyboard.STANDARD)),
            new KeyButtonData(188, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_R,          0x13, Keyboard.STANDARD)),
            new KeyButtonData(230, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_T,          0x14, Keyboard.STANDARD)),
            new KeyButtonData(272, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_Z,          0x15, Keyboard.STANDARD)),
            new KeyButtonData(314, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_U,          0x16, Keyboard.STANDARD)),
            new KeyButtonData(356, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_I,          0x17, Keyboard.STANDARD)),
            new KeyButtonData(398, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_O,          0x18, Keyboard.STANDARD)),
            new KeyButtonData(440, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_P,          0x19, Keyboard.STANDARD)),
            new KeyButtonData(482, 42,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_1,      0x1A, Keyboard.STANDARD)),
            new KeyButtonData(524, 42,  53,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_PLUS,   0x1B, Keyboard.STANDARD)),
            new KeyButtonData(577, 42,  52,  84, true,  false, new KeyScan(VirtualKeys.VK_RETURN,     0x1C, Keyboard.STANDARD)),
            new KeyButtonData(0,   84,  73,  42, true,  false, new KeyScan(VirtualKeys.VK_CAPITAL,    0x3A, Keyboard.STANDARD)),
            new KeyButtonData(73,  84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_A,          0x1E, Keyboard.STANDARD)),
            new KeyButtonData(115, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_S,          0x1F, Keyboard.STANDARD)),
            new KeyButtonData(157, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_D,          0x20, Keyboard.STANDARD)),
            new KeyButtonData(199, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_F,          0x21, Keyboard.STANDARD)),
            new KeyButtonData(241, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_G,          0x22, Keyboard.STANDARD)),
            new KeyButtonData(283, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_H,          0x23, Keyboard.STANDARD)),
            new KeyButtonData(325, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_J,          0x24, Keyboard.STANDARD)),
            new KeyButtonData(367, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_K,          0x25, Keyboard.STANDARD)),
            new KeyButtonData(409, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_L,          0x26, Keyboard.STANDARD)),
            new KeyButtonData(451, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_3,      0x27, Keyboard.STANDARD)),
            new KeyButtonData(493, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_7,      0x28, Keyboard.STANDARD)),
            new KeyButtonData(535, 84,  42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_2,      0x2B, Keyboard.STANDARD)),
            new KeyButtonData(0,   126, 62,  42, true,  false, new KeyScan(VirtualKeys.VK_LSHIFT,     0x2A, Keyboard.STANDARD)),
            new KeyButtonData(62,  126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_102,    0x56, Keyboard.STANDARD)),
            new KeyButtonData(104, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_Y,          0x2C, Keyboard.STANDARD)),
            new KeyButtonData(146, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_X,          0x2D, Keyboard.STANDARD)),
            new KeyButtonData(188, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_C,          0x2E, Keyboard.STANDARD)),
            new KeyButtonData(230, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_V,          0x2F, Keyboard.STANDARD)),
            new KeyButtonData(272, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_B,          0x30, Keyboard.STANDARD)),
            new KeyButtonData(314, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_N,          0x31, Keyboard.STANDARD)),
            new KeyButtonData(356, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_M,          0x32, Keyboard.STANDARD)),
            new KeyButtonData(398, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_COMMA,  0x33, Keyboard.STANDARD)),
            new KeyButtonData(440, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_PERIOD, 0x34, Keyboard.STANDARD)),
            new KeyButtonData(482, 126, 42,  42, true,  true,  new KeyScan(VirtualKeys.VK_OEM_MINUS,  0x35, Keyboard.STANDARD)),
            new KeyButtonData(524, 126, 105, 42, true,  false, new KeyScan(VirtualKeys.VK_RSHIFT,     0x36, Keyboard.EXTENDED)),
            new KeyButtonData(0,   168, 73,  42, true,  false, new KeyScan(VirtualKeys.VK_LCONTROL,   0x1D, Keyboard.STANDARD)),
            new KeyButtonData(73,  168, 62,  42, true,  false, new KeyScan(VirtualKeys.VK_LWIN,       0x5B, Keyboard.EXTENDED)),
            new KeyButtonData(135, 168, 62,  42, true,  false, new KeyScan(VirtualKeys.VK_LMENU,      0x38, Keyboard.STANDARD)),
            new KeyButtonData(197, 168, 218, 42, true,  false, new KeyScan(VirtualKeys.VK_SPACE,      0x39, Keyboard.STANDARD)),
            new KeyButtonData(415, 168, 62,  42, true,  false, new KeyScan(VirtualKeys.VK_RMENU,      0x38, Keyboard.EXTENDED)),
            new KeyButtonData(477, 168, 53,  42, true,  false, new KeyScan(VirtualKeys.VK_RWIN,       0x5C, Keyboard.EXTENDED)),
            new KeyButtonData(530, 168, 47,  42, true,  false, new KeyScan(VirtualKeys.VK_APPS,       0x5D, Keyboard.EXTENDED)),
            new KeyButtonData(577, 168, 52,  42, true,  false, new KeyScan(VirtualKeys.VK_RCONTROL,   0x1D, Keyboard.EXTENDED)),
        };
        #endregion

        //
        // Summary:
        //
        public StandardKeyPanel(string name, int index, Point offset, TKeyboardAlignment alignment, KeyboardPanel parent)
            : base(parent)
        {
            this.SuspendLayout();

            this.TabIndex = index;
            this.Name = name;
            this.BackColor = Color.Transparent;
            this.Location = new Point(LEFT + offset.X, TOP + offset.Y);
            this.Size = new Size(WIDTH, HEIGHT);

            this.Initialize(alignment);
        }

        //
        // Summary:
        //
        private void Initialize(TKeyboardAlignment alignment)
        {
            for (int index = 0; index < buttonData.Length; index++)
            {
                KeyButton button = new KeyButton("SK" + index, (index + 1).ToString(), index, this);
                button.Defaults = buttonData[index];
                button.PrepareTooltip();

                if (buttonData[index].Convert)
                {
                    button.Text = Win32Wrapper.KeyText.GetKeyTextAtOnce(alignment, button.Keyscan, button.Text);
                }
                else
                {
                    button.Text = Win32Wrapper.GetKeyNameText(button.Keyscan, button.Text);
                }

                this.Controls.Add(button);
            }
        }

        //
        // Summary:
        //
        public void SetKeys(TKeyboardKeys keys)
        {
            this.SuspendLayout();

            switch (keys)
            {
                case TKeyboardKeys.Keys101:
                    HideOEM102Key();
                    HideWindowsKeys();
                    break;
                case TKeyboardKeys.Keys102:
                    HideWindowsKeys();
                    ShowOEM102Key();
                    break;
                case TKeyboardKeys.Keys105:
                    ShowWindowsKeys();
                    HideOEM102Key();
                    break;
                case TKeyboardKeys.Keys106:
                    ShowWindowsKeys();
                    ShowOEM102Key();
                    break;
            }

            this.ResumeLayout(true);
        }

        //
        // Summary:
        //
        public void SetAlignment(TKeyboardAlignment alignment)
        {
            this.SuspendLayout();

            switch (alignment)
            {
                case TKeyboardAlignment.Germany:
                    ShowGermany();
                    break;
                case TKeyboardAlignment.USStandard:
                    ShowUSStandard();
                    break;
            }

            this.ResumeLayout(true);
        }

        //
        // Summary:
        //
        private void HideOEM102Key()
        {
            KeyButton btnOEM102 = FindButtonUseVK(VirtualKeys.VK_OEM_102);
            if (btnOEM102.Visible)
            {
                KeyButton btnLShift = FindButtonUseVK(VirtualKeys.VK_LSHIFT);

                int helper = btnLShift.Size.Width + btnOEM102.Size.Width;

                btnLShift.Size = new Size(helper, btnLShift.Size.Height);
                btnOEM102.Visible = false; // This button must be invisible in this case.
            }
        }

        //
        // Summary:
        //
        private void ShowOEM102Key()
        {
            KeyButton btnOEM102 = FindButtonUseVK(VirtualKeys.VK_OEM_102);
            if (!btnOEM102.Visible)
            {
                btnOEM102.RestoreDefaults(); // No need to run through the loop twice...
                FindButtonUseVK(VirtualKeys.VK_LSHIFT).RestoreDefaults();
            }
        }

        //
        // Summary:
        //
        private void HideWindowsKeys()
        {
            KeyButton btnLWin = FindButtonUseVK(VirtualKeys.VK_LWIN);
            if (btnLWin.Visible)
            {
                KeyButton btnRWin = FindButtonUseVK(VirtualKeys.VK_RWIN);
                KeyButton btnApps = FindButtonUseVK(VirtualKeys.VK_APPS);
                KeyButton btnLCtrl = FindButtonUseVK(VirtualKeys.VK_LCONTROL);
                KeyButton btnRCtrl = FindButtonUseVK(VirtualKeys.VK_RCONTROL);
                KeyButton btnLMenu = FindButtonUseVK(VirtualKeys.VK_LMENU);
                KeyButton btnRMenu = FindButtonUseVK(VirtualKeys.VK_RMENU);
                KeyButton btnSpace = FindButtonUseVK(VirtualKeys.VK_SPACE);

                // Move left alt key onto left win key's position.
                btnLMenu.Location = new Point(btnLWin.Location.X, btnLWin.Location.Y);

                // Move space bar close to the right of left alt key.
                btnSpace.Location = new Point(btnLMenu.Location.X + btnLMenu.Size.Width, btnSpace.Location.Y);

                // Save right control key's curretnly used outer right location.
                int helper = btnRCtrl.Location.X + btnRCtrl.Size.Width;

                // Resize right control key.
                btnRCtrl.Size = new Size(btnLCtrl.Size.Width, btnLCtrl.Size.Height);

                // Move right control key to the outer rim of its old position.
                btnRCtrl.Location = new Point(helper - btnRCtrl.Size.Width, btnRCtrl.Location.Y);

                // Move right alt key close to right control key.
                btnRMenu.Location = new Point(btnRCtrl.Location.X - btnRMenu.Size.Width, btnRMenu.Location.Y);

                // Resize space bar so that is fits in between of both alt keys.
                helper = btnRMenu.Location.X - (btnLMenu.Location.X + btnLMenu.Size.Width);
                btnSpace.Size = new Size(helper, btnSpace.Size.Height);

                btnLWin.Visible = false; // This button must be invisible in this case.
                btnRWin.Visible = false; // This button must be invisible in this case.
                btnApps.Visible = false; // This button must be invisible in this case.
            }
        }

        //
        // Summary:
        //
        private void ShowWindowsKeys()
        {
            KeyButton btnLWin = FindButtonUseVK(VirtualKeys.VK_LWIN);
            if (!btnLWin.Visible)
            {
                btnLWin.RestoreDefaults(); // No need to run through the loop twice...
                FindButtonUseVK(VirtualKeys.VK_RWIN).RestoreDefaults();
                FindButtonUseVK(VirtualKeys.VK_APPS).RestoreDefaults();
                FindButtonUseVK(VirtualKeys.VK_LCONTROL).RestoreDefaults();
                FindButtonUseVK(VirtualKeys.VK_RCONTROL).RestoreDefaults();
                FindButtonUseVK(VirtualKeys.VK_LMENU).RestoreDefaults();
                FindButtonUseVK(VirtualKeys.VK_RMENU).RestoreDefaults();
                FindButtonUseVK(VirtualKeys.VK_SPACE).RestoreDefaults();
            }
        }

        //
        // Summary:
        //
        private void ShowGermany()
        {
            //
            // IMPORTANT: Read general comment in method ShowUSStandard()
            //
            const int SC_BUTTON_1A = 0x1A;
            const int SC_BUTTON_1B = 0x1B;
            const int SC_BUTTON_2B = 0x2B;

            KeyButton btnOEMx2B = FindButtonUseSC(SC_BUTTON_2B);
            if (!btnOEMx2B.Location.Equals(btnOEMx2B.Defaults.Location))
            {
                // Before starting with button movement 
                // the keyboard needs to be re-arranged!
                MapLayoutDependingKeys(TLayout.DE);

                // Now begin with button movement.
                KeyButton btnReturn = FindButtonUseVK(VirtualKeys.VK_RETURN);

                btnOEMx2B.RestoreDefaults(); // No need to run through the loop twice...
                FindButtonUseSC(SC_BUTTON_1A).RestoreDefaults();
                FindButtonUseSC(SC_BUTTON_1B).RestoreDefaults();
                btnReturn.RestoreDefaults(); // No need to run through the loop twice...

                // As last step current button text has to be adapted!
                Win32Wrapper.KeyText.Load(TKeyboardAlignment.Germany);
                for (int index = 0; index < this.Controls.Count; index++)
                {
                    // Ensure usage of keyboard buttons only.
                    if (this.Controls[index] is KeyButton)
                    {
                        KeyButton button = (KeyButton)this.Controls[index];
                        if (button.Defaults.Convert)
                        {
                            button.Text = Win32Wrapper.KeyText.GetKeyText(button.Keyscan);
                            button.PrepareTooltip();
                        }
                    }
                }
                Win32Wrapper.KeyText.Unload();
            }
        }

        //
        // Summary:
        //
        private void ShowUSStandard()
        {
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // Layout of used keybord alignments
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //
            // Following keyboard layout alignments show the arrangement of such keyboards
            // and the scancodes used for every needed keyboard key. To move some buttons 
            // around it is impossible to use virtual keys because they are differently 
            // definde. This means under several keyboards the OEM key VK_xxx names have 
            // a different meaning. That's the reason why scancodes must be used instead!
            // Button with VK_RETURN can always be identified through its virtual key code!
            // Thus, there is no need to use the scancode of button VK_RETURN.
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // 
            //                              Germany
            //       +-----+-----+--------+-------+
            //       |  P  | x1A |  x1B   |       |
            //    +--+--+--+--+--+--+-----+ enter |
            //    |  L  | x27 | x28 | x2B |       |
            // +--+--+-----+-----+--+--+--+-------+
            // |  M  | x33 | x34 | x35 |  shift   |
            // +-----+-----+-----+-----+----------+
            //
            //                          US-Standard
            //       +-----+-----+-----+----------+
            //       |  P  | x1A | x1B |   x2B    |
            //    ---+--+--+--+--+--+--+----------+
            //    |  L  | x27 | x28 |    enter    |
            // +-----+--+--+--+--+--+--+----------+
            // |  M  | x33 | x34 | x35 |   shift  |
            // +-----+-----+-----+-----+----------+
            //
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // Goal: 1) Shrink button with scancode 0x1B to size of button with scancode 0x1A.
            //       2) Move button with scancode 0x2B next to button with scancode 0x1B.
            //       3) Grow button with scancode 0x2B so that it fills the gap.
            //       4) Move and resize button VK_RETURN so that it fills the gap.
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            const int SC_BUTTON_1A = 0x1A;
            const int SC_BUTTON_1B = 0x1B;
            const int SC_BUTTON_2B = 0x2B;
            const int SC_BUTTON_28 = 0x28;

            KeyButton btnOEMx2B = FindButtonUseSC(SC_BUTTON_2B);
            if (btnOEMx2B.Location.Equals(btnOEMx2B.Defaults.Location))
            {
                // Before starting with button movement 
                // the keyboard needs to be re-arranged!
                MapLayoutDependingKeys(TLayout.US);

                // Now begin with button movement.
                KeyButton btnOEMx1A = FindButtonUseSC(SC_BUTTON_1A);
                KeyButton btnOEMx1B = FindButtonUseSC(SC_BUTTON_1B);
                KeyButton btnReturn = FindButtonUseVK(VirtualKeys.VK_RETURN);
                KeyButton btnOEMx28 = FindButtonUseSC(SC_BUTTON_28);

                // Shrink button with scancode 0x1B to size of button with scancode 0x1A.
                btnOEMx1B.Size = new Size(btnOEMx1A.Size.Width, btnOEMx1A.Size.Height);

                // Move button with scancode 0x2B next to button with scancode 0x1B.
                btnOEMx2B.Location = new Point(btnOEMx1B.Location.X + btnOEMx1B.Size.Width, btnOEMx1B.Location.Y);

                // Grow button with scancode 0x2B so that it fills the gap.
                int helper = btnReturn.Location.X + btnReturn.Size.Width;
                btnOEMx2B.Size = new Size(helper - btnOEMx2B.Location.X, btnOEMx2B.Location.Y);

                // Move and resize button VK_RETURN so that it fills the gap.
                helper = btnReturn.Location.X + btnReturn.Size.Width;
                btnReturn.Location = new Point(btnOEMx28.Location.X + btnOEMx28.Size.Width, btnOEMx28.Location.Y);
                btnReturn.Size = new Size(helper - btnReturn.Location.X, btnOEMx28.Size.Height);

                // As last step current button text has to be adapted!
                Win32Wrapper.KeyText.Load(TKeyboardAlignment.USStandard);
                for (int index = 0; index < this.Controls.Count; index++)
                {
                    // Ensure usage of keyboard buttons only.
                    if (this.Controls[index] is KeyButton)
                    {
                        KeyButton button = (KeyButton)this.Controls[index];
                        if (button.Defaults.Convert)
                        {
                            button.Text = Win32Wrapper.KeyText.GetKeyText(button.Keyscan);
                            button.PrepareTooltip();
                        }
                    }
                }
                Win32Wrapper.KeyText.Unload();
            }
        }

        //
        // Summary:
        //
        private void MapLayoutDependingKeys(TLayout layout)
        {
            // NOTE: Depending on physical keyboard layouts apportionment of the 
            //       virtual key codes may vary. In the special case of using 
            //       german and US keyboard layouts the virtual keys are really 
            //       different. For details see following list.
            //
            //       Scancode | DE           | US
            //       ---------+--------------+-------------
            //       0x29     | VK_OEM_5     | VK_OEM_3
            //       0x0C     | VK_OEM_4     | VK_OEM_MINUS
            //       0x0D     | VK_OEM_6     | VK_OEM_PLUS
            //       0x15     | VK_Z         | VK_Y
            //       0x1A     | VK_OEM_1     | VK_OEM_4
            //       0x1B     | VK_OEM_PLUS  | VK_OEM_6
            //       0x27     | VK_OEM_3     | VK_OEM_1
            //       0x2B     | VK_OEM_2     | VK_OEM_5
            //       0x2C     | VK_Y         | VK_Z
            //       0x35     | VK_OEM_MINUS | VK_OEM_2
            //
            //       The only think what is hopefully constant in that relation 
            //       is the scancode of every keyboard key. Thus, the scancode 
            //       remains on every single key but the virtual key must be 
            //       changed. This is this methods job.

            int[][] layoutMap = new int[][]{
                //         SC    DE                        US
                new int[] {0x29, VirtualKeys.VK_OEM_5,     VirtualKeys.VK_OEM_3},
                new int[] {0x0C, VirtualKeys.VK_OEM_4,     VirtualKeys.VK_OEM_MINUS},
                new int[] {0x0D, VirtualKeys.VK_OEM_6,     VirtualKeys.VK_OEM_PLUS},
                new int[] {0x15, VirtualKeys.VK_Z,         VirtualKeys.VK_Y},
                new int[] {0x1A, VirtualKeys.VK_OEM_1,     VirtualKeys.VK_OEM_4},
                new int[] {0x1B, VirtualKeys.VK_OEM_PLUS,  VirtualKeys.VK_OEM_6},
                new int[] {0x27, VirtualKeys.VK_OEM_3,     VirtualKeys.VK_OEM_1},
                new int[] {0x2B, VirtualKeys.VK_OEM_2,     VirtualKeys.VK_OEM_5},
                new int[] {0x2C, VirtualKeys.VK_Y,         VirtualKeys.VK_Z},
                new int[] {0x35, VirtualKeys.VK_OEM_MINUS, VirtualKeys.VK_OEM_2},
            };

            for (int index = 0; index < layoutMap.Length; index++)
            {
                if (layout == TLayout.DE)
                {
                    FindButtonUseSC(layoutMap[index][0]).Keyscan.VKeyCode = layoutMap[index][1];
                }
                else if (layout == TLayout.US)
                {
                    FindButtonUseSC(layoutMap[index][0]).Keyscan.VKeyCode = layoutMap[index][2];
                }
            }
        }
    }
}
