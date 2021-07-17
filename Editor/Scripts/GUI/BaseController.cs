using System;
using System.Reflection;
using FavTool.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool
{
	public class BaseController: IDisposable
	{
		protected ProfileModel _profile;

		protected VisualElement _panel;
		internal VisualElement Panel => _panel;

		public virtual bool IsUseFilter { get; } = true;

		protected string FilterValue { get; set; }

		internal bool IsActive { get; private set; }

		internal BaseController(VisualElement root, bool autoHide = true)
		{
			_profile = ProfileModel.Instance;

			var type = GetType();
			var template = Resources.Load<VisualTreeAsset>(GetVisualAttribute(type).path);
			_panel = template.CloneTree();
			_panel.name = type.Name;
			if (root != null)
			{
				var visualRoot = root.Q<VisualElement>("visualRoot");
				if (visualRoot == null)
					visualRoot = root;
				if (visualRoot is ScrollView list)
				{
					list.Add(_panel);
				}
				else
				{
					visualRoot.Add(_panel);
				}
			}

			if (autoHide)
				Hide();
		}

		private ControllerTemplateAttribute GetVisualAttribute(Type type) => type.GetCustomAttribute<ControllerTemplateAttribute>(true);

		internal virtual void Filter(string filterParam) => FilterValue = filterParam;

		protected virtual void SubscribeEvents() { }
		protected virtual void UnSubscribeEvents() { }

		internal virtual void UpdateView(bool isHide) { }

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
			UpdateView(true);
			UnSubscribeEvents();
		}

		internal void Show()
		{
			IsActive = true;
			_panel.RemoveFromClassList("hide");
			UnSubscribeEvents();
			SubscribeEvents();
			UpdateView(false);
		}
	}
}
