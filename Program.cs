using SFML.Graphics;
using SFML.System;
using SFML.Window;
class Game
{
    static void Main()
    {
        RenderWindow window = new(new VideoMode((1280,720)), "Labyrinth of Horror"); //Window
        window.SetFramerateLimit(60);
        window.Closed += (sender,e) => window.Close();
        Clock clock = new();
        StateHandler state = new(window); //Class managing states of program
        float dt;
        while (window.IsOpen) //Main loop 
        {
            window.Clear();
            window.DispatchEvents();
            dt = clock.Restart().AsSeconds(); 
            state.UpdateState(dt);
            window.Display();
        }
    }
}