using UnityEngine;


public class DriftAndDissapear : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private float speed = 10f;
    void OnEnable()
    {
        direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Invoke("Disable", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
