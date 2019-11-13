using System;
using System.Collections.Generic;

namespace RobotBek
{
    class Program
    {

        // Robot Exception
        public class RobotMovementException : Exception
        {
            public RobotMovementException(string message) : base(message)
            {
            }
        }

        // Robot Face values
        public class Face
        {
            public const int NORTH = 1;
            public const int EAST  = 2;
            public const int SOUTH = 3;
            public const int WEST  = 4;
            
            private readonly Dictionary<int, string> VIEW = new Dictionary<int, string>() {
                { NORTH, "▲" },
                { EAST,  "►" },
                { WEST,  "◄" },
                { SOUTH, "▼" }
            };

            public String View(int direction)
            {
                return VIEW[direction];
            }
        }
        public class Robot : Face
        {
            // Constants
            const int DIRT = 1;
            private readonly int BOARD_DIMENSION = 3;

            private int[,] board;
            private int currentPlaceX, currentPlaceY;
            private int currentFace;

            public Robot()
            {
                board = new int[BOARD_DIMENSION, BOARD_DIMENSION];

                // Robot default face is NORTH
                currentFace = NORTH;

                // Robot default position
                currentPlaceX = 0;
                currentPlaceY = 0;
            }

            public Robot(int dimension)
            {
                BOARD_DIMENSION = dimension;
                board = new int[BOARD_DIMENSION, BOARD_DIMENSION];

                // Robot default face is NORTH
                currentFace = NORTH;

                // Robot default position
                currentPlaceX = 0;
                currentPlaceY = 0;
            }

            // Change robot by position (x, y)
            public void Inn(int x, int y)
            {
                // board.GetLength(0) -> gets row size
                // board.GetLength(1) -> gets column size
                if (x >= board.GetLength(0) && y >= board.GetLength(1))
                {
                    new IndexOutOfRangeException();
                }

                currentPlaceX = x;
                currentPlaceY = y;
            }

            // Clean current cell
            public void CleanCell()
            {
                board[currentPlaceX, currentPlaceY] = 0;
            }

            public void TurnRight()
            {
                if (currentFace == WEST)
                {
                    currentFace = NORTH;
                } else if (currentFace < WEST)
                {
                    currentFace += 1;
                }
            }

            public void Show()
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                for (int i = 0; i < BOARD_DIMENSION; i++)
                {
                    for (int j = 0; j < BOARD_DIMENSION;j++)
                    {
                        if (i == currentPlaceX && currentPlaceY == j)
                        {
                            Console.Write("{0}  ", View(currentFace));
                        } 
                        else
                        {
                            Console.Write("{0}  ", board[i, j]);
                        }
                    }
                    Console.WriteLine();
                }
            }

            public void MoveForward()
            {
                switch (currentFace)
                {
                    case NORTH:
                        if (currentPlaceY > 0)
                            currentPlaceY -= 1;
                        else
                            throw new RobotMovementException("You can't go forward");
                        
                        break;
                    case SOUTH:
                        if (currentPlaceY < BOARD_DIMENSION - 1)
                            currentPlaceY += 1;
                        else
                            throw new RobotMovementException("You can't go forward");

                        break;
                    case WEST:
                        if (currentPlaceX > 0)
                            currentPlaceX -= 1;
                        else
                            throw new RobotMovementException("You can't go forward");

                        break;
                    case EAST:
                        if (currentPlaceX < BOARD_DIMENSION - 1)
                            currentPlaceX += 1;
                        else
                            throw new RobotMovementException("You can't go forward");

                        break;
                    default:
                        return;
                }
            }

            public void Dirt(int x, int y)
            {
                if (x >= board.GetLength(0) && y >= board.GetLength(1))
                {
                    new IndexOutOfRangeException("You can't go outside of board");
                }

                board[x, y] = DIRT;
            }


        }

        public class Game
        {
            private readonly Robot robot;

            public Game()
            {
                robot = new Robot();
            }

            public void Run()
            {
                try
                {
                    robot.Inn(1, 1);
                    robot.Show();
                } 
                catch(RobotMovementException ex)
                {
                    Console.WriteLine(ex);
                }
                catch(IndexOutOfRangeException ex)
                {
                    Console.WriteLine(ex);
                }
                
            }
        }

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }
}
