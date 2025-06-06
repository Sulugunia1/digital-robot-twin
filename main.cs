using UnityEngine;

public class TankController : MonoBehaviour
{
    [Header("Sensors")]
    public HCSR04 us;
    public E18D50NK irL;
    public E18D50NK irC;
    public E18D50NK irR;
    public ST188 lineL;
    public ST188 lineR;

    [Header("Movement Settings")]
    public float maxFwSpeed = 5f;
    public float maxBwSpeed = 3f;
    public float acc = 3f;
    public float brake = 6f;
    public float rotate = 80f;
    public float safeDist = 5f;

    private float currSpeed;
    private float targSpeed;
    private float rotateInput;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        // Настройка физики
        rb.mass = 100f;
        rb.linearDamping = 2f;
        rb.angularDamping = 5f;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        HandleInput();
        ProcessSensors();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleInput()
    {
        // Разгон
        if (Input.GetKey(KeyCode.W))
        {
            targSpeed = Mathf.Min(targSpeed + acc * Time.deltaTime, maxFwSpeed);
        }
        // Торможение и назад
        else if (Input.GetKey(KeyCode.S))
        {
            targSpeed = Mathf.Max(targSpeed - acc * Time.deltaTime, -maxBwSpeed);
        }
        // остановка
        else
        {
            targSpeed = Mathf.Lerp(targSpeed, 0, Time.deltaTime * 3f);
        }

        // Поворот
        rotateInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
    }

    private void ProcessSensors()
    {
        // Проверка передних препятствий при движении вперед
        if (targSpeed > 0)
        {
            if (us.GetDistance() < safeDist ||
                (irC.IsObstacleDetected() && irC.GetDistance() < safeDist))
            {
                targSpeed = 0;
                Debug.Log("fwd obstacle");
            }
        }

        // Проверка боковых препятствий
        if (targSpeed > 0)
        {
            if ((irL.IsObstacleDetected() && irL.GetDistance() < safeDist) || 
                (irR.IsObstacleDetected() && irR.GetDistance() < safeDist))
            {
                targSpeed = 0;
                Debug.Log("side obstacle");
            }
        }
    }

    private void ApplyMovement()
    {
        // Плавное изменение скорости
        currSpeed = Mathf.Lerp(currSpeed, targSpeed, Time.deltaTime * 5f);

        // Движение вперед/назад по оси X
        Vector3 moveDirection = transform.right * currSpeed;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);

        // Поворот с учетом направления движения
        if (Mathf.Abs(rotateInput) > 0.1f)
        {
            float directionMultiplier = currSpeed >= 0 ? 1f : -1f;
            float rotation = rotateInput * rotate * Time.fixedDeltaTime * directionMultiplier;
            transform.Rotate(0, rotation, 0);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"Speed: {currSpeed:F3}");
        GUI.Label(new Rect(10, 30, 300, 20), $"Front Distance: {us.GetDistance():F2}m");
        GUI.Label(new Rect(10, 70, 300, 20), $"left dist: {irL.GetDistance():F2}m");
        GUI.Label(new Rect(10, 90, 300, 20), $"right dist: {irR.GetDistance():F2}m");
        GUI.Label(new Rect(10, 110, 300, 20), $"centr dist: {irC.GetDistance():F2}m");
    }
}