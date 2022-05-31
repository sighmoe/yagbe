using System.IO;

public class Program {
    public static void Main(string[] args) {
        byte[] rom = File.ReadAllBytes("/home/simo/development/chip8-roms/TETRIS");
        Z80 sharp8 = new Z80();
        sharp8.LoadRom(rom);
        Yagbe gui = new Yagbe(sharp8);
        gui.Start();
    }
}