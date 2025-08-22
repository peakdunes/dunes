/*Aumentar el tamano de la letra en las vistas */
document.addEventListener("DOMContentLoaded", function () {
    const slider = document.getElementById("fontSizeSlider");
    const html = document.documentElement;

    if (!slider) return;

    const savedSize = localStorage.getItem("fontSize");
    if (savedSize) {
        html.style.fontSize = `${savedSize}px`;
        slider.value = savedSize;
    }

    slider.addEventListener("input", function () {
        const newSize = this.value;
        html.style.fontSize = `${newSize}px`;
        localStorage.setItem("fontSize", newSize);
    });
});