using SFML.Graphics;
using SFML.System;

public interface IEntity
{
    Vector2f Position {get;}
    Vector2f Hitbox {get;}

    void Draw(RenderWindow window);
}

public class Player : IEntity
{
    public Vector2f Position { get; private set;}
    public Vector2f Hitbox { get; private set;}

    public Player()
    {
        Position = (0,0);
        Hitbox = (1,1); //undecided yet
    }

    public void Draw(RenderWindow window)
    {
        //todo
    }
}