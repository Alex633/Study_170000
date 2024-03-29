SHORTCUTS
ctrl + k + d //number alignment in array
alt + arrow //move line of code
ctrl + r + r //rename variable 
ctrl + alt //set new edit cursor (can be more then one)

# DATA TYPES
int32 - integer;

# VARIABLES
int i;
int i = 1;
string str = "text";
int.MinValue //contain min value of type int

# ARRAYS
int[] arrayName = new int[arrayLength] { 0, 1, 2, 3, 4 };
int[] array = { 1, 2, 3, 4 };
arrayName[cellNumber] = i;
int[,] arrayMatrix = new int[5, 3]
int[,] arrayMatrix = { {0,1,2,3,4,5} {0,1,2,3,4,5} {0,1,2,3,4,5} }
arrayMatrix[0, 2] = i;
array.Length //number of cells
array.getLength(lines/rows) //number of cells (lines or rows) 

# COLLECTIONS
# list
List<int> listName = new List<int>(size); //size is unnecessary
listName.Add(value); // adding value in the end
listName.AddRange(new int[] { 3, 4, 5 }); //adding range of values in the end
listName.RemoveAt(element); // deletes element
listName.Remove(elementValue); // deletes element with selected value
listName.Clear(); //fully clears list
listName.IndexOf(selectedValue); //contains infex of element of selected value
listName.Count; // find list length (number of elements)
listName.Insert(index, value); // inserts value in index position and moving forward all next elements
------
# queue
Queue<int> queueName = new Queue<string>; //first in, first out (like queue in bad place)
queueName.Endqueue(value); //adds element in the end of the queue
queueName.Dequeue(); //contains first element in queue, deletes first element in queue
queueName.Peek(); //contains first element in queue
------
# stack
Stack<int> stackName = new Stack<int>(size) //first in, last out (like stack of books)
stackName.Push(value); //adds element in stack
stackName.Peek(); //contains last element in stack
stackName.Pop(); //contains last element in stack, deletes after use
------
# dictionary
Dictionary<string, string> dictionaryName = new Dictionary<string, string>(); //ex. word (called key) - meaning (called value). Structure like dictionary
dictionaryName.Add("key", "value"); //adds element in dictionary
dictionaryName[key]; //contains element value
dictionaryName.ContainsKey(key); //return boolean value, true if dictionary contains selected key, false if not
dictionaryName.Remove(key) //deletes element

# LOGICAL OPERATORS
i== //проверка условия на истинность elements
!=
>
>=
|| //or
&& //and

# instamath
i -= b //тоже самое что и i=i-b

# CONSOLE COMMANDS
Console.WriteLine("text");
Console.WriteLine('-', 10); //writes ten -
i = Console.ReadLine(); //for string
i = Convert.ToInt32(Console.ReadLine()); //for number
Console.ReadKey() //waits for user input (any key)
ConsoleKeyInfo i = Console.ReadKey() //var that contains key
i.Key //var wih key
Console.ForegroundColor = ConsoleColor.Blue;
Console.BackgroundColor = ConsoleColor.Green; //с Console.Clear для полного окрашивания
ConsoleColor i = Console.ForegroundColor; //var for font color
Console.WindowHeight = 6;
Console.WindowWidth = 6;
Console.SetCursorPosition(x, y);

# CONCATENATION
Console.WriteLine("text" + i + 1);

# INTERPOLATION
Console.WriteLine($"text {i}"");

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
for (int i = 0; i > 10; i++); //задача переменной, условие, шаг 
{code}

# FOREACH //для перебора массива, когда не нужно обращаться по индексу элемента
foreach (int i in arrayName)
{code}

# FUNCTION
static void FunctionName (int var, int var2, int optionalVar = 1, int optionalVar2 = 2)
{code}
FunctionName(var, var2, optionalVar2: 3); //function call

# RANDOM
Random randVarient = new Random(); //initialization
i = randVarient.Next(0,10); //from-to (last number dont included);

# CONSOLE
\n //перенос строки
\b //удалить последний символ
Console.Clear();

# TEXT
string.ToLower //all symbols are know in lower case
string.ToUpper //all symbols to upper case
string.Substring(varStart); //uses only string from varStart till end
int num = double.Parse("stringValue", CultureInfo.InvariantCulture); //converts string to int, returns exception if cant; CultureInfo.InvariantCulture to use only . in a number
TryParse(stringValue, out resultVar)

# MULTITHREADING
Task.Run(() =>
{
});

# OOP
class ClassName : ParentClass, IInterfaceOne, IInterfaceTwo //structure
{
    //fields
    public int PublicFieldName;
    private string _privateFieldName;
    
    //constructor
    public ClassName(int publicFieldName, string _privateFieldName, ClassName classObject) : base(parent args)
    {
        PublicFieldName = publicField
        _privateFieldName = privateField
    }
    public ClassName() {} //if перегрузка (dont want to fill all data in main block all the time) is needed
    
    //properties (getters and setters)
    public double getterSetterFieldName { get; private set; } //set не нужен, когда негде кроме конструктора значение не меняется
    public float getterSetterFieldOpenName
    {
        get
        {
            return getterSetterFieldOpenName
        }
        private set
        {
            getterSetterFieldOpenName = value;
        }
    }
    
    //methods
    public void MethodName(args) {code}
}
ClassName objectName = new ClassName(args); // in () data from constructor
ClassName objectName = { new ClassName(args), new ClassName(args) }; // for creating multiple instances (objects)
this.objectName = var; //uses field of current class
