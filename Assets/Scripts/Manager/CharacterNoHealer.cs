using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNoHealer : CharacterManager
{
    
    public override void Travel(List<HexCell> path)
    {
        base.Travel(path);
        location.CharacterManager = null;
        location = path[path.Count - 1];
        location.CharacterManager = this;
        pathToTravel = path;
        StopAllCoroutines();
        StartCoroutine(TravelPath());
    }

    IEnumerator TravelPath()
    {
        Vector3 a, b, c = pathToTravel[0].Position;

        float t = Time.deltaTime * travelSpeed;
        for (int i = 1; i < pathToTravel.Count; i++)
        {
            currentTravelLocation = pathToTravel[i];
            a = c;
            b = pathToTravel[i - 1].Position;
            c = (b + currentTravelLocation.Position) * 0.5f;
            Grid.IncreaseVisibility(pathToTravel[i], VisionRange);
            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                d.y = 0f;
               
                yield return null;
            }
            Grid.DecreaseVisibility(pathToTravel[i], VisionRange);
            t -= 1f;
        }
        currentTravelLocation = null;

        a = c;
        b = location.Position;
        c = b;
        Grid.IncreaseVisibility(location, VisionRange);
        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            transform.localPosition = Bezier.GetPoint(a, b, c, t);
            Vector3 d = Bezier.GetDerivative(a, b, c, t);
            d.y = 0f;
            yield return null;
        }

        transform.localPosition = location.Position;
        
        ListPool<HexCell>.Add(pathToTravel);
        if (CheckEnemy())
        {
            m_hudInGame.ShowActionAttackUi(this);
        }
        else
        {
            m_hudInGame.ShowActionNoAttackUi(this);
        }
        pathToTravel = null;
        _animator.SetBool("_IsMoving", false);
    }
}
