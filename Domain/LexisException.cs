namespace Domain;

[Serializable]
public class LexisException : Exception
{
    #region Data

    public const int InvalidDataCode = 1;
    public const int NotFound = 2;

    public const string DataTag = "Lexis.Code";
    public const string PropertiesTag = "Lexis.Properties";

    public int? Detail
    {
        get => Data.Contains(DataTag) && Data[DataTag] is int code
            ? code
            : default(int?);
        set
        {
            if (value.HasValue)
            {
                Data[DataTag] = value;
            }
        }
    }

    public string[] InvalidData
    {
        get => Data.Contains(PropertiesTag) && Data[PropertiesTag] is string[] properties
            ? properties
            : Array.Empty<string>();
        set
        {
            if (value != null)
            {
                Data[PropertiesTag] = value;
            }
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Get a new instance
    /// </summary>
    /// <param name="message">message describing the exception</param>
    public LexisException(string message) : base(message) { }

    /// <summary>
    /// Wrap an exception
    /// </summary>
    /// <param name="message">message describing the exception</param>
    /// <param name="innerEx">An exception</param>
    public LexisException(string message, Exception innerEx) : base(message, innerEx) { }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Create <see cref="LexisException"/>
    /// </summary>
    /// <param name="code">Application custom code, user to better identify the context of the exception</param>
    /// <param name="message">A message</param>
    /// <param name="invalidData">Optional list of invalid properties' names</param>
    /// <returns>A new <see cref="LexisException"/></returns>
    public static LexisException Create(int code, string message, IEnumerable<string> invalidData = null!)
    {
        return new LexisException(message)
        {
            Detail = code,
            InvalidData = invalidData?.ToArray()!
        };
    }

    /// <summary>
    /// Create an <see cref="LexisException"/> with additional context data
    /// </summary>
    /// <param name="code">Application custom code, user to better identify the context of the exception</param>
    /// <param name="message">A message</param>
    /// <param name="innerEx">An Exception</param>
    /// <returns>A new <see cref="LexisException"/></returns>
    public static LexisException Create(int code, string message, Exception innerEx)
    {
        return new LexisException(message, innerEx)
        {
            Detail = code
        };
    }

    #endregion
}