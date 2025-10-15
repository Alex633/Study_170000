namespace CsLearning;
//У вас есть автосервис, в котором будут машины для починки.
//Автосервис содержит баланс денег и склад деталей. В автосервисе стоит очередь машин.
//Машина состоит из деталей и количество поломанных будет не меньше 1 детали. Надо показывать все детали, которые поломанные.
//Поломка всегда чинится заменой детали. При починке машины за раз можно заменять только одну деталь.
//При успешной починке детали сервис получает (цена детали + цена ремонта).
//Ремонт считается завершенным, когда все детали машины исправны. 
//От ремонта можно отказаться в любой момент.
//Если отказ перед ремонтом, то платите фиксированный штраф.
//Если отказ во время ремонта, то платите штраф за каждую непочиненную деталь.
//Количество деталей на складе ограничено.
//При замене целой детали в машине, деталь пропадает из склада, но вы ничего не получаете за замену данной детали. 
//За каждую удачную починку вы получаете выплату за ремонт, которая указана в чек-листе починки.
//Класс Деталь не может содержать значение “количество”. Деталь всего одна, за количество отвечает тот, кто хранит детали.
//При необходимости можно создать дополнительный класс для конкретной детали и работе с количеством.

//create few collections with each type of part
//make selector - fix broken part with that part

enum PartType
{
    Wheels,
    Engine,
    SteeringWheel,
    Seats,
    Windows,
    SomethingElseCarsHave  
}

public class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        RepairService repairService = new RepairService();
        repairService.Open();

        Helper.WaitForKeyPress();
    }
}

class RepairService
{
    private int _balance;
    private bool _isRunOutOfSpareParts;

    private Queue<Car> _cars;
    private Stack<Part> _spareParts;

    public RepairService()
    {
        StockUp();

        _balance = 0;
        _isRunOutOfSpareParts = false;

        _cars = CarFactory.CreateFew(amount: 4);
    }

    public void Open()
    {
        while (_cars.Count != 0 && _spareParts.Count > 0)
        {
            ShowHud();
            ShowCars();

            RepairCar(_cars.Dequeue());
        }

        ShowHud();
        ShowResultMessage();
    }

    public void RepairCar(Car car)
    {
        if (TryProceedFixing("My car is not working. Can you fix it?") == false)
        {
            PayFine(value: 10);
            return;
        }

        while (TryFindBrokenPartInCar(car, out int brokenPartIndex))
        {
            Helper.WriteTitle($"Repairing...");

            car.ShowInfo();

            if (_spareParts.Count <= 0)
            {
                _isRunOutOfSpareParts = true;
                Helper.WriteAt("No spare parts left.", foregroundColor: ConsoleColor.Red);
                Helper.WaitForKeyPress();
                return;
            }

            if (TryProceedFixing("So you found a broken part. Maybe you can fix it") == false)
            {
                Console.WriteLine($"Broken parts left: {car.BrokenParts}");

                int penaltyPerPart = 4;
                PayFine(value: car.BrokenParts * penaltyPerPart);
                return;
            }

            SwapPart(brokenPartIndex, car);
        }

        Helper.WriteAt("Car is in perfect condition", foregroundColor: ConsoleColor.DarkGreen);
        Helper.WaitForKeyPress(shouldClearAfter: true);
    }

    private void ShowResultMessage()
    {
        if (_balance > 0)
        {
            if (_isRunOutOfSpareParts == false)
            {
                Helper.WriteAt($"No more cars left. You win ({_balance}). Bye-bye", foregroundColor: ConsoleColor.Yellow);
            }
            else
            {
                Helper.WriteAt($"Closing the repair service :(. Hey, at least you got {_balance} usd dollars", foregroundColor: ConsoleColor.Gray);
            }
        }
        else
        {
            if (_isRunOutOfSpareParts == false)
            {
                Helper.WriteAt($"You are in debt ({_balance}) and alone", foregroundColor: ConsoleColor.Magenta);
            }
            else
            {
                Helper.WriteAt($"You are in debt ({_balance}), but at least you are not alone. Success?", foregroundColor: ConsoleColor.Gray);
            }
        }
    }

    private bool TryProceedFixing(string message)
    {
        Helper.WriteTitle("Negotiating a deal");

        ShowHud();

        Console.WriteLine(message);

        const int YesCommand = 1;
        const int NoCommand = 2;
        string yesCommandDescription = "i guess i can try";
        string noCommandDescription = "but i don't wanna";
        int userInput = Helper.ReadInt($"Whatcha you say? ({YesCommand} - {yesCommandDescription}, {NoCommand} - {noCommandDescription})");

        return userInput == 1;
    }

