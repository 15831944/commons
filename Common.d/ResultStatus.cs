namespace System
{
    #region Enums

    /// <summary>
    /// Defines the ResultStatus
    /// </summary>
    public enum ResultStatus
    {
        #region HTTP标准状态,IIS扩展状态,Nginx扩展状态等

        /// <summary>
        /// 等效于 HTTP 状态 100。System.Net.HttpStatusCode.Continue 指示客户端可以继续其请求。
        /// </summary>
        Continue = 100,

        /// <summary>
        /// 等效于 HTTP 状态为 101。System.Net.HttpStatusCode.SwitchingProtocols 指示正在更改的协议版本或协议。
        /// </summary>
        SwitchingProtocols = 101,

        Processing = 102,

        EarlyHints = 103,

        Checkpoint = 103,
        //
        // 摘要:
        //     等效于 HTTP 状态 200。System.Net.HttpStatusCode.OK 指示请求成功，且请求的信息包含在响应中。 这是要接收的最常见状态代码。

        /// <summary>
        /// Defines the OK
        /// </summary>
        OK = 200,

        //
        // 摘要:
        //     等效于 HTTP 状态 201。System.Net.HttpStatusCode.Created 指示请求导致已发送响应之前创建一个新的资源。

        /// <summary>
        /// Defines the Created
        /// </summary>
        Created = 201,

        //
        // 摘要:
        //     等效于 HTTP 状态 202。System.Net.HttpStatusCode.Accepted 指示请求已被接受进行进一步处理。

        /// <summary>
        /// Defines the Accepted
        /// </summary>
        Accepted = 202,

        //
        // 摘要:
        //     等效于 HTTP 状态 203。System.Net.HttpStatusCode.NonAuthoritativeInformation 指示返回的元信息来自而不是原始服务器的缓存副本，因此可能不正确。

        /// <summary>
        /// Defines the NonAuthoritativeInformation
        /// </summary>
        NonAuthoritativeInformation = 203,

        //
        // 摘要:
        //     等效于 HTTP 状态 204。System.Net.HttpStatusCode.NoContent 指示已成功处理请求和响应是有意留为空白。

        /// <summary>
        /// Defines the NoContent
        /// </summary>
        NoContent = 204,

        //
        // 摘要:
        //     等效于 HTTP 状态 205。System.Net.HttpStatusCode.ResetContent 指示客户端应重置 （而不是重新加载） 的当前资源。

        /// <summary>
        /// Defines the ResetContent
        /// </summary>
        ResetContent = 205,

        //
        // 摘要:
        //     等效于 HTTP 206 状态。System.Net.HttpStatusCode.PartialContent 指示根据包括字节范围的 GET 请求的请求的响应是部分响应。

        /// <summary>
        /// Defines the PartialContent
        /// </summary>
        PartialContent = 206,

        MultiStatus = 207,

        AlreadyReported = 208,

        IMUsed = 226,
        //
        // 摘要:
        //     等效于 HTTP 状态 300。System.Net.HttpStatusCode.MultipleChoices 指示所需的信息有多种表示形式。 默认操作是将此状态视为一个重定向，并按照与此响应关联的位置标头的内容。

        /// <summary>
        /// Defines the MultipleChoices
        /// </summary>
        MultipleChoices = 300,

        //
        // 摘要:
        //     等效于 HTTP 状态 300。System.Net.HttpStatusCode.Ambiguous 指示所需的信息有多种表示形式。 默认操作是将此状态视为一个重定向，并按照与此响应关联的位置标头的内容。

        /// <summary>
        /// Defines the Ambiguous
        /// </summary>
        Ambiguous = 300,

        //
        // 摘要:
        //     等效于 HTTP 状态 301。System.Net.HttpStatusCode.MovedPermanently 指示已将所需的信息移动到的位置标头中指定的
        //     URI。 当收到此状态时的默认操作是遵循与响应关联的位置标头。

        /// <summary>
        /// Defines the MovedPermanently
        /// </summary>
        MovedPermanently = 301,

        //
        // 摘要:
        //     等效于 HTTP 状态 301。System.Net.HttpStatusCode.Moved 指示已将所需的信息移动到的位置标头中指定的 URI。 当收到此状态时的默认操作是遵循与响应关联的位置标头。
        //     当原始请求方法是 POST 时，重定向的请求将使用 GET 方法。

        /// <summary>
        /// Defines the Moved
        /// </summary>
        Moved = 301,

        //
        // 摘要:
        //     等效于 HTTP 状态 302。System.Net.HttpStatusCode.Found 指示所需的信息位于的位置标头中指定的 URI。 当收到此状态时的默认操作是遵循与响应关联的位置标头。
        //     当原始请求方法是 POST 时，重定向的请求将使用 GET 方法。

        /// <summary>
        /// Defines the Found
        /// </summary>
        Found = 302,

        //
        // 摘要:
        //     等效于 HTTP 状态 302。System.Net.HttpStatusCode.Redirect 指示所需的信息位于的位置标头中指定的 URI。 当收到此状态时的默认操作是遵循与响应关联的位置标头。
        //     当原始请求方法是 POST 时，重定向的请求将使用 GET 方法。

        /// <summary>
        /// Defines the Redirect
        /// </summary>
        Redirect = 302,

        //
        // 摘要:
        //     等效于 HTTP 状态 303。System.Net.HttpStatusCode.SeeOther 自动将客户端重定向到的位置标头中指定作为公告的结果的
        //     URI。 对指定的位置标头的资源的请求将会执行与 GET。

        /// <summary>
        /// Defines the SeeOther
        /// </summary>
        SeeOther = 303,

        //
        // 摘要:
        //     等效于 HTTP 状态 303。System.Net.HttpStatusCode.RedirectMethod 自动将客户端重定向到的位置标头中指定作为公告的结果的
        //     URI。 对指定的位置标头的资源的请求将会执行与 GET。

        /// <summary>
        /// Defines the RedirectMethod
        /// </summary>
        RedirectMethod = 303,

        //
        // 摘要:
        //     等效于 HTTP 状态 304。System.Net.HttpStatusCode.NotModified 指示客户端的缓存的副本是最新。 不会传输资源的内容。

        /// <summary>
        /// Defines the NotModified
        /// </summary>
        NotModified = 304,

        //
        // 摘要:
        //     等效于 HTTP 状态 305。System.Net.HttpStatusCode.UseProxy 指示该请求应使用的位置标头中指定的 uri 的代理服务器。

        /// <summary>
        /// Defines the UseProxy
        /// </summary>
        UseProxy = 305,

        //
        // 摘要:
        //     等效于 HTTP 状态 306。System.Net.HttpStatusCode.Unused 是对未完全指定的 HTTP/1.1 规范建议的扩展。

        /// <summary>
        /// Defines the Unused
        /// </summary>
        Unused = 306,

        //
        // 摘要:
        //     等效于 HTTP 状态 307。System.Net.HttpStatusCode.TemporaryRedirect 指示请求信息位于的位置标头中指定的
        //     URI。 当收到此状态时的默认操作是遵循与响应关联的位置标头。 当原始请求方法是 POST 时，重定向的请求还将使用 POST 方法。

        /// <summary>
        /// Defines the TemporaryRedirect
        /// </summary>
        TemporaryRedirect = 307,

        //
        // 摘要:
        //     等效于 HTTP 状态 307。System.Net.HttpStatusCode.RedirectKeepVerb 指示请求信息位于的位置标头中指定的
        //     URI。 当收到此状态时的默认操作是遵循与响应关联的位置标头。 当原始请求方法是 POST 时，重定向的请求还将使用 POST 方法。

        /// <summary>
        /// Defines the RedirectKeepVerb
        /// </summary>
        RedirectKeepVerb = 307,

        PermanentRedirect = 308,
        //
        // 摘要:
        //     等效于 HTTP 状态 400。System.Net.HttpStatusCode.BadRequest 指示无法由服务器理解此请求。System.Net.HttpStatusCode.BadRequest
        //     如果没有其他错误适用，或者如果具体的错误是未知的或不具有其自己的错误代码发送。

        /// <summary>
        /// Defines the BadRequest
        /// </summary>
        BadRequest = 400,

        //
        // 摘要:
        //     等效于 HTTP 状态 401。System.Net.HttpStatusCode.Unauthorized 指示所请求的资源需要身份验证。 Www-authenticate
        //     标头包含如何执行身份验证的详细信息。

        /// <summary>
        /// Defines the Unauthorized
        /// </summary>
        Unauthorized = 401,

        //
        // 摘要:
        //     等效于 HTTP 状态 402。System.Net.HttpStatusCode.PaymentRequired 已保留供将来使用。

        /// <summary>
        /// Defines the PaymentRequired
        /// </summary>
        PaymentRequired = 402,

        //
        // 摘要:
        //     等效于 HTTP 状态 403。System.Net.HttpStatusCode.Forbidden 指示服务器拒绝无法完成请求。

        /// <summary>
        /// Defines the Forbidden
        /// </summary>
        Forbidden = 403,

        //
        // 摘要:
        //     等效于 HTTP 状态 404。System.Net.HttpStatusCode.NotFound 指示所请求的资源不存在的服务器上。

        /// <summary>
        /// Defines the NotFound
        /// </summary>
        NotFound = 404,

        //
        // 摘要:
        //     等效于 HTTP 状态 405。System.Net.HttpStatusCode.MethodNotAllowed 指示请求方法 （POST 或 GET）
        //     不允许对所请求的资源。

        /// <summary>
        /// Defines the MethodNotAllowed
        /// </summary>
        MethodNotAllowed = 405,

        //
        // 摘要:
        //     等效于 HTTP 状态 406。System.Net.HttpStatusCode.NotAcceptable 表示客户端已指定使用 Accept 标头，它将不接受任何可用的资源表示。

        /// <summary>
        /// Defines the NotAcceptable
        /// </summary>
        NotAcceptable = 406,

        //
        // 摘要:
        //     等效于 HTTP 状态 407。System.Net.HttpStatusCode.ProxyAuthenticationRequired 指示请求的代理要求身份验证。
        //     代理服务器进行身份验证标头包含如何执行身份验证的详细信息。

        /// <summary>
        /// Defines the ProxyAuthenticationRequired
        /// </summary>
        ProxyAuthenticationRequired = 407,

        //
        // 摘要:
        //     等效于 HTTP 状态 408。System.Net.HttpStatusCode.RequestTimeout 指示客户端的服务器预期请求的时间内没有未发送请求。

        /// <summary>
        /// Defines the RequestTimeout
        /// </summary>
        RequestTimeout = 408,

        //
        // 摘要:
        //     等效于 HTTP 状态 409。System.Net.HttpStatusCode.Conflict 指示该请求可能不会执行由于在服务器上发生冲突。

        /// <summary>
        /// Defines the Conflict
        /// </summary>
        Conflict = 409,

        //
        // 摘要:
        //     等效于 HTTP 状态 410。System.Net.HttpStatusCode.Gone 指示所请求的资源不再可用。

        /// <summary>
        /// Defines the Gone
        /// </summary>
        Gone = 410,

        //
        // 摘要:
        //     等效于 HTTP 状态 411。System.Net.HttpStatusCode.LengthRequired 指示缺少必需的内容长度标头。

        /// <summary>
        /// Defines the LengthRequired
        /// </summary>
        LengthRequired = 411,

        //
        // 摘要:
        //     等效于 HTTP 状态 412。System.Net.HttpStatusCode.PreconditionFailed 表示失败，此请求的设置的条件，无法执行请求。
        //     使用条件请求标头，如果匹配项，如设置条件无-If-match，或如果-修改-自从。

        /// <summary>
        /// Defines the PreconditionFailed
        /// </summary>
        PreconditionFailed = 412,

        //
        // 摘要:
        //     等效于 HTTP 状态 413。System.Net.HttpStatusCode.RequestEntityTooLarge 指示请求来说太大的服务器能够处理。

        /// <summary>
        /// Defines the RequestEntityTooLarge
        /// </summary>
        RequestEntityTooLarge = 413,

        //
        // 摘要:
        //     等效于 HTTP 状态 414。System.Net.HttpStatusCode.RequestUriTooLong 指示 URI 太长。

        /// <summary>
        /// Defines the RequestUriTooLong
        /// </summary>
        RequestUriTooLong = 414,

        //
        // 摘要:
        //     等效于 HTTP 状态 415。System.Net.HttpStatusCode.UnsupportedMediaType 指示该请求是不受支持的类型。

        /// <summary>
        /// Defines the UnsupportedMediaType
        /// </summary>
        UnsupportedMediaType = 415,

        //
        // 摘要:
        //     等效于 HTTP 416 状态。System.Net.HttpStatusCode.RequestedRangeNotSatisfiable 指示从资源请求的数据范围不能返回，或者因为范围的开始处，然后该资源的开头或范围的末尾后在资源的结尾。

        /// <summary>
        /// Defines the RequestedRangeNotSatisfiable
        /// </summary>
        RequestedRangeNotSatisfiable = 416,

        //
        // 摘要:
        //     等效于 HTTP 状态 417。System.Net.HttpStatusCode.ExpectationFailed 指示无法由服务器满足 Expect
        //     标头中给定。

        /// <summary>
        /// Defines the ExpectationFailed
        /// </summary>
        ExpectationFailed = 417,

        ImATeapot = 418,

        MethodFailure = 420,

        EnhanceYourCalm = 420,

        MisdirectedRequest = 421,

        UnprocessableEntity = 422,

        Locked = 423,

        FailedDependency = 424,
        //
        // 摘要:
        //     等效于 HTTP 状态 426。System.Net.HttpStatusCode.UpgradeRequired 指示客户端应切换到不同的协议，例如 TLS/1.0。

        /// <summary>
        /// Defines the UpgradeRequired
        /// </summary>
        UpgradeRequired = 426,

        PreconditionRequired = 428,

        TooManyRequests = 429,

        RequestHeaderFieldsTooLarge = 431,

        LoginTimeOut = 440,

        NoResponse = 444,

        RetryWith = 449,

        BlockedByWindowsParentalControls = 450,

        UnavailableForLegalReasons = 451,

        ExchangeActiveSyncRedirect = 451,

        RequestHeaderTooLarge = 494,

        SSLCertificateError = 495,

        SSLCertificateRequired = 496,

        HTTPRequestSentToHTTPSPort = 497,

        InvalidToken = 498,

        TokenRequired = 499,

        ClientClosedRequest = 499,
        //
        // 摘要:
        //     等效于 HTTP 状态 500。System.Net.HttpStatusCode.InternalServerError 表示在服务器上发生一般性错误。

        /// <summary>
        /// Defines the InternalServerError
        /// </summary>
        InternalServerError = 500,

        //
        // 摘要:
        //     等效于 HTTP 状态 501。System.Net.HttpStatusCode.NotImplemented 指示服务器不支持所请求的功能。

        /// <summary>
        /// Defines the NotImplemented
        /// </summary>
        NotImplemented = 501,

        //
        // 摘要:
        //     等效于 HTTP 状态 502。System.Net.HttpStatusCode.BadGateway 指示中间代理服务器从另一个代理或原始服务器接收到错误响应。

        /// <summary>
        /// Defines the BadGateway
        /// </summary>
        BadGateway = 502,

        //
        // 摘要:
        //     等效于 HTTP 状态 503。System.Net.HttpStatusCode.ServiceUnavailable 指示将服务器暂时不可用，通常是由于高负载或维护。

        /// <summary>
        /// Defines the ServiceUnavailable
        /// </summary>
        ServiceUnavailable = 503,

        //
        // 摘要:
        //     等效于 HTTP 状态 504。System.Net.HttpStatusCode.GatewayTimeout 指示中间代理服务器在等待来自另一个代理或原始服务器的响应时已超时。

        /// <summary>
        /// Defines the GatewayTimeout
        /// </summary>
        GatewayTimeout = 504,

        //
        // 摘要:
        //     等效于 HTTP 状态 505。System.Net.HttpStatusCode.HttpVersionNotSupported 指示服务器不支持请求的
        //     HTTP 版本。

        /// <summary>
        /// Defines the HttpVersionNotSupported
        /// </summary>
        HttpVersionNotSupported = 505,

        VariantAlsoNegotiates = 506,

        InsufficientStorage = 507,

        LoopDetected = 508,

        BandwidthLimitExceeded = 509,

        NotExtended = 510,

        NetworkAuthenticationRequired = 511,

        UnknownError = 520,

        WebServerIsDown = 521,

        ConnectionTimedOut = 522,

        OriginIsUnreachable = 523,

        ATimeoutOccurred = 524,

        SSLHandshakeFailed = 525,

        InvalidSSLCertificate = 526,

        RailgunError = 527,

        OriginDNSError = 528,

        SiteIsFrozen = 530,

        NetworkReadTimeoutError = 598,

        #endregion HTTP标准状态,IIS扩展状态,Nginx扩展状态等

        #region 自定义状态

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 601,

        /// <summary>
        /// 未登录
        /// </summary>
        NotLogin = 602,

        /// <summary>
        /// 需要登录
        /// </summary>
        NeedLogin = 603,

        ParamEmpty = 604,

        #endregion 自定义状态

        #region 插旗

        Flag1 = 701,

        Flag2 = 702,

        Flag3 = 703,

        Flag4 = 704,

        Flag5 = 705,

        Flag6 = 706,

        Flag7 = 707,

        Flag8 = 708,

        Flag9 = 709,

        #endregion 插旗
    }

    #endregion Enums
}