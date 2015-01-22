#ScheduledTriggerActivity
##Overview
*<i class="fa fa-warning fa-2x" style="color:#ff6a00"></i> MUST BE SET as STARTUP ACTIVITY*
Allows you to create a scheduled workflow. Scheduld task will be created for each enabled [`IntervalSchedule`](IntervalSchedule.html).
You can choose from 2 different schedules
* [`DailySchedule`](DailySchedule.html)
* [`WeeklySchedule`](WeeklySchedule.html)
* [`MonthlySchedule`](MonthlySchedule.html)


##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr><td>IntervalScheduleCollection</td><td> - The list of schedules for this activity to run</td></tr>
</tbody></table>



## See also

[Activity](Activity.html)
[DailySchedule](DailySchedule.html)
[WeeklySchedule](WeeklySchedule.html)
[MonthlySchedule](MonthlySchedule.html)