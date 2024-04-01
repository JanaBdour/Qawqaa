using System;
using UnityEngine;

namespace Scripts.Tutorial
{
	public interface ITutorialService
	{
		event Action<string> OnShowText;
	}
}