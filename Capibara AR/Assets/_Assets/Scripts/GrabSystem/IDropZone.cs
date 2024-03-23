public interface IDropZone
{
    public bool IsRemovable { get; set; }
    public void ItemReceived(IGrabbable grabbableReceived);
    public void RemoveItem(IGrabbable grabbableRemoved);
}
