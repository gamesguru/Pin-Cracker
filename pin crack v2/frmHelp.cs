using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1
    {
    public partial class frmHelp : Form
        {
        public frmHelp()
            {
            InitializeComponent();
            }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
            System.Diagnostics.Process.Start("mailto:mathmuncher11@gmail.com");
            }

        private void button1_Click(object sender, EventArgs e)
            {
            this.Close();
            }

        private void frmHelp_Load(object sender, EventArgs e)
            {
            textBox1.Select(0, 0);
            }
        }
    }