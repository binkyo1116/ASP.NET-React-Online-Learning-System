var width = 700;
var height = 400;
var barWidth = 20;

var margin = { top: 20, right: 10, bottom: 145, left: 10 };

var width = width - margin.left - margin.right,
    height = height - margin.top - margin.bottom;

var totalWidth = width + margin.left + margin.right;
var totalheight = height + margin.top + margin.bottom;

//==========================================================
if (statistic_data_grade.length > 0 || statistic_data_assessment.length > 0 ) {
    d3.select("#headingQuiz").append("h6").text("Quiz Grading Statistics");
    DrowBoxPlot(statistic_data_grade, "#my_dataviz");
    d3.select("#headingAss").append("h6").text("Assessment Grading Statistics");
    DrowBoxPlot(statistic_data_assessment, "#my_assessment_chart");
}

//============================================================
function DrowBoxPlot (GraphData, DivId) { 
// Generate five 100 count, normal distributions with random means
var groupCounts = {};
var individualGCounts = {};
var globalCounts = [];

    GraphData.forEach(function (d, i) {
    //var key = i.toString();
    var key = d.label;
    groupCounts[key] = [];
    d.quizList.forEach(function (e, i) {
        groupCounts[key].push(e);
        globalCounts.push(e);
    });
        individualGCounts[key] = [];
        individualGCounts[key].push(d.individualGrade == null ? 0 : d.individualGrade);
    //d.individualGrade.forEach(function (e, i) {
    //    individualGCounts[key].push(e);
    //})
})

// Sort group counts so quantile methods work
for (var key in groupCounts) {
    var groupCount = groupCounts[key];
    groupCounts[key] = groupCount.sort(d3.descending);
}
for (var key in individualGCounts) {
    var individualCount = individualGCounts[key];
    individualGCounts[key] = individualCount.sort(d3.descending);
}

// Prepare the data for the box plots
var boxPlotData = [];
for (var [key, groupCount] of Object.entries(groupCounts)) {

    var record = {};
    var localMin = d3.min(groupCount);
    var localMax = d3.max(groupCount);

    record["key"] = key;
    record["counts"] = groupCount;
    record["quartile"] = boxQuartiles(groupCount);
    record["whiskers"] = [localMax, localMin];
    record["color"] = "#2c79bf"; /* colorScale(key);*/

    boxPlotData.push(record);
}
//---------------------------------
var boxPlotDataIndividual = [];
for (var [key, individualCount] of Object.entries(individualGCounts)) {

    var record = {};
    var localMin = d3.min(individualCount);
    var localMax = d3.max(individualCount);

    record["key"] = key;
    record["counts"] = individualCount;
    record["quartile"] = boxQuartiles(individualCount);
    record["whiskers"] = [localMax, localMin];

    boxPlotDataIndividual.push(record);
}

//--------------------------------
// Compute an ordinal xScale for the keys in boxPlotData
var xScale = d3.scalePoint()
    .domain(Object.keys(groupCounts))
    .rangeRound([0, width-15])
    .padding([.5])

// Compute a global y scale based on the global counts
var min = d3.min(globalCounts);
var max = d3.max(globalCounts);

var yScale = d3.scaleLinear()
    .domain([max, min])
    .range([0, height]);


    // Setup the svg and group we will draw the box plot in
    d3.select(DivId).append("p").text(course_name).style("font-size", "15px").style("margin-bottom", "15px");
var svg = d3.select(DivId).append("svg")
    .attr("width", totalWidth)
    .attr("height", totalheight)
    .append("g")
    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

// Move the left axis over 25 pixels, and the top axis over 35 pixels
var axisG = svg.append("g")
    .attr("transform", "translate(25,-5)");
var axisTopG = svg.append("g").attr("transform", "translate(30, " + height + ")");

// Setup the group the box plot elements will render in
var g = svg.append("g")
    .attr("transform", "translate(20,-5)");

// Draw the box plot vertical lines
var verticalLines = g.selectAll(".verticalLines")
    .data(boxPlotData)
    .enter()
    .append("line")
    .attr("x1", function (datum) {
        return xScale(datum.key) + barWidth / 2;
    }
    )
    .attr("y1", function (datum) {
        var whisker = datum.whiskers[0];
        return yScale(whisker);
    }
    )
    .attr("x2", function (datum) {
        return xScale(datum.key) + barWidth / 2;
    }
    )
    .attr("y2", function (datum) {
        var whisker = datum.whiskers[1];
        return yScale(whisker);
    }
    )
    .attr("stroke", "#000")
    .attr("stroke-width", 1)
    .attr("fill", "none");

// Draw the boxes of the box plot, filled in white and on top of vertical lines
var rects = g.selectAll("rect")
    .data(boxPlotData)
    .enter()
    .append("rect")
    .attr("width", barWidth)
    .attr("height", function (datum) {
        var quartiles = datum.quartile;
        var height = yScale(quartiles[2]) - yScale(quartiles[0]);
        return height;
    }
    )
    .attr("x", function (datum) {
        return xScale(datum.key);
    }
    )
    .attr("y", function (datum) {
        return yScale(datum.quartile[0]);
    }
    )
    .attr("fill", function (datum) {
        return datum.color;
    }
    )
    .attr("stroke", "#000")
    .attr("stroke-width", .5);

// Now render all the horizontal lines at once - the whiskers and the median
var horizontalLineConfigs = [
    // Top whisker
    {
        x1: function (datum) { return xScale(datum.key) },
        y1: function (datum) { return yScale(datum.whiskers[0]) },
        x2: function (datum) { return xScale(datum.key) + barWidth },
        y2: function (datum) { return yScale(datum.whiskers[0]) }
    },
    // Median line
    {
        x1: function (datum) { return xScale(datum.key) },
        y1: function (datum) { return yScale(datum.quartile[1]) },
        x2: function (datum) { return xScale(datum.key) + barWidth },
        y2: function (datum) { return yScale(datum.quartile[1]) }
    },
    // Bottom whisker
    {
        x1: function (datum) { return xScale(datum.key) },
        y1: function (datum) { return yScale(datum.whiskers[1]) },
        x2: function (datum) { return xScale(datum.key) + barWidth },
        y2: function (datum) { return yScale(datum.whiskers[1]) }
    }
];

for (var i = 0; i < horizontalLineConfigs.length; i++) {
    var lineConfig = horizontalLineConfigs[i];

    // Draw the whiskers at the min for this series
    var horizontalLine = g.selectAll(".whiskers")
        .data(boxPlotData)
        .enter()
        .append("line")
        .attr("x1", lineConfig.x1)
        .attr("y1", lineConfig.y1)
        .attr("x2", lineConfig.x2)
        .attr("y2", lineConfig.y2)
        .attr("stroke", "#000")
        .attr("stroke-width", 1)
        .attr("fill", "none");
}
//-------------------------------------------------------------------------------
var circle = g.selectAll("circle")
    .data(boxPlotDataIndividual)
    .enter()
    .append("circle")
    .attr("cx", function (datum) {
        return xScale(datum.key) + barWidth / 2;
    }
    )
    .attr("cy", function (datum) {
        return yScale(datum.quartile[0]);
    }
    )
    .attr("r", 5)
    .attr("fill", "#e9ea4c")
    .attr("stroke", "#000")
    .attr("stroke-width", .5);
//--------------------------------------------------------------------------------
// Setup a scale on the left
var axisLeft = d3.axisLeft(yScale);
axisG.append("g")
    .call(axisLeft);

// Setup a series axis on the top
var axisTop = d3.axisBottom(xScale);
axisTopG.append("g")
    .call(axisTop)
    .selectAll("text")
    .style("text-anchor", "end")
    .attr("dx", "-.8em")
    .attr("dy", ".15em")
    .attr("transform", "rotate(-65)");

function boxQuartiles(d) {
    return [
        d3.quantile(d, .25),
        d3.quantile(d, .5),
        d3.quantile(d, .75)
    ];
}

// Perform a numeric sort on an array
function sortNumber(a, b) {
    return a - b;
}
//console.log(groupCounts);
//console.log(individualGCounts);
}