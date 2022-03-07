using System.Collections.Generic;
using PathCreation.Utility;
using UnityEngine;
using UnityEditor;
using NameSpaceName;
namespace PathCreation.Examples {
   
    public class RoadMeshCreator : PathSceneTool {
        [Header ("Road settings")]
        public float roadWidth = .4f;
        [Range (0, .5f)]
        public float thickness = .15f;
        public bool flattenSurface;

        [Header ("Material settings")]
        public Material roadMaterial;
        public Material undersideMaterial;
        public Material sideMaterial;

        public float textureTiling = 1;
        [Header("SidePath settings")]
        public PathCreator leftPath;
        public PathCreator rightPath;
        public float distFromCenterPath = 1.5f;
        public float minDistanceBetweenPoints = 3f;
        public bool debug;
       public GameObject meshHolder;
        [SerializeField] GameObject endRoad;
        [SerializeField] GameObject startRoad;

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        Mesh mesh;
     Vector3[] cubepos;
 
        protected override void PathUpdated()
        {
            if (pathCreator != null)
            {
         
                    AssignMeshComponents();
                    AssignMaterials();
                    CreateRoadMesh();

                CreateSidePaths();
                
                endRoad.transform.position = pathCreator.path.GetPoint(pathCreator.path.NumPoints-1);
                endRoad.transform.eulerAngles = new Vector3(0, Vector3.Angle(Vector3.forward, pathCreator.path.GetNormal(pathCreator.path.NumPoints - 1))-90, 0);
                startRoad.transform.position = pathCreator.path.GetPoint(0);
            }
        }


        /*
        private void OnDrawGizmos()
        {

            if (!debug)
                return;
            for (int i = 0; i < cubepos.Count; i++)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawWireCube(cubepos[i].point, Vector3.one);

                
                float angle = 0;
                /*
                if (i == cubepos.Count - 1)
                {
                    angle = Vector3.Angle(cubepos[i], cubepos[i]);
                }
                else
                {
                    angle = Vector3.Angle(cubepos[i] , cubepos[i + 1]);
                }
                Gizmos.color = Color.red;
                
                if (cubepos.Count > 3)
                {
                    if (i < cubepos.Count - 1)
                    {
                        st = new Vector3(cubepos[i + 1].point.x - cubepos[i].point.x, 0, cubepos[i + 1].point.z - cubepos[i].point.z);
                        Gizmos.DrawLine(cubepos[i].point, cubepos[i + 1].point);
                    }
                    if (i < cubepos.Count - 2)
                    {
                        Gizmos.DrawLine(cubepos[i].point, cubepos[i + 2].point);
                        nd = new Vector3(cubepos[i + 2].point.x - cubepos[i].point.x, 0, cubepos[i + 2].point.z - cubepos[i].point.z);
                    }

                    angle = Vector3.Angle(nd, st);

                    Handles.color = Color.red;
                    GUIStyle guistyle = new GUIStyle();
                    guistyle.fontSize = 20;
                    guistyle.normal.textColor = Color.white;
                    guistyle.alignment = TextAnchor.MiddleCenter;
                    if (i < cubepos.Count - 1)
                    {
                        Handles.Label(cubepos[i].point, (angle + " >" + (360/3-  angles[i] * angles[i])).ToString(), guistyle);
                      //  angles.Add(angle);
                    }
                }
           
            }
        
        }


    */


