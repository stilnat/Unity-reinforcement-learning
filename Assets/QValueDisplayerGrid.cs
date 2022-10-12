using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QValueDisplayerGrid : MonoBehaviour
{
    GameObject[,] _textTabForwardZ;
    GameObject[,] _textTabBackwardZ;
    GameObject[,] _textTabForwardX;
    GameObject[,] _textTabBackwardX;
    float posZ = 0.055f;
    float scale = 0.04f;

    float posXForwardZ = -2.85f;
    float posYForwardZ = -4.1f;
    float rotationForwardZ = 0f;

    float posXBackwardZ = -2.85f;
    float posYBackwardZ = -4.9f;
    float rotationBackwardZ = -180f;

    float posXForwardX = -2.63f;
    float posYForwardX = -4.5f;
    float rotationForwardX = -90f;

    float posXBackwardX = -3.35f;
    float posYBackwardX = -4.5f;
    float rotationBackwardX = 90f;
    Canvas _canvas;

    private void Awake()
    {
        _textTabForwardZ = new GameObject[7, 10];
        _textTabBackwardZ = new GameObject[7, 10];
        _textTabForwardX = new GameObject[7, 10];
        _textTabBackwardX = new GameObject[7, 10];
        _canvas = GetComponent<Canvas>();
        createDisplay(posXForwardZ, posYForwardZ,rotationForwardZ, _textTabForwardZ, "forwardZ");
        createDisplay(posXBackwardZ, posYBackwardZ,rotationBackwardZ, _textTabBackwardZ, "backwardZ");
        createDisplay(posXForwardX, posYForwardX, rotationForwardX, _textTabForwardX, "forwardX");
        createDisplay(posXBackwardX, posYBackwardX,rotationBackwardX, _textTabBackwardX, "backwardX");
    }

    void createDisplay(float posXInit, float posYInit, float rotation, GameObject[,] textTab, string nameAction)
    {
        float posX = posXInit;
        float posY;
        TextMeshPro textMeshPro;
        for (int i = 0; i < 7; i++)
        {
            posY = posYInit;
            for (int j = 0; j < 10; j++)
            {
                textTab[i, j] = new GameObject();
                textTab[i, j].name = "(" + i + "," + j + ") : " + nameAction;
                textTab[i, j].transform.parent = _canvas.transform;
                textTab[i, j].AddComponent<TextMeshPro>();
                textMeshPro = textTab[i, j].GetComponent<TextMeshPro>();
                textMeshPro.color = new Color(255, 0, 0);
                textMeshPro.rectTransform.localRotation = Quaternion.Euler(0, 0, rotation); 
                textMeshPro.rectTransform.localPosition = new Vector3(posX, posY, posZ);
                textMeshPro.rectTransform.localScale = new Vector3(scale, scale, scale);
                textMeshPro.gameObject.AddComponent<QValueShower>();
                textMeshPro.overflowMode = TextOverflowModes.Truncate;
                textMeshPro.alignment = TextAlignmentOptions.Center;
                textMeshPro.maxVisibleCharacters = 7;
                posY += 1;

            }
            posX += 1;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
