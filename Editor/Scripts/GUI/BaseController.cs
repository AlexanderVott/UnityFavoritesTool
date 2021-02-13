using System;
using FavTool.Models;
using UnityEngine.UIElements;

namespace FavTool
{
	public class BaseController: IDisposable
	{
		protected ProfileModel _profile;

		protected VisualElement _panel;
		protected string FilterValue { get; set; }

		internal BaseController(VisualElement root)
		{
			_profile = ProfileModel.Instance;
			_panel = root.Q<VisualElement>(GetType().Name);
			Hide();
		}

		internal virtual void Filter(string filterParam) => FilterValue = filterParam;

		protected virtual void SubscribeEvents() { }
		protected virtual void UnSubscribeEvents() { }

		#region IDisposable
		public virtual void Dispose() { }
		#endregion

		internal void Hide() => _panel.AddToClassList("hide");
		internal void Show() => _panel.RemoveFromClassList("hide");
	}
}
