using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool crawl;
		public bool punch;
		public bool push;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

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

		public void OnPush(InputValue value)
		{
			PushInput(value.isPressed);
		}

		public void OnPunch(InputValue value)
		{
			PunchInput(value.isPressed);
		}

		public void OnCrawl(InputValue value)
		{
			CrawlInput(value.isPressed);
		}
#endif


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
			Debug.Log("Input:Jump!!!");
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			Debug.Log("Input:Sprint!!!");
			sprint = newSprintState;
		}

		public void CrawlInput(bool newCrawlState)
		{
			Debug.Log("Input:Crawl!!!");
			crawl = newCrawlState;
		}

		public void PunchInput(bool newPunchState)
		{
			Debug.Log("Input:Punch!!!");
			punch = newPunchState;
		}

		public void PushInput(bool newPushState)
		{
			Debug.Log("Input:Push!!!");
			push = newPushState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}