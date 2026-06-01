document.addEventListener("DOMContentLoaded", function () {
    const htmlElement = document.getElementById("theme-html")
    const themeToggleBtn = document.getElementById("theme-toggle")
    const themeIcon = document.getElementById("theme-icon")
    console.log("ninja")
    // const savedTheme = localStorage.getItem("theme")

    // if (savedTheme) {
    //     htmlElement.setAttribute("data-bs-theme", savedTheme)
    //     updateButtonIcon(savedTheme)
    // }
    
    themeToggleBtn.addEventListener("click", function () {
        console.log("Ninja")
        const currentTheme = htmlElement.getAttribute("data-bs-theme")
        let newTheme = "dark"

        if (currentTheme === "dark") {
            newTheme = "light"
        }

        htmlElement.setAttribute("data-bs-theme", newTheme)
        localStorage.setItem("theme", newTheme)
        updateButtonIcon(newTheme)
    });

    function updateButtonIcon(theme) {
        if (theme === "dark") {
            themeIcon.className = "bi bi-sun-fill"
            themeToggleBtn.className = "btn btn-lg"
        } else {
            themeIcon.className = "bi bi-moon-fill"
            themeToggleBtn.className = "btn btn-lg"
        }
    }

    updateButtonText(htmlElement.getAttribute("data-bs-theme"))
});