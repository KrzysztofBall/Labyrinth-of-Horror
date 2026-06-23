using SFML.Graphics;
using SFML.Window;
using SFML.System;
class Menu : IState
{
    private List<Button> buttons = new();
    private StateHandler Handler;
    private Texture backGround;
    private Sprite BackGround;

    public Menu(StateHandler stateHandler)
    {
        Handler = stateHandler;

        // Start Game → zawsze NOWY gameplay
        buttons.Add(new Button((440,425),(400,50),"Plej", _ =>
        {
            Handler.ChangeState(new Gameplay(Handler));
        }));


        backGround = new Texture("menu.png");
        BackGround = new Sprite(backGround);
    }

    public void Update(float dt, RenderWindow window)
    {
        foreach (var button in buttons)
            button.Clicked(window);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(BackGround);
        foreach (var button in buttons)
            button.Draw(window);
    }
}

class Gameplay : IState
{
    private StateHandler Handler;
    public MapHandler Map = new();
    private float elapsedTime = 0f;
    private Text timerText;


    public Gameplay(StateHandler handler)
    {
        Handler = handler;
        timerText = new Text(new Font("arial.ttf"),"0.00", 32);
        timerText.FillColor = Color.White;
        timerText.Position = new Vector2f(1000, 20); // prawy górny róg

    }

    public void Update(float dt, RenderWindow window)
    {
        // Pauza → przekazujemy referencję do TEGO gameplayu
        if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
        {
            Handler.ChangeState(new Paused(Handler, this));
            return;
        }

        int val = Map.Update(dt, window);

        if (val == 1)
        {
            Handler.ChangeState(new Win(Handler, elapsedTime));

        }
        else if (val == 2)
        {
            Handler.ChangeState(new Lose(Handler));
        }
        elapsedTime += dt;
        timerText.DisplayedString = elapsedTime.ToString("0.00");

    }

    public void Draw(RenderWindow window)
    {
        Map.Draw(window);
        foreach (var enemy in Map.Enemies)
            enemy.Draw(window, Map);

        Map.Player.Draw(window);
        Map.DrawMinimap(window);
        window.Draw(timerText);

    }
}

class Paused : IState
{
    private StateHandler Handler;
    private Gameplay gameplay; // referencja do aktualnego gameplayu
    private List<Button> buttons = new();
    private Texture backGround;
    private Sprite BackGround;

    public Paused(StateHandler handler, Gameplay gameplay)
    {
        Handler = handler;
        this.gameplay = gameplay;

        // Resume → wracamy do TEGO gameplayu
        buttons.Add(new Button((440,425),(400,50),"Reasume", _ =>
        {
            Handler.ChangeState(gameplay);
        }));

        // Back to Menu → niszczy gameplay (bo nie ma referencji)
        buttons.Add(new Button((440,500),(400,50),"Exit to menu", _ =>
        {
            Handler.ChangeState(new Menu(Handler));
        }));

        // Exit
        buttons.Add(new Button((440,575),(400,50),"Exit game", _ =>
        {
            Handler.Window.Close();
        }));

        backGround = new Texture("paused.png");
        BackGround = new Sprite(backGround);
    }

    public void Update(float dt, RenderWindow window)
    {
        foreach (var button in buttons)
            button.Clicked(window);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(BackGround);
        foreach (var button in buttons)
            button.Draw(window);
    }
}

class Win : IState
{
    private StateHandler Handler;
    private List<Button> buttons = new();
    private Texture backGround;
    private Sprite BackGround;

    private float finalTime;
private Text timeText;

public Win(StateHandler handler, float time)
{
    Handler = handler;
    finalTime = time;

    buttons.Add(new Button(new Vector2f(440,500), new Vector2f(400,50), "Menu",
        _ => Handler.ChangeState(new Menu(Handler))));

    backGround = new Texture("win.png");
    BackGround = new Sprite(backGround);

    timeText = new Text(new Font("arial.ttf"),$"Czas: {finalTime:0.00}s", 48);
    timeText.FillColor = Color.Yellow;
    timeText.Position = new Vector2f(400, 300);
}


    public void Update(float dt, RenderWindow window)
    {
        foreach (var button in buttons)
            button.Clicked(window);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(BackGround);
        foreach (var button in buttons)
            button.Draw(window);
        window.Draw(timeText);

    }
}

class Lose : IState
{
    private StateHandler Handler;
    private List<Button> buttons = new();
    private Texture backGround;
    private Sprite BackGround;

    public Lose(StateHandler handler)
    {
        Handler = handler;

        buttons.Add(new Button((440,500),(400,50),"Menu", _ =>
        {
            Handler.ChangeState(new Menu(Handler));
        }));

        backGround = new Texture("lose.png");
        BackGround = new Sprite(backGround);
    }

    public void Update(float dt, RenderWindow window)
    {
        foreach (var button in buttons)
            button.Clicked(window);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(BackGround);
        foreach (var button in buttons)
            button.Draw(window);
    }
}
