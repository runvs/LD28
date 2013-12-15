using System;
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
                else if (action == BattleAction.Block)
                {

                }
                else if (action == BattleAction.Magic)
                {
                    DoMagicCalculation(actor1, actor2);
                }

            }
        }

        private static void DoMagicCalculation(Actor actor1, Actor actor2)
        {
            if (!DoesMagicEvade(actor1, actor2))
            {
                int baseDamage = actor1.GetMagicBaseDamage() + RollTheDie.AttributeProbe(actor1.ActorAttributes.Intelligence);

                // magic damage is reduced by intelligence
                int reducedDamage = CalculateDamageWithArmor(baseDamage, actor2.ActorAttributes.Intelligence);
                actor2.TakeDamage(reducedDamage);
            }
        }



        private static void DoAttackCalculation(Actor actor1, Actor actor2)
        {
            // check if the hit is successfull
            if (!DoesEvade(actor1, actor2))
            {
                int baseDamage = actor1.GetBaseDamage() + RollTheDie.AttributeProbe(actor1.ActorAttributes.Strength);

                // physical damage is reduced by Endurance
                int reducedDamage = CalculateDamageWithArmor(baseDamage, actor2.ActorAttributes.Endurance);
                actor2.TakeDamage(reducedDamage);
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


        private static bool DoesMagicEvade(Actor actor1, Actor actor2)
        {
            int AttackerHits = RollTheDie.AttributeProbe(actor1.ActorAttributes.Intelligence);
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
            System.Console.Out.WriteLine("reduction " + modifier);
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
