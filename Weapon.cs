using SFML.System;
using SFML.Graphics;
using System.Numerics;
public class Weapon
{
    public enum WeaponState { Ready, Fired, Reloading }

    public WeaponState State = WeaponState.Ready;

    public float FireCooldown = 1.2f;
    private float timer = 0f;

    public RectangleShape Sprite;
    public CircleShape Sight;

    public Weapon()
    {
        Sprite = new RectangleShape(new Vector2f(200, 80));
        Sprite.Origin = new Vector2f(100, 40);
        Sight = new CircleShape(3); // 3px, żeby było widać
        Sight.Origin = new Vector2f(3, 3); // środek kształtu
        Sight.FillColor = Color.Black;

    }

    public void Update(float dt)
    {
        if (State == WeaponState.Fired)
        {
            timer += dt;
            if (timer >= 0.2f)
            {
                State = WeaponState.Reloading;
                timer = 0f;
            }
        }
        else if (State == WeaponState.Reloading)
        {
            timer += dt;
            if (timer >= FireCooldown)
            {
                State = WeaponState.Ready;
                timer = 0f;
            }
        }
    }

    public bool TryShoot()
    {
        if (State != WeaponState.Ready)
            return false;

        State = WeaponState.Fired;
        timer = 0f;
        return true;
    }

    public void Draw(RenderWindow window)
    {
        // kolor wg stanu
        if (State == WeaponState.Ready)
            Sprite.FillColor = Color.Green;
        else if (State == WeaponState.Fired)
            Sprite.FillColor = Color.Red;
        else
            Sprite.FillColor = Color.Yellow;

        // pozycja na dole ekranu
        Sprite.Position = new Vector2f(window.Size.X / 2, window.Size.Y - 100);
        Sight.Position = new Vector2f(window.Size.X / 2f, window.Size.Y / 2f);


        window.Draw(Sprite);
        window.Draw(Sight);
    }
}
