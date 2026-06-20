using SFML.Graphics;
using SFML.System;

public interface IEntity
{
    Vector2f Position { get; }
    Vector2f Hitbox { get; }
}

public abstract class Actor : IEntity
{
    public Vector2f Position { get; set; }
    public float Radius { get; protected set; } = 0.25f;

    public Vector2f Hitbox => new Vector2f(Radius * 2, Radius * 2);

    protected void Move(float dx, float dy, MapHandler map)
    {
        float newX = Position.X + dx;
        float newY = Position.Y + dy;

        // --- KOLIZJA OŚ X ---
        float checkX = newX + MathF.Sign(dx) * Radius;
        int tileX = (int)checkX;
        int tileY = (int)Position.Y;

        bool blockX = tileX < 0 || tileY < 0 ||
                      tileX >= map.Map.GetLength(1) ||
                      tileY >= map.Map.GetLength(0) ||
                      map.Map[tileY, tileX] != 0;

        if (!blockX)
            Position = new Vector2f(newX, Position.Y);

        // --- KOLIZJA OŚ Y ---
        float checkY = newY + MathF.Sign(dy) * Radius;
        tileX = (int)Position.X;
        tileY = (int)checkY;

        bool blockY = tileX < 0 || tileY < 0 ||
                      tileX >= map.Map.GetLength(1) ||
                      tileY >= map.Map.GetLength(0) ||
                      map.Map[tileY, tileX] != 0;

        if (!blockY)
            Position = new Vector2f(Position.X, newY);
    }

    public abstract void Update(float dt, MapHandler map, RenderWindow window);
}
public interface IState
{
    void Update(float dt, RenderWindow window);
    void Draw(RenderWindow window);
}