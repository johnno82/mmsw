using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [field: SerializeField]
    protected PlayerManager playerManager { get; private set; }

    [field: SerializeField]
    protected HudManager hudManager { get; private set; }

    [field: SerializeField]
    protected GameObject planetPrefab { get; private set; }

    [field: SerializeField]
    protected GameObject starPrefab { get; private set; }

    [field: SerializeField]
    protected GameObject spaceShipPrefab { get; private set; }


    // Start is called before the first frame update
    private void Start()
    {
        CreateStarSystem();
    }

    // Update is called once per frame
    private void Update()
    {
        this.hudManager.SetResorcesCount(playerManager.GetTotalResourcesCount());
    }

    private void CreateStarSystem()
    {
        Star star = CreateStar();

        Planet homePlanet = CreatePlanet(star.transform.position, true);
        this.playerManager.OwnPlanet(homePlanet);
        

        for(int i = 0; i < 5; i++)
            CreateSpaceShips(homePlanet.transform.position);

        for (int i = 0; i < 5; i++)
            CreatePlanet(star.transform.position);

    }

    private Star CreateStar()
    {
        GameObject gameObject = Instantiate(this.starPrefab, Vector3.zero, Quaternion.identity);
        Star star = gameObject.GetComponent("Star") as Star;
        star.transform.position = Vector3.zero;
        star.isSelectable = true;
        star.isKillable = true;
        star.SetSize(Random.Range(14, 18));
        return star;
    }

    private Planet CreatePlanet(Vector3 starPosition, bool isHomePlanet = false)
    {
        GameObject gameObject = Instantiate(this.planetPrefab, Vector3.zero, Quaternion.identity);
        Planet planet = gameObject.GetComponent("Planet") as Planet;
        planet.transform.position = GetCloseToOriginRandomPosition(starPosition);
        planet.isSelectable = true;
        planet.isKillable = true;
        planet.SetSize(Random.Range(4, 8));
        planet.SetResourcesStats(0, 250, 10000, 5);
        planet.SetMainColor(isHomePlanet ? Color.white : Color.gray);
        return planet;
    }

    private SpaceShip CreateSpaceShips(Vector3 origin)
    {
        GameObject gameObject = Instantiate(this.spaceShipPrefab, Vector3.zero, Quaternion.identity);
        SpaceShip spaceShip = gameObject.GetComponent("SpaceShip") as SpaceShip;
        spaceShip.transform.position = GetCloseToOriginRandomPosition(origin, 20f, 40f);
        return spaceShip;
    }

    private Vector3 GetCloseToOriginRandomPosition(Vector3 origin, float minDistance = 60f, float maxDistance = 160f)
    {
        Vector2 randomCirclePosition = Random.insideUnitCircle;
        Vector3 planetPosition = new Vector3(randomCirclePosition.x, 0, randomCirclePosition.y);
        Vector3 direction = (planetPosition - origin).normalized;
        return origin + direction * Random.Range(minDistance, maxDistance);
    }

}