    private void PayFine(int value)
    {
        _balance -= value;
        ShowHud();
        Console.WriteLine($"Client left. You paid ${value} fine");

        Helper.WaitForKeyPress();
    }

    private void SwapPart(int brokenPartIndex, Car car)
    {
        Helper.WriteAt($"Replacing part at {brokenPartIndex}...");

        Part currentPart = _spareParts.Pop();
        int minDice = 1;
        int maxDice = 2;
        bool isFailedRepair = Helper.GetRandomInt(minDice, maxDice + 1) == 1;

        if (isFailedRepair)
        {
            Helper.WriteAt($"You failed the repair process T_T \n", foregroundColor: ConsoleColor.DarkRed);
            Helper.WaitForKeyPress();

            return;
        }

        int repairPrice = 10;

        car.ReplacePartAt(brokenPartIndex, currentPart);
        _balance += repairPrice + currentPart.Price;

        ShowHud();
        Helper.WriteAt($"Success. You got ${repairPrice} for work and ${currentPart.Price} for the part\n", foregroundColor: ConsoleColor.DarkGreen);
        Helper.WaitForKeyPress();
    }

    private bool TryFindBrokenPartInCar(Car car, out int brokenPartIndex)
    {
        brokenPartIndex = -1;

        for (int i = 0; i < car.Parts.Count; i++)
        {
            if (car.Parts[i].IsBroken)
            {
                brokenPartIndex = i;
                return true;
            }
        }

        return false;
    }

    private void ShowCars()
    {
        Helper.WriteTitle("Garage");

        foreach (Car car in _cars)
        {
            car.ShowInfo();
        }

        Helper.WaitForKeyPress(shouldClearAfter: true);
    }

    private void ShowHud()
    {
        int xPosition = 104;

        Helper.WriteAt($"Balance: ${_balance}", yPosition: 0, xPosition: xPosition);
        Helper.WriteAt($"Spare Parts: {_spareParts.Count}", yPosition: 1, xPosition: xPosition);
        Helper.WriteAt($"Cars: {_cars.Count}", yPosition: 2, xPosition: xPosition);
    }

    private void StockUp(int carsQuantity = 4, int uniquePartsQuantity = 3)
    {
        _spareParts = new Stack<Part>();

        for (int i = 0; i < uniquePartsQuantity; i++)
        {
            _spareParts.Push(PartFactory.Create(false));
        }
    }
    
    private void BuyParts(string name)
}

static class CarFactory
{
    public static Queue<Car> CreateFew(int amount = 1)
    {
        Queue<Car> newCars = new Queue<Car>();

        for (int i = 0; i < amount; i++)
        {
            newCars.Enqueue(Create());
        }

        return newCars;
    }

    public static Car Create()
    {
        Queue<PartType> partsNames = new Queue<PartType>();
        
        List<Part> parts = PartFactory.CreateFew(CreateNewCarSet());
        
        return new Car(parts);
    }

    private static Queue<PartType> CreateNewCarSet()
    {
        Queue<PartType> partTypes = new Queue<PartType>();
        
        partTypes.Enqueue(PartType.Wheels);
        partTypes.Enqueue(PartType.Engine);
        partTypes.Enqueue(PartType.SteeringWheel);
        partTypes.Enqueue(PartType.Seats);
        partTypes.Enqueue(PartType.Windows);
        partTypes.Enqueue(PartType.SomethingElseCarsHave);

        return partTypes;
    }
}

static class PartFactory
{
    public static List<Part> CreateFew(Queue<PartType> partTypes, int quantity = 6)
    {
        List<Part> parts = new List<Part>();

        int brokenPartValue = 0;
        int goodPartValue = 1;
        bool hasManufacturingDefect = false;
        
        for (int i = 0; i < quantity; i++)
        {
            bool isBrokenPart = Convert.ToBoolean(Helper.GetRandomInt(brokenPartValue, goodPartValue + 1));

            if (isBrokenPart && hasManufacturingDefect == false)
                hasManufacturingDefect = true;

            if (hasManufacturingDefect == false && quantity == i + 1)
                isBrokenPart = true;
            
            parts.Add(Create(partTypes.Dequeue(), isBrokenPart));
        }

        return parts;
    }

    public static Part Create(PartType type, bool isBroken, int? price = null)
    {
        if (price.HasValue == false)
        {
            int minPrice = 0;
            int maxPrice = 10;
            price = Helper.GetRandomInt(minPrice, maxPrice + 1);
        }

        return new Part(type, isBroken, price.Value);
    }
}

