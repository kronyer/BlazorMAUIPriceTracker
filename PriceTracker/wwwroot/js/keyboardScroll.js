window.registerOnHold = (element, dotNetHelper, id) => {
    let timer;
    element.onmousedown = element.ontouchstart = function () {
        timer = setTimeout(() => {
            dotNetHelper.invokeMethodAsync('OnHold', id);
        }, 600); // 600ms para considerar "hold"
    };
    element.onmouseup = element.ontouchend = function () {
        clearTimeout(timer);
    };
};
