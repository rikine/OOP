[System.Serializable]
public class NoAlgorithmException : System.Exception
{
    public NoAlgorithmException() { }
    public NoAlgorithmException(string message) : base(message) { }
    public NoAlgorithmException(string message, System.Exception inner) : base(message, inner) { }
    protected NoAlgorithmException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}