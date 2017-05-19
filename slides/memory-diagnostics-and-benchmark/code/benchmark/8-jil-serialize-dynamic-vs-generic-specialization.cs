using System;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using Jil;

[MemoryDiagnoser]
public class JilSerializeDynamicVsGenericSpecialization
{
    [Benchmark]
    public object serialize()
    {
        return JSON.Serialize(Data);
    }

    [Benchmark]
    public object serialzie_dynamic()
    {
        return JSON.SerializeDynamic(Data);
    }

    [Benchmark]
    public object serialize_generic()
    {
        return _serialize(Data, Options.Default);
    }

    [Benchmark]
    public object serialize_generic_reflection()
    {
        return _serializeReflection.Invoke(null, new object[] { Data, Options.Default });
    }

    private static readonly MethodInfo _serializeReflection =
        typeof(JSON)
            .GetTypeInfo()
            .GetDeclaredMethods("Serialize")
            .First(m => m.ReturnParameter.ParameterType == typeof(string))
            .MakeGenericMethod(typeof(Message));

    private static readonly Func<Message, Options, string> _serialize = (Func<Message, Options, string>)
        typeof(JSON)
            .GetTypeInfo()
            .GetDeclaredMethods("Serialize")
            .First(m => m.ReturnParameter.ParameterType == typeof(string))
            .MakeGenericMethod(typeof(Message))
            .CreateDelegate(typeof(Func<Message, Options, string>));
            
    private static readonly Message Data = new Message
    {
        AudioSeconds = 10,
        AudioId = "adfasfda8172681736182731",
        ImageId = "ajfoiwjflkwejfwlkjkjioll",
        Text = "Hello, what are you doing how",
        Status = MessageStatus.Failed,
        CreatedAt = long.MaxValue,
    };
}