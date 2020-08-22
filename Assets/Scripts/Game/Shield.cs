using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private int _currentStrength = 2;
    [SerializeField]
    private Color[] _strengthColors;

    private SpriteRenderer _shieldVisual;
    #endregion

    void Start()
    {
        _shieldVisual = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Called when shield is active and player gets hit.
    /// </summary>
    /// <returns>true if shield is still active otherwise false.</returns>
    public bool DamageShield()
    {
        --_currentStrength;

        if (_currentStrength >= 0)
        {
            _shieldVisual.color = _strengthColors[_currentStrength];
        } else
        {
            _shieldVisual.color = _strengthColors[_strengthColors.Length - 1];
            _currentStrength = 2;
            gameObject.SetActive(false);
            return false;
        }

        return true;
    }
}
