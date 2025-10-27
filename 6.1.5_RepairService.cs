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
    SomethingElseCarsHave,
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

    private readonly Queue<Car> _cars;
    private readonly PartsStorage _spareParts;

    private readonly PartFactory _partFactory;

    public RepairService()
    {
        _partFactory = new PartFactory();
        CarFactory carFactory = new(_partFactory);

        _spareParts = new PartsStorage();
        Restock();

        _balance = 0;

        _cars = carFactory.CreateFew(4);
    }

    private static bool TryCompleteReplacement()
    {
        const int MinDice = 1;
        const int MaxDice = 2;

        bool isSuccessful = Helper.GetRandomInt(MinDice, MaxDice + 1) == 2;

        if (isSuccessful)
            return true;

        Helper.WriteAt($"You failed the replacement process T_T \n", foregroundColor: ConsoleColor.DarkRed);
        Helper.WaitForKeyPress();

        return false;
    }

    public void Open()
    {
        while (_cars.Count != 0 && _spareParts.HasParts)
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
        if (TryProceedFixing($"My {car.Info} is not working ({car.Parts.BrokenPartsCount}). Can you fix it?",
                "Negotiating with a client") == false)
        {
            PayFine(true);
            return;
        }

        Console.Clear();

        while (_spareParts.HasParts && car.Parts.TryGetBrokenPartIndex(out int? brokenPartIndex, out PartType? brokenPartType))
        {
            Helper.WriteTitle($"Repairing broken {car.Info} ({car.Parts.BrokenPartsCount} total broken parts)");

            if (TryProceedFixing(
                    $"So you found broken {brokenPartType}. " +
                    $"Maybe you can fix it",
                    $"Thinking if you are in the mood to repair some {brokenPartType} today") == false)
            {
                PayFine(false, car.Parts.BrokenPartsCount);
                return;
            }

            if (brokenPartType == null)
                return;

            if (_spareParts.TryGetPart(brokenPartType.Value, out Part? replacementPart) == false)
            {
                PayFine(false, car.Parts.BrokenPartsCount);

                Helper.WriteAt($"You have no spare parts to fix this car.", foregroundColor: ConsoleColor.Red);
                Helper.WaitForKeyPress();

                return;
            }

            if (replacementPart != null && brokenPartIndex != null)
                SwapPart(brokenPartIndex.Value, car, replacementPart);
        }
    }

    private void ShowResultMessage()
    {
        if (_balance > 0)
        {
            if (_cars.Count == 0)
                Helper.WriteAt($"No cars left. You win {_balance} usd dollars. Bye-bye",
                    foregroundColor: ConsoleColor.Yellow);
            else
                Helper.WriteAt(
                    $"Closing the repair service :(. Hey, at least you got {_balance} usd dollars",
                    foregroundColor: ConsoleColor.Gray);
        }
        else
        {
            if (_cars.Count == 0)
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
        int userInput = Helper.ReadInt(
            $"What you say? ({YesCommand} - {YesCommandDescription}, {NoCommand} - {NoCommandDescription})");

        return userInput == 1;
    }

    private void PayFine(bool isStaticFine, int brokenPartsQuantity = 1)
    {
        const int PenaltyPerPart = 4;
        int fineValue = isStaticFine ? 10 : PenaltyPerPart * brokenPartsQuantity;

        _balance -= fineValue;
        ShowHud();

        if (isStaticFine == false)
            Console.WriteLine($"Broken parts left in this car: {brokenPartsQuantity}");

        Console.WriteLine($"Client left. You paid ${fineValue} fine");

        Helper.WaitForKeyPress();
    }

    private void SwapPart(int brokenPartIndex, Car car, Part replacementPart)
    {
        Helper.WriteAt($"Replacing {replacementPart.Type}...");

        if (TryCompleteReplacement() == false || car.Parts.TryReplaceWithSameTypePart(brokenPartIndex, replacementPart) == false)
            return;

        const int RepairPrice = 10;

        _balance += RepairPrice + replacementPart.Price;

        ShowHud();
        Helper.WriteAt($"Success. You got ${RepairPrice} for work and ${replacementPart.Price} for the part\n",
            foregroundColor: ConsoleColor.DarkGreen);
        Helper.WaitForKeyPress();
    }

    private void ShowCars()
    {
        Helper.WriteTitle("Garage");

        foreach (Car car in _cars)
            car.ShowInfo();

        Helper.WaitForKeyPress();
    }

    private void ShowHud()
    {
        int xPosition = Console.WindowWidth - 32;

        Helper.WriteAt($"Balance: ${_balance}", 0, xPosition);
        Helper.WriteAt($"Cars: {_cars.Count}", 2, xPosition);

        _spareParts.ShowByType(4, xPosition, "Spare parts in stock: ");
    }

    private void Restock(int eachPartTypeQuantity = 3)
    {
        foreach (PartType partType in Enum.GetValues<PartType>())
        {
            for (int i = 0; i < eachPartTypeQuantity; i++)
            {
                Part part = _partFactory.Create(partType, false);
                _spareParts.AddPart(part);
            }
        }
    }
}

