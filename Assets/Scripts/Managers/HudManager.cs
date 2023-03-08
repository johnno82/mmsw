using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    private int _resourcesCount;
    private int _resourcesDelta;
    private int _resourcesCountShown;

    private bool _isUpdatingResourcesCount;

    private BaseEntity _selectedEntity;

    [field: SerializeField]
    protected Text resourcesCountText { get; private set; }

    [field: SerializeField]
    protected Button buildButtonPrefab { get; private set; }

    public void SetResorcesCount(int amount)
    {
        _resourcesCount = amount;
        _resourcesDelta = _resourcesCount - _resourcesCountShown;

    }

    public void SetSelectedEntity(BaseEntity entity)
    {
        _selectedEntity = entity;
        if (_selectedEntity != null)
        {

        }
        else
        {

        }
    }

    // Update is called once per frame  
    void Update()
    {
        if (_resourcesDelta != 0)
        {
            if (!_isUpdatingResourcesCount)
            {
                _isUpdatingResourcesCount = true;
                StartCoroutine(UpdateResourcesCountShownCoroutine());
            }
        }
        else
        {
            if (_isUpdatingResourcesCount)
            {
                _isUpdatingResourcesCount = false;
                StopCoroutine(UpdateResourcesCountShownCoroutine());                
            }
        }
    }

    IEnumerator UpdateResourcesCountShownCoroutine()
    {
        while (_resourcesCount != _resourcesCountShown)
        {
            yield return new WaitForSeconds(.01f);
            
            _resourcesCountShown += 1;
            if(this.resourcesCountText != null)
                this.resourcesCountText.text = _resourcesCountShown.ToString();            
        }

        _resourcesDelta = 0;
    }
}
