using System;

//Легенда:
//Вы - ментор и у вас есть несколько умений, которые вы можете использовать против ученика.
//Вы должны сломить волю ученика и только после этого будет вам покой. 
//Формально:
//Перед вами ученик, у которого есть определенное количество жизней и атака.
//Атака может быть как всегда одной и той же, так и определяться рандомом в начале раунда.
//У ученика обычная атака. Ученик должен иметь возможность убить ментора.
//У ментора есть 4 умения
//1. Найти запрещенный комментарий в коде ученика (все комментарии являются запрещенными)
//2. Найти ненужную пустую строку на {random.Next(1001) строчке кода, который тратит энергию
//3. Найти идущую подряд вторую пустую строку. Можно вызывать, только если был использован (2) найти ненужную пустую строку.
//Для повторного применения надо повторно использовать огненный шар.
//4. Притвориться другим ментором и найти совсем другие ошибки. Восстанавливает здоровье и ману, но не больше их максимального значения.
//(4) Можно использовать ограниченное число раз, количество ментором все же ограничено. Также понижает мораль ученика
//Если пользователь ошибся с вводом команды или не выполнилось условие, то ментор пропускает ход и происходит атака ученика
//Программа завершается только после слома воли у ученика или окончании рабочего дня ментора,
//а если у вас возможно одновременно убить друг друга, то надо сообщить о ничье. 

