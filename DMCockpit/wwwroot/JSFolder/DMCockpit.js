function dragAndDrop(className, boundsElementId) {
    const boundsElement = document.getElementById(boundsElementId);
    const position = { x: 0, y: 0 };

    interact(className).draggable({
        listeners: {
            start(event) {
                console.log(event.type, event.target);
            },
            move(event) {
                //if mouse is outside the bounds of the element, return
                if (event.clientX < boundsElement.getBoundingClientRect().left || event.clientX > boundsElement.getBoundingClientRect().right || event.clientY < boundsElement.getBoundingClientRect().top || event.clientY > boundsElement.getBoundingClientRect().bottom) {
                    return;
                }

                //check if shift key is pressed
                if (!event.shiftKey) {
                    return;
                }

                position.x += event.dx;
                position.y += event.dy;

                // Restrict the draggable object to the bounds of the specified element
                const boundsRect = boundsElement.getBoundingClientRect();
                const maxX = boundsRect.width - event.target.offsetWidth;
                const maxY = boundsRect.height - event.target.offsetHeight;

                position.x = Math.max(0, Math.min(position.x, maxX - 1));
                position.y = Math.max(0, Math.min(position.y, maxY - 1));

                event.target.style.transform = `translate(${position.x}px, ${position.y}px)`;
            },
        }, 
    });
};

function getPositionByID(childID, parentID) {
    const childElement = document.getElementById(childID);
    const parentElement = document.getElementById(parentID);

    var childPosition = childElement.getBoundingClientRect();
    var parentPosition = parentElement.getBoundingClientRect();

    const relativePos = {};

    relativePos.top = childPosition.top - parentPosition.top,
        relativePos.right = childPosition.right - parentPosition.left,
        relativePos.bottom = childPosition.bottom - parentPosition.top,
        relativePos.left = childPosition.left - parentPosition.left;

    var relativeCoordinates = [
        { x: relativePos.left, y: relativePos.top },
        { x: relativePos.right, y: relativePos.bottom },
    ];

    var parentWidth = parentElement.clientWidth;
    var parentHeight = parentElement.clientHeight;

    relativeCoordinates[0].x = (relativeCoordinates[0].x / parentWidth) * 100;
    relativeCoordinates[0].y = (relativeCoordinates[0].y / parentHeight) * 100;

    relativeCoordinates[1].x = (relativeCoordinates[1].x / parentWidth) * 100;
    relativeCoordinates[1].y = (relativeCoordinates[1].y / parentHeight) * 100;

    return relativeCoordinates;
}

function resizeImage(imgID) {
    const imageELement = document.getElementById(imgID);

    const naturalWidth = imageELement.naturalWidth;
    const naturalHeight = imageELement.naturalHeight;

    const clientWidth = imageELement.clientWidth;
    const clientHeight = imageELement.clientHeight;

    // calculate the largest size that fits in the container
    const widthRatio = clientWidth / naturalWidth;
    const heightRatio = clientHeight / naturalHeight;

    const ratio = Math.min(widthRatio, heightRatio);

    const newWidth = naturalWidth * ratio;
    const newHeight = naturalHeight * ratio;

    imageELement.style.width = newWidth + 'px';
    imageELement.style.height = newHeight + 'px';
}

function drawImageWithCoordinates(imgID, canvasID, drawFromX, drawFromY, drawToX, drawToY) {
    var imageElement = document.getElementById(imgID);
    var canvasElement = document.getElementById(canvasID);

    var canvasParent = canvasElement.parentElement;
    canvasElement.width = canvasParent.clientWidth;
    canvasElement.height = canvasParent.clientHeight;

    drawFromX = drawFromX * imageElement.width / 100;
    drawFromY = drawFromY * imageElement.height / 100;
    drawToX = drawToX * imageElement.width / 100;
    drawToY = drawToY * imageElement.height / 100;

    var canvasWidth = canvasElement.width;
    var canvasHeight = canvasElement.height;

    var width = drawToX - drawFromX;
    var height = drawToY - drawFromY;

    var context = canvasElement.getContext("2d");
    context.imageSmoothingEnabled = false;

    context.drawImage(imageElement, drawFromX, drawFromY, width, height, 0, 0, canvasWidth, canvasHeight);
}

