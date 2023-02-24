using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    List<GameObject> cmVcams = new List<GameObject>();
    int cameraId = 0;
    public static CameraController instance;
    [SerializeField] ParticleSystem confetti;
    [SerializeField] ParticleSystem smallConfetti;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            cmVcams.Add(transform.GetChild(i).gameObject);
            cmVcams[i].SetActive(false);
        }
        cmVcams[cameraId].SetActive(true);
    }
    public void ChangeCamera()
    {
        cameraId++;
        cmVcams[cameraId].SetActive(true);
        StartCoroutine(SetDisableCameras());
    }
    public void ChangeCamera(int _cameraId)
    {
        cameraId = _cameraId;
        cmVcams[cameraId].SetActive(true);
        StartCoroutine(SetDisableCameras());
    }
    IEnumerator SetDisableCameras()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < cmVcams.Count; i++)
        {
            if (i != cameraId)
            {
                cmVcams[i].SetActive(false);
            }
        }
    }
    public void PlayConfetti(bool isBig = true)
    {
        if (isBig)
        {
            confetti.Play();
        }
        else
        {
            smallConfetti.Play();
        }
    }
}
