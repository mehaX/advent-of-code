static void Part(int days)
{
    var input = File.ReadAllText("input.txt");
    var fishes = input.Split(",").Select(c => Convert.ToInt64(c)).ToList();
    var fishesCount = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    foreach (var fish in fishes)
    {
        fishesCount[fish]++;
    }

    for (var day = 1; day <= days; day++)
    {
        var newFishes = fishesCount[0];
        fishesCount[0] = fishesCount[1];
        fishesCount[1] = fishesCount[2];
        fishesCount[2] = fishesCount[3];
        fishesCount[3] = fishesCount[4];
        fishesCount[4] = fishesCount[5];
        fishesCount[5] = fishesCount[6];
        fishesCount[6] = fishesCount[7] + newFishes;
        fishesCount[7] = fishesCount[8];
        fishesCount[8] = newFishes;
    }

    var count = fishesCount.Sum();
    
    Console.WriteLine($"Result for {days} days: {count}");
}

Part(80);
Part(256);
