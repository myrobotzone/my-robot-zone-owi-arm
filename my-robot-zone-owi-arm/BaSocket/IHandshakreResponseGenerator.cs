namespace BaSocket
{
    public interface IHandshakeResponseGenerator
    {
        string Generate(string request);
    }
}