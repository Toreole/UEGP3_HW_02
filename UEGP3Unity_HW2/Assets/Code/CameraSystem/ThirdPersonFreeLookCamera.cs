using System.Collections;
using UEGP3.PlayerSystem;
using UnityEngine;

namespace UEGP3.CameraSystem
{
	public class ThirdPersonFreeLookCamera : MonoBehaviour
	{
		// Constant to describe a full spin around any axis in degree
		private const float FullSpinDegree = 360f;
	
		[Header("Camera Rig Settings")]
		[Tooltip("The object that the camera pivots around.")]
		[SerializeField]
		private Transform _cameraPivot;
		[SerializeField] private PlayerController _playerController;

		[Header("Camera Speed Settings")]
		[Tooltip("The speed the camera uses to rotate around the x-axis")]
		[SerializeField] private float _pitchSpeed = 100f;
		[Tooltip("The speed the camera uses to rotate around the y-axis")]
		[SerializeField] private float _yawSpeed = 100f;
		[Tooltip("The speed which controls the camera zoom")] [SerializeField]
		private float _zoomSpeed = 250f;
		[Tooltip("Multiplier that increases zoom speed based on distance")] [SerializeField]
		private float _zoomDistanceMultiplier = 0.3f;
		[Tooltip("Time it takes to reset the camera from current position to initial position")] [SerializeField]
		private float _resetToInitialStateDuration = 0.25f;
	
		[Header("Camera Smoothing")]
		[SerializeField] private float _turnSmoothTime = 0.12f;
		[SerializeField] private float _positionSmoothTime = 0.7f;

		[Header("Camera Movement & Rotation Constraints")] [Tooltip("Smallest amount of yaw allowed. -360 allows a full-spin.")] [SerializeField]
		private float _minimumYaw = -FullSpinDegree;
		[Tooltip("Biggest amount of yaw allowed. 360 allows a full-spin.")]
		[SerializeField] private float _maximumYaw = FullSpinDegree;
		[Tooltip("Smallest amount of pitch allowed. -360 allows a full-spin.")] [SerializeField]
		private float _minimumPitch = -20f;
		[Tooltip("Biggest amount of pitch allowed. 360 allows a full-spin.")]
		[SerializeField]
		private float _maximumPitch = 80f;
		[Tooltip("Minimum distance from the pivot to the camera")]
		[SerializeField]
		private float _minimumDistance = 2.0f;
		[Tooltip("Maximum distance from the pivot to the camera")]
		[SerializeField] 
		private float _maximumDistance = 50.0f;

		[Header("Debug Settings")] 
		[Tooltip("Do we want to lock the cursor?")]
		[SerializeField]
		private bool _lockCursor;
	
		// Default offset as configured in the scene view
		private Vector3 _defaultPosition;
	
		// Current rotation in degrees on the x-axis
		private float _currentPitch;
		// Current rotation in degrees on the y-axis
		private float _currentYaw;
		// Current distance on the local z-axis in meters from the camera
		private float _currentDistance;
		
		// Initial values of yaw, pitch, distance
		private float _initialPitch;
		private float _initialDistance;
	
		// Is the camera currently resetting?
		private bool _isResetting;
		
		private void Awake()
		{
			// Subtract our position from the observed object position to receive a directional vector from our position to the objects position
			_defaultPosition = _cameraPivot.position - transform.position;

			// Set initial values
			_initialPitch = transform.rotation.eulerAngles.x;
			_initialDistance = _defaultPosition.magnitude;

			// Set current values to initial values
			_currentPitch = _initialPitch;
			_currentDistance = _initialDistance;

			// In builds of our game, we always want the cursor to be locked and invisible.
#if !UNITY_EDITOR
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
#endif
		}

