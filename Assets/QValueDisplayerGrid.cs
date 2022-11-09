using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QValueDisplayerGrid : MonoBehaviour
{
    QValueCollection _qValueCollection;

    GameObject[,] _textTabForwardZ;
    GameObject[,] _textTabBackwardZ;
    GameObject[,] _textTabForwardX;
    GameObject[,] _textTabBackwardX;

    GameObject[,] _colorRectangleForwardZ;
    GameObject[,] _colorRectangleBackwardZ;
    GameObject[,] _colorRectangleForwardX;
    GameObject[,] _colorRectangleBackwardX;

    float posZ = 0.055f;
    float scale = 0.04f;

    float textposXForwardZ = -2.85f;
    float textposYForwardZ = -4.1f;
    float rectangleposXForwardZ = -3.3f;
    float rectangleposYForwardZ = -4.15f;
    float rotationForwardZ = 0f;

    float textposXBackwardZ = -2.85f;
    float textposYBackwardZ = -4.9f;
    float rectangleposXBackwardZ = -2.7f;
    float rectangleposYBackwardZ = -4.85f;
    float rotationBackwardZ = -180f;

    float textposXForwardX = -2.63f;
    float textposYForwardX = -4.5f;
    float rectangleposXForwardX = -2.65f;
    float rectangleposYForwardX = -4.2f;
    float rotationForwardX = -90f;

    float textposXBackwardX = -3.35f;
    float textposYBackwardX = -4.5f;
    float rectangleposXBackwardX = -3.35f;
    float rectangleposYBackwardX = -4.8f;
    float rotationBackwardX = 90f;
    Canvas _canvas;
    bool initialized = false;

    bool _displayColor = false;
    bool _displayNumber = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("keyspace pressed");
            if (_displayColor == false && _displayNumber == false) _displayColor = true;
            else if(_displayColor == true && _displayNumber == false) { _displayNumber = true; _displayColor = false; }
            else{ _displayColor = false; _displayNumber = false; }
        }
        if (!initialized)
        {
            var trainer = FindObjectOfType<WalkerTrainerWithSarsaLambda>();
            _qValueCollection = trainer._trainingMethod._qValues;
            initialized = true;
        }

        if (_displayColor)
        {
            float max = _qValueCollection.Max();
            float min = _qValueCollection.Min();
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _colorRectangleBackwardZ[i, j].GetComponent<QValueShower>().DisplayColor(_qValueCollection, min, max);
                    _colorRectangleForwardZ[i, j].GetComponent<QValueShower>().DisplayColor(_qValueCollection, min, max);
                    _colorRectangleBackwardX[i, j].GetComponent<QValueShower>().DisplayColor(_qValueCollection, min, max);
                    _colorRectangleForwardX[i, j].GetComponent<QValueShower>().DisplayColor(_qValueCollection, min, max);
                }
            }
        }
        else if (_displayNumber)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _textTabBackwardZ[i, j].GetComponent<QValueShower>().DisplayText(_qValueCollection);
                    _textTabForwardZ[i, j].GetComponent<QValueShower>().DisplayText(_qValueCollection);
                    _textTabBackwardX[i, j].GetComponent<QValueShower>().DisplayText(_qValueCollection);
                    _textTabForwardX[i, j].GetComponent<QValueShower>().DisplayText(_qValueCollection);
                    _colorRectangleBackwardZ[i, j].GetComponent<QValueShower>().UnDisplayColor();
                    _colorRectangleForwardZ[i, j].GetComponent<QValueShower>().UnDisplayColor();
                    _colorRectangleBackwardX[i, j].GetComponent<QValueShower>().UnDisplayColor();
                    _colorRectangleForwardX[i, j].GetComponent<QValueShower>().UnDisplayColor();
                }
            }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _colorRectangleBackwardZ[i, j].GetComponent<QValueShower>().UnDisplayColor();
                    _colorRectangleForwardZ[i, j].GetComponent<QValueShower>().UnDisplayColor();
                    _colorRectangleBackwardX[i, j].GetComponent<QValueShower>().UnDisplayColor();
                    _colorRectangleForwardX[i, j].GetComponent<QValueShower>().UnDisplayColor();
                    _textTabBackwardZ[i, j].GetComponent<QValueShower>().UnDisplayText();
                    _textTabForwardZ[i, j].GetComponent<QValueShower>().UnDisplayText();
                    _textTabBackwardX[i, j].GetComponent<QValueShower>().UnDisplayText();
                    _textTabForwardX[i, j].GetComponent<QValueShower>().UnDisplayText();
                }
            }
        }

       
    }

    private void Awake()
    {
        _textTabForwardZ = new GameObject[7, 10];
        _textTabBackwardZ = new GameObject[7, 10];
        _textTabForwardX = new GameObject[7, 10];
        _textTabBackwardX = new GameObject[7, 10];
        _colorRectangleForwardZ = new GameObject[7, 10];
        _colorRectangleBackwardZ = new GameObject[7, 10];
        _colorRectangleForwardX = new GameObject[7, 10];
        _colorRectangleBackwardX = new GameObject[7, 10];


        _canvas = GetComponent<Canvas>();

        createDisplayText(textposXForwardZ, textposYForwardZ, rotationForwardZ, _textTabForwardZ, "forwardZ");
        createDisplayText(textposXBackwardZ, textposYBackwardZ, rotationBackwardZ, _textTabBackwardZ, "backwardZ");
        createDisplayText(textposXForwardX, textposYForwardX, rotationForwardX, _textTabForwardX, "forwardX");
        createDisplayText(textposXBackwardX, textposYBackwardX, rotationBackwardX, _textTabBackwardX, "backwardX");

        createDisplayColorRectangle(rectangleposXForwardZ, rectangleposYForwardZ, rotationForwardZ, _colorRectangleForwardZ, "forwardZ");
        createDisplayColorRectangle(rectangleposXBackwardZ, rectangleposYBackwardZ, rotationBackwardZ, _colorRectangleBackwardZ, "backwardZ");
        createDisplayColorRectangle(rectangleposXForwardX, rectangleposYForwardX, rotationForwardX, _colorRectangleForwardX, "forwardX");
        createDisplayColorRectangle(rectangleposXBackwardX, rectangleposYBackwardX, rotationBackwardX, _colorRectangleBackwardX, "backwardX");
    }

    void createDisplayColorRectangle(float posXInit, float posYInit, float rotation, GameObject[,] rectangleTab, string nameAction)
    {
        float posX = posXInit;
        float posY;

        for (int i = 0; i < 7; i++)
        {
            posY = posYInit;
            for (int j = 0; j < 10; j++)
            {
                rectangleTab[i, j] = new GameObject();
                rectangleTab[i, j].name = "(" + i + "," + j + ") : " + nameAction;
                rectangleTab[i, j].transform.parent = _canvas.transform;
                rectangleTab[i, j].AddComponent<MeshFilter>();
                rectangleTab[i, j].GetComponent<MeshFilter>().mesh = createMeshRectangle() ;
                rectangleTab[i, j].AddComponent<MeshRenderer>();
                rectangleTab[i, j].GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
                rectangleTab[i, j].AddComponent<QValueShower>();
                rectangleTab[i, j].gameObject.transform.localScale = new Vector3(0.6f, 0.1f, 0.1f);
                rectangleTab[i, j].gameObject.transform.localPosition = new Vector3(posX, posY, posZ);
                rectangleTab[i, j].gameObject.transform.localRotation = Quaternion.Euler(0, 0, rotation);
                posY += 1;

            }
            posX += 1;
        }
    }

    Mesh createMeshRectangle()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0) };
        mesh.triangles = new int[] { 0, 2,1, 1, 2, 3,0,1,2,3,2,1};
        return mesh;
    }

    void createDisplayText(float posXInit, float posYInit, float rotation, GameObject[,] textTab, string nameAction)
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
}
