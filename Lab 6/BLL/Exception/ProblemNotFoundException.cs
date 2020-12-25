[System.Serializable]
public class ProblemNotFoundException : System.Exception
{
    public ProblemNotFoundException() { }
    public ProblemNotFoundException(string message) : base(message) { }
    public ProblemNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    protected ProblemNotFoundException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}