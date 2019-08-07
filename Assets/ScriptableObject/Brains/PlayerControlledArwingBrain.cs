using UnityEngine;

[CreateAssetMenu(menuName = "Brains/Player Controlled Arwing")]
public class PlayerControlledArwingBrain : ArwingBrain
{

    public int PlayerNumber;
    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private string m_FireButton;
    private string m_foxForceFour;

    public void OnEnable()
    {
        m_MovementAxisName = "Vertical" + PlayerNumber;
        m_TurnAxisName = "Horizontal" + PlayerNumber;
        m_FireButton = "Fire" + PlayerNumber;
    }

    public override void Think(ArwingThinker arwingThinker)
    {
        arwingThinker.Remember("asdf", "asdf");
        // Debug.Log("Yo");
        // var movement = arwingThinker.GetComponent<TankMovement>();

        // movement.Steer(Input.GetAxis(m_MovementAxisName), Input.GetAxis(m_TurnAxisName));

        // var shooting = arwingThinker.GetComponent<TankShooting>();

        // if (Input.GetButton(m_FireButton))
        //     shooting.BeginChargingShot();
        // else
        //     shooting.FireChargedShot();
    }

    public override void Move(ArwingThinker arwingThinker)
    {
        var movement = arwingThinker.GetComponent<ArwingMovement>();
        float h = movement.joystick ? Input.GetAxis("Horizontal") : Input.GetAxis("Mouse X");
        float v = movement.joystick ? Input.GetAxis("Vertical") : Input.GetAxis("Mouse Y");

        movement.LocalMove(h, v, movement.xySpeed);
        movement.RotationLook(h, v, movement.lookSpeed);
        movement.HorizontalLean(h, 80, .1f);

        if (Input.GetButtonDown("Action"))
            movement.Boost(true);

        if (Input.GetButtonUp("Action"))
            movement.Boost(false);

        if (Input.GetButtonDown("Fire3"))
            movement.Break(true);

        if (Input.GetButtonUp("Fire3"))
            movement.Break(false);

        if (Input.GetButtonDown("TriggerL") || Input.GetButtonDown("TriggerR"))
        {
            int dir = Input.GetButtonDown("TriggerL") ? -1 : 1;
            movement.QuickSpin(dir);
        }
    }
}