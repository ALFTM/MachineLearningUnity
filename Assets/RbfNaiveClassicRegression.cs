using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RbfNaiveClassicRegression : MonoBehaviour {
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    GameObject[] trainingSpheres;

    private System.IntPtr _model;

    // Use this for initialization
    void Start()
    {

        var inputs = GetAllPosition(trainingSpheres);
        var outputs = GetAllPositionY(trainingSpheres);
        var gamma = 0.1;

        _model = rbfNaiveClassicCreate(trainingSpheres.Length);

        rbfNaiveClassicTraining(_model, inputs, outputs, inputs.Length, 2, gamma);
        
        var testSpheres = new List<GameObject>();

        for (var i = 0; i < 100; i++)
        {
            for (var j = 0; j < 100; j++)
            {
                testSpheres.Add(Instantiate(prefab, new Vector3(i*0.1f - 5f, 0, j*0.1f - 5f), Quaternion.identity));
            }
        }
        
        for (var i = 0; i < testSpheres.Count; i++)
        {
            double[] testInput = new double[2]
            {
                testSpheres[i].transform.position.x,
                testSpheres[i].transform.position.z
            };

            float Y = (float)rbfNaiveClassicPredict(_model, inputs, testInput, inputs.Length, 2, gamma);

            testSpheres[i].transform.position += new Vector3(0, Y, 0);
        }

        freePtr(_model);
    }

    // Update is called once per frame
    void Update()
    {

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
    private static extern double rbfNaiveClassicPredict(System.IntPtr weights, double[] inputs, double[] testInput, int nbSamples, int width, double gamma);
    [DllImport("GlaDOS")]
    private static extern void freePtr(System.IntPtr model);
}
