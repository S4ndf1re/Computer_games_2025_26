using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        [SerializeField] private float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        [SerializeField] private float SprintSpeed = 5.335f;

        [Tooltip("Crouched speed of the character in m/s")]
        [SerializeField] private float CrouchSpeed = 1.5f;

        [Tooltip("Crouched sprint speed of the character in m/s")]
        [SerializeField] private float CrouchSprintSpeed = 3.0f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float SpeedChangeRate = 10.0f;

        [SerializeField] private AudioClip LandingAudioClip;
        [SerializeField] private AudioClip[] FootstepAudioClips;
        [Range(0, 1)] [SerializeField] private float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        [SerializeField] private float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        [SerializeField] private float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField] private float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField] private float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        private bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        [SerializeField] private float GroundedOffset = -0.14f;

        [Tooltip("What layers the character uses as ground")]
        [SerializeField] private LayerMask GroundLayers;

        [Header("Cinemachine")]
        [SerializeField] private GameObject CinemachineCameraTarget;
        [SerializeField] private float TopClamp = 70.0f;
        [SerializeField] private float BottomClamp = -30.0f;
        [SerializeField] private float CameraAngleOverride = 0.0f;
        [SerializeField] private bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private bool _isCrawling = false;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDPunch;
        private int _animIDPush;
        private int _animIDCrawl;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private List<CharacterController> _allHandControllers = new List<CharacterController>();
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private CinemachineCamera _theCinemachineCamera;

        private const float _threshold = 0.01f;
        private bool _hasAnimator;

        // -----------------------
        // Erweiterte Movement-Fähigkeiten
        // -----------------------
        [Header("Advanced Movement Settings")]
        [SerializeField] private float CoyoteTime = 0.15f;
        [SerializeField] private float MaxJumpHoldTime = 0.35f;
        [SerializeField] private float HoldJumpGravityMultiplier = 0.3f;

        [Header("Dash Settings")]
        [SerializeField] private bool CanDash = true;
        [SerializeField] private float DashSpeed = 15f;
        [SerializeField] private float DashDuration = 0.2f;
        [SerializeField] private float DashCooldown = 0.5f;

        [Header("Wall Climb Settings")]
        [SerializeField] private float WallCheckDistance = 0.6f;
        [SerializeField] private float WallSlideSpeed = 2f;
        [SerializeField] private float WallJumpForce = 7f;
        [SerializeField] private Vector2 WallJumpAngle = new Vector2(1f, 1.5f);
        [SerializeField] private LayerMask WallLayer;
        [SerializeField] private float WallSlideMaxTime = 2f;

        [Header("Double Jump Settings")]
        [SerializeField] private bool CanDoubleJump = true;
        [SerializeField] private float DoubleJumpHeight = 1.0f;
        [SerializeField] private float DoubleJumpDelay = 0.15f;

        private float _doubleJumpDelayTimer = 0f;
        private bool _hasDoubleJumped = false;
        private bool _isDashing = false;
        private float _dashEndTime = 0f;
        private float _lastDashTime = -999f;
        private Vector3 _dashDirection;

        private bool _isTouchingWall;
        private bool _isWallSliding;
        private bool _canWallJump;
        private float _wallSlideStartTime;
        private Vector3 _lastWallNormal;

        private float _lastGroundedTime;
        private bool _isHoldingJump;
        private float _jumpHoldTimeCounter;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            if (_mainCamera == null)
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            _theCinemachineCamera = GetComponentInChildren<CinemachineCamera>();
            if (_theCinemachineCamera == null)
                Debug.LogWarning("No CinemachineCamera found!");
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _cinemachineTargetPitch = CinemachineCameraTarget.transform.rotation.eulerAngles.x;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#endif
            AssignAnimationIDs();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {

            _hasAnimator = TryGetComponent(out _animator);

            float _mouseWheelDelta = Input.mouseScrollDelta.y;
            if (_mouseWheelDelta != 0.0f)
                Zoom(_mouseWheelDelta);

            GroundedCheck();

            HandleDash();
            HandleWallSlide();

            HandleDoubleJump();
            JumpAndGravity();

            Punch();
            Push();
            Crawl();
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDPunch = Animator.StringToHash("Punch");
            _animIDPush = Animator.StringToHash("Push");
            _animIDCrawl = Animator.StringToHash("Crawl");
        }


        /// <summary>
        /// Checks whether a player is in contact with a ground layer.
        /// </summary>
        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, _controller.radius, GroundLayers, QueryTriggerInteraction.Ignore);

            if (_hasAnimator)
                _animator.SetBool(_animIDGrounded, Grounded);
        }

        /// <summary>
        /// Rotates Player Camera based on the mouse input around the player.
        /// </summary>
        private void CameraRotation()
        {
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }


        /// <summary>
        /// Handles general movement
        /// </summary>
        private void Move()
        {
            float targetSpeed = MoveSpeed;

            //sets speed based on input permutations
            if (_input.sprint && _isCrawling) targetSpeed = CrouchSprintSpeed;
            if (_input.sprint && !_isCrawling && Grounded) targetSpeed = SprintSpeed;
            if (!_input.sprint && _isCrawling) targetSpeed = CrouchSpeed;
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            //absolute speed scalar of 3D speed vector
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;

            //if gamepad is active: use input magnitude. For Keyboard input this is set to 1.
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // Smoothing the velocity change (Acceleration)
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                //interpolates velocity smoothly
                _speed = Mathf.Lerp(_speed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                //rounds speed to avoid floating point errors
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                //if we are already at max speed we dont need to accelerate
                _speed = targetSpeed;
            }

            // Smoothing the Animation Changes
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // Gets direction of input
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // Rotates Character in input-Direction
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            // quest mode
            if (LockCameraPosition && _input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

                _targetRotation -= 180.0f;

                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                //rotation -= Mathf.PI;
                // rotate to face input direction relative to camera position
                //transform.rotation *= Quaternion.Euler(0, -90, 0);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // Updates position
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        /// <summary>
        /// Handles Dash-Ability
        /// </summary>
        private void HandleDash()
        {
            // Checks if ability is unlocked
            if (!CanDash)
                return;

            // Only dash while in air
            if (Grounded)
                return;

            // If already dashing only update
            if (_isDashing)
            {
                _controller.Move(_dashDirection * DashSpeed * Time.deltaTime);

                // End Dash
                if (Time.time >= _dashEndTime)
                    _isDashing = false;

                return;
            }

            // Start Dash when airborne and not already dashing
            if (_input.sprint && !_isDashing && Time.time - _lastDashTime >= DashCooldown)
            {
                _lastDashTime = Time.time;
                _isDashing = true;
                _dashEndTime = Time.time + DashDuration;

                _dashDirection = transform.forward.normalized;
                _dashDirection.y = 0f;

                _controller.Move(_dashDirection * DashSpeed * 10f * Time.deltaTime);

                if (_hasAnimator)
                    _animator.SetTrigger("Dash");
            }
        }

        /// <summary>
        /// Allows the player to perform a second jump while airborne,
        /// but only after a short delay following the initial jump.
        /// </summary>
        private void HandleDoubleJump()
        {
            if (!CanDoubleJump)
                return;

            if (_doubleJumpDelayTimer > 0f)
                return;

            if (!Grounded && !_isWallSliding && !_hasDoubleJumped && _input.jump)
            {
                Debug.Log("DOUBLE JUMP!");

                _verticalVelocity = Mathf.Sqrt(DoubleJumpHeight * -2f * Gravity);
                _isHoldingJump = true;
                _jumpHoldTimeCounter = 0f;
                _hasDoubleJumped = true;

                if (_hasAnimator)
                    _animator.SetBool(_animIDJump, true);

                _input.jump = false;
            }
        }


        /// <summary>
        /// Handles the Wall Slide
        /// </summary>
        private void HandleWallSlide()
        {
            RaycastHit hit;

            bool forwardHit = Physics.Raycast(transform.position, transform.forward, out hit, WallCheckDistance, WallLayer);
            bool backHit    = Physics.Raycast(transform.position, -transform.forward, out hit, WallCheckDistance, WallLayer);
            bool rightHit   = Physics.Raycast(transform.position, transform.right, out hit, WallCheckDistance, WallLayer);
            bool leftHit    = Physics.Raycast(transform.position, -transform.right, out hit, WallCheckDistance, WallLayer);

            bool wallHit = forwardHit || backHit || rightHit || leftHit;

            _isTouchingWall = wallHit;

            if (!Grounded && wallHit && _verticalVelocity < 0)
            {
                if (!_isWallSliding)
                    _wallSlideStartTime = Time.time;

                _isWallSliding = true;

                if (Time.time - _wallSlideStartTime > WallSlideMaxTime)
                {
                    _isWallSliding = false;
                }
            }
            else
            {
                _isWallSliding = false;
            }

            if (_isWallSliding)
            {
                _verticalVelocity = Mathf.Max(_verticalVelocity, -WallSlideSpeed);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _lastGroundedTime = Time.time;
                _fallTimeoutDelta = FallTimeout;

                // Reset Double Jump when grounded
                _hasDoubleJumped = false;

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // Reset slight downward velocity
                if (_verticalVelocity < 0.0f)
                    _verticalVelocity = -2f;

                // Normal jump from ground
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    _isHoldingJump = true;
                    _jumpHoldTimeCounter = 0f;

                    // Starte den Double Jump Delay-Timer
                    _doubleJumpDelayTimer = DoubleJumpDelay;

                    if (_hasAnimator)
                        _animator.SetBool(_animIDJump, true);
                }

                if (_jumpTimeoutDelta >= 0.0f)
                    _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                    _fallTimeoutDelta -= Time.deltaTime;
                else if (_hasAnimator)
                    _animator.SetBool(_animIDFreeFall, true);

                // --- Wall Jump ---
                if (_input.jump && _isWallSliding)
                {
                    _isWallSliding = false;
                    _isHoldingJump = false;

                    Vector3 jumpDir = (_lastWallNormal + Vector3.up).normalized;
                    _verticalVelocity = jumpDir.y * WallJumpForce;
                    Vector3 wallPush = jumpDir * WallJumpForce;
                    _controller.Move(wallPush * Time.deltaTime);

                    if (_hasAnimator)
                        _animator.SetBool(_animIDJump, true);
                    return;
                }

                // --- Coyote Time Jump ---
                if (_input.jump && Time.time - _lastGroundedTime <= CoyoteTime)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    _isHoldingJump = true;
                    _jumpHoldTimeCounter = 0f;

                    // Starte Delay nach Coyote-Sprung ebenfalls
                    _doubleJumpDelayTimer = DoubleJumpDelay;

                    if (_hasAnimator)
                        _animator.SetBool(_animIDJump, true);
                }

                // --- Handle Double Jump ---
                HandleDoubleJump();

                // consume jump input after processing
                _input.jump = false;
            }

            // --- Variable Jump Height (Hold Jump) ---
            if (_isHoldingJump && _jumpHoldTimeCounter < MaxJumpHoldTime && _input.jump)
            {
                _verticalVelocity += Gravity * HoldJumpGravityMultiplier * Time.deltaTime;
                _jumpHoldTimeCounter += Time.deltaTime;
            }
            else if (!_input.jump)
            {
                _isHoldingJump = false;
            }

            // --- Apply Gravity ---
            if (!_isWallSliding && _verticalVelocity < _terminalVelocity)
                _verticalVelocity += Gravity * Time.deltaTime;

            // --- Timer für Double Jump Delay herunterzählen ---
            if (_doubleJumpDelayTimer > 0f)
                _doubleJumpDelayTimer -= Time.deltaTime;
        }


        /// <summary>
        /// Ability to zoom in and out
        /// </summary>
        private void Zoom(float mouseWheelDelta)
        {
            if (_theCinemachineCamera == null) return;
            if (mouseWheelDelta > 0) _theCinemachineCamera.Lens.FieldOfView += 1.0f;
            if (mouseWheelDelta < 0) _theCinemachineCamera.Lens.FieldOfView -= 1.0f;
        }

        /// <summary>
        /// Punch Animation
        /// </summary>
        private void Punch()
        {
            if (_input.punch == false && _hasAnimator)
                _animator.SetBool(_animIDPunch, false);

            if (_input.punch && !_isCrawling)
            {
                if (_hasAnimator)
                    _animator.SetBool(_animIDPunch, true);
                _input.punch = false;
            }
        }

        /// <summary>
        /// Push Animation
        /// </summary>
        private void Push()
        {
            if (_input.push == false && _hasAnimator)
                _animator.SetBool(_animIDPush, false);

            if (_input.push && !_isCrawling)
            {
                if (_hasAnimator)
                    _animator.SetBool(_animIDPush, true);
                _input.push = false;
            }
        }

        /// <summary>
        /// Crawl Animation
        /// </summary>
        private void Crawl()
        {
            if (_input.crawl)
            {
                if (_isCrawling == false)
                {
                    _isCrawling = true;
                    if (_hasAnimator)
                        _animator.SetBool("Crawl", true);
                    _controller.height = 0.88f;
                    _controller.center = new Vector3(0.0f, 0.40f, 0.0f);
                }
                else
                {
                    _isCrawling = false;
                    if (_hasAnimator)
                        _animator.SetBool("Crawl", false);
                    _controller.height = 1.8f;
                    _controller.center = new Vector3(0.0f, 0.93f, 0.0f);
                }
                _input.crawl = false;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color color = Grounded ? new Color(0, 1, 0, 0.35f) : new Color(1, 0, 0, 0.35f);
            Gizmos.color = color;
            #if !UNITY_EDITOR
                Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), _controller.radius);
            #endif
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }
}
