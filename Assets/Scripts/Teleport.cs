using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public Text teleportText;
    public Camera playerCamera;

    string s1 = "Press E to Teleport";
    string s2 = "Press E to Restart";
    string s3;
    private void Start()
    {
        playerCamera = Camera.main;
        teleportText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowTeleportText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowTeleportText(false);
        }
    }

    private void LateUpdate()
    {
        
        if (teleportText.gameObject.activeSelf && SceneManager.GetActiveScene().name == "WorldMap")
        {
            Vector3 targetPosition = playerCamera.transform.position;
            targetPosition.y = teleportText.transform.position.y + 10;
            teleportText.transform.LookAt(targetPosition, Vector3.up);
        }
        else if(teleportText.gameObject.activeSelf && SceneManager.GetActiveScene().name == "BossStage")
        {

            Vector3 targetPosition = playerCamera.transform.position;
            
            targetPosition.y = teleportText.transform.position.y + 10;
            Vector3 screenPos = playerCamera.WorldToScreenPoint(targetPosition);

            teleportText.GetComponent<RectTransform>().position = new Vector3(960, 800, 0);
        }
    }

    private void ShowTeleportText(bool show)
    {
        if (SceneManager.GetActiveScene().name == "WorldMap")
        {
            s3 = s1;
        }
        else s3 = s2;
        teleportText.gameObject.SetActive(show);
        teleportText.text = show ? s3 : "";
    }
}
