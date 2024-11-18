public interface IApiConfiguration
{
    public Server ServerType { get; set; }
    public string ServerAddress { get; set; }
    public uint ServerPort { get; set; }
}