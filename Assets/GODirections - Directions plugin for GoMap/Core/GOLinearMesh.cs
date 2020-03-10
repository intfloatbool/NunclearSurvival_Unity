using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GoMap {

	public class GOLinearMesh
	{

		MeshFilter filter;
		MeshRenderer renderer;

		public Vector3[] verts;

		public float width;
		private Mesh mesh;

		Vector3[] vertices = new Vector3[0];
		int[] triangles = new int[0];
		Vector2[] uvs = new Vector2[0];

		public void load (GameObject go) {
			
			filter = go.GetComponent<MeshFilter> ();
			if (filter == null) {
				filter = (MeshFilter)go.AddComponent(typeof(MeshFilter));
			}

			renderer = go.GetComponent<MeshRenderer> ();
			if (renderer == null) {
				renderer = (MeshRenderer)go.AddComponent(typeof(MeshRenderer));
			}

			UpdateVertices();

			filter.sharedMesh = CreateMesh();
		}

		public void UpdateVertices()
		{
			if (verts.Length < 2) return; // minimum to make a line

			int count = verts.Length - 1;

			vertices = new Vector3[count*4];
			uvs = new Vector2[count*4];
			triangles = new int[(verts.Length-1)*6];

			List <Vector3> dirs = new List<Vector3> ();
			List <Vector3> tans = new List<Vector3> ();

			Vector3 tanVect = Vector3.down;

			for (int p = 0; p<verts.Length; p++)
			{
				Vector3 dir;
				Vector3 tangent;

				if (p == 0) // First
				{
					dir = (verts[p+1] - verts[p]).normalized; 
					tangent = Vector3.Cross( tanVect, dir).normalized;
				}

				else if (p != verts.Length-1) // Middles
				{
					dir = (verts[p+1] - verts[p]).normalized; 

					Vector3 dirBefore = (verts [p] - verts [p-1]).normalized;
					Vector3 tangentBefore = Vector3.Cross(tanVect, dirBefore).normalized;

					tangent =  Vector3.Cross( tanVect,(dirBefore + dir) * 0.5f ).normalized;

				}

				else // Last
				{
					Vector3 dirBefore = (verts [p] - verts [p-1]).normalized;
					Vector3 tangentBefore = Vector3.Cross( tanVect, dirBefore).normalized;

					dir = dirBefore; 
					tangent = Vector3.Cross( tanVect, dir).normalized;
				}

				dirs.Add (dir);
				tans.Add (tangent);

			}


			for (int i = 0; i<count; i++)
			{
				vertices[(i*4)+0] = verts[i] + (tans[i] * (width));
				vertices[(i*4)+1] = verts[i] - (tans[i] * (width));
				vertices[(i*4)+2] = verts[i+1] + (tans[i+1] * (width));
				vertices[(i*4)+3] = verts[i+1] - (tans[i+1] * (width));

				Vector2 offsetRight = new Vector2 (1,Vector3.Distance(vertices[(i*4)+1],vertices[(i*4)+3] )); // Green - Blue
				Vector2 offsetLeft = new Vector2 (1,Vector3.Distance(vertices[(i*4)+0],vertices[(i*4)+2] )); 	// Red _ Yellow

				uvs [(i * 4) + 0] = Vector2.zero;
				uvs [(i * 4) + 1] = Vector2.right;
				uvs [(i * 4) + 2] = Vector2.Scale(Vector2.up , offsetLeft);
				uvs [(i * 4) + 3] = Vector2.Scale(Vector2.one, offsetRight);

				triangles[(i*6)+0] = (i*4)+0;
				triangles[(i*6)+1] = (i*4)+2;
				triangles[(i*6)+2] = (i*4)+1;
				triangles[(i*6)+3] = (i*4)+2;
				triangles[(i*6)+4] = (i*4)+3;
				triangles[(i*6)+5] = (i*4)+1;




			}
//			Debug.DrawLine (vertices [0], vertices [0] + 100 * Vector3.up, Color.red, 1000);
//			Debug.DrawLine (vertices [1], vertices [1] + 100 * Vector3.up, Color.green, 1000);
//			Debug.DrawLine (vertices [2], vertices [2] + 100 * Vector3.up, Color.yellow, 1000);
//			Debug.DrawLine (vertices [3], vertices [3] + 100 * Vector3.up, Color.blue, 1000);
			for (int i = 0; i < vertices.Length; i++) 
			{
				
				vertices[i] = filter.transform.InverseTransformPoint(vertices[i]);
			}

		}
			
		public Mesh CreateMesh()
		{

			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.uv = uvs;

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			;

			return mesh;
		}

	}
}