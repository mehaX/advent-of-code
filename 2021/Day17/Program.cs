
var TargetX = new[] { 230, 283 };
var TargetY = new[] { -107, -57 };

static bool Targets(int velocityX, int velocityY, int[] targetX, int[] targetY, out List<(int x, int y)> positions)
{
    var posX = 0;
    var posY = 0;
    positions = new() { (posX, posY) };

    while (posX < targetX.Max() && posY > targetY.Min())
    {
        posX += velocityX;
        posY += velocityY;
        positions.Add((posX, posY));

        if (posX >= targetX[0] && posX <= targetX[1]
                               && posY >= targetY[0] && posY <= targetY[1])
        {
            return true;
        }

        if (velocityX > 0)
        {
            velocityX--;
        }

        velocityY--;
    }

    return false;
}

int highestY = 0;
int winningCount = 0;

for (var velocityY = TargetY[0] * -2; velocityY >= TargetY[0]; velocityY--)
{
    for (var velocityX = 1; velocityX <= TargetX[1]; velocityX++)
    {
        if (Targets(velocityX, velocityY, TargetX, TargetY, out var positions))
        {
            highestY = Math.Max(highestY, positions.Max(p => p.y));
            winningCount++;
        }
    }
}

Console.WriteLine($"Part1 result: {highestY}");
Console.WriteLine($"Part2 result: {winningCount}");
