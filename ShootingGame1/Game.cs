using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using SDL2;
using System.Runtime.InteropServices;

class Game
{
    private Dictionary<string, nint> mCashedTextures = new Dictionary<string, nint>();
    private IntPtr mWindow;
    private IntPtr mRenderer;
    private uint mTicksCount;
    public const int ScreenWidth = 768;
    public const int ScreenHeight = 1024;
    private bool mIsRunning;
    private bool mUpdatingActors;
    private bool mGameClear;
    private List<Actor> mActors = new List<Actor>();
    private List<Actor> mPendingActors = new List<Actor>();
    private List<SpriteComponent> mSprites = new List<SpriteComponent>();
    private Ship mShip;
    private List<Enemy> mEnemies = new List<Enemy>();
    private string AssetsPath = "Assets\\";
    private Scene mScene;
    private Scene mNextScene;
    public Game()
    {
        mWindow = IntPtr.Zero;
        mRenderer = IntPtr.Zero;
        mTicksCount = 0;
        mIsRunning = true;
        mUpdatingActors = false;
        mGameClear = false;
    }

    public bool Initialize()
    {
        if (!InitSDL())
        {
            SDL.SDL_Log(SDL.SDL_GetError());
            return false;
        }

        mTicksCount = SDL.SDL_GetTicks();

        InitScene();

        return true;
    }

    public void RunLoop()
    {
        mScene = new ReadyScene(this);
        mNextScene = mScene;
        StartScene();

        while (mIsRunning)
        {
            UpdateScene();

            if (mScene.GetSceneName().CompareTo(mNextScene.GetSceneName()) != 0)
            {
                mScene = mNextScene;
                StartScene();
            }
        }
    }

    public void Shutdown()
    {

    }

    public void AddActor(Actor actor)
    {
        if (mUpdatingActors)
            mPendingActors.Add(actor);
        else
            mActors.Add(actor);
    }

    public void RemoveActor(Actor actor)
    {
        mPendingActors.Remove(actor);
        mActors.Remove(actor);
    }

    public void AddSprite(SpriteComponent sprite)
    {
        int myDrawOrder = sprite.GetDrawOrder();
        int index = 0;
        for (; index < mSprites.Count; ++index)
        {
            if (myDrawOrder < mSprites[index].GetDrawOrder())
            {
                break;
            }
        }
        mSprites.Insert(index, sprite);
    }

    public void RemoveSprite(SpriteComponent sprite)
    {
        mSprites.Remove(sprite);
    }

    public void AddEnemy(Enemy enemy)
    {
        mEnemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        mEnemies.Remove(enemy);
    }

    private bool InitSDL()
    {
        bool success = SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO) == 0;
        if (!success) return false;

        mWindow = SDL.SDL_CreateWindow("ShootingGame", 100, 100, ScreenWidth, ScreenHeight, 0);
        if (mWindow == 0) return false;

        mRenderer = SDL.SDL_CreateRenderer(mWindow, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        if (mRenderer == 0) return false;

        success = SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) != 0;
        if (!success) return false;

        return true;
    }

    private void InitScene()
    {
        var bgBack = new Actor(this);
        bgBack.SetPosition(new Vector2(ScreenWidth / 2, ScreenHeight / 2));
        var bgBackSprite = new ScrollSpriteComponent(bgBack, 10);
        bgBackSprite.SetTexture(LoadTexture(GetAssetsPath() + "bg_back.png"));
        bgBackSprite.SetScrollSpeedY(100.0f);

        var bgFront = new Actor(this);
        bgFront.SetPosition(new Vector2(ScreenWidth / 2, ScreenHeight / 2));
        var bgFrontSprite = new ScrollSpriteComponent(bgFront, 20);
        bgFrontSprite.SetTexture(LoadTexture(GetAssetsPath() + "bg_front.png"));
        bgFrontSprite.SetScrollSpeedY(200.0f);
    }

    private void StartScene()
    {
        mScene.Start();
    }

    private void UpdateScene()
    {
        ProcessInput();

        while (SDL.SDL_TICKS_PASSED(SDL.SDL_GetTicks(), mTicksCount + 16) == false) ;

        float deltaTime = (SDL.SDL_GetTicks() - mTicksCount) / 1000.0f;
        if (deltaTime > 0.05f)
        {
            deltaTime = 0.05f;
        }
        mTicksCount = SDL.SDL_GetTicks();

        mUpdatingActors = true;
        foreach (var actor in mActors)
        {
            actor.Update(deltaTime);
        }
        mUpdatingActors = false;

        foreach (var pending in mPendingActors)
        {
            mActors.Add(pending);
        }
        mPendingActors.Clear();

        mScene.Update(deltaTime);

        List<Actor> deadActors = new List<Actor>();
        foreach (var actor in mActors)
        {
            if (actor.GetState() == Actor.State.EDead)
            {
                deadActors.Add(actor);
            }
        }
        foreach (var actor in deadActors)
        {
            actor.Dispose();
        }

        GenerateOutput();
    }

    private void ProcessInput()
    {
        SDL.SDL_Event myevent;
        while (SDL.SDL_PollEvent(out myevent) != 0)
        {
            switch (myevent.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    mIsRunning = false;
                    break;
            }
        }

        IntPtr statePtr = SDL.SDL_GetKeyboardState(out int numKeys);
        byte[] stateArray = new byte[numKeys];
        Marshal.Copy(statePtr, stateArray, 0, numKeys);
        if (stateArray[(int)SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE] != 0)
        {
            mIsRunning = false;
        }

        mScene.ProcessInput(statePtr);
    }

    private void GenerateOutput()
    {
        SDL.SDL_SetRenderDrawColor(mRenderer, 19, 56, 111, 255);
        SDL.SDL_RenderClear(mRenderer);

        foreach (var sprite in mSprites)
        {
            sprite.Draw(mRenderer);
        }

        SDL.SDL_RenderPresent(mRenderer);
    }

    public nint LoadTexture(string fileName)
    {
        nint tex = IntPtr.Zero;
        if (mCashedTextures.ContainsKey(fileName)) {
            tex = mCashedTextures[fileName];
        }
        else
        {
            var surf = SDL_image.IMG_Load(fileName);
            if (surf == 0)
            {
                SDL.SDL_Log($"Error load texture file {fileName}");
                Console.WriteLine(SDL.SDL_GetError());
                return 0;
            }
            tex = SDL.SDL_CreateTextureFromSurface(mRenderer, surf);
            SDL.SDL_FreeSurface(surf);
            if (tex == 0)
            {
                SDL.SDL_Log($"Error convert surface to texture {fileName}");
                return 0;
            }

            mCashedTextures.Add(fileName, tex);
        }
        return tex;
    }

    public Scene GetNextScene() { return mNextScene; }
    public void SetNextScene(Scene scene) { mNextScene = scene; }
    
    public Scene GetScene() { return mScene; }

    public Ship GetShip() { return mShip; }

    public List<Enemy> GetEnemies() { return mEnemies; }
    public string GetAssetsPath() { return AssetsPath; }
    public bool GetGameClear() { return mGameClear; }
    public void SetGameClear( bool isClear) { mGameClear = isClear; }

    public List<Actor> GetActors() { return mActors; }

    public void SetShip(Ship ship) { mShip = ship; }
}
