using System.Collections;
using UnityEngine;

public class CubeGen : MonoBehaviour
{
    [SerializeField]
    private GameObject cubePrefab = null;

    private GameObject root_ = null;

    private int count = 0;
    private bool inited_ = false;
    private bool cooltime_ = false;

    
    private void Initialize()
    {
        ResetRoot();

        inited_ = true;
    }


    private void ResetRoot()
    {
        if (root_ != null)
        {
            Destroy(root_);
            root_ = null;
        }

        root_ = new GameObject("CubeRoot");
        root_.transform.SetParent(transform, false);


        count = 0;
    }


    private IEnumerator WaitTimer(float time)
    {
        cooltime_ = true;

        if (count > 250)
        {
            ResetRoot();
        }

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        GameObject cube = Instantiate(cubePrefab, root_.transform);
        count++;

        cooltime_ = false;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (!inited_)
        {
            Initialize();
            return;
        }

        if (cooltime_)
            return;

        StartCoroutine(WaitTimer(0.2f));
    }
}
