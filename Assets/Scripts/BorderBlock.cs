using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BorderBlock : MonoBehaviour
{
    public static GameObject gridPref;
    public static GameObject ground;
    private static BorderBlock singleton = null;

    public List<GameObject> blocks = new List<GameObject>();

    public static BorderBlock GetSingleton()
    {
        if (singleton == null)
        {
            singleton = new GameObject("BorderBlock").AddComponent<BorderBlock>();
        }
        return singleton;

    }

    public void GridStart(float y)
    {
        StartCoroutine(InstantiateGrid(y));
    }

    public void GridReset()
    {
        StartCoroutine(Reset());
    }


    IEnumerator InstantiateGrid(float y)
    {
        blocks.Clear();
        Instantiate(ground, new Vector3(0, y, 0), Quaternion.identity);

        for (int i = 0; i < 16; i++)
        {
            if(i==0 || i == 15)
            {
                for (int k = 0; k < 9; k++)
                {
                    GameObject newBlock = Instantiate(gridPref, new Vector3(.5f * k, y + 5, .5f * i), Quaternion.identity, singleton.transform);
                    newBlock.transform.DOMoveY(y , .5f, true);
                    newBlock.transform.DOScale(.5f, 1f);
                    blocks.Add(newBlock);
                    yield return new WaitForSeconds(.005f);
                }

            }
            else
            {
                GameObject newBlockL = Instantiate(gridPref, new Vector3(0, y + 5, .5f * i), Quaternion.identity, singleton.transform);
                GameObject newBlockR = Instantiate(gridPref, new Vector3(.5f * 8, y + 5, .5f * i), Quaternion.identity, singleton.transform);
                newBlockL.transform.DOMoveY(y, .5f, true);
                newBlockR.transform.DOMoveY(y, .5f, true);
                newBlockL.transform.DOScale(.5f, 1f);
                newBlockR.transform.DOScale(.5f, 1f);
                blocks.Add(newBlockL);
                blocks.Add(newBlockR);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }


    IEnumerator Reset()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].transform.DOMoveY(blocks[i].transform.position.y + .5f, .05f, true);
            yield return new WaitForSeconds(.05f);
            Destroy(blocks[i], .5f);
        }
    }
}
