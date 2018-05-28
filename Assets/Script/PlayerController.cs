using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float healthBar = 1f;
    [SerializeField]
    private float healthBarRegenSpeed = 0.1f;
    [SerializeField]
    private float healthBarAmount = 1f;

    private float timeNextJump;

    [Header("Spring settings:")]
	[SerializeField]
	private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    private PlayerMotor motor;
	private ConfigurableJoint joint;
   
	void Start ()
	{
		motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint>();

		SetJointSettings(jointSpring);

        timeNextJump = Time.time;
    }

	void Update ()
    {
        if (PrincipalPauseMenu.isOn || !GetComponent<Player>().ReadyToPlay)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;

            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);

            return;
        }

        if(Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Set target pos quand on jump sur une surface
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
        {
            joint.targetPosition = new Vector3(0f, -hit.point.y, 0f);
        } else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

		//Calculate movement velocity as a 3D vector
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		// Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

		//Apply movement
		motor.Move(_velocity);

		//Calculate rotation as a 3D vector (turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		//Apply rotation
		motor.Rotate(_rotation);

		//Calculate camera rotation as a 3D vector (turning around)
		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		//Apply camera rotation
		motor.RotateCamera(_cameraRotationX);

		// Calculate the thrusterforce based on player input
		Vector3 _thrusterForce = Vector3.zero;

		if (Input.GetButtonDown("Jump") && timeNextJump < Time.time)
        {
            timeNextJump = Time.time + .6f;
            _thrusterForce = Vector3.up * thrusterForce;
			SetJointSettings(0f);
        } else
		{
			SetJointSettings(jointSpring);
		}

		// Apply the thruster force
		motor.ApplyThruster(_thrusterForce);

	}

	private void SetJointSettings (float _jointSpring)
	{
		joint.yDrive = new JointDrive {
			positionSpring = _jointSpring,
			maximumForce = jointMaxForce
		};
	}

}
