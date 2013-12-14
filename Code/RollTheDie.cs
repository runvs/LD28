﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamTemplate
{
    class RollTheDie
    {

        static Random _randomGenerator = new Random();

        static public int RollTheDie()
        {
            return _randomGenerator.Next(1, 7);
        }

        static public bool IsHit(int border)
        {
            if (RollTheDie() >= border)
            {
                return true;
            }
            return false;
        }

        static public int AttributeProbe(int attribute)
        {
            int roll = RollTheDie();
            if (roll > attribute)
            {
                return 0;
            }
            else
            {
                return (attribute - roll + 1);
            }
        }

    }
}