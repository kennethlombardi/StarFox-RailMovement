using UnityEngine;

[CreateAssetMenu(menuName = "Brains/Wingman Arwing Brain")]
public class WingmanArwingBrain : ArwingBrain
{

    public int PlayerNumber;
    private bool m_foxForceFour = false;
    private bool m_horizontalSplit = false;
    private float h = 0;
    private float v = 0;

    public override void Initialize(ArwingThinker arwing)
    {
        h = 0;
        v = 0;
    }

    public void OnEnable()
    {
    }

    public override void Think(ArwingThinker arwingThinker)
    {
        m_foxForceFour = arwingThinker.Remember<bool>("foxForceFour");
        m_horizontalSplit = arwingThinker.Remember<bool>("horizontalSplit");

        if (m_foxForceFour)
        {
            Vector3 foxForceFourTacticNodePosition = arwingThinker.foxForceFourTacticNode.transform.localPosition;
            Vector3 arwingPosition = arwingThinker.GetComponent<Transform>().transform.localPosition;
            Vector3 diff = foxForceFourTacticNodePosition - arwingPosition;
            h = diff.x;
            v = diff.y;
        }
        else if (m_horizontalSplit)
        {
            Vector3 horizontalSplitTacticNodePosition = arwingThinker.horizontalSplitTacticNode.transform.localPosition;
            Vector3 arwingPosition = arwingThinker.GetComponent<Transform>().transform.localPosition;
            Vector3 diff = horizontalSplitTacticNodePosition - arwingPosition;
            h = diff.x;
            v = diff.y;
        }

        if (GameObject.Equals(arwingThinker.collidedWith, arwingThinker.foxForceFourTacticNode))
        {
            m_foxForceFour = false;
        }

        // if (GameObject.Equals(arwingThinker.collidedWith, arwingThinker.ho))
    }

    public override void Move(ArwingThinker arwingThinker)
    {
        var movement = arwingThinker.GetComponent<ArwingMovement>();
        movement.LocalMove(h, v, movement.xySpeed);
        movement.RotationLook(h, v, movement.lookSpeed);
        movement.HorizontalLean(h, 80, .1f);
    }
}