using SFML.Graphics;

public class StateHandler
{
    public RenderWindow Window;
    public IState Menu {get;}
    public IState Paused {get;}
    public IState Gameplay {get;}
    public IState CurrentState {get; private set;}

    public StateHandler(RenderWindow window)
    {
        Window = window;
        Menu = new Menu(this);
        Paused = new Paused(this);
        Gameplay = new Gameplay(this);
        //todo more states
        CurrentState = Menu;
    }

    public void ChangeState(IState newState)
    {
        CurrentState = newState;
        if(CurrentState == Gameplay)
        {
            Window.SetMouseCursorVisible(false);
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