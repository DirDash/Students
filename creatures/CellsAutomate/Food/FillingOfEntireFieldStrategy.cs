﻿using System.Drawing;
using CellsAutomate.Constants;

namespace CellsAutomate.Food
{
    public class FillingOfEntireFieldStrategy : IFoodDistributionStrategy
    {
        private int _counterOfTurns;
        private readonly int _frequency;

        public FillingOfEntireFieldStrategy(int frequency)
        {
            _counterOfTurns = -1;
            _frequency = frequency;
        }

        public void Build(bool[,] creatures, FoodMatrix eatMatrix)
        {
            _counterOfTurns++;

            if(_counterOfTurns % _frequency != 0)
                return;

            for (int i = 0; i < eatMatrix.Length; i++)
            {
                for (int j = 0; j < eatMatrix.Height; j++)
                {
                    if (!creatures[i, j]
                        && !eatMatrix.HasMaxFoodLevel(new Point(i, j)))
                    {
                        eatMatrix.AddFood(new Point(i, j));
                    }
                }
            }
        }
    }
}
