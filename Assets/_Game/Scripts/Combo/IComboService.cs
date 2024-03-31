using System;
using UnityEngine;

namespace Scripts.Combo
{
    public interface IComboService
    {
        float Progress      { get; }
        bool  IsCombo       { get; }
        float ComboProgress { get; }

        event Action OnStartCombo;
        event Action OnEndCombo;
    }
}