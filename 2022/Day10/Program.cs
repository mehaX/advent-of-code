using Day10.Models;

var instructions = File.ReadAllLines("input.txt");
var definedCycles = new[] { 20, 60, 100, 140, 180, 220 };
var part1 = 0;
var part2 = "";

var valueRegister = new Register(); // Single register that gets manipulated by ADDX instructions
var cycleRegister = new Register(); // Single program counter cycle that increases for each instruction
var memory = new Memory(instructions); // Memory that holds all instructions in one place
var cpu = new CPU(memory, valueRegister, cycleRegister); // CPU for handling instructions
var crt = new CRT(valueRegister, cycleRegister); // Handles drawing

while (cpu.IsRunning)
{
    cpu.BeginCycle();
    
    if (definedCycles.Contains(cycleRegister.Value))
    {
        part1 += cycleRegister.Value * valueRegister.Value;
    }
    
    part2 += crt.Draw();
    
    cpu.EndCycle();
}

Console.WriteLine("Part 1: " + part1);
Console.WriteLine("Part 2: " + part2);
