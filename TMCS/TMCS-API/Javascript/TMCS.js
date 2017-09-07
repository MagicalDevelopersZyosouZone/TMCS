(function()
{
    /**
     * @class
     * @param {string} [address] - The address of a TMCS server.
     */
    function TMCS(address)
    {
        this.address = "";
        this.uid = "";
        this.user = null;
        this.websocket = new WebSocket();
        this.connected = false;
        this.status = TMCS.Status.Disconnected;
        
        var tmcs = this;
        Object.defineProperty(this, "connected", {
            get: function ()
            {
                return (tmcs.websocket.readyState == 1);
            }

        })
        if (address)
            this.address = address;
    }
    
    /**
    * Enum for TMCS status.
    * @readonly
    * @public
    * @enum {number}
    */
    TMCS.Status = { Disconnected: 0, Connecting: 1, HandShaking: 2, Running: 3, Closing: 4 };

    /**
     * The version of the TMCS client.
     * @public
     * @static
     */
    TMCS.version = "0.1.0";

    /**
     * Connect to the TMCS server.
     * @public
     * @param {string} [address] - The address of a TMCS server
     */
    TMCS.prototype.connect = function (address, callback)
    {
        
        if (url)
            this.url = url;
        if (!this.url)
            throw new Error("URL required.");
        var tmcs = this;
        this.websocket.onopen = function (e)
        {
            tmcs.status = TMCS.Status.HandShaking();

        }
        this.status = TMCS.Status.Connecting;
        this.websocket = new WebSocket(this.url);

    };

    /**
     * @private Shake hand with the TMCS server.
     */
    TMCS.prototype.handshake = function (callback)
    {

    };

    

    /**
    * Login with password or private key.
    * @function
    * @param {string} uid - The uid of the user.
    * @param {string} key - The password or privateKey of the user.
    * @param {resultCallback} [callback] - The callback that handles the result.
    * @return undefined
    */
    TMCS.prototype.login = function (uid, key, callback)
    {

    };

    /**
    * Register a user.
    * @param {string} uid - The uid of the user.
    * @param {string} pubKey - The public key.
    * @param {resultCallback} [callback] - The callback that handles the result.
    */
    TMCS.prototype.register = function (uid, pubKey, callback)
    {

    };

    /**
     * Set the infomation of the user.
     * @param {string} key - The name of the info.
     * @param {string|number} value - The value of the info.
     * @param {resultCallback} [callback] - The callback that handles the result.
     */
    TMCS.prototype.setInfo = function (key, value, callback)
    {

    };

    /**
     * Get the infomation of the user.
     * @param {string} key - The name of the info.
     * @param {resultCallback} [callback] - The callback that handles the result.
     * @param {string} [uid] - The name of the user.
     */
    TMCS.prototype.getInfo = function (key, callback, uid)
    {
    };

    /**
     * Get the friends list of the user.
     * @param {resultCallback} [callback] - The callback that handles the result.
     */
    TMCS.prototype.getFriends = function (callback)
    {

    };

    /**
     * Add a user as friend.
     * @param {string} uid - The uid of the friend.
     * @param {resultCallback} [callback] - The callback that handles the result.
     */
    TMCS.prototype.addFriend = function (uid, callback)
    {

    };

    /**
     * Remove a friend.
     * @param {string} uid - The uid of the user to be removed.
     * @param {resultCallback} [callback] - The callback that handles the result.
     */
    TMCS.prototype.removeFriend = function (uid, callback)
    {

    };


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
            for (var i = this.length-1; i >=index; i--)
            {
                this[i + 1] = this[i];
            }
            this[index] = obj;
        }
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
        }
        list.remove = function (obj)
        {
            for (var i = 0; i < list.length; i++)
            {
                if (list[i] == obj)
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
        }
        list.clear = function ()
        {
            list.length = 0;
        }
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
        }
        list.contain = function (obj)
        {
            return (list.indexOf(obj) >= 0);
        }
        return list;
    }

    
    var Event=(function()
    {
        /**
         * The Event Class.
         * @class
         */
        function Event()
        {
            this.def=null;
            this.handlers=ArrayList();
        }
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

    /**
     * The callback that handles the result
     * @callback resultCallback
     * @param {string} result - The result.
     * @return undefined
     */
})();