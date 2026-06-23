using SFML.System;
using SFML.Graphics;
using SFML.Audio;

public class Weapon
{
    public enum WeaponState { Ready, Fired, Reloading }

    public WeaponState State = WeaponState.Ready;

    private SoundBuffer shootBuffer;
    private Sound shootSound;

    private Texture state1; // Ready
    private Texture state2; // Fired
    private Texture state3; // Reloading

    public float FireCooldown = 1.2f;
    private float timer = 0f;

    public RectangleShape Sprite;
    public CircleShape Sight;

    public Weapon()
    {
        // DUŻA, KWADRATOWA BROŃ
        Sprite = new RectangleShape(new Vector2f(350, 350));
        Sprite.Origin = new Vector2f(350, 350);

        // Celownik
        Sight = new CircleShape(3);
        Sight.Origin = new Vector2f(3, 3);
        Sight.FillColor = Color.Black;

        // Dźwięk
        shootBuffer = new SoundBuffer("shoot2.ogg");
        shootSound = new Sound(shootBuffer);

        // Tekstury broni
        state1 = new Texture("weapon1.png"); // Ready
        state2 = new Texture("weapon2.png"); // Fired
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
        // Wybór tekstury wg stanu
        if (State == WeaponState.Ready)
            Sprite.Texture = state2;
        else if (State == WeaponState.Fired)
            Sprite.Texture = state1;
        else
            Sprite.Texture = state3;

        // Pozycja w prawym dolnym rogu
        Sprite.Position = new Vector2f(
            window.Size.X,
            window.Size.Y
        );

        // Celownik na środku
        Sight.Position = new Vector2f(
            window.Size.X / 2f,
            window.Size.Y / 2f
        );

        window.Draw(Sprite);
        window.Draw(Sight);
    }
}
