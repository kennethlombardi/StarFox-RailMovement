using UnityEngine;

[CreateAssetMenu(menuName = "Brains/Wingman Arwing Brain")]
public class WingmanArwingBrain : ArwingBrain
{

    public int PlayerNumber;
    private bool m_foxForceFour;
    private bool m_horizontalSplit;

    public void OnEnable()
    {
    }

    public override void Think(ArwingThinker arwingThinker)
    {
    }

    public override void Move(ArwingThinker arwingThinker)
    {
    }
}