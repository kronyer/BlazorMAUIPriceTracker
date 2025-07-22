window.scrollToBottomIfNearEnd = function (thresholdRem) {
    const rem = parseFloat(getComputedStyle(document.documentElement).fontSize);
    const thresholdPx = rem * thresholdRem;
    const scrollable = document.scrollingElement || document.documentElement;
    const distanceToBottom = scrollable.scrollHeight - scrollable.scrollTop - scrollable.clientHeight;

    if (distanceToBottom < thresholdPx) {
        // Rola até o final da página
        const anchor = document.getElementById('keyboard-bottom-anchor');
        if (anchor) {
            anchor.scrollIntoView({ behavior: 'smooth', block: 'end' });
        } else {
            window.scrollTo({ top: scrollable.scrollHeight, behavior: 'smooth' });
        }
    }
}

window.scrollDownIfFocusNearBottom = function (rem) {
    const remInPx = rem * parseFloat(getComputedStyle(document.documentElement).fontSize);

    const el = document.activeElement;

    if (!el || (el.tagName !== 'INPUT' && el.tagName !== 'TEXTAREA')) {
        return;
    }

    const rect = el.getBoundingClientRect();
    const viewportHeight = window.innerHeight;
    const threshold = viewportHeight - remInPx;

    if (rect.bottom >= threshold) {
        window.scrollBy({ top: remInPx, left: 0, behavior: 'smooth' });
    }
};


window.scrollDownByRem = function (rem) {
    const remInPx = rem * parseFloat(getComputedStyle(document.documentElement).fontSize);
    window.scrollBy({ top: remInPx, left: 0, behavior: 'smooth' });
};