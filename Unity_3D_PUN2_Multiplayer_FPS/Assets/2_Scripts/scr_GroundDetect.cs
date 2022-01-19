using UnityEngine;

public class scr_GroundDetect : MonoBehaviour
{
    private scr_PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<scr_PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.gameObject)
        {
            return;
        }
        playerController.setGroundedState(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == this.gameObject)
        {
            return;
        }
        playerController.setGroundedState(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == this.gameObject)
        {
            return;
        }
        playerController.setGroundedState(true);
    }
}
