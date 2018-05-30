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

            KeyPreview = true;

            // Setting up the text boxes
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

            // Starting the game
            gb.NextRound();
            gb.Update(this);
        }

        private void btn_up_Click(object sender, EventArgs e)
        {
            int[,] cells = (int[,])gb.cells.Clone();

            gb.Move(Dir.UP);
            gb.Sum(Dir.UP);
            gb.Move(Dir.UP);

            if(!gb.cells.OfType<int>().SequenceEqual(cells.OfType<int>())) gb.Update(this);
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            int[,] cells = (int[,])gb.cells.Clone();

            gb.Move(Dir.DOWN);
            gb.Sum(Dir.DOWN);
            gb.Move(Dir.DOWN);

            if (!gb.cells.OfType<int>().SequenceEqual(cells.OfType<int>())) gb.Update(this);
        }

        private void btn_right_Click(object sender, EventArgs e)
        {
            int[,] cells = (int[,])gb.cells.Clone();

            gb.Move(Dir.RIGHT);
            gb.Sum(Dir.RIGHT);
            gb.Move(Dir.RIGHT);

            if (!gb.cells.OfType<int>().SequenceEqual(cells.OfType<int>())) gb.Update(this);
        }

        private void btn_left_Click(object sender, EventArgs e)
        {
            int[,] cells = (int[,])gb.cells.Clone();

            gb.Move(Dir.LEFT);
            gb.Sum(Dir.LEFT);
            gb.Move(Dir.LEFT);

            if (!gb.cells.OfType<int>().SequenceEqual(cells.OfType<int>())) gb.Update(this);
        }

        public void UpdateTexts(int[,] cells)
        {
            // Goes to the next round
            gb.NextRound();

            // Checks if the play has lost yet
            if (gb.isLost) Lost_lbl.Text = "Gameover!";

            // Changes the texts on the text boxes to match the cell value
            string s = "";
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    texts[x, y].Text = cells[x, y] != 0 ? cells[x, y].ToString() : "";
                    s += cells[x, y] + ", ";
                }
                s += "\n";
            }

            System.Diagnostics.Debug.WriteLine(s);
        }

        private void btn_restart_Click(object sender, EventArgs e)
        {
            // Restart the game
            Lost_lbl.Text = "";
            gb.Restart();
            gb.NextRound();
            gb.Update(this);
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            // Keyboard Support
            switch (e.KeyCode)
            {
                case Keys.Up:
                    btn_up_Click(null, null);
                    break;
                case Keys.Down:
                    btn_down_Click(null, null);
                    break;
                case Keys.Right:
                    btn_right_Click(null, null);
                    break;
                case Keys.Left:
                    btn_left_Click(null, null);
                    break;
            }
        }
    }
}
