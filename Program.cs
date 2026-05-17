using SFML.Graphics;
using SFML.Window;
using SFML.System;
class Game
{
    static void Main()
    {
        RenderWindow window = new(new VideoMode((1280,720)), "Labyrinth of Horror");
        window.SetFramerateLimit(60);
        window.Closed += (sender,e) => window.Close();
        //testing purposes
        Menu menu = new();
        Button ExitButton = new((50,50),(100,40));
        menu.AddButton(ExitButton);
        while (window.IsOpen)
        {
            window.Clear();
            menu.Draw(window);
            menu.Update(window);
            window.DispatchEvents();
            window.Display();
        }
    }
}