using Util;

using System.Diagnostics;

public class Yagbe 
{

    private const uint WINDOW_HEIGHT = 600;
    private const uint WINDOW_WIDTH = 800;
    
        
    private float SCALING_FACTOR_X = WINDOW_WIDTH / Constants.DISPLAY_WIDTH;
    private float SCALING_FACTOR_Y = WINDOW_HEIGHT / Constants.DISPLAY_HEIGHT;

    private SFML.Graphics.Image buffer;
    private SFML.Graphics.Texture viewPort;
    private Z80 sharp8;
    
    private Stopwatch clock;


    public Yagbe(Z80 sharp8)
    {
        this.buffer = new SFML.Graphics.Image(WINDOW_HEIGHT, WINDOW_WIDTH);
        this.viewPort = new SFML.Graphics.Texture(WINDOW_HEIGHT, WINDOW_WIDTH);
        this.sharp8 = sharp8;
        this.clock = new Stopwatch();

    }
    public void Start()
    {
        var mode = new SFML.Window.VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
        var window = new SFML.Graphics.RenderWindow(mode, "SHARP-8");
        window.KeyPressed += Window_KeyPressed;
        window.KeyReleased += Window_KeyReleased;

        var s8s = this.sharp8.GetSharp8State();
        // Start the game loop
        while (window.IsOpen)
        {
            this.clock.Restart();
            window.DispatchEvents();
            this.sharp8.Cycle();
            if (s8s.VramChanged)
            {
                s8s.VramChanged = false;
                UpdatePixelBuffer(s8s.Vram);
                var sprite = new SFML.Graphics.Sprite(this.viewPort);
                sprite.Scale = new SFML.System.Vector2f(SCALING_FACTOR_X,SCALING_FACTOR_Y);
                window.Draw(sprite);
                
                // Finally, display the rendered frame on screen
                window.Display();
            }
            var elapsedTimeMicro = clock.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
            while (elapsedTimeMicro < 1852)
            {
                elapsedTimeMicro = clock.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
            }
            ProcessInput();
        }
    }

    private void ProcessInput()
    {
        var s8s = this.sharp8.GetSharp8State();
        for (var i = 0; i < 0x10; i++)
        {
            if (s8s.K[i] != 0xFF)
            {
                if (s8s.WaitForKey)
                {
                    s8s.V[s8s.KeyPressRegister] = s8s.K[i];
                    s8s.WaitForKey = false;
                    s8s.KeyPressRegister = 0x0;
                    s8s.Pc += 2;
                }
            }
        }
    }

    private void UpdatePixelBuffer(byte[] vram)
    {
        for (uint r = 0; r < Constants.DISPLAY_HEIGHT; r++)
        {
            for (uint c = 0; c < Constants.DISPLAY_WIDTH; c++)
            {
                var idx = (r * Constants.DISPLAY_WIDTH)+c;
                SFML.Graphics.Color color = vram[idx] == 1 ? SFML.Graphics.Color.White: SFML.Graphics.Color.Black;
                this.buffer.SetPixel(c,r,color);
            }
        }
        this.viewPort.Update(buffer);
    }

    private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
    {
        var s8s = this.sharp8.GetSharp8State();
        var window = (SFML.Window.Window)sender;
        if (e.Code == SFML.Window.Keyboard.Key.Escape)
        {
            window.Close();
        }
        else if (e.Code == SFML.Window.Keyboard.Key.Num1)
        {
            K[0x0] = 0x1;
        }
    }
    
    private void Window_KeyReleased(object sender, SFML.Window.KeyEventArgs e)
    {
        var s8s = this.sharp8.GetSharp8State();
        var K = s8s.K;
        var window = (SFML.Window.Window)sender;
        if (e.Code == SFML.Window.Keyboard.Key.Num1)
        {
            K[0x0] = 0xFF;
        }
        else if (e.Code == SFML.Window.Keyboard.Key.Num2)
        {
            K[0x1] = 0xFF;
        }
    }
}