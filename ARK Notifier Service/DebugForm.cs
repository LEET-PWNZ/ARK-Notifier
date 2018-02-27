using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARKNotifierService
{
    public partial class DebugForm : Form
    {
        private ServiceCore serviceCore = null;
        public DebugForm()
        {
            InitializeComponent();
            serviceCore = new ServiceCore();
            serviceCore.DebugStart();
        }

        private void DebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            serviceCore.Stop();
        }
    }
}
