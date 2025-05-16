using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TicTacToe : Form
    {
        public TicTacToe()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private Color _btnDown = Color.FromArgb(64,64,64);
        private Color _btnUp = System.Drawing.Color.Gray;

        private void ButtonMouseEnter(Label lbl)
        {
            lbl.BackColor = _btnDown;
            Cursor = Cursors.Hand;
        }
        private void ButtonMouseLeave(Label lbl)
        {
            lbl.BackColor = _btnUp;
            Cursor = Cursors.Default;
        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblExit_MouseEnter(object sender, EventArgs e)
        {
            ButtonMouseEnter(lblExit);
        }

        private void lblExit_MouseLeave(object sender, EventArgs e)
        {
            ButtonMouseLeave(lblExit);
        }
    }
}
