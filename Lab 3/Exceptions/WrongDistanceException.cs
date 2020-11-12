[System.Serializable]
public class WrongDistanceException : System.Exception
{
    public WrongDistanceException() { }
    public WrongDistanceException(string message) : base(message) { }
    public WrongDistanceException(string message, System.Exception inner) : base(message, inner) { }
    protected WrongDistanceException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}