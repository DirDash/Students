﻿using System;
using System.Drawing;
using System.Linq;
using CellsAutomate.Constants;
using CellsAutomate.Creatures;
using CellsAutomate.Food;
using CellsAutomate.Tools;

namespace CellsAutomate
{
    public class Membrane
    {
        private readonly Random _random;
        private int _parentMark;
        public Point Position { get; set; }
        public int Generation { get; }
        private int _energyPoints = CreatureConstants.StartEnergyPoints;
        public BaseCreature Creature { get; }
        private readonly Creator _creator;

        public Membrane(BaseCreature creature, Random random, Point position, int generation, int parentMark, Creator creator)
        {
            Creature = creature;
            _random = random;
            _parentMark = parentMark;
            Position = position;
            Generation = generation;
            _creator = creator;
        }

        public Tuple<ActionEnum, DirectionEnum> Turn(FoodMatrix eatMatrix, 
            Membrane[,] creatures)
        {
            if (HasToDie()) return Tuple.Create(ActionEnum.Die, DirectionEnum.Stay);

            _energyPoints -= CreatureConstants.MinFoodToSurvive;

            var result = Creature.MyTurn(eatMatrix, creatures, Position, _random, HasOneBite(eatMatrix), _energyPoints);

            return result.Item1 == ActionEnum.MakeChild ? Tuple.Create(ActionEnum.MakeChild, GetDirectionForChild(creatures)) : result;
        }

        private bool HasOneBite(FoodMatrix eatMatrix)
        {
            return eatMatrix.HasOneBite(Position);
        }

        private bool HasToDie()
        {
            return _energyPoints < CreatureConstants.MinFoodToSurvive;
        }

        private DirectionEnum GetDirectionForChild(Membrane[,] creatures)
        {
            var points = CommonMethods.GetPoints(Position);
            var directions = (from item in points where CommonMethods.IsValidAndFree(item, creatures)
                              select DirectionEx.DirectionByPoints(Position, item)).ToList();
            return directions.Count == 0 ? DirectionEnum.Stay : directions.ElementAt(_random.Next(directions.Count));
        }

        public void Eat(FoodMatrix eatMatrix)
        {
            if (eatMatrix.TakeFood(Position))
            {
                _energyPoints += CreatureConstants.OneBite;
            }
        }

        public Membrane MakeChild(Point childPosition)
        {
            _energyPoints -= CreatureConstants.ChildPrice;
            var child = _creator.MakeChild(Creature);
            return new Membrane(child, _random, childPosition, Generation + 1, _parentMark, _creator);
        }
        
        public void Move(Membrane[,] creatures, Point newPosition)
        {
            creatures[Position.X, Position.Y] = null;
            Position = newPosition;
            creatures[Position.X, Position.Y] = this;
        }

        public int ParentMark
        {
            get { return _parentMark; }
            set { _parentMark = value; }
        }
    }
}
