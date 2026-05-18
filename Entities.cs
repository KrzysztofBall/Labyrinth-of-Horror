using SFML.Graphics;
using SFML.System;
using SFML.Window;

public interface IEntity
{
    Vector2f Position {get;}
    Vector2f Hitbox {get;}

    void Draw(RenderWindow window);
}

public class Player : IEntity
{
    public Vector2f Position { get; set; }
    public float Angle { get; private set; } = 0f;

    public float Speed = 3f;
    public float RotationSpeed = 2.5f;
    public float Radius { get; private set; } = 0.3f;

    public Player(float x, float y)
    {
        Position = new Vector2f(x, y);
    }

    public Vector2f Hitbox => new Vector2f(Radius * 2, Radius * 2);

    public void Draw(RenderWindow window)
    {
        // todo
    }

    public void Update(float dt, MapHandler map, RenderWindow window)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
        {
            Move(MathF.Cos(Angle) * Speed * dt,
                 MathF.Sin(Angle) * Speed * dt,
                 map);
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
        {
            Move(-MathF.Cos(Angle) * Speed * dt,
                 -MathF.Sin(Angle) * Speed * dt,
                 map);
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            Angle -= 2f * dt;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            Angle += 2f * dt;
        }
        HandleMouse(window);
    }
    public void HandleMouse(RenderWindow window)
    {
        var center = new Vector2i((int)window.Size.X / 2, (int)window.Size.Y / 2);
        var mouse = Mouse.GetPosition(window);
        int deltaX = mouse.X - center.X;
        Angle += deltaX * 0.002f;
        Mouse.SetPosition(center, window);
    }   


    private void Move(float dx, float dy, MapHandler map)
    {
        float newX = Position.X + dx;
        float newY = Position.Y + dy;

        if (!map.IsWall((int)(newX + MathF.Sign(dx) * Radius), (int)Position.Y))
            Position = new Vector2f(newX, Position.Y);

        if (!map.IsWall((int)Position.X, (int)(newY + MathF.Sign(dy) * Radius)))
            Position = new Vector2f(Position.X, newY);
    }
}