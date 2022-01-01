using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ScancodeMapping
{
    public partial class MappingRawData : Form
    {
        public MappingRawData( string[] listData )
        {
            InitializeComponent();
            rawDataListBox.Items.AddRange(listData);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
