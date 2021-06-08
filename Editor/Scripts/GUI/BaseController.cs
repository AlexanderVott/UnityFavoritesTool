using System;
using System.Linq;
using System.Reflection;
using FavTool.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool
{
	public class BaseController: IDisposable
	{
		protected ProfileModel _profile;

		protected VisualElement _panel;
		internal VisualElement Panel => _panel;

		protected string FilterValue { get; set; }

		internal bool IsActive { get; private set; }

		internal BaseController(VisualElement root)
		{
			_profile = ProfileModel.Instance;

			var type = GetType();
			var template = Resources.Load<VisualTreeAsset>(GetVisualAttribute(type).path);
			_panel = template.CloneTree();
			_panel.name = type.Name;
			var visualRoot = root.Q<VisualElement>("visualRoot");
			if (visualRoot == null)
				visualRoot = root;
			visualRoot.Add(_panel);

			Hide();
		}

		private ControllerTemplateAttribute GetVisualAttribute(Type type) => type.GetCustomAttribute<ControllerTemplateAttribute>(true);

		internal virtual void Filter(string filterParam) => FilterValue = filterParam;

		protected virtual void SubscribeEvents() { }
		protected virtual void UnSubscribeEvents() { }

		#region IDisposable

		public virtual void Dispose()
		{
			UnSubscribeEvents();
		}
		#endregion

		internal void Hide()
		{
			IsActive = false;
			_panel.AddToClassList("hide");
		}

		internal void Show()
		{
			IsActive = true;
			_panel.RemoveFromClassList("hide");
		}
	}
}
