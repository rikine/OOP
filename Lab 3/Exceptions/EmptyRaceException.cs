[System.Serializable]
public class EmptyRaceException : System.Exception
{
    public EmptyRaceException() { }
    public EmptyRaceException(string message) : base(message) { }
    public EmptyRaceException(string message, System.Exception inner) : base(message, inner) { }
    protected EmptyRaceException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}