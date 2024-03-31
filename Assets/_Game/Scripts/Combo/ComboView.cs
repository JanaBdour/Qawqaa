using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Combo
{
	public class ComboView : MonoBehaviour
	{
		[SerializeField] private Image         fillImage;
		[Inject]         private IComboService _comboService;
		
		private void Update( )
		{
			fillImage.fillAmount = !_comboService.IsCombo ? _comboService.Progress : 1 - _comboService.ComboProgress;
		}
	}
}