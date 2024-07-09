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
            const string CommandAbilityEmptyString = "2";
            const string CommandAbilitySecondEmptyString = "3";
            const string CommandAbilitySummon = "4";
            const string CommandSecretUltimateAbility = "5";
            const int CommandStudentUnneededCreativity = 0;
            const int CommandUnnecessaryComplication = 1;
            const int CommandWrongVariablesNaming = 2;
            const int CommandWrongFormatting = 3;
            const int CommandBattleWithMentor = 4;

            string abilityComment = "Ни слова по-русски";
            string abilityEmptyString = "Безупречный куплет";
            string abilitySecondEmptyString = "Залипший энтер";
            string abilitySecretUltimate = "Новая надежда";
            string abilitySummon = "Призыв коллеги";

            #region student parameters
            Random random = new Random();
            int studentWillPower = 100;
            int studentTimeWastePerTurn = 1;
            int codeLength = 1001;
            int studentKnowledge = -1;

            int currentTypeOfWastingTime = 0;
            int maxTypesOfWastingTime = 4;
            string unneededCreativity = $"Ученик использует ненужную креативность и тратит {studentTimeWastePerTurn} час вашего времени";
            string unnecessaryComplication = $"Ученик усложняет все до предела и тратит {studentTimeWastePerTurn} час вашего времени";
            string wrongVariablesNaming = $"Ученик называет bool переменную ZdesVihodIzCikla и тратит {studentTimeWastePerTurn} час вашего времени";
            string wrongFormatting = $"Ученик объявляет все переменные в одну строчку и тратит {studentTimeWastePerTurn} час вашего времени";
            string battleWithMentor = $"Ученик делает битву ментора с учеником и тратит {studentTimeWastePerTurn} час вашего времени...\n" +
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

            int AbilitySecretUltimateMentalDamage = 1000000;
            bool isAllHopeLost;
            bool isHopeRestored = false;
            #endregion

            #region game info 
            string legend = "Вы - ментор и у вас есть несколько умений, которые вы можете использовать против ученика.\n" +
                "Вы должны сломить волю ученика в течении рабочего дня и только после этого будет вам покой\n";

            string abilityCommentDescription = $"\n{CommandAbilityComment} - {abilityComment} \n" +
                $"Найти запрещенный комментарий в коде ученика (все комментарии являются запрещенными).\n\n";
            string abilityEmptyStringDescription = $"{CommandAbilityEmptyString} - {abilityEmptyString} \n" +
                $"Разыскать ненужную пустую строку. Тратит: {abilityEmptyStringEnergyCost} энергии.\n\n";
            string abilitySecondEmptyStringDescription = $"{CommandAbilitySecondEmptyString} - {abilitySecondEmptyString} \n" +
               $"Обнаружить идущую подряд вторую пустую строку. Можно вызывать, только если был использован {abilityEmptyString}]\n\n";
            string abilitySummonDescription = $"{CommandAbilitySummon} - {abilitySummon}\n" +
               $"Призвать другого ментора, чтобы он нашел совсем другие ошибки.\n" +
               $"Восстанавливает {abilitySummonHoursRestoration} рабочих часов и {abilitySummonEnergyRestoration} энергии. " +
               $"Можно использовать всего {abilitySummonUses} раз (ибо количество менторов ограничего).\n";
            string abilitySecretUltimateDescription = $"\n{CommandSecretUltimateAbility} - {abilitySecretUltimate}\n" +
                $"Использовать только в случае крайней необходимости. Последствия могут быть... непредвиденными\n";

            string allDefaultMentorAbilitiesDescriptions = abilityCommentDescription + abilityEmptyStringDescription +
                abilitySecondEmptyStringDescription + abilitySummonDescription;
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
                Console.WriteLine($" | Тратит {studentTimeWastePerTurn} ваших рабочих часов в ход\n");
                Console.WriteLine(new string('_', 100));
                Console.Write(allDefaultMentorAbilitiesDescriptions);
                isAllHopeLost = abilitySummonUses == 0 && mentorWorkHours <= studentTimeWastePerTurn && mentorEnergy <= abilityEmptyStringEnergyCost;

                if (isAllHopeLost)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(abilitySecretUltimateDescription);
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
                        studentWillPower -= abilityCommentMentalDamage;
                        studentKnowledge++;
                        Console.WriteLine($"Вы используете {abilityComment}. \n" +
                            $"И находите тщательно замаскированный комментарий на 131 строке.\n" +
                            $"Сила воли ученика сломлена на {abilityCommentMentalDamage}");
                        break;

                    case CommandAbilityEmptyString:
                        if (mentorEnergy >= abilityEmptyStringEnergyCost)
                        {
                            isAbilityEmptyStringActive = true;
                            studentWillPower -= abilityEmptyStringMentalDamage;
                            mentorEnergy -= abilityEmptyStringEnergyCost;
                            studentKnowledge++;
                            Console.WriteLine($"Вы используете {abilityEmptyString}.\n" +
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
                            Console.WriteLine($"Вы используете {abilitySecondEmptyString}.\n" +
                                $"Ученик получает критический урон и старается удержаться в кресле. У него это не удается\n" +
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
                            Console.WriteLine($"Вы используете {abilitySummon}.\n" +
                                $"Именно в тот момент, когда ученик думал, что задание сдано\n" +
                                $"Сила воли ученика сломлена на {abilitySummonMentalDamage}");
                        }
                        else
                        {
                            Console.WriteLine($"\nВы последний ментор. Воля ученика дОлЖнА бЫтЬ сЛоМлЕнА");
                        }
                        break;

                    case CommandSecretUltimateAbility:
                        if (isAllHopeLost)
                        {
                            studentWillPower -= AbilitySecretUltimateMentalDamage;
                            int forbiddenKnowledge = 1000000;
                            studentKnowledge += forbiddenKnowledge;
                            isHopeRestored = true;
                            Console.WriteLine($"Вы вынуждены использовать {abilitySecretUltimate}.\n" +
                                $"Только он сможет восстановить баланс сил. Вы читаете том запретных заклинаний и призываете Рому Сакутина.\n" +
                                $"Количество ошибок в коде ученика {AbilitySecretUltimateMentalDamage}. Сила воли ученика сломлена безвозвратно.");
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
                    mentorWorkHours -= studentTimeWastePerTurn;

                    switch (currentTypeOfWastingTime)
                    {
                        case CommandStudentUnneededCreativity:
                            Console.WriteLine(unneededCreativity);
                            break;
                        case CommandUnnecessaryComplication:
                            Console.WriteLine(unnecessaryComplication);
                            break;
                        case CommandWrongVariablesNaming:
                            Console.WriteLine(wrongVariablesNaming);
                            break;
                        case CommandWrongFormatting:
                            Console.WriteLine(wrongFormatting);
                            break;
                        case CommandBattleWithMentor:
                            Console.WriteLine(battleWithMentor);
                            break;
                    }

                    currentTypeOfWastingTime++;

                    if (currentTypeOfWastingTime > maxTypesOfWastingTime)
                        currentTypeOfWastingTime = 0;
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
            int geniousAmountOfKnowledge = 1000;

            if (studentKnowledge >= geniousAmountOfKnowledge)
                Console.WriteLine("Секретная концовка\n" +
                    "Ученик познает тайны вселенной, но вынужден провести остаток дней в психиатрической больнице. Оно того стоило.\n" +
                    $"Знания ученика: свыше {studentKnowledge - 1}");
            else if (studentWillPower <= 0 && mentorWorkHours <= 0)
                Console.WriteLine($"Ультимативная победа! Знания ученика преумножены, а ментор идет отдыхать.\n" +
                    $"Знания ученика: {studentKnowledge}");
            else if (studentWillPower > 0)
                Console.WriteLine($"Задание сдано. Отзыв в виде комментария: принято. Рассудок ученика цел, но стал ли он сегодня умнее?\n" +
                    $"Знания ученика всего: {studentKnowledge}");
            else if (mentorWorkHours > 0)
                Console.WriteLine("Задание провалено. Отзыв в виде комментария: доработать. Знания ученика преумножены, но какой ценой?\n" +
                    $"Знания ученика: {studentKnowledge}");
            #endregion
        }
    }
}
