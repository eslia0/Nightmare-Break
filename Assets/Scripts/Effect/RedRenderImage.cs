using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class RedRenderImage : MonoBehaviour
{

	#region Variables
	public Shader curShader;
	public float redScaleAmount = 1.0f;
	private Material curMaterial;

	#endregion

	#region Properties
	Material material//material을 확인하고 material이 없는 경우 material을 붙여 준다.
	{
		get {
			if (curMaterial == null) {
				curMaterial = new Material (curShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return curMaterial;
		}
	}
	#endregion
	// Use this for initialization
	void Start () 
	{
		if (!SystemInfo.supportsImageEffects) //시스템에서 이미지 이펙트를 지원하는지 확인후 지원안하면 사용하지 않는다.
		{
			enabled = false;
			return;
		}
		if (!curShader && !curShader.isSupported) //셰이더가 없거나 지원하지 않는지 확인
		{
			enabled = false;
		}
	}
	void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)//렌더러로 부터 렌더링된 이미지를 갈무리하기위해 사용
	{
		if (curShader != null) 
		{
			material.SetFloat ("_LuminosityAmount", redScaleAmount);
			Graphics.Blit (sourceTexture, destTexture, material);
		}
		else 
		{
			Graphics.Blit (sourceTexture, destTexture);
		}
	}
	// Update is called once per frame
	void Update () 
	{
		redScaleAmount = Mathf.Clamp (redScaleAmount, 0.0f, 1.0f);//붉은 정도 범위 제한


	}
	void OnDisable()
	{
		if (curMaterial) 
		{
			DestroyImmediate (curMaterial);
		}
	}
}
