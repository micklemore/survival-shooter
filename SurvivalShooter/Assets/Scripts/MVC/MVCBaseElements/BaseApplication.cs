using UnityEngine;
using System.Collections.Generic;

namespace SmartMVC
{
    /// <summary>
    /// Extension of the BaseApplication class to handle different types of Model View Controllers.
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="C"></typeparam>
    public class BaseApplication<M, V, C> : BaseApplication
        where M : Element
        where V : Element
        where C : Element
    {
        /// <summary>
        /// Model reference using the new type.
        /// </summary>
        new public M model { get { return (M)(object)base.model; } }

        /// <summary>
        /// View reference using the new type.
        /// </summary>
        new public V view { get { return (V)(object)base.view; } }

        /// <summary>
        /// Controller reference using the new type.
        /// </summary>
        new public C controller { get { return (C)(object)base.controller; } }
    }

    /// <summary>
    /// Root class for the scene's scripts.
    /// </summary>
    public class BaseApplication : Element
    {
        /// <summary>
        /// The handler of every MVC event
        /// </summary>
        /// <param name="data">Data sent to the method. must be unboxed on the receiver side</param>
        public delegate void OnNotificationSentHandler(params object[] data);

        /// <summary>
        /// Little static init.
        /// </summary>
        public BaseApplication() 
        {
            if (events == null)
            {
                events = new Dictionary<int, SmartMVCEvent>();
            }
        }

        /// <summary>
        /// Fetches the root Model instance.
        /// </summary>
        public Model model { get { return m_model = Find<Model>(m_model); } }
        Model m_model;

        /// <summary>
        /// Fetches the root View instance.
        /// </summary>
        public View view { get { return m_view = Find<View>(m_view); } }
        View m_view;

        /// <summary>
        /// Fetches the root Controller instance.
        /// </summary>
        public Controller controller { get { return m_controller = Find<Controller>(m_controller); } }
        Controller m_controller;

        /// <summary>
        /// All the MVC events managed by the application
        /// </summary>
        static Dictionary<int, SmartMVCEvent> events;

        /// <summary>
        /// Notifies to the listening controllers the event
        /// </summary>
        /// <param name="eventID">The name of the event to notify</param>
        /// <param name="sender">The object that called this method</param>
        /// <param name="data">The parameters to pass to the listening controllers</param>
        public void Notify(int eventID, Object sender, params object[] data)
        {
            if (!events.ContainsKey(eventID))
            {
                return;
            }
            if (events[eventID] == null) { return; }
            events[eventID].CallEvent(data);
        }

        /// <summary>
        /// Attaches a method to an event in the dictionary
        /// </summary>
        /// <param name="eventID">The name of the event to attach</param>
        /// <param name="method">The method to detach</param>
        public void AddEventListener(int eventID, OnNotificationSentHandler method)
        {
            if (events == null)
            {
                events = new Dictionary<int, SmartMVCEvent>();
            }
            if (events.ContainsKey(eventID))
            {
                events[eventID].AddMethod(method);
                return;
            }
            SmartMVCEvent newSmartEvent = new SmartMVCEvent();
            newSmartEvent.OnEventCalled += method;
            events.Add(eventID, newSmartEvent);
        }

        /// <summary>
        /// Detach a method from an event in the dictionary
        /// </summary>
        /// <param name="eventID">The name of the event to detach</param>
        /// <param name="method">The method to detach</param>
        public void RemoveEventListener(int eventID, OnNotificationSentHandler method)
        {
            if (events == null) { return; }
            if (!events.ContainsKey(eventID)) { return; }
            if (events[eventID] != null)
            {
                events[eventID].RemoveMethod(method);
            }
            if (events[eventID].EventIsEmpty)
            {
                events.Remove(eventID);
            }
        }

        protected virtual void Awake()
        {
            if (events != null) { return; }
            events = new Dictionary<int, SmartMVCEvent>();
        }
    }

    /// <summary>
    /// Represents a smart MVC event
    /// </summary>
    public class SmartMVCEvent
    {
        public event BaseApplication.OnNotificationSentHandler OnEventCalled;
        public void AddMethod(BaseApplication.OnNotificationSentHandler method) { OnEventCalled += method; }
        public void RemoveMethod(BaseApplication.OnNotificationSentHandler method) { OnEventCalled -= method; }
        public void CallEvent(params object[] data) { if (!EventIsEmpty) { OnEventCalled(data); } }
        public bool EventIsEmpty { get { return OnEventCalled == null; } }
    }
}
