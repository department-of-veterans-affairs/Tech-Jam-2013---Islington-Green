/* Copyright (c) 2006-2007 MetaCarta, Inc., published under the BSD license.
 * See http://svn.openlayers.org/trunk/openlayers/release-license.txt 
 * for the full text of the license. */

/**
 * @class
 */
OpenLayers.Popup = OpenLayers.Class.create();

/** Corners object contains image information for image required to create popup corners.
 * I decided to simply call it corners in an attempt to keep the code readable later on.
 * Created to simplify updating of the popup shape, still needs some work to make it truly generic.
 * */
CORNERS = {

    /* top images*/
    TL : {src:'corners/topl.png', sizing: 'scale'},
    TM : {src:'corners/topm.png', sizing: 'crop'},
    TR : {src:'corners/topr.png', sizing: 'scale'},
    
    /* middle images */
    ML : {src:'corners/midl.png', sizing: 'crop'},
    MR : {src:'corners/midr.png', sizing: 'crop'},
    
    /* bottom images */
    BL : {src:'corners/botl.png', sizing: 'scale'},
    BM : {src:'corners/botm.png', sizing: 'crop'},
    BR : {src:'corners/botr.png', sizing: 'scale'}
}

// how often the increment method will be called in milliseconds 1000 = 1 second
OpenLayers.Popup.SlideTimerInterval = 10;

// total duration for slide effect to complete
OpenLayers.Popup.SlideAnimationTime = 2000;

