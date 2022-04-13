# DATA TYPES
int32 - whole numbers

# VARIABLES
int i;
int i = 1;
string str = "text";

# LOGICAL OPERATORS
i== //проверка условия на истинность
!=
>
>=

# CONSOLE COMMANDS
Console.WriteLine("text"); \n
i = Console.ReadLine(); //for string \n
i = Convert.ToInt32(Console.ReadLine()); //for number

# CONCATENATION
Console.WriteLine("text" + i + 1);

# INTERPOLATION
Console.WriteLine($"text {i}");

# CONVERT
i = Convert.ToSingle(i);

# INCREMENT AND DECREMENT
i++, i--, ++i, --i; //i + 1 / i - 1

# IF
if (i > 1) {code} else {code}
if (i > 1) {code} else if {code} else if {code}

# SWITCH //в случае взаимозаключающих вариантов
switch (i)
{
case "option one": code; break;
case "option two": code; break;
case "option three": case "option four": code; break;
default: code; break;
}

# WHILE
while (i-- > 0)
{code}
end of while - if with break;
skip iteration - if with continue;

# FOR //когда есть четкие границы выполнения
for (int i = 1; i > 10; i++); //задача переменной, условие, шаг 
{code}

# RANDOM
Random randVarient - new Random(); //initialization
i = randVarient.Next(0,10); //from-to (last number don't included)

# CONSOLE
\n //перенос строки
\b //удалить последний символ
Console.Clear();
Console.SetCursorPosition(0, 5);
Console.ForegroundColor = ConsoleColor.Blue;
Console.BackgroundColor = ConsoleColor.Green; //с Console.Clear для полного окрашивания
Console.WindowHeight = 6;
Console.WindowWidth = 6;
