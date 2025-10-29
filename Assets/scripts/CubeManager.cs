using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CubeManager : MonoBehaviour
{
    public static CubeManager Instance;
    public float resetDelay = 3f;
    private List<CubeInfo> cubes = new List<CubeInfo>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterCube(Transform cube)
    {
        cubes.Add(new CubeInfo(cube, cube.position, cube.rotation));
    }

    public void ResetAllCubes()
    {
        StartCoroutine(ResetAllCoroutine());
    }

    private IEnumerator ResetAllCoroutine()
    {
        yield return new WaitForSeconds(resetDelay);

        foreach (var info in cubes)
        {
            Rigidbody rb = info.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        foreach (var info in cubes)
        {
            info.transform.position = info.originalPosition;
            info.transform.rotation = info.originalRotation;

            // Reset scoring state
            CubeRegister reg = info.transform.GetComponent<CubeRegister>();
            if (reg != null)
                reg.ResetState();
        }

        yield return new WaitForSeconds(0.2f);
        foreach (var info in cubes)
        {
            Rigidbody rb = info.transform.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;
        }
    }

    private class CubeInfo
    {
        public Transform transform;
        public Vector3 originalPosition;
        public Quaternion originalRotation;

        public CubeInfo(Transform t, Vector3 pos, Quaternion rot)
        {
            transform = t;
            originalPosition = pos;
            originalRotation = rot;
        }
    }
}
