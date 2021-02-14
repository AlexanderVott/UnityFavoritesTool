namespace FavTool.Models
{
    internal interface ICollectModel
    {
	    void Add(string guid);
	    void Remove(string guid);
	    void Clear();
    }
}
