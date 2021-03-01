using System;
using System.Collections.Generic;
using System.Text;


namespace TowerOfHanoi
{

    public class Towers
    {
        public int NumberOfDiscs { get; set; }
        public int NumberOfMoves { get; set; }

        public bool IsComplete;

        public int MinimumPossibleMoves;

        public Stack<int>[] towerArray = new Stack<int>[3];


        public Towers(int numberOfDiscs)
        {
            if (numberOfDiscs >= 1 && numberOfDiscs <= 9)
            {
                NumberOfDiscs = numberOfDiscs;
                MinimumPossibleMoves = (int)(Math.Pow(2, NumberOfDiscs) - 1);

                for (int i = 0; i < 3; i++)
                {
                    Stack<int> poles = new Stack<int>();
                    towerArray[i] = poles;
                }
                for (int i = numberOfDiscs; i > 0; i--)
                {
                    towerArray[0].Push(i);
                }
            }
            else throw new ApplicationException("Invalid Height");

            IsComplete = false;
        }

        public MoveRecord Move(int from, int to)
        {
            NumberOfMoves++;
            // index var with correct numbers
            int fromIndex = from - 1;
            int toIndex = to - 1;

            if (from < 1 || from > 3 || to < 1 || to > 3)
            {
                throw new ApplicationException("Invalid Tower Number");
            }

            else if (from == to)
            {
                throw new ApplicationException("Move Cancelled. This Is The Same Tower");
            }

            else if (towerArray[fromIndex].Count == 0)
            {
                throw new ApplicationException($"Tower {from} Is Empty");
            }

            else if (towerArray[toIndex].Count != 0)
            {
                if (Convert.ToInt32(towerArray[fromIndex].Peek()) > Convert.ToInt32(towerArray[toIndex].Peek()))
                {
                    throw new ApplicationException($"Top disc of tower {from} is larger than disc of tower {to}.");
                }
            }

            int diskNumber = towerArray[fromIndex].Pop();
            towerArray[toIndex].Push(diskNumber);
            MoveRecord tempRecord = new MoveRecord(NumberOfMoves, diskNumber, from, to);

            if (towerArray[2].Count == NumberOfDiscs)
            {
                IsComplete = true;
            }
            else
            {
                IsComplete = false;
            }
            return tempRecord;
        }

        public int[][] ToArray()
        {
            int[][] polesJagged = new int[3][];

            for (int i = 0; i < 3; i++)
            {
                int[] newArray = towerArray[i].ToArray();
                polesJagged[i] = new int[towerArray[i].Count];

                for (int j = 0; j < towerArray[i].Count; j++)
                {
                    polesJagged[i][j] = Convert.ToInt32(newArray[j]);
                }
            }
            return polesJagged;
        }
    }
}
