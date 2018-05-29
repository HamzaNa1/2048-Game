﻿using System;
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

        int currentRound = 0;
        
        Random r = new Random();

        public GameBoard(int width, int height)
        {
            this.width = width;
            this.height = height;

            cells = new int[width, height];

            for(int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y] = 0;
                }
            }
        }

        public GameBoard Copy()
        {
            GameBoard gb = new GameBoard(width, height);

            gb.cells = (int[,])cells.Clone();
            gb.currentRound = currentRound;
            gb.r = r;
            gb.isLost = isLost;
            
            return gb;
        }

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

        public void Update(Main Game)
        {
            Game.UpdateTexts(cells);
        }

        public void NextRound()
        {
            int? i = null;
            int? j = null;
            int tries = 0;
            do
            {
                if(tries == 1000)
                {
                    break;
                }
                i = r.Next(0, width);
                j = r.Next(0, height);
                tries++;
            } while (cells[(int)i, (int)j] != 0);

            if (i != null && j != null) cells[(int)i, (int)j] = r.Next(0, 100) >= 20 ? 2 : 4;
            else
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    for (int y = 0; y < cells.GetLength(1); y++)
                    {
                        if(cells[x, y] != 0) cells[x, y] = r.Next(0, 100) >= 20 ? 2 : 4;
                    }
                }
            }

            isLost = CheckLost();

            currentRound++;
        }

        public bool CheckLost()
        {
            GameBoard gb = this.Copy();

            var g = gb.Copy();
            gb.Move(Dir.UP);
            gb.Collide(Dir.UP);
            if (gb.cells.OfType<int>().SequenceEqual(g.cells.OfType<int>()))
            {
                gb.Move(Dir.DOWN);
                gb.Collide(Dir.DOWN);
                if (gb.cells.OfType<int>().SequenceEqual(g.cells.OfType<int>()))
                {
                    gb.Move(Dir.LEFT);
                    gb.Collide(Dir.LEFT);
                    if (gb.cells.OfType<int>().SequenceEqual(g.cells.OfType<int>()))
                    {
                        gb.Move(Dir.RIGHT);
                        gb.Collide(Dir.RIGHT);
                        if (gb.cells.OfType<int>().SequenceEqual(g.cells.OfType<int>()))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void Move(Dir dir)
        {
            for (int p = 0; p < 4; p++)
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

        public void Collide(Dir dir)
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