using Day10;

var valueRegister = new Register(); // Single register that gets manipulated by ADDX instructions
var cycleRegister = new Register(); // Single program counter cycle that increases for each instruction
var memory = new Memory(); // Memory that holds all instructions in all place
var cpu = new CPU(memory, valueRegister, cycleRegister); // CPU for handling instructions
var crt = new CRT(valueRegister, cycleRegister); // Handles drawing

var definedCycles = new[] { 20, 60, 100, 140, 180, 220 };
var part1 = 0;
var part2 = "";

while (cpu.InProgress || memory.HasInstructions)
{
    if (definedCycles.Contains(cycleRegister.Value))
    {
        part1 += cycleRegister.Value * valueRegister.Value;
    }
    
    part2 += crt.Draw();
    cpu.RunCycle();
}

Console.WriteLine("Part 1: " + part1);
Console.WriteLine("Part 2: " + part2);
