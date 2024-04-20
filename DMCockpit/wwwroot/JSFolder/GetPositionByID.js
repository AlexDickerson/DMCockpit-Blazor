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