namespace SmartMVC
{
    /// <summary>
    /// Base class for all View related classes.
    /// A View's purpose is to display data and objects (tipically contained in the model)
    /// </summary>
    public class View : Element { }

    /// <summary>
    /// Base class for all View related classes.
    /// </summary>
    public class View<T> : View where T : BaseApplication
    {
        /// <summary>
        /// Returns app as a custom 'T' type.
        /// </summary>
        new public T app { get { return (T)base.app; } }
    }
}