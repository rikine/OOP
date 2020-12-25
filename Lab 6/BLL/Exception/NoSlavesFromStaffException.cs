[System.Serializable]
public class NoSlavesFromStaffException : System.Exception
{
    public NoSlavesFromStaffException() { }
    public NoSlavesFromStaffException(string message) : base(message) { }
    public NoSlavesFromStaffException(string message, System.Exception inner) : base(message, inner) { }
    protected NoSlavesFromStaffException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}