[System.Serializable]
public class NoRestorePointReturnedException : System.Exception
{
    public NoRestorePointReturnedException() { }
    public NoRestorePointReturnedException(string message) : base(message) { }
    public NoRestorePointReturnedException(string message, System.Exception inner) : base(message, inner) { }
    protected NoRestorePointReturnedException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}