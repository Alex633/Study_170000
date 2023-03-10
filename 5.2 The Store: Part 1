using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CodingCoursesButThisTimeForFree
{
    internal class program
    {
        static void Main(string[] args)
        {
            //bool isTimed = true; //timed 
            //long currentTime = 0;
            //long duration = 100;
            //Stopwatch timer = new Stopwatch();
            int selectedOption;
            string userInput = null;
            bool isWorkingHours = true;
            Random random = new Random();

            List<string> mainOptions = new List<string>();
            mainOptions.AddRange(new string[] { "Open Store", "Not Today" });
            List<string> mainOptionsRU = new List<string>();
            mainOptionsRU.AddRange(new string[] { "Открыть Магазин", "Не сегодня" });
            List<string> languageSelect = new List<string>();
            languageSelect.AddRange(new string[] { "English", "Русский" });

            List<int> transactions = new List<int>();
            transactions.AddRange(new int[] { 9, 54, 79, 19 });

            Queue<string> clients = new Queue<string>();
            Queue<int> checks = new Queue<int>();
            int balance = 0;
            int balanceTemp = 161;
            int time = 8;
            int timeTemp = 22;
            int count = 0;
            int clientsTemp = 0;
            string latestClient = "";
            bool isHudOn = false;
            bool isRegretOn = false;
            int victoryBalance = 600;
            int doingGoodBalance = 400;
            bool isEndingDoneBadAlone = false;
            bool isEndingDoneBadGeralt = false;
            bool isEndingDoneGoodAlone = false;
            bool isEndingDoneGoodGeralt = false;
            Queue<string> notification = new Queue<string>();

            for (int i = 0; i < transactions.Count; i++)
                balance += transactions[i];

            for (int i = 0; i < 12; i++)
                checks.Enqueue(random.Next(90, 100));

            Task.Run(() =>
            {
                notification.Enqueue($" + {balance - balanceTemp}g ");
                while (true)
                {
                    if (balance > balanceTemp)
                    {
                        //notification.Enqueue($" + {balance - balanceTemp}g ");
                        WriteIn($" + {balance - balanceTemp}g ", 2, 6, true, 3000, true, ConsoleColor.DarkGray);
                        balanceTemp = balance;
                    }
                    else if (balance < balanceTemp)
                    {
                        //notification.Enqueue($" - {balance - balanceTemp}g ");
                        WriteIn($" - {balance - balanceTemp}g ", 2, 6, true, 3000, true, ConsoleColor.DarkGray);
                        balanceTemp = balance;
                    }

                    if (time > timeTemp)
                    {
                        //notification.Enqueue($" {time - timeTemp} hours passed ");
                        WriteIn($" {time - timeTemp} часа спустя ", 2, 6, true, 3000, true, ConsoleColor.DarkGray);
                        timeTemp = time;
                    }
                    
                    Thread.Sleep(300);
                    
                    //if (clients.Count > clientsTemp)
                    //{
                    //    notification.Enqueue($" {clients.Peek()} enters the store ");
                    //    WriteIn($" {clients.Peek()} enters the store ", 2, 6, true, 8000, true, ConsoleColor.DarkGray);
                    //    clientsTemp = clients.Count;
                    //    latestClient = clients.Peek();
                    //}
                    //else if (clients.Count < clientsTemp)
                    //{
                    //    notification.Enqueue($" {latestClient} leaves the store ");
                    //    WriteIn($" {latestClient} leaves the store ", 2, 7, true, 8000, true, ConsoleColor.DarkGray);
                    //    latestClient = "error";
                    //}
                }
            }); //notification write in q

            Task.Run(() =>
            {
                while (true)
                {
                    DisplayBalanceAndTime(ref balance, ref time, victoryBalance, isHudOn);
                    Thread.Sleep(500);
                }
            }); //time and money HUD

            //testing
            //MenuInput(mainOptions, out isOptionSelected, ref selectedOption, selectorCharXPos, ref selectorCharYPos, middleMenuStartX, middleMenuStartY);
            //Console.ReadKey();
            //

            Console.CursorVisible = false;

            WriteIn("Alexander's game", 47, 14, true);
            WriteIn("The Store: Part 1", 46, 14, true, 2000);
            WriteIn("Посвящается Нике", 53, 14, true, 2000);


            SelectOption(languageSelect, out selectedOption, 3, 1);

            switch (selectedOption)
            {
                case 0: //english
                    Console.WriteLine("\n Welcome to WORK");
                    SelectOption(mainOptions, out selectedOption, 3, 3);

                    switch (selectedOption)
                    {
                        case 0: //open store                              
                            Notify($"Your goal is simple. Earn {victoryBalance}g by the 23:59", "I can do it");
                            Notify("Now you have... let's see...", "Let's");
                            isHudOn = true;
                            Notify("Down there. See?", "I do");
                            Notify("And there is time too. See? See?", "Yeah, I see it");
                            Notify("Okay. Let's now wait for your First Customer", "Oh yeah");
                            Notify("Any minute now", "Can't wait");
                            Notify("Still waiting", "Okay");
                            time += 4; //12
                            Notify("4 hours passed", "Really?");
                            Notify("Here he comes. Behind the door!", "Finaly");
                            Notify("No, he passed through", "God damn it");
                            time += 10; //22
                            Notify("10 hours passed", "It's not looking good");
                            Notify("It's almost end of the day", "...");
                            Notify("Maybe they will come closer to night?", "Sure");
                            Notify("Probobly not tho", "I have no words");
                            Notify("Wait...", "What now");

                            while (isWorkingHours)
                            {
                                isHudOn = true;
                                if (isEndingDoneBadGeralt || isEndingDoneGoodGeralt || isEndingDoneBadAlone || isEndingDoneGoodAlone)
                                    Notify("Your First Customer has arrived!", "I hope It's not Frank");
                                else
                                {
                                    Notify("Your First Customer has arrived!", "It took so long");
                                }

                                clients.Enqueue("Frank"); //1

                                if (isEndingDoneBadGeralt || isEndingDoneGoodGeralt || isEndingDoneBadAlone || isEndingDoneGoodAlone)
                                    Notify($"{clients.Peek()} enters the store", "It's him");
                                else
                                    Notify($"{clients.Peek()} enters the store", "I'm so glad you came");

                                ColorizeText("\n  <<I would like to buy these olives, please>>", ConsoleColor.Blue);

                                List<string> johnAnswers = new List<string>();
                                johnAnswers.AddRange(new string[] { "Okay then", "Do you want two?", "Olives, really?" });

                                SelectOption(johnAnswers, out selectedOption, 3, 3);

                                switch (selectedOption)
                                {
                                    case 0: //okay then                                       
                                        balance += checks.Peek();
                                        checks.Dequeue();
                                        Notify("<<Thank you very much>>", "Not bad", 2, 1, ConsoleColor.Blue);
                                        break;
                                    case 1: //do you want two
                                        Notify("<<I mean sure, I can use two>>", "Continue", 2, 1, ConsoleColor.Blue);
                                        balance += checks.Peek() * 2;
                                        checks.Dequeue();
                                        Notify("You sold two olives and got twice more g. Nice", "I'm definatly going for the good ending now");
                                        break;
                                    case 2: //olives, really
                                        ColorizeText("\n  <<Yeah, it's a new hotness. Didn't you know?>>", ConsoleColor.Blue);

                                        List<string> johnAnswer2 = new List<string>();
                                        johnAnswer2.AddRange(new string[] { "I actually did", "I actually didn't", "You are insane" });
                                        SelectOption(johnAnswer2, out selectedOption, 3, 3);

                                        switch (selectedOption)
                                        {
                                            case 0:
                                                ColorizeText("\n  <<Glad to meet another olives lover. So can I have one?>>", ConsoleColor.Blue);
                                                break;
                                            case 1:
                                                ColorizeText("\n  <<You are insane, everybody knows that>>", ConsoleColor.Blue);
                                                break;
                                            case 2:
                                                ColorizeText("\n  <<Wow, that was rude. You lucky I can't live without my olives>>", ConsoleColor.Blue);
                                                break;
                                        }

                                        List<string> johnAnswer3 = new List<string>();
                                        johnAnswer3.AddRange(new string[] { "Sell him his olives", "Refuse Service" });
                                        SelectOption(johnAnswer3, out selectedOption, 3, 3);

                                        switch (selectedOption)
                                        {
                                            case 0: //sell                                     
                                                balance += checks.Peek();
                                                checks.Dequeue();
                                                Notify("<<Thank you very much. I hope I'll not be late>>", "What a madman");
                                                break;
                                            case 1: //refuse
                                                isRegretOn = true;
                                                Notify("<<You'll regret it>>", "I doubt it");
                                                break;
                                        }
                                        break;
                                }

                                clients.Dequeue(); //0
                                Notify($"You hear the door closing", "I actually don't");
                                time += 1; //23
                                Notify($"Moonlight starts shining throgh your magnificent store", "Wish I could see that");
                                clients.Enqueue("Larisa"); //1
                                Notify($"<<Hi there, hello>>", "Where did you come from?", 2, 1, ConsoleColor.Magenta);
                                Notify($"<<Do you have... aaaa...>>", "We have everything", 2, 1, ConsoleColor.Magenta);
                                Notify($"<<Do you have some olives?>>", "You too???", 2, 1, ConsoleColor.Magenta);
                                ColorizeText($"\n  <<I need olives>>", ConsoleColor.Magenta);

                                List<string> larisaAnswer = new List<string>();
                                larisaAnswer.AddRange(new string[] { "Fine, take your olives", "Get out of my store, you lunatics" });
                                SelectOption(larisaAnswer, out selectedOption, 3, 3);

                                switch (selectedOption)
                                {
                                    case 0: //sell                                     
                                        balance += checks.Peek();
                                        checks.Dequeue();
                                        Notify("She just takes her olives and silently leaves", "I'm not sure I want to do this anymore");
                                        break;
                                    case 1: //refuse
                                        Notify("She runs out of the store. Was it necessary?", "It sure was");
                                        break;
                                }

                                clients.Dequeue(); //0
                                Notify($"You done your second customer, I'm proud of you", "Unlike my father ever did");
                                time += 1; //24
                                Notify($"It's starts raining", "I kinda like that view");
                                clients.Enqueue("Grandpa"); //1
                                Notify($"Suddenly, you hear a door bell", "I didn't know I had one");
                                Notify($"You approuch the door and open it", "But it's open");
                                Notify($"It's Alexander's Grandpa", "Maybe I should offer him a cup of coffee");
                                Notify($"<<We could finally spent some time together>>, - a thought appears in your head", "But it's first time I'm seeing that man");
                                Notify($"<<So how is your study going?>>", "What is he talking about?", 2, 1, ConsoleColor.DarkRed);

                                clients.Dequeue(); //0
                                Notify($"Without waiting for an answer, he leaves the store", "What was that?");
                                time += 1; //25
                                Notify($"You stand for full hour looking into nothing", "That's gotta look hella sad");
                                clients.Enqueue("Geralt"); //1 
                                Notify($"{clients.Peek()} falls from the celling window", $"He does what?");
                                ColorizeText($"\n  <<I need two silver swords>>", ConsoleColor.Yellow);

                                List<string> geraltAnswers = new List<string>();
                                geraltAnswers.AddRange(new string[] { "We have only one left", "Are you sure you don't want olives?" });
                                SelectOption(geraltAnswers, out selectedOption, 3, 3);

                                switch (selectedOption)
                                {
                                    case 0: //sell                                     
                                        balance += checks.Peek();
                                        checks.Dequeue();
                                        Notify("<<I'll take one then. By the way, did I told you that I'm a witcher?>>", "Please don't", 2, 1, ConsoleColor.Yellow);
                                        clients.Dequeue(); //0
                                        Notify($"Geralt leaves the store. He seemed upset you didn't let him finish>>", "I'm glad he is gone");
                                        break;
                                    case 1: //olives
                                        Notify("<<Olives is evil. Lesser, greater, middling… Makes no difference. The degree is arbitar. The...>>", "Yeah, I got it", 2, 1, ConsoleColor.Yellow);
                                        Notify($"<<...definition’s blurred. If I’m to choose between one olive and another…>>", "He just can't stop", 2, 1, ConsoleColor.Yellow);
                                        Notify($"You insist that he chooses what he needs (except swords), but he lost in his mind", "Well that's just great");
                                        Notify($"Now he just stands here", "Amazing");
                                        break;
                                }

                                time += 1; //26
                                if (balance > doingGoodBalance)
                                    Notify($"You have more than {doingGoodBalance}. Doing pretty good", "Why is it 26:00 on my watch?");
                                else
                                    Notify($"Less than {balance}. Things are not going well", "Why is it 26:00 on my watch?");

                                clients.Enqueue("Nika"); //1 or 2
                                Notify($"Nika approaches the dog section", "She looks lovely");
                                Notify($"Nika picks up small pink ball", "This is the most beautiful girl that I ever saw");
                                Notify($"You notice a gold ring on her finger", "You are immeasurably dissapointed");

                                if (clients.Count > 1)
                                    Notify($"<<Are you in line, mister>>, - she askes {clients.Peek()}", "No, he is just stands here. Ignore him", 2, 1, ConsoleColor.Cyan);

                                ColorizeText($"\n  <<How much for this ball? I feel like my dog will really loves it>>", ConsoleColor.Cyan);

                                List<string> nikaAnswers = new List<string>();
                                nikaAnswers.AddRange(new string[] { $"Let's see... (random.Next(90, 100). It's {checks.Peek()}", "For you it's free" });
                                SelectOption(nikaAnswers, out selectedOption, 3, 3);

                                switch (selectedOption)
                                {
                                    case 0: //sell
                                        Notify($"You feel like you have seen something that you shouldn't seen", "But it was most definatly intended");
                                        balance += checks.Peek() + 25;
                                        checks.Dequeue();
                                        Notify("<<Thanks. And here is a tip for you. Have a nice day!>>", "Man, I hope I'll see you again", 2, 1, ConsoleColor.Cyan);

                                        if (clients.Count > 1)
                                        {
                                            clients.Dequeue(); //1
                                            WriteIn($"                                        ", 2, 7); //geralt never leaves
                                            clients.Dequeue(); //0
                                            clients.Enqueue("Geralt"); //1
                                            WriteIn($"                                        ", 2, 6); //geralt never comes
                                            Notify($"<<And you too, mister>>", "Who is she talking to?", 2, 1, ConsoleColor.Cyan);
                                        }
                                        else
                                        {
                                            clients.Dequeue(); //0
                                            Notify($"And than she is gone", "Like a sun rays throgh my fingers");
                                        }

                                        Notify("You earned 25 extra G, but you wish you have won this girls heart", "More then anything else", 2, 1);
                                        Notify("But she is married", "But she is married", 2, 1);
                                        Notify("Let's move on", "How can I?", 2, 1);
                                        break;
                                    case 1: //free
                                        Notify("Are you trying to flirt with her?", "I mean...", 2, 1);
                                        Notify("Didn't you see that she is married?", "How coud I resist?", 2, 1);
                                        Notify("Okay, you asked for it", "But", 2, 1);
                                        Notify("Let's try it again", "For you it's free, gorgeous", 2, 1);
                                        Notify("Is what you trying to say, but instead you awkwardly stand there in silence", "...", 2, 1);
                                        Notify("Blinded by her beauty", "...", 2, 1);
                                        Notify("Like a total loser", "...", 2, 1);
                                        Notify("<<Are you alright?>>", "I think I love you", 2, 1, ConsoleColor.Cyan);
                                        Notify("But only thing she hears is the dead silence of your great store (probobly for the best)", "...", 2, 1);
                                        Notify("Can I help you?", "I wish I could speak to you", 2, 1);
                                        Notify("Thankfully you can't", "...", 2, 1);
                                        Notify("Look, now she is living the store", "Please don't", 2, 1);
                                        Notify("And thinks you are weirdo", "I don't deserve it", 2, 1);

                                        if (clients.Count > 1)
                                        {
                                            Notify($"<<Have a nice day, mister>>", "Is sh...", 2, 1, ConsoleColor.Cyan);
                                            clients.Dequeue(); //geralt leaving 1
                                            WriteIn($"                                        ", 2, 7); //geralt never leaves
                                            clients.Dequeue(); //nika leaving 0
                                            clients.Enqueue("Geralt"); //geralt coming back 1
                                            WriteIn($"                                        ", 2, 6); //geralt never comes
                                            Notify("No, she was talking to Geralt", "I'll dream of you", 2, 1);
                                        }
                                        else
                                        {
                                            Notify("You will never see her again", "What is this coming from my eyes?", 2, 1);
                                            clients.Dequeue(); //nika is leaving 0
                                        }

                                        break;
                                }

                                if (isRegretOn)
                                {
                                    time += 1; //26
                                    clients.Enqueue("Rook"); // 1 or 2
                                    Notify($"Rook appears out of nowhere. He is wearing mask", "Who the hell is Rook?");
                                    Notify($"<<I told you, that you will regret it>>", "Frank?", 2, 1, ConsoleColor.DarkBlue);
                                    balance = 0;
                                    ColorizeText($"\n  He grabs all your G (olives too) and tries to runs away", ConsoleColor.Gray);

                                    List<string> brookSituation = new List<string>();
                                    brookSituation.AddRange(new string[] { "Stop him", "Don't stop him (what if he has a gun)" });
                                    SelectOption(brookSituation, out selectedOption, 3, 3);

                                    switch (selectedOption)
                                    {
                                        case 0: //free                                     
                                            Notify("You are trying to grab his hand. But he is too fast", "Not like this", 2, 1, ConsoleColor.DarkRed);
                                            break;
                                    }

                                    if (clients.Count > 1)
                                    {
                                        Notify("<<...I’d rather not choose at all>>", "Geralt?", 2, 1, ConsoleColor.Yellow);
                                        Notify($"{clients.Peek()} draws his sword and cuts John in half", "Oh my God");
                                        clients.Dequeue(); //geralt leaves
                                        WriteIn($"                                        ", 2, 7); //geralt never leaves
                                        clients.Dequeue(); //frank leaves
                                        clients.Enqueue("Geralt"); //geralt comes back
                                        WriteIn($"                                        ", 2, 6); //geralt never come back
                                        Notify($"Million olives drops out of Frank's packets. You are now rich", "I am?", 2, 1, ConsoleColor.Green);
                                        balance += 1000000;
                                        Notify($"You are", "Wow", 2, 1, ConsoleColor.Green);
                                    }
                                    else
                                    {
                                        clients.Dequeue(); //0
                                        Notify($"Frank slams the door (he even broke your door). He screams something but you can't recognize what", "What a terrible game");
                                    }
                                }

                                Notify($"It's {time}:00... I mean it's 0{time % 24}:00  You finish your day with {balance}G. And now it's time for conclusion", "Oh man");
                                isHudOn = false;

                                if (clients.Count == 1 & balance < 1000)
                                {
                                    ColorizeText("\n  MONSTER HUNTER SLAYER ENDING");
                                    Notify($"No, wait. We forgot about {clients.Peek()}", "He is STILL here?", 2, 3);
                                    Notify($"Yes, and he wants his money", "For what?!");
                                    Notify($"He says for his guard service ", "I never asked for this...");

                                    if (balance < victoryBalance)
                                    {
                                        balance = 0;
                                        clients.Dequeue(); //0
                                        Notify($"Now you broke and completely alone", "He took all my money?!");
                                        Notify($"Worst ending achived, you did it", "Great");
                                    }
                                    else
                                    {
                                        balance = 0;
                                        clients.Dequeue(); //0
                                        Notify($"Now you broke", "He took all my money?!");
                                        Notify($"But he promised to be with you until the end of your days", "Oh, did he?");
                                        Notify($"Worsest ending achived?", "Fuck");
                                    }
                                    isEndingDoneBadGeralt = true;
                                }

                                else if (clients.Count == 1 & balance > 1000)
                                {
                                    ColorizeText("\n  OLIVES ENDING");
                                    Notify($"You and {clients.Peek()} moved to Novigrad. You even had enough G to buy the best apartment in Novigrad. And since that monent you and Geralt lived happily ever after", "But I prefer to live alone", 2, 3);
                                    isEndingDoneGoodGeralt = true;
                                }

                                else if (clients.Count == 0 & balance < victoryBalance)
                                {
                                    ColorizeText("\n  MAYBE TOMORROW ENDING");
                                    Notify($"<<Tomorrow will be better day than today>>, - you say to yourself.", "It sure will", 2, 3);

                                    if (isRegretOn)
                                        Notify($"But somehow next morning never came", "Didn't I had enough");
                                    else
                                        Notify($"Will it?", "It will");

                                    isEndingDoneBadAlone = true;
                                }

                                else if (clients.Count == 0 & balance > victoryBalance)
                                {
                                    ColorizeText("\n  AMONG THE STARS ENDING");
                                    Notify($"You close your eyes and say to youself:", "I finally did it", 2, 3);
                                    Notify($"What's next for you, I wonder. Only stars", "Are we going to space?");
                                    isEndingDoneGoodAlone = true;
                                }

                                WriteIn("The End", 4, 3, true);
                                Console.SetCursorPosition(0, 1);

                                if (isEndingDoneBadGeralt)
                                    ColorizeText(" Monster Hunter Slayer Ending (Achived)", ConsoleColor.Green);
                                else
                                    ColorizeText(" Monster Hunter Slayer Ending", ConsoleColor.White);

                                if (isEndingDoneGoodGeralt)
                                    ColorizeText(" Olives Ending (Achived)", ConsoleColor.Green);
                                else
                                    ColorizeText(" Olives Ending", ConsoleColor.White);

                                if (isEndingDoneBadAlone)
                                    ColorizeText(" Maybe Tomorrow Ending (Achived)", ConsoleColor.Green);
                                else
                                    ColorizeText(" Maybe Tomorrow Ending", ConsoleColor.White);

                                if (isEndingDoneGoodAlone)
                                    ColorizeText(" Among The Stars Ending (Achived)", ConsoleColor.Green);
                                else
                                    ColorizeText(" Among The Stars Ending", ConsoleColor.White);

                                if (isEndingDoneBadGeralt && isEndingDoneGoodGeralt && isEndingDoneBadAlone && isEndingDoneGoodAlone)
                                {
                                    Console.Clear();
                                    WriteIn("Wow", 50, 14, true, 2000);
                                    WriteIn("You completed all endings", 45, 14, true, 2000);
                                    WriteIn("Didn't know you had it in you, Nika", 40, 14, true, 2000);
                                    WriteIn("Frank, Geralt and Nika will return in", 40, 14, true, 2000);
                                    WriteIn("The Store", 48, 14, true, 2000);
                                    WriteIn("The Store:", 48, 14, true, 2000);
                                    WriteIn("The Store: Part 2", 48, 14, true, 2000);
                                    isWorkingHours = false;
                                }
                                else
                                {
                                    WriteIn("Try Again?", 2, 8, false);
                                    List<string> tryAgain = new List<string>();
                                    tryAgain.AddRange(new string[] { "Of course, I haven't finished all the endings yet", "No (but Alexander's heart will be broken)" });
                                    SelectOption(tryAgain, out selectedOption, 3, 10);

                                    switch (selectedOption)
                                    {
                                        case 1: //free                                     
                                            isWorkingHours = false;
                                            break;
                                    }
                                }

                                //we have some cleaning to do
                                isRegretOn = false;
                                balance = 0;

                                for (int i = 0; i < 10; i++)
                                    checks.Enqueue(random.Next(90, 100));

                                for (int i = 0; i < transactions.Count; i++)
                                    balance += transactions[i];

                                balanceTemp = 161;
                                latestClient = "";
                                time = 22;
                                timeTemp = 22;

                                while (clients.Count > 0)
                                {
                                    clients.Dequeue();
                                    WriteIn($"                                        ", 2, 8);
                                }

                            }
                            break;
                        case 1: //exit
                            isWorkingHours = false;
                            break;
                    }
                    break;

                case 1: //russian
                    Console.WriteLine("\n Добро пожаловать на РАБОТУ");
                    SelectOption(mainOptionsRU, out selectedOption, 3, 3);

                    switch (selectedOption)
                    {
                        case 0: //open store                              
                            Notify($"Ваша цель проста. Заработайте {victoryBalance}g до 23:59", "Я справлюсь");
                            Notify("Сейчас у вас... давайте проверим...", "Давайте");
                            isHudOn = true;
                            Notify("Внизу. Видите?", "Да");
                            Notify("Еще там время есть. Видно? Видно?", "Да я уже заметила");
                            Notify("Хорошо. Ждем вашего первого клиента", "О да");
                            Notify("Он может появиться в любую секунду", "Жду не дождусь");
                            Notify("Ожидание", "Хорошо");
                            time += 4; //12
                            Notify("4 часа спустя", "Серьезно?");
                            Notify("А вот и клиент. Стоит за дверью!", "Наконец-то");
                            Notify("О нет, он прошел мимо", "Будь ты проклят");
                            time += 10; //22
                            Notify("10 часов спустя", "Сегодня не мой день");
                            Notify("И день почти закончился", "...");
                            Notify("Может они все придут ночью?", "Да, конечно");
                            Notify("Но скорее всего нет", "У меня нет слов");
                            Notify("Секунду...", "Что теперь?");

                            while (isWorkingHours)
                            {
                                isHudOn = true;
                                if (isEndingDoneBadGeralt || isEndingDoneGoodGeralt || isEndingDoneBadAlone || isEndingDoneGoodAlone)
                                    Notify("Ваш первый клиент открывает дверь!", "Надеюсь это не Фрэнк");
                                else
                                {
                                    Notify("Ваш первый клиент открывает дверь!", "Прошло так много времени");
                                }

                                clients.Enqueue("Фрэнк"); //1

                                if (isEndingDoneBadGeralt || isEndingDoneGoodGeralt || isEndingDoneBadAlone || isEndingDoneGoodAlone)
                                    Notify($"{clients.Peek()} входит в магазин", "Это он");
                                else
                                    Notify($"{clients.Peek()} входит в магазин", "Я так рада, что вы пришли");

                                ColorizeText("\n  <<Я хотел бы купить вот эти оливки, пожалуйста>>", ConsoleColor.Blue);

                                List<string> johnAnswers = new List<string>();
                                johnAnswers.AddRange(new string[] { "Ладно", "Не хотите две?", "Оливки, серьезно?" });

                                SelectOption(johnAnswers, out selectedOption, 3, 3);

                                switch (selectedOption)
                                {
                                    case 0: //okay then                                       
                                        balance += checks.Peek();
                                        checks.Dequeue();
                                        Notify("<<Большое спасибо>>", "Неплохо", 2, 1, ConsoleColor.Blue);
                                        break;
                                    case 1: //do you want two
                                        Notify("<<Почему бы и нет, две мне точно пригодятся>>", "Продолжить", 2, 1, ConsoleColor.Blue);
                                        balance += checks.Peek() * 2;
                                        checks.Dequeue();
                                        Notify("Вы продали две оливки и получили в два раза больше G. Класс", "Теперь я точно иду на хорошую концовку");
                                        break;
                                    case 2: //olives, really
                                        ColorizeText("\n  <<Да, с ними жить в семь раз лучше. Вы не знали?>>", ConsoleColor.Blue);

                                        List<string> johnAnswer2 = new List<string>();
                                        johnAnswer2.AddRange(new string[] { "Ну вообще знала", "Ну вообще не знала", "Ты сумасшедший" });
                                        SelectOption(johnAnswer2, out selectedOption, 3, 3);

                                        switch (selectedOption)
                                        {
                                            case 0:
                                                ColorizeText("\n  <<Рад встретить еще одного любителя оливок. Так можно мне оливок?>>", ConsoleColor.Blue);
                                                break;
                                            case 1:
                                                ColorizeText("\n  <<Ты сумасшедший, это все знают>>", ConsoleColor.Blue);
                                                break;
                                            case 2:
                                                ColorizeText("\n  <<Как грубо. Вам везет, что я не могу жить без моих оливок>>", ConsoleColor.Blue);
                                                break;
                                        }

                                        List<string> johnAnswer3 = new List<string>();
                                        johnAnswer3.AddRange(new string[] { "Продать ему эти чертовы оливки", "Отказать в обслуживании" });
                                        SelectOption(johnAnswer3, out selectedOption, 3, 3);

                                        switch (selectedOption)
                                        {
                                            case 0: //sell                                     
                                                balance += checks.Peek();
                                                checks.Dequeue();
                                                Notify("<<Спасибо. Надеюсь, я не опоздаю>>", "Ну и безумец");
                                                break;
                                            case 1: //refuse
                                                isRegretOn = true;
                                                Notify("<<Ты еще пожалеешь>>", "Сомневаюсь", 2, 1, ConsoleColor.Blue);
                                                break;
                                        }
                                        break;
                                }

                                clients.Dequeue(); //0
                                Notify($"Вы слышите как закрывается дверь", "Вообще-то не слышу");
                                time += 1; //23
                                Notify($"Лунный свет проникает в вашу чудесную лавку", "Хотелось бы мне это видеть");
                                clients.Enqueue("Лариса"); //1
                                Notify($"<<Здравствуйте, привет>>", "Откуда вы взялись?", 2, 1, ConsoleColor.Magenta);
                                Notify($"<<У вас есть... аааа...>>", "У нас есть все", 2, 1, ConsoleColor.Magenta);
                                Notify($"<<У вас есть немножко оливок?>>", "Ты тоже???", 2, 1, ConsoleColor.Magenta);
                                ColorizeText($"\n  <<Мне нужны оливки>>", ConsoleColor.Magenta);

                                List<string> larisaAnswer = new List<string>();
                                larisaAnswer.AddRange(new string[] { "Черт с вами, забирайте свои оливки", "Идите вы малину, больные культисты" });
                                SelectOption(larisaAnswer, out selectedOption, 3, 3);

                                switch (selectedOption)
                                {
                                    case 0: //sell                                     
                                        balance += checks.Peek();
                                        checks.Dequeue();
                                        Notify("Она быстро хватает свои оливки и по-тихому уходит", "Я не уверена, что хочу продолжать в это играть");
                                        break;
                                    case 1: //refuse
                                        Notify("Она выбегает из магазина. Неужели нельзя было по нормальному?", "Нет, нельзя");
                                        break;
                                }

                                clients.Dequeue(); //0
                                Notify($"Вы закончили со вторым клиентом. Я горжусь вами", "В отличии от моего отца");
                                time += 1; //24
                                Notify($"За окном идет дождь", "Такой вид мне больше нравится");
                                clients.Enqueue("Дедушка Александра"); //1
                                Notify($"Неожиданно, вы слышите дверной звонок", "Не знал, что у меня такой есть");
                                Notify($"Вы подходите к двери и открываете ее", "Но она уже была открыта");
                                Notify($"Это дедушка Александра", "Может предложить ему кофе?");
                                Notify($"<<Наконец, мы сможем провести немного времени вместе>>, - мысль пролетает в вашей голове", "Но я впервые вижу этого человека");
                                Notify($"<<Как учеба продвигается?>>", "О чем он вообще?", 2, 1, ConsoleColor.DarkRed);

                                clients.Dequeue(); //0
                                Notify($"Не дождавшись ответа, он покидает магазин", "Скорее всего дела");
                                time += 1; //25
                                Notify($"Вы стоите и размышляете, почему у него никогда нет на вас времени", "Время улетает незаметно");
                                clients.Enqueue("Геральт"); //1 
                                Notify($"{clients.Peek()} спрыгивает с зенитного окна", $"Что-что простите?");
                                ColorizeText($"\n  <<Мне нужно два серебряных меча>>", ConsoleColor.Yellow);

                                List<string> geraltAnswers = new List<string>();
                                geraltAnswers.AddRange(new string[] { "У нас один остался", "Вы уверены, что не хотите оливок?" });
                                SelectOption(geraltAnswers, out selectedOption, 3, 3);

                                switch (selectedOption)
                                {
                                    case 0: //sell                                     
                                        balance += checks.Peek();
                                        checks.Dequeue();
                                        Notify("<<Давайте один тогда. Кстати уже вам говорил, что я ведьмак?>>", "Пожалуйста не надо", 2, 1, ConsoleColor.Yellow);
                                        clients.Dequeue(); //0
                                        Notify($"Геральт покидает магазин. Он выглядит растроенным, потому что вы не позволили ему сказать это>>", "Я рада, что он ушел");
                                        break;
                                    case 1: //olives
                                        Notify("<<Оливки это зло. Меньшие, большие, средние - все едины, пропорции условны, а...>>", "Да я слышала это", 2, 1, ConsoleColor.Yellow);
                                        Notify($"<<...границы размыты. Я не святой отшельник, не только одно добро творил в жизни. Но...>>", "Он не может остановиться", 2, 1, ConsoleColor.Yellow);
                                        Notify($"Вы настаиваете, чтобы он определился, что ему надо (кроме мечей), но похоже он в своей голове", "Замечательно");
                                        Notify($"Теперь он просто стоит", "Потрясающе");
                                        break;
                                }

                                time += 1; //26
                                if (balance > doingGoodBalance)
                                    Notify($"У вас больше {doingGoodBalance}. Дела идут неплохо", "Почему на моих часах 26:00?");
                                else
                                    Notify($"Меньше {balance}. Сегодня бизнес так себе", "Почему на моих часах 26:00?");

                                clients.Enqueue("Ника"); //1 or 2
                                Notify($"Ника идет к секции для животных", "Она выглядит милой");
                                Notify($"Она поднимает маленький розовый мячик", "Это самая красивая девушка, которую я когла либо видела");
                                Notify($"Вы замечаете кольцо на ее пальце", "Я невероятно разочарован");

                                if (clients.Count > 1)
                                    Notify($"<<Вы стоите в очереди, мистер?>>, - она спрашивает {clients.Peek()}а", "Нет, он просто здесь стоит. Не обращайте внимания", 2, 1, ConsoleColor.Cyan);

                                ColorizeText($"\n  <<Сколько за этот мячик? Мне кажется моей собаке он крайне полюбится>>", ConsoleColor.Cyan);

                                List<string> nikaAnswers = new List<string>();
                                nikaAnswers.AddRange(new string[] { $"Давайте посмотрим... (random.Next(90, 100). Всего {checks.Peek()}", "Для вас - бесплатно" });
                                SelectOption(nikaAnswers, out selectedOption, 3, 3);

                                switch (selectedOption)
                                {
                                    case 0: //sell
                                        Notify($"Вам кажется, что вы увидели что-то, что не должны были увидеть", "Но все наиболее точно так и было задумано");
                                        balance += checks.Peek() + 25;
                                        checks.Dequeue();
                                        Notify("<<Спасибо. И небольшие чаевые. С праздником!>>", "Сегодня праздник?", 2, 1, ConsoleColor.Cyan);

                                        if (clients.Count > 1)
                                        {
                                            clients.Dequeue(); //1
                                            WriteIn($"                                        ", 2, 7); //geralt never leaves
                                            clients.Dequeue(); //0
                                            clients.Enqueue("Геральт"); //1
                                            WriteIn($"                                        ", 2, 6); //geralt never comes
                                            Notify($"<<И вас тоже, мистер>>", "Продолжить", 2, 1, ConsoleColor.Cyan);
                                        }
                                        else
                                        {
                                            clients.Dequeue(); //0
                                            Notify($"И вот ее снова нет", "Как лучи солнца, сквозь мои пальцы");
                                        }

                                        Notify("Вы заработали на 25 G больше, но вместо этого, вы бы предпочли завоевать ее сердце", "Больше, чем что-либо еще", 2, 1);
                                        Notify("Но она замужем", "Но она замужем", 2, 1);
                                        Notify("Давайте двигаться вперед", "Я не смогу", 2, 1);
                                        break;
                                    case 1: //free
                                        Notify("Вы пытаетесь с ней флиртовать?", "Ну...", 2, 1);
                                        Notify("Разве вы не поняли, что она замужем?", "Как я мог устоять?", 2, 1);
                                        Notify("Ладно, вы сами напросились", "Но", 2, 1);
                                        Notify("Попробуем сначала", "Для вас - бесплатно, милашка", 2, 1);
                                        Notify("Что вы пытаетесь сказать, но вместо этого вы стоите в тишине", "...", 2, 1);
                                        Notify("Ослепленные ее красотой", "...", 2, 1);
                                        Notify("Как полная неудачница", "...", 2, 1);
                                        Notify("<<Вы в порядке?>>", "Мне кажется я люблю тебя", 2, 1, ConsoleColor.Cyan);
                                        Notify("Но все, что она слышит это мертвая тишина вашего прекрасного магазина (скорее всего это к лучшему)", "...", 2, 1);
                                        Notify("<<Я могу вам как-нибдуь помочь?>>", "Хотела бы я поговорить с тобой", 2, 1, ConsoleColor.Cyan);
                                        Notify("К счастью, вы не можете", "...", 2, 1);
                                        Notify("Смотрите, теперь она покидает магазин", "Пожалуйста не надо", 2, 1);
                                        Notify("И думает, что вы чудачка", "Я этого не заслуживаю", 2, 1);

                                        if (clients.Count > 1)
                                        {
                                            Notify($"<<Хорошего дня, мистер>>", "Она ска...", 2, 1, ConsoleColor.Cyan);
                                            clients.Dequeue(); //geralt leaving 1
                                            clients.Dequeue(); //nika leaving 0
                                            clients.Enqueue("Геральт"); //geralt coming back 1
                                            Notify("Нет, это она Геральту", "Я увижу тебя в моих снах", 2, 1);
                                        }
                                        else
                                        {
                                            Notify("Вы никогда не увидете ее снова", "Что это за жидкость в моих глазах?", 2, 1);
                                            clients.Dequeue(); //nika is leaving 0
                                        }

                                        break;
                                }

                                if (isRegretOn)
                                {
                                    time += 1; //26
                                    clients.Enqueue("Рук"); // 1 or 2
                                    Notify($"Рук возникает из ниоткуда. На нем маска", "Какой еще Рук?");
                                    Notify($"<<Я говорил, что ты пожалеешь>>", "Фрэнк?", 2, 1, ConsoleColor.DarkBlue);
                                    balance = 0;
                                    ColorizeText($"\n  Он хватает все ваши G (оливки тоже) и пытается убежать", ConsoleColor.Gray);

                                    List<string> brookSituation = new List<string>();
                                    brookSituation.AddRange(new string[] { "Остановить его", "Не останавливать его (вдруг у него есть пистолет)" });
                                    SelectOption(brookSituation, out selectedOption, 3, 3);

                                    switch (selectedOption)
                                    {
                                        case 0: //free                                     
                                            Notify("Вы пытаетесь схватить его за руку. Но он был слишком ловок", "Только не так", 2, 1);
                                            break;
                                    }

                                    if (clients.Count > 1)
                                    {
                                        Notify("<<...я предпочту не выбирать вовсе>>", "Геральт?", 2, 1, ConsoleColor.Yellow);
                                        Notify($"{clients.Peek()} достает свой мечь и разрубает Фрэнка напополам", "Боже мой");
                                        clients.Dequeue(); //geralt leaves
                                        clients.Dequeue(); //frank leaves
                                        clients.Enqueue("Geralt"); //geralt comes back
                                        Notify($"Миллион оливок выкатываются из карманов Фрэнка. Теперь вы богаты", "Я богата?", 2, 1, ConsoleColor.Green);
                                        balance += 1000000;
                                        Notify($"Вы богаты", "Ну ничего себе", 2, 1, ConsoleColor.Green);
                                    }
                                    else
                                    {
                                        clients.Dequeue(); //0
                                        Notify($"Фрэнк хлопает дверью (он даже сломал вашу дверь). Он кричит что-то напоследок, но вы не можете разобрать что", "Ужасная игра");
                                    }
                                }

                                Notify($"На ваших часах {time}:00... То есть 0{time % 24}:00. Вы закончили день с {balance}G. Настало время конца", "Ну и ну");
                                isHudOn = false;

                                if (clients.Count == 1 & balance < 1000)
                                {
                                    ColorizeText("\n  УБИЙЦА ЧУДОВИЩ КОНЦОВКА");
                                    Notify($"Нет, подождите. Мы забыли про {clients.Peek()}а", "Он ВСЕ ЕЩЕ здесь?!", 2, 3);
                                    Notify($"Да, и он требует оплаты", "За что?!");
                                    Notify($"Он говорит за работу охранником", "Я никогда этого не просила...");

                                    if (balance < victoryBalance)
                                    {
                                        balance = 0;
                                        clients.Dequeue(); //0
                                        Notify($"Теперь вы бедны и совсем одиноки", "Все мои деньги?!");
                                        Notify($"Худшая концовка достигнута, ты это сделала", "Прекрасно");
                                    }
                                    else
                                    {
                                        balance = 0;
                                        clients.Dequeue(); //0
                                        Notify($"Теперь вы бедны", "Все мои деньги?!");
                                        Notify($"Но он пообещал, что останется с вами до конца ваших дней", "Да неужели");
                                        Notify($"Наихудшейшая концовка достигнута?", "Холера");
                                    }
                                    isEndingDoneBadGeralt = true;
                                }

                                else if (clients.Count == 1 & balance > 1000)
                                {
                                    ColorizeText("\n  КОНЦОВКА С ОЛИВКАМИ");
                                    Notify($"Вы и {clients.Peek()} переехали в Новиград. У вас даже хватило G, чтобы купить лучшую квартиру в Новиграде. И с этого момента вы с Геральтом жили долго и счастливо", "Уж лучше я бы жила одна", 2, 3);
                                    isEndingDoneGoodGeralt = true;
                                }

                                else if (clients.Count == 0 & balance < victoryBalance)
                                {
                                    ColorizeText("\n  МОЖЕТ ЗАВТРА, КОНЕЦ");
                                    Notify($"<<Завтра будет лучше чем вчера>>, - вы говорите себе.", "Точно будет", 2, 3);

                                    if (isRegretOn)
                                        Notify($"Но следующее утро так и не наступило", "Неужели я страдал не достаточно");
                                    else
                                        Notify($"Уверена?", "Уверена");

                                    isEndingDoneBadAlone = true;
                                }

                                else if (clients.Count == 0 & balance > victoryBalance)
                                {
                                    ColorizeText("\n  ФИНАЛ СРЕДИ ЗВЕЗД");
                                    Notify($"Вы закрываете глаза и думаете:", "Я это сделала", 2, 3);
                                    Notify($"Что же дальше, мне интересно. Только звезды", "Мы летим в космос?");
                                    isEndingDoneGoodAlone = true;
                                }

                                WriteIn("Конец", 4, 3, true);
                                Console.SetCursorPosition(0, 1);

                                if (isEndingDoneBadGeralt)
                                    ColorizeText(" Убийца чудовищ концовка (достигнута)", ConsoleColor.Green);
                                else
                                    ColorizeText(" Убийца чудовищ концовка", ConsoleColor.White);

                                if (isEndingDoneGoodGeralt)
                                    ColorizeText(" Концовка с оливками (достигнута)", ConsoleColor.Green);
                                else
                                    ColorizeText(" Концовка с оливками", ConsoleColor.White);

                                if (isEndingDoneBadAlone)
                                    ColorizeText(" Может завтра, конец (достигнута)", ConsoleColor.Green);
                                else
                                    ColorizeText(" Может завтра, конец", ConsoleColor.White);

                                if (isEndingDoneGoodAlone)
                                    ColorizeText(" Финал среди звезд (Achived)", ConsoleColor.Green);
                                else
                                    ColorizeText(" Финал среди звезд", ConsoleColor.White);

                                if (isEndingDoneBadGeralt && isEndingDoneGoodGeralt && isEndingDoneBadAlone && isEndingDoneGoodAlone)
                                {
                                    Console.Clear();
                                    WriteIn("Надо же", 49, 14, true, 2000);
                                    WriteIn("Вы получили все концовки", 45, 14, true, 2000);
                                    WriteIn("Я знал, что ты справишься, Ника", 40, 14, true, 2000);
                                    WriteIn("Фрэнк, Геральт и Ника вернуться в", 40, 14, true, 2000);
                                    WriteIn("The Store", 48, 14, true, 2000);
                                    WriteIn("The Store:", 48, 14, true, 2000);
                                    WriteIn("The Store: Part 2", 48, 14, true, 2000);
                                    isWorkingHours = false;
                                }
                                else
                                {
                                    WriteIn("Попробовать снова?", 2, 8, false);
                                    List<string> tryAgain = new List<string>();
                                    tryAgain.AddRange(new string[] { "Конечно, я еще не получила все концовки", "Нет (но сердце Александра будет разбито)" });
                                    SelectOption(tryAgain, out selectedOption, 3, 10);

                                    switch (selectedOption)
                                    {
                                        case 1: //free                                     
                                            isWorkingHours = false;
                                            break;
                                    }
                                }

                                //we have some cleaning to do
                                isRegretOn = false;
                                balance = 0;

                                for (int i = 0; i < 10; i++)
                                    checks.Enqueue(random.Next(90, 100));

                                for (int i = 0; i < transactions.Count; i++)
                                    balance += transactions[i];

                                balanceTemp = 161;
                                latestClient = "";
                                time = 22;
                                timeTemp = 22;

                                while (clients.Count > 0)
                                {
                                    clients.Dequeue();
                                    WriteIn($"                                        ", 2, 8);
                                }

                            }
                            break;
                        case 1: //exit
                            isWorkingHours = false;
                            break;
                    }
                    break;

            }
            if (isEndingDoneBadGeralt && isEndingDoneGoodGeralt && isEndingDoneBadAlone && isEndingDoneGoodAlone)
                WriteIn(":)", 50, 14, true);
            else
                WriteIn(":(", 5, 2, true);
        }

        static void DisplayBalanceAndTime(ref int balance, ref int time, int victoryBalance, bool isHudOn)
        {
            if (isHudOn)
            {
                Console.SetCursorPosition(0, 28);
                ColorizeText($" {time}:00  |  ", ConsoleColor.DarkGray, false);
                ColorizeText($" Баланс:", ConsoleColor.DarkGray, false);
                ColorizeText($" {balance}g", ConsoleColor.Green, false);
                ColorizeText($"  |  Цель: {victoryBalance}g", ConsoleColor.DarkGray, false);
                Console.SetCursorPosition(0, 0);
            }
        }

        static void DrawHoverChar(int menuStartX, int menuStartY, bool isVisible = true, ConsoleColor color = ConsoleColor.DarkGreen)
        {
            char hoverChar = '♦';
            if (menuStartX > 1)
            {
                Console.SetCursorPosition(menuStartX - 2, menuStartY);

                if (isVisible == true)
                {
                    ColorizeText(hoverChar, color);
                }
                else
                {
                    Console.Write(" ");
                }
            }
        } //menu CHAR

        static void DrawMenu(List<string> options, int selectedOption, int menuStartX = 55, int menuStartY = 14)
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (selectedOption == i)
                {
                    WriteIn($" {options[i]} ", menuStartX, menuStartY + i, false, 0, true);
                    DrawHoverChar(menuStartX, menuStartY + i, true);
                }
                else
                {
                    WriteIn($" {options[i]} ", menuStartX, menuStartY + i, false, 0, false);
                    DrawHoverChar(menuStartX, menuStartY + i, false);
                }
            }
        }  //menu DRAW

        static void SelectOption(List<string> options, out int selectedOption, int menuStartX = 55, int menuStartY = 14)
        {
            ConsoleKeyInfo pressedButton = new ConsoleKeyInfo();
            bool isOptionSelected = false;
            Console.CursorVisible = false;
            selectedOption = 0;

            while (!isOptionSelected)
            {
                DrawMenu(options, selectedOption, menuStartX, menuStartY);
                pressedButton = Console.ReadKey();

                switch (pressedButton.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (options.Count > 0)
                        {
                            if (selectedOption != 0)
                            {
                                selectedOption--;
                            }
                            else
                            {
                                selectedOption = options.Count - 1;
                            }
                        }
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (options.Count > 0)
                        {
                            if (selectedOption != options.Count - 1)
                            {
                                selectedOption++;
                            }
                            else
                            {
                                selectedOption = 0;
                            }
                        }
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        isOptionSelected = true;
                        Console.Clear();
                        break;
                }
            }

            Console.CursorVisible = true;
        } //menu DRAW AND INPUT

        static void Notify(string title, string buttonText, int menuStartX = 2, int menuStartY = 1, ConsoleColor color = ConsoleColor.Gray, bool displayTitle = true)
        {
            ConsoleKeyInfo pressedButton = new ConsoleKeyInfo();
            bool isOptionSelected = false;
            Console.CursorVisible = false;

            if (displayTitle)
            {
                Console.SetCursorPosition(menuStartX, menuStartY);
                ColorizeText($"{title}", color);
            }

            WriteIn($" {buttonText} ", menuStartX + 1, menuStartY + 2, false, 0, true);

            while (!isOptionSelected)
            {
                pressedButton = Console.ReadKey();

                switch (pressedButton.Key)
                {
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        isOptionSelected = true;
                        break;
                }
            }

            Console.Clear();
        }

        static void EnterTextGUI(out string userInput, string title, string buttonText = "Confirm", int menuStartX = 0, int menuStartY = 2)
        {
            userInput = "";

            Console.WriteLine(title);
            WriteIn($" {buttonText} ", menuStartX, menuStartY + 1, false, 0, true); //confirm button
            WriteIn($"              ", menuStartX, menuStartY - 1, false, 0, true, ConsoleColor.DarkGray); //text field
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(menuStartX, menuStartY - 1);
            userInput = Console.ReadLine();
            Console.BackgroundColor = default;

        }

        static void WriteIn(string text, int xPosition = 47, int yPosition = 14, bool isTimed = false, long duration = 1600, bool isSelected = false, ConsoleColor selectionColor = ConsoleColor.DarkGreen)
        {
            Stopwatch timer = new Stopwatch();
            long currentTime = 0;

            ClearInput(xPosition, yPosition, text.Length);
            Console.SetCursorPosition(xPosition, yPosition);
            ConsoleHelper.SetCurrentFont("Consolas", 28);

            if (isSelected)
            {
                Console.BackgroundColor = selectionColor;
                Console.WriteLine(text);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.WriteLine(text);
            }


            if (isTimed)
            {
                while (currentTime < duration)
                {
                    timer.Start();
                    currentTime = timer.ElapsedMilliseconds;
                }

                Console.SetCursorPosition(xPosition, yPosition);
                Console.WriteLine("                                             ");
                timer.Stop();
                ClearInput(xPosition, yPosition, text.Length);
            }

            Console.SetCursorPosition(0, 0);
        } //write TEXT

        static void WriteInLog(string text, int xPos = 1, int yPos = 13)
        {
            ClearInput(xPos, yPos);
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(text);
        } //write LOG

        static void ClearInput(int xPos = 0, int yPos = 0, int length = 0)
        {
            Console.SetCursorPosition(xPos, yPos);

            for (int i = 0; i < length; i++)
                Console.Write(" ");
        } //clear

        static void ColorizeText(char character, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write(character);
            Console.ForegroundColor = ConsoleColor.White;
        } //colorize char

        static void ColorizeText(string text, ConsoleColor color = ConsoleColor.Gray, bool isWriteLine = true)
        {
            if (isWriteLine)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = color;
                Console.Write(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
        } //colorize text

        static void ColorizeBackground(char character, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.BackgroundColor = color;
            Console.ForegroundColor = color;
            Console.Write(character);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        } //colored tile
        static void ColorizeBackground(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.BackgroundColor = color;
            //Console.ForegroundColor = color;
            Console.Write(text);
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.ForegroundColor = ConsoleColor.White;
        } //colored BG

        public static class ConsoleHelper
        {
            private const int FixedWidthTrueType = 54;
            private const int StandardOutputHandle = -11;

            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern IntPtr GetStdHandle(int nStdHandle);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


            private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct FontInfo
            {
                internal int cbSize;
                internal int FontIndex;
                internal short FontWidth;
                public short FontSize;
                public int FontFamily;
                public int FontWeight;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
                public string FontName;
            }

            public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
            {
                //Console.WriteLine("Set Current Font: " + font);

                FontInfo before = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>()
                };

                if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
                {

                    FontInfo set = new FontInfo
                    {
                        cbSize = Marshal.SizeOf<FontInfo>(),
                        FontIndex = 0,
                        FontFamily = FixedWidthTrueType,
                        FontName = font,
                        FontWeight = 400,
                        FontSize = fontSize > 0 ? fontSize : before.FontSize
                    };

                    // Get some settings from current font.
                    if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
                    {
                        var ex = Marshal.GetLastWin32Error();
                        Console.WriteLine("Set error " + ex);
                        throw new System.ComponentModel.Win32Exception(ex);
                    }

                    FontInfo after = new FontInfo
                    {
                        cbSize = Marshal.SizeOf<FontInfo>()
                    };
                    GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                    return new[] { before, set, after };
                }
                else
                {
                    var er = Marshal.GetLastWin32Error();
                    Console.WriteLine("Get error " + er);
                    throw new System.ComponentModel.Win32Exception(er);
                }
            }
        } //font and font size
    }
}
