using SFML.Graphics;
using SFML.Window;
using SFML.System;

public class StateHandler
{
    public RenderWindow Window;
    public IState Menu {get;}
    public IState Paused {get;}
    public IState Gameplay {get;}
    public IState Win {get;}
    public IState Lose {get;}
    public IState CurrentState {get; private set;}
    public StateHandler(RenderWindow window)
    {
        Window = window;
        Menu = new Menu(this);
        Paused = new Paused(this);
        Gameplay = new Gameplay(this);
        Win = new Win(this);
        Lose = new Lose(this);
        //todo more states
        CurrentState = Menu;
    }

    public void ChangeState(IState newState)
    {
        CurrentState = newState;
        if(CurrentState == Gameplay)
        {
            Window.SetMouseCursorVisible(false);
            Mouse.SetPosition(new Vector2i((int)Window.Size.X / 2, (int)Window.Size.Y / 2), Window);

        }
        else
        {
            Window.SetMouseCursorVisible(true);
        }

    }

    public void UpdateState(float dt)
    {
        CurrentState.Update(dt,Window);
        CurrentState.Draw(Window);
    }
}