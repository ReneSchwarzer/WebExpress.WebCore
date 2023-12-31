﻿using System;
using System.Collections.Generic;

namespace WebExpress.WebCore.WebSession
{
    /// <summary>
    /// Internal directory for storing session data.
    /// kex = the session id
    /// value = the session
    /// </summary>
    internal class SessionDictionary : Dictionary<Guid, Session>
    {
    }
}
