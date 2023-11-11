using System.Collections.Generic;

namespace WebExpress.Core.WebTask
{
    /// <summary>
    /// Directory with the current tasks.
    /// Key = The task id.
    /// Value = The task.
    /// </summary>
    internal class TaskDictionary : Dictionary<string, Task>
    {
    }
}
