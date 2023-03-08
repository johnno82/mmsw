using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int _totalResourcesCount;

    private Planet _homePlanet;

    private List<Planet> _ownedPlanets;    

    public void OwnPlanet(Planet planet)
    {
        if (_ownedPlanets.Count == 0)
            _homePlanet = planet;

        if(!_ownedPlanets.Any(x => x.name.ToLower() == planet.name.ToLower()))
            _ownedPlanets.Add(planet);
    }

    public int GetTotalResourcesCount()
    {
        return _totalResourcesCount;
    }

    private void Awake()
    {
        _ownedPlanets = new List<Planet>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        _totalResourcesCount = _ownedPlanets.Sum(x => x.GetResourcesCount());
    }
}
