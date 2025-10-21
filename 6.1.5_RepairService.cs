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

internal enum PartType
{
    Wheels,
    Engine,
    SteeringWheel,  
    Seats,
    Windows,
    SomethingElseCarsHave
}

public class RepairServiceTask
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        RepairService repairService = new();
        repairService.Open();

        Helper.WaitForKeyPress();
    }
}

internal class RepairService
{
    private int _balance;
    private bool _isRunOutOfSpareParts;

    private readonly Queue<Car> _cars;
    private readonly List<Part> _spareParts;

    public RepairService()
    {
        _spareParts = [];

        StockUp();

        _balance = 0;
        _isRunOutOfSpareParts = false;

        _cars = CarFactory.CreateFew(4);
    }

    private static bool TryReplace()
    {
        const int MinDice = 1;
        const int MaxDice = 2;

        bool isSuccessful = Helper.GetRandomInt(MinDice, MaxDice + 1) == 2;

        if (isSuccessful) return true;
        
        Helper.WriteAt($"You failed the replacement process T_T \n",
            foregroundColor: ConsoleColor.DarkRed);
        Helper.WaitForKeyPress();

        return false;
    }

    private static bool TryFindBrokenPartInCar(Car car, out int brokenPartIndex)
    {
        brokenPartIndex = -1;

        for (int i = 0; i < car.Parts.Count; i++)
        {
            if (car.Parts[i].IsBroken == false) continue;

            brokenPartIndex = i;
            return true;
        }

        return false;
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

    private void RepairCar(Car car)
    {
        if (TryProceedFixing(
                $"My car is not working ({car.BrokenPartsQuantity}). Can you fix it?",
                "Negotiating with a client") == false)
        {
            PayFine(true);
            return;
        }

        Console.Clear();
        
        while (_isRunOutOfSpareParts == false && TryFindBrokenPartInCar(car, out int brokenPartIndex))
        {
            PartType brokenPartType = car.Parts[brokenPartIndex].Type;

            Helper.WriteTitle($"Repairing a broken part at {brokenPartIndex}");

            if (TryProceedFixing(
                    $"So you found broken {brokenPartType} ({car.BrokenPartsQuantity} total broken parts left). Maybe you can fix it",
                    $"Thinking if you are in the mood to repair some {brokenPartType} today") ==
                false)
            {
                PayFine(false, car.BrokenPartsQuantity);
                return;
            }

            if (TryFindReplacementPart(brokenPartType, out Part? replacementPart) == false)
            {
                _isRunOutOfSpareParts = _spareParts.Count <= 0;
                PayFine(false, car.BrokenPartsQuantity);

                if (_isRunOutOfSpareParts == false) return;

                Helper.WriteAt($"You are out of the spare parts.",
                    foregroundColor: ConsoleColor.Red);
                Helper.WaitForKeyPress();
            }

            if (replacementPart != null)
                SwapPart(brokenPartIndex, car, replacementPart);
        }

        Helper.WriteAt("Car is in perfect condition",
            foregroundColor: ConsoleColor.DarkGreen);
        Helper.WaitForKeyPress();
    }

    private void ShowResultMessage()
    {
        if (_balance > 0)
        {
            if (_isRunOutOfSpareParts == false)
                Helper.WriteAt(
                    $"No more cars left. You win ({_balance}). Bye-bye",
                    foregroundColor: ConsoleColor.Yellow);
            else
                Helper.WriteAt(
                    $"Closing the repair service :(. Hey, at least you got {_balance} usd dollars",
                    foregroundColor: ConsoleColor.Gray);
        }
        else
        {
            if (_isRunOutOfSpareParts == false)
                Helper.WriteAt($"You are in debt ({_balance}) and alone",
                    foregroundColor: ConsoleColor.Magenta);
            else
                Helper.WriteAt(
                    $"You are in debt ({_balance}), but at least you are not alone. Success?",
                    foregroundColor: ConsoleColor.Gray);
        }
    }

    private bool TryProceedFixing(string message, string title)
    {
        Helper.WriteTitle(title);

        ShowHud();

        Console.WriteLine(message);

        const int YesCommand = 1;
        const int NoCommand = 2;
        const string YesCommandDescription = "i guess i can try";
        const string NoCommandDescription = "but i don't wanna";
        int userInput =
            Helper.ReadInt(
                $"What you say? ({YesCommand} - {YesCommandDescription}, {NoCommand} - {NoCommandDescription})");

        return userInput == 1;
    }

    private void PayFine(bool isStaticFine, int brokenPartsQuantity = 1)
    {
        const int PenaltyPerPart = 4;
        int fineValue =
            isStaticFine ? 10 : PenaltyPerPart * brokenPartsQuantity;

        _balance -= fineValue;
        ShowHud();

        if (isStaticFine == false)
            Console.WriteLine(
                $"Broken parts left in this car: {brokenPartsQuantity}");

        Console.WriteLine($"Client left. You paid ${fineValue} fine");

        Helper.WaitForKeyPress();
    }

    private void SwapPart(int brokenPartIndex, Car car, Part replacementPart)
    {
        PartType brokenPartType = car.Parts[brokenPartIndex].Type;

        Helper.WriteAt($"Replacing {brokenPartType}...");

        if (TryReplace() == false || car.TryReplacePartAt(brokenPartIndex, replacementPart) == false) return;

        const int RepairPrice = 10;

        _balance += RepairPrice + replacementPart.Price;

        ShowHud();
        Helper.WriteAt(
            $"Success. You got ${RepairPrice} for work and ${replacementPart.Price} for the part\n",
            foregroundColor: ConsoleColor.DarkGreen);
        Helper.WaitForKeyPress();
    }

    private bool TryFindReplacementPart(PartType type, out Part? replacementPart)
    {
        replacementPart = null;

        foreach (Part part in _spareParts)
        {
            if (part.Type != type)
                continue;

            Helper.WriteAt(
                $"You still have some {type} in your inventory. That's good\n",
                foregroundColor: ConsoleColor.DarkGreen);
            Helper.WaitForKeyPress();

            replacementPart = part;
            _spareParts.Remove(replacementPart);

            return true;
        }

        Helper.WriteAt(
            $"No replacement {type} found in your inventory. Aborting repair process\n",
            foregroundColor: ConsoleColor.DarkRed);
        Helper.WaitForKeyPress();

        return false;
    }

    private void ShowCars()
    {
        Helper.WriteTitle("Garage");

        foreach (Car car in _cars) car.ShowInfo();

        Helper.WaitForKeyPress();
    }

    private void ShowHud()
    { 
        int xPosition = Console.WindowWidth - 32;;

        Helper.WriteAt($"Balance: ${_balance}", 0, xPosition);
        Helper.WriteAt($"Cars: {_cars.Count}", 2, xPosition);
        ShowSpareParts(4, xPosition);
    }

    private void ShowSpareParts(int yPosition, int xPosition)
    {
        int wheelsCount = 0;
        int engineCount = 0;
        int steeringWheelCount = 0;
        int seatsCount = 0;
        int windowsCount = 0;
        int somethingElseCount = 0;

        foreach (Part part in _spareParts)
        {
            switch (part.Type)
            {
                case PartType.Wheels:
                    wheelsCount++;
                    break;
                case PartType.Engine:
                    engineCount++;
                    break;
                case PartType.SteeringWheel:
                    steeringWheelCount++;
                    break;
                case PartType.Seats:
                    seatsCount++;
                    break;
                case PartType.Windows:
                    windowsCount++;
                    break;
                case PartType.SomethingElseCarsHave:
                    somethingElseCount++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        Helper.WriteAt("Spare parts in stock: ", yPosition, xPosition);
        Helper.WriteAt($"Wheels - {wheelsCount}", yPosition + 1, xPosition);
        Helper.WriteAt($"Engine - {engineCount}", yPosition + 2, xPosition);
        Helper.WriteAt($"Steering Wheel - {steeringWheelCount}", yPosition + 3,
            xPosition);
        Helper.WriteAt($"Seats - {seatsCount}", yPosition + 4, xPosition);
        Helper.WriteAt($"Windows - {windowsCount}", yPosition + 5, xPosition);
        Helper.WriteAt($"Something Else Cars Have - {somethingElseCount}",
            yPosition + 6, xPosition);
    }

    private void StockUp()
    {
        foreach (PartType partType in Enum.GetValues(typeof(PartType)))
        {
            const int Quantity = 3;

            for (int i = 0; i < Quantity; i++)
                _spareParts.Add(PartFactory.Create(partType, false));
        }
    }
}

internal static class CarFactory
{
    public static Queue<Car> CreateFew(int amount = 1)
    {
        Queue<Car> newCars = [];

        for (int i = 0; i < amount; i++) newCars.Enqueue(Create());

        return newCars;
    }

    private static Car Create()
    {
        List<Part> parts = PartFactory.CreateFew(CreateNewCarSet());

        return new Car(parts);
    }

    private static Queue<PartType> CreateNewCarSet()
    {
        Queue<PartType> partTypes = new();

        partTypes.Enqueue(PartType.Wheels);
        partTypes.Enqueue(PartType.Engine);
        partTypes.Enqueue(PartType.SteeringWheel);
        partTypes.Enqueue(PartType.Seats);
        partTypes.Enqueue(PartType.Windows);
        partTypes.Enqueue(PartType.SomethingElseCarsHave);

        return partTypes;
    }
}

internal static class PartFactory
{
    public static List<Part> CreateFew(Queue<PartType> partTypes, int quantity = 6)
    {
        List<Part> parts = [];

        const int BrokenPartValue = 0;
        const int GoodPartValue = 1;
        bool hasManufacturingDefect = false;

        for (int i = 0; i < quantity; i++)
        {
            bool isBrokenPart =
                Convert.ToBoolean(Helper.GetRandomInt(BrokenPartValue,
                    GoodPartValue + 1));

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
        if (price.HasValue) return new Part(type, isBroken, price.Value);

        const int MinPrice = 0;
        const int MaxPrice = 10;
        price = Helper.GetRandomInt(MinPrice, MaxPrice + 1);

        return new Part(type, isBroken, price.Value);
    }
}

internal class Car
{
    private readonly List<Part> _parts;

    private readonly Guid _id;

    public Car(List<Part> parts)
    {
        _parts = parts;

        _id = Guid.NewGuid();
    }

    public int BrokenPartsQuantity => CalculateBrokenParts();

    public IReadOnlyList<Part> Parts => _parts.AsReadOnly();

    public bool TryReplacePartAt(int index, Part newPart)
    {
        if (index < 0 || index >= _parts.Count)
            throw new ArgumentOutOfRangeException();

        if (newPart.Type == _parts[index].Type)
        {
            _parts[index] = newPart;
            return true;
        }

        Helper.WriteAt("Part type doesn't match. Replacement was canceled",
            foregroundColor: ConsoleColor.DarkRed);

        return false;
    }

    public void ShowInfo()
    {
        Helper.WriteTitle($"Car {_id}", true);

        bool haveBrokenPart = false;

        for (int i = 0; i < _parts.Count; i++)
        {
            if (_parts[i].IsBroken == false) continue;

            Helper.WriteAt($"Broken part at ({i})",
                foregroundColor: ConsoleColor.DarkMagenta);
            haveBrokenPart = true;
        }

        if (haveBrokenPart == false) Helper.WriteAt("In good condition");

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

internal class Part
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

internal static class Helper
{
    private static readonly Random s_random = new();

    private static string? ReadString(string helpText,
        ConsoleColor primary = ConsoleColor.Cyan,
        ConsoleColor secondary = ConsoleColor.Black)
    {
        ConsoleColor backgroundColor = Console.BackgroundColor;
        ConsoleColor foregroundColor = Console.ForegroundColor;

        WriteAt(helpText, foregroundColor: primary, isNewLine: true);

        int fieldStartY = Console.CursorTop;
        int fieldStartX = Console.CursorLeft;

        const char Space = ' ';
        WriteAt(new string(Space, helpText.Length - 1),
            backgroundColor: primary, isNewLine: false,
            xPosition: fieldStartX);

        Console.SetCursorPosition(fieldStartX + 1, fieldStartY);

        Console.BackgroundColor = primary;
        Console.ForegroundColor = secondary;

        string? input = Console.ReadLine();

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
        ConsoleColor backgroundColor =
            isSecondary ? ConsoleColor.DarkGray : ConsoleColor.Gray;

        WriteAt($" {title} ", foregroundColor: ConsoleColor.Black,
            backgroundColor: backgroundColor);

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

    public static void WriteAt(object element, int? yPosition = null,
        int? xPosition = null,
        ConsoleColor foregroundColor = ConsoleColor.White,
        ConsoleColor backgroundColor = ConsoleColor.Black,
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
