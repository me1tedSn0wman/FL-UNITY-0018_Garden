using UnityEngine;

public class LoadingFirstScreenUI : WindowUI
{
    [SerializeField] private float rotateSpeed = -3f;
    [SerializeField] private GameObject rotatingThings;

    void Update()
    {
        rotatingThings.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime, Space.Self);
    }
}
