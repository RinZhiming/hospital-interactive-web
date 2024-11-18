using System;

public enum HttpErrorCode
{
    Default = 200,
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    InternalServerError = 500
}

public static class HttpError
{
    /// <summary>
    /// For catch error code from http request
    /// <list type="HttpErrorCode">
    ///     <listheader>
    ///         <term>Error Code</term>
    ///         <description>Error code from html</description>
    ///     </listheader>
    ///     <item>
    ///         <term>200</term>
    ///         <description>No Error</description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>Bad Request</description>
    ///     </item>
    ///     <item>
    ///         <term>401</term>
    ///         <description>Unauthorized</description>
    ///     </item>
    ///     <item>
    ///         <term>404</term>
    ///         <description>Not Found</description>
    ///     </item>
    ///     <item>
    ///         <term>500</term>
    ///         <description>Internal Server Error</description>
    ///     </item>
    /// </list>
    /// </summary>
    /// <param name="error">string</param>
    /// <returns></returns>
    public static HttpErrorCode HandleErrorCode(string error)
    {
        foreach (string errorCodeName in Enum.GetNames(typeof(HttpErrorCode)))
        {
            var errorCodeEnum = (HttpErrorCode)Enum.Parse(typeof(HttpErrorCode), errorCodeName);
            if (error.Contains(((int)errorCodeEnum).ToString()))
            {
                return errorCodeEnum ;
            }
        }

        return HttpErrorCode.Default;
    }
}