OpenLayers.Popup.prototype = {

    /** @type OpenLayers.Events*/
    events: null,
    
    /** @type String */
    id: "",

    /** @type OpenLayers.LonLat */
    lonlat: null,

    /** @type DOMElement */
    div: null,

    /** @type OpenLayers.Size*/
    size: null,    

    /** @type String */
    contentHTML: "",
    
    /** @type float */
    opacity: "",
    
    /** @type DOMElement */
    contentDiv:null,

    /** @type int */
    padding: 0,

    /** this gets set in Map.js when the popup is added to the map
     * @type OpenLayers.Map */
    map: null,

    /** 
    * @constructor
    * 
    * @param {String} id
    * @param {OpenLayers.LonLat} lonlat
    * @param {OpenLayers.Size} size
    * @param {String} contentHTML
    * @param {Boolean} closeBox
    */
    initialize:function(id, lonlat, size, contentHTML, closeBox)
    {
        if (id == null)
        {
            id = OpenLayers.Util.createUniqueID(this.CLASS_NAME + "_");
        }
        this.size = (size != null) ? size : new OpenLayers.Size(200,200);
        this.id = id;
        this.lonlat = lonlat;
        
        // assign content to object scoped variable
        if (contentHTML != null) this.contentHTML = contentHTML;

        // construct container div (main)
        this.div = OpenLayers.Util.createDiv(this.id, null, null, null, null, null);
        this.div.className = 'olPopup';
       
        // construct content div
        var id = this.div.id + "_contentDiv";
        this.contentDiv = OpenLayers.Util.createDiv(id, null, this.size.clone(), null, "static", null);
        this.contentDiv.className = 'olPopupContent';
       
        // determine actual render dimensions
        this.getRenderedDimensions();
        
        // insert png corners
        this.insertCorners();

        // create close button
        var oBtnClose = document.createElement("A");
            oBtnClose.className="close";
            oBtnClose.style.position="absolute";
            oBtnClose.style.height= 14 + "px";
            oBtnClose.style.width= 14 + "px";
            oBtnClose.title="close popup";
            oBtnClose.href="#";
            this.div.appendChild(oBtnClose);
        
        // register close button event
        var closeEvents = new OpenLayers.Events(this, oBtnClose);
            closeEvents.register("mousedown", this, this.hide);

        // register object events
        this.registerEvents();
        
        // move the popup into position
        this.updatePosition();
    },
    
    /**
     * inserts corners into main container and merges elements. Although this has been constructed in a generic manner, it still 
     * requires more work before it can fully support unlimited shapes. The google drop shadow is achieved by inserting a shadow popup, this could be extended to offer
     * the same functionallity but it would need some pondering to ensure the UI remains responsive.
     */
    insertCorners : function ()
    {   
        // ie hack required for abs positioning  
        var arVersion = navigator.appVersion.split("MSIE");
        var version = parseFloat(arVersion[1]);
    
        //alert("inserting corners: " + this.size);
        // retrive image path 
        var sImgLoc = OpenLayers.Util.getImagesLocation();
               
        // insert top left 0,0
        var topl = OpenLayers.Util.createAlphaImageDiv("", new OpenLayers.Pixel(0,0), new OpenLayers.Size(38, 28), sImgLoc + CORNERS.TL.src, "absolute", 0, CORNERS.TL.sizing);
        this.div.appendChild(topl);
        
        // insert top middle 
        var topm = OpenLayers.Util.createAlphaImageDiv("", new OpenLayers.Pixel(38,0), new OpenLayers.Size(parseInt(this.size.w - 22), 28), sImgLoc + CORNERS.TM.src, "absolute", 0, CORNERS.TM.sizing);
        this.div.appendChild(topm);
        
        // insert top right 
        var topr = OpenLayers.Util.createAlphaImageDiv("", new OpenLayers.Pixel(parseInt(38 + this.size.w - ((version<7) ? 22:23)), 0), new OpenLayers.Size(38, 28), sImgLoc + CORNERS.TR.src, "absolute", 0, CORNERS.TR.sizing);
        this.div.appendChild(topr);
        
        // insert middle left
        var midl = OpenLayers.Util.createAlphaImageDiv("", new OpenLayers.Pixel(0,28), new OpenLayers.Size(38, parseInt(this.size.h - 12)), sImgLoc + CORNERS.ML.src, "absolute", 0, CORNERS.ML.sizing);
        this.div.appendChild(midl);
        
        // insert content
        this.contentDiv.style.position = "absolute";
        this.contentDiv.style.backgroundColor="#ffffff";
        this.contentDiv.style.top = 15 + "px";
        this.contentDiv.style.left = 15 + "px";
        this.contentDiv.style.width = this.size.w + "px";
        this.contentDiv.style.height = this.size.h + "px";
        
        // insert middle right w:38, h:600
        var midr = OpenLayers.Util.createAlphaImageDiv("", new OpenLayers.Pixel(parseInt(38 + this.size.w -22), 28), new OpenLayers.Size(38, parseInt(this.size.h - 13)), sImgLoc + CORNERS.MR.src, "absolute", 0, CORNERS.MR.sizing);
        this.div.appendChild(midr);
        
        // calculate middle width inc content
        var iMaxW = 38 + this.size.w + 38;
        
        // insert bottom left
        var botl = OpenLayers.Util.createAlphaImageDiv("", new OpenLayers.Pixel(0, 28 + this.size.h - 12), new OpenLayers.Size(38, 39), sImgLoc + CORNERS.BL.src, "absolute", 0, CORNERS.BL.sizing);
        this.div.appendChild(botl);
        
        // insert bottom middle
        var botm = OpenLayers.Util.createAlphaImageDiv("", new OpenLayers.Pixel(38, 28 + this.size.h - 13), new OpenLayers.Size(iMaxW - (38 + 97) -((version<7) ? 21:22), 40), sImgLoc + CORNERS.BM.src, "absolute", 0, CORNERS.BM.sizing);
        this.div.appendChild(botm);
        
        // insert bottom right inc handle
        var botr = OpenLayers.Util.createAlphaImageDiv("", new OpenLayers.Pixel((iMaxW - (38 + 97)) + 38 -((version<7) ? 21:22), 28 + this.size.h - 13), new OpenLayers.Size(97, 68), sImgLoc + CORNERS.BR.src, "absolute", 0, CORNERS.BR.sizing);
        this.div.appendChild(botr);

        //this.div.style.border="1px solid red";
        // append content to main popup.
        // inserted last to ensure the browser assigns correct z-index to place content on top
        this.div.appendChild(this.contentDiv);

        // update object size to reflect corners/shadow
        this.size = new OpenLayers.Size(this.size.w + 53, this.size.h + 40);
    },
    
    /**
     * renders the contentHTML offscreen to determine actual dimensions for popup sizing.
     * as we need layout to determine dimensions the content is rendered -99999px to the left and absolute
     * to ensure the scrollbars do not flicker
     */
    getRenderedDimensions : function()
    {
        // ensure content is set to contentDIV
        this.setContentHTML();
        
        // create temp container div with restricted size
        var container = document.createElement("div");
            container.className = 'olPopup';
            container.style.overflow="";
            container.style.position = "absolute";
            container.style.left="-9999999999px";
            container.style.width="20px";
            container.style.height="20px";
            
        // create temp content div and assign content
        var content = document.createElement("div");
            content.innerHTML = this.contentHTML;
            content.className = 'olPopupContent';
            content.style.overflow = "auto";
        
        // add content to restricted container 
        container.appendChild(content);
        
        // append container to body for rendering
        document.body.appendChild(container);
        
        // calculate scroll width of content and add corners and shadow width
        var w = parseInt(content.scrollWidth);// + 49 + 30;
        
        // update container width to allow height to adjust
        container.style.width = w + "px";
        
        // capture height and add shadow and corner image widths
        var h = parseInt(content.scrollHeight);// + 96 + 45;
        
        // update object size values
        this.size = new OpenLayers.Size(w, h);
            
        // remove elements
        document.body.removeChild(container);
        
        // cleanup
        container = null;
        content = null;
    },

    /** 
    */
    destroy: function() {
        if (this.map != null) {
            this.map.removePopup(this);
        }
        this.div = null;
        this.map = null;
    },

    /** 
    * @param {OpenLayers.Pixel} px
    * 
    * @returns Reference to a div that contains the drawn popup
    * @type DOMElement
    */
    draw: function(px)
    {
        if (px == null)
        {
            if ((this.lonlat != null) && (this.map != null)) 
            {
                px = this.map.getLayerPxFromLonLat(this.lonlat);
            }
        }
        
        this.setSize();
        this.setContentHTML();
        this.moveTo(px);

        return this.div;
    },

    /** 
     * if the popup has a lonlat and its map members set, 
     *  then have it move itself to its proper position
     */
    updatePosition: function()
    {
        if ((this.lonlat) && (this.map) && (this.anchor)) 
        {
            var px = this.map.getLayerPxFromLonLat(this.lonlat);
            this.moveTo(px);           
        }
    },

    /**
    * @param {OpenLayers.Pixel} px
    */
    moveTo: function(px) 
    {
        if ((px != null) && (this.div != null)) 
        {
            this.div.style.left = px.x + "px";
            this.div.style.top = px.y + "px";
        }
    },

    /**
     * @returns Boolean indicating whether or not the popup is visible
     * @type Boolean
     */
    visible: function() 
    {
        return OpenLayers.Element.visible(this.div);
    },

    /**
     * 
     */
    toggle: function() 
    {
        OpenLayers.Element.toggle(this.div);
    },

    /**
     *
     */
    show: function() 
    {
        OpenLayers.Element.show(this.div);
    },

    /**
     *
     */
    hide: function()
    {
        if (this.map) this.map.removePopup(this); 
        OpenLayers.Element.hide(this.div);
    },

    /**
    * @param {OpenLayers.Size} size
    */
    setSize:function(size)
    { 
        if (size != undefined)
        {
            this.size = size; 
        }
        
        if (this.div != null)
        {
            this.div.style.width = this.size.w   + "px";
            this.div.style.height = this.size.h  + "px";
        }
    },  
    
    /**
    * @param {float} opacity
    */
    setOpacity:function(opacity) { 
        if (opacity != undefined) {
            this.opacity = opacity; 
        }
        
        if (this.div != null) {
            // for Mozilla and Safari
            this.div.style.opacity = this.opacity;

            // for IE
            this.div.style.filter = 'alpha(opacity=' + this.opacity*100 + ')';
        }
    },    
    
    /**
     * @param {String} contentHTML
     */
    setContentHTML:function(contentHTML) {
        if (contentHTML != null) {
            this.contentHTML = contentHTML;
        }
        
        if (this.contentDiv != null) {
            this.contentDiv.innerHTML = this.contentHTML;
        }    
    },
      
    /** Do this in a separate function so that subclasses can 
     *   choose to override it if they wish to deal differently
     *   with mouse events
     * 
     *   Note in the following handler functions that some special
     *    care is needed to deal correctly with mousing and popups. 
     *   
     *   Because the user might select the zoom-rectangle option and
     *    then drag it over a popup, we need a safe way to allow the
     *    mousemove and mouseup events to pass through the popup when
     *    they are initiated from outside.
     * 
     *   Otherwise, we want to essentially kill the event propagation
     *    for all other events, though we have to do so carefully, 
     *    without disabling basic html functionality, like clicking on 
     *    hyperlinks or drag-selecting text.
     */
     registerEvents:function() {
        this.events = new OpenLayers.Events(this, this.div, null, true);
        this.events.register("mousedown", this, this.onmousedown);
        this.events.register("mousemove", this, this.onmousemove);
        this.events.register("mouseup", this, this.onmouseup);
        this.events.register("click", this, OpenLayers.Util.safeStopPropagation);
        this.events.register("mouseout", this, this.onmouseout);
        this.events.register("dblclick", this, OpenLayers.Util.safeStopPropagation);
     },

    /** When mouse goes down within the popup, make a note of
     *   it locally, and then do not propagate the mousedown 
     *   (but do so safely so that user can select text inside)
     * 
     * @param {Event} evt
     */
    onmousedown: function (evt) {
        this.mousedown = true;
        OpenLayers.Util.safeStopPropagation(evt);
    },

    /** If the drag was started within the popup, then 
     *   do not propagate the mousemove (but do so safely
     *   so that user can select text inside)
     * 
     * @param {Event} evt
     */
    onmousemove: function (evt) {
        if (this.mousedown) {
            OpenLayers.Util.safeStopPropagation(evt);
        }
    },

    /** When mouse comes up within the popup, after going down 
     *   in it, reset the flag, and then (once again) do not 
     *   propagate the event, but do so safely so that user can 
     *   select text inside
     * 
     * @param {Event} evt
     */
    onmouseup: function (evt) {
        if (this.mousedown) {
            this.mousedown = false;
            OpenLayers.Util.safeStopPropagation(evt);
        }
    },

    /** When mouse goes out of the popup set the flag to false so that
     *   if they let go and then drag back in, we won't be confused.
     * 
     * @param {Event} evt
     * 
     * @type Boolean
     */
    onmouseout: function (evt) {
        this.mousedown = false;
    },
    
    CLASS_NAME: "OpenLayers.Popup"
};


