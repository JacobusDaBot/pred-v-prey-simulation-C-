const xhttp = new XMLHttpRequest();

var TestCSV = String;
arrlines = []
arrline = []
arrNo = []
arrPrey = []
arrPredator = []
icount = 0;
xhttp.onload = function() {

    TestCSV = this.responseText.toString()

    arrlines = TestCSV.split("\n")
    arrlines.shift()
        // console.log(arrlines)

    //for (let x in arrlines) {
    for (let i = 0; i < arrlines.length; i++) {
        // console.log(arrlines[i])
        arrline = arrlines[i].split(",")

        arrNo[icount] = Number(arrline[0])
        arrPrey[icount] = Number(arrline[2])
        arrPredator[icount] = Number(arrline[1])
        icount++;
        arrline

    }
    //console.log(arrPrey)

    new Chart("myChart", {
        type: "line",
        data: {
            labels: arrNo,
            datasets: [{
                    label: 'Prey',
                    data: arrPrey, //[860, 1140, 1060, 1060, 1070, 1110, 1330, 2210, 7830, 2478],
                    borderColor: "green",
                    fill: false

                }, {
                    label: 'Predator',
                    data: arrPredator, // [1600, 1700, 1700, 1900, 2000, 2700, 4000, 5000, 6000, 7000],
                    borderColor: "red",
                    fill: false,

                }
                /*, {
                                data: [300, 700, 2000, 5000, 6000, 4000, 2000, 1000, 200, 100],
                                borderColor: "blue",
                                fill: false
                            }*/
            ]
        },
        options: {
            plugins: {
                legend: {
                    display: true,
                    labels: {
                        color: 'rgb(255, 99, 132)'
                    }
                }
            }
        }
    });
}


xhttp.open("GET", "test.csv");
xhttp.send();