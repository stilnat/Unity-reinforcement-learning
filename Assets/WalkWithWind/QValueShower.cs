using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QValueShower : MonoBehaviour
{
    public TextMeshPro _qValueDisplay;
    MeshRenderer _colorRectangle;
    WalkerAgent _agent;
    RectTransform rect;
    State s;
    EnvironmentAction a;

    bool IsText =>  !(_qValueDisplay is null);
    bool IsRectangle => !(_colorRectangle is null);


    void Start()
    {
       
        _agent = FindObjectOfType<WalkerAgent>();
        _qValueDisplay = GetComponent<TextMeshPro>();
        TryGetComponent(out rect);

        _colorRectangle = GetComponent<MeshRenderer>();
        s = ComputeState();
        a = ComputeAction();
    }

    public void DisplayColor(QValueCollection _qValueCollection, float min, float max)
    {
        if (!_colorRectangle.enabled)
        {
            _colorRectangle.enabled = true;
        }
        UpdateColor(_qValueCollection, min, max);
    }

    public void UnDisplayColor()
    {
        if (!(_colorRectangle is null))
        {
            _colorRectangle.enabled = false;
        }
            
    }

    void UpdateColor(QValueCollection _qValueCollection, float min, float max)
    {

            if (_qValueCollection != null && _qValueCollection.ContainsKey(s) && _qValueCollection[s].ContainsKey(a))
            {
                _colorRectangle.material.SetColor("_Color", ComputeColor(_qValueCollection, min, max));
            }
            else
            {
                _colorRectangle.enabled = false;
                _colorRectangle.material.SetColor("_Color", new Color(255, 255, 255));
            }
    }

    public void DisplayText(QValueCollection _qValueCollection)
    {
        if (!_qValueDisplay.enabled)
        {
            _qValueDisplay.enabled = true;
        }
        UpdateText(_qValueCollection);
    }

    void UpdateText(QValueCollection _qValueCollection)
    {
        if (_qValueCollection != null && _qValueCollection.ContainsKey(s) && _qValueCollection[s].ContainsKey(a))
        {
            _qValueDisplay.text = _qValueCollection[s][a]._stateActionValue.ToString();
        }
        else
        {
            _qValueDisplay.text = "";
        }
    }

    public void UnDisplayText()
    {
        if(!(_qValueDisplay is null))
        _qValueDisplay.enabled = false;
    }


    Color ComputeColor(QValueCollection _qValueCollection, float min, float max)
    {
        float value = _qValueCollection[s][a]._stateActionValue;
        float distToMaxFraction = 1 - (max - value) / (max - min);
        if (distToMaxFraction < 0.5)
        {
            float g = distToMaxFraction * 2;
            return new Color(1f, g, 0f);
        }
        else
        {
            float r = 1 - (distToMaxFraction - 0.5f) * 2;
            return new Color(r, 1f, 0f);
        }
    }

    State ComputeState()
    {
        int x =0, y =0;

        if(_colorRectangle.gameObject.name == "(6,0) : forwardX")
        {
            Debug.Log("6,0");
        }

        if (_colorRectangle.gameObject.name == "(0,5) : forwardX")
        {
            Debug.Log("6,0");
        }

        if (IsText)
        {
            x = (int)rect.position.x;
            y = (int)rect.position.z;
        }
        if(IsRectangle)
        {
            x = (int)_colorRectangle.gameObject.transform.position.x;
            y = (int)_colorRectangle.gameObject.transform.position.z;
        }
        return new State(false, x, y);
    }

    EnvironmentAction ComputeAction()
    {
        if (IsText)
        {
            if (rect.localRotation.eulerAngles.z == 0) return new EnvironmentAction(_agent.MoveForwardZ);
            if (rect.localRotation.eulerAngles.z == 270) return new EnvironmentAction(_agent.MoveForwardX);
            if (rect.localRotation.eulerAngles.z == 180) return new EnvironmentAction(_agent.MoveBackwardZ);
            return new EnvironmentAction(_agent.MoveBackwardX);
        }
        else
        {
            if (_colorRectangle.gameObject.transform.localRotation.eulerAngles.z == 0) return new EnvironmentAction(_agent.MoveForwardZ);
            if (_colorRectangle.gameObject.transform.localRotation.eulerAngles.z == 270) return new EnvironmentAction(_agent.MoveForwardX);
            if (_colorRectangle.gameObject.transform.localRotation.eulerAngles.z == 180) return new EnvironmentAction(_agent.MoveBackwardZ);
            return new EnvironmentAction(_agent.MoveBackwardX);
        }
       
    }


}
