using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parralaxMultiplier;
    private Vector3 startPos;
    private Vector2 size;

    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        startPos = transform.position;
        size.x = GetComponent<MeshRenderer>().bounds.size.x;
        size.y = GetComponent<MeshRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 temp = new Vector2(cam.transform.position.x * (1 - parralaxMultiplier),
            cam.transform.position.y * (1 - parralaxMultiplier));
        Vector2 dist = new Vector2(cam.transform.position.x * parralaxMultiplier, cam.transform.position.y * parralaxMultiplier);
        transform.position = new Vector3(startPos.x + dist.x, startPos.y + dist.y, transform.position.z);

        if (temp.x > startPos.x + size.x)
            startPos.x += size.x;
        else if (temp.x < startPos.x - size.x)
            startPos.x -= size.x;
        else if (temp.y > startPos.y + size.y)
            startPos.y += size.y;
        else if (temp.y < startPos.y - size.y)
            startPos.y -= size.y;
    }
}
