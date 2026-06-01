document.addEventListener("DOMContentLoaded", function () {
    const htmlElement = document.getElementById("theme-html");
    const themeToggleBtn = document.getElementById("theme-toggle");
    console.log("ninja")
    const savedTheme = localStorage.getItem("theme");

    if (savedTheme) {
        htmlElement.setAttribute("data-bs-theme", savedTheme);
        updateButtonText(savedTheme);
    }

    themeToggleBtn.addEventListener("click", function () {
        console.log("Ninja")
        const currentTheme = htmlElement.getAttribute("data-bs-theme");
        let newTheme = "dark";

        if (currentTheme === "dark") {
            newTheme = "light";
        }

        htmlElement.setAttribute("data-bs-theme", newTheme);
        localStorage.setItem("theme", newTheme);
        updateButtonText(newTheme);
    });

    function updateButtonText(theme) {
        if (theme === "dark") {
            themeToggleBtn.textContent = "Light Mode";
            themeToggleBtn.className = "btn btn-light btn-sm";
        } else {
            themeToggleBtn.textContent = "Dark Mode";
            themeToggleBtn.className = "btn btn-dark btn-sm ";
        }
    }

    updateButtonText(htmlElement.getAttribute("data-bs-theme"));
});