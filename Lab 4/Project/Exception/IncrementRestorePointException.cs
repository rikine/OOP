[System.Serializable]
public class IncrementRestorePointException : System.Exception
{
    public IncrementRestorePointException() { }
    public IncrementRestorePointException(string message) : base(message) { }
    public IncrementRestorePointException(string message, System.Exception inner) : base(message, inner) { }
    protected IncrementRestorePointException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}