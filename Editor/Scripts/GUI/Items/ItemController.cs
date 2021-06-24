using FavTool.GUI;
using FavTool.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace FavTool
{
    internal class ItemController
    {
	    private ItemVisual _visual;
	    internal ItemVisual Visual => _visual;
		
	    private ICollectModel _data;
	    private string _guid;

		internal ItemController(ICollectModel item, string guid)
		{
			_data = item;
			_guid = guid;
		    _visual = new ItemVisual(guid);
			
		    var preview = ToolUtils.GetPreviewAsset(ToolUtils.GetAssetByGuid<Object>(guid));
		    if (preview == null) 
			    preview = ToolUtils.GetThumbnailAsset(ToolUtils.GetAssetByGuid<Object>(guid));
		    _visual.SetIcon(preview);

			_visual.Field.value = ToolUtils.GetAssetByGuid<Object>(_guid);
			_visual.Field.RegisterCallback<ChangeEvent<Object>>(e =>
			{
				if (e.newValue != e.previousValue && e.newValue != ToolUtils.GetAssetByGuid<Object>(this._guid))
					_visual.Field.value = e.previousValue;
			});

			_visual.BtnDelete.clicked += () => _data.Remove(this._guid);
		}

		internal void Destroy() => _visual.RemoveFromHierarchy();
    }
}
