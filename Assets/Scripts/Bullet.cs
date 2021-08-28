
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    private MeshRenderer meshRenderer;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    public void Fire()
    {
        meshRenderer.enabled = true;
        //StartCoroutine(Shoot());
        //meshRenderer.enabled = false;
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(.1f);
        }
    }
}
