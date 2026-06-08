document.addEventListener("DOMContentLoaded", function () {
    const htmlElement = document.getElementById("theme-html")
    const themeToggleBtn = document.getElementById("theme-toggle")
    const themeIcon = document.getElementById("theme-icon")
    console.log("ninja")
    
    // Funkcja do odczytu ciasteczka
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
    }
    
    // Funkcja do zapisu ciasteczka
    function setCookie(name, value, days) {
        const date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        const expires = `expires=${date.toUTCString()}`;
        document.cookie = `${name}=${value}; ${expires}; path=/`;
    }
    
    const savedTheme = getCookie("theme");
    
    if (savedTheme) {
        htmlElement.setAttribute("data-bs-theme", savedTheme)
        updateButtonIcon(savedTheme)
    }
    
    themeToggleBtn.addEventListener("click", function () {
        console.log("Ninja")
        const currentTheme = htmlElement.getAttribute("data-bs-theme")
        let newTheme = "dark"

        if (currentTheme === "dark") {
            newTheme = "light"
        }

        htmlElement.setAttribute("data-bs-theme", newTheme)
        setCookie("theme", newTheme, 100)
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
