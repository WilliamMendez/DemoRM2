using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IronPython.Runtime;
using IronPython.Hosting;
using UnityEditor.Scripting.Python;

public class meshGenerator : MonoBehaviour
{
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;
    dynamic meshGeneratorPy;
    dynamic py;



    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        // // set a test mesh
        // vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 0, 1) };
        // triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 4, 5, 0, 5, 1, 1, 5, 6, 1, 6, 2, 2, 6, 7, 2, 7, 3, 3, 7, 4, 3, 4, 0, 4, 7, 6, 4, 6, 5 };
        // updateMesh();

        var engine = Python.CreateEngine();
        var scope = engine.CreateScope();
        var paths = engine.GetSearchPaths();

        Debug.Log(Application.dataPath + "/Python/Lib");

        paths.Add(Application.dataPath + "/Python/Lib");
        paths.Add(Application.dataPath + "/Python/Lib/site-packages");
		engine.SetSearchPaths (paths);

        // test if python is working
        // Debug.Log(test);
        dynamic testClass = engine.ExecuteFile(Application.dataPath + "/test.py", scope);
        var test2 = testClass.ClassTest();
        // Debug.Log(test2);
        var test3 = test2.test2();
        // Debug.Log(test3);
        // Debug.Log(engine.Execute("a", scope));

//      get the test mesh: tuple of vertices and triangles
        var testMesh = test2.testMesh();
        // Debug.Log(testMesh);
        // Debug.Log(testMesh.GetType());

        // IronPython.Runtime.List
        var vertex = testMesh[0];
        // IronPython.Runtime.List
        var triangle = testMesh[1];

        // set the vertices and triangles as a mesh
        setMesh(vertex, triangle);

        engine.Execute("import sys", scope);
        engine.Execute("sys.version", scope);
        engine.Execute("import numpy as np", scope);
        // engine.Execute("from skimage import measure", scope);

        py = engine.ExecuteFile(Application.dataPath + "/MeshFromFunction.py");
        // PythonRunner.RunFile(Application.dataPath + "/MeshFromFunction.py")
        // createShape();

    }

    void createShape()
    {
        var function = "x**2 + y**2 - z";
        // create the python class instance and call the method
        meshGeneratorPy = py.meshGenerator(function, -50, 50, 100);
        meshGeneratorPy.generateMesh();

        // get the vertices and triangles from the python class
        vertices = meshGeneratorPy.vertices;
        triangles = meshGeneratorPy.triangles;
        updateMesh();


        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void updateMesh()
    {
        mesh.Clear();
        // Debug.Log(vertices);
        mesh.vertices = vertices;
        // Debug.Log(triangles);
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void setMesh(IronPython.Runtime.List vertices, IronPython.Runtime.List triangles)
    {
        this.vertices = new Vector3[vertices.Count];
        this.triangles = new int[triangles.Count*3];

        for (int i = 0; i < vertices.Count; i++)
        {
            IronPython.Runtime.PythonTuple vertex = (IronPython.Runtime.PythonTuple)((IronPython.Runtime.List)vertices)[i];
            // Debug.Log(vertex);
            // Debug.Log(vertex[0]);
            // Debug.Log(vertex[0].GetType());
            // convert from int to float
            var x = (float)(int)vertex[0];
            var y = (float)(int)vertex[1];
            var z = (float)(int)vertex[2];

            this.vertices[i] = new Vector3(x, y, z);
        }

        for (int i = 0; i < triangles.Count; i=i+3)
        {
            var triangle = (IronPython.Runtime.PythonTuple)((IronPython.Runtime.List)triangles)[i/3];

            var a = (int)triangle[0];
            var b = (int)triangle[1];
            var c = (int)triangle[2];

            this.triangles[i] = a;
            this.triangles[i+1] = b;
            this.triangles[i+2] = c;

        }

        updateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
