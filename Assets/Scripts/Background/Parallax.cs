using System;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    enum ParallaxType {Game, Menu, UI}
    
    [SerializeField] private Transform followTrans;
    [SerializeField] private Camera cam;
    [SerializeField] private ParallaxType state;
    [SerializeField] private float parralaxMultiplier;
    
    private Vector3 startPos;
    
    private Vector2 size;
    
    private Vector3 mousePos = Vector3.zero;


    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        if (state == ParallaxType.Game)
        {
            size.x = GetComponent<MeshRenderer>().bounds.size.x;
            size.y = GetComponent<MeshRenderer>().bounds.size.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case ParallaxType.Game:
                GameParralax();
                return;
            case ParallaxType.Menu:
                MenuParralax();
                return;
            case ParallaxType.UI:
                UIParralax();
                return;
        }
    }

    void GameParralax()
    {
        Vector2 temp = new Vector2(followTrans.transform.position.x * (1 - parralaxMultiplier),
            followTrans.transform.position.y * (1 - parralaxMultiplier));
        Vector2 dist = new Vector2(followTrans.transform.position.x * parralaxMultiplier, followTrans.transform.position.y * parralaxMultiplier);
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
    void MenuParralax()
    {
        transform.position = new Vector3(startPos.x + followTrans.position.x * parralaxMultiplier, startPos.y + followTrans.position.y * parralaxMultiplier, transform.position.z);
    }

    void UIParralax()
    {
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        float rayIterationCount = cam.transform.position.z / -cameraRay.direction.z;

        Vector3 planeSpaceMouse = new Vector3(cameraRay.origin.x + cameraRay.direction.x * rayIterationCount,
            cameraRay.origin.y + cameraRay.direction.y * rayIterationCount, 0);
        mousePos = planeSpaceMouse;


        transform.position = new Vector3(startPos.x + mousePos.x * parralaxMultiplier, startPos.y + mousePos.y * parralaxMultiplier, transform.position.z);
    }
}
