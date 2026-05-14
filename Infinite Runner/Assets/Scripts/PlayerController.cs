using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Lanes")]
    [SerializeField] private float laneOffset = 2f;
    [SerializeField, Min(1)] private int laneCount = 3;
    [SerializeField] private float laneSwitchSpeed = 14f;

    [Header("Jump")]
    [SerializeField] private float jumpVelocity = 8f;
    [SerializeField] private float gravity = -25f;

    private int _laneIndex;
    private float _y;
    private float _yVel;
    private Vector2 _prevMove;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.3f;

    private float _groundY;

    void Awake()
    {
        // We drive position directly, so any rigidbody on this object must be kinematic.
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        // Stop input after game over
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        Vector2 v = ctx.ReadValue<Vector2>();

        if (v.x > 0.5f && _prevMove.x <= 0.5f)
            ChangeLane(+1);
        else if (v.x < -0.5f && _prevMove.x >= -0.5f)
            ChangeLane(-1);

        // if (v.y > 0.5f && _prevMove.y <= 0.5f && _y <= 0f)
        //     _yVel = jumpVelocity;

        if (v.y > 0.5f && IsGrounded())
        {
            _yVel = jumpVelocity;
        }

        _prevMove = v;
    }

    private void ChangeLane(int delta)
    {
        int half = laneCount / 2;
        _laneIndex = Mathf.Clamp(_laneIndex + delta, -half, half);
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        return Physics.Raycast(ray, groundCheckDistance, groundLayer);
    }

    void Update()
    {
        // Stop movement after game over
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        // _yVel += gravity * Time.deltaTime;
        // _y += _yVel * Time.deltaTime;

        // if (_y < 0f)
        // {
        //     _y = 0f;
        //     _yVel = 0f;
        // }

        _yVel += gravity * Time.deltaTime;
        _y += _yVel * Time.deltaTime;

        // Ground detection
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            float targetGroundY = hit.point.y;

            if (_y <= targetGroundY)
            {
                _y = targetGroundY;
                _yVel = 0f;
            }
        }
        
        Vector3 pos = transform.position;

        pos.x = Mathf.MoveTowards(
            pos.x,
            _laneIndex * laneOffset,
            laneSwitchSpeed * Time.deltaTime
        );

        pos.y = _y;
        pos.z = 0f;

        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}