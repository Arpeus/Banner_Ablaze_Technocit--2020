using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealer : CharacterManager
{
    public List<CharacterManager> _allyNeighbor;

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
        bool isEnemyAround, isAllyAround;
        isEnemyAround = CheckEnemy();
        isAllyAround = CheckAllies();
        if (isEnemyAround && isAllyAround)
        {
            m_hudInGame.ShowActionAttackUi(this, true, true, true);
        }
        else if (isAllyAround)
        {
            m_hudInGame.ShowActionAttackUi(this, false, true, true);
        }
        else if (isEnemyAround)
        {
            m_hudInGame.ShowActionAttackUi(this, true, false, false);
        }
        else
        {
            m_hudInGame.ShowActionAttackUi(this, false, true, false);
        }
        
        pathToTravel = null;
        _animator.SetBool("_IsMoving", false);
    }

    public void Heal()
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell neighbor = null;
            if (location.GetNeighbor(d) != null)
            {
                neighbor = location.GetNeighbor(d);
                if (neighbor.CharacterManager != null && neighbor.CharacterManager._playerNumberType == this._playerNumberType)
                {
                    _allyNeighbor.Add(neighbor.CharacterManager);
                }
                if (_character._range > 1)
                {
                    for (HexDirection e = HexDirection.NE; e <= HexDirection.NW; e++)
                    {
                        HexCell neighborTest = null;
                        if (neighbor.GetNeighbor(e) != null)
                        {
                            neighborTest = neighbor.GetNeighbor(e);
                            if (
                                    neighborTest.CharacterManager != null 
                                    && neighborTest.CharacterManager._playerNumberType == this._playerNumberType 
                                    && neighborTest.CharacterManager != this
                                )
                            {
                                _allyNeighbor.Add(neighborTest.CharacterManager);
                            }
                        }
                    }
                }
            }
        }
    }

    public bool CheckAllies()
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            HexCell neighbor = null;
            if (location.GetNeighbor(d) != null)
            {
                neighbor = location.GetNeighbor(d);
                if (neighbor.CharacterManager != null && neighbor.CharacterManager._playerNumberType == this._playerNumberType)
                    return true;
                if (_character._range > 1)
                {
                    for (HexDirection e = HexDirection.NE; e <= HexDirection.NW; e++)
                    {
                        HexCell neighborTest = null;
                        if (neighbor.GetNeighbor(e) != null)
                        {
                            neighborTest = neighbor.GetNeighbor(e);
                            if (
                                neighborTest.CharacterManager != null
                                && neighborTest.CharacterManager._playerNumberType == this._playerNumberType
                                && neighborTest.CharacterManager != this
                                )
                                return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public void ClearAlly()
    {
        _allyNeighbor.Clear();
    }

}
