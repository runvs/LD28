﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamTemplate
{
    public class BattleManager
    {
        static public void DoBattleAction(Actor actor1, Actor actor2, BattleAction action)
        {
            if (actor1 != null && actor2 != null)
            {
                // actor 1 hits actor 2
                if (action == BattleAction.Attack)
                {
                    DoAttackCalculation(actor1, actor2);
                }
                if (action == BattleAction.Block)
                {

                }


            }
        }

        private static void DoAttackCalculation(Actor actor1, Actor actor2)
        {
            System.Console.Out.WriteLine("Doing an Attack");
            // check if the hit is successfull
            if (!DoesEvade(actor1, actor2))
            {
                System.Console.Out.WriteLine("Hit");
                int baseDamage = actor1.GetBaseDamage() + RollTheDie.AttributeProbe(actor1.ActorAttributes.Strength);
                System.Console.Out.WriteLine("Base Damage " + baseDamage);

                int reducedDamage = CalculateDamageWithArmor(baseDamage, 99);
                actor2.TakeDamage(reducedDamage);
                System.Console.Out.WriteLine("Reduced Damage " + baseDamage);
            }
            else
            {
                System.Console.Out.WriteLine("Evade");
            }
        }

        static private bool DoesEvade(Actor actor1, Actor actor2)
        {

            int AttackerHits = RollTheDie.AttributeProbe(actor1.ActorAttributes.Agility);
            int DefenderHits = RollTheDie.AttributeProbe(actor2.ActorAttributes.Agility);
            if (actor2.IsBlocking)
            {
                DefenderHits += GameProperties.BlockEvadeBonus;
            }
            if (AttackerHits >= DefenderHits - GameProperties.AttackerEvadeBonus)
            {
                return false;
            }
            return true;

        }

        static private int CalculateDamageWithArmor(int baseDamage, int Strength)
        {
            int modifier = RollTheDie.AttributeProbe(Strength);
            int finalDamage = baseDamage - modifier;
            if (finalDamage <= 0)
            {
                finalDamage = 1;
            }
            return finalDamage;
        }



    }

    public enum BattleAction
    {
        Attack,
        Magic,
        Block
    }
}