namespace millionDollarsCourses
{
    internal class Program
    {
        static void Main()
        {
            const string CommandAbilityComment = "1";
            const string AbilityComment = "Ни слова по-русски";
            const string CommandAbilityEmptyString = "2";
            const string AbilityEmptyString = "Безупречный куплет";
            const string CommandAbilitySecondEmptyString = "3";
            const string AbilitySecondEmptyString = "Залипший энтер";
            const string CommandAbilitySummon = "4";
            const string AbilitySummon = "Призыв коллеги";
            const string CommandSecretUltimateAbility = "5";
            const string AbilitySecretUltimate = "Новая надежда";

            #region student parameters
            Random random = new Random();
            int studentWillPower = 100;
            int studentTimeWaste = 1;
            int codeLength = 1001;
            int studentKnowledge = -1;

            int typeOfWaistingTime = 0;
            string unneededCreativity = $"Ученик использует ненужную креативность и тратит {studentTimeWaste} час вашего времени";
            string unnecessaryComplication = $"Ученик усложняет все до предела и тратит {studentTimeWaste} час вашего времени";
            string wrongVariablesNaming = $"Ученик называет bool переменную ZdesVihodIzZikla и тратит {studentTimeWaste} час вашего времени";
            string wrongFormatting = $"Ученик объявляет все переменные в одну строчку и тратит {studentTimeWaste} час вашего времени";
            string battleWithMentor = $"Ученик делает битву ментора с учеником и тратит {studentTimeWaste} час вашего времени...\n" +
                $"Он просит у вас прощения, но за подобные грехи не прощают. Ему по-прежнему жаль";
            #endregion

            #region mentor parameters
            int mentorMaxWorkHours = 4;
            int mentorMaxEnergy = 8;
            int mentorWorkHours = mentorMaxWorkHours;
            int mentorEnergy = mentorMaxEnergy;

            int abilityCommentMentalDamage = 12;

            int abilityEmptyStringMentalDamage = 8;
            int abilityEmptyStringEnergyCost = 5;
            bool isAbilityEmptyStringActive = false;

            int abilitySecondEmptyStringMentalDamage = 18;

            int abilitySummonMentalDamage = 36;
            int abilitySummonHoursRestoration = mentorMaxWorkHours;
            int abilitySummonEnergyRestoration = 3;
            int abilitySummonUses = 1;

            int AbilitySecretUltimateAbilityMentalDamage = 1000000;
            bool isAllHopeLost;
            bool isHopeRestored = false;
            #endregion

            #region game info 
            string legend = "Вы - ментор и у вас есть несколько умений, которые вы можете использовать против ученика.\n" +
                "Вы должны сломить волю ученика в течении рабочего дня и только после этого будет вам покой\n";

            string displayAbilityCommentDescription = $"\n{CommandAbilityComment} - {AbilityComment} \n" +
                $"Найти запрещенный комментарий в коде ученика (все комментарии являются запрещенными).\n\n";
            string displayAbilityEmptyStringDescription = $"{CommandAbilityEmptyString} - {AbilityEmptyString} \n" +
                $"Разыскать ненужную пустую строку. Тратит: {abilityEmptyStringEnergyCost} энергии.\n\n";
            string displayAbilitySecondEmptyStringDescription = $"{CommandAbilitySecondEmptyString} - {AbilitySecondEmptyString} \n" +
               $"Обнаружить идущую подряд вторую пустую строку. Можно вызывать, только если был использован [{CommandAbilityEmptyString} {AbilityEmptyString}]\n\n";
            string displayAbilitySummonDescription = $"{CommandAbilitySummon} - {AbilitySummon}\n" +
               $"Призвать другого ментора, чтобы он нашел совсем другие ошибки.\n" +
               $"Восстанавливает {abilitySummonHoursRestoration} рабочих часов и {abilitySummonEnergyRestoration} энергии. " +
               $"Можно использовать всего {abilitySummonUses} раз (ибо количество менторов ограничего).\n";
            string displayAbilitySecretUltimateDescription = $"\n{CommandSecretUltimateAbility} - {AbilitySecretUltimate}\n" +
                $"Использовать только в случае крайней необходимости. Последствия могут быть... непредвиденными\n";

            string allDefaultMentorAbilitiesDescriptions = displayAbilityCommentDescription + displayAbilityEmptyStringDescription +
                displayAbilitySecondEmptyStringDescription + displayAbilitySummonDescription;
            #endregion

            string userInput;

            Console.WriteLine(legend);
            Console.WriteLine(new string('_', 100));

            while (studentWillPower > 0 && mentorWorkHours > 0)
            {
                #region display game info
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\nВаши характеристики:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{mentorWorkHours} рабочих часов");

                if (isAbilityEmptyStringActive)
                    Console.Write($" | вы только что нашли пустую строку, но что если там есть вторая...");

                Console.ResetColor();
                Console.WriteLine($" | {mentorEnergy} энергии\n");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\nХарактеристики ученика:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{studentWillPower} силы воли");
                Console.ResetColor();
                Console.WriteLine($" | Тратит {studentTimeWaste} ваших рабочих часов в ход\n");
                Console.WriteLine(new string('_', 100));
                Console.Write(allDefaultMentorAbilitiesDescriptions);
                isAllHopeLost = abilitySummonUses == 0 && mentorWorkHours <= studentTimeWaste && mentorEnergy <= abilityEmptyStringEnergyCost;

                if (isAllHopeLost)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(displayAbilitySecretUltimateDescription);
                    Console.ResetColor();
                }
                #endregion

                #region AttackStudent
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\nВыберите умение: ");
                userInput = Console.ReadLine();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Ваш ход:");
                Console.ResetColor();

                switch (userInput)
                {
                    case CommandAbilityComment:
                        //здесь нет комментария
                        studentWillPower -= abilityCommentMentalDamage;
                        studentKnowledge++;
                        Console.WriteLine($"Вы используете {AbilityComment}. Сила воли ученика сломлена на {abilityCommentMentalDamage}");
                        break;

                    case CommandAbilityEmptyString:

                        if (mentorEnergy >= abilityEmptyStringEnergyCost)
                        {
                            isAbilityEmptyStringActive = true;
                            studentWillPower -= abilityEmptyStringMentalDamage;
                            mentorEnergy -= abilityEmptyStringEnergyCost;
                            studentKnowledge++;
                            Console.WriteLine($"Вы используете {AbilityEmptyString}.\n" +
                                $"Вы разыскиваете ненужную пустую строку на {random.Next(codeLength)} строчке кода\n" +
                                $"Сила воли ученика сломлена на {abilityEmptyStringMentalDamage}");
                        }
                        else
                        {
                            Console.WriteLine($"Вы пытаетесь разыскать ненужную пустую строку, но все как будто бы в тумане.\n" +
                                $"У вас не получается сосредоточиться, но у вас получается не упасть в обморок");
                        }
                        break;

                    case CommandAbilitySecondEmptyString:
                        if (isAbilityEmptyStringActive)
                        {
                            isAbilityEmptyStringActive = false;
                            studentWillPower -= abilitySecondEmptyStringMentalDamage;
                            studentKnowledge++;
                            Console.WriteLine($"Вы используете {AbilitySecondEmptyString}.\n" +
                                $"Ученик получает критический урон и старается удержаться на кресле. У него это не удается\n" +
                                $"Сила воли ученика сломлена на {abilitySecondEmptyStringMentalDamage}");
                        }
                        else
                        {
                            Console.WriteLine($"\nВы ищете вторую пустую строку, но не можете найти даже первую");
                        }
                        break;


                    case CommandAbilitySummon:
                        if (abilitySummonUses > 0)
                        {
                            abilitySummonUses--;
                            studentWillPower -= abilitySummonMentalDamage;
                            mentorWorkHours += abilitySummonHoursRestoration;
                            mentorEnergy += abilitySummonEnergyRestoration;

                            if (mentorWorkHours > mentorMaxWorkHours)
                                mentorWorkHours = mentorMaxWorkHours;

                            if (mentorEnergy > mentorMaxEnergy)
                                mentorEnergy = mentorMaxEnergy;

                            studentKnowledge++;
                            Console.WriteLine($"Вы используете {AbilitySummon}.\n" +
                                $"Именно в тот момент, когда ученик думал, что задание сдано\n" +
                                $"Сила воли ученика сломлена на {abilitySummonMentalDamage}");
                        }
                        else
                        {
                            Console.WriteLine($"\nВы последний ментор. На вас вся надежда. Воля ученика дОлЖнА бЫтЬ сЛоМлЕнА");
                        }
                        break;

                    case CommandSecretUltimateAbility:
                        if (abilitySummonUses == 0 && mentorWorkHours <= 5 && mentorEnergy <= 5)
                        {
                            studentWillPower -= AbilitySecretUltimateAbilityMentalDamage;
                            int forbiddenKnowledge = 1000000;
                            studentKnowledge += forbiddenKnowledge;
                            isHopeRestored = true;
                            Console.WriteLine($"Вы вынуждены использовать {AbilitySecretUltimate}.\n" +
                                $"Только он сможет восстановить баланс силю. Вы читаете том запретных заклинаний и призываете Рому Сакутина.\n" +
                                $"Количество ошибок в коде ученика {AbilitySecretUltimateAbilityMentalDamage}. Сила воли ученика сломлена безвозвратно.");
                        }
                        else
                        {
                            Console.WriteLine($"\nРома занят");
                        }
                        break;

                    default:
                        Console.WriteLine("Вы задумались всего на секунду, а ученик уже сотворил какую-то глупость\n" +
                            "В недоумении вы пропускаете ход");
                        break;
                }
                #endregion

                #region AttackMentor
                if (isHopeRestored == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nХод ученика:");
                    Console.ResetColor();
                    mentorWorkHours -= studentTimeWaste;

                    switch (typeOfWaistingTime)
                    {
                        case 0:
                            Console.WriteLine(unneededCreativity);
                            break;
                        case 1:
                            Console.WriteLine(unnecessaryComplication);
                            break;
                        case 2:
                            Console.WriteLine(wrongVariablesNaming);
                            break;
                        case 3:
                            Console.WriteLine(wrongFormatting);
                            break;
                        case 4:
                            Console.WriteLine(battleWithMentor);
                            break;
                    }

                    typeOfWaistingTime++;

                    if (typeOfWaistingTime > 4)
                        typeOfWaistingTime = 0;
                }
                #endregion

                #region display game info at round end
                Console.WriteLine();

                if (isHopeRestored == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"\nВаши характеристики:");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{mentorWorkHours} рабочих часов");

                    if (isAbilityEmptyStringActive)
                        Console.Write($" | вы только что нашли пустую строку, но что если там есть вторая...");

                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"\n\nХарактеристики ученика:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{studentWillPower} силы воли");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\nНажмите что угодно чтобы продолжить");
                Console.ResetColor();
                Console.ReadKey(true);
                Console.Clear();
                #endregion
            }

            #region CheckWinner
            int geniousStudent = 1000;

            if (studentKnowledge >= geniousStudent)
                Console.WriteLine("Секретная концовка\n" +
                    "Ученик познает тайны вселенной, но вынужден провести остаток дней в психиатрической больнице. Оно того стоило.\n" +
                    $"Знания ученика: свыше {studentKnowledge - 1}");
            else if (studentWillPower <= 0 && mentorWorkHours <= 0)
                Console.WriteLine($"Ультимативная победа! Знания ученика преумножены, а ментор идет отдыхать.\n" +
                    $"Знания ученика: {studentKnowledge}");
            else if (studentWillPower > 0)
                Console.WriteLine($"Победа ученика? Рассудок ученика цел, но стал ли он сегодня умнее?\n" +
                    $"Знания ученика всего: {studentKnowledge}");
            else if (mentorWorkHours > 0)
                Console.WriteLine("Победа ментора! Знания ученика преумножены, но какой ценой?\n" +
                    $"Знания ученика: {studentKnowledge}");
            #endregion
        }
    }
}
