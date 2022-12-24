using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRPAnalisis
{
    public partial class CreateForm : Form
    {
        //private TRPData _TRP;
        public CreateForm()
        {
            InitializeComponent();
        }
        //public CreateForm(TRPData trp) : this()
        //{
        //    _TRP = trp;
        //}
        private void btnCreateOk_Click(object sender, EventArgs e)
        {
          TRPData.Id = 17;

            DialogResult = DialogResult.OK;
            Close();

        }
    }
}
