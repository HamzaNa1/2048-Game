using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Game
{
    class GameBoard
    {
        public int[,] cells { get; private set; }

        public bool isLost { get; private set; }

        int width;
        int height;

        int RoundCount = 0;
        
        Random r = new Random();
        /// <summary>
        /// Create a new GameBoard
        /// </summary>
        /// <param name="width">The width of the GameBoard</param>
        /// <param name="height">The height of the GameBoard</param>
        public GameBoard(int width, int height)
        {
            this.width = width;
            this.height = height;

            //Create the cells
            cells = new int[width, height];

            //Restart the GameBoard
            Restart();
        }


        /// <summary>
        /// Makes a shallow copy of the GameBoard
        /// </summary>
        /// <returns>a shallow copy of the GameBoard</returns>
        public GameBoard Copy()
        {
            GameBoard gb = new GameBoard(width, height);

            gb.cells = (int[,])cells.Clone();
            gb.RoundCount = RoundCount;
            gb.r = r;
            gb.isLost = isLost;
            
            return gb;
        }


        /// <summary>
        /// Reset the GameBoard values
        /// </summary>
        public void Restart()
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y] = 0;
                }
            }
        }

        /// <summary>
        /// Updates the texts on the form
        /// </summary>
        /// <param name="Game">The Main form</param>
        public void Update(Main Game)
        {
            Game.UpdateTexts(cells);
        }


        /// <summary>
        /// Setup the next round
        /// </summary>
        public void NextRound()
        {
            int? i = null;
            int? j = null;
            int tries = 0;
            do
            {
                // Breaks out of the loop if there's no available cell
                if (tries == 5000)
                {
                    break;
                }

                // Picks 2 random coordinates
                i = r.Next(0, width);
                j = r.Next(0, height);

                // Plus up the amount of tries
                tries++;

                // Checks if the cell is available
            } while (cells[(int)i, (int)j] != 0);

            // Changes the cell value to 2 (80%) or 4 (20%)
            if (i != null && j != null) cells[(int)i, (int)j] = r.Next(0, 100) >= 20 ? 2 : 4;
            else
            {
                // if the randomizer didnt find any cell picks the next available cell inorder

                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    for (int y = 0; y < cells.GetLength(1); y++)
                    {
                        if(cells[x, y] != 0) cells[x, y] = r.Next(0, 100) >= 20 ? 2 : 4;
                    }
                }
            }

            // Checks if the player lost
            isLost = CheckLost();
            
            // Plus up the round count
            RoundCount++;
        }

        /// <summary>
        /// Checks if the player has lost
        /// </summary>
        /// <returns>Whether the player has lost or hasnt lost</returns>
        public bool CheckLost()
        {
            // Makes 2 shallow copies of the board
            GameBoard gb = this.Copy();
            var g = gb.Copy();

            // Try to move one copy and then comparing it with the other one
            // If both copies were the same, the player loses
            gb.Move(Dir.UP);
            gb.Sum(Dir.UP);
            if (gb.cells.OfType<int>().SequenceEqual(g.cells.OfType<int>()))
            {
                gb.Move(Dir.DOWN);
                gb.Sum(Dir.DOWN);
                if (gb.cells.OfType<int>().SequenceEqual(g.cells.OfType<int>()))
                {
                    gb.Move(Dir.LEFT);
                    gb.Sum(Dir.LEFT);
                    if (gb.cells.OfType<int>().SequenceEqual(g.cells.OfType<int>()))
                    {
                        gb.Move(Dir.RIGHT);
                        gb.Sum(Dir.RIGHT);
                        if (gb.cells.OfType<int>().SequenceEqual(g.cells.OfType<int>()))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Moves the all the cells to a specific direction
        /// </summary>
        /// <param name="dir">the direction to move the cells to</param>
        public void Move(Dir dir)
        {
            // Moves 3 times to make sure the cell moved as much as it can
            for (int p = 0; p < 3; p++)
            {
                switch (dir)
                {
                    case Dir.UP:
                        for (int x = 0; x < cells.GetLength(0); x++)
                        {
                            for (int y = 1; y < cells.GetLength(1); y++)
                            {
                                if (cells[x, y - 1] == 0)
                                {
                                    cells[x, y - 1] = cells[x, y];
                                    cells[x, y] = 0;
                                }
                            }
                        }
                        break;
                    case Dir.DOWN:
                        for (int x = 0; x < cells.GetLength(0); x++)
                        {
                            for (int y = cells.GetLength(1) - 2; y >= 0; y--)
                            {
                                if (cells[x, y + 1] == 0)
                                {
                                    cells[x, y + 1] = cells[x, y];
                                    cells[x, y] = 0;
                                }
                            }
                        }
                        break;
                    case Dir.RIGHT:
                        for (int x = cells.GetLength(0) - 2; x >= 0; x--)
                        {
                            for (int y = 0; y < cells.GetLength(1); y++)
                            {
                                if (cells[x + 1, y] == 0)
                                {
                                    cells[x + 1, y] = cells[x, y];
                                    cells[x, y] = 0;
                                }
                            }
                        }
                        break;
                    case Dir.LEFT:
                        for (int x = 1; x < cells.GetLength(0); x++)
                        {
                            for (int y = 0; y < cells.GetLength(1); y++)
                            {
                                if (cells[x - 1, y] == 0)
                                {
                                    cells[x - 1, y] = cells[x, y];
                                    cells[x, y] = 0;
                                }
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Sums the cells with the same values in a specific direction
        /// </summary>
        /// <param name="dir">the direction of the cells</param>
        public void Sum(Dir dir)
        {
            switch (dir)
            {
                case Dir.UP:
                    for (int x = 0; x < cells.GetLength(0); x++)
                    {
                        for (int y = 1; y < cells.GetLength(1); y++)
                        {
                            if (cells[x, y - 1] == cells[x, y])
                            {
                                cells[x, y - 1] = cells[x, y] + cells[x, y - 1];
                                cells[x, y] = 0;
                            }
                        }
                    }
                    break;
                case Dir.DOWN:
                    for (int x = 0; x < cells.GetLength(0); x++)
                    {
                        for (int y = cells.GetLength(1) - 2; y >= 0; y--)
                        {
                            if (cells[x, y + 1] == cells[x, y])
                            {
                                cells[x, y + 1] = cells[x, y] + cells[x, y + 1];
                                cells[x, y] = 0;
                            }
                        }
                    }
                    break;
                case Dir.RIGHT:
                    for (int x = cells.GetLength(0) - 2; x >= 0; x--)
                    {
                        for (int y = 0; y < cells.GetLength(1); y++)
                        {
                            if (cells[x + 1, y] == cells[x, y])
                            {
                                cells[x + 1, y] = cells[x, y] + cells[x + 1, y];
                                cells[x, y] = 0;
                            }
                        }
                    }
                    break;
                case Dir.LEFT:
                    for (int x = 1; x < cells.GetLength(0); x++)
                    {
                        for (int y = 0; y < cells.GetLength(1); y++)
                        {
                            if (cells[x - 1, y] == cells[x, y])
                            {
                                cells[x - 1, y] = cells[x, y] + cells[x - 1, y];
                                cells[x, y] = 0;
                            }
                        }
                    }
                    break;
            }
        }

    }
}


public enum Dir
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}