using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace AstronautPlayer
{

	public class AstronautPlayer : MonoBehaviour 
	{
		[SerializeField] float _radius = 5;
		[SerializeField] float _speed = 3;
		[SerializeField] float _force = 5;

		private Animator anim;
		private CharacterController controller;
		private Vector3 moveDirection = Vector3.zero;

		int trigger;	// key up을 할 시 문제발생을 차단

		void Start ()
		{
			controller = GetComponent <CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();
		}

		void Update()
		{
			if (anim.GetInteger("AnimationPar") != 2)
			{
				if (Input.GetKeyDown("d"))
				{
					moveDirection += Vector3.right;
					trigger = 1;
				}
				else if (Input.GetKeyDown("a"))
				{
					moveDirection += Vector3.left;
					trigger = 1;
				}
				else if (Input.GetKeyDown("w"))
				{
					moveDirection += Vector3.up;
					trigger = 1;
				}
				else if (Input.GetKeyDown("s"))
				{
					moveDirection += Vector3.down;
					trigger = 1;
				}


				if (trigger > 0 && Input.GetKeyUp("d"))
				{
					moveDirection -= Vector3.right;
				}
				else if (trigger > 0 && Input.GetKeyUp("a"))
				{
					moveDirection -= Vector3.left;
				}
				else if (trigger > 0 && Input.GetKeyUp("w"))
				{
					moveDirection -= Vector3.up;
				}
				else if (trigger > 0 && Input.GetKeyUp("s"))
				{
					moveDirection -= Vector3.down;
				}

				if (moveDirection != Vector3.zero)
				{
					if (SceneManager.GetActiveScene().name != "3DRunningTest") anim.SetInteger("AnimationPar", 1);
					controller.Move(moveDirection.normalized * _speed * Time.deltaTime);
				}
				else
				{
					anim.SetInteger("AnimationPar", 0);
				}
				if (SceneManager.GetActiveScene().name == "3DRunningTest")
				{
					if (Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2) >= Mathf.Pow(_radius, 2))
					{
						anim.SetInteger("AnimationPar", 2);
					}
				}
			}
			else
			{
				controller.Move(moveDirection.normalized * -_force * Time.deltaTime);

                if (anim.GetCurrentAnimatorStateInfo(0).IsTag("DisableMove"))
                {
                    moveDirection = Vector3.zero;
                    anim.SetInteger("AnimationPar", 0);
                    trigger = 0;
                }
            }
		}
	}
}
