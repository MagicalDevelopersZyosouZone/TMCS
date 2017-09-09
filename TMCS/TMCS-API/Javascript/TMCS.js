window.TMCS = (function ()
{
    /**
     * @class
     * @param {string} [address] - The address of a TMCS server.
     * @param {bool} [useSsl=false] - Connect with SSL.
     */
    function TMCS(address,useSsl)
    {
        this.address = "";
        this.uid = "";
        this.user = null;
        this.ssl = false;
        this.websocket = null;//new WebSocket();
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
        if (useSsl)
            this.ssl = true;
    }

    /**
     * The result of an api calling.
     * @class
     * @param {number} code - The error code of the result;
     * @param {object} data - The data of the result;
     */
    function APIResult(code, data)
    {
        this.code = code;
        this.data = data;
    }

    /**
     * A message to be sent.
     * @class
     * @param {object} sender - The sender.
     * @param {string} receiver - The receiver.
     * @param {object} data - The message.
     */
    function Message(sender, receiver, data)
    {
        this.sender = sender;
        this.receiver = receiver;
        this.data = data;
    }
    TMCS.Message = Message;
    
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
    TMCS.prototype.connect = function (address, useSsl, callback)
    {
        
        if (url)
            this.url = url;
        if (!this.url)
            throw new Error("URL required.");
        if (useSsl)
            this.ssl = true;
        var tmcs = this;
        this.websocket.onopen = function (e)
        {
            tmcs.status = TMCS.Status.HandShaking();

        }
        this.status = TMCS.Status.Connecting;
        if (this.ssl)
        {
            this.websocket = new WebSocket("wss://" + address + "/TMCS");
        }
        else
        {
            this.websocket = new WebSocket("ws://" + address + "/TMCS");
        }

    };

    /**
     * @param {string} url - The URL of the API.
     * @param {object} params - The parameters.
     * @param {string} method - The method of the http request.
     * @param {responseCallback} [callback] - The callback that handles the result.
     */ 
    TMCS.prototype.callAPI = function (url, method, params, callback)
    {
        if (!this.address)
            throw new Error("The address of the TMCS server is required.");
        var request = new XMLHttpRequest();
        if (this.ssl) {
            request.open(method.toUpperCase(), "https://" + this.address + "/api" + url);
        }
        else {
            request.open(method.toUpperCase(), "http://" + this.address + "/api" + url);
        }
        if (method.toUpperCase() === "PUT" || method.toUpperCase() === "POST")
            request.setRequestHeader("Content-Type", "application/json");
        request.onreadystatechange = function (e)
        {
            if (request.readyState != 4)
                return;
            if (!callback)
                return;
            var code = 0;
            var data = null;
            if (request.status != 200)
            {
                code = request.status;
                data = request.statusText;
            }
            else
            {
                try {
                    var result = JSON.parse(request.responseText);
                    code = result.code;
                    data = result.data;
                }
                catch (ex) {
                    code = -110;
                    data = "HTTP Response error."
                }
            }
            callback(new APIResult(code, data));
        }
        request.send(JSON.stringify(params));

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
        var tmcs = this;
        this.getLoginMethod(uid, function (result)
        {
            if (result.code != 0)
            {
                if (callback)
                    callback(result);
                return;
            }
            //Auth by password.
            if (result.data.authType == "Password") 
            {
                tmcs.callAPI(
                    "/login/password",
                    "POST",
                    {
                        uid: uid,
                        hash: pidCrypt.SHA512(key)
                    },
                    function (result)
                    {
                        if (result != 0) 
                        {
                            switch (result.code)
                            {
                                case -202:
                                    result.data = "User dose not exist.";
                                    break;
                                case -201:
                                    result.data = "Password incorrect.";
                                    break;
                            }
                        }
                        if (callback)
                            callback(result);
                    });
            }
            //Auth by private key.
            else if (result.data.authType=="PrivateKey")
            {
                var jsEnc = new JSEncrypt({ default_key_size: 1024 });
                jsEnc.getKey();
                var authCode = jsEnc.decrypt(jsEnc.encrypt(result.data.authCode));
                tmcs.callAPI(
                    "/login/key-auth",
                    "POST",
                    {
                        uid: uid,
                        authCode: authCode
                    },
                    function (result)
                    {
                        if (result != 0) {
                            switch (result.code) {
                                case -202:
                                    result.data = "User dose not exist.";
                                    break;
                                case -201:
                                    result.data = "Private key incorrect.";
                                    break;
                            }
                        }
                        if (callback)
                            callback(result);
                    });
            }
        });
    };

    /**
     * Get the login method and data.
     * @param {string} uid - The uid of the user to login.
     * @param {resultCallback} [callback] - The callback that handles the result.
     */
    TMCS.prototype.getLoginMethod = function (uid, callback)
    {
        this.callAPI("/login/" + encodeURIComponent(uid), "GET", null, function (response)
        {
            if (!callback)
                return;

            if(response.code !=0)
            {
                switch(response.code)
                {
                    case -202:
                        response.data="User dose not exist.";
                        break;
                }
            }
            callback(response);
        });
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
     * Get the infom ation of the user.
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

    /**
     * Send some messages to other.
     * @param {Message[]} messages - The array of the messages to be sent.
     * @param {resultCallback} [callback] - The callback that handles the result.
     */
    TMCS.prototype.sendMsg = function (messages, callback)
    {
        
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

    return TMCS;

    /**
     * The callback that handles the result.
     * @callback resultCallback
     * @param {string} result - The result.
     * @return undefined
     */

    /**
     * The callback that handles the HTTP Response.
     * @callback responseCallback
     * @param {XMLHttpRequestResponseType} response - The HTTP Response.
     * @return undefined
     */
})();