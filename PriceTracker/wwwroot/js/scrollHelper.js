window.isScrollAtBottom = function (selector) {
    var el = document.querySelector(selector);
    if (!el) return false;
    return el.scrollTop + el.clientHeight >= el.scrollHeight - 1;
}