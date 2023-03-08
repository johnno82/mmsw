using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : CelestialBody
{
    private bool _isUpdatingResorucesCount;

    [field: SerializeField]
    private int _resourcesCount = 0;

    [field: SerializeField]
    private int _resourcesRate = 0;

    [field: SerializeField]
    private int _resourcesLimit = 0;

    [field: SerializeField]
    private float _resourcesIncrementInterval = 0f;

    public void SetResourcesStats(
        int count, int rate, int limit, float incrementInterval)
    {
        _resourcesCount = Mathf.Max(0, count);
        _resourcesRate = rate;
        _resourcesLimit = Mathf.Min(limit, 999999);
        _resourcesIncrementInterval = Mathf.Max(1, incrementInterval);
    }

    public int GetResourcesCount()
    {
        return _resourcesCount;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        SetMainColor(Color.green);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        UpdateResourceCount();
    }

    private void UpdateResourceCount()
    {
        if (_resourcesCount < _resourcesLimit)
        {
            if (!_isUpdatingResorucesCount)
            {
                _isUpdatingResorucesCount = true;
                StartCoroutine(UpdateResourcesCountCoroutine());
            }
        }
        else
        {
            if (_isUpdatingResorucesCount)
            {
                _isUpdatingResorucesCount = false;
                StopCoroutine(UpdateResourcesCountCoroutine());
            }
        }
    }

    private IEnumerator UpdateResourcesCountCoroutine()
    {
        while (_resourcesCount < _resourcesLimit)
        {
            yield return new WaitForSeconds(
                Mathf.Max(1, _resourcesIncrementInterval));

            _resourcesCount = Mathf.Clamp(
                _resourcesCount + _resourcesRate,
                0,
                _resourcesLimit
            );
        }
    }

}
