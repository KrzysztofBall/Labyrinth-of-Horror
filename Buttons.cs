using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class Button
{
    public RectangleShape Shape { get; private set; }
    public Text Label { get; private set; }
    public Action<Button>? OnClick { get; set; }

    private bool wasPressedLastFrame = false;

    // 🔥 WSPÓLNY FONT DLA WSZYSTKICH PRZYCISKÓW
    private static readonly Font DefaultFont = new Font("arial.ttf");

    public Button(Vector2f position, Vector2f size, string text, Action<Button>? onClick = null)
    {
        // Prostokąt
        Shape = new RectangleShape(size)
        {
            Position = position,
            FillColor = Color.Green
        };

        // Tekst
        Label = new Text(DefaultFont,text, 32);
        Label.FillColor = Color.Black;

        CenterText();

        OnClick = onClick;
    }

    private void CenterText()
    {
        FloatRect bounds = Label.GetLocalBounds();

        Label.Origin = new Vector2f(
            bounds.Left + bounds.Width / 2f,
            bounds.Top + bounds.Height / 2f
        );

        Label.Position = new Vector2f(
            Shape.Position.X + Shape.Size.X / 2f,
            Shape.Position.Y + Shape.Size.Y / 2f
        );
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Shape);
        window.Draw(Label);
    }

    public bool Hovered(RenderWindow window)
    {
        var mouse = Mouse.GetPosition(window);
        return Shape.GetGlobalBounds().Contains(mouse);
    }

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
