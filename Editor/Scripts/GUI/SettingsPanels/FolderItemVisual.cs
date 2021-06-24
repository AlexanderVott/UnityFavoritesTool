using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool.GUI
{
	internal class FolderItemVisual : VisualElement
	{
		private TextField _field;
		public TextField Field => _field;

		private Button _btnDelete;
		public Button BtnDelete => _btnDelete;
		
		private Button _btnRename;
		public Button BtnRename => _btnRename;

		internal FolderItemVisual(string nameParameter) => Initialize(nameParameter);

		private void Initialize(string nameParameter)
		{
			var template = Resources.Load<VisualTreeAsset>("Settings/FolderItem");
			template.CloneTree(this);

			_field = this.Q<TextField>("nameFolderField");

			_btnRename = this.Q<Button>("btnRename");
			_btnDelete = this.Q<Button>("btnDelete");
		}
	}
}