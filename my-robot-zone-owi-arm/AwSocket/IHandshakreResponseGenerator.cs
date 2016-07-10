namespace AwSocket
{
    public interface IHandshakeResponseGenerator
    {
        string Generate(string request);
    }
}