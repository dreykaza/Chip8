namespace Chip8;

class Program
{
    static void Main(string[] args)
    {
        byte[] game = File.ReadAllBytes("Tetris.ch8");
        Emulator.Start(game);
    }
}
