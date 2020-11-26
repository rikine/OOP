[System.Serializable]
public class FileNotFoundException : System.Exception
{
    public FileNotFoundException() { }
    public FileNotFoundException(string message) : base(message) { }
    public FileNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    protected FileNotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}