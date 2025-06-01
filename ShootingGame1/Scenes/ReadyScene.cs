using SDL2;
using System.Numerics;
using System.Runtime.InteropServices;

class ReadyScene : Scene
{
    private Actor mStartMsg;
    public ReadyScene(Game game) : base (game)
    {

    }

    public override void Start()
    {
        mGame.SetGameClear(false);
        mStartMsg = new Actor(mGame);
        mStartMsg.SetPosition(new Vector2(Game.ScreenWidth / 2, Game.ScreenHeight / 2));
        var startMsgSprite = new SpriteComponent(mStartMsg, 200);
        startMsgSprite.SetTexture(mGame.LoadTexture(mGame.GetAssetsPath() + "msg_start.png"));
        Ship ship = new Ship(mGame);
        ship.SetPosition(new Vector2(Game.ScreenWidth / 2, Game.ScreenHeight - 200));
        mGame.SetShip(ship);
    }

    public override void ProcessInput(nint state)
    {
        int numKeys = (int)SDL.SDL_Scancode.SDL_NUM_SCANCODES;
        byte[] stateArray = new byte[numKeys];
        Marshal.Copy(state, stateArray, 0, numKeys);
        if (stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE] != 0
            || stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_RETURN] != 0)
        {
            mGame.SetNextScene(new GameScene(mGame));
            mStartMsg.SetState(Actor.State.EDead);
        }

    }

    public override string GetSceneName()
    {
        return "READY";
    }
}