function resizeWithScroll(elementID, restrictElementID) {
    var element = {};
    element.e = document.getElementById(elementID);
    const restrictElement = document.getElementById(restrictElementID);

    element.minZoom = Math.min(element.e.offsetWidth, element.e.offsetHeight) * .05; // = 100;
    element.maxZoom = Math.min(window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth, window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight);
    element.zoomSpeed = 0.05; // = 1 nominal speed;

    if (element.e.addEventListener) {
        element.e.addEventListener("mousewheel", MouseWheelHandler, false);
        element.e.addEventListener("DOMMouseScroll", MouseWheelHandler, false);
    } else element.e.attachEvent("onmousewheel", MouseWheelHandler);

    function MouseWheelHandler(e) {
        // cross-browser wheel delta
        var e = window.event || e;
        var delta = e.wheelDelta * element.zoomSpeed;
        var newWidth = Math.max(element.e.offsetWidth + delta, element.minZoom);
        var newHeight = Math.max(element.e.offsetHeight + delta, element.minZoom);

        // Maintain aspect ratio
        var aspectRatio = element.e.offsetWidth / element.e.offsetHeight;
        if (newWidth / newHeight > aspectRatio) {
            newWidth = newHeight * aspectRatio;
        } else {
            newHeight = newWidth / aspectRatio;
        }

        // if new size would place part of the element outside the restrictElement, adjust the size
        var elementPosition = element.e.getBoundingClientRect();
        var restrictElementPosition = restrictElement.getBoundingClientRect();
        var elementTop = elementPosition.top;
        var elementRight = elementPosition.right;
        var elementBottom = elementPosition.bottom;
        var elementLeft = elementPosition.left;
        var restrictElementTop = restrictElementPosition.top;
        var restrictElementRight = restrictElementPosition.right;
        var restrictElementBottom = restrictElementPosition.bottom;
        var restrictElementLeft = restrictElementPosition.left;

        if (elementTop < restrictElementTop) {
            newHeight = newHeight - (restrictElementTop - elementTop);
        }

        if (elementRight > restrictElementRight) {
            newWidth = newWidth - (elementRight - restrictElementRight);
        }

        if (elementBottom > restrictElementBottom) {
            newHeight = newHeight - (elementBottom - restrictElementBottom);
        }

        if (elementLeft < restrictElementLeft) {
            newWidth = newWidth - (restrictElementLeft - elementLeft);
        }

        element.e.style.width = newWidth + "px";
        element.e.style.height = newHeight + "px";

        // Prevent scrolling the window when within the bounds of the element
        return false;
    }
}

var maskHasChanged = false;
var transparentPixels = [];
var hasProvidedMask = false;

function drawableMaskCanvas(canvasID, mapID, excludeElement) {
    var canvasElement = document.getElementById(canvasID);
    var mapElement = document.getElementById(mapID);
    var excludeElement = document.getElementById(excludeElement);

    var context = canvasElement.getContext("2d");
    resize();

    // last known position
    var pos = { x: 0, y: 0 };

    window.addEventListener('resize', resize);
    document.addEventListener('mousemove', draw);
    document.addEventListener('mousedown', setPosition);
    document.addEventListener('mouseenter', setPosition);

    // new position from mouse event
    function setPosition(e) {
        const canvasElement = document.getElementById(canvasID);
        const canvasRect = canvasElement.getBoundingClientRect();

        pos.x = e.clientX - canvasRect.left;
        pos.y = e.clientY - canvasRect.top;
    }

    // resize canvas
    function resize() {
        context.canvas.width = mapElement.width;
        context.canvas.height = mapElement.height;
        context.fillStyle = 'rgba(32, 32, 32, 0.8)'; // Darker more opaque grey
        context.fillRect(0, 0, canvasElement.width, canvasElement.height);
    }

    function draw(e) {
        // mouse left button must be pressed
        if (e.buttons !== 1) return;

        //if mouse is over the excluded element, return
        if (e.target === excludeElement && e.shiftKey) {
            return;
        }

        context.globalCompositeOperation = 'destination-out'; // Erase
        context.beginPath(); // begin

        context.lineWidth = 30;
        context.lineCap = 'round';
        context.strokeStyle = 'rgba(0, 0, 0, 1)'; // Fully opaque black

        context.moveTo(pos.x, pos.y); // from
        setPosition(e);
        context.lineTo(pos.x, pos.y); // to

        context.stroke(); // draw it!

        maskHasChanged = true;
        hasProvidedMask = false;

        context.globalCompositeOperation = 'source-over'; // Restore default
    }
}

function getCanvasBitmap(canvasID) {
    if (!maskHasChanged) {
        if(hasProvidedMask){
            return [];
        }
        return transparentPixels;
    }

    var canvasElement = document.getElementById(canvasID);

    //return an array that indicates which pixels are transparent
    var context = canvasElement.getContext("2d");
    context.willReadFrequently = true;
    var imageData = context.getImageData(0, 0, canvasElement.width, canvasElement.height);
    var data = imageData.data;
    transparentPixels = [];

    for (var i = 0; i < data.length; i += 4) {
        if (data[i + 3] === 0) {
            transparentPixels.push(true);
        } else {
            transparentPixels.push(false);
        }
    }

    maskHasChanged = false;
    hasProvidedMask = true;

    return transparentPixels;
}

function overlayMaskCanvas(maskCanvasID, mapCanvasID) {
    var maskCanvasElement = document.getElementById(maskCanvasID);
    var mapCanvasElement = document.getElementById(mapCanvasID);

    //position the mask canvas over the map canvas
    maskCanvasElement.style.position = 'absolute';
    maskCanvasElement.style.left = mapCanvasElement.offsetLeft + 'px';
    maskCanvasElement.style.top = mapCanvasElement.offsetTop + 'px';

    maskCanvasElement.width = mapCanvasElement.width;
    maskCanvasElement.height = mapCanvasElement.height;
}

function addCookie(cookieName, cookieValue) {
    document.cookie = cookieName + "=" + cookieValue + ";";
}