/**********************************************************************************************/

/* Copyright (c) 2006-2007 MetaCarta, Inc., published under the BSD license.
 * See http://svn.openlayers.org/trunk/openlayers/release-license.txt 
 * for the full text of the license. */


/**
 * @class
 * 
 * @requires OpenLayers/Popup.js
 */
OpenLayers.Popup.Anchored = OpenLayers.Class.create();
OpenLayers.Popup.Anchored.prototype = OpenLayers.Class.inherit( OpenLayers.Popup, {

    /** "lr", "ll", "tr", "tl" - relative position of the popup.
     * @type String */
    relativePosition: "tl",

    /** Object which must have expose a 'size' (OpenLayers.Size) and 
     *                                 'offset' (OpenLayers.Pixel) 
     * @type Object */
    anchor: null,
    
    /** number of X pixels required to pan the map for the popup to be visible
     * @type Unsigned Integer */
    differenceX : 0,
    
    /** number of Y pixels required to pan the map for the popup to be visible
     * @type Unsigned Integer */
    differenceY : 0,

    /** 
    * @constructor
    * 
    * @param {String} id
    * @param {OpenLayers.LonLat} lonlat
    * @param {OpenLayers.Size} size
    * @param {String} contentHTML
    * @param {Object} anchor  Object which must expose a 
    *                         - 'size' (OpenLayers.Size) and 
    *                         - 'offset' (OpenLayers.Pixel) 
    *                         (this is generally an OpenLayers.Icon)
    * @param {Boolean} closeBox
    */
    initialize:function(id, lonlat, size, contentHTML, anchor, closeBox)
    {
        var newArguments = new Array(id, lonlat, size, contentHTML, closeBox);
        OpenLayers.Popup.prototype.initialize.apply(this, newArguments);

        this.anchor = (anchor != null) ? anchor : { size: new OpenLayers.Size(0,0), offset: new OpenLayers.Pixel(0,0)};
        
        this.registerEvents();
    },

    /** 
    * @param {OpenLayers.Pixel} px
    * 
    * @returns Reference to a div that contains the drawn popup
    * @type DOMElement
    */
    draw: function(px) 
    {
        if (px == null) 
        {
            if ((this.lonlat != null) && (this.map != null)) 
            {
                px = this.map.getLayerPxFromLonLat(this.lonlat);
            }
        }
        
        // check the popup is in view, if not reposition it
        this.updatePos()
        
        // return result of base draw call
        return OpenLayers.Popup.prototype.draw.apply(this, arguments);
    },
    
    updatePos : function()
    {
        var oMapSize = support.getPos(this.map.div);
        var oPopupPx = this.calculateNewPx(this.map.getLayerPxFromLonLat(this.lonlat));
        oPopupPx = this.map.getViewPortPxFromLayerPx(oPopupPx);

        var x2 = parseInt(oPopupPx.x + this.size.w);
        var y2 = parseInt(oPopupPx.y + this.size.h);
        
        // if left is not visible ?
        if (oPopupPx.x < oMapSize.x)
        {
            this.differenceX = parseInt(oPopupPx.x - oMapSize.x);
        }
        
        // if right is not visible
        if (x2 > oMapSize.w)
        {
            this.differenceX = parseInt(x2 - oMapSize.w);
        }
        
        // if bottom is not visible
        if (y2 > oMapSize.h)
        {
            this.differenceY = parseInt(y2 - oMapSize.h);
        }
        
        // if top is not visible
        if (oPopupPx.y < oMapSize.y)
        {
            this.differenceY = parseInt(oPopupPx.y - oMapSize.y);
        }
        
        // if a difference has been assigned call the Shit Method
        if (this.differenceX!=0 || this.differenceY!=0)
        {
            ShiftMap(this);
        }
    },
    /*
     registerEvents:function() {
        this.events = new OpenLayers.Events(this, this.div, null, true);
        this.events.register("mousedown", this, this.onmousedown);
        this.events.register("mousemove", this, this.onmousemove);
        this.events.register("mouseup", this, this.onmouseup);
        this.events.register("click", this, OpenLayers.Util.safeStopPropagation);
        this.events.register("mouseout", this, this.onmouseout);
        this.events.register("dblclick", this, OpenLayers.Util.safeStopPropagation);
     },
    */
    /**
    * @param {OpenLayers.Pixel} px
    */
    moveTo: function(px) 
    {
        var newPx = this.calculateNewPx(px);
        
        var newArguments = new Array(newPx);        
        OpenLayers.Popup.prototype.moveTo.apply(this, newArguments);
    },
    
    /**
    * @param {OpenLayers.Size} size
    */
    setSize : function(size) 
    { 
        OpenLayers.Popup.prototype.setSize.apply(this, arguments);

        if ((this.lonlat) && (this.map))
        {
            var px = this.map.getLayerPxFromLonLat(this.lonlat);
            this.moveTo(px);
        }
    },
        
    /** 
     * @private 
     * 
     * @param {OpenLayers.Pixel} px
     * 
     * @returns The the new px position of the popup on the screen
     *           relative to the passed-in px
     * @type OpenLayers.Pixel
     */
    calculateNewPx:function(px) {
        var newPx = px.offset(this.anchor.offset);

        var top = (this.relativePosition.charAt(0) == 't');
        newPx.y += (top) ? -this.size.h : this.anchor.size.h;
        
        var left = (this.relativePosition.charAt(1) == 'l');
        newPx.x += (left) ? -this.size.w : this.anchor.size.w;
        newPx.x += 55;
        newPx.y -= 15;
        
        return newPx;   
    },

    CLASS_NAME: "OpenLayers.Popup.Anchored"
});


