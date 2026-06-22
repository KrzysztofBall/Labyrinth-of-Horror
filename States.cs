using SFML.Graphics;
using SFML.Window;



class Menu : IState
{
    private List<Button> buttons = [];
    private StateHandler Handler;
    private Texture backGround;
    private Sprite BackGround;

    public Menu(StateHandler stateHandler)
    {
        //addbuttons
        Handler = stateHandler;
        buttons.Add(new Button((440,425),(400,50),_ => Handler.ChangeState(Handler.Gameplay)));
        //buttons.Add(new Button((440,500),(400,50),_ => Handler.Window.Close()));
        backGround = new Texture("menu.png");
        BackGround = new Sprite(backGround);
    }
    public void Update(float dt, RenderWindow window)
    {
        foreach (var button in buttons)
        {
            button.Clicked(window);
        }
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(BackGround);
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
            Handler.ChangeState(Handler.Paused);
        }
        if (Map.Update(dt, window)==1)
        {
                Handler.ChangeState(Handler.Win);
        }
        else if (Map.Update(dt, window) == 2)
        {
            Handler.ChangeState(Handler.Lose);
        }
            
    }

    public void Draw(RenderWindow window)
    {
        Map.Draw(window); //raycasting
        foreach (var enemy in Map.Enemies) enemy.Draw(window,Map);
        Map.Player.Draw(window); //firstperson view (weapon/hand + UI)
        Map.DrawMinimap(window);
        
    }
}

class Paused : IState
{
    private StateHandler Handler;
    private List<Button> buttons = [];
    private Texture backGround;
    private Sprite BackGround;
    public Paused(StateHandler handler)
    {
        Handler = handler;
        buttons.Add(new Button((440,425),(400,50),_=> Handler.ChangeState(Handler.Gameplay)));
        buttons.Add(new Button((440,575),(400,50),_ => Handler.Window.Close()));
        buttons.Add(new Button((440,500),(400,50),_=> Handler.ChangeState(Handler.Menu)));
        backGround = new Texture("paused.png");
        BackGround = new Sprite(backGround);
    }
    public void Update(float dt, RenderWindow window)
    {
        foreach (var button in buttons)
        {
            button.Clicked(window);
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
        {
            Handler.ChangeState(Handler.Gameplay);
        }
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(BackGround);
        foreach (var button in buttons)
        {
            button.Draw(window);
        }
    }
}

class Win : IState
{
    private StateHandler Handler;
    private List<Button> buttons = [];
    private Texture backGround;
    private Sprite BackGround;
    public Win(StateHandler handler)
    {
        this.Handler = handler;
        buttons.Add(new Button((440,500),(400,50),_=> Handler.ChangeState(Handler.Menu)));
        backGround = new Texture("win.png");
        BackGround = new Sprite(backGround);
    }

    public void Update(float dt,RenderWindow window)
    {
        foreach (var button in buttons)
        {
            button.Clicked(window);
        }
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(BackGround);
        foreach (var button in buttons)
        {
            button.Draw(window);
        }
    }
}

class Lose : IState
{
    private StateHandler Handler;
    private List<Button> buttons = [];
    private Texture backGround;
    private Sprite BackGround;
    public Lose(StateHandler handler)
    {
        this.Handler = handler;
        buttons.Add(new Button((440,500),(400,50),_=> Handler.ChangeState(Handler.Menu)));
        backGround = new Texture("lose.png");
        BackGround = new Sprite(backGround);
    }

    public void Update(float dt,RenderWindow window)
    {
        foreach (var button in buttons)
        {
            button.Clicked(window);
        }
    }
    public void Draw(RenderWindow window)
    {
        window.Draw(BackGround);
        foreach (var button in buttons)
        {
            button.Draw(window);
        }
    }
}