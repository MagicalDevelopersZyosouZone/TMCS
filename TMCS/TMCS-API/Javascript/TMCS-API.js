(function ()
{
    if (!window.JSEncrypt)
    {
        loadLib("/Javascript/lib/jsencrypt/jsencrypt.min.js");
        var encrypt = new JSEncrypt({ default_key_size: 1024 });
        encrypt.getKey(function ()
        {
            document.write(encrypt.getPublicKey() + "<br>" + encrypt.getPrivateKey());
        });
    }

    function loadLib(url)
    {
        var request = new XMLHttpRequest();
        request.open("GET", url, false);
        request.send();
        if (request.readyState != 4)
            throw new Error("Network error.");
        try
        {
            eval.call(window, request.responseText);
        }
        catch (ex)
        {
            throw ex;
        }
    }
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
            if (!this.head)
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
        };
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
        };
    }

    //ArrayList
    function ArrayList()
    {
        var list=[];
        list.add = function (obj)
        {
            list[list.length] = obj;
            return list.length - 1;
        };
        list.insert = function (obj, index)
        {
            if (isNaN(index) || index < 0)
            {
                throw new Error("Invalid index.");
            }
            for (var i = this.length - 1; i >= index; i--)
            {
                this[i + 1] = this[i];
            }
            this[index] = obj;
        };
        list.removeAt = function (index)
        {
            if (isNaN(index) || index < 0 || index >= list.length)
            {
                throw new Error("Invalid index.");
            }
            for (var i = index; i < list.length - 1; i++)
            {
                list[i] = list[i + 1];
            }
            list.length -= 1;
        };
        list.remove = function (obj)
        {
            for (var i = 0; i < list.length; i++)
            {
                if (list[i] === obj)
                {
                    for (; i < list.length - 1; i++)
                    {
                        list[i] = list[i + 1];
                    }
                    list.length -= 1;
                    return;
                }
            }
            throw new Error("Object not found.");
        };
        list.clear = function ()
        {
            list.length = 0;
        };
        list.addRange = function (arr, startIndex, count)
        {
            if (!startIndex || isNaN(startIndex))
                startIndex = 0;
            if (!count || isNaN(count))
                count = arr.length;
            for (var i = startIndex; i < count; i++)
            {
                list[list.length] = arr[i];
            }
        };
        list.contain = function (obj)
        {
            return list.indexOf(obj) >= 0;
        };
        return list;
    }

    var Event=(function()
    {
        function Event()
        {
            this.def=null;
            this.handlers=ArrayList();
        }
        Event.version = 0.1;
        Event.prototype.invoke = function (args)
        {
            if (!args["handled"])
                args.handled = false;
            if (this.def)
                this.def(args);
            for (var i = 0; i < this.handlers.length; i++)
            {
                if (args.handled)
                    return;
                if (this.handlers[i])
                    this.handlers[i](args);
            }
        };
        Event.prototype.add = function (handler)
        {

            this.handlers.add(handler);
        };
        Event.prototype.remove = function (handler)
        {
            if (this.def === handler)
                this.def = null;
            this.handlers.remove(handler);
        };
        
        function EventManager()
        {
            this.events={};
            this.eventNames=ArrayList();
        }
        EventManager.prototype.register = function (name, event)
        {
            if (name === undefined || name === null)
                throw new Error("A name of the event required.");
            if (this.eventNames.indexOf(name) > 0)
                throw new Error("Event existed.");
            this.events[name] = event;
            this.eventNames.add(name);
        };
        Event.EventManager=EventManager;
        
        function defineEvent(obj,name,handler)
        {
            if(!obj)
                throw new Error("An object required.");
            if(name===undefined || name===null)
                throw new Error("A name of the event required.");
            if(!obj.eventManager)
            {
                obj.eventManager=new EventManager();
                
            }
            
            if(obj.eventManager.eventNames.contain(name))
                throw new Error("Event existed.");
            var event=new Event();
            obj.eventManager.register(name);
            Object.defineProperty(obj, name, {
                get: function ()
                {
                    return event;
                },
                set: function (handler)
                {
                    event.def = handler;
                }
            });
        }
        Event.defineEvent=defineEvent;
        return Event;
    })();

    function Request(rid, api, params, callback)
    {
        this.rid = rid;
        this.api = api;
        this.params = params;
        this.callback = callback;
    }

    function TMCS()
    {
        this.websocket = new WebSocket();
        this.requests = ArrayList();
        Event.defineEvent(this, "onMessage");

        var self=this;
        Object.defineProperty(this, "connected", {
            get: function ()
            {
                if (self.websocket.readyState === WebSocket.OPEN)
                    return true;
                return false;
            }
        });
    }
    TMCS.prototype.messageCallback = function (e)
    {
        var data = JSON.parse(e.data);
        if (data.rid < 0)
        {
            var msgList = data.data;
            this.onMessage.invoke(msgList);
        }
        else
        {
            for (var i = 0; i < this.requests.length; i++)
            {
                if (this.requests[i].rid === data.rid && this.requests[i].callback)
                    this.requests[i].callback(data.data);
            }
        }
    };
    TMCS.prototype.errorCallback = function (e)
    {

    };
    TMCS.prototype.connect = function (url, protocols)
    {
        if (this.websocket && this.connected)
        {
            this.websocket.close();
        }
        this.websocket = new WebSocket(url, protocols);
        var self = this;
        this.websocket.onopen = function ()
        {
            self.websocket.onmessage = self.messageCallback;
            this.websocket.onerror = this.errorCallback;
        };
    };
    var ridAcmlt = 0;
    TMCS.prototype.apiCall = function (apiName, params, callback)
    {
        if (!this.connected)
        {
            throw new Error("Disconnected.");
        }

        var request = { type: "Request", rid: ridAcmlt++, api: apiName, params: params };
        var requireJSON = JSON.stringify(request);
        this.requests.add(new Request(request.rid, apiName, params, callback));
        this.websocket.send(requireJSON);
    };
    TMCS.prototype.login = function (uid, token, callback)
    {
        this.apiCall("Login", { uid: uid, token: token }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode === 0)
                callback({ errCode: result.errCode, msg: result.msg });
            else
                callback({ errCode: result.errCode, msg: result.msg });
        });
    };
    TMCS.prototype.logout = function (callback)
    {
        this.apiCall("Logout", {}, function (result)
        {
            if (!callback)
                return;
            if (result.errCode === 0)
                callback({ errCode: result.errCode, msg: result.msg });
            else
                callback({ errCode: result.errCode, msg: result.msg });
        });
    };
    TMCS.prototype.getFriends = function (callback)
    {
        this.apiCall("GetFriends", {}, function (result)
        {
            if (!callback)
                return;
            if (result.errCode === 0)
                callback({ errCode: result.errCode, msg: result.msg, friends: result.friends });
            else
                callback({ errCode: result.errCode, msg: result.msg, friends: [] });
        });
    };
    TMCS.prototype.addFriend = function (uid, callback)
    {
        this.apiCall("AddFriend", { targetUid: uid }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode === 0)
                callback({ errCode: result.errCode, msg: result.msg });
            else
                callback({ errCode: result.errCode, msg: result.msg });
        });
    };
    TMCS.prototype.searchUsers = function (name, callback)
    {
        this.apiCall("SearchUsers", { targetName: name }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode === 0)
                callback({ errCode: result.errCode, msg: result.msg, users: result.users });
            else
                callback({ errCode: result.errCode, msg: result.msg, users: [] });
        });
    };
    TMCS.prototype.getMessages = function (targetUid, timestamp, count, callback)
    {
        this.apiCall("GetMessages", { targetUid: targetUid, timestamp: timestamp, count: count }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode === 0)
                callback({ errCode: result.errCode, msg: result.msg, messages: result.messages });
            else
                callback({ errCode: result.errCode, msg: result.msg, messages: [] });
        });
    };
    TMCS.prototype.sendMessages = function (targetUid, messages, callback)
    {
        this.apiCall("SendMessages", { targetUid: targetUid, messages: messages }, function (result)
        {
            if (!callback)
                return;
            if (result.errCode === 0)
                callback({ errCode: result.errCode, msg: result.msg });
            else
                callback({ errCode: result.errCode, msg: result.msg });
        });
    };
})();