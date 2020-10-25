[System.Serializable]
public class AvailableException : System.Exception
{
    public AvailableException() { }
    public AvailableException(string message) : base(message) { }
    public AvailableException(string message, System.Exception inner) : base(message, inner) { }
    protected AvailableException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}