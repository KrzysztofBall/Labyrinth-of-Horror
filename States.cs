using SFML.Graphics;
using SFML.System;
using SFML.Window;

public interface IState
{
    void Update(RenderWindow window);
    void Draw(RenderWindow window);
}

class Menu : IState
{
    private List<Button> buttons = [];
    public void Update(RenderWindow window)
    {
        Vector2f MousePosition = (Vector2f)Mouse.GetPosition();
        foreach (var button in buttons)
        {
            if (button.Hovered(window))
            {
                //dosmth
                button.Shape.FillColor = Color.White;
            }
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