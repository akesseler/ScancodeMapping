using System;
using System.Drawing;
using ScancodeHook.LowLevel;

namespace ScancodeMapping
{
    // The designer requires this class as the first part of this file!
    public class ControlKeyPanel : KeyPanel
    {
        private const int LEFT = 629;
        private const int TOP = 0;
        private const int WIDTH = 168;
        private const int HEIGHT = 270;

        #region Buttons' location, size and VK assignments.
        private KeyButtonData[] buttonData = new KeyButtonData[]{
            new KeyButtonData(21,  0,   42, 42, true, false, new KeyScan(VirtualKeys.VK_SNAPSHOT, 0x37, Keyboard.EXTENDED)),
            new KeyButtonData(63,  0,   42, 42, true, false, new KeyScan(VirtualKeys.VK_SCROLL,   0x46, Keyboard.STANDARD)),
            new KeyButtonData(105, 0,   42, 42, true, false, new KeyScan(VirtualKeys.VK_PAUSE,    0x45, Keyboard.STANDARD)),
            new KeyButtonData(21,  60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_INSERT,   0x52, Keyboard.EXTENDED)),
            new KeyButtonData(63,  60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_HOME,     0x47, Keyboard.EXTENDED)),
            new KeyButtonData(105, 60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_PRIOR,    0x49, Keyboard.EXTENDED)),
            new KeyButtonData(21,  102, 42, 42, true, false, new KeyScan(VirtualKeys.VK_DELETE,   0x53, Keyboard.EXTENDED)),
            new KeyButtonData(63,  102, 42, 42, true, false, new KeyScan(VirtualKeys.VK_END,      0x4F, Keyboard.EXTENDED)),
            new KeyButtonData(105, 102, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NEXT,     0x51, Keyboard.EXTENDED)),
            new KeyButtonData(63,  186, 42, 42, true, false, new KeyScan(VirtualKeys.VK_UP,       0x48, Keyboard.EXTENDED)),
            new KeyButtonData(21,  228, 42, 42, true, false, new KeyScan(VirtualKeys.VK_LEFT,     0x4B, Keyboard.EXTENDED)),
            new KeyButtonData(63,  228, 42, 42, true, false, new KeyScan(VirtualKeys.VK_DOWN,     0x50, Keyboard.EXTENDED)),
            new KeyButtonData(105, 228, 42, 42, true, false, new KeyScan(VirtualKeys.VK_RIGHT,    0x4D, Keyboard.EXTENDED))
        };
        #endregion

        //
        // Summary:
        //
        public ControlKeyPanel(string name, int index, Point offset, TKeyboardAlignment alignment, KeyboardPanel parent)
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
                KeyButton button = new KeyButton("CK" + index, (index + 1).ToString(), index, this);
                button.Defaults = buttonData[index];
                button.PrepareTooltip();

                if (buttonData[index].Convert)
                {
                    button.Text = Win32Wrapper.KeyText.GetKeyTextAtOnce( alignment, button.Keyscan, button.Text );
                }
                else
                {
                    button.Text = Win32Wrapper.GetKeyNameText( button.Keyscan, button.Text );
                }

                this.Controls.Add(button);
            }
        }
    }
}
