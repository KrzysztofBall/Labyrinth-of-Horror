using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Button
{
    public RectangleShape Shape { get; private set; }

    public Button(Vector2f position, Vector2f size)
    {
        Shape = new RectangleShape(size)
        {
            Position = position,
            FillColor = Color.Green
        };
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
}
