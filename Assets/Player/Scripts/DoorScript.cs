using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Camera playerCamera;
    public float openAngle = 90f;
    public float animationSpeed = 2f;
    private bool isOpened = false;
    public bool isAnimating = false;
    private GameObject currentDoor;
    private GameObject animatingDoor;
    private float t = 0f;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private MoveDestination MD;
    public GameObject key;
    public bool Lock;
    private bool isLocked;

    void Start()
    {
        isLocked = Lock;
        MD = FindAnyObjectByType<MoveDestination>();
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f))
        {
            currentDoor = hit.transform.gameObject;
        }
        else
        {
            currentDoor = null;
        }

        if (key != null && currentDoor != null && Vector3.Distance(key.transform.position, playerCamera.transform.position) <= 2f)
        {
            DoorScript doorScript = currentDoor.GetComponent<DoorScript>();
            if (doorScript != null)
                doorScript.isLocked = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && currentDoor != null && currentDoor.CompareTag("Door") && !isAnimating)
        {
            DoorScript doorScript = currentDoor.GetComponent<DoorScript>();
            if (doorScript != null && !doorScript.isLocked)
            {
                doorScript.StartDoorAnimation();
            }
        }

        if (isAnimating && animatingDoor != null)
        {
            t += Time.deltaTime * animationSpeed;
            t = Mathf.Clamp01(t);

            animatingDoor.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            if (t >= 1f)
            {
                isAnimating = false;
                isOpened = !isOpened;
                animatingDoor = null;
            }
        }
    }

    public void StartDoorAnimation()
    {
        if (isAnimating) return;

        isAnimating = true;
        t = 0f;
        animatingDoor = gameObject; // ← ВАЖНО: используем саму дверь, а не currentDoor
        startRotation = animatingDoor.transform.rotation;

        if (!isOpened)
        {
            targetRotation = startRotation * Quaternion.Euler(0, openAngle, 0);
        }
        else
        {
            targetRotation = startRotation * Quaternion.Euler(0, -openAngle, 0);
        }
    }
}