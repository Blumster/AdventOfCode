using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var input = lines[0];

var inputRegex = new Regex(@"^target area: x=([-\d]+)\.\.([-\d]+), y=([-\d]+)\.\.([-\d]+)$");

var m = inputRegex.Match(input);

var x1 = int.Parse(m.Groups[1].Value);
var x2 = int.Parse(m.Groups[2].Value);
var y1 = int.Parse(m.Groups[3].Value);
var y2 = int.Parse(m.Groups[4].Value);

var target = new Box(new Point(Math.Min(x1, x2), Math.Max(y1, y2)), new Point(Math.Max(x1, x2), Math.Min(y1, y2)));
var start = new Point(0, 0);

var validXVelocities = new List<int>();

var xMin = target.TopLeft.X - start.X;
var xMax = target.BottomRight.X - start.X;
var biggestXDistance = xMin < start.X ? xMin : xMax;

var projectile = new Projectile(start, 0, 0);

var i = 0;
var inc = 0;
var end = 0;

if (target.TopLeft.X <= start.X && start.X <= target.BottomRight.X)
{
	i = target.TopLeft.X - start.X;
	inc = 1;
	end = target.BottomRight.X - start.X;
}
else if (start.X > target.BottomRight.X)
{
	i = 0;
	inc = -1;
	end = target.TopLeft.X - start.X;
}
else if (start.X < target.TopLeft.X)
{
	i = 0;
	inc = 1;
	end = target.BottomRight.X - start.X;
}

for (; i <= end; i += inc)
{
	projectile.Coord = start;
	projectile.XVelocity = i;

	for (var j = 0; j < 100; ++j)
	{
		projectile.Step();

		if (projectile.IsInBoxByX(target))
		{
			validXVelocities.Add(i);
			break;
		}
	}
}

var shots = new Dictionary<(int XVelocity, int YVelocity), int>();

foreach (var xVelocity in validXVelocities)
{
	for (var yVelocity = 1000; yVelocity != -1000; --yVelocity)
    {
		projectile.Coord = start;
		projectile.HighestYCoord = start.Y;
		projectile.XVelocity = xVelocity;
		projectile.YVelocity = yVelocity;

		var wasInBox = false;

		for (var j = 0; j < 1000; ++j)
		{
			projectile.Step();

			if (projectile.IsInBox(target))
			{
				wasInBox = true;
				break;
			}
		}

		if (wasInBox)
			shots.Add((xVelocity, yVelocity), projectile.HighestYCoord);
	}
}

Console.WriteLine($"Task1: highest y position: {shots.Select(s => s.Value).Max()}");

Console.WriteLine($"Task2: distinct shot velocities: {shots.Select(s => s.Key).Distinct().Count()}");

struct Point
{
	public int X { get; set; }
	public int Y { get; set; }

	public Point(int x, int y)
    {
		X = x;
		Y = y;
    }
}
record Box(Point TopLeft, Point BottomRight)
{
	public bool IsIn(Point point)
	{
		return IsInX(point) && TopLeft.Y >= point.Y && point.Y >= BottomRight.Y;
	}

	public bool IsInX(Point point)
	{
		return TopLeft.X <= point.X && point.X <= BottomRight.X;
	}

	public Point GetMiddlePoint()
	{
		return new Point(TopLeft.X + Math.Abs(BottomRight.X - TopLeft.X) / 2, TopLeft.Y + Math.Abs(TopLeft.Y - BottomRight.Y) / 2);
	}
}

class Projectile
{
	public Point Coord { get; set; }
	public int HighestYCoord { get; set; }
	public int XVelocity { get; set; }
	public int YVelocity { get; set; }
	
	public Projectile(Point startPoint, int startXVelocity, int startYVelocity)
    {
		Coord = startPoint;
		HighestYCoord = Coord.Y;
		XVelocity = startXVelocity;
		YVelocity = startYVelocity;
    }

	public bool IsInBox(Box box)
    {
		return box.IsIn(Coord);
    }

	public bool IsInBoxByX(Box box)
	{
		return box.IsInX(Coord);
	}

	public void Step()
    {
		Coord = new Point(Coord.X + XVelocity, Coord.Y + YVelocity);

		if (Coord.Y > HighestYCoord)
			HighestYCoord = Coord.Y;

		if (XVelocity > 0)
			XVelocity -= 1;
		else if (XVelocity < 0)
			XVelocity += 1;

		--YVelocity;
    }
}
