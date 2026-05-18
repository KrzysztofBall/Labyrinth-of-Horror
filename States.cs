using SFML.Graphics;
using SFML.Window;

public interface IState
{
    void Update(float dt, RenderWindow window);
    void Draw(RenderWindow window);

}

class Menu : IState
{
    private List<Button> buttons = [];
    private StateHandler Handler;

    public Menu(StateHandler stateHandler)
    {
        //addbuttons
        Handler = stateHandler;
        buttons.Add(new Button((50,50),(50,50),_ => Handler.ChangeState(Handler.Gameplay)));
    }
    public void Update(float dt, RenderWindow window)
    {
        foreach (var button in buttons)
        {
            button.Clicked(window);
        }
    }

    public void AddButton(Button newbutton)
    {
        buttons.Add(newbutton);
    }

    public void Draw(RenderWindow window)
    {
        foreach (var button in buttons)
        {
            button.Draw(window);
        }
    }
}

class Gameplay : IState
{
    StateHandler Handler;
    MapHandler Map = new();
    public Gameplay(StateHandler handler)
    {
        Handler = handler;  
    }
    public void Update(float dt, RenderWindow window)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
        {
            //Change state to Paused
            Handler.ChangeState(Handler.Paused);
        }
        Map.Update(dt,window);
    }

    public void Draw(RenderWindow window)
    {
        Map.Draw(window); //raycasting
        Map.Player.Draw(window); //firstperson view (weapon/hand + UI)
    }
}

class Paused : IState
{
    private StateHandler Handler;
    private List<Button> buttons = [];

    public Paused(StateHandler handler)
    {
        Handler = handler;
        buttons.Add(new Button((100,100),(100,40),_ => Handler.Window.Close()));
    }
    public void Update(float dt, RenderWindow window)
    {
        foreach (var button in buttons)
        {
            button.Clicked(window);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
        {
            //unpause
            Handler.ChangeState(Handler.Gameplay);
        }
    }

    public void Draw(RenderWindow window)
    {
        foreach (var button in buttons)
        {
            button.Draw(window);
        }
    }
}