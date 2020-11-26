[System.Serializable]
public class DoubleFileException : System.Exception
{
    public DoubleFileException() { }
    public DoubleFileException(string message) : base(message) { }
    public DoubleFileException(string message, System.Exception inner) : base(message, inner) { }
    protected DoubleFileException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}