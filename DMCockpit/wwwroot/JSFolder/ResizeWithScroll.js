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