[System.Serializable]
public class TimeOfLastChangeException : System.Exception
{
    public TimeOfLastChangeException() { }
    public TimeOfLastChangeException(string message) : base(message) { }
    public TimeOfLastChangeException(string message, System.Exception inner) : base(message, inner) { }
    protected TimeOfLastChangeException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}