using System;
using UnityEngine.UIElements;

namespace FavTool
{
	public class BaseController: IDisposable
	{
		protected string FilterValue { get; set; }

	    internal BaseController(VisualElement panel) { }

		internal virtual void Filter(string filterParam) => FilterValue = filterParam;
		public virtual void Dispose() { }
	}
}
