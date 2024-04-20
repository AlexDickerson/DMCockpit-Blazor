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