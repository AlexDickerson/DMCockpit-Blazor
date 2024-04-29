function dragAndDrop(className, boundsElementId) {
    const boundsElement = document.getElementById(boundsElementId);
    const position = { x: 0, y: 0 };

    interact(className).draggable({
        listeners: {
            start(event) {
                console.log(event.type, event.target);
            },
            move(event) {
                if (event.clientX < boundsElement.getBoundingClientRect().left || event.clientX > boundsElement.getBoundingClientRect().right || event.clientY < boundsElement.getBoundingClientRect().top || event.clientY > boundsElement.getBoundingClientRect().bottom) {
                    return;
                }

                if (!event.shiftKey) {
                    return;
                }

                position.x += event.dx;
                position.y += event.dy;

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

    element.minZoom = Math.min(element.e.offsetWidth, element.e.offsetHeight) * .05;
    element.maxZoom = Math.min(window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth, window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight);
    element.zoomSpeed = 0.05;

    if (element.e.addEventListener) {
        element.e.addEventListener("mousewheel", MouseWheelHandler, false);
        element.e.addEventListener("DOMMouseScroll", MouseWheelHandler, false);
    } else element.e.attachEvent("onmousewheel", MouseWheelHandler);

    function MouseWheelHandler(e) {
        var e = window.event || e;
        var delta = e.wheelDelta * element.zoomSpeed;
        var newWidth = Math.max(element.e.offsetWidth + delta, element.minZoom);
        var newHeight = Math.max(element.e.offsetHeight + delta, element.minZoom);

        var aspectRatio = element.e.offsetWidth / element.e.offsetHeight;
        if (newWidth / newHeight > aspectRatio) {
            newWidth = newHeight * aspectRatio;
        } else {
            newHeight = newWidth / aspectRatio;
        }

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

    var pos = { x: 0, y: 0 };

    window.addEventListener('resize', resize);
    document.addEventListener('mousemove', draw);
    document.addEventListener('mousedown', setPosition);
    document.addEventListener('mouseenter', setPosition);

    function setPosition(e) {
        const canvasElement = document.getElementById(canvasID);
        const canvasRect = canvasElement.getBoundingClientRect();

        pos.x = e.clientX - canvasRect.left;
        pos.y = e.clientY - canvasRect.top;
    }

    function resize() {
        context.canvas.width = mapElement.width;
        context.canvas.height = mapElement.height;
        context.fillStyle = 'rgba(32, 32, 32, 0.8)';
        context.fillRect(0, 0, canvasElement.width, canvasElement.height);
    }

    function draw(e) {
        if (e.buttons !== 1) return;

        if (e.target === excludeElement && e.shiftKey) {
            return;
        }

        context.globalCompositeOperation = 'destination-out';
        context.beginPath();

        context.lineWidth = 30;
        context.lineCap = 'round';
        context.strokeStyle = 'rgba(0, 0, 0, 1)';

        context.moveTo(pos.x, pos.y);
        setPosition(e);
        context.lineTo(pos.x, pos.y);

        context.stroke();

        maskHasChanged = true;
        hasProvidedMask = false;

        context.globalCompositeOperation = 'source-over';
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

    maskCanvasElement.style.position = 'absolute';
    maskCanvasElement.style.left = mapCanvasElement.offsetLeft + 'px';
    maskCanvasElement.style.top = mapCanvasElement.offsetTop + 'px';

    maskCanvasElement.width = mapCanvasElement.width;
    maskCanvasElement.height = mapCanvasElement.height;
}

function addCookie(cookieName, cookieValue) {
    document.cookie = cookieName + "=" + cookieValue + ";";
}

window.JsFunctions = {
    addKeyboardListenerEvent: function (foo) {
        let serializeEvent = function (e) {
            if (e) {
                return {
                    key: e.key,
                    code: e.keyCode.toString(),
                    location: e.location,
                    repeat: e.repeat,
                    ctrlKey: e.ctrlKey,
                    shiftKey: e.shiftKey,
                    altKey: e.altKey,
                    metaKey: e.metaKey,
                    type: e.type
                };
            }
        };

        window.document.addEventListener('keydown', function (e) {
            if ((e.shiftKey || e.ctrlKey || e.altKey) && e.key !== "Shift" && e.key !== "Control" && e.key !== "Alt") {
                e.preventDefault();
                DotNet.invokeMethodAsync('DMCockpit', 'JsKeyDown', serializeEvent(e));
            }
        });
    }
};

function GetElementStyleByClass(className) {
    var element = document.getElementsByClassName(className)[0];
    var children = element.children;

    var styleString = GetStylesForElement(element);

    for (var i = 0; i < children.length; i++) {
        styleString += GetStylesForElement(children[i]);
    }

    return styleString;
}

function GetStylesForElement(element) {
    var styles = window.getComputedStyle(element);

    var styleNames = [];

    for (var i = 0; i < styles.length; i++) {
        styleNames.push(styles[i]);
    }

    var styleValues = [];

    for (var i = 0; i < styles.length; i++) {
        styleValues.push(styles.getPropertyValue(styles[i]));
    }

    var styleString = "";

    for (var i = 0; i < styles.length; i++) {
        styleString += styles[i] + ":" + styles.getPropertyValue(styles[i]) + ";";
    }

    return styleString;
}

function GetElementsByClass(className) {
    return document.getElementsByClassName(className);
}

function AddGetImageSrcButton(elements, className) {
    elements = Array.from(elements);

    elements.forEach(element => {
        var newButton = document.createElement("button");

        newButton.innerHTML = "Show Image To Players";

        element.parentElement.parentElement.appendChild(newButton);

        newButton.addEventListener("click", function () {
            var content = GetImageSrcByClassName(className);
            var request = new XMLHttpRequest();
            request.open("POST", "http://localhost:8080", true);
            request.send(content);
        });
    })
}

function GetImageSrcByClassName(className) {
    var imageElements = document.getElementsByClassName(className);

    var imageSrcs = [];

    for (var i = 0; i < imageElements.length; i++) {
        imageSrcs.push(imageElements[i].src);
    }

    return imageSrcs[0];
}