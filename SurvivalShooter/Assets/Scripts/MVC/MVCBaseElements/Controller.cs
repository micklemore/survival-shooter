namespace SmartMVC
{
    /// <summary>
    /// Base class for all Controllers in the application.
    /// A Controller's purpose is to act as bridge between its view and model, 
    /// reacting on events and performing operations on either side
    /// </summary>
    public class Controller : Element { }

    /// <summary>
    /// Base class for all Controller related classes.
    /// </summary>
    public class Controller<T> : Controller where T : BaseApplication
    {
        /// <summary>
        /// Returns app as a custom 'T' type.
        /// </summary>
        new public T app { get { return (T)base.app; } }
    }
}