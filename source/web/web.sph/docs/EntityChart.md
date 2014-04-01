#EntityChart
[`EntityChart`](EntityChart.html) is a visual presentation of the current [`EntityView`](EntityView.html) which allow you to display in a visual manner. [`EntityChart`](EntityChart.html) will always filter the data accoring to the current [`EntityView`](EntityView.html) query as you set them in the [`Filter`](Filter.html) section of the view.

Internally the chart query uses `ElasticSearch` aggregation features, available in v1.0 and above only, as such it's perfectly acceptable to write your own query for the chart.

##Aggregate
There 3 types of aggregates supported in [`EntityChart`](EntityChart.html) so far. They are the followings

* [`Terms`](http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-aggregations-bucket-terms-aggregation.html)- This will aggregate using the [`TermQuery`](TermQuery.html). This is only good for string field which you have not set to be full text analyzed. 

* [`Histogram`](http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-aggregations-bucket-histogram-aggregation.html) - This is good for numerical values where you can define an interval for your steps.

* [`DateHistogram`](http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-aggregations-bucket-datehistogram-aggregation.html) - Allow you to aggregate date field according to the interval you specify in the `DateInterval` section

## Field
This is option for the field available in your [`EntityDefinition`](EntityDefinition.html)

## Type
The chart type, currently these are the only options

* Pie
* Line
* Bar
* Column

## Saving your chart
SPH also allows you to save you chart, this enable you to quickly render your predefined charts by just selected it from the options

![alt](http://i.imgur.com/gSWKWp6.png)
Create your chart as usual and click on the save button to save your chart

## Editing existing chart
Once a predefined chart is selected, there are options to edit them or removing the from the system
![alt](http://i.imgur.com/X76kWUW.png)
