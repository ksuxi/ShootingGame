using SDL2;

class Scene
{
    protected Game mGame;
    public Scene(Game game)
    {
        mGame = game; ;
    }

    public virtual void Start()
    {

    }

    public virtual void ProcessInput(nint state)
    {

    }

    public virtual void Update(float deltaTime)
    {

    }

    public virtual string GetSceneName()
    {
        return "";
    }
}