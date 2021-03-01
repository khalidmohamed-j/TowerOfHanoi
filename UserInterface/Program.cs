using System;
using TowerOfHanoi;
using System.Collections.Generic;

namespace UserInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            bool playGame = true;
            bool notValidEntry = true;
            string userInput;
            int numberOfDiscs;
            Towers poles;

            // Source tower
            int sourceTower = 0;

            // Destination tower
            int destinationTower = 0;

            int auxTower = 2;

            // Move Log 
            Queue<MoveRecord> moveLog = new Queue<MoveRecord>();

            // Temp Move Record
            MoveRecord tempMoveRecord;

            const string CtrlZ = "Z";
            const string CtrlY = "Y";


            Stack<MoveRecord> undoLog = new Stack<MoveRecord>();

            Stack<MoveRecord> redoLog = new Stack<MoveRecord>();

            while (playGame)
            {
                Console.WriteLine("How many discs in your tower (default is 5, max is 9)");
                userInput = Console.ReadLine().ToUpper();

                if (userInput == "X")
                {
                    Console.WriteLine("Bye!");
                    break;
                }

                numberOfDiscs = Convert.ToInt32(userInput);
                poles = new Towers(numberOfDiscs);
                Console.Clear();
                TowerUtilities.DisplayTowers(poles);

                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("- M - Solve the puzzle manually");
                Console.WriteLine("- A - Auto-solve Recursively");
                Console.WriteLine("- S - Auto-solve Iteratively");
                Console.WriteLine();
                Console.Write("Choose an approach: ");
                userInput = Console.ReadLine().ToUpper();


                if (userInput.ToUpper() == "A")
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter to watch closely!");
                    userInput = Console.ReadLine().ToUpper();

                    if (userInput != null)
                    {
                        sourceTower = 1;
                        destinationTower = 3;
                        RecursiveAutoSolve(poles.NumberOfDiscs, sourceTower, destinationTower, auxTower, moveLog, 1);
                        Console.WriteLine($"Recursion Call Finished");

                        foreach (var moveRecord in moveLog)
                        {
                            sourceTower = moveRecord.From;
                            destinationTower = moveRecord.To;
                            tempMoveRecord = poles.Move(sourceTower, destinationTower);
                            TowerUtilities.DisplayTowers(poles);

                            Console.WriteLine($"Move {poles.NumberOfMoves} completed. Successfully moved disc {moveRecord.Disc} from tower {moveRecord.From} to tower {moveRecord.To}.");

                            System.Threading.Thread.Sleep(250);
                            //Console.ReadLine();
                        }
                        Console.WriteLine($"Number of moves: {poles.NumberOfMoves}");
                    }

                }
                else if (userInput.ToUpper() == "S")
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter to watch closely!");
                    userInput = Console.ReadLine();

                    if (userInput != null)
                    {
                        sourceTower = 1;
                        destinationTower = 3;

                        IterativeAutoSolve(poles, numberOfDiscs, moveLog, sourceTower, auxTower, destinationTower);

                    }
                }
                else
                {
                    do
                    {

                        //First Question- Source tower

                        do
                        {
                            // If block logic to determine if redoLog has entry. If log has entry, display redo message
                            if (redoLog.Count == 0)
                            {
                                Console.WriteLine("Enter 'from' tower number 1,2 or 3, Z to undo");
                            }
                            else
                            {
                                Console.WriteLine("Enter 'from' tower number 1,2 or 3, Z to undo or Y to redo");
                            }

                            userInput = Console.ReadLine().ToUpper();

                            if (userInput == "X")
                            {
                                playGame = false;
                                break;
                            }

                            else
                            {
                                //Check for ctrl keys, if none, convert to int
                                if (userInput == CtrlZ || userInput == CtrlY)
                                {
                                    Console.WriteLine("UNDO / REDO MOVE Requested");
                                    notValidEntry = false;
                                }
                                else
                                {
                                    sourceTower = Convert.ToInt32(userInput);
                                    if (sourceTower < 1 || sourceTower > 3)
                                    {
                                        Console.WriteLine("Invalid Tower Number");
                                    }
                                    else
                                    {
                                        notValidEntry = false;
                                    }
                                }

                            }
                        } while (notValidEntry);

                        //Second question - Destination tower
                        //Wrap 2nd question in If check for ctrl key
                        if (userInput != CtrlZ && userInput != CtrlY)
                        {
                            notValidEntry = true;
                            do
                            {
                                if (redoLog.Count == 0)
                                {
                                    Console.WriteLine("Enter 'to' tower number 1,2 or 3, Ctr Z to undo");
                                }
                                else
                                {
                                    Console.WriteLine("Enter 'to' tower number 1,2 or 3, Ctrl Z to undo or Ctrl Y to redo");
                                }
                                userInput = Console.ReadLine().ToUpper();

                                if (userInput == "X")
                                {
                                    playGame = false;
                                    break;
                                }

                                else
                                {
                                    if (userInput == CtrlZ || userInput == CtrlY)
                                    {
                                        //Console.WriteLine("UNDO / REDO MOVE Requested");
                                        notValidEntry = false;
                                    }
                                    else
                                    {
                                        destinationTower = Convert.ToInt32(userInput);
                                        if (destinationTower < 1 || destinationTower > 3)
                                        {
                                            Console.WriteLine("Invalid Tower Number");
                                        }
                                        else
                                        {
                                            notValidEntry = false;
                                        }
                                    }

                                }


                            } while (notValidEntry);

                        }


                        // Switch Case check to to see if Undo or Redo Request was entered else default regular move
                        switch (userInput.ToUpper())
                        {
                            case CtrlZ:
                                Console.WriteLine($"undoLog BEFORE POP Count = {undoLog.Count}");
                                // Pop undoLog
                                tempMoveRecord = undoLog.Pop();
                                sourceTower = tempMoveRecord.From;
                                destinationTower = tempMoveRecord.To;
                                // Move record redoLog
                                redoLog.Push(tempMoveRecord);

                                Console.WriteLine($"source = {sourceTower}, destination = {destinationTower}");
                                Console.WriteLine($"undoLog After POP Count = {undoLog.Count}");
                                // Call the Move but reverse values of sourceTower and destinationTower
                                tempMoveRecord = poles.Move(destinationTower, sourceTower);

                                break;
                            case CtrlY:
                                Console.WriteLine($"redoLog BEFORE POP Count = {redoLog.Count}");
                                // Pop redoLog
                                tempMoveRecord = redoLog.Pop();
                                sourceTower = tempMoveRecord.From;
                                destinationTower = tempMoveRecord.To;
                                // Move record to undoLog
                                undoLog.Push(tempMoveRecord);
                                Console.WriteLine($"source = {sourceTower}, destination = {destinationTower}");
                                Console.WriteLine($"redoLog AFTER POP Count = {redoLog.Count}");

                                // Call the Move
                                tempMoveRecord = poles.Move(sourceTower, destinationTower);

                                break;
                            default:
                                // Set temp Move Record to the Move method
                                tempMoveRecord = poles.Move(sourceTower, destinationTower);
                                // Record Move into undoLog
                                Console.WriteLine($"source = {sourceTower}, destination = {destinationTower}");
                                undoLog.Push(tempMoveRecord);
                                // Clear redoLog Records
                                redoLog.Clear();

                                break;
                        }

                        // Record Move Record into log
                        moveLog.Enqueue(tempMoveRecord);

                        // Display Code
                        TowerUtilities.DisplayTowers(poles);

                        Console.WriteLine($"Move {poles.NumberOfMoves} complete. Sucessfully moved tower {sourceTower} to tower {destinationTower}.");

                        // Display stats
                        Console.WriteLine($"Number of moves is {poles.NumberOfMoves}");

                        // Determine if they won

                        if (poles.IsComplete == true)
                        {
                            if (poles.MinimumPossibleMoves == poles.NumberOfMoves)
                            {
                                Console.WriteLine($"Your number of moves is minimum number of move of {poles.MinimumPossibleMoves}.");
                            }

                            playGame = false;
                        }


                    } while (playGame);

                }

                // Ask if they want to see number of moves
                Console.WriteLine();
                Console.WriteLine("Do you want to see the number of moves? 'Y' for yes.");
                userInput = Console.ReadLine().ToUpper();
                if (userInput == "Y")
                {
                    // Loop thru queue to and display all items
                    foreach (var moveRecord in moveLog)
                    {
                        Console.WriteLine($"{moveRecord.MoveNumber}. Disc {moveRecord.Disc} moved from tower {moveRecord.From} to tower {moveRecord.To}.");
                    }
                    // Loop thru queue to remove

                    while (moveLog.Count != 0)
                    {
                        //Console.WriteLine($"{moveRecord.MoveNumber}. Disc {moveRecord.Disc} moved from tower {moveRecord.From} to tower {moveRecord.To}.");
                        moveLog.Dequeue();
                    }
                }
                

                // Ask if they want to play again
                Console.WriteLine();
                Console.WriteLine("Do you want to play again? 'Y' for yes.");
                userInput = Console.ReadLine().ToUpper();

                if (userInput == "Y")
                {
                    //Clear all undoLog and redoLog and moveLog
                    undoLog.Clear();
                    redoLog.Clear();
                    moveLog.Clear();

                    playGame = true;
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Nice day!");
                    break;
                }
            }
        }
        public static int RecursiveAutoSolve(int diskNumber, int pTower1, int pTower3, int pTower2, Queue<MoveRecord> pQueue, int pNumberOfMoves)
        {
            Queue<MoveRecord> tempMoveQueue = pQueue;
            MoveRecord tempMoveRecord;
            int tempMoves = pNumberOfMoves;

            if (diskNumber == 1)
            {
                tempMoveRecord = new MoveRecord(tempMoves, diskNumber, pTower1, pTower3);
                tempMoveQueue.Enqueue(tempMoveRecord);

                tempMoves++;
                return tempMoves;
            }

            tempMoves = RecursiveAutoSolve(diskNumber - 1, pTower1, pTower2, pTower3, tempMoveQueue, tempMoves);

            tempMoveRecord = new MoveRecord(tempMoves, diskNumber, pTower1, pTower3);
            tempMoveQueue.Enqueue(tempMoveRecord);
            tempMoves++;
            return RecursiveAutoSolve(diskNumber - 1, pTower2, pTower3, pTower1, tempMoveQueue, tempMoves);

        }


        public static void IterativeAutoSolve(Towers poles, int discs, Queue<MoveRecord> moveLog, int pTower1, int pTower2, int pTower3)
        {
            MoveRecord tempMoveRecord;
            string userInput;
            bool done = false;

            if (discs % 2 == 0)
            {
                int temp = pTower3;
                pTower3 = pTower2;
                pTower2 = temp;
            }

            
            do
            {
                for (int i = 1; i <= poles.MinimumPossibleMoves; i++)
                {
                    if (i % 3 == 1)
                    {
                        tempMoveRecord = MoveBetweenPoles(poles, pTower1, pTower3);
                        moveLog.Enqueue(tempMoveRecord);
                        TowerUtilities.DisplayTowers(poles);
                        Console.WriteLine($"\nMove {poles.NumberOfMoves}. Successfully moved disc from pole {pTower1} to pole {pTower3}.");
                    }
                    else if (i % 3 == 2)
                    {
                        tempMoveRecord = MoveBetweenPoles(poles, pTower1, pTower2);
                        moveLog.Enqueue(tempMoveRecord);
                        TowerUtilities.DisplayTowers(poles);
                        Console.WriteLine($"\nMove {poles.NumberOfMoves}. Successfully moved disc from pole {pTower1} to pole {pTower3}.");

                    }
                    else if (i % 3 == 0)
                    {
                        tempMoveRecord = MoveBetweenPoles(poles, pTower2, pTower3);
                        moveLog.Enqueue(tempMoveRecord);
                        TowerUtilities.DisplayTowers(poles);
                        Console.WriteLine($"\nMove {poles.NumberOfMoves}. Successfully moved disc from pole {pTower1} to pole {pTower3}.");

                    }



                    Console.WriteLine("\nPress Enter to move to the next step or \"X\" to quit.");
                    userInput = Console.ReadKey().Key.ToString().ToUpper(); 


                    if (userInput.ToUpper() == "X")
                    {
                        done = true;
                        break;
                    }
                }
                done = true;



            } while (!done);
        }



        public static MoveRecord MoveBetweenPoles(Towers poles, int from, int to)
        {
            int[][] arrayOfTowers = poles.ToArray();
            MoveRecord tempMoveRecord;

            if (arrayOfTowers[from - 1].Length == 0)
            {
                tempMoveRecord = poles.Move(to, from);
            }
            else if (arrayOfTowers[to - 1].Length == 0)
            {
                tempMoveRecord = poles.Move(from, to);
            }
            else if (arrayOfTowers[from - 1][0] > arrayOfTowers[to - 1][0])
            {
                tempMoveRecord = poles.Move(to, from);
            }
            else
            {
                tempMoveRecord = poles.Move(from, to);
            }

            return tempMoveRecord;
        }
    }
}

