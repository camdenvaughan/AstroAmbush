using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    [SerializeField] private float parralaxMultiplier;
    [SerializeField] private Transform ship;
    private Vector3 startPos;
    
    // Start is called before the first frame update
    void Start()
    {

        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(startPos.x + ship.position.x * parralaxMultiplier, startPos.y + ship.position.y * parralaxMultiplier, transform.position.z);
        
    }
}
