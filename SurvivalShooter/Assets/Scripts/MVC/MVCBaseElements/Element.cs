using UnityEngine;

namespace SmartMVC
{
    /// <summary>
    /// Extension of the element class to handle different BaseApplication types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Element<T> : Element where T : BaseApplication
    {
        /// <summary>
        /// Returns app as a custom 'T' type.
        /// </summary>
        new public T app { get { return (T)base.app; } }
    }

    /// <summary>
    /// Base class for all MVC related classes.
    /// </summary>
    public class Element : MonoBehaviour
    {
        /// <summary>
        /// Reference to the root application of the scene.
        /// </summary>
        public BaseApplication app
        {
            get { return m_app = Find<BaseApplication>(m_app, true); }
        }
        private BaseApplication m_app;

        /// <summary>
        /// Finds a instance of 'T' if 'var' is null. Returns 'var' otherwise.
        /// If 'global' is 'true' searches in all scope, otherwise, searches in childrens.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_var"></param>
        /// <param name="searchGlobally"></param>
        /// <returns></returns>
        public T Find<T>(T p_var, bool searchGlobally = false) where T : Object { return p_var == null ? (searchGlobally ? GameObject.FindObjectOfType<T>() : transform.GetComponentInChildren<T>(true)) : p_var; }
        public T FindInParent<T>(T p_var) where T : Object { return p_var == null ? (transform.GetComponentInParent<T>()) : p_var; }

        /// <summary>
        /// Finds a instance of 'T' locally if 'var' is null. Returns 'var' otherwise.        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_var"></param>
        /// <returns></returns>
        public T FindLocal<T>(T p_var) where T : Object { return p_var == null ? (p_var = GetComponent<T>()) : p_var; }

        /// <summary>
        /// Notifies to the listening controllers the event
        /// </summary>
        /// <param name="eventID">The name of the event to notify</param>
        /// <param name="data">The parameters to pass to the listening controllers</param>
        public void Notify(int eventID, params object[] data)
        {
			app.Notify(eventID, this, data);
        }

        /// <summary>
        /// Attach a method to an event in the dictionary
        /// </summary>
        /// <param name="eventID">The name of the event to add</param>
        /// <param name="callback">The method to a</param>
        public void AddEventListenerToApp(int eventID, BaseApplication.OnNotificationSentHandler callback)
        {
            if (!app) { Debug.Log("app null"); return; }
            app.AddEventListener(eventID, callback);
        }

        /// <summary>
        /// Detach a method from an event in the dictionary
        /// </summary>
        /// <param name="eventID">The name of the event to detach</param>
        /// <param name="callback">The method to detach</param>
        public void RemoveEventListenerFromApp(int eventID, BaseApplication.OnNotificationSentHandler callback)
        {
            if (!app) { return; }
            app.RemoveEventListener(eventID, callback);
        }
    }
}