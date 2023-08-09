using GetirAracPackageHelper;
using System.Text.Json;

const int TableWidth = 73;
Car[]? cars = InitializeCars();

if (cars == null || cars.Length <= 0)
{
    Console.WriteLine("There are no cars.");
    return;
}

while (true)
{
    cars.Select((car, i) => $"{i}. {car.PricePerMinute}TL/min range car").ToList().ForEach(Console.WriteLine);
    int carIndex = -1;
    while (carIndex < 0 || carIndex >= cars.Length)
    {
        Console.Write("Please select a car: ");
        carIndex = Convert.ToInt32(Console.ReadLine());
    }

    int estimatedKms = -1;
    while (estimatedKms < 0)
    {
        Console.Write("Estimated distance in kilometers: ");
        estimatedKms = Convert.ToInt32(Console.ReadLine());
    }

    int estimatedMinutes = -1;
    while (estimatedMinutes < 0)
    {
        Console.Write("Estimated duration in minutes: ");
        estimatedMinutes = Convert.ToInt32(Console.ReadLine());
    }

    decimal discount = -1;
    while (discount < 0)
    {
        Console.Write("Discount in TL: ");
        discount = Convert.ToInt32(Console.ReadLine());
    }

    List<(Package package, decimal totalPrice)> totalPrices = new();

    for (int i = 0; i < cars[carIndex].Packages.Length; i++)
    {
        Package package = cars[carIndex].Packages[i];
        int minutesToPay = Math.Max(estimatedMinutes - package.FreeMinutes, 0);
        int kilometersToPay = Math.Max(estimatedKms - package.FreeKilometers, 0);
        decimal totalPrice = (minutesToPay * cars[carIndex].PricePerMinute) + (kilometersToPay * cars[carIndex].PricePerKilometer) + package.Price - discount;
        totalPrices.Add((package, totalPrice));
    }

    Console.WriteLine($"\nThe cheapest top 3 packages for {cars[carIndex].PricePerMinute}TL/min range car, {estimatedKms}km, {estimatedMinutes}min, {discount}TL are:");
    DrawTable(totalPrices.OrderBy(f => f.totalPrice).Take(3));

    Console.WriteLine("\nPress any key to calculate again...\n");
    _ = Console.ReadKey();
}

static void DrawTable(IEnumerable<(Package package, decimal totalPrice)> items)
{
    PrintLine();
    PrintRow("Total Price", "Free Mins", "Free Kms", "Price");
    PrintLine();
    foreach ((Package package, decimal totalPrice) in items)
    {
        PrintRow($"{totalPrice}TL", $"{package.FreeMinutes}min", $"{package.FreeKilometers}km", $"{package.Price}TL{(package.FreeMinutes == 0 ? " (No package)" : null)}");
    }

    PrintLine();
}

static Car[]? InitializeCars()
{
    string jsonFilePath = "cars.json";
    string jsonContent = File.ReadAllText(jsonFilePath);
    return JsonSerializer.Deserialize<Car[]?>(jsonContent);
}

static void PrintLine()
{
    Console.WriteLine(new string('-', TableWidth));
}

static void PrintRow(params string[] columns)
{
    int width = (TableWidth - columns.Length) / columns.Length;
    string row = "|";

    foreach (string column in columns)
    {
        row += AlignCentre(column, width) + "|";
    }

    Console.WriteLine(row);
}

static string AlignCentre(string text, int width)
{
    text = text.Length > width ? text[..(width - 3)] + "..." : text;

    return string.IsNullOrEmpty(text) ? new string(' ', width) : text.PadRight(width - ((width - text.Length) / 2)).PadLeft(width);
}