using SDL2;
using System.Numerics;
using System.Runtime.InteropServices;

class Ship : Actor
{
    private float mRightMove;
    private float mDownMove;
    private bool mIsCanShot;
    private float mDeltaShotTime;
    private ColliderComponent mCollider;
    private const float ShipSpeed = 480.0f; 
    private const float CanShotTime = 0.15f; 
    public Ship(Game game) : base(game)
    {
        var sprite = new SpriteComponent(this);
        sprite.SetTexture(GetGame().LoadTexture(GetGame().GetAssetsPath() + "ship.png"));
        mCollider = new ColliderComponent(this);
        mCollider.SetRadius(70.0f * GetScale());
    }

    public override void UpdateActor(float deltaTime)
    {
        base.UpdateActor(deltaTime);
        
        if (GetGame().GetScene().GetSceneName().CompareTo("END") != 0)
        {
            Vector2 pos = GetPosition();
            pos.X += mRightMove * deltaTime;
            pos.Y += mDownMove * deltaTime;
            if (pos.X < 25.0f)
            {
                pos.X = 25.0f;
            }
            else if (pos.X > Game.ScreenWidth - 25.0f)
            {
                pos.X = Game.ScreenWidth - 25.0f;
            }
            if (pos.Y < 25.0f)
            {
                pos.Y = 25.0f;
            }
            else if (pos.Y > Game.ScreenHeight - 25.0f)
            {
                pos.Y = Game.ScreenHeight - 25.0f;
            }
            SetPosition(pos);
        }

        foreach (var enemy in GetGame().GetEnemies())
        {
            if (ColliderComponent.Intersect(mCollider, enemy.GetCollider()))
            {
                GetGame().SetNextScene(new EndScene(GetGame()));
                SetState(State.EDead);
                var bomb = new BombEffect(GetGame());
                bomb.SetPosition(GetPosition());
                return;
            }
        }

        if (!mIsCanShot)
        {
            mDeltaShotTime += deltaTime;
            if (mDeltaShotTime > CanShotTime)
            {
                mIsCanShot = true;
                mDeltaShotTime = 0.0f;
            }
        }
    }

    public void ProcessKeyboard(nint state)
    {
        mRightMove = 0.0f;
        mDownMove = 0.0f;

        int numKeys = (int)SDL.SDL_Scancode.SDL_NUM_SCANCODES;
        byte[] stateArray = new byte[numKeys];
        Marshal.Copy(state, stateArray, 0, numKeys);
        if (stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_D] != 0)
        {
            mRightMove += ShipSpeed;
        }
        if (stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_A] != 0) {
            mRightMove -= ShipSpeed;
        }
        if(stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_S] != 0) {
            mDownMove += ShipSpeed;
        }
        if(stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_W] != 0) {
            mDownMove -= ShipSpeed;
        }
        if(stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_K] != 0) {
            if (mIsCanShot)
            {
                mIsCanShot = false;
                mDeltaShotTime = 0.0f;

                var missile = new Missile(GetGame());
                Vector2 pos = GetPosition();
                missile.SetPosition(new Vector2(pos.X, pos.Y - 30.0f));
            }
        }
    }
}