using System;
using UnityEngine.UIElements;

namespace FavTool
{
	public class BaseController: IDisposable
	{
		protected VisualElement panel;
		protected string FilterValue { get; set; }

		internal BaseController(VisualElement root)
		{
			panel = root.Q<VisualElement>(GetType().Name);
			Hide();
		}

		internal virtual void Filter(string filterParam) => FilterValue = filterParam;

		#region IDisposable
		public virtual void Dispose() { }
		#endregion

		internal void Hide() => panel.AddToClassList("hide");
		internal void Show() => panel.RemoveFromClassList("hide");
	}
}