/***********************************************************************************/


/* Copyright (c) 2006-2007 MetaCarta, Inc., published under the BSD license.
 * See http://svn.openlayers.org/trunk/openlayers/release-license.txt 
 * for the full text of the license. */


/**
 * @class
 * 
 * @requires OpenLayers/Popup/Anchored.js
 */
OpenLayers.Popup.AnchoredBubble = OpenLayers.Class.create();

OpenLayers.Popup.AnchoredBubble.prototype = OpenLayers.Class.inherit( OpenLayers.Popup.Anchored, {

    /** 
    * @constructor
    * 
    * @param {String} id
    * @param {OpenLayers.LonLat} lonlat
    * @param {OpenLayers.Size} size
    * @param {String} contentHTML
    * @param {Object} anchor  Object which must expose a 
    *                         - 'size' (OpenLayers.Size) and 
    *                         - 'offset' (OpenLayers.Pixel) 
    *                         (this is generally an OpenLayers.Icon)
    * @param {Boolean} closeBox
    */
    initialize:function(id, lonlat, size, contentHTML, anchor, closeBox)
    {
        OpenLayers.Popup.Anchored.prototype.initialize.apply(this, arguments);
    },

    /** 
    * @param {OpenLayers.Pixel} px
    * 
    * @returns Reference to a div that contains the drawn popup
    * @type DOMElement
    */
    draw: function(px) {
        
        OpenLayers.Popup.Anchored.prototype.draw.apply(this, arguments);

        this.setContentHTML();

        return this.div;
    },

    /**
    * @param {OpenLayers.Size} size
    */
    setSize:function(size)
    { 
        OpenLayers.Popup.Anchored.prototype.setSize.apply(this, arguments);
    },  
    
    /**
     * @param {float} opacity
     */
    setOpacity:function(opacity)
    { 
        if (opacity != undefined)
        {
            this.opacity = opacity; 
        }
        
        if (this.div != null)
        {
            if (this.contentDiv != null)
            {
                OpenLayers.Rico.Corner.changeOpacity(this.contentDiv, this.opacity);
            }
        }
    },  

    CLASS_NAME: "OpenLayers.Popup.AnchoredBubble"
});