        void CreateRoadMesh () {
            Vector3[] verts = new Vector3[path.NumPoints * 8];
            Vector2[] uvs = new Vector2[verts.Length];
            Vector3[] normals = new Vector3[verts.Length];

            int numTris = 2 * (path.NumPoints - 1) + ((path.isClosedLoop) ? 2 : 0);
            int[] roadTriangles = new int[numTris * 3];
            int[] underRoadTriangles = new int[numTris * 3];
            int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

            int vertIndex = 0;
            int triIndex = 0;

            // Vertices for the top of the road are layed out:
            // 0  1
            // 8  9
            // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
            int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
            int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

            bool usePathNormals = !(path.space == PathSpace.xyz && flattenSurface);

            for (int i = 0; i < path.NumPoints; i++) {
                Vector3 localUp = (usePathNormals) ? Vector3.Cross (path.GetTangent (i), path.GetNormal (i)) : path.up;
                Vector3 localRight = (usePathNormals) ? path.GetNormal (i) : Vector3.Cross (localUp, path.GetTangent (i));

                // Find position to left and right of current path vertex
                Vector3 vertSideA = path.GetPoint (i) - localRight * Mathf.Abs (roadWidth);
                Vector3 vertSideB = path.GetPoint (i) + localRight * Mathf.Abs (roadWidth);

                // Add top of road vertices
                verts[vertIndex + 0] = vertSideA;
                verts[vertIndex + 1] = vertSideB;
                // Add bottom of road vertices
                verts[vertIndex + 2] = vertSideA - localUp * thickness;
                verts[vertIndex + 3] = vertSideB - localUp * thickness;

                // Duplicate vertices to get flat shading for sides of road
                verts[vertIndex + 4] = verts[vertIndex + 0];
                verts[vertIndex + 5] = verts[vertIndex + 1];
                verts[vertIndex + 6] = verts[vertIndex + 2];
                verts[vertIndex + 7] = verts[vertIndex + 3];

                // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                uvs[vertIndex + 0] = new Vector2 (0, path.times[i]);
                uvs[vertIndex + 1] = new Vector2 (1, path.times[i]);

                // Top of road normals
                normals[vertIndex + 0] = localUp;
                normals[vertIndex + 1] = localUp;
                // Bottom of road normals
                normals[vertIndex + 2] = -localUp;
                normals[vertIndex + 3] = -localUp;
                // Sides of road normals
                normals[vertIndex + 4] = -localRight;
                normals[vertIndex + 5] = localRight;
                normals[vertIndex + 6] = -localRight;
                normals[vertIndex + 7] = localRight;

                // Set triangle indices
                if (i < path.NumPoints - 1 || path.isClosedLoop) {
                    for (int j = 0; j < triangleMap.Length; j++) {
                        roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                        // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                        underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                    }
                    for (int j = 0; j < sidesTriangleMap.Length; j++) {
                        sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                    }

                }

                vertIndex += 8;
                triIndex += 6;
            }

            mesh.Clear ();
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.normals = normals;
            mesh.subMeshCount = 3;
            mesh.SetTriangles (roadTriangles, 0);
            mesh.SetTriangles (underRoadTriangles, 1);
            mesh.SetTriangles (sideOfRoadTriangles, 2);
            mesh.RecalculateBounds ();
            //Debug.Log(path.length);
            string str = ( Mathf.FloorToInt(path.length).ToString());
            //Debug.Log(str.Length);
            str = str.Substring(0,str.Length-1);
            int result;
            int.TryParse(str, out result);
            textureTiling = result/2;
         
        }

        // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
        void AssignMeshComponents() {

            meshHolder = GameObject.FindGameObjectWithTag("RoadMeshHolder");
         //   DestroyImmediate(meshHolder);
        
                if (meshHolder == null)
                {
                    meshHolder = new GameObject("Road Mesh Holder");

                   
                }
            if (meshHolder.gameObject.GetComponent<MeshCollider>())
            {
                DestroyImmediate(meshHolder.gameObject.GetComponent<MeshCollider>());
            }

            if(!meshHolder.gameObject.GetComponent<MeshCollider>())
            {
                meshHolder.gameObject.AddComponent<MeshCollider>();
                meshHolder.gameObject.layer = LayerMask.NameToLayer("Water");
                meshHolder.gameObject.tag = "RoadMeshHolder";
            }
        

            meshHolder.transform.rotation = Quaternion.identity;
            meshHolder.transform.position = Vector3.zero;
            meshHolder.transform.localScale = Vector3.one;

            // Ensure mesh renderer and filter components are assigned
            if (!meshHolder.gameObject.GetComponent<MeshFilter> ()) {
                meshHolder.gameObject.AddComponent<MeshFilter> ();
            }
            if (!meshHolder.GetComponent<MeshRenderer> ()) {
                meshHolder.gameObject.AddComponent<MeshRenderer> ();
            }

            meshRenderer = meshHolder.GetComponent<MeshRenderer> ();
            meshFilter = meshHolder.GetComponent<MeshFilter> ();
            if (mesh == null) {
                mesh = new Mesh ();
            }
            meshFilter.sharedMesh = mesh;
        }

        void AssignMaterials () {
            if (roadMaterial != null && undersideMaterial != null) {
                meshRenderer.sharedMaterials = new Material[] { roadMaterial, undersideMaterial, sideMaterial };
                meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3 (1, textureTiling);
            }
        }

       
        void CreateSidePaths()
        {

            Vector3[] pns = new Vector3[((pathCreator.bezierPath.NumPoints - 1) / 3) + 1];
            int indx = 0;
            for (int i = 0; i < pathCreator.bezierPath.NumPoints; i += 3)
            {
                //Vector3 localUp = Vector3.Cross(pc.path.GetTangent(i), pc.path.GetNormal(i));//  pc.path.up;
                Vector3 localRight = pathCreator.path.GetNormalAtWorldpoint(pathCreator.bezierPath.GetPoint(i));// Vector3.Cross(localUp, pc.bezierPath.GetTangent(i)); //pc.path.GetNormal(i) :
                pns[indx] = pathCreator.bezierPath.GetPoint(i) - localRight * distFromCenterPath;
                indx++;
            }

            BezierPath shs = new BezierPath(pns, false, PathSpace.xyz);

            if (leftPath == null)
            {
                GameObject go = new GameObject("LeftPath");
                go.tag = "LeftPath";
                go.transform.SetParent(this.transform);
                
                go.gameObject.AddComponent<SidePath>();
                leftPath = go.gameObject.AddComponent<PathCreator>();
                go.gameObject.GetComponent<SidePath>().pc = leftPath;

            }
            leftPath.bezierPath = shs;
            leftPath.bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
            leftPath.bezierPath.IsClosed = false;
            if (leftPath.GetComponent<SidePath>())
            {
                leftPath.GetComponent<SidePath>().PathUpdated(minDistanceBetweenPoints);
            }
            Vector3[] pns2 = new Vector3[((pathCreator.bezierPath.NumPoints - 1) / 3) + 1];
            int indx2 = 0;
            for (int i = 0; i < pathCreator.bezierPath.NumPoints; i += 3)
            {
                Vector3 localRight = pathCreator.path.GetNormalAtWorldpoint(pathCreator.bezierPath.GetPoint(i));// Vector3.Cross(localUp, pc.bezierPath.GetTangent(i)); //pc.path.GetNormal(i) :
                pns2[indx2] = pathCreator.bezierPath.GetPoint(i) + localRight * distFromCenterPath;
                indx2++;
            }


            BezierPath shs2 = new BezierPath(pns2, false, PathSpace.xyz);
            if (rightPath == null)
            {
             //    = this.gameObject.AddComponent<PathCreator>();
                GameObject go = new GameObject("RightPath");
                go.tag = "RightPath";

                go.transform.SetParent(this.transform);
                go.gameObject.AddComponent<SidePath>();
                rightPath = go.gameObject.AddComponent<PathCreator>();

                go.gameObject.GetComponent<SidePath>().pc = rightPath;
            }
             rightPath.bezierPath= shs2;
            rightPath.bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
            rightPath.bezierPath.IsClosed = false;
            if (rightPath.GetComponent<SidePath>())
            {
                rightPath.GetComponent<SidePath>().PathUpdated(minDistanceBetweenPoints);
            }
        }
    }
}