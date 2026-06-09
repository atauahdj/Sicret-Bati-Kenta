using UnityEngine;

public class DoorScriptEnemy : MonoBehaviour
{
    public Transform EnemyPosition;
    public Camera playerCamera;
    public float openAngle = 90f;
    public float animationSpeed = 2f;
    private bool isOpened = false;
    public bool isAnimating = false;
    private GameObject currentDoor;
    private GameObject animatingDoor;
    private float t = 0f;
    private Vector3 startAngles;
    private Vector3 targetAngles;
    private MoveDestination MD;
    void Start()
    {
        MD = FindAnyObjectByType<MoveDestination>();
    }
    void Update()
    {
        Ray ray = new Ray(EnemyPosition.position, EnemyPosition.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f))
        {
            currentDoor = hit.transform.gameObject;
        }
        else
        {
            currentDoor = transform.gameObject;
        }

        if ((currentDoor != null && currentDoor.CompareTag("Door") && !isAnimating) && (Vector3.Distance(currentDoor.transform.position, EnemyPosition.position) <= 2f))
        {
            StartDoorAnimation();
        }

        if (isAnimating && animatingDoor != null)
        {
            t += Time.deltaTime * animationSpeed;
            t = Mathf.Clamp01(t);

            Vector3 newAngles = Vector3.Lerp(startAngles, targetAngles, t);
            animatingDoor.transform.eulerAngles = newAngles;

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
        isAnimating = true;
        t = 0f;
        animatingDoor = currentDoor;
        startAngles = animatingDoor.transform.eulerAngles;

        if (!isOpened)
        {
            targetAngles = startAngles + new Vector3(0, openAngle, 0);
        }
        else
        {
            targetAngles = startAngles - new Vector3(0, openAngle, 0);
        }
    }
}