/*****************************************************/

/* Copyright (c) 2006-2007 MetaCarta, Inc., published under the BSD license.
 * See http://svn.openlayers.org/trunk/openlayers/release-license.txt 
 * for the full text of the license. */


/**
 * @class
 * 
 * @requires OpenLayers/Layer/Markers.js
 */
OpenLayers.Layer.Text = OpenLayers.Class.create();
OpenLayers.Layer.Text.prototype = OpenLayers.Class.inherit( OpenLayers.Layer.Markers, {

    /** store url of text file - this should be specified in the 
     *   "options" hashtable
    * @type str */
    location:null,

    /** @type Array(OpenLayers.Feature) */
    features: null,

    /** @type OpenLayers.Feature */
    selectedFeature: null,

    /**
    * @constructor
    *
    * @param {String} name
    * @param {String} location
    * @param {Object} options Hashtable of extra options to tag onto the layer
    */
    initialize: function(name, options) {
        OpenLayers.Layer.Markers.prototype.initialize.apply(this, arguments);
        
        this.features = new Array();
        
        if (this.location != null)
        {
            OpenLayers.loadURL(this.location, null, this, this.parseData);
        }
    },


   /**
     * 
     */
    destroy: function() {
        this.clearFeatures();
        this.features = null;
        OpenLayers.Layer.Markers.prototype.destroy.apply(this, arguments);
    },
    
    
    /**
     * @param {XMLHttpRequest} ajaxRequest
     */
    parseData: function(ajaxRequest) {
        var text = ajaxRequest.responseText;
        var lines = text.split('\n');
        var columns;
        
        // length - 1 to allow for trailing new line
        for (var lcv = 0; lcv < (lines.length - 1); lcv++)
        {
            var currLine = lines[lcv].replace(/^\s*/,'').replace(/\s*$/,'');
        
            if (currLine.charAt(0) != '#') { /* not a comment */
            
                if (!columns)
                {
                    //First line is columns
                    columns = currLine.split('\t');
                }
                else
                {
                    var vals = currLine.split('\t');
                    var location = new OpenLayers.LonLat(0,0);
                    var title; var url;
                    var icon, iconSize, iconOffset;
                    var set = false;
                    for (var valIndex = 0; valIndex < vals.length; valIndex++)
                    {
                        if (vals[valIndex]) 
                        {
                            if (columns[valIndex] == 'point')
                            {
                                var coords = vals[valIndex].split(',');
                                location.lat = parseFloat(coords[0]);
                                location.lon = parseFloat(coords[1]);
                                set = true;
                            }
                            else if (columns[valIndex] == 'lat')
                            {
                                location.lat = parseFloat(vals[valIndex]);
                                set = true;
                            } 
                            else if (columns[valIndex] == 'lon')
                            {
                                location.lon = parseFloat(vals[valIndex]);
                                set = true;
                            } 
                            else if (columns[valIndex] == 'title') title = vals[valIndex];
                            else if (columns[valIndex] == 'image' || columns[valIndex] == 'icon') url = vals[valIndex];
                            else if (columns[valIndex] == 'iconSize')
                            {
                                var size = vals[valIndex].split(',');
                                iconSize = new OpenLayers.Size(parseFloat(size[0]), parseFloat(size[1]));
                            } 
                            else if (columns[valIndex] == 'iconOffset')
                            {
                                var offset = vals[valIndex].split(',');
                                iconOffset = new OpenLayers.Pixel(parseFloat(offset[0]), parseFloat(offset[1]));
                            } 
                            else if (columns[valIndex] == 'title') 
                            {
                                title = vals[valIndex];
                            } 
                            else if (columns[valIndex] == 'description')
                            {
                                description = vals[valIndex];
                            }
                        }
                    }
                    if (set) 
                    {
                      var data = new Object();
                      if (url != null)
                      {
                          data.icon = new OpenLayers.Icon(url, iconSize, iconOffset);
                      }
                      else
                      {
                          data.icon = OpenLayers.Marker.defaultIcon();

                          //allows for the case where the image url is not 
                          // specified but the size is. use a default icon
                          // but change the size
                          if (iconSize != null)
                          {
                              data.icon.setSize(iconSize);
                          }
                      }
                      if ((title != null) && (description != null)) 
                      {
                          data['popupContentHTML'] = '<h2>'+title+'</h2><p>'+description+'</p>';
                      }
                      var feature = new OpenLayers.Feature(this, location, data);
                      this.features.push(feature);
                      var marker = feature.createMarker();
                      marker.events.register('click', feature, this.markerClick);
                      this.addMarker(marker);
                    }
                }
            }
        }
    },
    
        
    /**
     * @param {Event} evt
     */
    markerClick: function(evt)
    {
        sameMarkerClicked = (this == this.layer.selectedFeature);
        this.layer.selectedFeature = (!sameMarkerClicked) ? this : null;
        
        for(var i=0; i < this.layer.map.popups.length; i++) {
            this.layer.map.removePopup(this.layer.map.popups[i]);
        }
        
        this.layer.map.addPopup(this.createPopup());

        OpenLayers.Event.stop(evt);
    },

    /**
     * 
     */
    clearFeatures: function() {
        if (this.features != null) {
            while(this.features.length > 0) {
                var feature = this.features[0];
                OpenLayers.Util.removeItem(this.features, feature);
                feature.destroy();
            }
        }        
    },
    
    /** @final @type String */
    CLASS_NAME: "OpenLayers.Layer.Text"
});


