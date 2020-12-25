[System.Serializable]
public class UpdatePersonException : System.Exception
{
    public UpdatePersonException() { }
    public UpdatePersonException(string message) : base(message) { }
    public UpdatePersonException(string message, System.Exception inner) : base(message, inner) { }
    protected UpdatePersonException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}