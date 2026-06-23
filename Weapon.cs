using SFML.System;
using SFML.Graphics;
using SFML.Audio;

public class Weapon
{
    public enum WeaponState { Ready, Fired, Reloading }

    public WeaponState State = WeaponState.Ready;

    private SoundBuffer shootBuffer;
    private Sound shootSound;

    private Texture state1; // Fired
    private Texture state2; // Ready
    private Texture state3; // Reloading

    public float FireCooldown = 1.2f;
    private float timer = 0f;

    public RectangleShape Sprite;
    public CircleShape Sight;

    public Weapon()
    {
        Sprite = new RectangleShape(new Vector2f(350, 350));
        Sprite.Origin = new Vector2f(350, 350);

        Sight = new CircleShape(3);
        Sight.Origin = new Vector2f(3, 3);
        Sight.FillColor = Color.Black;

        shootBuffer = new SoundBuffer("shoot2.ogg");
        shootSound = new Sound(shootBuffer);

        state1 = new Texture("weapon1.png"); // Fired
        state2 = new Texture("weapon2.png"); // Ready
        state3 = new Texture("weapon3.png"); // Reloading
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

        shootSound.Play();
        State = WeaponState.Fired;
        timer = 0f;

        return true;
    }

    public void Draw(RenderWindow window)
    {
        // Textures on states
        if (State == WeaponState.Ready)
            Sprite.Texture = state2;
        else if (State == WeaponState.Fired)
            Sprite.Texture = state1;
        else
            Sprite.Texture = state3;

        Sprite.Position = new Vector2f(
            window.Size.X,
            window.Size.Y
        );

        // Sight
        Sight.Position = new Vector2f(
            window.Size.X / 2f,
            window.Size.Y / 2f
        );

        window.Draw(Sprite);
        window.Draw(Sight);
    }
}
