[System.Serializable]
public class LimitDoubtException : System.Exception
{
    public LimitDoubtException() { }
    public LimitDoubtException(string message) : base(message) { }
    public LimitDoubtException(string message, System.Exception inner) : base(message, inner) { }
    protected LimitDoubtException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}