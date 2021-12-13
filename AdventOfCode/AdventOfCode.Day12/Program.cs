var lines = File.ReadAllLines("input.txt");

var availableRoutes = new Dictionary<string, HashSet<string>>();

foreach (var line in lines)
{
    var parts = line.Split('-', 2);

    var from = parts[0];
    var to = parts[1];

    if (availableRoutes.ContainsKey(from))
    {
        availableRoutes[from].Add(to);
    }
    else
    {
        availableRoutes.Add(from, new() { to });
    }

    if (from == "start" || to == "end")
        continue;

    if (availableRoutes.ContainsKey(to))
    {
        availableRoutes[to].Add(from);
    }
    else
    {
        availableRoutes.Add(to, new() { from });
    }
}

var routes = new HashSet<string>();

void CalcRoutesTask1(string from, List<string>? takenRoute = null)
{
    if (takenRoute == null)
        takenRoute = new List<string>();

    takenRoute.Add(from);

    if (from == "end")
    {
        routes.Add(takenRoute.Aggregate((a, b) => $"{a},{b}"));
        return;
    }

    foreach (var route in availableRoutes[from])
    {
        var expectedLength = takenRoute.Count;

        if (route == route.ToLowerInvariant() && takenRoute.Contains(route))
            continue;

        CalcRoutesTask1(route, takenRoute);

        if (takenRoute.Count > expectedLength)
        {
            takenRoute.RemoveRange(expectedLength, takenRoute.Count - expectedLength);
        }
    }
}

CalcRoutesTask1("start");

Console.WriteLine($"Task1: route count: {routes.Count}");

bool IsAnySmallCaveVisitedTwice(List<string> route)
{
    return route.Where(r => r == r.ToLowerInvariant()).GroupBy(r => r).Any(c => c.Count() == 2);
}

routes = new HashSet<string>();

void CalcRoutesTask2(string from, List<string>? takenRoute = null)
{
    if (takenRoute == null)
        takenRoute = new List<string>();

    takenRoute.Add(from);

    if (from == "end")
    {
        routes!.Add(takenRoute.Aggregate((a, b) => $"{a},{b}"));
        return;
    }

    foreach (var route in availableRoutes[from])
    {
        var expectedLength = takenRoute.Count;

        if (route == route.ToLowerInvariant() && IsAnySmallCaveVisitedTwice(takenRoute) && takenRoute.Contains(route))
            continue;

        CalcRoutesTask2(route, takenRoute);

        if (takenRoute.Count > expectedLength)
        {
            takenRoute.RemoveRange(expectedLength, takenRoute.Count - expectedLength);
        }
    }
}

CalcRoutesTask2("start");

Console.WriteLine($"Task2: route count: {routes.Count}");
