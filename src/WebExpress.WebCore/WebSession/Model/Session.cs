using System;
using System.Collections.Generic;

namespace WebExpress.WebCore.WebSession.Model
{
    /// <summary>
    /// Represents a session.Through a session, session data can be assigned to
    /// a user. Session data is stored on the server side, turning the stateless 
    /// http protocol into a state-based one.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Returns the session id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Returns the creation time.
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Returns or sets the time of the last access.
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Returns or sets properties for the session.
        /// </summary>
        public Dictionary<Type, ISessionProperty> Properties { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Session()
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="id">The session id.</param>
        public Session(Guid id)
        {
            Id = id;
            Created = DateTime.Now;
            Updated = DateTime.Now;

            Properties = new Dictionary<Type, ISessionProperty>();
        }

        /// <summary>
        /// Returns a session property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <returns>The property or null.</returns>
        public T GetProperty<T>() where T : class, ISessionProperty, new()
        {
            lock (Properties)
            {
                if (Properties.ContainsKey(typeof(T)))
                {
                    return Properties[typeof(T)] as T;
                }
            }

            return default;
        }

        /// <summary>
        /// Returns a property if it already exists. Otherwise, a new property will be created.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <returns>The property or null.</returns>
        public T GetOrCreateProperty<T>() where T : class, ISessionProperty, new()
        {
            lock (Properties)
            {
                if (Properties.ContainsKey(typeof(T)))
                {
                    return Properties[typeof(T)] as T;
                }

                var property = new T();
                SetProperty(property);

                return property;
            }
        }

        /// <summary>
        /// Sets a property.
        /// </summary>
        /// <param name="property">The property to set.</param>
        public void SetProperty(ISessionProperty property)
        {
            lock (Properties)
            {
                if (!Properties.ContainsKey(property.GetType()))
                {
                    Properties.Add(property.GetType(), property);
                }

                Properties[property.GetType()] = property;
            }
        }

        /// <summary>
        /// Removes a property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        public void RemoveProperty<T>() where T : class, ISessionProperty, new()
        {
            lock (Properties)
            {
                if (Properties.ContainsKey(typeof(T)))
                {
                    Properties.Remove(typeof(T));
                }
            }
        }

    }
}