/**
 * ShiftMap is a function called by the Anchor class to slide the current popup into view.
 * Popups are displayed exclusivly meaning "there can be only one" at a time.
 * This function was not embedded into any OpenLayer classes due to the nature of its operation and scoping issues.
 * 
 * @Function
 */
function ShiftMap(obj)
{
    var o = obj; // copy object locally to facilitate timeout execution
    
    if (o!=null)
    {
        // record the date and time of first call to this method using anchor object as scope
        if (!o.ShiftStartTime) o.ShiftStartTime = (new Date()).getTime();
        
        if (o.differenceX!=0)
        {
            //
            // X difference detected, process
            //
            
            // determine direction of motion
            var dir = (o.differenceX>0) ? "+":"-";
            
            // calculated elapsed time since start of animation
            var elapsed = (new Date()).getTime() - o.ShiftStartTime;
            
            // calculated the required incrememt to ensure animation is complete within predefined timescale
            var increment = Math.round(elapsed / OpenLayers.Popup.SlideAnimationTime * Math.abs(o.differenceX));
            
            switch(dir)
            {
                case "+":
                {
                    if ((o) && (o.differenceX>1) && (o.map))
                    {
                        o.differenceX-=increment;
                        o.map.pan(+increment,0);
                        window.setTimeout(function() { ShiftMap(o); }, OpenLayers.Popup.SlideTimerInterval);
                    }
                }break;
                
                case "-":
                {
                    if ((o) && (o.differenceX<1) && (o.map))
                    {
                        o.differenceX+=increment;
                        o.map.pan(-increment,0);
                        window.setTimeout(function() { ShiftMap(o); }, OpenLayers.Popup.SlideTimerInterval);
                    }
                }break;
            }
        }
        
        if (o.differenceY!=0)
        {
            //
            // Y difference detected, process
            //
            
            // determine direction of motion
            var dir = (o.differenceY>0) ? "+":"-";
            
            // calculated elapsed time since start of animation
            var elapsed = (new Date()).getTime() - o.ShiftStartTime;
            
            // calculated the required incrememt to ensure animation is complete within predefined timescale
            var increment = Math.round(elapsed / OpenLayers.Popup.SlideAnimationTime * Math.abs(o.differenceY));
            
            switch(dir)
            {
                case "+":
                {
                    if ((o) && (o.differenceY>1) && (o.map))
                    {
                        o.differenceY-=increment;
                        o.map.pan(0,+increment);
                        window.setTimeout(function() { ShiftMap(o); }, OpenLayers.Popup.SlideTimerInterval);
                    }
                }break;
                
                case "-":
                {
                    if ((o) && (o.differenceY<1) && (o.map))
                    {
                        o.differenceY+=increment;
                        o.map.pan(0,-increment);
                        window.setTimeout(function() { ShiftMap(o); }, OpenLayers.Popup.SlideTimerInterval);
                    }
                }break;
            }
        }
    }
}


