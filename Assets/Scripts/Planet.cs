using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Planet : MonoBehaviour
{
    public Material m_GroundMaterial;
    public Material m_OceanMaterial;

    public Camera CameraPosition;

    GameObject m_GroundMesh;
    GameObject m_OceanMesh;

    public List<Polygon> m_Polygons;
    public List<Vector3> m_Vertices;

    public List<Plant> Plants;

    public int temperatureOffset = 50;

    public bool mode = false;
    int viewMode;

    PolySet landPolys;

    Color32 colorOcean = new Color32(0, 80, 220, 255 / 2);
    Color32 colorGrass = new Color32(0, 220, 0, 0);
    Color32 colorDirt = new Color32(125, 71, 21, 0);
    Color32 colorDirtDark = new Color32(105, 51, 1, 0);
    Color32 colorSand = new Color32(235, 201, 52, 0);
    Color32 colorGravel = new Color32(166, 150, 146, 0);
    Color32 colorStone = new Color32(100, 116, 117, 0);
    Color32 colorDeepOcean = new Color32(0, 40, 110, 0);
    Color32 colorToHot = new Color32(255, 20, 0, 0);
    Color32 colorToCold = new Color32(0, 0, 255, 0);
    Color32 colorToDry = new Color32(255, 210, 148, 0);
    Color32 colorToHumid = new Color32(15, 135, 255, 0);
    Color32 colorSulfur = new Color32(200, 200, 0, 0);
    Color32 colorNeutral = new Color32(255, 255, 255, 0);
    Color32 colorGreenerGrass = new Color32(21, 140, 0, 0);

    public void Start()
    {
        InitAsIcosohedron();
        Subdivide(3);
        CalculateNeighbours();

        foreach (Polygon p in m_Polygons)
            p.m_Color = colorOcean;

        landPolys = new PolySet();
        PolySet sides;
            System.Random rnd = new Random();

        for (int i = 0; i < 5; i++)
        {
            float continentSize = UnityEngine.Random.Range(0.3f, 0.9f);
            PolySet newLand = GetPolysInSphere(UnityEngine.Random.onUnitSphere, continentSize, m_Polygons);
            byte random = (byte)rnd.Next(0, 4);
            foreach (Polygon poly in newLand)
            {
                poly.groundType = random;
            }
            landPolys.UnionWith(newLand);
        }

        for (int i = 0; i < 3; i++)
        {
            float islandSize = UnityEngine.Random.Range(0.1f, 0.3f);
            PolySet newIslands = GetPolysInSphere(UnityEngine.Random.onUnitSphere, islandSize, m_Polygons);
            landPolys.UnionWith(newIslands);
        }

        var oceanPolys = new PolySet();
        foreach (Polygon poly in m_Polygons)
        {
            if (!landPolys.Contains(poly))
                oceanPolys.Add(poly);
        }

        var oceanSurface = new PolySet(oceanPolys);
        sides = Inset(oceanSurface, 0.05f);
        sides.ApplyColor(colorOcean);
        sides.ApplyAmbientOcclusionTerm(1.0f, 0.0f);
        if (m_OceanMesh != null)
            Destroy(m_OceanMesh);

        m_OceanMesh = GenerateMesh("Ocean Surface", m_OceanMaterial);

        generateTemperature();
        generateHumidity();
        generateSulfurLevels();

        foreach (Polygon landPoly in landPolys)
        {
            switch (landPoly.groundType)
            {
                case 0:
                    landPoly.m_Color = colorSand;
                    break;
                case 1:
                    landPoly.m_Color = colorGravel;
                    break;
                case 2:
                    landPoly.m_Color = colorDirt;
                    break;
                case 3:
                    landPoly.m_Color = colorStone;
                    break;
                default:
                    landPoly.m_Color = colorDirt;
                    break;
            }
        }

        sides = Extrude(landPolys, 0.05f);
        sides.ApplyColor(colorDirtDark);
        sides.ApplyAmbientOcclusionTerm(1.0f, 0.0f);

        PolySet hillPolys = landPolys.RemoveEdges();
        sides = Inset(hillPolys, 0.03f);
        sides.ApplyColor(colorDirtDark);
        sides.ApplyAmbientOcclusionTerm(0.0f, 1.0f);
        sides = Extrude(hillPolys, 0.05f);
        sides.ApplyColor(colorDirtDark);

        sides.ApplyAmbientOcclusionTerm(1.0f, 0.0f);

        sides = Extrude(oceanPolys, -0.02f);
        sides.ApplyColor(colorOcean);
        sides.ApplyAmbientOcclusionTerm(0.0f, 1.0f);
        sides = Inset(oceanPolys, 0.02f);
        sides.ApplyColor(colorOcean);
        sides.ApplyAmbientOcclusionTerm(1.0f, 0.0f);

        var deepOceanPolys = oceanPolys.RemoveEdges();
        sides = Extrude(deepOceanPolys, -0.05f);
        sides.ApplyColor(colorDeepOcean);
        deepOceanPolys.ApplyColor(colorDeepOcean);

        if (m_GroundMesh != null)
            Destroy(m_GroundMesh);
        m_GroundMesh = GenerateMesh("Ground Mesh", m_GroundMaterial);
        m_GroundMesh.AddComponent<MeshCollider>();
    }

    public void InitAsIcosohedron()
    {
        m_Polygons = new List<Polygon>();
        m_Vertices = new List<Vector3>();
        Plants = new List<Plant>();

        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2;

        m_Vertices.Add(new Vector3(-1, t, 0).normalized);
        m_Vertices.Add(new Vector3(1, t, 0).normalized);
        m_Vertices.Add(new Vector3(-1, -t, 0).normalized);
        m_Vertices.Add(new Vector3(1, -t, 0).normalized);
        m_Vertices.Add(new Vector3(0, -1, t).normalized);
        m_Vertices.Add(new Vector3(0, 1, t).normalized);
        m_Vertices.Add(new Vector3(0, -1, -t).normalized);
        m_Vertices.Add(new Vector3(0, 1, -t).normalized);
        m_Vertices.Add(new Vector3(t, 0, -1).normalized);
        m_Vertices.Add(new Vector3(t, 0, 1).normalized);
        m_Vertices.Add(new Vector3(-t, 0, -1).normalized);
        m_Vertices.Add(new Vector3(-t, 0, 1).normalized);

        m_Polygons.Add(new Polygon(0, 11, 5));
        m_Polygons.Add(new Polygon(0, 5, 1));
        m_Polygons.Add(new Polygon(0, 1, 7));
        m_Polygons.Add(new Polygon(0, 7, 10));
        m_Polygons.Add(new Polygon(0, 10, 11));
        m_Polygons.Add(new Polygon(1, 5, 9));
        m_Polygons.Add(new Polygon(5, 11, 4));
        m_Polygons.Add(new Polygon(11, 10, 2));
        m_Polygons.Add(new Polygon(10, 7, 6));
        m_Polygons.Add(new Polygon(7, 1, 8));
        m_Polygons.Add(new Polygon(3, 9, 4));
        m_Polygons.Add(new Polygon(3, 4, 2));
        m_Polygons.Add(new Polygon(3, 2, 6));
        m_Polygons.Add(new Polygon(3, 6, 8));
        m_Polygons.Add(new Polygon(3, 8, 9));
        m_Polygons.Add(new Polygon(4, 9, 5));
        m_Polygons.Add(new Polygon(2, 4, 11));
        m_Polygons.Add(new Polygon(6, 2, 10));
        m_Polygons.Add(new Polygon(8, 6, 7));
        m_Polygons.Add(new Polygon(9, 8, 1));
    }

    public void Subdivide(int recursions)
    {
        Dictionary<int, int> midPointCache = new Dictionary<int, int>();

        for (int i = 0; i < recursions; i++)
        {
            List<Polygon> newPolys = new List<Polygon>();
            foreach (Polygon poly in m_Polygons)
            {
                int a = poly.m_Vertices[0];
                int b = poly.m_Vertices[1];
                int c = poly.m_Vertices[2];

                int ab = GetMidPoint(midPointCache, a, b);
                int bc = GetMidPoint(midPointCache, b, c);
                int ca = GetMidPoint(midPointCache, c, a);

                newPolys.Add(new Polygon(a, ab, ca));
                newPolys.Add(new Polygon(b, bc, ab));
                newPolys.Add(new Polygon(c, ca, bc));
                newPolys.Add(new Polygon(ab, bc, ca));
            }

            m_Polygons = newPolys;
        }
    }

    private int GetMidPoint(Dictionary<int, int> cache, int indexA, int indexB)
    {
        int smallerIndex = Mathf.Min(indexA, indexB);
        int greaterIndex = Mathf.Max(indexA, indexB);
        int key = (smallerIndex << 16) + greaterIndex;

        int ret;
        if (cache.TryGetValue(key, out ret))
            return ret;

        Vector3 p1 = m_Vertices[indexA];
        Vector3 p2 = m_Vertices[indexB];
        Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

        ret = m_Vertices.Count;
        m_Vertices.Add(middle);

        cache.Add(key, ret);
        return ret;
    }

    public GameObject GenerateMesh(string name, Material material)
    {
        GameObject meshObject = new GameObject(name);
        meshObject.transform.parent = transform;

        MeshRenderer surfaceRenderer = meshObject.AddComponent<MeshRenderer>();
        surfaceRenderer.material = material;

        Mesh terrainMesh = new Mesh();

        int vertexCount = m_Polygons.Count * 3;

        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];

        for (int i = 0; i < m_Polygons.Count; i++)
        {
            Polygon poly = m_Polygons[i];

            indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;

            vertices[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
            vertices[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
            vertices[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];

            uvs[i * 3 + 0] = poly.m_UVs[0];
            uvs[i * 3 + 1] = poly.m_UVs[1];
            uvs[i * 3 + 2] = poly.m_UVs[2];

            colors[i * 3 + 0] = poly.m_Color;
            colors[i * 3 + 1] = poly.m_Color;
            colors[i * 3 + 2] = poly.m_Color;

            if (poly.m_SmoothNormals)
            {
                normals[i * 3 + 0] = m_Vertices[poly.m_Vertices[0]];
                normals[i * 3 + 1] = m_Vertices[poly.m_Vertices[1]];
                normals[i * 3 + 2] = m_Vertices[poly.m_Vertices[2]];
            }
            else
            {
                Vector3 ab = m_Vertices[poly.m_Vertices[1]] - m_Vertices[poly.m_Vertices[0]];
                Vector3 ac = m_Vertices[poly.m_Vertices[2]] - m_Vertices[poly.m_Vertices[0]];

                Vector3 normal = Vector3.Cross(ab, ac).normalized;

                normals[i * 3 + 0] = normal;
                normals[i * 3 + 1] = normal;
                normals[i * 3 + 2] = normal;
            }
        }

        terrainMesh.vertices = vertices;
        terrainMesh.normals = normals;
        terrainMesh.colors32 = colors;
        terrainMesh.uv = uvs;

        terrainMesh.SetTriangles(indices, 0);

        MeshFilter terrainFilter = meshObject.AddComponent<MeshFilter>();
        terrainFilter.mesh = terrainMesh;

        return meshObject;
        //Kolpa has small pp
    }

    public void CalculateNeighbours()
    {
        foreach (Polygon poly in m_Polygons)
        {
            foreach (Polygon other_poly in m_Polygons)
            {
                if (poly == other_poly)
                    continue;

                if (poly.isNeighbourOf(other_poly))
                    poly.m_Neighbours.Add(other_poly);
            }
        }
    }

    public List<int> CloneVertices(List<int> old_verts)
    {
        List<int> new_verts = new List<int>();

        foreach (int old_vert in old_verts)
        {
            Vector3 cloned_vert = m_Vertices[old_vert];
            new_verts.Add(m_Vertices.Count);
            m_Vertices.Add(cloned_vert);
        }

        return new_verts;
    }

    public PolySet StitchPolys(PolySet polys)
    {
        PolySet stichedPolys = new PolySet();

        EdgeSet edgeSet = polys.CreateEdgeSet();

        List<int> originalVerts = edgeSet.GetUniqueVertices();

        List<int> newVerts = CloneVertices(originalVerts);

        edgeSet.Split(originalVerts, newVerts);

        foreach (Edge edge in edgeSet)
        {
            Polygon stitch_poly1 = new Polygon(edge.m_OuterVerts[0], edge.m_OuterVerts[1], edge.m_InnerVerts[0]);
            Polygon stitch_poly2 = new Polygon(edge.m_OuterVerts[1], edge.m_InnerVerts[1], edge.m_InnerVerts[0]);

            edge.m_InnerPoly.ReplaceNeighbour(edge.m_OuterPoly, stitch_poly2);
            edge.m_OuterPoly.ReplaceNeighbour(edge.m_InnerPoly, stitch_poly1);

            m_Polygons.Add(stitch_poly1);
            m_Polygons.Add(stitch_poly2);

            stichedPolys.Add(stitch_poly1);
            stichedPolys.Add(stitch_poly2);
        }

        foreach (Polygon poly in polys)
        {
            for (int i = 0; i < 3; i++)
            {
                int vert_id = poly.m_Vertices[i];
                if (!originalVerts.Contains(vert_id))
                    continue;

                int vert_index = originalVerts.IndexOf(vert_id);
                poly.m_Vertices[i] = newVerts[vert_index];
            }
        }

        return stichedPolys;
    }

    public PolySet Extrude(PolySet polys, float height)
    {
        PolySet stitchedPolys = StitchPolys(polys);
        List<int> verts = polys.GetUniqueVertices();

        foreach (int vert in verts)
        {
            Vector3 v = m_Vertices[vert];
            v = v.normalized * (v.magnitude + height);
            m_Vertices[vert] = v;
        }

        return stitchedPolys;
    }

    public PolySet Inset(PolySet polys, float interpolation)
    {
        PolySet stitchedPolys = StitchPolys(polys);
        List<int> verts = polys.GetUniqueVertices();

        Vector3 center = Vector3.zero;
        foreach (int vert in verts)
        {
            center += m_Vertices[vert];
        }
        center /= verts.Count;

        foreach (int vert in verts)
        {
            Vector3 v = m_Vertices[vert];
            float height = v.magnitude;
            v = Vector3.Lerp(v, center, interpolation);
            v = v.normalized * height;
            m_Vertices[vert] = v;
        }

        return stitchedPolys;
    }

    public PolySet GetPolysInSphere(Vector3 center, float radius, IEnumerable<Polygon> source)
    {
        PolySet newSet = new PolySet();
        foreach (Polygon p in source)
        {
            foreach (int vertexIndex in p.m_Vertices)
            {
                float distanceToSphere = Vector3.Distance(center, m_Vertices[vertexIndex]);
                float randomness = UnityEngine.Random.Range(0.0f, 0.2f);
                if (radius - (distanceToSphere + randomness) > 0)
                {
                    newSet.Add(p);
                    break;
                }
            }
        }

        return newSet;
    }

    public void changeViewMode()
    {
        viewMode++;
        if (viewMode == 4)
            viewMode = 0;
        switch (viewMode)
        {
            case 0:
                foreach (Polygon landPoly in landPolys)
                {
                    switch (landPoly.groundType)
                    {
                        case 0:
                            landPoly.m_Color = colorSand;
                            break;
                        case 1:
                            landPoly.m_Color = colorGravel;
                            break;
                        case 2:
                            landPoly.m_Color = colorDirt;
                            break;
                        case 3:
                            landPoly.m_Color = colorStone;
                            break;
                        default:
                            landPoly.m_Color = colorDirt;
                            break;
                    }

                    foreach (Plant plant in Plants)
                    {
                        switch (plant.getState())
                        {
                            case -1:
                                plant.position.m_Color = new Color32(0, 0, 0, 0);
                                break;
                            case 1:
                                plant.position.m_Color = colorGreenerGrass;
                                break;
                            case 0:
                                plant.position.m_Color = colorGrass;
                                break;
                            default:
                                break;

                        }
                    }
                }
                break;
            case 1:
                foreach (Polygon landPoly in landPolys)
                {
                    landPoly.m_Color = Color32.Lerp(colorToCold, colorToHot, (landPoly.temperature * 2) / 255f);
                }
                break;
            case 2:
                foreach (Polygon landPoly in landPolys)
                {
                    landPoly.m_Color = Color32.Lerp(colorNeutral, colorToHumid, landPoly.humidity / 100f);
                }
                break;
            case 3:
                foreach (Polygon landPoly in landPolys)
                {
                    landPoly.m_Color = Color32.Lerp(colorNeutral, colorSulfur, landPoly.sulfurLevel / 255f);
                }
                break;
        }

        if (m_GroundMesh != null)
            Destroy(m_GroundMesh);
        m_GroundMesh = GenerateMesh("Ground Mesh", m_GroundMaterial);
    }

    public void generateTemperature()
    {
        Vector3 centre = Vector3.zero;
        foreach (Polygon poly in m_Polygons)
        {
            foreach (int vertex in poly.m_Vertices)
            {
                centre += m_Vertices[vertex];
            }
            centre /= poly.m_Vertices.Count;
            poly.calculateTemperature(centre, temperatureOffset);
        }

        System.Random rnd = new System.Random();
        NoiseTest.Noise.Seed = rnd.Next();
    }

    public void generateHumidity()
    {
        Vector3 centre = Vector3.zero;
        foreach(Polygon poly in m_Polygons)
        {
            foreach (int vertex in poly.m_Vertices)
            {
                centre += m_Vertices[vertex];
            }
            centre /= poly.m_Vertices.Count;
            poly.calculateHumidity(centre);
        }

        System.Random rnd = new System.Random();
        NoiseTest.Noise.Seed = rnd.Next();
    }

    public void generateSulfurLevels()
    {
        Vector3 centre = Vector3.zero;
        foreach (Polygon poly in m_Polygons)
        {
            foreach (int vertex in poly.m_Vertices)
            {
                centre += m_Vertices[vertex];
            }
            centre /= poly.m_Vertices.Count;
            poly.calculateSulfurLevel(centre);
        }

        System.Random rnd = new System.Random();
        NoiseTest.Noise.Seed = rnd.Next();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetClickedPoly();
        }

        foreach (Plant plant in Plants)
        {
            sbyte state = plant.checkGroundCompatibility(plant.position.temperature, plant.position.humidity, plant.position.sulfurLevel, plant.position.groundType);
            sbyte lastState = plant.lastState;
            
            if (state != lastState)
            {
                switch (state)
                {
                    case -1:
                        plant.position.m_Color = new Color32(0, 0, 0, 0);
                        break;
                    case 1:
                        plant.position.m_Color = colorGreenerGrass;
                        break;
                    case 0:
                        plant.position.m_Color = colorGrass;
                        break;
                    default:
                        break;
                        
                }
                plant.lastState = state;

                if (m_GroundMesh != null)
                    Destroy(m_GroundMesh);
                m_GroundMesh = GenerateMesh("Ground Mesh", m_GroundMaterial);
                m_GroundMesh.AddComponent<MeshCollider>();
            }
        }
    }

    private void GetClickedPoly()
    {
        RaycastHit hit;

        Ray ray = CameraPosition.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Polygon p = m_Polygons[hit.triangleIndex];

            if (landPolys.Contains(p))
            {
                Plants.Add(new Plant(p));
            }
        }
    }
}
