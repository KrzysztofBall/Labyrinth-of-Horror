using SFML.Graphics;
using SFML.System;
using SFML.Window;
class Game
{
    static void Main()
    {
        RenderWindow window = new(new VideoMode((1280,720)), "Labyrinth of Horror");
        window.SetFramerateLimit(60);
        window.Closed += (sender,e) => window.Close();
        Clock clock = new();
        StateHandler state = new(window);
        float dt;
        while (window.IsOpen)
        {
            window.Clear();
            dt = clock.Restart().AsSeconds();
            window.DispatchEvents();
            state.UpdateState(dt);
            window.Display();
        }
    }
}