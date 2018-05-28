using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048_Game
{
    public partial class Main : Form
    {
        public const int WIDHT = 4;
        public const int HEIGHT = 4;

        RichTextBox[,] texts = new RichTextBox[WIDHT,HEIGHT];

        GameBoard gb = new GameBoard(WIDHT, HEIGHT);

        public Main()
        {
            InitializeComponent();

            texts[0, 0] = TextBox00;
            texts[1, 0] = TextBox10;
            texts[2, 0] = TextBox20;
            texts[3, 0] = TextBox30;
            texts[0, 1] = TextBox01;
            texts[1, 1] = TextBox11;
            texts[2, 1] = TextBox21;
            texts[3, 1] = TextBox31;
            texts[0, 2] = TextBox02;
            texts[1, 2] = TextBox12;
            texts[2, 2] = TextBox22;
            texts[3, 2] = TextBox32;
            texts[0, 3] = TextBox03;
            texts[1, 3] = TextBox13;
            texts[2, 3] = TextBox23;
            texts[3, 3] = TextBox33;

            gb.NextRound();
            gb.Update(this);
        }

        private void btn_up_Click(object sender, EventArgs e)
        {
            int[,] cells = (int[,])gb.cells.Clone();

            gb.Move(Dir.UP);
            gb.Collide(Dir.UP);
            gb.Move(Dir.UP);

            if(!gb.cells.OfType<int>().SequenceEqual(cells.OfType<int>())) gb.Update(this);
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            int[,] cells = (int[,])gb.cells.Clone();

            gb.Move(Dir.DOWN);
            gb.Collide(Dir.DOWN);
            gb.Move(Dir.DOWN);

            if (!gb.cells.OfType<int>().SequenceEqual(cells.OfType<int>())) gb.Update(this);
        }

        private void btn_right_Click(object sender, EventArgs e)
        {
            int[,] cells = (int[,])gb.cells.Clone();

            gb.Move(Dir.RIGHT);
            gb.Collide(Dir.RIGHT);
            gb.Move(Dir.RIGHT);

            if (!gb.cells.OfType<int>().SequenceEqual(cells.OfType<int>())) gb.Update(this);
        }

        private void btn_left_Click(object sender, EventArgs e)
        {
            int[,] cells = (int[,])gb.cells.Clone();

            gb.Move(Dir.LEFT);
            gb.Collide(Dir.LEFT);
            gb.Move(Dir.LEFT);

            if (!gb.cells.OfType<int>().SequenceEqual(cells.OfType<int>())) gb.Update(this);
        }

        public void UpdateTexts(int[,] cells)
        {
            gb.NextRound();

            string s = "";
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    texts[x, y].Text = cells[x, y] != 0 ? cells[x, y].ToString() : "";
                    s += texts[x, y].Text + ", ";
                }
                s += "\n";
            }

            System.Diagnostics.Debug.WriteLine(s);

            
        }

        private void btn_restart_Click(object sender, EventArgs e)
        {
            gb.Restart();
            gb.NextRound();
            gb.Update(this);
        }
    }
}
