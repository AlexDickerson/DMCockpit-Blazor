function resizeWithScroll(elementID) {
  var element = {};
  element.e = document.getElementById(elementID);

  element.minZoom = Math.min(element.e.offsetWidth, element.e.offsetHeight); // = 100;
  element.maxZoom = Math.min(window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth, window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight);
  element.zoomSpeed = 0.1; // = 1 nominal speed;

  if (element.e.addEventListener) {
    element.e.addEventListener("mousewheel", MouseWheelHandler, false);
    element.e.addEventListener("DOMMouseScroll", MouseWheelHandler, false);
  } else element.e.attachEvent("onmousewheel", MouseWheelHandler);

  function MouseWheelHandler(e) {
    // cross-browser wheel delta
    var e = window.event || e;
    var delta = Math.max(-1, Math.min(1, (e.wheelDelta || -e.detail))) * element.zoomSpeed;
    var newWidth = Math.max(element.minZoom, Math.min(element.maxZoom, element.e.offsetWidth + (element.minZoom * delta)));
    var newHeight = Math.max(element.minZoom, Math.min(element.maxZoom, element.e.offsetHeight + (element.minZoom * delta)));
    element.e.style.width = newWidth + "px";
    element.e.style.height = newHeight + "px";

    return false;
  }
}