		// Update is called every frame. Sometimes it might be called before the character movement, sometimes after the character movement.
		// This means, it is a bad idea to move the camera in update. However, we can poll inputs inside of update.
		private void Update()
		{
#if UNITY_EDITOR
			// Check if the cursor was enabled/disabled in the inspector
			EditorCheckLockCursor();
#endif
			// Do not take any input into account, if camera is currently resetting
			if (_isResetting)
			{
				return;
			}

			// User started resetting, do not process other inputs
			if (Input.GetButtonDown("ResetCamera"))
			{
				StartCoroutine(ResetToInitialState());
				return;
			}
			
			// Fetch inputs from the mouse, multiply by Time.deltaTime so its frame-rate independent
			float deltaYaw = Input.GetAxis("Mouse X") * Time.deltaTime * _yawSpeed;
			float deltaPitch = Input.GetAxis("Mouse Y") * Time.deltaTime * _pitchSpeed;
			// Scroll Wheel Input values in Unity are inverted, -1 * value gives the desired motion
			float deltaDistance = -1 * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * _zoomSpeed;
			deltaDistance *= _currentDistance * _zoomDistanceMultiplier;

			// Add yaw and pitch values to their member variables
			_currentYaw += deltaYaw;
			_currentPitch += deltaPitch;
			_currentDistance += deltaDistance;
		
			// Clamp yaw and pitch, so they do not overshoot their limits
			_currentYaw = ClampRotation(_currentYaw, _minimumYaw, _maximumYaw);
			_currentPitch = ClampRotation(_currentPitch, _minimumPitch, _maximumPitch);
			// Directly clamp zoom, we dont need custom logic for it
			_currentDistance = Mathf.Clamp(_currentDistance, _minimumDistance, _maximumDistance);
		}

		// Camera movement (positioning, rotation, ...) is handled LateUpdate so we can be sure the observed object has already moved this frame.
		// This helps avoiding jittery movement.
		private void LateUpdate()
		{
			// Input the currentPitch and currentYaw variables into Quaternion.Euler, so it creates the correct rotation with Quaternions
			Quaternion currentRotation = Quaternion.Euler(_currentPitch, _currentYaw, 0);
			currentRotation = Quaternion.Slerp(transform.rotation, currentRotation, _turnSmoothTime);
		
			// Multiply the rotation with our distance vector, to get the distance from the camera with correct rotation and add the cameraPivot position.
			transform.position = Vector3.Lerp(transform.position, _cameraPivot.position + currentRotation * new Vector3(0, 0, -_currentDistance), _positionSmoothTime);
		
			// Make the camera face the cameraPivot.
			transform.LookAt(_cameraPivot.position);
		}
		
		/// <summary>
		/// Clamps an angle between min and max rotation. Min/Max values of -360/360 allow full spins, without ever
		/// increasing the angle above 360 or decreasing it below -360
		/// </summary>
		/// <param name="angle">Current rotation in degrees</param>
		/// <param name="minRotation">Minimum allowed rotation in degrees. -360 for no restriction.</param>
		/// <param name="maxRotation">Maximum allowed rotation in degrees. 360 for no restriction.</param>
		/// <returns>The angle clamped between min and max rotation</returns>
		private float ClampRotation(float currentAngle, float minimumAngle, float maximumAngle)
		{
			// Add a full turn, in case we made a full left-turn. This means after -360° we start -1° rotation again
			if (currentAngle < -FullSpinDegree)
			{
				currentAngle += FullSpinDegree;
			}
		
			// Subtract a full turn, in case we made a full right-turn. This means after 360° we start 1° rotation again
			if (currentAngle > FullSpinDegree)
			{
				currentAngle -= FullSpinDegree;
			}
		
			// Clamp the angle between minimum and maximum allowed rotation
			return Mathf.Clamp(currentAngle, minimumAngle, maximumAngle);
		}

		/// <summary>
		/// Coroutine that reset the cmaera to its initial yaw, pitch and distance.
		/// </summary>
		/// <returns></returns>
		private IEnumerator ResetToInitialState()
		{
			// Resetting has started
			_isResetting = true;

			float t = 0;
			// Store yaw, pitch and distance at t0
			float currentYaw = _currentYaw;
			float currentPitch = _currentPitch;
			float currentDistance = _currentDistance;

			while (t < 1)
			{
				// Code
				t += Time.deltaTime / _resetToInitialStateDuration;
				
				// Lerp values from t0 to initial values
				_currentYaw = Mathf.Lerp(currentYaw, _playerController.CurrentYaw, t);
				_currentPitch = Mathf.Lerp(currentPitch, _initialPitch, t);
				_currentDistance = Mathf.Lerp(currentDistance, _initialDistance, t);
				
				yield return new WaitForEndOfFrame();
			}
			
			// Resetting done
			_isResetting = false;
		}

#if UNITY_EDITOR
		/// <summary>
		/// Editor-only method to check if the cursor needs to be locked or not
		/// </summary>
		private void EditorCheckLockCursor()
		{
			Cursor.visible = !_lockCursor;
			Cursor.lockState = _lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
		}
#endif
	}
}
