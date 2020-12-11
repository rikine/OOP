[System.Serializable]
public class TakeOfException : System.Exception
{
    public TakeOfException() { }
    public TakeOfException(string message) : base(message) { }
    public TakeOfException(string message, System.Exception inner) : base(message, inner) { }
    protected TakeOfException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}