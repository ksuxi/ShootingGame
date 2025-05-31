using System.ComponentModel;

class Component : IDisposable
{
    protected Actor mActor;
    protected int mUpdateOrder;
    public Component(Actor actor, int updateOrder = 100)
    {
        mActor = actor;
        mUpdateOrder = updateOrder;
        mActor.AddComponent(this);
    }

    public virtual void Dispose()
    {
        mActor.RemoveComponent(this);
    }

  
    public virtual void Update(float deltaTime)
    {

    }

    public int GetUpdateOrder()  { return mUpdateOrder; }
}
