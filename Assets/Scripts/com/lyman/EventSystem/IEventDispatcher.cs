using System;
using System.Text;

public delegate void EventHandler(IEvent evt);
abstract public class IEventDispatcher
{
    public abstract bool registerEvent(string ID, Type eventType);
    public abstract void unregisterEvent(string ID);
    public abstract bool bindHandler(string ID, EventHandler handler);
    public abstract void unbindHandler(string ID, EventHandler handler);
    public abstract void post(IEvent evt);
    public abstract void trigger(IEvent evt);
    public abstract void dispatch();
    #region "DEBUG"
#if DEBUG
    public abstract void dumpAllEvents();
    public abstract void dumpAllHandles();
#endif
    #endregion "DEBUG"
}