using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Button
{
    public RectangleShape Shape { get; private set; }
    public Action<Button>? OnClick { get; set; }

    public Button(Vector2f position, Vector2f size, Action<Button>? onClick = null)
    {
        Shape = new RectangleShape(size)
        {
            Position = position,
            FillColor = Color.Green
        };

        OnClick = onClick;
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Shape);
    }

    public bool Hovered(RenderWindow window)
    {
        var mouse = Mouse.GetPosition(window);
        return Shape.GetGlobalBounds().Contains(mouse);
    }

    bool wasPressedLastFrame = false;
    public void Clicked(RenderWindow window)
    {
        bool pressed = Mouse.IsButtonPressed(Mouse.Button.Left);
        if (Hovered(window) && pressed && !wasPressedLastFrame)
        {
            OnClick?.Invoke(this);
        }
        wasPressedLastFrame = pressed;
    }
}
