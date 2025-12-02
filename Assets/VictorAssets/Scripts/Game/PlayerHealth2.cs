using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth2 : MonoBehaviour
{

    public bool IsDead = false;

    private Animator animator;
    private PlayerController controller;

    [SerializeField] private GameObject deathUIPanel;
    private UIVideoGame UIGame;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<PlayerController>();
        UIGame = FindObjectOfType<UIVideoGame>();
    }

    private void Start()
    {
        deathUIPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Dopple")
        {
            IsDead = true;
            animator.SetTrigger("Death");
            Debug.Log("Player died");
            UIGame.ActiveLose();
        }
    }
    private void Update()
    {
        if (IsDead ==true) 
        {
            if (controller.playerCamera != null)
            {
                controller.playerCamera.gameObject.SetActive(false);

            }
        }

    }


    public void ResetPlayer()
    {
        IsDead = false;

        if (controller != null)
            controller.enabled = true;

        if (controller.playerCamera != null)
            controller.playerCamera.gameObject.SetActive(true);

        if ( deathUIPanel != null)
            deathUIPanel.SetActive(false);

        if (animator != null)
            animator.Rebind();
    }
}
