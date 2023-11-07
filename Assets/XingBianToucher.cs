using UnityEngine;

public class XingBianToucher : MonoBehaviour
{
    public MeshFilter targetMeshFilter;
    private Mesh targetMesh;

   // public Camera mainCamera;

    private Vector3[] originalVertices, displacedVertices, vertexVelocities;

    private int verticesCount;

    public float forceOffset = 0.1f;
    public float springForce = 20f;
    public float damping = 5f;

    void Start()
    {
        targetMesh = targetMeshFilter.mesh;

        verticesCount = targetMesh.vertices.Length;

        originalVertices = targetMesh.vertices;
        displacedVertices = targetMesh.vertices;
        vertexVelocities = new Vector3[verticesCount];
    }

   
    public void AddForce(Vector3 forceStart,Vector3 forceDir,float forcePower)
    {
        Vector3 actingForcePoint = targetMeshFilter.transform.InverseTransformPoint(forceStart + forceDir * forceOffset);//������ָ����ı�����������

        for (int i = 0; i < verticesCount; i++)
        {
            Vector3 pointToVertex = displacedVertices[i] - actingForcePoint;//��������ָ��ǰ����λ�õ�����

            float actingForce = forcePower / (1f + pointToVertex.sqrMagnitude);//��������С
            vertexVelocities[i] += pointToVertex.normalized * actingForce * Time.deltaTime;//�����ٶ�����
        }
    }
    void Update()
    {
        //�α�Ļָ�
        {
            for (int i = 0; i < verticesCount; i++)
            {
                vertexVelocities[i] += (originalVertices[i] - displacedVertices[i]) * springForce * Time.deltaTime;//����+���㵱ǰλ��ָ�򶥵��ʼλ�õ��ٶ�����==�ص���
                vertexVelocities[i] *= 1f - damping * Time.deltaTime;//��������
                displacedVertices[i] += vertexVelocities[i] * Time.deltaTime;//����������һ��λ��
            }
        }
        targetMesh.vertices = displacedVertices;
        targetMesh.RecalculateNormals();
    }
}