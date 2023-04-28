document.addEventListener("DOMContentLoaded", function () {
    const finishUntil = new Date(
        document.querySelector("#finish-until").textContent
    );
    const countdownElement = document.getElementById("countdown");
    const statusElement = document.querySelector(".todo-details p:nth-child(4)");
    const isCompleted = statusElement.textContent.includes("Not Completed");
    if (isCompleted) {
        function updateCountdown() {
            const now = new Date();
            let diff = finishUntil - now;
            if (diff < 0) {
                countdownElement.innerHTML = "Time has expired.";
                return;
            }

            const hours = Math.floor(diff / (1000 * 60 * 60));
            diff %= 1000 * 60 * 60;
            const minutes = Math.floor(diff / (1000 * 60));
            diff %= 1000 * 60;
            const seconds = Math.floor(diff / 1000);

            countdownElement.innerHTML = `Time left: ${hours}h ${minutes}m ${seconds}s`;
        }

        updateCountdown();
        setInterval(updateCountdown, 1000);
    } else {
        countdownElement.style.display = "none";
    }
});
