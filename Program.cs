namespace Chip8;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Name");
        string name = Console.ReadLine();
        string filePath = Path.Combine("ROMs", name);
        byte[] game = File.ReadAllBytes(filePath);
        Emulator.Start(game);
        Task.Delay(-1).Wait();
    }
}
