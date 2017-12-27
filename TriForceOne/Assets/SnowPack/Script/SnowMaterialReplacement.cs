using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowMaterialReplacement : MonoBehaviour {

    Material newMaterial;

    [Range(0f, 5f)]
    public float snowAmount;

    [SerializeField]
    Texture2D snowAlbedo;
    [SerializeField]
    Texture2D snowMetallic;
    [SerializeField]
    Texture2D snowNormal;

    MeshRenderer[] replaceMaterial;
    Dictionary<string, Material> snowMaterials = new Dictionary<string, Material>();

	// Use this for initialization
	void Start () {
        replaceMaterial = FindObjectsOfType<MeshRenderer>();

        newMaterial = new Material(Shader.Find("SnowShader"));
        newMaterial.SetTexture("_SnowAlbedo", snowAlbedo);
        newMaterial.SetTexture("_SnowMetallic", snowMetallic);
        newMaterial.SetTexture("_SnowNormal", snowNormal);

        for(int i = 0; i < replaceMaterial.Length; i++)
        {
            Material oldMat = replaceMaterial[i].material;
            Debug.Log(oldMat.name);
            if (!snowMaterials.ContainsKey(oldMat.name))
            {
                newMaterial.SetTexture("_BaseAlbedo", oldMat.GetTexture("_MainTex"));
                newMaterial.SetTexture("_BaseNormal", oldMat.GetTexture("_BumpMap"));
                newMaterial.SetTexture("_BaseMetallic", oldMat.GetTexture("_MetallicGlossMap"));
                newMaterial.SetFloat("_SnowAmount", snowAmount);
                snowMaterials.Add(oldMat.name, new Material(newMaterial));
                replaceMaterial[i].material = snowMaterials[oldMat.name];
            } else if (snowMaterials.ContainsKey(oldMat.name))
            {
                Debug.Log("Existing Material!");
                replaceMaterial[i].material = snowMaterials[oldMat.name];
            }
        }
            
           // replaceMaterial[i].material = newMaterial;
        }
    }
	

