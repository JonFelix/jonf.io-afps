using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerQMovement : MonoBehaviour
{
    Transform PlayerView; // Must be a camera
    public float PlayerViewYOffset = 0.6f; // The height at which the camera is bound to
    public float XSensitivity = 30.0f;
    public float YSensitivity = 30.0f;
    /* Frame occuring factors */
    public float Gravity = 20.0f;
    public float Friction = 6f; // Ground friction
    /* Movement stuff */
    public float MoveSpeed = 7.0f; // Ground move speed
    public float RunAcceleration = 14f; // Ground accel
    public float RunDeacceleration = 10f; // Deacceleration that occurs when running on the ground
    public float AirAcceleration = 2.0f; // Air accel
    public float AirDeacceleration = 2.0f; // Deacceleration experienced when opposite strafing
    public float AirControlFloat = 0.3f; // How precise air control is
    public float SideStrafeAcceleration = 50f; // How fast acceleration occurs to get up to sideStrafeSpeed when side strafing
    public float SideStrafeSpeed = 1f; // What the max speed to generate when side strafing
    public float JumpSpeed = 8.0f; // The speed at which the character's up axis gains when hitting jump
    public float MoveScale = 1.0f;
    public float ClimbableSlope = 0.2f;
    /* Sound stuff */
    public AudioClip[] JumpSounds;
    /* FPS Stuff */
    public float FpsDisplayRate = 4.0f; // 4 updates per sec.

    private float frameCount = 0f;
    private Vector3 mPlayerInput;
    private float dt = 0.0f;
    private float fps = 0.0f;
    private CharacterController controller;
    // Camera rotationals
    private float rotX = 0.0f;
    private float rotY = 0.0f;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 moveDirectionNorm  = Vector3.zero;
    private Vector3 playerVelocity = Vector3.zero;
    private float playerTopVelocity = 0.0f;
    // If true then the player is fully on the ground
    private bool grounded = false;
    // Q3: players can queue the next jump just before he hits the ground
    private bool wishJump = false;
    // Used to display real time friction values
    private float slope = 0f;
    private Vector3 previousPosition;
    private CharacterController mController;
    private Vector3 mExplosionVelocity;
    private Vector3 mExplosionActual;
    private bool mCheckedCeiling = false;
    private int mCeilingCheckCount = 0;
    private float mOriginalRunSpeed;

    private float playerFriction = 0.0f;

    CapsuleCollider mCollider;
    Text mUPSDisplay;
    Text mVelDisplay;
    InterfaceIngameOptions mOptions;

    public Vector3 Velocity
    {
        get
        {
            return playerVelocity;
        }
        set
        {
            playerVelocity = value;
        }
    }

    public Vector3 ExplosionVelocity
    {
        set
        {
            mExplosionVelocity += value;
        }
    }

    public float RunSpeed
    {
        get
        {
            return MoveSpeed;
        }
        set
        {
            MoveSpeed = value;
        }
    }

    public void ResetRunSpeed()
    {
        MoveSpeed = mOriginalRunSpeed;
    }

    void Start()
    {
        mPlayerInput = Vector3.zero;
        mCollider = GetComponent<CapsuleCollider>();
        PlayerView = transform.Find("PlayerCamera").transform;
        PlayerView.position = new Vector3(transform.position.x, transform.position.y + PlayerViewYOffset, transform.position.z);
        mUPSDisplay = GameObject.Find("UPSDisplay").GetComponent<Text>();
        mController = GetComponent<CharacterController>();
        mVelDisplay = GameObject.Find("VelocityDebugDisplay").GetComponent<Text>();
        mOptions = GetComponent<InterfaceIngameOptions>();
        mOriginalRunSpeed = RunSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!mOptions.InOptions)
        {
            if(Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if(Cursor.visible)
            {
                Cursor.visible = false;
            }
        }
        else
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if(!Cursor.visible)
            {
                Cursor.visible = true;
            }
        }

        if(GetComponent<NetworkView>().isMine)
        {
            XSensitivity = PlayerPrefs.GetFloat("HorizontalSensitivity");
            YSensitivity = PlayerPrefs.GetFloat("VerticalSensitivity");
            if(!mOptions.InOptions)
            {
                rotX -= Input.GetAxis("Mouse Y") * XSensitivity * 0.02f;
                rotY += Input.GetAxis("Mouse X") * YSensitivity * 0.02f;
                rotX = Mathf.Clamp(rotX, -90f, 90f);
                transform.rotation = Quaternion.Euler(0f, rotY, 0f);
                PlayerView.rotation = Quaternion.Euler(rotX, rotY, 0f);
                PlayerView.position = new Vector3(transform.position.x, PlayerView.position.y, transform.position.z);
                QueueJump();
            }


            if(mController.isGrounded)
            {
                GroundMove();
            }
            else
            {
                AirMove();
            }

            if(mExplosionVelocity != Vector3.zero)
            {
                AddExplosionVelocity();
            }

            CheckCeilingCollision();
            mController.Move(playerVelocity * Time.deltaTime);
            if(mUPSDisplay != null)
            {
                mUPSDisplay.text = Mathf.Round(playerVelocity.magnitude * 10f).ToString() + "UPS";
                mVelDisplay.text = "x:" + playerVelocity.x.ToString() + "\ny:" + playerVelocity.y.ToString() + "\nz:" + playerVelocity.z.ToString();
            }
        }
        else
        {
            NetworkSerializedStream mStream = GetComponent<NetworkSerializedStream>();
            mStream.SyncTime += Time.deltaTime;
            if(Vector3.Lerp(mStream.StartPosition, mStream.EndPosition, mStream.SyncTime / mStream.SyncDelay) != Vector3.zero)
            {
                transform.position = Vector3.Lerp(mStream.StartPosition, mStream.EndPosition, mStream.SyncTime / mStream.SyncDelay);
            }
        }
    }

    void GetPlayerInput()
    {
        if(!mOptions.InOptions)
        {
            mPlayerInput = new Vector3(Input.GetAxis("Horizontal") * MoveSpeed, Input.GetAxis("Jump"), Input.GetAxis("Vertical") * MoveSpeed);
        }
        else
        {
            mPlayerInput = Vector3.zero;
        }
    }

    //**********************************************
    //*         MOVEMENT
    //**********************************************

    void QueueJump()
    {
        if(mPlayerInput.y > 0f && !wishJump)
        {
            wishJump = true;
        }
        if(mPlayerInput.y <= 0f)
        {
            wishJump = false;
        }
    }

    //***
    //* Air Movement
    //***
    void AirMove()
    {
        float wishvel = AirAcceleration;
        float Accel;
        float scale = Scale();
        GetPlayerInput();
        Vector3 wishdir = new Vector3(mPlayerInput.x, 0, mPlayerInput.z);
        wishdir = transform.TransformDirection(wishdir);
        float wishspeed = wishdir.magnitude;
        wishspeed *= MoveSpeed;
        wishdir.Normalize();
        moveDirectionNorm = wishdir;
        wishspeed *= scale;
        // CPM: Aircontrol
        float wishspeed2 = wishspeed;
        if(Vector3.Dot(playerVelocity, wishdir) < 0)
        {
            Accel = AirDeacceleration * Time.deltaTime;
        }
        else
        {
            Accel = AirAcceleration * Time.deltaTime;
        }
        // If the player is ONLY strafing left or right
        if(mPlayerInput.z == 0 && mPlayerInput.x != 0)
        {
            if(wishspeed > SideStrafeSpeed)
            {
                wishspeed = SideStrafeSpeed;
            }
            Accel = SideStrafeAcceleration;
            Accelerate(wishdir, wishspeed, Accel);
        }

        if(AirControlFloat != 0f)
        {
            AirControl(wishdir, wishspeed2);
        }
        // !CPM: Aircontrol
        // Apply gravity
        playerVelocity.y -= Gravity * Time.deltaTime;
    }

    void AirControl(Vector3 wishdir, float wishspeed)
    {
        float zspeed;
        float speed;
        float dot;
        float k;
        // Can't control movement if not moving forward or backward
        if(mPlayerInput.z == 0 || wishspeed == 0)
        {
            return;
        }
        else
        {
            zspeed = playerVelocity.y;
            playerVelocity.y = 0;
            /* Next two lines are equivalent to idTech's VectorNormalize() */
            speed = playerVelocity.magnitude;
            playerVelocity.Normalize();
            dot = Vector3.Dot(playerVelocity, wishdir);
            k = 32;
            k *= AirControlFloat * dot * dot * Time.deltaTime;
            // Change direction while slowing down
            if(dot > 0)
            {
                playerVelocity.x = playerVelocity.x * speed + wishdir.x * k;
                playerVelocity.y = playerVelocity.y * speed + wishdir.y * k;
                playerVelocity.z = playerVelocity.z * speed + wishdir.z * k;
                playerVelocity.Normalize();
                moveDirectionNorm = playerVelocity;
            }
            playerVelocity.x *= speed;
            playerVelocity.y = zspeed; // Note this line
            playerVelocity.z *= speed;
        }
    }

    //***
    //* Grounded Movement
    //***

    void GroundMove()
    {
        Vector3 wishdir;
        // Do not apply friction if the player is queueing up the next jump
        if(!wishJump)
        {
            ApplyFriction(1.0f);
        }
        else
        {
            ApplyFriction(0f);
        }

        GetPlayerInput();
        float scale = Scale();
        wishdir = new Vector3(mPlayerInput.x, 0, mPlayerInput.z);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();
        moveDirectionNorm = wishdir;
        float wishspeed = wishdir.magnitude;
        wishspeed *= MoveSpeed;
        Accelerate(wishdir, wishspeed, RunAcceleration);
        // Reset the gravity velocity
        playerVelocity.y = 0;
        if(wishJump)
        {
            playerVelocity.y = JumpSpeed;
            wishJump = false;
            PlayJumpSound();
        }
    }

    void ApplyFriction(float t)
    {
        Vector3 vec = playerVelocity; // Equivalent to: VectorCopy();
        float speed;
        float newspeed;
        float control;
        float drop;
        vec.y = 0.0f;
        speed = vec.magnitude;
        drop = 0.0f;
        /* Only if the player is on the ground then apply friction */
        if(mController.isGrounded)
        {
            control = speed < RunDeacceleration ? RunDeacceleration : speed;
            drop = control * Friction * Time.deltaTime * t;
        }
        newspeed = speed - drop;
        playerFriction = newspeed;
        if(newspeed < 0)
        {
            newspeed = 0;
        }
        if(speed > 0)
        {
            newspeed /= speed;
        }
        playerVelocity.x *= newspeed;
        playerVelocity.y *= newspeed;
        playerVelocity.z *= newspeed;
    }

    float Scale()
    {
        int max;
        float total;
        float scale;
        max = Mathf.Abs((int)mPlayerInput.z);
        if(Mathf.Abs((int)mPlayerInput.x) > max)
        {
            max = Mathf.Abs((int)mPlayerInput.x);
        }
        if(max == 0)
        {
            return 0;
        }
        total = Mathf.Sqrt(mPlayerInput.z * mPlayerInput.z + mPlayerInput.x * mPlayerInput.x);
        scale = MoveSpeed * max / (MoveScale * total);
        return scale;
    }

    void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addspeed;
        float accelspeed;
        float currentspeed;
        currentspeed = Vector3.Dot(playerVelocity, wishdir);
        addspeed = wishspeed - currentspeed;
        if(addspeed <= 0)
        {
            return;
        }
        accelspeed = accel * Time.deltaTime * wishspeed;
        if(accelspeed > addspeed)
        {
            accelspeed = addspeed;
        }
        playerVelocity.x += accelspeed * wishdir.x;
        playerVelocity.z += accelspeed * wishdir.z;
        playerVelocity.y += accel * wishdir.y;
    }

    //**********************************************
    //*         SURROUNDINGS
    //**********************************************

    void CheckFloor()
    {
        Ray mRay = new Ray(transform.position, -transform.up);
        if(Physics.Raycast(mRay, mCollider.bounds.extents.y + 0.1f))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

    }

    void AddExplosionVelocity()
    {

        if(Vector3.Distance(Vector3.zero, mExplosionActual) < Vector3.Distance(Vector3.zero, mExplosionVelocity))
        {

            mExplosionActual += Vector3.Lerp(mExplosionActual, mExplosionVelocity, 10 * Time.deltaTime);
            playerVelocity += mExplosionActual;
        }
        else
        {
            mExplosionVelocity = Vector3.zero;
            mExplosionActual = Vector3.zero;
        }
    }

    void CheckCeilingCollision()
    {
        if(mController.collisionFlags == CollisionFlags.Above)
        {
            mExplosionActual = Vector3.zero;
            mExplosionVelocity = Vector3.zero;
            playerVelocity.y = Mathf.Min(playerVelocity.y, 0f);
            print("Touched Ceiling!");
            mCheckedCeiling = true;
        }
        if(!mCheckedCeiling)
        {

        }
        else
        {
            mCeilingCheckCount++;

            if(mCeilingCheckCount > 3)
            {
                mCheckedCeiling = false;
                mCeilingCheckCount = 0;
            }
        }
    }

    void PlayJumpSound()
    {
        AudioSource mAudio = GetComponent<AudioSource>();
        if(mAudio.isPlaying)
        {
            return;
        }
        mAudio.clip = JumpSounds[Random.Range(0, JumpSounds.Length)];
        mAudio.Play();
    }

}
