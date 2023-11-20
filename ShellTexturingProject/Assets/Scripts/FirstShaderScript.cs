using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FirstShaderScript : MonoBehaviour
{
    //The Mesh and shader to use for this obj
    public Mesh mesh;
    public Shader shader;

    [Header("Shader Variables")]
    public int numShells;

    public float shellLength;

    public float density;

    public float angle = 90;

    public Color shellColor1;
    public Color shellColor2;

    public Vector2 dimensions = new Vector2(0.5f, 1f);

    //private variables for fewer instantiations

    private Material shellMaterial;
    private GameObject[] shells;

    void OnEnable()
    {
        //Set the shell material and instantiate the list with the number of shells
        shellMaterial = new Material(shader);

        shells = new GameObject[numShells];

        //loop for the number of shells and create the layers
        for (int i = 0; i < numShells; i++)
        {
            //create the shell and set its name to the index
            shells[i] = new GameObject("Shell " + i.ToString());

            //Create the needed components for the shell
            shells[i].AddComponent<MeshFilter>();
            shells[i].AddComponent<MeshRenderer>();

            //Set the componetns for the shell
            shells[i].GetComponent<MeshFilter>().mesh = mesh;
            shells[i].GetComponent<MeshRenderer>().material = shellMaterial;

            //Set the parent to this empty object
            shells[i].transform.SetParent(this.transform, false);

            //Set the values for the shader
            shells[i].GetComponent<MeshRenderer>().material.SetInt("_ShellCount", numShells);
            shells[i].GetComponent<MeshRenderer>().material.SetInt("_ShellIndex", i);
            shells[i].GetComponent<MeshRenderer>().material.SetFloat("_ShellLength", shellLength);
            shells[i].GetComponent<MeshRenderer>().material.SetFloat("_Density", density);
            shells[i].GetComponent<MeshRenderer>().material.SetFloat("_Angle", angle);
            shells[i].GetComponent<MeshRenderer>().material.SetVector("_ShellColor1", shellColor1);
            shells[i].GetComponent<MeshRenderer>().material.SetVector("_ShellColor2", shellColor2);
            shells[i].GetComponent<MeshRenderer>().material.SetVector("_Dimensions", dimensions);
        }
    }
}
