using SDL2;
using System.Numerics;
using System.Runtime.InteropServices;

class EndScene: Scene
{
    private Actor mEndMsg;
    public EndScene(Game game)  : base(game)
    {

    }

    public override void Start()
    {
        mEndMsg = new Actor(mGame);
        mEndMsg .SetPosition(new Vector2(Game.ScreenWidth / 2, Game.ScreenHeight / 2));
        var endMsgSprite = new SpriteComponent(mEndMsg, 200);
        endMsgSprite.SetTexture(
            mGame.LoadTexture(mGame.GetAssetsPath() + (mGame.GetGameClear() ? "msg_clear.png" : "msg_over.png")));
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    public override void ProcessInput(nint state)
    {
        int numKeys = (int)SDL.SDL_Scancode.SDL_NUM_SCANCODES;
        byte[] stateArray = new byte[numKeys];
        Marshal.Copy(state, stateArray, 0, numKeys);
        if (stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE] != 0 | stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_RETURN] != 0)
        {
            mGame.SetNextScene(new ReadyScene(mGame));
            mEndMsg.SetState(Actor.State.EDead);

            mGame.GetShip().SetState(Actor.State.EDead);
            foreach (var enemy in mGame.GetEnemies())
            {
                enemy.SetState(Actor.State.EDead);
            }
        }
    }
    public override string GetSceneName()
    {
        return "END";
    }
}