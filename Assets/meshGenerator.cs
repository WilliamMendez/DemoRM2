using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class meshGenerator : MonoBehaviour
{
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;

    // Función generada en forma de cadena
    string function = "z-0.5*(x*x+y*y)";
    float l = -200;
    float u = 200;
    int resolution = 100;

    // URL de tu API FastAPI
    string apiUrl = "http://localhost:8000/generate_mesh";

    // Start is called before the first frame update
    void Start()
    {
        SaveMesh();

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
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

    IEnumerator GetMeshFromAPI()
    {
        Debug.Log("Solicitando a la API...");
        // Construir la URL de la solicitud con los parámetros generados
        string url =
            $"{apiUrl}?function={UnityWebRequest.EscapeURL(function)}&l={l}&u={u}&resolution={resolution}";

        // Realizar la solicitud GET a la API
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Debug.Log("Solicitud completada");

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Obtener la respuesta de la API
            string response = request.downloadHandler.text;
            // Debug.Log(response);

            Debug.Log("Cargando malla...");
            LoadMesh(response);
        }
        else
        {
            Debug.LogError("Error en la solicitud a la API: " + request.error);
        }
    }

    // Método para iniciar la solicitud a la API desde Unity
    public void GenerateMeshFromAPI()
    {
        StartCoroutine(GetMeshFromAPI());
    }

    // Update is called once per frame
    void Update() { }

    void SaveMesh()
    {
        string json = JsonUtility.ToJson(new MeshSaveData(GetComponent<MeshFilter>().mesh));
        string savePath = Application.persistentDataPath + "/mesh.json";

        Debug.Log("saving mesh data to " + savePath);

        File.WriteAllText(savePath, json);
    }

    void LoadMesh(string json)
    {
        MeshSaveData savedMesh = JsonUtility.FromJson<MeshSaveData>(json);

        this.vertices = savedMesh.vertices;
        this.triangles = savedMesh.triangles;
        updateMesh();
    }
}

// Clase para representar la respuesta de la API en Unity
// La respuesta viene en formato JSON de la forma:
// {"vertices":[{"x":0.0,"y":0.0,"z":0.0},...],"triangles":[[0,1,2],...]
// x,y,z son las coordenadas de los vértices en float
// i,j,k son los índices de los vértices que forman cada triángulo
[System.Serializable]
public class MeshLoadData
{
    public Vector3[] vertices;
    public int[] triangles;
}

// Create a serializable wrapper class for the mesh data
// (class Mesh is sealed and not serializable)
// all properties of this class must be public for the serializer
[System.Serializable]
public class MeshSaveData
{
    public Vector3[] vertices;
    public int[] triangles;

    // add whatever properties of the mesh you need...

    public MeshSaveData(Mesh mesh)
    {
        this.vertices = mesh.vertices;
        this.triangles = mesh.triangles;
    }
}
