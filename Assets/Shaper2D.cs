using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Shaper2D : MonoBehaviour {

	Mesh mesh;
	List<Vector3> vertices=new List<Vector3>(200);
	List<Vector3> uvs=new List<Vector3>(200);
	List<Color> colors=new List<Color>(200);
	int[] triangles;

	public Color meshColor;
	Color meshColorPrev;
	[Range(3,50)]
	public int sectorCount=36;
	int sectorCountPrev;
	[Range(1,360)]
	public float arcDegrees=360;
	float arcDegreesPrev;
	[Range(0,360)]
	public float rotation=90;
	float rotationPrev;
	[Range(0.1f,10f)]
	public float outterRadius=2f;
	float outterRadiusPrev;
	[Range(0f,10f)]
	public float innerRadius=1.5f;
	float innerRadiusPrev;
	[Range(0f,1f)]
	public float starrines=0f;
	float starrinesPrev;
	public Material useMaterial;

	//Private
	float a,radiusPlus;

	void Awake(){
		#if UNITY_EDITOR
		PrefabUtility.DisconnectPrefabInstance(gameObject);
		#endif
		//Create random color if no color is set
		if(meshColor.a==0){
			float hue=Random.Range(0f,1f);
			while((hue*360f)>=236f && (hue*360f)<=246f){
				hue=Random.Range(0f,1f);
			}
			float saturation=Random.Range(0.9f,1f);
			meshColor=Color.HSVToRGB(hue,saturation/2,1f);
		}
		MakeMesh();
	}

	void OnDrawGizmos(){
		Gizmos.color=meshColor;
		Gizmos.DrawMesh(mesh,transform.position,transform.rotation);
		Gizmos.color=Color.black;
		Gizmos.DrawWireMesh(mesh,transform.position,transform.rotation);
	}

	void OnDrawGizmosSelected(){
		Gizmos.color=Color.green;
		Gizmos.DrawWireMesh(mesh,transform.position,transform.rotation);
	}

	void OnRenderObject(){
		#if UNITY_EDITOR
		if(EditorApplication.isPaused){
			ApplyChangesIfAny();
			Graphics.DrawMesh(mesh,transform.position,transform.localRotation,useMaterial,gameObject.layer);
		}
		#endif
	}

	void Update(){
		ApplyChangesIfAny();
		Graphics.DrawMesh(mesh,transform.position,transform.localRotation,useMaterial,gameObject.layer);
	}

	void ApplyChangesIfAny(){
		if(somethingChanged){
			//Don't allow inner radius to be bigger than outter radius
			if(innerRadius>=outterRadius) outterRadius=innerRadius+0.1f;
			if(outterRadius<=innerRadius) innerRadius=outterRadius-0.1f;
			//When generating a star, we only allow even numbr of sectors larger or equal than 6
			//6 sectors is a star with 3 points
			if(starrines>0){
				if(sectorCount<6) sectorCount=6;
				if(sectorCount%2!=0) sectorCount++;
			}
			MakeMesh();
		}
	}

	bool somethingChanged{
		get{
			bool change=false;
			if(meshColor!=meshColorPrev){
				meshColorPrev=meshColor;
				change=true;
			}
			if(sectorCount!=sectorCountPrev){
				sectorCountPrev=sectorCount;
				change=true;
			}
			if(arcDegrees!=arcDegreesPrev){
				arcDegreesPrev=arcDegrees;
				change=true;
			}
			if(rotation!=rotationPrev){
				rotationPrev=rotation;
				change=true;
			}
			if(outterRadius!=outterRadiusPrev){
				outterRadiusPrev=outterRadius;
				change=true;
			}
			if(innerRadius!=innerRadiusPrev){
				innerRadiusPrev=innerRadius;
				change=true;
			}
			if(starrines!=starrinesPrev){
				starrinesPrev=starrines;
				change=true;
			}
			return change;
		}
	}

	//Called whenever we need to regenerate the mesh
	private void MakeMesh(){
		if(mesh==null) mesh=new Mesh();
		mesh.Clear();
		if(innerRadius==0){
			GenerateCircle();
		} else{
			GenerateArc();
		}
	}

	void GenerateCircle(){
		vertices.Clear();
		radiusPlus=0f;
		int realSectorCount=sectorCount;
		if(arcDegrees!=360f) realSectorCount++;
		for(int i=0;i<realSectorCount;i++){
			a=(((arcDegrees/sectorCount)*i+rotation)*Mathf.Deg2Rad);
			if(starrines>0){
				if(i%2==0) radiusPlus=(outterRadius*starrines);
				else radiusPlus=-(outterRadius*starrines);
			}
			vertices.Add(new Vector2(
				(float)(Mathf.Cos(a)*(outterRadius+radiusPlus)),
				(float)(Mathf.Sin(a)*(outterRadius+radiusPlus))
			));
		}
		vertices.Add(Vector3.zero);
		mesh.SetVertices(vertices);

		SetUVsAndColor();

		int trianglesNum=vertices.Count-1; //-1 because last point is center
		if(arcDegrees!=360f) trianglesNum--; //Downt join the ends
		if(triangles==null || triangles.Length!=trianglesNum*3) triangles=new int[trianglesNum*3];
		for(int i=0;i<trianglesNum;i++){
			triangles[(i*3)+0]=(i+1==vertices.Count-1?0:i+1);
			triangles[(i*3)+1]=i;
			triangles[(i*3)+2]=vertices.Count-1;
		}

		mesh.SetTriangles(triangles,0);
		mesh.RecalculateNormals();
	}

	void GenerateArc(){
		vertices.Clear();
		radiusPlus=0f;
		for(int i=0;i<(sectorCount+1);i++){
			a=((((arcDegrees/((sectorCount+1)-1))*i))+rotation)*Mathf.Deg2Rad;
			if(starrines>0){
				if(i%2==0) radiusPlus=(outterRadius*starrines);
				else radiusPlus=-(outterRadius*starrines);
			}
			vertices.Add(new Vector3(
				(float)(Mathf.Cos(a)*(outterRadius+radiusPlus)),
				(float)(Mathf.Sin(a)*(outterRadius+radiusPlus))
			));
		}
		for(int i=(sectorCount+1)-1;i>=0;i--){
			a=((arcDegrees/((sectorCount+1)-1))*i)+rotation;
			if(starrines>0){
				if(i%2==0) radiusPlus=(innerRadius*starrines);
				else radiusPlus=-(innerRadius*starrines);
			}
			vertices.Add(new Vector3(
				(float)(Mathf.Cos(a*Mathf.Deg2Rad)*(innerRadius+radiusPlus)),
				(float)(Mathf.Sin(a*Mathf.Deg2Rad)*(innerRadius+radiusPlus))
			));
		}
		mesh.SetVertices(vertices);

		SetUVsAndColor();

		if(triangles==null || triangles.Length!=sectorCount*6) triangles=new int[sectorCount*6];
		for(int i=0;i<sectorCount;i++){
			//First triangle
			triangles[(i*6)+0]=i+1;
			triangles[(i*6)+1]=i;
			triangles[(i*6)+2]=vertices.Count-(i+1);
			//Second triangle
			triangles[(i*6)+3]=i+1;
			triangles[(i*6)+4]=vertices.Count-(i+1);
			triangles[(i*6)+5]=vertices.Count-(i+2);
		}
		mesh.SetTriangles(triangles,0);
		mesh.RecalculateNormals();
	}

	void SetUVsAndColor(){
		uvs.Clear();
		for(int i=0;i<vertices.Count;i++){
			uvs.Add(Vector3.zero);
		}
		mesh.SetUVs(0,uvs);
		colors.Clear();
		for(int i=0;i<vertices.Count;i++){
			colors.Add(meshColor);
		}
		mesh.SetColors(colors);
	}

}
