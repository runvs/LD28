/// This Program is provided as is with absolutely no warranty.
/// This File is published under the LGPL 3. See lgpl.txt
/// Published by Julian Dinges and Simon Weis, 2013
/// Contact laguna_1989@gmx.net

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamTemplate
{
    class RollTheDie
    {

        static Random _randomGenerator = new Random();

        static public int Roll()
        {
            return _randomGenerator.Next(1, 7);
        }

        static public bool IsHit(int border)
        {
            if (Roll() >= border)
            {
                return true;
            }
            return false;
        }

        static public int AttributeProbe(int attribute)
        {
            int roll = Roll();
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
