(function ()
{
    function Queue()
    {
        function Node(obj)
        {
            this.next = null;
            this.obj = obj;
        }
        this.head = null;
        this.tail = null;
        this.length = 0;
        this.in = function (obj)
        {
            var p = new Node(obj);
            if(this.head==null)
            {
                this.head = p;
                this.tail = p;
            }
            else
            {
                this.tail.next = p;
                this.tail = p;
            }
            this.length++;
        }
        this.out = function (obj)
        {
            if (!this.head)
            {
                return null;
            }
            var p = this.head;
            this.head = this.head.next;
            if (!this.head)
                this.tail = this.head = null;
            this.length--;
            return p.obj;
        }
    }

    var Event=(function()
    {
        function Event()
        {
            this.def=null;
            this.handlers=ArrayList();
        }
        Event.version = 0.1;
        Event.prototype.invoke=function(args)
        {
            if(!args["handled"])
                args.handled=false;
            if(this.def)
                this.def(args);
            for(var i=0;i<this.handlers.length;i++)
            {
                if(args.handled)
                    return;
                if(this.handlers[i])
                    this.handlers[i](args);
            }
        }
        Event.prototype.add=function(handler)
        {
            
            this.handlers.add(handler);
        }
        Event.prototype.remove=function(handler)
        {
            if(this.def==handler)
                this.def=null;
            this.handlers.remove(handler);
        }
        
        function EventManager()
        {
            this.events={};
            this.eventNames=ArrayList();
        }
        EventManager.prototype.register=function(name,event)
        {
            if(name==undefined || name==null)
                throw new Error("A name of the event required.");
            if(this.eventNames.indexOf(name)>0)
                throw new Error("Event existed.");
            this.events[name]=event;
            this.eventNames.add(name);
        }
        Event.EventManager=EventManager;
        
        function defineEvent(obj,name,handler)
        {
            if(!obj)
                throw new Error("An object required.");
            if(name==undefined || name==null)
                throw new Error("A name of the event required.");
            if(!obj.eventManager)
            {
                obj.eventManager=new EventManager();
                
            }
            
            if(obj.eventManager.eventNames.contain(name))
                throw new Error("Event existed.");
            var event=new Event();
            obj.eventManager.register(name);
            Object.defineProperty(obj,name,{
                get:function()
                {
                    return event;
                },
                set:function(handler)
                {
                    event.def=handler;
                }
            })
        }
        Event.defineEvent=defineEvent;
        return Event;
    })();

    function TMCS()
    {
        this.websocket = new WebSocket();
        this.requireQueue = new Queue();
        Event.defineEvent(this, "onMessage");

        var self=this;
        Object.defineProperty(this, "connected", {
            get: function ()
            {
                if (self.websocket.readyState == WebSocket.OPEN)
                    return true;
                return false;
            }
        })
    }
    TMCS.prototype.messageCallback = function (e)
    {
        
    }
    TMCS.prototype.errorCallback = function (e)
    {
        
    }
    TMCS.prototype.connect = function (url, protocols)
    {
        if (this.websocket && this.connected)
        {
            this.websocket.close();
        }
        this.websocket = new WebSocket(url, protocols);
        this.websocket.onopen = function ()
        {
            this.websocket.onmessage = this.messageCallback;
            this.websocket.onerror = this.errorCallback;
        }
    }
    TMCS.prototype.apiCall = function (apiName, params, callback)
    {
        if (!this.connected)
        {
            throw new Error("Disconnected.");
        }
        this.requireQueue.in(callback);

        var require = { api: apiName, params: params };
        var requireJSON = JSON.stringify(require);
        this.websocket.send(requireJSON);
    }
    TMCS.prototype.login = function (uid, token, callback)
    {
        this.apiCall("Login", { uid:uid, token: token }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode == 0)
                callback({ errCode: result.errCode, msg: result.msg });
            else
                callback({ errCode: result.errCode, msg: result.msg });
        });
    }
    TMCS.prototype.logout = function (callback)
    {
        this.apiCall("Logout", {}, function (result)
        {
            if (!callback)
                return;
            if (result.errCode == 0)
                callback({ errCode: result.errCode, msg: result.msg });
            else
                callback({ errCode: result.errCode, msg: result.msg });
        });
    }
    TMCS.prototype.getFriends = function (callback)
    {
        this.apiCall("GetFriends", {}, function (result)
        {
            if (!callback)
                return;
            if (result.errCode == 0)
                callback({ errCode: result.errCode, msg: result.msg, friends: result.friends });
            else
                callback({ errCode: result.errCode, msg: result.msg, friends: [] });
        });
    }
    TMCS.prototype.addFriend = function (uid, callback)
    {
        this.apiCall("AddFriend", { targetUid: uid }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode == 0)
                callback({ errCode: result.errCode, msg: result.msg });
            else
                callback({ errCode: result.errCode, msg: result.msg });
        });
    }
    TMCS.prototype.searchUsers = function (name, callback)
    {
        this.apiCall("SearchUsers", { targetName: name }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode == 0)
                callback({ errCode: result.errCode, msg: result.msg, users: result.users });
            else
                callback({ errCode: result.errCode, msg: result.msg, users: [] });
        });
    }
    TMCS.prototype.getMessages = function (targetUid, timestamp, count, callback)
    {
        this.apiCall("GetMessages", { targetUid: targetUid, timestamp: timestamp, count: count }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode == 0)
                callback({ errCode: result.errCode, msg: result.msg, messages: result.messages });
            else
                callback({ errCode: result.errCode, msg: result.msg, messages: [] });
        });
    }
    TMCS.prototype.sendMessages = function (targetUid, messages, callback)
    {
        this.apiCall("SendMessages", { targetUid: targetUid, messages: messages }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode == 0)
                callback({ errCode: result.errCode, msg: result.msg });
            else
                callback({ errCode: result.errCode, msg: result.msg });
        });
    }
})();