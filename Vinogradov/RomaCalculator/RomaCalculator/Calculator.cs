﻿using System;
using System.Collections.Generic;
using System.Text;
using RomaCalculator.KindsOfOperators;

namespace RomaCalculator
{
    public class Calculator
    {
        private Dictionary<string, IOperator> operators;

        public Calculator()
        {
            operators = new Dictionary<string, IOperator>();
            operators["+"] = new Addition();
            operators["-"] = new Subtraction();
            operators["*"] = new Multiplication();
        }

        public string Calculate(string operandA, string operandB, IOperator operatorForAAndB)
        {

            var A = ConvertFromRomanToArabNumber(operandA);
            var B = ConvertFromRomanToArabNumber(operandB);

            var answerForExpression = operatorForAAndB.CalculateIt(A, B);

            return ConvertFromArabToRomanNumber(answerForExpression);
        }

        public string Calculate(string expression)
        {
            var separetedExpression = expression.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);

            return Calculate(separetedExpression[0], separetedExpression[2], operators[separetedExpression[1]]);
        }

        //Create Dictionary
        public int ConvertFromRomanToArabDigit(char romanDigit)
        {
            switch (romanDigit)
            {
                case 'I':
                    return 1;
                case 'V':
                    return 5;
                case 'X':
                    return 10;
                case 'L':
                    return 50;
                case 'C':
                    return 100;
                case 'D':
                    return 500;
                case 'M':
                    return 1000;
                default:
                    return int.MaxValue;
            }
        }

        public int ConvertFromRomanToArabNumber(string romanNumber)
        {
            int result = 0;
            int previousCount = 0;
            var abacus = new List<int>();

            for (int i = 0; i < romanNumber.Length; i++)
            {
                abacus.Add(ConvertFromRomanToArabDigit(romanNumber[i]));
            }

            while (abacus.Count != 0)
            {
                previousCount = abacus.Count;

                SubtractThatPossible(abacus);

                AddUpAllIfPossible(abacus, previousCount, ref result);
            }
            return result;
        }

        private void AddUpAllIfPossible(List<int> abacus, int previousCount, ref int result)
        {
            if (abacus.Count == previousCount)
            {
                foreach (var item in abacus)
                {
                    result += item;
                }
                abacus.Clear();
            }
        }

        private void SubtractThatPossible(List<int> abacus)
        {
            for (int i = 0; i < abacus.Count - 1; i++)
            {
                if (abacus[i] < abacus[i + 1])
                {
                    abacus[i] = abacus[i + 1] - abacus[i];
                    abacus.RemoveAt(i + 1);
                    i = abacus.Count;
                }
            }
        }

        public string ConvertFromArabToRomanNumber(int arabNumber)
        {
            var result = new StringBuilder();
            if (arabNumber == 0)
            {
                result.Append("zero");
            }
            else
            {
                if (arabNumber < 0)
                {
                    result.Append("-");
                    arabNumber = -arabNumber;
                }
                MakeRomanNumber(arabNumber, result);
            }
            return result.ToString();
        }

        //return StringBuilder result
        private void MakeRomanNumber(int arabNumber, StringBuilder result)
        {
            var arab = new int[] {1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 1000};
            var roman = new string[] {"I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD", "D", "CM", "M"};
            var index = 0;
            while (arabNumber != 0)
            {
                index = 0;

                FindNecessaryNumber(arabNumber, ref index, arab);

                arabNumber -= arab[index - 1];
                result.Append(roman[index - 1]);
            }
        }

        private void FindNecessaryNumber(int arabNumber, ref int index, int[] arab)
        {
            while ((index != 13) && (arabNumber >= arab[index]))
            {
                index++;
            }
        }
    }
}