internal class CarFactory
{
    private readonly List<string> _carManufacturer;
    private readonly List<string> _carType;

    private readonly PartFactory _partFactory;

    private int _manufacturerIndex;
    private int _carTypeIndex;

    public CarFactory(PartFactory partFactory)
    {
        _partFactory = partFactory ?? throw new ArgumentNullException(nameof(partFactory));

        _carType =
        [
            "Sedan", "Hatchback", "Crossover", "Pickup", "Truck", "Coupe", "Roadster", "Wagon", "Van", "SUV"
        ];

        _carManufacturer =
        [
            "Nissan", "Toyota", "Honda", "Ford", "Chevrolet", "Hyundai", "Kia", "Mazda", "Volkswagen", "BMW", "Mercedes-Benz"
        ];
    }

    public Queue<Car> CreateFew(int amount = 1)
    {
        Queue<Car> newCars = [];

        for (int i = 0; i < amount; i++)
        {
            newCars.Enqueue(Create());
            _manufacturerIndex = (_manufacturerIndex + 1) % _carManufacturer.Count;
            _carTypeIndex = (_carTypeIndex + 1) % _carType.Count;
        }

        return newCars;
    }

    private Car Create()
    {
        List<Part> parts = _partFactory.CreateFew(CreateNewCarSet());
        PartsStorage carParts = new(parts);

        KeyValuePair<string, string> carInfo = new(_carType[_carTypeIndex], _carManufacturer[_manufacturerIndex]);

        return new Car(carParts, carInfo);
    }

    private static Queue<PartType> CreateNewCarSet()
    {
        List<PartType> partTypes = Enum.GetValues<PartType>().ToList();
        return new Queue<PartType>(partTypes);
    }
}

internal class PartFactory
{
    public List<Part> CreateFew(Queue<PartType> partTypes)
    {
        ArgumentNullException.ThrowIfNull(partTypes);

        int quantity = partTypes.Count;

        if (quantity == 0)
            return [];

        const int BrokenPartValue = 0;
        const int GoodPartValue = 1;

        Queue<PartType> partTypesCopy = new(partTypes);
        List<Part> parts = [];

        bool hasAtLeastOneBroken = false;

        for (int i = 0; i < quantity; i++)
        {
            bool isBrokenPart = Convert.ToBoolean(Helper.GetRandomInt(BrokenPartValue, GoodPartValue + 1));

            if (isBrokenPart && hasAtLeastOneBroken == false)
                hasAtLeastOneBroken = true;

            if (hasAtLeastOneBroken == false && i == quantity - 1)
                isBrokenPart = true;

            parts.Add(Create(partTypesCopy.Dequeue(), isBrokenPart));
        }

        return parts;
    }

    public Part Create(PartType type, bool isBroken, int? price = null)
    {
        if (price.HasValue)
            return new Part(type, isBroken, price.Value);

        const int MinPrice = 0;
        const int MaxPrice = 10;
        price = Helper.GetRandomInt(MinPrice, MaxPrice + 1);

        return new Part(type, isBroken, price.Value);
    }
}