/**
 * Custom support object containg static methods required by the updated Popup and associated Classes, could be merged into the OpenLayers.Util collection
 * I have included an opacity fade method, however with the map I am using for testing loading slow, sliding and fading appeared too much.
 * 
 * @Object
 */
var support = {

    viewport : function()
    {
        return {
            height : (document.body.clientHeight || document.documentElement.clientHeight || window.innerHeight || 0),
            width : (document.body.clientWidth || document.documentElement.clientWidth || window.innerWidth || 0),
            scrollX : (document.body.scrollLeft || document.documentElement.scrollLeft || self.pageXOffset || 0),
            scrollY : (document.body.scrollTop || document.documentElement.scrollTop || self.pageYOffset || 0)
        };
    },
    
	/* return position of element */
	getPos : function(el)	
	{
		var x=0, y=0, w=0, h=0; 
	
		if (document.getBoxObjectFor)
		{ 

			var bo = document.getBoxObjectFor(el); 
			return {x:bo.x, y:bo.y, w:bo.width, h:bo.height}
		} 
		else if (el.getBoundingClientRect)
		{
			var rect = el.getBoundingClientRect(); 
			return {x:rect.left + support.viewport().scrollX, y:rect.top + support.viewport().scrollY, w:(rect.right - rect.left), h:(rect.bottom - rect.top)}
		}
		else
		{

			w = el.offsetWidth;
			h = el.offsetHeight;
			do { x += el.offsetLeft || 0; y += el.offsetTop || 0; el = el.offsetParent; } while (el);
			return {x:x, y:y, w:w, h:h}
		}
	},

    fade : function(id, opacStart, opacEnd, millisec, func)
    {
        //speed for each frame
        var speed = Math.round(millisec / 100);
        var timer = 0;

        //determine the direction for the blending, if start and end are the same nothing happens
        if(opacStart > opacEnd)
        {
            for(i = opacStart; i >= opacEnd; i--)
            {

                setTimeout("support.changeOpac(" + i + ",'" + id + "')",(timer * speed));
                timer++;
            }
        } 
        else if(opacStart < opacEnd)
        {
            for(i = opacStart; i <= opacEnd; i++)
            {
                setTimeout("support.changeOpac(" + i + ",'" + id + "')",(timer * speed));
                timer++;
            }
        }
        if (opacStart == opacEnd)
        {
            func();
        }
    },

    //change the opacity for different browsers
    changeOpac : function(opacity, id)
    {
        var object = document.getElementById(id).style;
        object.opacity = (opacity / 100);
        object.MozOpacity = (opacity / 100);
        object.KhtmlOpacity = (opacity / 100);
        object.filter = "alpha(opacity=" + opacity + ")";
    }
}