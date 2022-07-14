function StudentProgressGraph(sdata, DomId) {
    var width = 300;
    var height = 55;

    var data = sdata.data;
    var individual = sdata.individual;
    var colors = ['#02245b', '#ffc600'];

    var svg = d3
        .select(DomId)
        .append("svg")
        .attr("width", width)
        .attr("height", height);

    var g = svg.selectAll("g")
        .data(data)
        .enter()
        .append("g")
        .attr("transform", function (d, i) {
            return "translate(0,0)";
        })

    g.append("circle").attr("cx", function (d, i) {
        return ((width - 30) / 100) * d;

    })
        .attr("transform", "translate(10,0)")
        .attr("cy", function (d, i) {
            return 10;
        })
        .attr("r", function () {
            return 10;
        })
        .attr("fill", function (d, i) {
            if (d == individual) {
                return colors[0];
            }
            else
                return colors[1];
        })
        .attr("fill-opacity", function (d, i) {
            if (d == individual)
                return 1;
            else
                return .6;
        })

    // Create scale
    var scale = d3.scalePoint()
        .domain(["0%", "25%", "50%", "75%", "100%"])
        .range([0, width - 30]);

    // Add scales to axis
    var x_axis = d3.axisBottom()
        .scale(scale);

    //Append group and insert axis
    svg.append("g")
        .attr("transform", "translate(10,30)")
        .call(x_axis);
}