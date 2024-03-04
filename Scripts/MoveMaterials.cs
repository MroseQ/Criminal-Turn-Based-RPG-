using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveMaterials : MonoBehaviour
{
    [SerializeField] private Material floorMaterialSource, skyboxMaterialSource;
    private Material floorMaterialOutput, skyboxMaterialOutput;
    [SerializeField] GameObject floor,skybox;
    float floorOffset,skyboxOffset;
    void Start()
    {
        floorMaterialOutput = new(floorMaterialSource);
        skyboxMaterialOutput = new(skyboxMaterialSource);
        floor.GetComponent<RawImage>().material = floorMaterialOutput;
        skybox.GetComponent<RawImage>().material = skyboxMaterialOutput;
        floorOffset = 0.5f;
        skyboxOffset = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        floorMaterialOutput.SetTextureOffset("_MainTex", new(floorOffset, -0.16f));
        skyboxMaterialOutput.SetTextureOffset("_MainTex", new(skyboxOffset, 1.8f));
        floorOffset = (floorOffset <= 2.5f) ? floorOffset + 0.0001f : 0.5f;
        skyboxOffset = (skyboxOffset <= 2.5f) ? skyboxOffset + 0.00002f : 0.5f;
    }
}
