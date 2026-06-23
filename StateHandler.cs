using SFML.Graphics;
using SFML.Window;
using SFML.System;

public class StateHandler
{
    public RenderWindow Window;
    public IState CurrentState { get; private set; }

    public StateHandler(RenderWindow window)
    {
        Window = window;

        // Start from menu
        CurrentState = new Menu(this);
    }

    public void ChangeState(IState newState)
    {
        CurrentState = newState;

        // Hide mouse
        if (newState is Gameplay)
        {
            Window.SetMouseCursorVisible(false);
            Mouse.SetPosition(
                new Vector2i((int)Window.Size.X / 2, (int)Window.Size.Y / 2),
                Window
            );
        }
        else //Not hide mouse
        {
            Window.SetMouseCursorVisible(true);
        }
    }

    public void UpdateState(float dt)
    {
        CurrentState.Update(dt, Window);
        CurrentState.Draw(Window);
    }
}
