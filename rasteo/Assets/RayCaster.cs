using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public struct DatoEsfera
{
    public GameObject esfera;
    public Vector3 centro;
    public float radio;
    public Vector3 Ka,Ks,Kd;
    public float alpha;

}


public class RayCaster : MonoBehaviour
{
    public Vector3 Ia = new Vector3(.7f,.7f,.7f);
    public Vector3 Id = new Vector3(.8f,0.8f,1f);
    public Vector3 Is = new Vector3(1f,1f,1f);

    [SerializeField] private int _numeroDeEsferas = 20;
    

    private List<DatoEsfera> _esferas ;
    private List<GameObject> _listaEsferas;

    void GenerarEsferas()
    {
        for (int i = 0; i < _numeroDeEsferas; i++)
        {
            GameObject esfera = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            var radio = Random.Range(0.1f,.35f);
            var cx = Random.Range(-2f, 2f);
            var cy = Random.Range(2f, 6f);
            var cz = Random.Range(8f, 10f);

            Vector3 centros = new Vector3(cx, cy, cz);
            esfera.transform.position = centros;
            esfera.transform.localScale = new Vector3(radio*2, radio*2, radio*2);


            DatoEsfera datoEsfera = new DatoEsfera();
            datoEsfera.esfera = esfera;
            datoEsfera.centro = centros;
            datoEsfera.radio = radio;

            Vector3 kdtemp = new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
            Vector3 Katemp = new Vector3(kdtemp.x/5f, kdtemp.y/5f, kdtemp.z/5f);
            Vector3 Kstemp = new Vector3(kdtemp.x/3f, kdtemp.y/3f, kdtemp.z/3f);

            datoEsfera.Ka = Katemp ;
            datoEsfera.Ks = Kstemp ;
            datoEsfera.Kd = kdtemp ;
            datoEsfera.alpha = Random.Range(500f, 600f);


            //asignar material segun las propiedades de la esfera
            Material material = new Material(Shader.Find("Specular"));
            esfera.GetComponent<Renderer>().material = material;
            esfera.GetComponent<Renderer>().material.SetColor("_Color", new Color(datoEsfera.Kd.x, datoEsfera.Kd.y, datoEsfera.Kd.z));
            esfera.GetComponent<Renderer>().material.SetColor("_SpecColor", new Color(datoEsfera.Ks.x, datoEsfera.Ks.y, datoEsfera.Ks.z));
            esfera.GetComponent<Renderer>().material.SetFloat("_Shininess", datoEsfera.alpha);

            _esferas.Add(datoEsfera);
            _listaEsferas.Add(esfera);
        }

            
    }

    // Start is called before the first frame update
    void Start()
    {
        _esferas = new List<DatoEsfera>();
        _listaEsferas = new List<GameObject>();

        Camera.main.transform.position = new Vector3(0,4,5.5f);
        Vector3 forward =  Camera.main.transform.forward;
        float near = Camera.main.nearClipPlane;
        Vector3 up = Camera.main.transform.up;

        float angleFOV = Camera.main.fieldOfView * Mathf.Deg2Rad;

        float h = 2 * Mathf.Tan(angleFOV / 2) * near;
        
        // Ancho del plano cercano (W = H * aspect ratio)
        float widthAtNear = h *Camera.main.aspect;

        // El punto izquierdo de la cámara en el plano cercano
        Vector3 leftAtNear = Camera.main.transform.position + Camera.main.transform.forward * near - Camera.main.transform.right * (widthAtNear / 2f);

        Vector3 neartopLeft = Camera.main.transform.position + forward * near + up * (h / 2f) + leftAtNear* widthAtNear/2f;

        



        GenerarEsferas();

        //generacion de luz puntual
        GameObject luz = new GameObject();
        luz.AddComponent<Light>();
        luz.transform.position = new Vector3(0, 7.5f, 3f);
        luz.GetComponent<Light>().type = LightType.Point;
        luz.GetComponent<Light>().intensity = 3f;

        GameObject esfera = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        esfera.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        esfera.transform.position =  neartopLeft;



        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
