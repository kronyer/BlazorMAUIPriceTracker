public class Paged<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
}