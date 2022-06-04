using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Connector", menuName = "ScritpableObjects/Connector/Player")]
public class PlayerConnector : ScriptableObject
{
    #region Movement Variables

    public Vector2 movementRaw;
    public float speed = 10f;

    #endregion
}
