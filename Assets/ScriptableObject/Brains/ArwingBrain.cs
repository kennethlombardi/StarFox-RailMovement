using UnityEngine;

public abstract class ArwingBrain : ScriptableObject
{
    public virtual void Initialize(ArwingThinker arwing) { }
    public abstract void Think(ArwingThinker arwing);
}
