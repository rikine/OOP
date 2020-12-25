[System.Serializable]
public class DeleteException : System.Exception
{
    public DeleteException() { }
    public DeleteException(string message) : base(message) { }
    public DeleteException(string message, System.Exception inner) : base(message, inner) { }
    protected DeleteException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}