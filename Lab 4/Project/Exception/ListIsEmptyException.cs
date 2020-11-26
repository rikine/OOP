[System.Serializable]
public class ListIsEmptyException : System.Exception
{
    public ListIsEmptyException() { }
    public ListIsEmptyException(string message) : base(message) { }
    public ListIsEmptyException(string message, System.Exception inner) : base(message, inner) { }
    protected ListIsEmptyException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}