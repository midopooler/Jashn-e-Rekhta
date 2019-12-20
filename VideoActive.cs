using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoActive : MonoBehaviour
{
    MeshRenderer MR;
    VideoPlayer AS;


    // Start is called before the first frame update
    void Start()
    {
        MR = transform.GetComponent<MeshRenderer>();
        AS = transform.GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(MR.enabled == true)
        {
            AS.enabled = true;
        }
        else
        {
            AS.enabled = false;
        }
    }
}
