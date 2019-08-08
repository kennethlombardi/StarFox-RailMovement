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
            // foxForceFourTacticNodePosition.y += 15;
            Vector3 arwingPosition = arwingThinker.GetComponent<Transform>().transform.localPosition;
            Vector3 diff = foxForceFourTacticNodePosition - arwingPosition;
            Debug.Log(diff);
            // diff *= .2
            // diff = Vector2.ClampMagnitude(diff, 1);
            h = diff.x;
            v = diff.y;
            // h = -0.5f;
            // v = 0.5f;
        }
        else if (m_horizontalSplit)
        {
            h = -.5f;
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