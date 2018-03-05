using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MoveSphereScript : MonoBehaviour {

    [SerializeField]
    private Transform[] sphereTransform;

	// Use this for initialization
	void Start () {
        Debug.Log("GlaDOS Start");
        Debug.Log(glaDOS());
        sphereTransform[0].position += Vector3.down * 2f;
        sphereTransform[1].position += Vector3.up * 2f;
        sphereTransform[2].position += Vector3.forward * 2f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [DllImport("GlaDOS")]
    private static extern int glaDOS();
}
