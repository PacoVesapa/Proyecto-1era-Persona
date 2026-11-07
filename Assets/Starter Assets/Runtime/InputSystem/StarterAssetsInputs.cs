using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		public static bool jugadorMuerto = false;
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Shooting Settings")]
		public GameObject proyectilPrefab;
		public Transform proyectilSpawnPoint;
		public float ShootForce = 20f;
		private void Start()
		{
			jugadorMuerto = false;
			SetCursorState(cursorLocked);
		}
#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
        public void OnShoot(InputValue value)
        {
			if (jugadorMuerto)
			{
				return;
			}
            Debug.Log("Shoot");
			Shoot();
        }
#endif

		private void Shoot()
		{
			if (jugadorMuerto)
			{
				return;
			}
			GameObject newProjectile = Instantiate(proyectilPrefab, proyectilSpawnPoint.position, Quaternion.identity);
			newProjectile.GetComponent<Rigidbody>().AddForce(proyectilSpawnPoint.up * ShootForce, ForceMode.Impulse);
			Destroy(newProjectile, 3f);
		}
        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			if (jugadorMuerto)
			{
				return;
			}
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !newState;
		}
	}
	
}