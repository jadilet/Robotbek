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
            
            private static readonly Dictionary<int, string> VIEW = new Dictionary<int, string>() {
                { NORTH, "▲" },
                { EAST,  "►" },
                { WEST,  "◄" },
                { SOUTH, "▼" }
            };

            public static String View(int direction)
            {
                return VIEW[direction];
            }
        }

        public class Game
        {
            public virtual void Run()
            {

            }
        }

        public class Robot : Game
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
                currentFace = Face.NORTH;

                // Robot default position
                currentPlaceX = 0;
                currentPlaceY = 0;
            }

            void Facing(int face)
            {
               if (face >= Face.NORTH && face <= Face.WEST)
                {
                    currentFace = face;
                }
            }

            public Robot(int dimension)
            {
                BOARD_DIMENSION = dimension;
                board = new int[BOARD_DIMENSION, BOARD_DIMENSION];

                // Robot default face is NORTH
                currentFace = Face.NORTH;

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
                if (currentFace == Face.WEST)
                {
                    currentFace = Face.NORTH;
                } else if (currentFace < Face.WEST)
                {
                    currentFace += 1;
                }
            }

            public void Show()
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine("");
                for (int i = 0; i < BOARD_DIMENSION; i++)
                {
                    for (int j = 0; j < BOARD_DIMENSION;j++)
                    {
                        if (i == currentPlaceX && currentPlaceY == j)
                        {
                            Console.Write("{0}  ", Face.View(currentFace));
                        } 
                        else
                        {
                            Console.Write("{0}  ", board[i, j]);
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            public void MoveForward()
            {
                switch (currentFace)
                {
                    case Face.NORTH:
                        if (currentPlaceX > 0)
                            currentPlaceX -= 1;
                        else
                            throw new RobotMovementException("You can't go forward");
                        
                        break;
                    case Face.SOUTH:
                        if (currentPlaceX < BOARD_DIMENSION - 1)
                            currentPlaceX += 1;
                        else
                            throw new RobotMovementException("You can't go forward");

                        break;
                    case Face.WEST:
                        if (currentPlaceY > 0)
                            currentPlaceY -= 1;
                        else
                            throw new RobotMovementException("You can't go forward");

                        break;
                    case Face.EAST:
                        if (currentPlaceY < BOARD_DIMENSION - 1)
                            currentPlaceY += 1;
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

            // Stop robot when the all the cells are clean
            public bool IsOver()
            {
                int total = 0;
                for (int i = 0; i < BOARD_DIMENSION; i++)
                    for (int j = 0; j < BOARD_DIMENSION; j++)
                        if (board[i, j] != DIRT)
                            total++;

                return total == BOARD_DIMENSION * BOARD_DIMENSION;
            }

            public override void Run()
            {
                int com, x, y;
                Show();
                while(true)
                {
                    try
                    {
                        Console.WriteLine("Please choose following commands: ");
                        Console.WriteLine("1. Dirt");
                        Console.WriteLine("2. IN");
                        Console.WriteLine("3. Facing");
                        Console.WriteLine("4. MoveForward");
                        Console.WriteLine("5. TurnRight");
                        Console.WriteLine("6. CleanCell");
                        Console.WriteLine("");

                        com = Convert.ToInt32(Console.ReadLine());

                        switch (com)
                        {
                            case 1:
                                Console.WriteLine("X: ");
                                x = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Y: ");
                                y = Convert.ToInt32(Console.ReadLine());
                                Dirt(x, y);
                                Show();
                                break;
                            case 2:
                                Console.WriteLine("X: ");
                                x = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Y: ");
                                y = Convert.ToInt32(Console.ReadLine());
                                Inn(x, y);
                                Show();
                                break;
                            case 3:
                                Console.WriteLine("Please choose following direction: ");
                                Console.WriteLine("{0} North\n{1} East\n{2} South\n{3} West", 
                                    Face.NORTH, Face.EAST, Face.SOUTH, Face.WEST);
                                x = Convert.ToInt32(Console.ReadLine());
                                Facing(x);
                                Show();
                                break;
                            case 4:
                                MoveForward();
                                Show();
                                break;
                            case 5:
                                TurnRight();
                                Show();
                                break;
                            case 6:
                                CleanCell();
                                Show();
                                
                                if (IsOver()) return;

                                break;
                            default:
                                Console.WriteLine("Please choose the right command!");
                                break;
                        }
                    } 
                    catch(IndexOutOfRangeException ex)
                    {
                        Console.WriteLine(ex);
                    } 
                    catch(RobotMovementException ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Game game = new Robot();
            game.Run();
        }
    }
}
