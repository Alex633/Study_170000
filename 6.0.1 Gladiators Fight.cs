using System;

namespace CsRealLearning
{
    internal class Program
    {
        static void Main()
        {
            Berserk berserk = new Berserk(100, 0, 30, 2);
            Knight knight = new Knight(100, 5, 30, 1);
            int count = 0;

            Console.WriteLine("\nFIGHT\n");
            while (berserk.HP >= 0 && knight.HP >= 0)
            {
                Console.WriteLine("\nLoop:" + ++count + "\n");

                ////
                knight.Attack(berserk);

                if (IsItOver(knight, berserk))
                    break;

                berserk.BattleCry();

                if (IsItOver(knight, berserk))
                    break;

                knight.Attack(berserk);

                if (IsItOver(knight, berserk))
                    break;

                berserk.Attack(knight);

                if (IsItOver(knight, berserk))
                    break;

                knight.DefenceStance();
                berserk.Attack(knight);
                ////
            }

            if (berserk.HP <= 0)
            {
                Console.WriteLine("\nKnight is victorious");
            }
            else if (knight.HP <= 0)
            {
                Console.WriteLine("\nBerserk is victorious");
            }
        }
        static bool IsItOver(Warrior warrior1, Warrior warrior2)
        {
            return warrior1.HP <= 0 || warrior2.HP <= 0;
        }


        class Warrior
        {
            protected int _hp;
            protected int _armor;
            protected int _damage;
            protected int _attackSpeed;


            public int HP => _hp;

            public Warrior(int hp, int armor, int damage, int attackSpeed)
            {
                _hp = hp;
                _armor = armor;
                _damage = damage;
                _attackSpeed = attackSpeed;
            }

            public void DisplayHP()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{GetType().Name} HP: {_hp}");
                Console.ResetColor();
            }

            public void Attack(Warrior target)
            {
                int realDamage = Math.Max(_damage - target._armor, 0);

                if (target._hp > realDamage)
                {
                    target._hp -= realDamage;
                }
                else
                {
                    target._hp = 0;
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{GetType().Name} attacks {target.GetType().Name} for {realDamage}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{target.GetType().Name} HP: {target._hp}\n");
                Console.ResetColor();
            }

            public void TakeDamage(int damage, Warrior target)
            {
                int realDamage = Math.Max(damage - _armor, 0);

                if (_hp > realDamage)
                {
                    _hp -= realDamage;
                    DisplayHP();
                }
                else
                {
                    _hp = 0;
                    DisplayHP();
                }
            }
        }

        class Knight : Warrior
        {
            public Knight(int hp, int armor, int damage, int attackSpeed) : base(hp, armor, damage, attackSpeed)
            {
            }

            public void DefenceStance()
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                _armor += 20;
                Console.WriteLine($"{GetType().Name} uses Defence Stance. His armor increased to: {_armor} armor\n");
                Console.ResetColor();
            }
        }

        class Berserk : Warrior
        {
            public Berserk(int hp, int armor, int damage, int attackSpeed) : base(hp, armor, damage, attackSpeed)
            {
            }

            public void BattleCry()
            {
                _attackSpeed = 2;
                _damage = (int)(_damage * _attackSpeed);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(GetType().Name + " uses Battle Cry. His attack speed increased: " + _attackSpeed + "x. But he also hurt his throat");
                TakeDamage(5, this);
                if (this._hp == 0)
                {
                    Console.WriteLine($"{GetType().Name} killed himself with that scream. Really?");
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }
    }
}
