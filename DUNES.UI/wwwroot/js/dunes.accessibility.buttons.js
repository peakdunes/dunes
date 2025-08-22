document.addEventListener("DOMContentLoaded", function () {
    const increaseBtn = document.getElementById("btnFontIncrease");
    const decreaseBtn = document.getElementById("btnFontDecrease");
    const html = document.documentElement;

    const MIN_SIZE = 14;
    const MAX_SIZE = 24;

    let currentSize = parseInt(localStorage.getItem("fontSize") || 16);
    html.style.fontSize = `${currentSize}px`;

    function updateFontSize(size) {
        currentSize = Math.max(MIN_SIZE, Math.min(MAX_SIZE, size));
        html.style.fontSize = `${currentSize}px`;
        localStorage.setItem("fontSize", currentSize);
    }

    if (increaseBtn) {
        increaseBtn.addEventListener("click", () => updateFontSize(currentSize + 1));
    }

    if (decreaseBtn) {
        decreaseBtn.addEventListener("click", () => updateFontSize(currentSize - 1));
    }
});
