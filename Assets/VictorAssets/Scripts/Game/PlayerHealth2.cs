using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth2 : MonoBehaviour
{

    public bool IsDead = false;

    private Animator animator;
    private UIManager UIManager;
    private PlayerController controller;

    private UIVideoGame UIGame;
    private bool isA=false;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<PlayerController>();
        UIManager = GetComponentInChildren<UIManager>();    
    }
    private void Update()
    {
        if (IsDead == true&& !isA)
        {
            IsDead = true;
            animator.SetTrigger("Death");
            Debug.Log("Player died");
            //controller.playerCamera.gameObject.SetActive(false);
            SinglePlayer.VideoGameManager.Instance.UpdateDead();
            UIManager.Fail();
            isA = true;
        }
        /*if (IsDead ==true) 
        {
            if (controller.playerCamera != null)
            {
                controller.playerCamera.gameObject.SetActive(false);

            }
        }*/

    }


    public void ResetPlayer()
    {
        /* IsDead = false;

         if (controller != null)
             controller.enabled = true;

         if (controller.playerCamera != null)
             controller.playerCamera.gameObject.SetActive(true);

         if ( deathUIPanel != null)
             deathUIPanel.SetActive(false);

         if (animator != null)
             animator.Rebind();*/
    }
}
