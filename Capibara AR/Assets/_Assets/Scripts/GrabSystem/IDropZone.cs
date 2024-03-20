public interface IDropZone
{
    public bool IsRemovable { get; set; }
    public void ItemReceived();
    public void RemoveItem();
}
