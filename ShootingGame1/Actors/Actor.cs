using System.Numerics;

class Actor : IDisposable
{
    public enum State
    {
        EActive,
        EDead
    }
    private State mState;
    private Vector2 mPosition;
    private float mScale;
    private float mRotation;
    private Game mGame;
    private List<Component> mComponents = new List<Component>();
    public Actor(Game game)
    {
        mState = State.EActive;
        mPosition = Vector2.Zero;
        mScale = 1.0f;
        mRotation = 0.0f;
        mGame = game;
        mGame.AddActor(this);
    }

    public virtual void Dispose()
    {
        mGame.RemoveActor(this);
        while (mComponents.Count > 0)
        {
            mComponents[0].Dispose();
        }
    }

    public void Update(float deltaTime)
    {
        if (mState == State.EActive)
        {
            UpdateComponents(deltaTime);
            UpdateActor(deltaTime);
        }
    }

    public void UpdateComponents(float deltaTime)
    {
        foreach (var component in mComponents)
        {
            component.Update(deltaTime);
        }
    }

    public virtual void UpdateActor(float deltaTime)
    {

    }

    public void AddComponent(Component component)
    {
        int myOrder = component.GetUpdateOrder();
        int index = 0;
        for (; index < mComponents.Count; ++index)
        {
            if (myOrder < mComponents[index].GetUpdateOrder())
            {
                break;
            }
        }
        mComponents.Insert(index, component);
    }

    public void RemoveComponent(Component comonent)
    {
        mComponents.Clear();
    }

    public virtual void SetPosition(Vector2 pos)
    {
        mPosition = pos;
    }


    public State GetState() { return mState; }
    public void SetState(State state) { mState = state; }
    public Vector2 GetPosition() { return mPosition; }
    public float GetScale() { return mScale; }
    public void SetScale(float scale) { mScale = scale; }
    public float GetRotation() { return mRotation; }
    public Game GetGame() { return mGame; }
}