class Car
{
    private List<Part> _parts;

    private Guid _id;

    public Car(List<Part> parts)
    {
        _parts = parts;

        _id = Guid.NewGuid();
    }

    public int BrokenParts => CalculateBrokenParts();

    public IReadOnlyList<Part> Parts => _parts.AsReadOnly();

    public void ReplacePartAt(int index, Part newPart)
    {
        if (index < 0 || index >= _parts.Count)
            throw new ArgumentOutOfRangeException();

        _parts[index] = newPart;
    }

    public void ShowInfo()
    {
        Helper.WriteTitle($"Car {_id}", isSecondary: true);

        bool haveBrokenPart = false;

        for (int i = 0; i < _parts.Count; i++)
        {
            if (_parts[i].IsBroken)
            {
                Helper.WriteAt($"Broken part at ({i})", foregroundColor: ConsoleColor.DarkMagenta);
                haveBrokenPart = true;
            }
        }

        if (haveBrokenPart == false)
        {
            Helper.WriteAt("In good condition");
        }

        Console.WriteLine();
    }

    private int CalculateBrokenParts()
    {
        int brokenParts = 0;

        foreach (Part part in _parts)
        {
            if (part.IsBroken)
                brokenParts++;
        }

        return brokenParts;
    }
}

class Part
{
    public Part(PartType type, bool isBroken, int price)
    {
        Type = type;
        IsBroken = isBroken;
        Price = price;
    }

    public PartType Type { get; private set; }
    public int Price { get; private set; }
    public bool IsBroken { get; private set; }
}

class Helper
{
    private static readonly Random s_random = new Random();

    public static string ReadString(string helpText, ConsoleColor primary = ConsoleColor.Cyan, ConsoleColor secondary = ConsoleColor.Black)
    {
        ConsoleColor backgroundColor = Console.BackgroundColor;
        ConsoleColor foregroundColor = Console.ForegroundColor;

        WriteAt(helpText, foregroundColor: primary, isNewLine: true);

        int fieldStartY = Console.CursorTop;
        int fieldStartX = Console.CursorLeft;

        const char Space = ' ';
        WriteAt(new string(Space, helpText.Length - 1), backgroundColor: primary, isNewLine: false, xPosition: fieldStartX);

        Console.SetCursorPosition(fieldStartX + 1, fieldStartY);

        Console.BackgroundColor = primary;
        Console.ForegroundColor = secondary;

        string input = Console.ReadLine();

        Console.BackgroundColor = backgroundColor;
        Console.ForegroundColor = foregroundColor;

        Console.WriteLine();

        return input;
    }

    public static int ReadInt(string text)
    {
        int result;

        while (int.TryParse(ReadString(text), out result) == false) ;

        return result;
    }

    public static int GetRandomInt(int min, int max)
    {
        return s_random.Next(min, max);
    }

    public static void WriteTitle(string title, bool isSecondary = false)
    {
        ConsoleColor backgroundColor = isSecondary ? ConsoleColor.DarkGray : ConsoleColor.Gray;

        WriteAt($" {title} ", foregroundColor: ConsoleColor.Black, backgroundColor: backgroundColor);

        if (isSecondary == false)
            Console.WriteLine();
    }

    public static void WaitForKeyPress(bool shouldClearAfter = true)
    {
        WriteAt("Press any key", foregroundColor: ConsoleColor.Cyan);
        Console.ReadKey(true);

        if (shouldClearAfter)
            Console.Clear();
    }

    public static void WriteAt(object element, int? yPosition = null, int? xPosition = null,
        ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black,
        bool isNewLine = true)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        int yStart = Console.CursorTop;
        int xStart = Console.CursorLeft;

        bool isCustomPosition = yPosition.HasValue || xPosition.HasValue;

        if (isCustomPosition)
        {
            int targetY = yPosition ?? yStart;
            int targetX = xPosition ?? xStart;

            targetY = Math.Max(0, targetY);
            targetX = Math.Max(0, targetX);

            int safeYPosition = Math.Min(targetY, Console.WindowHeight - 1);
            int safeXPosition = Math.Min(targetX, Console.WindowWidth - 1);

            Console.SetCursorPosition(safeXPosition, safeYPosition);
        }

        if (isNewLine)
            Console.WriteLine(element);
        else
            Console.Write(element);

        Console.ResetColor();

        if (isCustomPosition)
        {
            Console.CursorTop = yStart;
            Console.CursorLeft = xStart;
        }
    }
}
