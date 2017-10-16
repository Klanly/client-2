using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;

 public class EventDispatcher : IEventDispatcher
	{
        private class EventOwner
        {
            public string evtID;
            public Type evtType;
			public event EventHandler evtHandler ;
            public void Trigger(IEvent evt)
            {
				if (evtHandler != null)
				{
					evtHandler (evt);
				}
            }
        }
        private Dictionary<string, EventOwner> registers_;
        private List<IEvent> cur_events_;

        public EventDispatcher()
        {
            registers_ = new Dictionary<string, EventOwner>();
            cur_events_ = new List<IEvent>();
        }

        public override bool registerEvent(string ID, Type eventType)
        {
            if (eventType != null && !eventType.IsSubclassOf(typeof(IEvent)))
            {
                Debug.LogError(string.Format("EventDispatcher.registerEvent Failed, {0} is not a subclass of IEvent", eventType));
                return false;
            }

            EventOwner ew;
            if (registers_.TryGetValue(ID, out ew))
            {
                Debug.LogError(string.Format("EventDispatcher.registerEvent Failed,{0} is allways registered", ID));
                return false;
            }

            ew = new EventOwner();
            ew.evtID = ID;
            ew.evtType = eventType;
            registers_.Add(ID, ew);
           // Debug.LogError(string.Format("EventDispatcher.registerEvent Successful,ID={0},type={1}", ID, eventType));
            return true;
        }

        public override void unregisterEvent(string ID)
        {
            if (registers_.ContainsKey(ID) && registers_.Remove(ID))
            {
                Debug.LogError(string.Format("EventDispatcher.unregisterEvent Successful,ID={0}", ID));
            }
        }
        public override bool bindHandler(string ID, EventHandler handler)
        {
            EventOwner ew;
            if (registers_.TryGetValue(ID, out ew))
            {
                ew.evtHandler += handler;
                Debug.LogError(string.Format("EventDispatcher.bindHandler key={0},handler={1}", ID, handler));
                return true;
            }
            Debug.LogError(string.Format("EventDispatcher.bindHandler Failed--bind a not regisiter event,key={0}", ID));
            return false;
        }
        public override void unbindHandler(string ID, EventHandler handler)
        {
            EventOwner ew;
            if (registers_.TryGetValue(ID, out ew))
            {
                ew.evtHandler -= handler;
                Debug.LogError(string.Format("EventDispatcher.unbindHandler key={0},handler={1}", ID, handler));
            }
        }

        public override void post(IEvent evt)
        {
            cur_events_.Add(evt);
        }

        public override void trigger(IEvent evt)
        {
            EventOwner ew;
            if (registers_.TryGetValue(evt.ID, out ew))
            {
                ew.Trigger(evt);
            }
            else
            {
                Debug.LogError(string.Format("EventDispatcher.postAndDispatch Failed,Try to post a not register event,id={0}", evt.ID));
            }
        }

        public override void dispatch()
        {
            foreach (IEvent evt in cur_events_)
            {
                trigger(evt);
            }
            cur_events_.Clear();
        }

        #region "DEBUG"
#if DEBUG
        public override void dumpAllHandles()
        {
            Debug.LogError(string.Format("----------------------Dump Registered Event-Handlers Begin-----------"));
            foreach (EventOwner ew in registers_.Values)
            {
                Debug.Log(string.Format("EventID={0},Handler={1}", ew.evtID));
            }
            Debug.LogError(string.Format("----------------------Dump Registered Event-Handlers End-------------"));
        }

        public override void dumpAllEvents()
        {
            Debug.LogError(string.Format("----------------------Dump All Current Events Begin-----------"));
            foreach (IEvent e in cur_events_)
            {
                Debug.Log(string.Format("EventID={0},eventType={1}", e.ID, e.GetType()));
            }
            Debug.LogError(string.Format("----------------------Dump All Current Events End-------------"));
        }
#endif
        #endregion "DEBUG"
	}
