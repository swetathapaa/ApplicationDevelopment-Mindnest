window.renderAnalyticsCharts = (moodLabels, moodValues, tagLabels, tagValues) => {

    const moodCanvas = document.getElementById("moodChart");
    if (moodCanvas) {
        new Chart(moodCanvas, {
            type: "doughnut",
            data: {
                labels: moodLabels,
                datasets: [{
                    data: moodValues,
                    backgroundColor: [
                        "#FF7F50", "#4FA9A1", "#E6C85E",
                        "#D45D5D", "#7EBE8E", "#FFB347"
                    ]
                }]
            },
            options: { responsive: true, maintainAspectRatio: false }
        });
    }

    const tagCanvas = document.getElementById("tagChart");
    if (tagCanvas) {
        new Chart(tagCanvas, {
            type: "bar",
            data: {
                labels: tagLabels,
                datasets: [{
                    label: "Tags",
                    data: tagValues,
                    backgroundColor: "#4FA9A1"
                }]
            },
            options: { responsive: true, maintainAspectRatio: false }
        });
    }
};