internal class PartsStorage
{
    private readonly List<Part> _parts;

    public PartsStorage(List<Part> parts)
    {
        _parts = parts;
    }

    public PartsStorage()
    {
        _parts = [];
    }

    public bool HasParts => _parts.Count > 0;
    public int PartsCount => _parts.Count;

    public int BrokenPartsCount => CountBrokenParts();

    public void ShowByType(int? yPosition, int? xPosition, string title = "Parts")
    {
        Dictionary<PartType, int> partsCounter = CountPartsByType();

        Helper.WriteAt(title, yPosition, xPosition);

        foreach (KeyValuePair<PartType, int> keyValuePair in partsCounter)
        {
            yPosition++;
            Helper.WriteAt($"{keyValuePair.Key} - {keyValuePair.Value}", yPosition, xPosition);
        }
    }

    public void ShowAll(string title)
    {
        Helper.WriteTitle(title, true);

        bool haveBrokenPart = false;

        for (int i = 0; i < PartsCount; i++)
        {
            ConsoleColor color = _parts[i].IsBroken ? ConsoleColor.DarkMagenta : ConsoleColor.White;

            Helper.WriteAt($"{_parts[i].Type}",
                foregroundColor: color);

            if (_parts[i].IsBroken)
                haveBrokenPart = true;
        }

        if (haveBrokenPart == false)
            Helper.WriteAt("No broken parts found");

        Console.WriteLine();
    }

    public bool TryReplaceWithSameTypePart(int index, Part newPart)
    {
        if (newPart.Type != _parts[index].Type)
        {
            Helper.WriteAt("Part type doesn't match. Replacement was canceled",
                foregroundColor: ConsoleColor.DarkRed);
            Helper.WaitForKeyPress();

            return false;
        }

        TryReplacePartAt(index, newPart);
        return true;
    }

    public bool TryReplacePartAt(int index, Part newPart)
    {
        if (index < 0 || index >= _parts.Count)
            return false;

        _parts[index] = newPart;
        return true;
    }

    public bool TryGetBrokenPartIndex(out int? brokenPartIndex, out PartType? brokenPartType)
    {
        brokenPartIndex = null;
        brokenPartType = null;

        for (int i = 0; i < PartsCount; i++)
        {
            if (_parts[i].IsBroken == false)
                continue;

            brokenPartType = _parts[i].Type;
            brokenPartIndex = i;
            return true;
        }

        Helper.WriteAt("All parts in good condition", foregroundColor: ConsoleColor.DarkGreen);
        Helper.WaitForKeyPress();
        return false;
    }

    public bool TryGetPart(PartType type, out Part? part)
    {
        part = null;

        if (HasParts == false)
            return false;

        for (int i = 0; i < _parts.Count; i++)
        {
            if (_parts[i].Type != type)
                continue;

            Helper.WriteAt($"{type} found.\n",
                foregroundColor: ConsoleColor.DarkGreen);
            Helper.WaitForKeyPress();

            part = _parts[i];
            _parts.RemoveAt(i);
            return true;
        }

        Helper.WriteAt($"No {type} left.\n",
            foregroundColor: ConsoleColor.DarkRed);
        Helper.WaitForKeyPress();
        return false;
    }

    public void AddPart(Part part)
    {
        ArgumentNullException.ThrowIfNull(part);

        _parts.Add(part);
    }

    private Dictionary<PartType, int> CountPartsByType()
    {
        Dictionary<PartType, int> partsCounter = new();

        foreach (Part part in _parts)
        {
            if (partsCounter.ContainsKey(part.Type))
                partsCounter[part.Type]++;
            else
                partsCounter[part.Type] = 1;
        }

        return partsCounter;
    }

    private int CountBrokenParts()
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

internal class Car
{
    public readonly PartsStorage Parts;
    public readonly KeyValuePair<string, string> Info;

    public Car(PartsStorage parts, KeyValuePair<string, string> info)
    {
        Parts = parts;
        Info = info;
    }

    public void ShowInfo()
    {
        Parts.ShowAll($"{Info}");
    }
}
