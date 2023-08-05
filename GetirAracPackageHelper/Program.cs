using GetirAracPackageHelper;
using System.Text.Json;

Car[]? cars = InitializeCars();

if (cars == null || cars.Length <= 0)
{
    Console.WriteLine("There are no cars.");
    return;
}

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

Console.WriteLine("");
Console.WriteLine($"The cheapest top 3 packages for {cars[carIndex].PricePerMinute}TL/min range car, {estimatedKms}km, {estimatedMinutes}min, {discount}TL are:");
totalPrices.OrderBy(f => f.totalPrice).Take(3).Select(f => $"{f.totalPrice}TL\t=> Free Mins:{f.package.FreeMinutes}min\tFree Kms:{f.package.FreeKilometers}km\tPrice:{f.package.Price}TL{(f.package.FreeMinutes == 0 ? "\t(No package)" : null)}").ToList().ForEach(Console.WriteLine);

Console.ReadKey();

static Car[]? InitializeCars()
{
    string jsonFilePath = "cars.json";
    string jsonContent = File.ReadAllText(jsonFilePath);
    return JsonSerializer.Deserialize<Car[]?>(jsonContent);
}