using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RbfNaiveClassic : MonoBehaviour {

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    GameObject[] trainingSpheres;

    private System.IntPtr _model;

    // Use this for initialization
    void Start () {

        var inputs = GetAllPosition(trainingSpheres);
        var outputs = GetAllPositionY(trainingSpheres);

        Debug.Log("Start Create");
        Debug.Log("TRAINING SPHERE LENGHT : ");
        Debug.Log(trainingSpheres.Length);
        _model = rbfNaiveClassicCreate(trainingSpheres.Length);

        double[] array = new double[trainingSpheres.Length];
        Marshal.Copy(_model, array, 0, trainingSpheres.Length);

        
        foreach (var item in array)
        {
            Debug.Log(item);
        }
        
        //unsafe {

        //    double* poids = (double*)_model.ToPointer();


        //    for (int t = 0; t < trainingSpheres.Length; t++) {
        //        double value = poids[t];

        //        Debug.Log(value);
        //    }
        //}



        Debug.Log("Start Training");
        rbfNaiveClassicTraining(_model, inputs, outputs, inputs.Length, 2, 0.1);

        Debug.Log("Training FInish");

        array = new double[trainingSpheres.Length];
        Marshal.Copy(_model, array, 0, trainingSpheres.Length);


        foreach (var item in array) {
            Debug.Log(item);
        }

        //unsafe {

        //    double* poids = (double*)_model.ToPointer();


        //    for (int t = 0; t < trainingSpheres.Length; t++) {
        //        double value = poids[t];

        //        Debug.Log(value);
        //    }
        //}

        Debug.Log("After unsafe");

         array = new double[trainingSpheres.Length];
        Marshal.Copy(_model, array, 0, trainingSpheres.Length);

        /*
        foreach (var item in array)
        {
            Debug.Log(item);
        }
        */

        var testSpheres = new List<GameObject>();

        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                testSpheres.Add(Instantiate(prefab, new Vector3(i - 5f, 0, j - 5f), Quaternion.identity));
            }
        }

        Debug.Log("Start Classify");
        for (var i = 0; i < testSpheres.Count; i++)
        {
            double[] testInput = new double[2]
            {
                testSpheres[i].transform.position.x,
                testSpheres[i].transform.position.z
            };

            float Y = (float) rbfNaiveClassicClassify(_model, inputs, testInput, inputs.Length, 2, 0.1);

            testSpheres[i].transform.position += new Vector3(0, Y, 0);

            if (Y >= 0)
            {
                AddColorToSphere(testSpheres[i], Color.red);
            }
            else
            {
                AddColorToSphere(testSpheres[i], Color.blue);
            }
        }

        freePtr(_model);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private double[] GetAllPosition(GameObject[] gameObject)
    {
        var positions = new List<double>();

        for (int i = 0; i < gameObject.Length; i++)
        {
            positions.Add(gameObject[i].transform.position.x);
            positions.Add(gameObject[i].transform.position.z);
        }

        return positions.ToArray();
    }

    private double[] GetAllPositionY(GameObject[] gameObject)
    {
        var positions = new List<double>();

        for (int i = 0; i < gameObject.Length; i++)
        {
            positions.Add(gameObject[i].transform.position.y);
        }

        return positions.ToArray();
    }

    private void AddColorToSphere(GameObject gameObject, Color color)
    {
        gameObject.transform.GetComponent<Renderer>().material.color = color;
    }

    [DllImport("GlaDOS")]
    private static extern System.IntPtr rbfNaiveClassicCreate(int nbSamples);
    [DllImport("GlaDOS")]
    private static extern void rbfNaiveClassicTraining(System.IntPtr weights, double[] inputs, double[] outputs, int nbSamples, int width, double gamma);
    [DllImport("GlaDOS")]
    private static extern double rbfNaiveClassicClassify(System.IntPtr weights, double[] inputs, double[] testInput, int nbSamples, int width, double gamma);
    [DllImport("GlaDOS")]
    private static extern double rbfNaiveClassic(System.IntPtr weights, double[] inputs, double[] testInput, int nbSamples, int width, double gamma);
    [DllImport("GlaDOS")]
    private static extern void freePtr(System.IntPtr model);
}
